
using BuildingBlocks.Contracts;
using BuildingBlocks.Infrastructure.Messaging;

namespace Modules.Orchestrator.Application.Consumers;

public class OrderCreatedConsumer : IEventHandler<OrderCreatedEvent>
{
    private readonly IEventBus _bus;
    public OrderCreatedConsumer(IEventBus bus) => _bus = bus;

    public async Task HandleAsync(OrderCreatedEvent evt)
    {
        Console.WriteLine($"[Orchestrator] Order created -> PaymentCollectRequested for Order {evt.OrderId}");
        var payReq = new PaymentCollectRequestedEvent(evt.OrderId, evt.PatientId, evt.Amount);
        await _bus.PublishAsync("payments.collect.requested", payReq);
    }
}
