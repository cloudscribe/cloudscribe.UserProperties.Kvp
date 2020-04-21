using cloudscribe.Web.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class RoutingAndMvc
    {

        /// this traditional mvc routing works around a bug in endpoint routing
        public static IRouteBuilder UseCustomRoutes(this IRouteBuilder routes)
        {
            routes.AddCultureBlogRoutesForSimpleContent(new CultureSegmentRouteConstraint());
            routes.AddBlogRoutesForSimpleContent();
            routes.AddSimpleContentStaticResourceRoutes();
            routes.AddCloudscribeFileManagerRoutes();
            routes.MapRoute(
                name: "errorhandler",
                template: "oops/error/{statusCode?}",
                defaults: new { controller = "Oops", action = "Error" }
                );


            routes.MapRoute(
                       name: "api-sitemap-culture",
                       template: "{culture}/api/sitemap"
                       , defaults: new { controller = "CultureSiteMap", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapRoute(
                       name: "api-rss-culture",
                       template: "{culture}/api/rss"
                       , defaults: new { controller = "CultureRss", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapRoute(
                       name: "api-metaweblog-culture",
                       template: "{culture}/api/metaweblog"
                       , defaults: new { controller = "CultureMetaweblog", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapRoute(
                name: "sitemap-localized",
                template: "{culture}/sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                , constraints: new { culture = new CultureSegmentRouteConstraint() }
                );

            routes.MapRoute(
                name: "sitemap",
                template: "sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                );
            routes.MapRoute(
                    name: "default-localized",
                    template: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );

            routes.MapRoute(
                name: "def",
                template: "{controller}/{action}"
                , defaults: new { action = "Index" }
                );
            routes.AddCulturePageRouteForSimpleContent(new CultureSegmentRouteConstraint());
            routes.AddDefaultPageRouteForSimpleContent();


            return routes;
        }











        // this new endpoint routing has bugs that breaks folder and culture route constraints, this code could be used later after aspnetcore team fixes the bug
        // https://github.com/aspnet/AspNetCore/issues/14877
        public static IEndpointRouteBuilder UseCustomRoutes(this IEndpointRouteBuilder routes)
        {
            routes.AddCultureBlogRoutesForSimpleContent(new CultureSegmentRouteConstraint());
            routes.AddBlogRoutesForSimpleContent();
            routes.AddSimpleContentStaticResourceRoutes();
            routes.AddCloudscribeFileManagerRoutes();
            routes.MapControllerRoute(
                name: "errorhandler",
                pattern: "oops/error/{statusCode?}",
                defaults: new { controller = "Oops", action = "Error" }
                );


            routes.MapControllerRoute(
                       name: "api-sitemap-culture",
                       pattern: "{culture}/api/sitemap"
                       , defaults: new { controller = "CultureSiteMap", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapControllerRoute(
                       name: "api-rss-culture",
                       pattern: "{culture}/api/rss"
                       , defaults: new { controller = "CultureRss", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapControllerRoute(
                       name: "api-metaweblog-culture",
                       pattern: "{culture}/api/metaweblog"
                       , defaults: new { controller = "CultureMetaweblog", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapControllerRoute(
                name: "sitemap-localized",
                pattern: "{culture}/sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                , constraints: new { culture = new CultureSegmentRouteConstraint() }
                );

            routes.MapControllerRoute(
                name: "sitemap",
                pattern: "sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                );
            routes.MapControllerRoute(
                    name: "default-localized",
                    pattern: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );

            routes.MapControllerRoute(
                name: "def",
                pattern: "{controller}/{action}"
                , defaults: new { action = "Index" }
                );
            routes.AddCulturePageRouteForSimpleContent(new CultureSegmentRouteConstraint());
            routes.AddDefaultPageRouteForSimpleContent();


            return routes;
        }

        public static IServiceCollection SetupMvc(
            this IServiceCollection services,
            bool sslIsAvailable
            )
        {
            services.Configure<MvcOptions>(options =>
            {
                // workaround for 
                //https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/466
                options.EnableEndpointRouting = false;
                if (sslIsAvailable)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }

                options.CacheProfiles.Add("SiteMapCacheProfile",
                     new CacheProfile
                     {
                         Duration = 30
                     });


                options.CacheProfiles.Add("RssCacheProfile",
                     new CacheProfile
                     {
                         Duration = 100
                     });
            });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddMvc()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());
                })
                ;

            return services;
        }

    }
}
