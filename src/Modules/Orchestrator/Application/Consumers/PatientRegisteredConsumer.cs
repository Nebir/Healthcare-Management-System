
using BuildingBlocks.Contracts;
using BuildingBlocks.Infrastructure.Messaging;

namespace Modules.Orchestrator.Application.Consumers;

public class PatientRegisteredConsumer : IEventHandler<PatientRegisteredEvent>
{
    private readonly IEventBus _bus;
    public PatientRegisteredConsumer(IEventBus bus) => _bus = bus;

    public async Task HandleAsync(PatientRegisteredEvent evt)
    {
        Console.WriteLine($"[Orchestrator] Patient registered -> OrderCreateRequested for {evt.FullName}");
        var request = new OrderCreateRequestedEvent(evt.PatientId, evt.FullName, evt.Email);
        await _bus.PublishAsync("orders.create.requested", request);
    }
}
