namespace AuctionHub.Web.Controllers
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using Web.Models;

    public class ProductController : Controller
    {
        private readonly AuctionHubDbContext db;
        public ProductController(AuctionHubDbContext db)
        {
            this.db = db;
        }

        // GET: /Product/Details/{id}
        [HttpGet]
        public IActionResult Details(int? productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }

            var currentProduct = db
                .Products
                .Include(p => p.Owner)
                .FirstOrDefault(p => p.Id == productId);

            if (currentProduct == null)
            {
                return NotFound();
            }

            return View(currentProduct);

        }


        // GET: /Product/Create
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        [Authorize]
        public IActionResult Create(Product productToCreate)
        {
            if (ModelState.IsValid)
            {
                var ownerId = db.Users
                    .First(u => u.UserName == this.User.Identity.Name)
                    .Id;

                productToCreate.OwnerId = ownerId;

                db.Products.Add(productToCreate);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");

            }

            return View(productToCreate);
        }


        // GET: /Product/List
        [HttpGet]
        public IActionResult List()
        {

            var allProducts = db
                .Products
                .Include(p => p.Owner)
                .OrderByDescending(p => p.Id)
                .ToList();

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
