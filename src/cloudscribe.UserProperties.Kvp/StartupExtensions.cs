using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Kvp.Models;
using cloudscribe.UserProperties.Models;
using cloudscribe.UserProperties.Services;
using cloudscribe.UserProperties.Kvp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using cloudscribe.Versioning;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpUserProperties(this IServiceCollection services, IConfigurationRoot configuration = null)
        {
            services.TryAddScoped<IUserPropertyService, UserPropertyService>();
            services.TryAddScoped<IUserPropertyValidator, UserPropertyValidator>();
            services.TryAddScoped<IProfileOptionsResolver, TenantProfileOptionsResolver>();
            services.TryAddScoped<IKvpStorageService, KvpStorageService>();

            services.TryAddScoped<IHandleCustomRegistration, KvpRegistrationHandler>();
            services.TryAddScoped<IHandleCustomUserInfo, KvpUserInfoHandler>();
            services.TryAddScoped<IHandleCustomUserInfoAdmin, KvpUserInfoAdminHandler>();
            
            // Register post-delete handler for automatic KVP cleanup when users are deleted
            services.TryAddScoped<IHandleUserPostDelete, KvpUserPostDeleteHandler>();
            
            services.AddScoped<IVersionProvider, VersionProvider>();


            return services;
        }
    }
}
