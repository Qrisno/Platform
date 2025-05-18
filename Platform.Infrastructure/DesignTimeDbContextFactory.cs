using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Platform.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PlatformDbContext>
    {
        //es rato damchirchda sruliad gaugebaria ver mivxvdi
        public PlatformDbContext CreateDbContext(string[] args)
        {
            // Get the directory where the command is being executed
            string basePath = Directory.GetCurrentDirectory();

            // Navigate to the API project where appsettings.json is located
            string apiProjectPath = Path.Combine(basePath, "..", "Platform.API");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            DbContextOptionsBuilder<PlatformDbContext> optionsBuilder = new();
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new PlatformDbContext(optionsBuilder.Options);
        }
    }
}