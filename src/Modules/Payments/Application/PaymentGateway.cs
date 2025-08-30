
namespace Modules.Payments.Application;

public interface IPaymentGateway
{
    Task<string> ChargeAsync(string orderId, string patientId, decimal amount, CancellationToken ct = default);
}

public class FakePaymentGateway : IPaymentGateway
{
    public Task<string> ChargeAsync(string orderId, string patientId, decimal amount, CancellationToken ct = default)
    {
        return Task.FromResult($"PGTXN-{Guid.NewGuid().ToString("N")[..8]}");
    }
}
