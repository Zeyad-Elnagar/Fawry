using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fawry.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class FawryDbContextFactory : IDesignTimeDbContextFactory<FawryDbContext>
{
    public FawryDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        FawryEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<FawryDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new FawryDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Fawry.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
