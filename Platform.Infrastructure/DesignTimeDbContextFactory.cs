using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Platform.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PlatformDbContext>
{
    //es rato damchirchda sruliad gaugebaria ver mivxvdi
    public PlatformDbContext CreateDbContext(string[] args)
    {

        // Get the directory where the command is being executed
        var basePath = Directory.GetCurrentDirectory();

        // Navigate to the API project where appsettings.json is located
        var apiProjectPath = Path.Combine(basePath, "..", "Platform.API");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PlatformDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new PlatformDbContext(optionsBuilder.Options);
    }
}