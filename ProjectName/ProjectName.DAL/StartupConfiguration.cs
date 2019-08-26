using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectName.DAL
{
    public static class StartupConfiguration
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServerConnection");
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
        }

        public static void MigrateDatabase(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dataContext = scopedServices.GetRequiredService<DataContext>();
                dataContext.Database.Migrate();
            }
        }
    }
}