using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;


namespace DispatchBalanceAPI.Model
{
    public class LogContextFactory : IDesignTimeDbContextFactory<DispatchBalanceContext>
    {
        public DispatchBalanceContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DispatchBalanceContext>();
            var connectionString = configuration.GetConnectionString("DbContext");
            dbContextOptionsBuilder
                .UseSqlServer(connectionString);

            return new DispatchBalanceContext(dbContextOptionsBuilder.Options, configuration);
        }
    }
}
