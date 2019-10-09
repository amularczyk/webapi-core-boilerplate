using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectName.Core.Interfaces;
using ProjectName.DAL;
using ProjectName.Validator.Validators;
using ProjectName.Web.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace ProjectName.Web
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

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.ConfigureDatabase(Configuration);
            services.MigrateDatabase();
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" }); });
        }

        private static void ConfigureMvc(IServiceCollection services)
        {
            services
                .AddMvc(options => { options.Filters.Add(new GlobalExceptionFilter()); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private static Assembly[] GetAssembliesForScanning()
        {
            return new[]
            {
                typeof(ITransient).Assembly,
                typeof(DataContext).Assembly,
                typeof(ItemsValidator).Assembly
            };
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}