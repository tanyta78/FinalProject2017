namespace AuctionHub.Web.Controllers
{
    using Common;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Services.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Models.Product;

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
        public IActionResult Create()
            => View();
        
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
        
        [HttpGet]
        public IActionResult List()
        {
            var allProducts = productService.List();

            return View(allProducts);
        }

        // GET: /Product/Edit/{id}
        [HttpGet]
        [Authorize]
        //[ValidateAntiForgeryToken]
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
                this.productService
                        .Edit(model.Id, model.Name, model.Description);

                return RedirectToAction("Details/" + model.Id, "Product");
            }

            return View(model);
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

            this.productService.Delete(id);

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
