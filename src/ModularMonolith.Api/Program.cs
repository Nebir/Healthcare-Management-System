
using BuildingBlocks.Infrastructure.Messaging;
using BuildingBlocks.Contracts;
using MongoDB.Driver;

// Patients
using Modules.Patients.Infrastructure;
using Modules.Patients.Application;

// Orders
using Modules.Orders.Infrastructure;
using Modules.Orders.Application.Consumers;

// Payments
using Modules.Payments.Infrastructure;
using Modules.Payments.Application;
using Modules.Payments.Application.Consumers;

// Orchestrator
using Modules.Orchestrator.Application.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Mongo
var mongoClient = new MongoClient(builder.Configuration["MongoDb:ConnectionString"]);
builder.Services.AddSingleton(mongoClient);
builder.Services.AddScoped(_ => new PatientRepository(mongoClient, builder.Configuration["MongoDb:PatientsDatabase"]!));
builder.Services.AddScoped(_ => new OrderRepository(mongoClient, builder.Configuration["MongoDb:OrdersDatabase"]!));
builder.Services.AddScoped(_ => new PaymentRepository(mongoClient, builder.Configuration["MongoDb:PaymentsDatabase"]!));

// RabbitMQ
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

// Domain services
builder.Services.AddScoped<PatientService>();

// Consumers (handlers)
builder.Services.AddScoped<PatientRegisteredConsumer>();
builder.Services.AddScoped<OrderCreateRequestedConsumer>();
builder.Services.AddScoped<OrderCreatedConsumer>();
builder.Services.AddScoped<PaymentCollectRequestedConsumer>();

// Payment Gateway
builder.Services.AddScoped<IPaymentGateway, FakePaymentGateway>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Subscribe orchestrations and domain consumers
using (var scope = app.Services.CreateScope())
{
    var bus = scope.ServiceProvider.GetRequiredService<IEventBus>();

    // Orchestrator
    bus.Subscribe<PatientRegisteredEvent, PatientRegisteredConsumer>("orchestrator_patient_registered");
    bus.Subscribe<OrderCreatedEvent, OrderCreatedConsumer>("orchestrator_order_created");

    // Orders domain reacts to orchestrator's request
    bus.Subscribe<OrderCreateRequestedEvent, OrderCreateRequestedConsumer>("orders_create_requested");

    // Payments domain reacts to orchestrator's request
    bus.Subscribe<PaymentCollectRequestedEvent, PaymentCollectRequestedConsumer>("payments_collect_requested");
}

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.MapControllers();
app.Run();
