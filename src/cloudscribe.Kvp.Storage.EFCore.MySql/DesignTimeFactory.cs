using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace cloudscribe.Kvp.Storage.EFCore.MySql
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<KvpDbContext>
    {
        public KvpDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<KvpDbContext>();
            builder.UseMySql("Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;");
            return new KvpDbContext(builder.Options);
        }
    }
}
