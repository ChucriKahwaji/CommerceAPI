using DataAccess.Interfaces;
using DataAccess.Repositories.Customers;
using DataAccess.Repositories.Orders;
using DataAccess.Repositories.Products;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Configuration
{
    public static class RepositoryConfig
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}
