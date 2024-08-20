using Microsoft.EntityFrameworkCore;
using Not.Again.Domain;

namespace Not.Again.Database
{
    public class NotAgainDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public NotAgainDbContext() { }

        public NotAgainDbContext(DbContextOptions<NotAgainDbContext> options)
            : base(options)
        {
        }

        public DbSet<TestAssembly> TestAssembly { get; set; }

        public DbSet<TestRecord> TestRecord { get; set; }

        public DbSet<TestRun> TestRun { get; set; }
    }
}