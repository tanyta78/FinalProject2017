using AuctionHub.Data;
using AuctionHub.Data.Models;
using AuctionHub.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuctionHub.Services.Implementations
{
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
