using cloudscribe.Kvp.Models;
using cloudscribe.Kvp.Storage.NoDb;
using cloudscribe.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpNoDbStorage(this IServiceCollection services, IConfigurationRoot configuration = null)
        {
            services.AddNoDb<KvpItem>();
            services.TryAddScoped<IKvpItemQueries, KvpItemQueries>();
            services.TryAddScoped<IKvpItemCommands, KvpItemCommands>();
            services.AddScoped<IVersionProvider, VersionProvider>();


            return services;
        }
    }
}
