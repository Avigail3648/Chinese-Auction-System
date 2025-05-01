using ChineseAuctionApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChineseAuctionApi.Repositories
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
       : base(options)
        {

        }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<GiftWhitNames> GiftWhitNames { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
