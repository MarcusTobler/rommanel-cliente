using MediatR;

namespace DevPack.Domain.Mediator;

public class DomainEventPublisher : INotificationPublisher
{
    public Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}