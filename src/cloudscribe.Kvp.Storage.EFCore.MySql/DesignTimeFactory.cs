using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace cloudscribe.Kvp.Storage.EFCore.MySql
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<KvpDbContext>
    {
        public KvpDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<KvpDbContext>();
            var connString = "Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;";

            // for breaking changes in Net5.0:
            builder.UseMySql(connString, ServerVersion.AutoDetect(connString));
            return new KvpDbContext(builder.Options);
        }
    }
}
