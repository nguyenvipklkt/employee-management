using Helper.NLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
			try
			{
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer("data source=DESKTOP-LSED99A;initial catalog=restaurant_management;user id=sa;password=1234$;MultipleActiveResultSets=true;TrustServerCertificate=True;");

                return new AppDbContext(optionsBuilder.Options);
            }
			catch (Exception ex)
			{
                BaseNLog.logger.Error(ex);
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                return new AppDbContext(optionsBuilder.Options);
			}
        }
    }
}
