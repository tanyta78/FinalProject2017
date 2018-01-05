namespace AuctionHub.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Data.Models;

    public class TownService : ITownService
    {
        private readonly AuctionHubDbContext db;

        public TownService(AuctionHubDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Town> All() => this.db.Towns.ToList();

        public Town GetById(int id) => this.db.Towns.FirstOrDefault(t => t.Id == id);

        public Town GetByName(string name) => this.db.Towns.FirstOrDefault(t => t.Name == name);
    }
}
