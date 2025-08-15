using Microsoft.EntityFrameworkCore;
using MoneyTracker.Models;

namespace MoneyTracker.Data
{
    public class MoneyTrackerDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }

        public MoneyTrackerDbContext(DbContextOptions<MoneyTrackerDbContext> options)
            : base(options) 
        {
        
        }
    }
}
