namespace AuctionHub.Web.Models.AuctionViewModels
{
    using System;

    public class AuctionEditViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime EndDate { get; set; }

        public decimal LastBiddedPrice { get; set; }

        public string OwnerName { get; set; }

        public string ProductName { get; set; }

        public string PicturePath { get; set; }

        public int CategoryId { get; set; }

        public int ProductId { get; set; }
    }
}