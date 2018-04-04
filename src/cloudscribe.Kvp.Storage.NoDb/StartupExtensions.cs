using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoDb;
using cloudscribe.Kvp.Storage.NoDb;
using System;
using System.Collections.Generic;
using System.Text;
using cloudscribe.Kvp.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpNoDbStorage(this IServiceCollection services, IConfigurationRoot configuration = null)
        {
            services.AddNoDb<KvpItem>();
            services.TryAddScoped<IKvpItemQueries, KvpItemQueries>();
            services.TryAddScoped<IKvpItemCommands, KvpItemCommands>();

            

            return services;
        }
    }
}
