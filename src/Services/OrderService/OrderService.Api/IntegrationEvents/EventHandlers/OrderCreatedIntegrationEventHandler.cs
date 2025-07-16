using EventBus.Base.Abstraction;
using MediatR;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application.Features.Commands.CreateOrder;

namespace OrderService.Api.IntegrationEvents.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        private readonly IMediator mediator;
        private readonly ILogger<OrderCreatedIntegrationEventHandler> logger;
        private readonly IServiceProvider _serviceProvider;

        public OrderCreatedIntegrationEventHandler(IMediator mediator, ILogger<OrderCreatedIntegrationEventHandler> logger, IServiceProvider serviceProvider)
        {
            this.mediator = mediator;
            this.logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            try
            {
                logger.LogInformation("Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})",
                @event.Id,
                typeof(Program).Namespace,
                @event);

                var createOrderCommand = new CreateOrderCommand(@event.Basket.Items,
                                @event.UserId, @event.UserName,
                                @event.City, @event.Street,
                                @event.State, @event.Country, @event.ZipCode,
                                @event.CardNumber, @event.CardHolderName, @event.CardExpiration,
                                @event.CardSecurityNumber, @event.CardTypeId);
               
                
                
               // await mediator.Send(createOrderCommand);// No service for type 'MediatR.IRequestHandler' has been registered. hatası atıyor

                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    await mediator.Send(createOrderCommand);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());
            }
        }
    }
}
