namespace AuctionHub.Services.Models.Auctions
{
    using Common.Mapping;
    using Data.Models;
    using Services.Models.Comments;
    using System.Collections.Generic;

    public class AuctionDetailsServiceModel : IMapFrom<Auction>/*, IHaveCustomMapping*/
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string OwnerId { get; set; }

        //public User Owner { get; set; }

        public decimal Price { get; set; }

        public string LastBidder { get; set; }

        public string CategoryName { get; set; }

        public string ProductName { get; set; }

        public int ProductId { get; set; }

        public List<Picture> Pictures { get; set; } = new List<Picture>();

        public List<CommentServiceModel> Comments { get; set; } = new List<CommentServiceModel>();

        //public void ConfigureMapping(Profile mapper)
        //{
        //    //mapper.CreateMap<Auction, AuctionDetailsServiceModel>().ForMember(a => a.LastBidder, cfg => cfg.MapFrom(a => a.LastBidder.UserName));
        //    mapper.CreateMap<Auction, AuctionDetailsServiceModel>().ForMember(a => a.CategoryName, cfg => cfg.MapFrom(a => a.Category.Name));
        //    mapper.CreateMap<Auction, AuctionDetailsServiceModel>().ForMember(a => a.ProductName, cfg => cfg.MapFrom(a => a.Product.Name));
        //}
    }
}
