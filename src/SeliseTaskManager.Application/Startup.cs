using Microsoft.Extensions.DependencyInjection;

namespace SeliseTaskManager.Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services.AddMediatR(
                cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
        }
    }
}
