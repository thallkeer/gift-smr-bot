using GiftSmrBot.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GiftSmrBot.Core
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
    }
}
