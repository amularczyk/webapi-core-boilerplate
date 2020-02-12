using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProjectName.Api.Controllers.Ex;
using ProjectName.Api.Filters;
using ProjectName.Core.Interfaces;
using ProjectName.DAL;
using ProjectName.Validator;
using Serilog;

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
            services.AddTransient<SomeObjectManager>();
            services.AddTransient<Controllers.Res.SomeObjectManager>();

            ConfigureDatabase(services);
            ConfigureAutomaticRegistration(services);
            ConfigureMvc(services);
            ConfigureSwagger(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API"); });
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseMvc();
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
            });
        }

        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.ConfigureDatabase(Configuration);
            services.MigrateDatabase();
        }

        protected virtual void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API"}); });
        }

        private static void ConfigureMvc(IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add(typeof(GlobalExceptionFilter));
                });
        }

        private static Assembly[] GetAssembliesForScanning()
        {
            return new[]
            {
                typeof(SomeObjectManager).Assembly,
                typeof(Controllers.Res.SomeObjectManager).Assembly,
                typeof(ITransient).Assembly,
                typeof(DataContext).Assembly,
                typeof(BaseValidator<>).Assembly
            };
        }
    }
}