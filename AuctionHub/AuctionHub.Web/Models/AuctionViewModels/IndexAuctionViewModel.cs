namespace AuctionHub.Web.Models.AuctionViewModels
{
    using System;

    public class IndexAuctionViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime EndDate { get; set; }

        public decimal LastBiddedPrice { get; set; }

        public string OwnerName { get; set; }

        public string ProductName { get; set; }

        public string PicturePath { get; set; }

        public bool IsActive { get; set; }

        public int? BuyerId { get; set; }
    }
}
