using cloudscribe.Kvp.Storage.EFCore.Common;
using cloudscribe.Kvp.Storage.EFCore.pgsql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpEFStoragePostgreSql(
            this IServiceCollection services,
            string connectionString,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<string> transientErrorCodesToAdd = null
            )
        {
            services.AddCloudscribeKvpEFCommon();

            //services.AddEntityFrameworkNpgsql()
            //        .AddDbContext<KvpDbContext>((serviceProvider, options) =>
            //        options.UseNpgsql(connectionString)
            //               .UseInternalServiceProvider(serviceProvider)
            //               );

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<KvpDbContext>(options =>
                    options.UseNpgsql(connectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        if (maxConnectionRetryCount > 0)
                        {
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: maxConnectionRetryCount,
                                maxRetryDelay: TimeSpan.FromSeconds(maxConnectionRetryDelaySeconds),
                                errorCodesToAdd: transientErrorCodesToAdd);
                        }


                    }));

            services.AddScoped<IKvpDbContext, KvpDbContext>();


            return services;
        }

    }
}
