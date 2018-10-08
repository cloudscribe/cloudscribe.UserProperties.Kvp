using cloudscribe.Kvp.Storage.EFCore.Common;
using cloudscribe.Kvp.Storage.EFCore.SQLite;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpEFStorageSQLite(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddCloudscribeKvpEFCommon();

            services.AddDbContext<KvpDbContext>((serviceProvider, options) =>
                    options.UseSqlite(connectionString), 
                    optionsLifetime: ServiceLifetime.Singleton
                    );

            services.AddScoped<IKvpDbContext, KvpDbContext>();
            services.AddSingleton<IKvpDbContextFactory, KvpDbContextFactory>();

            return services;
        }
    }
}
