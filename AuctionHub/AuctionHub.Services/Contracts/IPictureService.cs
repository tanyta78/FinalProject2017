﻿namespace AuctionHub.Services.Contracts
{
    using Data.Models;
    using Services.Models.Products;
    using System.Collections.Generic;

    public interface IPictureService
    {
        Picture GetPictureById(int? id);

        void AddPicture(string fullPath, int productId, string authorId);

        void DeletePicture(int? id);

        void DeleteAllPicturesByProductId(int productId);

        List<Picture> GetPicturesByProductId(int id);

        ProductDetailsServiceModel GetProductByPictureId(int pictureId);
    }
}