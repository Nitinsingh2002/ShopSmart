using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ShoapMart.Api.Data;
using System.IO;

namespace ShoapMart.Api.Data.Seed
{
    public class ShopMartContextFactory : IDesignTimeDbContextFactory<ShopMartContext>
    {
        public ShopMartContext CreateDbContext(string[] args)
        {
            // Build configuration to read connection string from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ShopMartContext>();
            var connectionString = configuration.GetConnectionString("ShopMartConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new ShopMartContext(optionsBuilder.Options);
        }
    }
}
