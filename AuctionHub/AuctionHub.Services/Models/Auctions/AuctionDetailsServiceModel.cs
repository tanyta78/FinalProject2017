namespace AuctionHub.Services.Models.Auctions
{
    using AuctionHub.Common.Mapping;
    using AutoMapper;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AuctionDetailsServiceModel : IMapFrom<Auction>/*, IHaveCustomMapping*/
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string LastBidder { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }

        //public void ConfigureMapping(Profile mapper)
        //{
        //    //mapper.CreateMap<Auction, AuctionDetailsServiceModel>().ForMember(a => a.LastBidder, cfg => cfg.MapFrom(a => a.LastBidder.UserName));
        //    mapper.CreateMap<Auction, AuctionDetailsServiceModel>().ForMember(a => a.CategoryName, cfg => cfg.MapFrom(a => a.Category.Name));
        //    mapper.CreateMap<Auction, AuctionDetailsServiceModel>().ForMember(a => a.ProductName, cfg => cfg.MapFrom(a => a.Product.Name));
        //}
    }
}
