using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Kvp.Storage.EFCore.SQLite
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<KvpDbContext>
    {
        public KvpDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<KvpDbContext>();
            builder.UseSqlite("Data Source=cloudscribe.db");
            return new KvpDbContext(builder.Options);
        }
    }
}
