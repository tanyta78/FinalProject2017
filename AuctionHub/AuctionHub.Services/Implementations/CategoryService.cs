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

        public void Create(string name)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
    }
}
