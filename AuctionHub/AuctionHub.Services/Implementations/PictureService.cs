namespace AuctionHub.Services.Implementations
{
    using AuctionHub.Data.Models;
    using AuctionHub.Services.Contracts;
    using Microsoft.AspNetCore.Http;
    using System;

    public class PictureService : IPictureService
    {
        public void AddPicture(FormCollection formCollection, int productId, Picture picture)
        {
            throw new NotImplementedException();
        }

        public void DeletePicture(int? id)
        {
            throw new NotImplementedException();
        }
        
        public void DeleteAllPicturesByProductId(int productId)
        {
            throw new NotImplementedException();
        }
    }
}
