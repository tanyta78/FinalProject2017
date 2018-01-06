namespace AuctionHub.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Contracts;
    using Services.Models.Products;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PictureService : IPictureService
    {
        private readonly AuctionHubDbContext db;

        public PictureService(AuctionHubDbContext db)
        {
            this.db = db;
        }

        public Picture GetPictureById(int? id) => this.db.Pictures.Find(id);

        public void AddPicture(string fullPath, int productId, string authorId)
        {
            var picture = new Picture
            {
                Path = fullPath,
                ProductId = productId,
                AuthorId = authorId
            };

            this.db.Pictures.Add(picture);
            this.db.SaveChanges();
        }

        public void DeletePicture(int? id)
        {
            var pictureToBeRemoved = this.db
                .Pictures
                .FirstOrDefault(p => p.Id == id);

            this.db
                .Pictures
                .Remove(pictureToBeRemoved);

            this.db.SaveChanges();
        }
        
        public void DeleteAllPicturesByProductId(int productId)
        {
            var picturesToBeDeleted = GetPicturesByProductId(productId);

            this.db
                .Pictures
                .RemoveRange(picturesToBeDeleted);

            this.db.SaveChanges();
        }

        public List<Picture> GetPicturesByProductId(int id)
        {
            var allPictures = this.db
                .Pictures
                .Where(p => p.ProductId == id)
                .ToList();

            return allPictures;
        }
    }
}
