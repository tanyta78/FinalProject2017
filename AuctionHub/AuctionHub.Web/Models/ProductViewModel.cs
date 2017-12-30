namespace AuctionHub.Web.Models
{
    public class ProductViewModel
    {
        public ProductViewModel(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}
