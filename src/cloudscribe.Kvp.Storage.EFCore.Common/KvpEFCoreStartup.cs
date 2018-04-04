using cloudscribe.Kvp.Storage.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KvpEFCoreStartup
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<IKvpDbContext>();

                await db.Database.MigrateAsync();


            }
        }
    }
}
