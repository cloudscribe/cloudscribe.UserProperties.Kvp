﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var config = host.Services.GetRequiredService<IConfiguration>();
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                try
                {
                    EnsureDataStorageIsReady(config, services);

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            var env = host.Services.GetRequiredService<IWebHostEnvironment>();
            ConfigureLogging(env, loggerFactory, host.Services);

            host.Run();
        }

        private static void EnsureDataStorageIsReady(IConfiguration config, IServiceProvider services)
        {
            var storage = config["DevOptions:DbPlatform"];

            switch (storage)
            {
                case "NoDb":
                    CoreNoDbStartup.InitializeDataAsync(services).Wait();
                    
                    break;

                case "ef":
                default:
                    // this creates ensures the database is created and initial data
                    CoreEFStartup.InitializeDatabaseAsync(services).Wait();

                    LoggingEFStartup.InitializeDatabaseAsync(services).Wait();

                    KvpEFCoreStartup.InitializeDatabaseAsync(services).Wait();

                    break;
            }
        }

        private static void ConfigureLogging(
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider
            )
        {
            LogLevel minimumLevel;
            if (env.IsProduction())
            {
                minimumLevel = LogLevel.Warning;
            }
            else
            {
                minimumLevel = LogLevel.Information;
            }

            
            // a customizable filter for logging
            // add exclusions to remove noise in the logs
            var excludedLoggers = new List<string>
            {
                "Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware",
                "Microsoft.AspNetCore.Hosting.Internal.WebHost",
            };

            Func<string, LogLevel, bool> logFilter = (string loggerName, LogLevel logLevel) =>
            {
                if (logLevel < minimumLevel)
                {
                    return false;
                }

                if (excludedLoggers.Contains(loggerName))
                {
                    return false;
                }

                return true;
            };

            loggerFactory.AddDbLogger(serviceProvider, logFilter);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });


        //https://joonasw.net/view/aspnet-core-2-configuration-changes

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .ConfigureAppConfiguration((builderContext, config) =>
        //        {
        //            config.AddJsonFile("app-userproperties.json", optional: true, reloadOnChange: true);
        //        })
        //        .UseStartup<Startup>()
        //        .Build();




    }
}
