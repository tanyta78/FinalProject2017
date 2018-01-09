using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionHub.Web.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task Bid(string bidderName, string bid)
        {
            await this.Clients.All.InvokeAsync("Bid", bidderName, bid);
        }
    }
}
