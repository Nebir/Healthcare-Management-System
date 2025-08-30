
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BuildingBlocks.Infrastructure.Messaging;

public class RabbitMqEventBus : IEventBus
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _sp;
    private readonly string _exchange;

    public RabbitMqEventBus(IOptions<RabbitMqOptions> options, IServiceProvider sp)
    {
        var opt = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = opt.Host,
            Port = opt.Port,
            UserName = opt.Username,
            Password = opt.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _exchange = opt.Exchange;
        _sp = sp;

        _channel.ExchangeDeclare(_exchange, ExchangeType.Topic, durable: true);
    }

    public Task PublishAsync<T>(string routingKey, T @event) where T : IEvent
    {
        var json = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(json);
        _channel.BasicPublish(_exchange, routingKey, basicProperties: null, body: body);
        return Task.CompletedTask;
    }

    public void Subscribe<T, THandler>(string queue)
        where T : IEvent
        where THandler : IEventHandler<T>
    {
        _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue, _exchange, routingKey: "#");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var evt = System.Text.Json.JsonSerializer.Deserialize<T>(json);
                using var scope = _sp.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<THandler>();
                await handler.HandleAsync(evt!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BUS] Error delivering message: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue, autoAck: true, consumer);
    }
}

public class RabbitMqOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string Exchange { get; set; } = "modular-monolith";
}
