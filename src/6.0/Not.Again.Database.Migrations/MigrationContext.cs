using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Not.Again.Database.Migrations
{
    public class MigrationContext : NotAgainDbContext
    {
        public MigrationContext() { }

        public MigrationContext(DbContextOptions<NotAgainDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration =
                    new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

                optionsBuilder
                    .UseSqlServer(
                        configuration.GetConnectionString("NOT-AGAIN"),
                        options => options.MigrationsAssembly("Not.Again.Database.Migrations")
                    );
            }
        }
    }
}