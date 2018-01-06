namespace AuctionHub.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;

    public class CategoryService : ICategoryService
    {
        private AuctionHubDbContext db;

        public CategoryService(AuctionHubDbContext db)
        {
            this.db = db;
        }

        public bool IsCategoryExist(int id)
        { 
            return this.db.Categories.Any(c => c.Id == id);
        }

        public Category GetCategoryById(int id)
        {
            var categoryById = this.db.Categories.FirstOrDefault(c => c.Id == id);
            return categoryById;
        }

        public Category GetCategoryByName(string name)
        {
            var categoryByName = this.db.Categories.FirstOrDefault(c => c.Name == name);
            return categoryByName;
        }

        public void Create(string name)
        {
            var category = new Category()
            {
                Name = name,
                Auctions = new List<Auction>()
            };
            this.db.Categories.Add(category);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Edit(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Category> CategoryList()
        {
            return this.db.Categories.ToList();
        }
    }
}
