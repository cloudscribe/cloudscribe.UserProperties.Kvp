using cloudscribe.Kvp.Models;
using Microsoft.Extensions.DependencyInjection;

namespace cloudscribe.Kvp.Storage.EFCore.Common
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpEFCommon(
            this IServiceCollection services
            )
        {
            services.AddScoped<IKvpItemQueries, KvpItemQueries>();
            services.AddScoped<IKvpItemCommands, KvpItemCommands>();
            services.AddScoped<IKvpUserSearchQueries, KvpUserSearchQueries>();

            return services;
        }
    }
}
