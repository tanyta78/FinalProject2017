namespace AuctionHub.Web.Controllers
{
    using System.Linq;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.AuctionViewModels;

    public class AuctionController : BaseController
    {
        private readonly AuctionHubDbContext db;
        public AuctionController(AuctionHubDbContext db)
        {
            this.db = db;
        }

        //GET Auction Index
        public ActionResult Index()
        {
            var auctions = this.db.Auctions
                               .Include(a => a.Product)
                               .Take(5)
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

    }
}
