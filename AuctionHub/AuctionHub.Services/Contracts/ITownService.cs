namespace AuctionHub.Services.Contracts
{
    using Data.Models;
    using System.Collections.Generic;

    public interface ITownService
    {
        Town GetByName(string name);

        Town GetById(int id);

        IEnumerable<Town> All();
    }
}
