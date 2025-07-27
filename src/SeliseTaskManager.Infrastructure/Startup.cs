using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Application.Interfaces.Services;
using SeliseTaskManager.Infrastructure.Auth;
using SeliseTaskManager.Infrastructure.Common.Interfaces;
using SeliseTaskManager.Infrastructure.Persistence;
using SeliseTaskManager.Infrastructure.Repositories;
using SeliseTaskManager.Infrastructure.Services;
using System.Text;

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

        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var config = configuration;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["SecuritySettings:JwtSettings:issuer"],
                        ValidAudience = config["SecuritySettings:JwtSettings:audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["SecuritySettings:JwtSettings:key"] ?? string.Empty))
                    };
                });

            services.AddAuthorization();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
