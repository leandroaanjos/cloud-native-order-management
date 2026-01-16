using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Abstractions;
using OrderManagement.Infrastructure.Persistence;

namespace OrderManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("Postgres")
                   ?? throw new InvalidOperationException("Connection string 'Postgres' was not found.");

            services.AddDbContext<OrderManagementDbContext>(opt => opt.UseNpgsql(conn));
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
