﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;


namespace ToDoFlutter.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var azConfigSettings = config.Build();
                    var azConfigConnection = azConfigSettings.GetConnectionString("AppConfig");
                    if (!string.IsNullOrEmpty(azConfigConnection))
                    {
                        // Use the connection string if it is available.
                        config.AddAzureAppConfiguration(azConfigConnection);
                    }
                    else if (Uri.TryCreate(azConfigSettings["Endpoints:AppConfig"], UriKind.Absolute, out var endpoint))
                    {
                        // Use Azure Active Directory authentication.
                        config.AddAzureAppConfiguration(options =>
                        {
                            options.Connect(endpoint, new DefaultAzureCredential());
                        });
                    }
                    var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                    config.AddAzureKeyVault(
                    keyVaultEndpoint,
                    new DefaultAzureCredential());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        /*public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var settings = config.Build();

                config.AddAzureAppConfiguration(options =>
                {
                    options.Connect(settings["ConnectionStrings:AppConfig"])
                            .ConfigureKeyVault(kv =>
                            {
                                kv.SetCredential(new DefaultAzureCredential());
                            });
                });
            })
            .UseStartup<Startup>());*/

        /*public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var azConfigSettings = config.Build();
                    var azConfigConnection = azConfigSettings.GetConnectionString("AppConfig");
                    if (!string.IsNullOrEmpty(azConfigConnection))
                    {
                        // Use the connection string if it is available.
                        config.AddAzureAppConfiguration(azConfigConnection);
                    }
                    else if (Uri.TryCreate(azConfigSettings["Endpoints:AppConfig"], UriKind.Absolute, out var endpoint))
                    {
                        // Use Azure Active Directory authentication.
                        config.AddAzureAppConfiguration(options =>
                        {
                            options.Connect(endpoint, new DefaultAzureCredential());
                        });
                    }
                    var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                    config.AddAzureKeyVault(
                    keyVaultEndpoint,
                    new DefaultAzureCredential());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });*/


        /*public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });*/


    }
}
