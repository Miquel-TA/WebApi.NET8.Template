using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Logic.Interfaces;
using MyApp.Repository;

namespace MyApp.Logic.Implementations.Dependencies
{
    public static class ServiceRegistration
    {
        // We'll do an InMemory DB for easy testing.
        public static void ConfigureServices_InMemory(IServiceCollection services, IConfiguration configuration)
        {
            // InMemory DB
            services.AddDbContext<MyAppDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));

            // Register repo
            services.AddScoped<IUserRepository, UserRepository>();

            // Register logic
            services.AddScoped<IUserLogic, UserLogic>();
        }
    }
}
