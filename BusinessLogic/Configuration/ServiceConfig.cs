using BusinessLogic.Interfaces;
using BusinessLogic.Services.Customers;
using BusinessLogic.Services.Orders;
using BusinessLogic.Services.Products;
using BusinessLogic.Services.Reports;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic.Configuration
{
    public static class ServiceConfig
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IReportService, ReportService>();
        }
    }
}
