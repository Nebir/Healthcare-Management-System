# National-Scale Healthcare Management System (HMS)

# Project Summary: 
Modular Monolith DDD with Event-Driven Orchestration

# Architecture:

Modular Monolith using Domain-Driven Design (DDD).

Each domain is implemented as a separate module/project but runs in the same process.

Orchestrator coordinates workflows between domains using domain events.

Event-Driven Communication implemented with RabbitMQ for decoupled message passing.

MongoDB used as the persistence store for each domain (separate databases per domain).

# Domains / Modules:

Patient: Handles patient registration. Aggregate root: Patient. Publishes PatientRegisteredEvent.

Order: Handles order creation. Aggregate root: Order. Listens to OrderCreateRequestedEvent and publishes OrderCreatedEvent.

Payment: Handles payment collection. Aggregate root: Payment. Listens to PaymentCollectRequestedEvent and publishes PaymentCollectedEvent.

Orchestrator: Listens to domain events (PatientRegisteredEvent, OrderCreatedEvent) and triggers next workflow steps by publishing events for other domains.

# Event Flow / Workflow:

Patient registers → PatientRegisteredEvent

Orchestrator receives event → publishes OrderCreateRequestedEvent

Order domain creates order → OrderCreatedEvent

Orchestrator receives order created → publishes PaymentCollectRequestedEvent

Payment domain collects payment (via fake payment gateway) → PaymentCollectedEvent

# Tools & Frameworks:

C# / .NET 8

ASP.NET Core Web API for API layer

MongoDB.Driver for domain persistence

RabbitMQ.Client for messaging and event bus

Record types for immutable events (C# 9+ feature)

Dependency Injection & SOLID principles applied throughout

Swagger for API testing

# Benefits / Design Decisions:

Clear separation of concerns: each domain owns its data and logic.

Event-driven orchestration reduces tight coupling between domains.

Easy to scale domains independently if later moved to microservices.

Fully testable and maintainable with modular design.
