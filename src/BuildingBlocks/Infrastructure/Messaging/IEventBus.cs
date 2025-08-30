
namespace BuildingBlocks.Infrastructure.Messaging;

public interface IEvent { }

public interface IEventHandler<T> where T : IEvent
{
    Task HandleAsync(T evt);
}

public interface IEventBus
{
    Task PublishAsync<T>(string routingKey, T @event) where T : IEvent;
    void Subscribe<T, THandler>(string queue)
        where T : IEvent
        where THandler : IEventHandler<T>;
}
