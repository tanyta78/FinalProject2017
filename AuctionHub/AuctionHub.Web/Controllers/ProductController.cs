namespace AuctionHub.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AuctionHub.Data;
    using AuctionHub.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ProductController : Controller
    {
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
                using (var db = new AuctionHubDbContext())
                {
                    var ownerId = db.Users
                        .First(u => u.UserName == this.User.Identity.Name)
                        .Id;

                    productToCreate.OwnerId = ownerId;

                    db.Products.Add(productToCreate);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(productToCreate);
        }


        // GET: /Product/List
        [HttpGet]
        public IActionResult List()
        {
            using (var db = new AuctionHubDbContext())
            {
                var allProducts = db
                    .Products
                    .Include(p => p.Owner)
                    .OrderByDescending(p => p.Id)
                    .ToList();

                return View(allProducts);
            }
        }
    }
}
