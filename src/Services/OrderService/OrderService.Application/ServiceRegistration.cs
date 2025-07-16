using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderService.Application.Features.Commands.CreateOrder;


namespace OrderService.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services, Type program)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(program.Assembly);
                cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommandHandler).Assembly);
            });

            services.AddAutoMapper(program.Assembly);

            return services;
        }
    }
}
