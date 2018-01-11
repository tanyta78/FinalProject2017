namespace AuctionHub.Services.Implementations
{
    using AuctionHub.Services.Models.Comments;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Models.Auctions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuctionService : IAuctionService
    {
        private readonly AuctionHubDbContext db;

        public AuctionService(AuctionHubDbContext db)
        {
            this.db = db;
        }

        public async Task<AuctionDetailsServiceModel> GetAuctionByIdAsync(int id)
            => await this.db
                .Auctions
                .Where(a => a.Id == id)
                .Select(a => new AuctionDetailsServiceModel
                {
                    Id = a.Id,
                    Description = a.Description,
                    OwnerId = a.Product.OwnerId,
                    Price = a.Price,
                    LastBidder = a.LastBidder.Id,
                    CategoryName = a.Category.Name,
                    ProductName = a.Product.Name,
                    Pictures = a.Product.Pictures,
                    Comments = a.Comments
                        .OrderByDescending(c => c.PublishDate)
                        .Select(c => new CommentServiceModel
                        {
                            Author = c.Author.Name,
                            Content = c.Content,
                            PublishDate = c.PublishDate
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            //var result = this.db
            //     .Auctions
            //     .FirstOrDefault(a => a.Id == id);
            //
            //var product = this.db
            //    .Products
            //    .Include(x => x.Pictures)
            //    .FirstOrDefault(x => x.Id == result.ProductId);
            //
            //var serviceModel = new AuctionDetailsServiceModel
            //{
            //    CategoryName = result.Category?.Name,
            //    Description = result.Description,
            //    LastBidder = result.LastBidder?.UserName,
            //    Price = result.Price,
            //    ProductName = result.Product?.Name,
            //    Id = result.Id,
            //    Pictures = product.Pictures
            //};
            //var result =  await this.db
            //     .Auctions
            //     .Where(a => a.Id == id)
            //     .ProjectTo<AuctionDetailsServiceModel>()
            //     .FirstOrDefaultAsync();

        public async Task<IEnumerable<AuctionDetailsServiceModel>> GetByCategoryNameAsync(string categoryName)
             => await this.db
                .Auctions
                .Where(a => a.Category.Name == categoryName)
                .ProjectTo<AuctionDetailsServiceModel>()
                .ToListAsync();

        public async Task<IQueryable<Auction>> ListAsync(string ownerId, int page, string search)
        {
          
            var auctionsQuery = this.db.Auctions.AsQueryable();
            if (ownerId!=null)
            {
                auctionsQuery = auctionsQuery.Where(a => a.Product.OwnerId == ownerId);
            }

            if (search!=null)
            {
                // we can add different search options
                auctionsQuery = auctionsQuery.Where(a =>
                    a.Product.Name.ToLower().Contains(search.ToLower()) ||
                    a.Product.Description.ToLower().Contains(search.ToLower())
                );
            }

            return auctionsQuery;

        }


        public async Task Create(string description, decimal price, DateTime startDate, DateTime endDate, int categoryId, int productId)
        {
           var auction = new Auction()
           {
               Description = description,
               Price = price,
               StartDate = startDate,
               EndDate = endDate,
               CategoryId = categoryId,
               ProductId = productId,
               IsActive = true
           };

            this.db.Auctions.Add(auction);

            await this.db.SaveChangesAsync();

            //make connection with category and product
            var category = this.db.Categories.FindAsync(categoryId).Result;
            var product = this.db.Products.FindAsync(productId).Result;
            category.Auctions.Add(auction);
            product.Auction = auction;
            await this.db.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var auction = await this.db.Auctions.FindAsync(id);

            
            if (auction == null)
            {
                return;
            }

            // Here, before we delete the product, its pictures in the file system should be deleted as well!

            this.db.Auctions.Remove(auction);
            await this.db.SaveChangesAsync();

           // delete connection with product
           // var auctionProductId = auction.ProductId;
           // this.db.Products.FindAsync(auctionProductId).Result.AuctionId = null;

           // delete connection with category
           // var auctionCategoryId = auction.CategoryId;
           // this.db.Categories.FindAsync(auctionCategoryId).Result.Auctions.Remove(auction);


           // delete connection with bidding??? NOT FINNISHED

           //var bidsList = this.db.Bids.Where(b => b.AuctionId == id);

           // this.db.SaveChanges();

           // auction.IsActive = false;
           // this.db.Auctions.Remove(auction);

           // await this.db.SaveChangesAsync();
        }

        public async Task Edit(int id, string productName, string description, string categoryName)
        {
            var auction = await this.GetAuctionByIdAsync(id);

            //if (endDate < DateTime.UtcNow)
            //{
            //    return;
            //}
            //auction.EndDate = endDate;
            auction.Id = id;
            
            //auction.CategoryName = categoryName;
            auction.ProductName = productName;
            auction.Description = description;

            await this.db.SaveChangesAsync();
        }

        public IEnumerable<Auction> IndexAuctionsList()
        {
            var auctionsToView = this.db.Auctions
                                        .Where(st => st.IsActive == true)
                                        .Include(a => a.Product)
                                        //.Take(DataConstants.AuctionToShow)
                                        .ToList();
            return auctionsToView;
        }

        public bool IsAuctionExist(int id)
        {
            return this.db.Auctions.Any(a => a.ProductId == id);
        }
    }
}
