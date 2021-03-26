using Microsoft.EntityFrameworkCore;
using RemailCore.Library.Models;

namespace RemailCore.Library.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) {}

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Email> Emails { get; set; }
    }
}