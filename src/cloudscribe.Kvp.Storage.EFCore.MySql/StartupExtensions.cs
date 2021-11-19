using cloudscribe.Kvp.Storage.EFCore.Common;
using cloudscribe.Kvp.Storage.EFCore.MySql;
using cloudscribe.Versioning;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpEFStorageMySql(
            this IServiceCollection services,
            string connectionString,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<int> transientSqlErrorNumbersToAdd = null
            )
        {
            services.AddCloudscribeKvpEFCommon();

            services // .AddEntityFrameworkMySql()
                .AddDbContext<KvpDbContext>(options =>
                    options.UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString),  // breaking change here in Net5.0
                    mySqlOptionsAction: sqlOptions =>
                    {
                        if (maxConnectionRetryCount > 0)
                        {
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: maxConnectionRetryCount,
                                maxRetryDelay: TimeSpan.FromSeconds(maxConnectionRetryDelaySeconds),
                                errorNumbersToAdd: transientSqlErrorNumbersToAdd);
                        }


                    }),
                    optionsLifetime: ServiceLifetime.Singleton
                    );

            

            services.AddScoped<IKvpDbContext, KvpDbContext>();
            services.AddSingleton<IKvpDbContextFactory, KvpDbContextFactory>();
            services.AddScoped<IVersionProvider, VersionProvider>();

            return services;
        }
    }
}
