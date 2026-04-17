using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shop.Domain.Interfaces;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Repositories;

namespace Shop.Infrastructure.Security
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
