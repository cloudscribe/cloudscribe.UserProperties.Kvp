using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Kvp.Storage.EFCore.PostgreSql
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<KvpDbContext>
    {
        public KvpDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<KvpDbContext>();
            builder.UseNpgsql("server=yourservername;UID=yourdatabaseusername;PWD=yourdatabaseuserpassword;database=yourdatabasename");
            return new KvpDbContext(builder.Options);
        }
    }
}
