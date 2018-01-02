namespace AuctionHub.Web.Controllers
{
    using AuctionHub.Services.Contracts;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using Web.Models;

    public class ProductController : BaseController
    {
        private readonly IProductService productService;
        private readonly AuctionHubDbContext db;

        public ProductController(AuctionHubDbContext db, IProductService productService)
        {
            this.productService = productService;
            this.db = db;
        }
        
        [HttpGet]
        [Route("Product/Details/{id}")]
        public IActionResult Details(int? productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }

            var currentProduct = productService.Details(productId);
            
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
        [Route("Product/Create")]
        public IActionResult Create(Product productToCreate)
        {
            if (ModelState.IsValid)
            {
                productService.Create(productToCreate, this.User.Identity.Name);

                return RedirectToAction("Index", "Home");
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
        public IActionResult Edit(int? productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }
            
            var loggedUserId = db.Users.First(u => u.Name == this.User.Identity.Name).Id;

            var productToEdit = db
                .Products
                .FirstOrDefault(p => p.Id == productId);

            if (!IsUserAuthorizedToEdit(productToEdit, loggedUserId))
            {
                return Forbid();
            }

            if (productToEdit == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel(productToEdit.Id, productToEdit.Name, productToEdit.Description);

            return View(model);
        }

        // POST: /Product/Edit
        [HttpPost]
        [Authorize]
        public IActionResult Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {

                var productToEdit = db
                    .Products
                    .First(p => p.Id == model.Id);

                productToEdit.Name = model.Name;
                productToEdit.Description = model.Description;

                db.Entry(productToEdit).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details/" + productToEdit.Id, "Product");

            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Product/Delete/{id}
        [HttpGet]
        [Authorize]
        public IActionResult Delete(int? productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }


            var loggedUserId = db.Users.First(u => u.Name == this.User.Identity.Name).Id;

            var productToBeDeleted = db
                .Products
                .FirstOrDefault(p => p.Id == productId);

            if (productToBeDeleted == null)
            {
                return NotFound();
            }

            if (!IsUserAuthorizedToEdit(productToBeDeleted, loggedUserId))
            {
                return Forbid();
            }

            return View(productToBeDeleted);
        }

        // POST: /Product/Delete/{id}
        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }

            var productToBeDeleted = db
                .Products
                .FirstOrDefault(p => p.Id == productId);

            if (productToBeDeleted == null)
            {
                return NotFound();
            }

            // Here, before we delete the product, its pictures in the file system should be deleted as well!
            // DeleteProductPictures(productToBeDeleted);

            db.Products.Remove(productToBeDeleted);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private bool IsUserAuthorizedToEdit(Product productToEdit, string loggedUserId)
        {
            bool isAdmin = this.User.IsInRole("Administrator");
            bool isAuthor = productToEdit.OwnerId == loggedUserId;

            return isAdmin || isAuthor;
        }
    }
}
