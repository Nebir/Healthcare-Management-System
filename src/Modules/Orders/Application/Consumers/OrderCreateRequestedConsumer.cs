
using BuildingBlocks.Contracts;
using BuildingBlocks.Infrastructure.Messaging;
using Modules.Orders.Domain;
using Modules.Orders.Infrastructure;

namespace Modules.Orders.Application.Consumers;

public class OrderCreateRequestedConsumer : IEventHandler<OrderCreateRequestedEvent>
{
    private readonly OrderRepository _repo;
    private readonly IEventBus _bus;

    public OrderCreateRequestedConsumer(OrderRepository repo, IEventBus bus)
    {
        _repo = repo;
        _bus = bus;
    }

    public async Task HandleAsync(OrderCreateRequestedEvent evt)
    {
        var order = new Order { PatientId = evt.PatientId, Amount = 100m };
        await _repo.AddAsync(order);
        Console.WriteLine($"[Orders] Created Order {order.Id} for Patient {evt.PatientId}");

        var created = new OrderCreatedEvent(order.Id, order.PatientId, order.Amount);
        await _bus.PublishAsync("orders.created", created);
    }
}
