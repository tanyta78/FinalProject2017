namespace AuctionHub.Services.Contracts
{
    using Data.Models;
    using Services.Models.Products;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task CreateAsync(string name, string description, List<Picture> pictures, string ownerId);

        Task EditAsync(int id, string name, string description);

        Task DeleteAsync(int id);

        Task<IEnumerable<ProductListingServiceModel>> ListAsync(string ownerId);

        Task<ProductDetailsServiceModel> GetProductByIdAsync(int id);

        Task<ProductDetailsServiceModel> GetProductByNameAsync(string name);

        Task<ProductDetailsServiceModel> GetProductByPictureId(int pictureId);

        bool IsProductExist(int id);

        int GetProductPicturesCount(int id);

        List<Picture> GetProductPictures(int id);
    }
}
