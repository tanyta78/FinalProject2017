using System;
using AuctionHub.Data;
using AuctionHub.Data.Models;
using AuctionHub.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHub.Web.Controllers
{
    public class BidController : BaseController
    {
        private readonly IBidService bidService;
        private readonly AuctionHubDbContext db;
        private readonly UserManager<User> userManager;

        public BidController(IBidService bidService, AuctionHubDbContext db, UserManager<User> userManager)
        {
            this.bidService = bidService;
            this.db = db;
            this.userManager = userManager;
        }

        public IActionResult Bidding(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var currentBid = bidService.GetBidById(id);

            if (currentBid == null)
            {
                return NotFound();
            }

            return View(currentBid);
        }
    }
}
