using Microsoft.EntityFrameworkCore;
using Flowers.Domain.Entities;

namespace Wlowers.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<FN> FN { get; set; }
        public DbSet<Category> Categories { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public object CreateDbContext()
        {
            throw new NotImplementedException();
        }
    }
}
