
using BuildingBlocks.Contracts;
using BuildingBlocks.Infrastructure.Messaging;
using Modules.Payments.Application;
using Modules.Payments.Domain;
using Modules.Payments.Infrastructure;

namespace Modules.Payments.Application.Consumers;

public class PaymentCollectRequestedConsumer : IEventHandler<PaymentCollectRequestedEvent>
{
    private readonly IPaymentGateway _gateway;
    private readonly PaymentRepository _repo;
    private readonly IEventBus _bus;

    public PaymentCollectRequestedConsumer(IPaymentGateway gateway, PaymentRepository repo, IEventBus bus)
    {
        _gateway = gateway;
        _repo = repo;
        _bus = bus;
    }

    public async Task HandleAsync(PaymentCollectRequestedEvent evt)
    {
        var providerTxnId = await _gateway.ChargeAsync(evt.OrderId, evt.PatientId, evt.Amount);
        var payment = new Payment
        {
            OrderId = evt.OrderId,
            PatientId = evt.PatientId,
            Amount = evt.Amount,
            ProviderTxnId = providerTxnId
        };
        await _repo.AddAsync(payment);
        Console.WriteLine($"[Payments] Collected {evt.Amount} for Order {evt.OrderId} (Txn: {providerTxnId})");

        var collected = new PaymentCollectedEvent(payment.Id, payment.OrderId, payment.PatientId, payment.Amount, payment.ProviderTxnId);
        await _bus.PublishAsync("payments.collected", collected);
    }
}
