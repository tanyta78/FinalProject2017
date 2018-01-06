namespace AuctionHub.Services.Contracts
{
    using System.Collections.Generic;
    using Data.Models;

    public interface ICategoryService
    {
        bool IsCategoryExist(int id);

        Category GetCategoryById(int id);

        void Create(string name);

        void Delete(int id);

        void Edit(int id);

        IEnumerable<Category> CategoryList();
    }
}
