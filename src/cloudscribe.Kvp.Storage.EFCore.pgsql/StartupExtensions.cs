using cloudscribe.Kvp.Storage.EFCore.Common;
using cloudscribe.Kvp.Storage.EFCore.pgsql;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpEFStoragePostgreSql(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddCloudscribeKvpEFCommon();
            services.AddEntityFrameworkNpgsql()
                    .AddDbContext<KvpDbContext>((serviceProvider, options) =>
                    options.UseNpgsql(connectionString)
                           .UseInternalServiceProvider(serviceProvider)
                           );

            services.AddScoped<IKvpDbContext, KvpDbContext>();


            return services;
        }

    }
}
