using AuctionHub.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionHub.Services.Contracts
{
    public interface IProductService
    {
        Product Details(int? id);

        void Create(Product product, string userName);

        IEnumerable<Product> List();
    }
}
