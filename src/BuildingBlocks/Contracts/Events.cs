
using BuildingBlocks.Infrastructure.Messaging;

namespace BuildingBlocks.Contracts;

// Patient Domain
public record PatientRegisteredEvent(string PatientId, string FullName, DateTime Dob, string Email) : IEvent;

// Order Domain
public record OrderCreateRequestedEvent(string PatientId, string FullName, string Email) : IEvent;
public record OrderCreatedEvent(string OrderId, string PatientId, decimal Amount) : IEvent;

// Payment Domain
public record PaymentCollectRequestedEvent(string OrderId, string PatientId, decimal Amount) : IEvent;
public record PaymentCollectedEvent(string PaymentId, string OrderId, string PatientId, decimal Amount, string ProviderTxnId) : IEvent;
