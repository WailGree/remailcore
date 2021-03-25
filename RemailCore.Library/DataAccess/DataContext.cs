using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RemailCore.Models;

namespace RemailCore.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Email> Emails { get; set; }
    }
}
