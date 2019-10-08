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
            services.ConfigureDatabase(Configuration);
            services.MigrateDatabase();

            services
                .AddMvc(options => { options.Filters.Add(new GlobalExceptionFilter()); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Scan(scan =>
            {
                var assemblies = GetAssembliesForScanning();

                scan.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(ITransient)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime();
            });
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
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}