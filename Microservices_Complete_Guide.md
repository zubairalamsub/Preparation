# Microservices Architecture Complete Guide
## From Fundamentals to Production Patterns

---

## Table of Contents
1. [Microservices Fundamentals](#1-microservices-fundamentals)
2. [Architecture Patterns](#2-architecture-patterns)
3. [Service Communication](#3-service-communication)
4. [API Gateway Pattern](#4-api-gateway-pattern)
5. [Service Discovery](#5-service-discovery)
6. [Data Management](#6-data-management)
7. [Authentication & Authorization](#7-authentication--authorization)
8. [Resilience Patterns](#8-resilience-patterns)
9. [Observability & Monitoring](#9-observability--monitoring)
10. [Event-Driven Architecture](#10-event-driven-architecture)
11. [CQRS & Event Sourcing](#11-cqrs--event-sourcing)
12. [Deployment Strategies](#12-deployment-strategies)
13. [Service Mesh](#13-service-mesh)
14. [Best Practices](#14-best-practices)
15. [Common Anti-Patterns](#15-common-anti-patterns)
16. [Real-World Implementation](#16-real-world-implementation)

---

## 1. Microservices Fundamentals

### 1.1 What are Microservices?

**Microservices** are small, autonomous services that work together, each running in its own process and communicating via lightweight mechanisms.

**Key Characteristics:**
- **Single Responsibility**: Each service does one thing well
- **Autonomous**: Can be deployed independently
- **Technology Agnostic**: Different tech stacks possible
- **Decentralized Data**: Each service owns its data
- **Failure Isolated**: Failure in one doesn't crash all
- **Scalable**: Scale services independently

### 1.2 Monolith vs Microservices

```
MONOLITHIC APPLICATION
┌──────────────────────────────────┐
│         UI Layer                  │
├──────────────────────────────────┤
│         Business Logic            │
│  ┌──────┐ ┌──────┐ ┌──────┐    │
│  │Order │ │User  │ │Inv   │    │
│  └──────┘ └──────┘ └──────┘    │
├──────────────────────────────────┤
│       Single Database            │
└──────────────────────────────────┘

MICROSERVICES ARCHITECTURE
┌─────────────────────────────────┐
│         API Gateway              │
└───────────┬─────────────────────┘
            │
    ┌───────┼───────┬─────────┐
    ▼       ▼       ▼         ▼
┌──────┐ ┌─────┐ ┌──────┐ ┌──────┐
│Order │ │User │ │Inv   │ │Pay   │
│Service│ │Svc │ │Svc   │ │Svc   │
├──────┤ ├─────┤ ├──────┤ ├──────┤
│ DB   │ │ DB  │ │ DB   │ │ DB   │
└──────┘ └─────┘ └──────┘ └──────┘
```

### 1.3 When to Use Microservices

**Use Microservices When:**
- ✅ Large, complex applications
- ✅ Multiple teams working independently
- ✅ Need to scale specific components
- ✅ Different tech stacks needed
- ✅ Frequent deployments required
- ✅ Long-term project (5+ years)

**Avoid Microservices When:**
- ❌ Small team (< 5 people)
- ❌ Simple application
- ❌ Tight coupling required
- ❌ No DevOps expertise
- ❌ Starting a new project (start monolith first)

### 1.4 Microservices Challenges

```
Challenges:
1. Distributed System Complexity
   - Network latency
   - Partial failures
   - Eventual consistency

2. Data Management
   - No distributed transactions
   - Data duplication
   - Query across services

3. Testing
   - Integration testing harder
   - End-to-end testing complex

4. Deployment
   - Multiple deployments
   - Version compatibility
   - Configuration management

5. Monitoring
   - Distributed tracing needed
   - Log aggregation required
   - Many more moving parts
```

---

## 2. Architecture Patterns

### 2.1 Decomposition Patterns

#### By Business Capability
```
E-Commerce System

┌─────────────────────┐
│   API Gateway       │
└──────────┬──────────┘
           │
     ┌─────┼─────┬────────┬─────────┐
     ▼     ▼     ▼        ▼         ▼
  ┌────┐ ┌───┐ ┌────┐ ┌──────┐ ┌──────┐
  │Cart│ │Ord│ │Pay │ │Inv   │ │User  │
  │Svc │ │Svc│ │Svc │ │Svc   │ │Svc   │
  └────┘ └───┘ └────┘ └──────┘ └──────┘
```

```csharp
// Example: Order Service
namespace OrderService
{
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IInventoryClient _inventoryClient;
        private readonly IPaymentClient _paymentClient;
        private readonly IMessageBus _messageBus;

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            // 1. Check inventory
            var inventoryAvailable = await _inventoryClient
                .CheckAvailability(dto.ProductId, dto.Quantity);

            if (!inventoryAvailable)
                return BadRequest("Out of stock");

            // 2. Process payment
            var paymentResult = await _paymentClient
                .ProcessPayment(dto.PaymentInfo);

            if (!paymentResult.Success)
                return BadRequest("Payment failed");

            // 3. Create order
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Status = OrderStatus.Pending
            };

            await _orderRepo.AddAsync(order);

            // 4. Publish event
            await _messageBus.Publish(new OrderCreatedEvent
            {
                OrderId = order.Id,
                ProductId = order.ProductId,
                Quantity = order.Quantity
            });

            return Ok(order);
        }
    }
}
```

#### By Subdomain (DDD)
```
Domain-Driven Design Decomposition

Core Domain:
  - Order Management
  - Pricing Engine

Supporting Domains:
  - Inventory Management
  - User Management

Generic Domains:
  - Notification
  - Authentication
```

### 2.2 Database per Service Pattern

```csharp
// Each service has its own database

// Order Service - SQL Server
public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(
            "Server=order-db;Database=Orders;User=sa;Password=xxx");
    }
}

// Inventory Service - MongoDB
public class InventoryRepository
{
    private readonly IMongoDatabase _database;

    public InventoryRepository()
    {
        var client = new MongoClient("mongodb://inventory-db:27017");
        _database = client.GetDatabase("inventory");
    }

    public async Task<Product> GetProduct(string productId)
    {
        var collection = _database.GetCollection<Product>("products");
        return await collection.Find(p => p.Id == productId).FirstOrDefaultAsync();
    }
}

// User Service - PostgreSQL
public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(
            "Host=user-db;Database=users;Username=postgres;Password=xxx");
    }
}
```

### 2.3 Strangler Fig Pattern (Migration)

```
Migrate from Monolith to Microservices

Phase 1: Add API Gateway
┌──────────┐     ┌──────────────┐
│  Client  │────▶│ API Gateway  │
└──────────┘     └──────┬───────┘
                        │
                        ▼
                 ┌──────────────┐
                 │   Monolith   │
                 └──────────────┘

Phase 2: Extract First Service
┌──────────┐     ┌──────────────┐
│  Client  │────▶│ API Gateway  │
└──────────┘     └──────┬───────┘
                        │
                 ┌──────┴──────┐
                 ▼             ▼
          ┌──────────┐   ┌──────────┐
          │  Order   │   │Monolith  │
          │ Service  │   │ (minus   │
          └──────────┘   │ orders)  │
                         └──────────┘

Phase 3: Continue Extracting
┌──────────┐     ┌──────────────┐
│  Client  │────▶│ API Gateway  │
└──────────┘     └──────┬───────┘
                        │
           ┌────────────┼────────────┐
           ▼            ▼            ▼
    ┌──────────┐ ┌──────────┐ ┌──────────┐
    │  Order   │ │   User   │ │Inventory │
    │ Service  │ │ Service  │ │ Service  │
    └──────────┘ └──────────┘ └──────────┘
```

```csharp
// API Gateway routes requests
public class OrderProxy
{
    private readonly HttpClient _newOrderService;
    private readonly HttpClient _legacyMonolith;

    public async Task<Order> GetOrder(int orderId)
    {
        // Check if order is in new service
        try
        {
            var response = await _newOrderService
                .GetAsync($"/orders/{orderId}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Order>();
        }
        catch { }

        // Fallback to monolith
        var legacyResponse = await _legacyMonolith
            .GetAsync($"/api/orders/{orderId}");

        return await legacyResponse.Content.ReadAsAsync<Order>();
    }
}
```

---

## 3. Service Communication

### 3.1 Synchronous Communication (REST)

```csharp
// Order Service calls Inventory Service

public interface IInventoryClient
{
    Task<bool> CheckAvailability(string productId, int quantity);
    Task ReserveStock(string productId, int quantity);
}

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _httpClient;

    public InventoryClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CheckAvailability(string productId, int quantity)
    {
        var response = await _httpClient.GetAsync(
            $"/api/inventory/{productId}/availability?quantity={quantity}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Failed to check inventory");

        var result = await response.Content.ReadAsAsync<AvailabilityResponse>();
        return result.Available;
    }

    public async Task ReserveStock(string productId, int quantity)
    {
        var request = new ReserveStockRequest
        {
            ProductId = productId,
            Quantity = quantity
        };

        var response = await _httpClient.PostAsJsonAsync(
            "/api/inventory/reserve", request);

        response.EnsureSuccessStatusCode();
    }
}

// Register in Startup.cs
services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
{
    client.BaseAddress = new Uri("http://inventory-service");
    client.Timeout = TimeSpan.FromSeconds(30);
});
```

### 3.2 Asynchronous Communication (Messaging)

```csharp
// Using RabbitMQ

// Publisher (Order Service)
public class OrderService
{
    private readonly IMessageBus _messageBus;

    public async Task CreateOrder(Order order)
    {
        // Save order
        await _orderRepository.AddAsync(order);

        // Publish event
        await _messageBus.Publish(new OrderCreatedEvent
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            Items = order.Items,
            TotalAmount = order.TotalAmount,
            CreatedAt = DateTime.UtcNow
        });
    }
}

// Subscriber (Inventory Service)
public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IInventoryRepository _inventoryRepo;

    public async Task Handle(OrderCreatedEvent @event)
    {
        foreach (var item in @event.Items)
        {
            await _inventoryRepo.DecrementStock(
                item.ProductId,
                item.Quantity);
        }
    }
}

// Subscriber (Notification Service)
public class OrderNotificationHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IEmailService _emailService;

    public async Task Handle(OrderCreatedEvent @event)
    {
        await _emailService.SendOrderConfirmation(
            @event.CustomerId,
            @event.OrderId);
    }
}
```

### 3.3 gRPC Communication

```protobuf
// inventory.proto
syntax = "proto3";

package inventory;

service InventoryService {
  rpc CheckAvailability (AvailabilityRequest) returns (AvailabilityResponse);
  rpc ReserveStock (ReserveRequest) returns (ReserveResponse);
}

message AvailabilityRequest {
  string product_id = 1;
  int32 quantity = 2;
}

message AvailabilityResponse {
  bool available = 1;
  int32 current_stock = 2;
}

message ReserveRequest {
  string product_id = 1;
  int32 quantity = 2;
  string order_id = 3;
}

message ReserveResponse {
  bool success = 1;
  string message = 2;
}
```

```csharp
// gRPC Server (Inventory Service)
public class InventoryServiceImpl : InventoryService.InventoryServiceBase
{
    private readonly IInventoryRepository _repository;

    public override async Task<AvailabilityResponse> CheckAvailability(
        AvailabilityRequest request,
        ServerCallContext context)
    {
        var product = await _repository.GetProduct(request.ProductId);

        return new AvailabilityResponse
        {
            Available = product.Stock >= request.Quantity,
            CurrentStock = product.Stock
        };
    }

    public override async Task<ReserveResponse> ReserveStock(
        ReserveRequest request,
        ServerCallContext context)
    {
        try
        {
            await _repository.ReserveStock(
                request.ProductId,
                request.Quantity,
                request.OrderId);

            return new ReserveResponse
            {
                Success = true,
                Message = "Stock reserved successfully"
            };
        }
        catch (Exception ex)
        {
            return new ReserveResponse
            {
                Success = false,
                Message = ex.Message
            };
        }
    }
}

// gRPC Client (Order Service)
public class InventoryGrpcClient
{
    private readonly InventoryService.InventoryServiceClient _client;

    public InventoryGrpcClient(InventoryService.InventoryServiceClient client)
    {
        _client = client;
    }

    public async Task<bool> CheckAvailability(string productId, int quantity)
    {
        var request = new AvailabilityRequest
        {
            ProductId = productId,
            Quantity = quantity
        };

        var response = await _client.CheckAvailabilityAsync(request);
        return response.Available;
    }
}
```

---

## 4. API Gateway Pattern

### 4.1 Ocelot API Gateway (.NET)

```json
// ocelot.json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/orders/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "order-service",
          "Port": 80
        }
      ],
      "UpstreamPathTemplate": "/orders/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer"
      },
      "RateLimitOptions": {
        "EnableRateLimiting": true,
        "Period": "1m",
        "Limit": 100
      }
    },
    {
      "DownstreamPathTemplate": "/api/products/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "inventory-service",
          "Port": 80
        }
      ],
      "UpstreamPathTemplate": "/products/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "FileCacheOptions": {
        "TtlSeconds": 60
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://api.example.com",
    "RateLimitOptions": {
      "DisableRateLimitHeaders": false,
      "QuotaExceededMessage": "Rate limit exceeded"
    }
  }
}
```

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

app.UseOcelot().Wait();
app.Run();
```

### 4.2 Backend for Frontend (BFF) Pattern

```csharp
// Mobile BFF
public class MobileBFFController : ControllerBase
{
    private readonly IOrderClient _orderClient;
    private readonly IInventoryClient _inventoryClient;
    private readonly IUserClient _userClient;

    [HttpGet("home")]
    public async Task<MobileHomeDto> GetHome()
    {
        // Aggregate data from multiple services
        // Optimized for mobile bandwidth

        var userId = User.GetUserId();

        var tasks = new[]
        {
            _orderClient.GetRecentOrders(userId, limit: 5),
            _inventoryClient.GetFeaturedProducts(limit: 10),
            _userClient.GetUserProfile(userId)
        };

        await Task.WhenAll(tasks);

        return new MobileHomeDto
        {
            User = tasks[2].Result,
            RecentOrders = tasks[0].Result
                .Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    Status = o.Status,
                    Total = o.TotalAmount
                })
                .ToList(),
            FeaturedProducts = tasks[1].Result
                .Select(p => new ProductCardDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ThumbnailUrl // Smaller image
                })
                .ToList()
        };
    }
}

// Web BFF
public class WebBFFController : ControllerBase
{
    [HttpGet("home")]
    public async Task<WebHomeDto> GetHome()
    {
        // Aggregate data from multiple services
        // More detailed data for web

        var userId = User.GetUserId();

        return new WebHomeDto
        {
            User = await _userClient.GetUserProfile(userId),
            RecentOrders = await _orderClient.GetRecentOrders(userId, limit: 20),
            FeaturedProducts = await _inventoryClient.GetFeaturedProducts(limit: 50),
            Recommendations = await _recommendationClient.GetRecommendations(userId)
        };
    }
}
```

---

## 5. Service Discovery

### 5.1 Client-Side Discovery (Consul)

```csharp
// Service Registration
public class ConsulServiceRegistry
{
    private readonly IConsulClient _consulClient;

    public async Task RegisterService(ServiceRegistration registration)
    {
        var agentServiceRegistration = new AgentServiceRegistration
        {
            ID = $"{registration.ServiceName}-{registration.ServiceId}",
            Name = registration.ServiceName,
            Address = registration.Address,
            Port = registration.Port,
            Tags = registration.Tags,
            Check = new AgentServiceCheck
            {
                HTTP = $"http://{registration.Address}:{registration.Port}/health",
                Interval = TimeSpan.FromSeconds(10),
                Timeout = TimeSpan.FromSeconds(5),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
            }
        };

        await _consulClient.Agent.ServiceRegister(agentServiceRegistration);
    }

    public async Task DeregisterService(string serviceId)
    {
        await _consulClient.Agent.ServiceDeregister(serviceId);
    }
}

// Service Discovery
public class ConsulServiceDiscovery
{
    private readonly IConsulClient _consulClient;

    public async Task<ServiceInstance> DiscoverService(string serviceName)
    {
        var services = await _consulClient.Health.Service(serviceName, null, true);

        if (!services.Response.Any())
            throw new Exception($"No healthy instances of {serviceName} found");

        // Simple round-robin
        var random = new Random();
        var service = services.Response[random.Next(services.Response.Length)];

        return new ServiceInstance
        {
            Address = service.Service.Address,
            Port = service.Service.Port
        };
    }
}

// HTTP Client with Service Discovery
public class ServiceDiscoveryHttpClient
{
    private readonly ConsulServiceDiscovery _discovery;
    private readonly HttpClient _httpClient;

    public async Task<HttpResponseMessage> Get(string serviceName, string path)
    {
        var instance = await _discovery.DiscoverService(serviceName);
        var url = $"http://{instance.Address}:{instance.Port}{path}";

        return await _httpClient.GetAsync(url);
    }
}
```

### 5.2 Server-Side Discovery (Kubernetes)

```yaml
# Kubernetes Service Discovery
apiVersion: v1
kind: Service
metadata:
  name: order-service
spec:
  selector:
    app: order-service
  ports:
    - protocol: TCP
      port: 80
      targetPort: 8080
  type: ClusterIP

---
apiVersion: apps/v1
kind: Deployment
metadata:
  name: order-service
spec:
  replicas: 3
  selector:
    matchLabels:
      app: order-service
  template:
    metadata:
      labels:
        app: order-service
    spec:
      containers:
      - name: order-service
        image: myregistry/order-service:latest
        ports:
        - containerPort: 8080
        env:
        - name: INVENTORY_SERVICE_URL
          value: "http://inventory-service"
        - name: USER_SERVICE_URL
          value: "http://user-service"
```

```csharp
// In Kubernetes, use service names directly
public class InventoryClient
{
    private readonly HttpClient _httpClient;

    public InventoryClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}

// Startup.cs
services.AddHttpClient<IInventoryClient, InventoryClient>(client =>
{
    // Kubernetes DNS will resolve "inventory-service"
    client.BaseAddress = new Uri("http://inventory-service");
});
```

---

## 6. Data Management

### 6.1 Saga Pattern (Distributed Transactions)

```csharp
// Orchestration-based Saga
public class OrderSagaOrchestrator
{
    private readonly IOrderRepository _orderRepo;
    private readonly IInventoryClient _inventoryClient;
    private readonly IPaymentClient _paymentClient;
    private readonly IShippingClient _shippingClient;

    public async Task<OrderResult> CreateOrder(CreateOrderCommand command)
    {
        var order = new Order { Status = OrderStatus.Pending };
        await _orderRepo.SaveAsync(order);

        try
        {
            // Step 1: Reserve inventory
            await _inventoryClient.ReserveStock(
                command.ProductId,
                command.Quantity,
                order.Id);

            order.Status = OrderStatus.InventoryReserved;
            await _orderRepo.SaveAsync(order);

            // Step 2: Process payment
            var paymentResult = await _paymentClient.ProcessPayment(
                order.Id,
                command.PaymentDetails);

            if (!paymentResult.Success)
                throw new PaymentFailedException();

            order.Status = OrderStatus.PaymentProcessed;
            await _orderRepo.SaveAsync(order);

            // Step 3: Arrange shipping
            await _shippingClient.CreateShipment(order.Id);

            order.Status = OrderStatus.Confirmed;
            await _orderRepo.SaveAsync(order);

            return OrderResult.Success(order);
        }
        catch (Exception ex)
        {
            // Compensating transactions
            await CompensateOrder(order, ex);
            return OrderResult.Failure(ex.Message);
        }
    }

    private async Task CompensateOrder(Order order, Exception ex)
    {
        if (order.Status >= OrderStatus.PaymentProcessed)
        {
            // Refund payment
            await _paymentClient.RefundPayment(order.Id);
        }

        if (order.Status >= OrderStatus.InventoryReserved)
        {
            // Release inventory
            await _inventoryClient.ReleaseStock(order.Id);
        }

        order.Status = OrderStatus.Cancelled;
        order.CancellationReason = ex.Message;
        await _orderRepo.SaveAsync(order);
    }
}

// Choreography-based Saga (Event-Driven)
public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IMessageBus _messageBus;

    public async Task Handle(OrderCreatedEvent @event)
    {
        try
        {
            // Reserve inventory
            await _inventoryRepo.ReserveStock(
                @event.ProductId,
                @event.Quantity,
                @event.OrderId);

            // Publish success event
            await _messageBus.Publish(new InventoryReservedEvent
            {
                OrderId = @event.OrderId,
                ProductId = @event.ProductId,
                Quantity = @event.Quantity
            });
        }
        catch
        {
            // Publish failure event
            await _messageBus.Publish(new InventoryReservationFailedEvent
            {
                OrderId = @event.OrderId,
                Reason = "Insufficient stock"
            });
        }
    }
}

public class InventoryReservedEventHandler : IEventHandler<InventoryReservedEvent>
{
    private readonly IPaymentService _paymentService;
    private readonly IMessageBus _messageBus;

    public async Task Handle(InventoryReservedEvent @event)
    {
        var result = await _paymentService.ProcessPayment(@event.OrderId);

        if (result.Success)
        {
            await _messageBus.Publish(new PaymentProcessedEvent
            {
                OrderId = @event.OrderId
            });
        }
        else
        {
            // Trigger compensation
            await _messageBus.Publish(new PaymentFailedEvent
            {
                OrderId = @event.OrderId
            });
        }
    }
}
```

### 6.2 Outbox Pattern

```csharp
// Ensures message publishing is atomic with database changes

public class OrderService
{
    private readonly OrderDbContext _dbContext;

    public async Task CreateOrder(Order order)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Save order
            _dbContext.Orders.Add(order);

            // Save outbox message (same transaction)
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                EventType = nameof(OrderCreatedEvent),
                Payload = JsonSerializer.Serialize(new OrderCreatedEvent
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    Items = order.Items
                }),
                CreatedAt = DateTime.UtcNow,
                ProcessedAt = null
            };

            _dbContext.OutboxMessages.Add(outboxMessage);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

// Background service to process outbox
public class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageBus _messageBus;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            // Get unprocessed messages
            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedAt == null)
                .OrderBy(m => m.CreatedAt)
                .Take(100)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    // Publish message
                    await _messageBus.PublishRaw(
                        message.EventType,
                        message.Payload);

                    // Mark as processed
                    message.ProcessedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    // Log error, will retry next iteration
                    message.RetryCount++;
                    message.LastError = ex.Message;
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
```

---

## 7. Authentication & Authorization

### 7.1 JWT Token-Based Auth

```csharp
// Auth Service
public class AuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public async Task<AuthResult> Login(LoginRequest request)
    {
        var user = await _userRepo.GetByEmail(request.Email);

        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            return AuthResult.Failure("Invalid credentials");

        var token = GenerateJwtToken(user);

        return AuthResult.Success(token, user);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("tenant_id", user.TenantId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

// API Gateway validates token
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy("CanManageOrders", policy =>
                policy.RequireClaim("permissions", "orders.write"));
        });
    }
}

// Protected Controller
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // ...
    }

    [HttpPost]
    [Authorize(Policy = "CanManageOrders")]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        // ...
    }
}
```

### 7.2 OAuth 2.0 / OpenID Connect

```csharp
// Using IdentityServer4 or Duende IdentityServer

// Identity Server Configuration
public class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("orders.read", "Read orders"),
            new ApiScope("orders.write", "Create and modify orders"),
            new ApiScope("inventory.read", "Read inventory"),
            new ApiScope("inventory.write", "Modify inventory")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // Mobile app
            new Client
            {
                ClientId = "mobile-app",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                RedirectUris = { "myapp://callback" },
                AllowedScopes = {
                    "openid",
                    "profile",
                    "orders.read",
                    "orders.write"
                }
            },

            // Web app
            new Client
            {
                ClientId = "web-app",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://localhost:5001/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                AllowedScopes = {
                    "openid",
                    "profile",
                    "email",
                    "orders.read",
                    "orders.write",
                    "inventory.read"
                }
            },

            // Service-to-service
            new Client
            {
                ClientId = "order-service",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {
                    "inventory.read",
                    "inventory.write"
                }
            }
        };
}

// Microservice validates token from Identity Server
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://identity-server";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanReadOrders", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "orders.read");
            });
        });
    }
}
```

---

**[Continuing in next part with Resilience Patterns, Observability, Event-Driven Architecture, etc...]**

## Quick Reference

### Communication Patterns
- **Synchronous**: REST, gRPC (immediate response needed)
- **Asynchronous**: Message queues (fire-and-forget, events)

### Data Patterns
- **Database per Service**: Each service owns its data
- **Saga**: Distributed transactions across services
- **Outbox**: Reliable event publishing

### Resilience Patterns
- **Circuit Breaker**: Prevent cascading failures
- **Retry**: Handle transient failures
- **Timeout**: Prevent indefinite waits
- **Bulkhead**: Isolate resources

### Decomposition Strategies
1. By business capability
2. By subdomain (DDD)
3. By user/client type
4. Self-contained services
