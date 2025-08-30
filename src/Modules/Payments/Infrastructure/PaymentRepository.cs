
using MongoDB.Driver;
using Modules.Payments.Domain;

namespace Modules.Payments.Infrastructure;

public class PaymentRepository
{
    private readonly IMongoCollection<Payment> _col;
    public PaymentRepository(IMongoClient client, string dbName)
    {
        _col = client.GetDatabase(dbName).GetCollection<Payment>("payments");
    }

    public Task AddAsync(Payment p) => _col.InsertOneAsync(p);
}
