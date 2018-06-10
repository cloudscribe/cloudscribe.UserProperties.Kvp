using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Kvp.Storage.EFCore.MSSQL
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<KvpDbContext>
    {
        public KvpDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<KvpDbContext>();
            builder.UseSqlServer("Server=(local);Database=DATABASENAME;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new KvpDbContext(builder.Options);
        }
    }
}
