using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace Eatster.Persistence.Shared
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            return Create();
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        private TContext Create()
        {
            //var connStr = new ConfigurationBuilder().Build().GetConnectionString("Default");

            //if (string.IsNullOrWhiteSpace(connStr))
            //{
            //    throw new InvalidOperationException(
            //        "Could not find a connection string named 'Default'.");
            //}

            return Create(@"Server=DESKTOP-P48ICQT\SQLEXPRESS;Database=EatsterDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        private TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(
             $"{nameof(connectionString)} is null or empty.",
             nameof(connectionString));

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            Console.WriteLine("DesignTimeDbContextFactory.Create(string): Connection string: {0}", connectionString);

            optionsBuilder.UseSqlServer(connectionString);

            var options = optionsBuilder.Options;
            return CreateNewInstance(options);
        }
    }
}