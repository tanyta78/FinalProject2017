namespace AuctionHub.Services.Contracts
{
    using Data.Models;
    using System.Collections.Generic;

    public interface IPictureService
    {
        Picture GetPictureById(int? id);

        void AddPicture(string fullPath, int productId, string authorId);

        void DeletePicture(int? id);

        void DeleteAllPicturesByProductId(int productId);

        List<Picture> GetPicturesByProductId(int id);

        Product GetProductByPictureId(int? pictureId);
    }
}