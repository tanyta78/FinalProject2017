namespace AuctionHub.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.AuctionViewModels;
    using Services.Contracts;

    public class AuctionController : BaseController
    {
        private readonly IAuctionService auctionService;
        private readonly IProductService productService;
        private readonly UserManager<User> userManager;

        public AuctionController()
        {
            
        }

        public AuctionController( IAuctionService auctionService, UserManager<User> userManager,IProductService productService)
        {
            this.auctionService = auctionService;
            this.userManager = userManager;
            this.productService = productService;
        }

        //GET Auction Index
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var auctions = this.auctionService.IndexAuctionsList()
                               .Select(a => new IndexAuctionViewModel()
                               {
                                   PicturePath = a.Product.Pictures.First().Path,
                                   Description = a.Description,
                                   EndDate = a.EndDate,
                                   LastBiddedPrice = a.Bids.Last().Value,
                                   OwnerName = a.Product.Owner.Name,
                                   ProductName = a.Product.Name,
                                   Id = a.Id
                               })
                               .ToList();

            return this.View(auctions);
        }

        //GET Auction/Create
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
            => View();

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
            User loggedUser = await this.userManager.FindByEmailAsync(this.User.Identity.Name);
          
           // var categoryForAuction = this.categoryService.GetCategoryById(auctionToCreate.CategoryId);
            

            if (!this.productService.IsProductExist(auctionToCreate.ProductId))
            {
                RedirectToAction(nameof(ProductController.List), "Product");
            }

            var productForAuction = this.productService.GetProductByIdAsync(auctionToCreate.ProductId);

            //if (productForAuction.OwnerId != loggedUser.Id)
            //{
            //    return Forbid();
            //}


            this.auctionService.Create(
                auctionToCreate.Description,
                auctionToCreate.Price,
                auctionToCreate.StartDate,
                auctionToCreate.EndDate,
                auctionToCreate.CategoryId,
                auctionToCreate.ProductId);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        
    }
}
