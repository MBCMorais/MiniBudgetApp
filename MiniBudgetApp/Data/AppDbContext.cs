using Microsoft.EntityFrameworkCore;
using MiniBudgetApp.Models;

namespace MiniBudgetApp.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
