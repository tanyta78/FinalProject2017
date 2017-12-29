using AuctionHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionHub.Services.Contracts
{
    public interface ITownService
    {
        Town GetByName(string name);
        Town GetById(int id);
        IEnumerable<Town> All();
    }
}
