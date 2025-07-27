using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Infrastructure.Persistence;
using SeliseTaskManager.Infrastructure.Repositories;

namespace SeliseTaskManager.Infrastructure
{
    public static class Startup
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            //services.AddIdentity<ApplicationUser, ApplicationRole>()
            //        .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            //services.AddTransient<ApplicationDbInitializer>();
            //services.AddTransient<ApplicationDbSeeder>();
        }
    }
}
