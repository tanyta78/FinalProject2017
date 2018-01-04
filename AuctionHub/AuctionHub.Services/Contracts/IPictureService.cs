using AuctionHub.Data.Models;
using Microsoft.AspNetCore.Http;

namespace AuctionHub.Services.Contracts
{
    public interface IPictureService
    {
        void AddPicture(FormCollection formCollection, int productId, Picture picture);

        void DeletePicture(int? id);

        void DeleteAllPicturesByProductId(int productId);
    }
}