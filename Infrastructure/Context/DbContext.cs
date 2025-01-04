using Helper.NLog;
using Microsoft.EntityFrameworkCore;
using NLog;
using Shared;
using System.Data;

namespace Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region DbSet
        public DbSet<User> Users { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
            }

        }
    }
}
