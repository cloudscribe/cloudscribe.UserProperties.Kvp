using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using cloudscribe.UserProperties.Models;
using cloudscribe.UserProperties.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeFeatures
    {
        
        public static IServiceCollection SetupDataStorage(
            this IServiceCollection services,
            IConfiguration config,
            IWebHostEnvironment env
            )
        {

            services.AddScoped<cloudscribe.Core.Models.Setup.ISetupTask, cloudscribe.Core.Web.Components.EnsureInitialDataSetupTask>();

            var storage = config["DevOptions:DbPlatform"];
            var efProvider = config["DevOptions:EFProvider"];

            switch (storage)
            {
                case "NoDb":
                    var useSingletons = true;
                    services.AddCloudscribeCoreNoDbStorage(useSingletons);                    
                    services.AddCloudscribeLoggingNoDbStorage(config);            
                    services.AddNoDbStorageForSimpleContent();
                    services.AddCloudscribeKvpNoDbStorage();

                    break;

                case "ef":
                default:

                    switch (efProvider)
                    {
                        case "sqlite":
                            var dbName = config.GetConnectionString("SQLiteDbName");
                            var dbPath = Path.Combine(env.ContentRootPath, dbName);
                            var slConnection = $"Data Source={dbPath}";
                            services.AddCloudscribeCoreEFStorageSQLite(slConnection);                            
                            services.AddCloudscribeLoggingEFStorageSQLite(slConnection);
                            services.AddCloudscribeSimpleContentEFStorageSQLite(slConnection);                            
                            services.AddCloudscribeKvpEFStorageSQLite(slConnection);

                            break;

                        case "pgsql-old":
                            var pgConnection = config.GetConnectionString("PostgreSqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeLoggingEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeSimpleContentEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeKvpEFStoragePostgreSql(pgConnection);

                            break;

                        case "pgsql":
                            var pgsConnection = config.GetConnectionString("PostgreSqlConnectionString");
                            services.AddCloudscribeCorePostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeLoggingPostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeSimpleContentPostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeKvpPostgreSqlStorage(pgsConnection);

                            break;


                        case "MySql":
                            var mysqlConnection = config.GetConnectionString("MySqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageMySql(mysqlConnection);
                            services.AddCloudscribeLoggingEFStorageMySQL(mysqlConnection);
                            services.AddCloudscribeSimpleContentEFStorageMySQL(mysqlConnection);
                            services.AddCloudscribeKvpEFStorageMySql(mysqlConnection);

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
                            services.AddCloudscribeSimpleContentEFStorageMSSQL(connectionString);
                            services.AddCloudscribeKvpEFStorageMSSQL(connectionString);
                            
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

            services.AddCloudscribeLogging(config);

            services.Configure<ProfilePropertySetContainer>(config.GetSection("ProfilePropertySetContainer"));
            services.AddScoped<TenantProfileOptionsResolver>();

            services.AddCloudscribeKvpUserProperties();

            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.Web.Navigation.NavigationNodePermissionResolver>();
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.SimpleContent.Web.Services.PagesNavigationNodePermissionResolver>();
            
            services.AddCloudscribeCoreMvc(config);
            services.AddCloudscribeCoreIntegrationForSimpleContent(config);
            services.AddSimpleContentMvc(config);
            services.AddContentTemplatesForSimpleContent(config);
            
            services.AddMetaWeblogForSimpleContent(config.GetSection("MetaWeblogApiOptions"));
            services.AddSimpleContentRssSyndiction();



            return services;
        }

    }
}
