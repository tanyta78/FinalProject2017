using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AuctionHub.Web.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task Bid(string bidderName, string bid)
        {
            await this.Clients.All.InvokeAsync("Bid", bidderName, bid);
        }

        public async Task Comment(string author, string comment, string publishDate, int commentId)
        {
            await this.Clients.All.InvokeAsync("Comment", author, comment, publishDate, commentId);
        }

        public async Task DeleteComment(string commentId)
        {
            await this.Clients.All.InvokeAsync("DeleteComment", commentId);
        }
        public async Task EditComment(string commentId, string newContent)
        {
            await this.Clients.All.InvokeAsync("EditComment", commentId, newContent);
        }
    }
}
