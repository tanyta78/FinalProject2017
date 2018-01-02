namespace AuctionHub.Web.Controllers
{
    using Data;

    public class AuctionController : BaseController
    {
        private readonly AuctionHubDbContext db;
        public AuctionController(AuctionHubDbContext db)
        {
            this.db = db;
        }


    }
}
