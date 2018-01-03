namespace AuctionHub.Web.Controllers
{
    using AuctionHub.Common;
    using AuctionHub.Services.Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Models;

    public class ProductController : BaseController
    {
        private readonly IProductService productService;
        private readonly AuctionHubDbContext db;
        private readonly UserManager<User> userManager;
        public ProductController(AuctionHubDbContext db, UserManager<User> userManager, IProductService productService)
        {
            this.productService = productService;
            this.db = db;
            this.userManager = userManager;
        }
        
        [HttpGet]
        [Route("Product/Details/{id}")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var currentProduct = productService.GetProductById(id);

            if (currentProduct == null)
            {
                return NotFound();
            }

            return View(currentProduct);
        }
        
        [HttpGet]
        [Authorize]
        [Route("Product/Create")]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("Product/Create")]
        public IActionResult Create(Product productToCreate)
        {
            if (ModelState.IsValid)
            {
                productService.Create(productToCreate, this.User.Identity.Name);

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(productToCreate);
        }
        
        [HttpGet]
        public IActionResult List()
        {
            var allProducts = productService.List();

            return View(allProducts);
        }

        // GET: /Product/Edit/{id}
        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

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
            if (ModelState.IsValid)
            {

                var productToEdit = this.db
                    .Products
                    .First(p => p.Id == model.Id);

                productToEdit.Name = model.Name;
                productToEdit.Description = model.Description;

                this.db.Entry(productToEdit).State = EntityState.Modified;
                this.db.SaveChanges();

                return RedirectToAction("Details/" + productToEdit.Id, "Product");

            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // GET: /Product/Delete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

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

            return View(productToBeDeleted);
        }

        // POST: /Product/Delete/{id}
        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var productToBeDeleted = this.db
                .Products
                .FirstOrDefault(p => p.Id == id);

            if (productToBeDeleted == null)
            {
                return NotFound();
            }

            // Here, before we delete the product, its pictures in the file system should be deleted as well!
            // DeleteProductPictures(productToBeDeleted);

            this.db.Products.Remove(productToBeDeleted);
            this.db.SaveChanges();
            this.ShowNotification(NotificationType.Success, Messages.ProductDeleted);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private bool IsUserAuthorizedToEdit(Product productToEdit, string loggedUserId)
        {
            bool isAdmin = this.User.IsInRole("Administrator");
            bool isAuthor = productToEdit.OwnerId == loggedUserId;

            return isAdmin || isAuthor;
        }
    }
}
