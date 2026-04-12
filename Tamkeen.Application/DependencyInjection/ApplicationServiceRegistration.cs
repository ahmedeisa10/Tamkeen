using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Tamkeen.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // AutoMapper
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
