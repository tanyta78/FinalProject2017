namespace AuctionHub.Web.Controllers
{
    using AuctionHub.Data;
    using AuctionHub.Data.Models;
    using AuctionHub.Services.Contracts;
    using AuctionHub.Services.Implementations;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BidController : BaseController
    {
        private readonly IAuctionService auctionService;
        private readonly IBidService bidService;
        private readonly UserManager<User> userManager;
        private readonly IProductService productService;

        public BidController(IBidService bidService, IAuctionService auctionService, IProductService productService, AuctionHubDbContext db, UserManager<User> userManager)
        {
            this.auctionService = auctionService;
            this.bidService = bidService;
            this.userManager = userManager;
            this.productService = productService;
        }

        public async Task<IActionResult> Create(int auctionId, decimal value)
        {
            IEnumerable<Bid> allByAuction = this.bidService.GetForAuction(auctionId);
            var currentAuction = await this.auctionService.GetAuctionByIdAsync(auctionId);

            decimal startingPrice = currentAuction.Price;
            decimal maxBid = startingPrice;

            if (allByAuction.Count() > 0)
            {
                maxBid = allByAuction.Select(b => b.Value).Max();
            }

            

            if (maxBid >= value || value < currentAuction.Price)
            {
                return BadRequest($"Bid value cannot be less than or equal to {maxBid}");
            }

            var userId = this.userManager.GetUserId(User);
            var auction = await auctionService.GetAuctionByIdAsync(auctionId);
            

            if (userId == auction.OwnerId)
            {
                return BadRequest($"You cannot bid on this auction!");
            }

            var bidTime = DateTime.UtcNow;

            await this.bidService
                .CreateAsync(bidTime, value, userId, auctionId);
            return Ok();
            //return RedirectToAction(string.Concat(nameof(AuctionController.Details), "/", "Auction"));
        }
    }
}
