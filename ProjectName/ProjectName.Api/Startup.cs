using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProjectName.Api.Filters;
using ProjectName.Core.Interfaces;
using ProjectName.DAL;
using ProjectName.Validator;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ProjectName.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatabase(services);
            ConfigureAutomaticRegistration(services);
            ConfigureMvc(services);
            ConfigureSwagger(services);

            services.AddMediatR(typeof(ITransient));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API"); });
            }

            app.UseSerilogRequestLogging();

            //app.UseHttpsRedirection(); //ToDo: Not work for tests

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void ConfigureAutomaticRegistration(IServiceCollection services)
        {
            services.Scan(scan =>
            {
                var assemblies = GetAssembliesForScanning();

                scan.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(ITransient)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime();

                scan.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(IScoped)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }

        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.ConfigureDatabase(Configuration);
            services.MigrateDatabase();
        }

        protected virtual void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API" }); });
        }

        private static void ConfigureMvc(IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    //options.Filters.Add<UnhandledExceptionFilter>(0);
                    //options.Filters.Add<AuthorizationExceptionFilter>(1);
                    //options.Filters.Add<NoFoundExceptionFilter>(2);
                    //options.Filters.Add<ValidationExceptionFilter>(3);

                    //options.Filters.Add<GlobalExceptionFilter>();

                    //options.Filters.Add<ResultHandlerFilter>();
                })
                .AddApplicationPart(typeof(Startup).Assembly);
        }

        private static Assembly[] GetAssembliesForScanning()
        {
            return new[]
            {
                typeof(ITransient).Assembly,
                typeof(DataContext).Assembly,
                typeof(BaseValidator<>).Assembly
            };
        }
    }
}