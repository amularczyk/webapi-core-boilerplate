using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace ProjectName.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            ConfigureLogger(configuration);

            try
            {
                Log.Logger.Information("Starting an app");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "App terminated unexpectedly");
            }
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config => { config.AddJsonFile("appsettings.local.json", true); })
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                .AddJsonFile("appsettings.local.json", true)
                .Build();
        }

        private static void ConfigureLogger(IConfigurationRoot configuration)
        {
            var basePath =
                Path.GetDirectoryName(
                    Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path)
                );

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.File(
                    new RenderedCompactJsonFormatter(),
                    Path.Combine(basePath, $"logs{Path.DirectorySeparatorChar}log.log"),
                    rollingInterval: RollingInterval.Day
                )
                .CreateLogger();
        }
    }
}