namespace AuctionHub.Services.Contracts
{
    using Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface IPictureService
    {
        void AddPicture(FormCollection formCollection, int productId, Picture picture);

        void DeletePicture(int? id);

        void DeleteAllPicturesByProductId(int productId);
    }
}