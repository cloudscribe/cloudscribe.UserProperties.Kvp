
using cloudscribe.Kvp.Storage.EFCore.Common;
using cloudscribe.Kvp.Storage.EFCore.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpEFStorageMSSQL(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddCloudscribeKvpEFCommon();

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<KvpDbContext>((serviceProvider, options) =>
                options.UseSqlServer(connectionString)
                       .UseInternalServiceProvider(serviceProvider)
                       );

            services.AddScoped<IKvpDbContext, KvpDbContext>();


            return services;
        }
    }
}
