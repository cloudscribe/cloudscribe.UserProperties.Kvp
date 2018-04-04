using cloudscribe.Kvp.Storage.EFCore.Common;
using cloudscribe.Kvp.Storage.EFCore.MySql;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpEFStorageMySql(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddCloudscribeKvpEFCommon();

            services.AddEntityFrameworkMySql()
                .AddDbContext<KvpDbContext>((serviceProvider, options) =>
                options.UseMySql(connectionString)
                       .UseInternalServiceProvider(serviceProvider)
                       );

            services.AddScoped<IKvpDbContext, KvpDbContext>();


            return services;
        }
    }
}
