namespace AuctionHub.Web.Controllers
{
    using AuctionHub.Common;
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
        private readonly AuctionHubDbContext db;
        private readonly UserManager<User> userManager;
        public ProductController(AuctionHubDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        // GET: /Product/Details/{id}
        [HttpGet]
        public IActionResult Details(int? productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }

            var currentProduct = this.db
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product productToCreate)
        {
            if (ModelState.IsValid)
            {
                var ownerId = this.db.Users
                    .First(u => u.UserName == this.User.Identity.Name)
                    .Id;

                productToCreate.OwnerId = ownerId;

                this.db.Products.Add(productToCreate);
                this.db.SaveChanges();

                return RedirectToAction(nameof(HomeController.Index));

            }

            return View(productToCreate);
        }


        // GET: /Product/List
        [HttpGet]
        public IActionResult List()
        {

            var allProducts = this.db
                .Products
                .Include(p => p.Owner)
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(allProducts);

        }

        // GET: /Product/Edit/{id}
        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id)
        {

            var loggedUserId = this.db.Users.First(u => u.Name == this.User.Identity.Name).Id;

            var productToEdit = this.db
                .Products
                .FirstOrDefault(p => p.Id == id);

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

                var productToEdit = this.db
                    .Products
                    .First(p => p.Id == model.Id);

                productToEdit.Name = model.Name;
                productToEdit.Description = model.Description;

                this.db.Entry(productToEdit).State = EntityState.Modified;
                this.db.SaveChanges();

                return RedirectToAction("Details/" + productToEdit.Id, "Product");

            }

            return RedirectToAction(nameof(HomeController.Index));
        }

        // GET: /Product/Delete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            User loggedUser = await this.userManager.FindByEmailAsync(User.Identity.Name);

            var productToBeDeleted = this.db
                .Products
                .FirstOrDefault(p => p.Id == id);

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
            return RedirectToAction(nameof(HomeController.Index));
        }

        private bool IsUserAuthorizedToEdit(Product productToEdit, string loggedUserId)
        {
            bool isAdmin = this.User.IsInRole("Administrator");
            bool isAuthor = productToEdit.OwnerId == loggedUserId;

            return isAdmin || isAuthor;
        }
    }
}
