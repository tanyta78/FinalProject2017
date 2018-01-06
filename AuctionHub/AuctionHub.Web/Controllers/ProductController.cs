namespace AuctionHub.Web.Controllers
{
    using Common;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Web.Models.Product;

    using static AuctionHub.Data.DataConstants;

    public class ProductController : BaseController
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IProductService productService;
        private readonly IPictureService pictureService;
        private readonly AuctionHubDbContext db;
        private readonly UserManager<User> userManager;

        public ProductController(AuctionHubDbContext db, 
            UserManager<User> userManager, 
            IProductService productService, 
            IPictureService pictureService, 
            IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.productService = productService;
            this.pictureService = pictureService;
            this.db = db;
            this.userManager = userManager;
        }

        // GET: /Product/Details/{id}
        [HttpGet]
        public IActionResult Details(int? id)
        {
            var currentProduct = productService.GetProductById(id);

            if (currentProduct == null)
            {
                return NotFound();
            }

            var model = new ProductFormModel
            {
                Id = currentProduct.Id,
                Name = currentProduct.Name,
                Description = currentProduct.Description,
                Pictures = currentProduct.Pictures
            };

            return View(model);
        }

        // GET: /Product/Create
        [HttpGet]
        [Authorize]
        public IActionResult Create()
            => View();

        // POST: /Product/Create
        [HttpPost]
        [Authorize]
        public IActionResult Create(ProductFormModel productToCreate)
        {
            if (!ModelState.IsValid)
            {
                return View(productToCreate);
            }

            this.productService.Create(
                productToCreate.Name,
                productToCreate.Description,
                productToCreate.Pictures,
                this.userManager.GetUserId(User));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // GET: /Product/List
        [HttpGet]
        public IActionResult List()
        {
            var allProducts = productService.List();

            return View(allProducts);
        }

        // GET: /Product/Edit/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var loggedUser = await this.userManager.FindByEmailAsync(User.Identity.Name);

            var productToEdit = productService.GetProductById(id);

            if (!IsUserAuthorizedToEdit(productToEdit, loggedUser.Id))
            {
                return Forbid();
            }

            if (productToEdit == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel()
            {
                Id = productToEdit.Id,
                Name = productToEdit.Name,
                Description = productToEdit.Description
            };

            return View(model);
        }

        // POST: /Product/Edit
        [HttpPost]
        [Authorize]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.productService
                        .Edit(model.Id, model.Name, model.Description);

            return RedirectToAction("Details/" + model.Id, "Product");

        }

        // GET: /Product/Delete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            User loggedUser = await this.userManager.FindByEmailAsync(User.Identity.Name);

            var productToBeDeleted = productService.GetProductById(id);

            if (productToBeDeleted == null)
            {
                return NotFound();
            }

            if (!IsUserAuthorizedToEdit(productToBeDeleted, loggedUser.Id))
            {
                return Forbid();
            }

            var model = new ProductViewModel()
            {
                Id = productToBeDeleted.Id,
                Name = productToBeDeleted.Name,
                Description = productToBeDeleted.Description
            };

            return View(model);
        }

        // POST: /Product/Delete/{id}
        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var productToBeDeleted = productService.GetProductById(id);

            if (productToBeDeleted == null)
            {
                return NotFound();
            }

            // Delete all pictures of this product from filesystem
            var allPictures = this.productService.GetProductPictures(id);

            foreach (var picture in allPictures)
            {
                var fileToBeDeleted = string.Concat(hostingEnvironment.WebRootPath, picture.Path);
                
                if (System.IO.File.Exists(fileToBeDeleted))
                {
                    System.IO.File.Delete(fileToBeDeleted);
                }
            }

            // Delete all pictures of this product from database
            this.pictureService.DeleteAllPicturesByProductId(id);

            // Delete product from database
            this.productService.Delete(id);

            this.ShowNotification(NotificationType.Success, Messages.ProductDeleted);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // GET: /Product/AddPictures/{productId}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddPictures(int? id)
        {
            var loggedUser = await this.userManager.FindByEmailAsync(User.Identity.Name);
            var productToAddPictures = this.productService.GetProductById(id);
            
            if (productToAddPictures == null)
            {
                return NotFound();
            }

            if (!IsUserAuthorizedToEdit(productToAddPictures, loggedUser.Id))
            {
                return Forbid();
            }
            
            var model = new ProductFormModel
            {
                Id = productToAddPictures.Id,
                Name = productToAddPictures.Name,
                Description = productToAddPictures.Description,
                Pictures = productToAddPictures.Pictures
            };

            return View(model);
        }

        // POST: /Product/AddPictures/{productId}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPictures(List<IFormFile> files, int id)
        {
            var author = await this.userManager.FindByEmailAsync(User.Identity.Name);
            var product = productService.GetProductById(id);

            if (!IsUserAuthorizedToEdit(product, author.Id))
            {
                return Forbid();
            }

            // Check whether pictures count >= picturesMaxCount
            int picsCount = productService.GetProductPicturesCount(id);

            if (picsCount >= PicturesPerProductMaxCount)
            {
                ViewBag.Error = string.Format("You cannot upload more than {0} images to this product!", PicturesPerProductMaxCount);

                return RedirectToAction(nameof(ProductController.AddPictures), "Product");
            }
            
            var uploadDirectory = Path.Combine(hostingEnvironment.WebRootPath, "images\\Products");

            foreach (var file in files)
            {
                // Validate file format
                string extension = Path.GetExtension(file.FileName);
                if (!IsExtensionValid(extension))
                {
                    ViewBag.Error = "Invalid file format!";

                    return RedirectToAction(string.Concat(nameof(ProductController.AddPictures), "/", product.Id), "Product");
                }
                
                var fullPath = Path.Combine(uploadDirectory, GetUniqueFileName(file.FileName));

                var uniqueFileName = fullPath
                    .Substring(fullPath.IndexOf(Path.GetFileNameWithoutExtension(file.FileName)));

                var dbPath = $"/images/Products/{uniqueFileName}";

                // Add the picture to filesystem
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Add the current picture to database
                pictureService.AddPicture(dbPath, id, author.Id);
            }

            return RedirectToAction(string.Concat(nameof(ProductController.AddPictures), "/", product.Id), "Product");
        }

        // POST: /Product/DeletePicture/{id}
        [Authorize]
        public async Task<IActionResult> DeletePicture(int? id)
        {
            var product = this.pictureService.GetProductByPictureId(id);
            var author = await this.userManager.FindByEmailAsync(User.Identity.Name);
            
            if (!IsUserAuthorizedToEdit(product, author.Id))
            {
                return Forbid();
            }

            var picturePath = pictureService
                .GetPictureById(id)
                .Path;

            var fileToBeDeleted = string.Concat(hostingEnvironment.WebRootPath, picturePath);

            // First delete the picture from file system
            if (System.IO.File.Exists(fileToBeDeleted))
            {
                System.IO.File.Delete(fileToBeDeleted);
            }

            // Then delete it from database:
            this.pictureService.DeletePicture(id);

            return RedirectToAction(string.Concat(nameof(ProductController.Details), "/", product.Id), "Product");
        }

        private bool IsExtensionValid(string extension)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                case ".jpg":
                    return true;
                case ".png":
                    return true;
                case ".jpeg":
                    return true;
                case ".bmp":
                    return true;
                default:
                    return false;
            }
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            var uniqueFileName = string.Format("{0}_{1}{2}",
                        Path.GetFileNameWithoutExtension(fileName),
                        Guid.NewGuid().ToString().Substring(0, 6),
                        Path.GetExtension(fileName));

            return uniqueFileName;
        }

        private bool IsUserAuthorizedToEdit(Product productToEdit, string loggedUserId)
        {
            bool isAdmin = this.User.IsInRole("Administrator");
            bool isAuthor = productToEdit.OwnerId == loggedUserId;

            return isAdmin || isAuthor;
        }
    }
}