using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Xunit;

namespace ProjectName.Api.E2ETests
{
    public class WebApiTesterFactory : WebApplicationFactory<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var configuration = GetConfiguration();
            ConfigureLogger(configuration);

            return new WebHostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) => { config.AddJsonFile("appsettings.json"); })
                .UseSerilog()
                .UseEnvironment("Test")
                .UseStartup<TestStartup>();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
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
                .WriteTo.File(
                    new RenderedCompactJsonFormatter(),
                    Path.Combine(basePath, $"logs{Path.DirectorySeparatorChar}log.log"),
                    rollingInterval: RollingInterval.Day
                )
                .CreateLogger();
        }
    }

    [CollectionDefinition(nameof(SharedDatabase))]
    public class SharedDatabase : IClassFixture<WebApiTesterFactory>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}