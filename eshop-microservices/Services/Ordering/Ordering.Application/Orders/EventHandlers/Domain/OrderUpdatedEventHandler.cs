namespace Ordering.Application.Orders.EventHandlers.Domain;
public class OrderUpdatedEventHandler
    (ILogger<OrderUpdatedEventHandler> logger)
    : INotificationHandler<OrderUpdatedEvent>
{
    public Task Handle(OrderUpdatedEvent updateEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", updateEvent.GetType().Name);
        return Task.CompletedTask;
    }
}
