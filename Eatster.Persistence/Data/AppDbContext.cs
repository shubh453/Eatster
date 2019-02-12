using Eatster.Domain.Abstract;
using Eatster.Domain.Entities;
using Eatster.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eatster.Persistence.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
        }

        #region Override SaveChange Methods

        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuitInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddAuitInfo()
        {
            var entries = ChangeTracker.Entries()
                            .Where(x => x.Entity is BaseEntity &&
                                  (x.State == EntityState.Added ||
                                   x.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).Created = DateTime.UtcNow;
                }
                ((BaseEntity)entry.Entity).Modified = DateTime.UtcNow;
            }
        }

        #endregion Override SaveChange Methods
    }
}