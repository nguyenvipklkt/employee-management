using Helper.NLog;
using Infrastructure.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Object.Model;

namespace Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region DbSet
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserFunction> UserFunction { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.SeedUser();
                modelBuilder.SeedRole();
                modelBuilder.SeedUserFunction();
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                base.OnConfiguring(optionsBuilder);
                optionsBuilder.ConfigureWarnings(warnings =>
                    warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
            }
        }
    }
}
