using System.Reflection;
using Application.Common.Behaviours;
using Application.Reports;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddScoped<IReportGenerator, PdfReportGenerator>();
            services.AddScoped<IReportGenerator, ExcelReportGenerator>();
            services.AddScoped<IReportGenerator, WordReportGenerator>();

            return services;
        }
    }
}
