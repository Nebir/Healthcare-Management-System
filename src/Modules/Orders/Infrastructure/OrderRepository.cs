
using MongoDB.Driver;
using Modules.Orders.Domain;

namespace Modules.Orders.Infrastructure;

public class OrderRepository
{
    private readonly IMongoCollection<Order> _col;
    public OrderRepository(IMongoClient client, string dbName)
    {
        _col = client.GetDatabase(dbName).GetCollection<Order>("orders");
    }

    public Task AddAsync(Order o) => _col.InsertOneAsync(o);
}
