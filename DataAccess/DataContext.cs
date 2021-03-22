using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RemailCore.Models;

namespace RemailCore.DataAccess
{
    class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<List<Account>> Accounts { get; set; }
        public DbSet<List<Email>> Emails { get; set; }
    }
}
