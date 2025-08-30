
namespace Modules.Payments.Domain;

public class Payment // Aggregate Root
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string OrderId { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string ProviderTxnId { get; set; } = string.Empty;
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
}
