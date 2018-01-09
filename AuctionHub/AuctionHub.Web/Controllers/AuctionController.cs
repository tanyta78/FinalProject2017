namespace AuctionHub.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.AuctionViewModels;
    using Services.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    public class AuctionController : BaseController
    {
        private readonly IAuctionService auctionService;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly UserManager<User> userManager;
        
        public AuctionController( IAuctionService auctionService, UserManager<User> userManager,IProductService productService, ICategoryService categoryService)
        {
            this.auctionService = auctionService;
            this.userManager = userManager;
            this.productService = productService;
            this.categoryService = categoryService;
        }

        //GET Auction Index
        [HttpGet]
        public IActionResult Index()
        {
            var auctions = this.auctionService.IndexAuctionsList()
                               .Where(x => x.IsActive == true)
                               .Select(a => new IndexAuctionViewModel()
                               {
                                   PicturePath = a.Product.Pictures.FirstOrDefault()?.Path,
                                   Description = a.Description,
                                   EndDate = a.EndDate,
                                   LastBiddedPrice = a.Bids != null ? a.Bids.Count > 0 ? a.Bids.Last().Value : 0 : 0,
                                   OwnerName = a.Product?.Owner?.Name,
                                   ProductName = a.Product?.Name,
                                   Id = a.Id,
                                   IsActive = a.IsActive
                               }).Where(pr => pr.IsActive == true);

            return this.View(auctions);
        }

        //GET Auction/Create
        [HttpGet]
        [Authorize]
        public IActionResult Create(int id)
        {
            var loggedUserId = this.userManager.GetUserId(User);

            var newAuction = new AuctionFormModel()
            {
                ProductId = id
            };

            return View(newAuction);
        }
            

        //POST Auction/Create
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AuctionFormModel auctionToCreate)
        {
            if (!ModelState.IsValid)
            {
                return View(auctionToCreate);
            }
            //Create(string description, decimal price, DateTime startDate, DateTime endDate, int categoryId, int productId)
            User loggedUser = await this.userManager.FindByNameAsync(this.User.Identity.Name);
          
            
            if (!this.categoryService.IsCategoryExist(auctionToCreate.CategoryId))
            {
                return this.BadRequest();
            }

            var categoryForAuction = this.categoryService.GetCategoryById(auctionToCreate.CategoryId);

            if (!this.productService.IsProductExist(auctionToCreate.ProductId))
            {
                RedirectToAction(nameof(ProductController.List), "Product");
            }

            var productForAuction = await this.productService.GetProductByIdAsync(auctionToCreate.ProductId);

            if (productForAuction.OwnerId != loggedUser.Id)
            {
                return Forbid();
            }

            if (!IsValid(auctionToCreate))
            {
                return this.BadRequest();
            }

             await this.auctionService.Create(
                auctionToCreate.Description,
                auctionToCreate.Price,
                auctionToCreate.StartDate,
                auctionToCreate.EndDate,
                auctionToCreate.CategoryId,
                auctionToCreate.ProductId);

            return RedirectToAction(nameof(AuctionController.Index), "Auction");
        }

        //GET: Auction/List
        [HttpGet]
        public async Task<IActionResult> List(int page = 1, string search = null, string user = null)
        {
            var pageSize = DataConstants.AuctionToShow;

            var ownerId = this.userManager.GetUserId(User);

            var allAuctions = await this.auctionService.ListAsync(ownerId,page,search);

            var result = allAuctions
                .OrderByDescending(a => a.EndDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                //.Include()
                .Select(a => new IndexAuctionViewModel()
                {
                    Description = a.Description,
                    EndDate = a.EndDate,
                    LastBiddedPrice = a.Bids != null ? a.Bids.Count > 0 ? a.Bids.Last().Value : 0 : 0,
                    OwnerName = a.Product.Owner.Name,
                    Id = a.Id,
                    PicturePath = a.Product.Pictures.FirstOrDefault().Path,
                    ProductName = a.Product.Name
                })
                .ToList();

            // how to make viewbag.currentpage = page;

            return View(result);
        }

        //GET: Auction/Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var currentAuction = await this.auctionService.GetAuctionByIdAsync(id);

            if (currentAuction == null)
            {
                return NotFound();
            }

            var model = new AuctionFormModel
            {
               Description = currentAuction.Description,
               CategoryId = currentAuction.CategoryId,
               EndDate = currentAuction.EndDate,
               Price = currentAuction.Price,
               ProductId = currentAuction.ProductId,
               StartDate = currentAuction.StartDate
            };

            return View(model);
        }

        //GET: Auction/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var currentAuction = await this.auctionService.GetAuctionByIdAsync(id);

            if (currentAuction == null)
            {
                return NotFound();
            }

           return this.View(currentAuction);
        }



        //POST : Auction/Delete
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentAuction = await this.auctionService.GetAuctionByIdAsync(id);

            if (currentAuction == null)
            {
                return NotFound();
            }

            this.auctionService.Delete(id);

            return RedirectToAction("List");
        }


        //GET: Auction/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var currentAuction = await this.auctionService.GetAuctionByIdAsync(id);

            if (currentAuction == null)
            {
                return NotFound();
            }

            return this.View(currentAuction);
        }

        //POST: Auction/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(AuctionEditViewModel auctionToEdit)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(auctionToEdit);
            //}

            User loggedUser = await this.userManager.FindByNameAsync(this.User.Identity.Name);

            if (!this.categoryService.IsCategoryExist(auctionToEdit.CategoryId))
            {
                return this.BadRequest();
            }

            var categoryForAuction = this.categoryService.GetCategoryById(auctionToEdit.CategoryId);

            if (!this.productService.IsProductExist(auctionToEdit.ProductId))
            {
                RedirectToAction(nameof(ProductController.List), "Product");
            }

            var productForAuction = await this.productService.GetProductByIdAsync(auctionToEdit.ProductId);

            if (productForAuction.OwnerId != loggedUser.Id)
            {
                return Forbid();
            }

            if (!IsValid(auctionToEdit))
            {
                return this.BadRequest();
            }

            await this.auctionService.Edit(
                auctionToEdit.Id,
                auctionToEdit.EndDate
                );

            return RedirectToAction("Details");


        }





















        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }

    }
}
