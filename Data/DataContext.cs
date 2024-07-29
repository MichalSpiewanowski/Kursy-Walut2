using KursyWalut2.Entities;
using Microsoft.EntityFrameworkCore;

namespace KursyWalut2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Rate> Rates { get; set; }
    }
}
