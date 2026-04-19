using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces;
using Shop.Application.IReadRepositories;
using Shop.Domain.Factories;
using Shop.Domain.Interfaces;
using Shop.Domain.Interfaces.IFactories;
using Shop.Domain.Interfaces.IRepositories;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Queries;
using Shop.Infrastructure.Repositories;

namespace Shop.Infrastructure.Security
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            // Read repositories
            services.AddScoped<IAddressReadRepository, AddressReadRepository>();
            services.AddScoped<IAnalyticsReadRepository, AnalyticsReadRepository>();
            services.AddScoped<ICartReadRepository, CartReadRepository>();
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();

            // Other
            services.AddScoped<ICategoryFactory, CategoryFactory>();
            services.AddScoped<ICustomerFactory, CustomerFactory>();
            services.AddScoped<IOrderFactory, OrderFactory>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
