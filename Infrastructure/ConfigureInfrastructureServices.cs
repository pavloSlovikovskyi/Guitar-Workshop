using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Queries;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IInstrumentRepository, InstrumentRepository>();
        services.AddScoped<IRepairOrderRepository, RepairOrderRepository>();
        services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();

        services.AddScoped<IInstrumentQueries, InstrumentQueries>();
        services.AddScoped<IRepairOrderQueries, RepairOrderQueries>();
        services.AddScoped<IServiceTypeQueries, ServiceTypeQueries>();

        services.AddScoped<IInstrumentPassportRepository, InstrumentPassportRepository>();
        services.AddScoped<IInstrumentPassportQueries, InstrumentPassportQueries>();

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerQueries, CustomerQueries>();

        return services;
    }

}
