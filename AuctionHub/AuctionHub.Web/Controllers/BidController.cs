namespace AuctionHub.Web.Controllers
{
    using AuctionHub.Data;
    using AuctionHub.Data.Models;
    using AuctionHub.Services.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BidController : BaseController
    {
        private readonly IBidService bidService;
        private readonly UserManager<User> userManager;

        public BidController(IBidService bidService, AuctionHubDbContext db, UserManager<User> userManager)
        {
            this.bidService = bidService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create(int auctionId, decimal value)
        {
            IEnumerable<Bid> allByAuction = this.bidService.GetForAuction(auctionId);
            decimal maxBid = 0;
            if (allByAuction.Count() > 0)
            {
                maxBid = allByAuction.Select(b => b.Value).Max();
            }

            if (maxBid >= value)
            {
                return BadRequest($"Bid value cannot be less than or equal to {maxBid}");
            }

            var userId = this.userManager.GetUserId(User);

            var bidTime = DateTime.UtcNow;

            await this.bidService
                .CreateAsync(bidTime, value, userId, auctionId);
            return Ok();
            //return RedirectToAction(string.Concat(nameof(AuctionController.Details), "/", "Auction"));
        }
    }
}
