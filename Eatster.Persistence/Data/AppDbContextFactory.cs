using Eatster.Persistence.Shared;
using Microsoft.EntityFrameworkCore;

namespace Eatster.Persistence.Data
{
    public class AppDbContextFactory : DesignTimeDbContextFactoryBase<AppDbContext>
    {
        protected override AppDbContext CreateNewInstance(DbContextOptions<AppDbContext> options)
        {
            return new AppDbContext(options);
        }
    }
}