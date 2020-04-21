using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using cloudscribe.UserProperties.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeFeatures
    {

        //public static IServiceCollection SetupDataStorage(
        //    this IServiceCollection services,
        //    IConfiguration config,
        //    IWebHostEnvironment env
        //    )
        //{
        //    services.AddCloudscribeCoreNoDbStorage();
        //    services.AddCloudscribeLoggingNoDbStorage(config);
        //    services.AddNoDbStorageForSimpleContent();


        //    services.AddCloudscribeKvpEFStorageMSSQL(connectionString);

        //    return services;
        //}

        public static IServiceCollection SetupDataStorage(
            this IServiceCollection services,
            IConfiguration config,
            IWebHostEnvironment env
            )
        {
            //services.AddScoped<cloudscribe.Core.Models.Setup.ISetupTask, cloudscribe.Core.Web.Components.EnsureInitialDataSetupTask>();

            var storage = config["DevOptions:DbPlatform"];
            var efProvider = config["DevOptions:EFProvider"];
            var useMiniProfiler = config.GetValue<bool>("DevOptions:EnableMiniProfiler");

            switch (storage)
            {
                case "NoDb":
                    var useSingletons = true;
                    services.AddCloudscribeCoreNoDbStorage(useSingletons);
                    services.AddCloudscribeLoggingNoDbStorage(config);
                    services.AddCloudscribeKvpNoDbStorage();
                    services.AddNoDbStorageForSimpleContent(useSingletons);

                    //if (useMiniProfiler)
                    //{
                    //    services.AddMiniProfiler();
                    //}


                    break;

                case "ef":
                default:

                    //if (useMiniProfiler)
                    //{
                    //    services.AddMiniProfiler()
                    //        .AddEntityFramework();
                    //}

                    switch (efProvider)
                    {
                        case "sqlite":

                            var dbName = config.GetConnectionString("SQLiteDbName");
                            var dbPath = Path.Combine(env.ContentRootPath, dbName);
                            var slConnection = $"Data Source={dbPath}";

                            //var slConnection = config.GetConnectionString("SQLiteEntityFrameworkConnectionString");
                            //Data Source=cloudscribe.dev2.db


                            services.AddCloudscribeCoreEFStorageSQLite(slConnection);
                            services.AddCloudscribeLoggingEFStorageSQLite(slConnection);
                            services.AddCloudscribeKvpEFStorageSQLite(slConnection);
                            services.AddCloudscribeSimpleContentEFStorageSQLite(slConnection);
                            break;

                        case "pgsql-old":
                            var pgConnection = config.GetConnectionString("PostgreSqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeLoggingEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeKvpEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeSimpleContentEFStoragePostgreSql(pgConnection);
                            break;

                        case "pgsql":
                            var pgsConnection = config.GetConnectionString("PostgreSqlConnectionString");
                            services.AddCloudscribeCorePostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeLoggingPostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeKvpPostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeSimpleContentPostgreSqlStorage(pgsConnection);

                            break;


                        case "MySql":
                            var mysqlConnection = config.GetConnectionString("MySqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageMySql(mysqlConnection);
                            services.AddCloudscribeLoggingEFStorageMySQL(mysqlConnection);
                            services.AddCloudscribeKvpEFStorageMySql(mysqlConnection);
                            services.AddCloudscribeSimpleContentEFStorageMySQL(mysqlConnection);

                            break;

                        case "MSSQL":
                        default:
                            var connectionString = config.GetConnectionString("EntityFrameworkConnectionString");

                            // this shows all the params with default values
                            // only connectionstring is required to be passed in
                            services.AddCloudscribeCoreEFStorageMSSQL(
                                connectionString: connectionString,
                                maxConnectionRetryCount: 0,
                                maxConnectionRetryDelaySeconds: 30,
                                transientSqlErrorNumbersToAdd: null,
                                useSql2008Compatibility: false);

                            //services.AddCloudscribeCoreEFStorageMSSQL(
                            //    connectionString: connectionString,
                            //    useSql2008Compatibility: true);


                            services.AddCloudscribeLoggingEFStorageMSSQL(connectionString);
                            services.AddCloudscribeKvpEFStorageMSSQL(connectionString);
                            services.AddCloudscribeSimpleContentEFStorageMSSQL(connectionString);

                            break;
                    }


                    break;
            }

            return services;
        }

        public static IServiceCollection SetupCloudscribeFeatures(
            this IServiceCollection services,
            IConfiguration config
            )
        {
            services.Configure<ProfilePropertySetContainer>(config.GetSection("ProfilePropertySetContainer"));
            services.AddCloudscribeKvpUserProperties();
            services.AddScoped<cloudscribe.Versioning.IVersionProvider, cloudscribe.Web.StaticFiles.VersionProvider>();
            
            services.AddCloudscribeLogging(config);            
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.Web.Navigation.NavigationNodePermissionResolver>();
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.SimpleContent.Web.Services.PagesNavigationNodePermissionResolver>();
            
            services.AddCloudscribeCoreMvc(config);
            services.AddCloudscribeCoreIntegrationForSimpleContent(config);
            services.AddSimpleContentMvc(config);
            services.AddContentTemplatesForSimpleContent(config);

            services.AddMetaWeblogForSimpleContent(config.GetSection("MetaWeblogApiOptions"));
            services.AddSimpleContentRssSyndiction();

            services.Configure<ProfilePropertySetContainer>(config.GetSection("ProfilePropertySetContainer"));
            services.AddCloudscribeKvpUserProperties();

            return services;
        }

    }
}

