using System.Collections.Generic;
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
using ProjectName.Api.Middlewares;
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
            ConfigureDatabase(services);
            ConfigureAutomaticRegistration(services);
            ConfigureMvc(services);
            ConfigureSwagger(services);

            services.AddMediatR(typeof(ITransient));

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("default", builder =>
            //    {
            //        builder.AuthenticationSchemes.Add("Bearer");
            //        builder.RequireAuthenticatedUser();
            //    });
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API"); });
            }

            app.UseMiddleware<OperationResultHandlingMiddleware>();

            app.UseSerilogRequestLogging();

            //app.UseHttpsRedirection(); //ToDo: Not work for tests

            app.UseRouting();

            app.UseMiddleware<AuthorizationMiddleware>();

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API" });
                c.AddSecurityDefinition("AuthorizationBearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      <br/> Enter your token in the text input below.
                      <br/> You don't have to add prefix 'Bearer'.
                      <br/> Example: 'this_is_my_token'",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                c.AddSecurityDefinition("Tmp", new OpenApiSecurityScheme
                {
                    Description = @"Foo",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "AuthorizationBearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer"
                        },
                        new List<string>()
                    }
                });
            });
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

                    //options.Filters.Add<OperationResultHandlerFilter>();
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