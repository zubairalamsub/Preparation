# Advanced .NET Interview Preparation Guide

## Table of Contents
1. [Advanced C# Concepts](#advanced-c-concepts)
2. [ASP.NET Core](#aspnet-core)
3. [Entity Framework Core](#entity-framework-core)
4. [Design Patterns & Architecture](#design-patterns--architecture)
5. [Asynchronous Programming](#asynchronous-programming)
6. [Memory Management & Performance](#memory-management--performance)
7. [Security](#security)
8. [Microservices & Distributed Systems](#microservices--distributed-systems)
9. [Testing](#testing)
10. [Advanced Scenarios & Problem Solving](#advanced-scenarios--problem-solving)

---

## Advanced C# Concepts

### 1. **Value Types vs Reference Types**
**Q: Explain the difference and when to use struct vs class?**

**Answer:**
- **Value Types** (struct): Stored on stack (if local variable), copied by value, no garbage collection overhead
- **Reference Types** (class): Stored on heap, copied by reference, managed by GC

**When to use struct:**
- Small data structures (< 16 bytes recommended)
- Immutable types
- No inheritance needed
- Example: Point, DateTime, decimal

```csharp
// Good struct usage
public readonly struct Point
{
    public readonly double X;
    public readonly double Y;

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }
}
```

### 2. **Delegates, Events, and Action/Func**
**Q: Explain the difference between delegates, events, Action, and Func?**

**Answer:**
- **Delegate**: Type-safe function pointer
- **Event**: Special delegate with restricted access (can only invoke from declaring class)
- **Action**: Built-in delegate for methods with no return value (up to 16 parameters)
- **Func**: Built-in delegate for methods with return value (last type parameter is return type)

```csharp
// Delegate
public delegate void LogHandler(string message);

// Event
public event LogHandler OnLog;

// Action (void return)
Action<string> log = (msg) => Console.WriteLine(msg);

// Func (returns value)
Func<int, int, int> add = (a, b) => a + b;
```

### 3. **Expression Trees**
**Q: What are expression trees and when are they used?**

**Answer:**
Expression trees represent code as data structures. Primarily used by:
- LINQ providers (Entity Framework translates to SQL)
- Dynamic code generation
- Creating queries at runtime

```csharp
// Lambda expression
Func<int, bool> funcDelegate = num => num > 5;

// Expression tree
Expression<Func<int, bool>> expression = num => num > 5;

// Analyzing the expression
var parameter = expression.Parameters[0]; // num
var body = expression.Body; // num > 5
```

### 4. **Covariance and Contravariance**
**Q: Explain covariance and contravariance with examples?**

**Answer:**
- **Covariance (out)**: Allows returning more derived types
- **Contravariance (in)**: Allows accepting less derived types

```csharp
// Covariance (out) - IEnumerable<out T>
IEnumerable<string> strings = new List<string>();
IEnumerable<object> objects = strings; // Valid - covariance

// Contravariance (in) - Action<in T>
Action<object> actObject = (obj) => Console.WriteLine(obj);
Action<string> actString = actObject; // Valid - contravariance

// Custom interface
public interface IRepository<out T> // Covariant
{
    T Get(int id);
}
```

### 5. **Span<T> and Memory<T>**
**Q: What are Span<T> and Memory<T>? When should they be used?**

**Answer:**
- **Span<T>**: Stack-only value type for working with contiguous memory regions
- **Memory<T>**: Similar to Span but can be used on heap (async scenarios)

**Benefits:**
- Zero allocation memory access
- High-performance scenarios
- String manipulation without creating new strings

```csharp
// Span usage
string text = "Hello World";
ReadOnlySpan<char> span = text.AsSpan(0, 5);
Console.WriteLine(span.ToString()); // "Hello"

// Memory usage with async
async Task ProcessAsync(Memory<byte> buffer)
{
    await Task.Delay(100);
    Span<byte> span = buffer.Span; // Get span when needed
}
```

### 6. **Record Types (C# 9+)**
**Q: What are record types and when should you use them?**

**Answer:**
Records are reference types designed for immutable data with value-based equality.

```csharp
// Record definition
public record Person(string FirstName, string LastName, int Age);

// Usage
var person1 = new Person("John", "Doe", 30);
var person2 = person1 with { Age = 31 }; // Non-destructive mutation

// Value-based equality
var person3 = new Person("John", "Doe", 30);
Console.WriteLine(person1 == person3); // True
```

### 7. **Pattern Matching**
**Q: Demonstrate advanced pattern matching in C#?**

```csharp
// Type pattern with property pattern
public static string Describe(object obj) => obj switch
{
    null => "null",
    int i => $"Integer: {i}",
    string { Length: 0 } => "Empty string",
    string s => $"String: {s}",
    Person { Age: > 18, FirstName: var name } => $"Adult named {name}",
    Person p => $"Minor: {p.FirstName}",
    _ => "Unknown"
};

// Relational and logical patterns
public static string GetPriceCategory(decimal price) => price switch
{
    < 0 => throw new ArgumentException("Price cannot be negative"),
    >= 0 and < 50 => "Budget",
    >= 50 and < 200 => "Mid-range",
    >= 200 => "Premium"
};
```

---

## Real-Life Example: E-Commerce Order Processing System

Let's build a complete example demonstrating multiple C# concepts in a real-world e-commerce scenario.

```csharp
// ==================== DOMAIN MODELS ====================

// Using Records for immutable DTOs (C# 9+)
public record OrderDto(int OrderId, string CustomerName, decimal Total, OrderStatus Status);

public record ProductDto(int Id, string Name, decimal Price, int Stock);

// Using Structs for small value types
public readonly struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative");

        Amount = amount;
        Currency = currency;
    }

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Cannot add different currencies");

        return new Money(a.Amount + b.Amount, a.Currency);
    }
}

// ==================== EVENT-DRIVEN ARCHITECTURE ====================

// Using Delegates and Events
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public OrderStatus Status { get; private set; }

    // Event declaration
    public event EventHandler<OrderStatusChangedEventArgs> StatusChanged;

    public void UpdateStatus(OrderStatus newStatus, string reason)
    {
        var oldStatus = Status;
        Status = newStatus;

        // Raise event
        OnStatusChanged(new OrderStatusChangedEventArgs
        {
            OrderId = Id,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            Reason = reason,
            Timestamp = DateTime.UtcNow
        });
    }

    protected virtual void OnStatusChanged(OrderStatusChangedEventArgs e)
    {
        StatusChanged?.Invoke(this, e);
    }
}

public class OrderStatusChangedEventArgs : EventArgs
{
    public int OrderId { get; set; }
    public OrderStatus OldStatus { get; set; }
    public OrderStatus NewStatus { get; set; }
    public string Reason { get; set; }
    public DateTime Timestamp { get; set; }
}

// Event subscribers
public class EmailNotificationService
{
    public void SubscribeToOrder(Order order)
    {
        order.StatusChanged += OnOrderStatusChanged;
    }

    private void OnOrderStatusChanged(object sender, OrderStatusChangedEventArgs e)
    {
        Console.WriteLine($"Sending email: Order {e.OrderId} status changed from {e.OldStatus} to {e.NewStatus}");
        // Send email logic
    }
}

// ==================== USING SPAN<T> FOR PERFORMANCE ====================

public class OrderNumberParser
{
    // Real-life scenario: Parsing order numbers efficiently
    // Format: "ORD-2024-001234-US"

    public (int year, int orderNum, string country) ParseOrderNumber(string orderNumber)
    {
        ReadOnlySpan<char> span = orderNumber.AsSpan();

        // Split without allocating new strings
        var firstDash = span.IndexOf('-');
        var secondDash = span.Slice(firstDash + 1).IndexOf('-') + firstDash + 1;
        var thirdDash = span.Slice(secondDash + 1).IndexOf('-') + secondDash + 1;

        var yearSpan = span.Slice(firstDash + 1, secondDash - firstDash - 1);
        var orderNumSpan = span.Slice(secondDash + 1, thirdDash - secondDash - 1);
        var countrySpan = span.Slice(thirdDash + 1);

        var year = int.Parse(yearSpan);
        var orderNum = int.Parse(orderNumSpan);
        var country = countrySpan.ToString();

        return (year, orderNum, country);
    }
}

// ==================== EXPRESSION TREES - DYNAMIC FILTERING ====================

public class OrderSearchService
{
    private readonly IQueryable<Order> _orders;

    // Build dynamic queries at runtime using expression trees
    public IQueryable<Order> SearchOrders(OrderSearchCriteria criteria)
    {
        var query = _orders;

        // Build dynamic where clause
        if (!string.IsNullOrEmpty(criteria.CustomerName))
        {
            Expression<Func<Order, bool>> filter = o => o.CustomerName.Contains(criteria.CustomerName);
            query = query.Where(filter);
        }

        if (criteria.MinAmount.HasValue)
        {
            Expression<Func<Order, bool>> filter = o => o.Items.Sum(i => i.Price * i.Quantity) >= criteria.MinAmount.Value;
            query = query.Where(filter);
        }

        if (criteria.Status.HasValue)
        {
            Expression<Func<Order, bool>> filter = o => o.Status == criteria.Status.Value;
            query = query.Where(filter);
        }

        return query;
    }

    // Advanced: Building complex dynamic queries
    public IQueryable<Order> BuildDynamicQuery(Dictionary<string, object> filters)
    {
        var parameter = Expression.Parameter(typeof(Order), "o");
        Expression combinedExpression = Expression.Constant(true);

        foreach (var filter in filters)
        {
            var property = Expression.Property(parameter, filter.Key);
            var constant = Expression.Constant(filter.Value);
            var equality = Expression.Equal(property, constant);

            combinedExpression = Expression.AndAlso(combinedExpression, equality);
        }

        var lambda = Expression.Lambda<Func<Order, bool>>(combinedExpression, parameter);
        return _orders.Where(lambda);
    }
}

// ==================== PATTERN MATCHING - REAL SCENARIOS ====================

public class DiscountCalculator
{
    // Real-life: Calculate discounts based on customer type and order
    public decimal CalculateDiscount(Customer customer, Order order) => customer switch
    {
        // Premium customers with large orders
        { Type: CustomerType.Premium, TotalPurchases: > 10000 } when order.Total > 500
            => order.Total * 0.20m,

        // Premium customers
        { Type: CustomerType.Premium } => order.Total * 0.15m,

        // Regular customers with first purchase
        { Type: CustomerType.Regular, TotalOrders: 0 } => order.Total * 0.10m,

        // Regular customers with loyalty points
        { Type: CustomerType.Regular, LoyaltyPoints: > 1000 } => order.Total * 0.08m,

        // Regular customers
        { Type: CustomerType.Regular } => order.Total * 0.05m,

        // New customers
        { Type: CustomerType.New } => 25.00m, // Flat discount

        _ => 0m
    };

    // Pattern matching for payment processing
    public string ProcessPayment(PaymentMethod payment, decimal amount) => payment switch
    {
        CreditCard { IsExpired: true } => "Card expired",
        CreditCard { Balance: var balance } when balance < amount => "Insufficient funds",
        CreditCard card => ProcessCreditCard(card, amount),

        PayPal { IsVerified: false } => "PayPal account not verified",
        PayPal paypal => ProcessPayPal(paypal, amount),

        BankTransfer { AccountStatus: AccountStatus.Frozen } => "Account frozen",
        BankTransfer bank => ProcessBankTransfer(bank, amount),

        _ => "Payment method not supported"
    };

    private string ProcessCreditCard(CreditCard card, decimal amount) => "Credit card processed";
    private string ProcessPayPal(PayPal paypal, decimal amount) => "PayPal processed";
    private string ProcessBankTransfer(BankTransfer bank, decimal amount) => "Bank transfer processed";
}

// ==================== COVARIANCE/CONTRAVARIANCE - REAL USE ====================

// Repository with covariance
public interface IReadRepository<out TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
}

// Usage of covariance
public class ReportService
{
    public async Task GenerateReport(IReadRepository<Order> orderRepo)
    {
        // Can assign IReadRepository<Order> to IReadRepository<object> due to covariance
        IReadRepository<object> baseRepo = orderRepo;

        var orders = await orderRepo.GetAllAsync();
        // Generate report
    }
}

// Contravariance example: Validators
public interface IValidator<in T>
{
    bool Validate(T item);
}

public class EntityValidator : IValidator<object>
{
    public bool Validate(object item)
    {
        return item != null;
    }
}

public class OrderValidationService
{
    public void ValidateOrder(Order order)
    {
        // Can use object validator for Order due to contravariance
        IValidator<Order> validator = new EntityValidator();
        var isValid = validator.Validate(order);
    }
}

// ==================== COMPLETE EXAMPLE USAGE ====================

public class ECommerceDemo
{
    public async Task DemoAsync()
    {
        // Create order
        var order = new Order
        {
            Id = 12345,
            CustomerName = "John Doe",
            Items = new List<OrderItem>
            {
                new() { ProductId = 1, ProductName = "Laptop", Price = 999.99m, Quantity = 1 },
                new() { ProductId = 2, ProductName = "Mouse", Price = 29.99m, Quantity = 2 }
            }
        };

        // Subscribe to events
        var emailService = new EmailNotificationService();
        emailService.SubscribeToOrder(order);

        // Update order status (triggers event)
        order.UpdateStatus(OrderStatus.Processing, "Payment confirmed");
        order.UpdateStatus(OrderStatus.Shipped, "Package dispatched");

        // Use Span<T> for efficient parsing
        var parser = new OrderNumberParser();
        var (year, orderNum, country) = parser.ParseOrderNumber("ORD-2024-001234-US");
        Console.WriteLine($"Year: {year}, Order: {orderNum}, Country: {country}");

        // Calculate discount using pattern matching
        var customer = new Customer
        {
            Type = CustomerType.Premium,
            TotalPurchases = 15000,
            LoyaltyPoints = 2500
        };

        var calculator = new DiscountCalculator();
        var discount = calculator.CalculateDiscount(customer, order);
        Console.WriteLine($"Discount: ${discount:F2}");

        // Use Money struct
        var price1 = new Money(999.99m);
        var price2 = new Money(59.98m);
        var total = price1 + price2;
        Console.WriteLine($"Total: {total.Amount} {total.Currency}");
    }
}

// Supporting classes
public enum OrderStatus { Pending, Processing, Shipped, Delivered, Cancelled }
public enum CustomerType { New, Regular, Premium }
public enum AccountStatus { Active, Frozen, Suspended }

public class Customer
{
    public CustomerType Type { get; set; }
    public decimal TotalPurchases { get; set; }
    public int LoyaltyPoints { get; set; }
    public int TotalOrders { get; set; }
}

public class OrderItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class OrderSearchCriteria
{
    public string CustomerName { get; set; }
    public decimal? MinAmount { get; set; }
    public OrderStatus? Status { get; set; }
}

public abstract class PaymentMethod { }
public class CreditCard : PaymentMethod
{
    public bool IsExpired { get; set; }
    public decimal Balance { get; set; }
}
public class PayPal : PaymentMethod
{
    public bool IsVerified { get; set; }
}
public class BankTransfer : PaymentMethod
{
    public AccountStatus AccountStatus { get; set; }
}
```

**Key Real-Life Takeaways:**
1. **Records**: Perfect for DTOs in API responses
2. **Structs**: Use for small value types like Money, Point, Color
3. **Events**: Implement notification systems, audit logs
4. **Span<T>**: High-performance string parsing, data processing
5. **Expression Trees**: Dynamic LINQ queries, ORM implementations
6. **Pattern Matching**: Complex business rules, payment processing
7. **Covariance/Contravariance**: Generic repositories, validation systems

---

## ASP.NET Core

### 1. **Middleware Pipeline**
**Q: Explain the ASP.NET Core middleware pipeline and how to create custom middleware?**

**Answer:**
Middleware components execute in order, forming a pipeline. Each can process request/response or pass to next.

```csharp
// Custom middleware
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

        await _next(context); // Call next middleware

        _logger.LogInformation($"Response: {context.Response.StatusCode}");
    }
}

// Registration
app.UseMiddleware<RequestLoggingMiddleware>();
```

### 2. **Dependency Injection Lifetimes**
**Q: Explain the three DI lifetimes in ASP.NET Core?**

**Answer:**
- **Transient**: New instance every time requested
- **Scoped**: One instance per HTTP request
- **Singleton**: Single instance for application lifetime

```csharp
// Registration
services.AddTransient<IEmailService, EmailService>(); // New each time
services.AddScoped<IUserContext, UserContext>(); // Per request
services.AddSingleton<ICache, MemoryCache>(); // App lifetime

// Pitfall: Never inject scoped/transient into singleton!
```

### 3. **Filters vs Middleware**
**Q: What's the difference between Filters and Middleware?**

**Answer:**
- **Middleware**: Works at HTTP request level, executes for all requests
- **Filters**: MVC-specific, works at action level, has access to MVC context

**Filter Types:**
1. Authorization filters
2. Resource filters
3. Action filters
4. Exception filters
5. Result filters

```csharp
// Custom Action Filter
public class LogActionFilter : IAsyncActionFilter
{
    private readonly ILogger<LogActionFilter> _logger;

    public LogActionFilter(ILogger<LogActionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        _logger.LogInformation($"Before action: {context.ActionDescriptor.DisplayName}");

        var resultContext = await next();

        if (resultContext.Exception != null)
            _logger.LogError(resultContext.Exception, "Action failed");
        else
            _logger.LogInformation("Action succeeded");
    }
}

// Apply to controller or action
[ServiceFilter(typeof(LogActionFilter))]
public class HomeController : Controller
{
    // ...
}
```

### 4. **Model Binding and Validation**
**Q: How does model binding work? How to implement custom validation?**

```csharp
// Custom validation attribute
public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime date)
        {
            if (date > DateTime.Now)
                return ValidationResult.Success;

            return new ValidationResult("Date must be in the future");
        }

        return new ValidationResult("Invalid date");
    }
}

// Model with validation
public class AppointmentRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    [FutureDate]
    public DateTime AppointmentDate { get; set; }

    [EmailAddress]
    public string Email { get; set; }
}

// Custom model binder
public class CustomModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        // Custom binding logic
        var value = valueProviderResult.FirstValue;
        // Process value and set result

        bindingContext.Result = ModelBindingResult.Success(/* your model */);
        return Task.CompletedTask;
    }
}
```

### 5. **Authentication vs Authorization**
**Q: Explain the difference and how to implement both in ASP.NET Core?**

**Answer:**
- **Authentication**: Who are you? (Identity verification)
- **Authorization**: What can you do? (Permission verification)

```csharp
// JWT Authentication setup
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
                Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });

// Policy-based authorization
services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("MinimumAge", policy =>
        policy.Requirements.Add(new MinimumAgeRequirement(18)));
});

// Custom authorization requirement
public class MinimumAgeRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }

    public MinimumAgeRequirement(int minimumAge)
    {
        MinimumAge = minimumAge;
    }
}

// Custom authorization handler
public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        var ageClaim = context.User.FindFirst(c => c.Type == "Age");

        if (ageClaim != null && int.TryParse(ageClaim.Value, out int age))
        {
            if (age >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}

// Usage
[Authorize(Policy = "MinimumAge")]
public IActionResult RestrictedContent()
{
    return View();
}
```

### 6. **Response Caching vs Output Caching**
**Q: What's the difference between response caching and output caching?**

```csharp
// Response Caching (client-side & intermediate caching)
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public IActionResult GetProducts()
{
    return Ok(_products);
}

// Output Caching (server-side) - .NET 7+
[OutputCache(Duration = 60)]
public IActionResult GetCategories()
{
    return Ok(_categories);
}

// Custom cache policy
builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("Expire30", builder =>
        builder.Expire(TimeSpan.FromSeconds(30)));

    options.AddPolicy("VaryByUser", builder =>
        builder.VaryByValue((context) =>
            new KeyValuePair<string, string>("user",
                context.User.Identity?.Name ?? string.Empty)));
});
```

---

## Real-Life Example: Building a Multi-Tenant SaaS API

Complete ASP.NET Core example demonstrating middleware, DI, authentication, filters, and more.

```csharp
// ==================== MULTI-TENANT MIDDLEWARE ====================

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        // Extract tenant from header, subdomain, or path
        var tenantId = ExtractTenantId(context);

        if (string.IsNullOrEmpty(tenantId))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { error = "Tenant ID required" });
            return;
        }

        // Validate and set tenant context
        var tenant = await tenantService.GetTenantAsync(tenantId);
        if (tenant == null || !tenant.IsActive)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid or inactive tenant" });
            return;
        }

        // Store tenant in HttpContext
        context.Items["TenantId"] = tenantId;
        context.Items["Tenant"] = tenant;

        await _next(context);
    }

    private string ExtractTenantId(HttpContext context)
    {
        // Try header first
        if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantId))
            return tenantId;

        // Try subdomain
        var host = context.Request.Host.Host;
        var parts = host.Split('.');
        if (parts.Length > 2)
            return parts[0]; // subdomain

        return null;
    }
}

// ==================== REQUEST/RESPONSE LOGGING MIDDLEWARE ====================

public class ApiLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiLoggingMiddleware> _logger;

    public ApiLoggingMiddleware(RequestDelegate next, ILogger<ApiLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString();

        // Log request
        context.Request.EnableBuffering();
        var requestBody = await ReadRequestBodyAsync(context.Request);

        _logger.LogInformation(
            "REQUEST: {RequestId} {Method} {Path} | Body: {Body}",
            requestId,
            context.Request.Method,
            context.Request.Path,
            requestBody);

        // Capture response
        var originalBody = context.Response.Body;
        using var newBody = new MemoryStream();
        context.Response.Body = newBody;

        await _next(context);

        stopwatch.Stop();

        // Log response
        newBody.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(newBody).ReadToEndAsync();

        _logger.LogInformation(
            "RESPONSE: {RequestId} {StatusCode} | Duration: {Duration}ms | Body: {Body}",
            requestId,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            responseBody);

        newBody.Seek(0, SeekOrigin.Begin);
        await newBody.CopyToAsync(originalBody);
    }

    private async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);
        return body;
    }
}

// ==================== CUSTOM AUTHORIZATION HANDLER ====================

// Requirement: User must have specific permission for tenant
public class TenantPermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public TenantPermissionRequirement(string permission)
    {
        Permission = permission;
    }
}

public class TenantPermissionHandler : AuthorizationHandler<TenantPermissionRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPermissionService _permissionService;

    public TenantPermissionHandler(
        IHttpContextAccessor httpContextAccessor,
        IPermissionService permissionService)
    {
        _httpContextAccessor = httpContextAccessor;
        _permissionService = permissionService;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TenantPermissionRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tenantId = httpContext?.Items["TenantId"]?.ToString();
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(userId))
        {
            context.Fail();
            return;
        }

        var hasPermission = await _permissionService.UserHasPermissionAsync(
            userId,
            tenantId,
            requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}

// ==================== RESULT FILTER FOR CONSISTENT API RESPONSES ====================

public class ApiResponseFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(
        ResultExecutingContext context,
        ResultExecutionDelegate next)
    {
        // Wrap successful responses
        if (context.Result is ObjectResult objectResult &&
            objectResult.StatusCode >= 200 &&
            objectResult.StatusCode < 300)
        {
            var apiResponse = new ApiResponse<object>
            {
                Success = true,
                Data = objectResult.Value,
                Timestamp = DateTime.UtcNow,
                RequestId = context.HttpContext.TraceIdentifier
            };

            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = objectResult.StatusCode
            };
        }

        await next();
    }
}

// ==================== EXCEPTION FILTER ====================

public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionFilter(
        ILogger<GlobalExceptionFilter> logger,
        IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Unhandled exception occurred");

        var errorResponse = new ApiResponse<object>
        {
            Success = false,
            Error = context.Exception.Message,
            Timestamp = DateTime.UtcNow,
            RequestId = context.HttpContext.TraceIdentifier
        };

        // Include stack trace in development
        if (_env.IsDevelopment())
        {
            errorResponse.StackTrace = context.Exception.StackTrace;
        }

        var statusCode = context.Exception switch
        {
            UnauthorizedAccessException => 401,
            ForbiddenException => 403,
            NotFoundException => 404,
            ValidationException => 400,
            _ => 500
        };

        context.Result = new ObjectResult(errorResponse)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}

// ==================== CUSTOM MODEL BINDER FOR ENCRYPTED IDS ====================

public class EncryptedIdModelBinder : IModelBinder
{
    private readonly IEncryptionService _encryptionService;

    public EncryptedIdModelBinder(IEncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        var encryptedValue = valueProviderResult.FirstValue;

        try
        {
            var decryptedId = _encryptionService.DecryptId(encryptedValue);
            bindingContext.Result = ModelBindingResult.Success(decryptedId);
        }
        catch (Exception)
        {
            bindingContext.ModelState.AddModelError(modelName, "Invalid ID format");
            bindingContext.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }
}

// Usage attribute
public class EncryptedIdAttribute : ModelBinderAttribute
{
    public EncryptedIdAttribute() : base(typeof(EncryptedIdModelBinder)) { }
}

// ==================== API CONTROLLERS ====================

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMemoryCache _cache;

    public ProductsController(IProductService productService, IMemoryCache cache)
    {
        _productService = productService;
        _cache = cache;
    }

    // GET: api/products
    [HttpGet]
    [ResponseCache(Duration = 60)]
    [ProducesResponseType(typeof(ApiResponse<List<ProductDto>>), 200)]
    public async Task<IActionResult> GetProducts(
        [FromQuery] ProductSearchRequest request)
    {
        var tenantId = HttpContext.Items["TenantId"]?.ToString();
        var products = await _productService.SearchProductsAsync(tenantId, request);
        return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProductDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProduct([EncryptedId] int id)
    {
        var tenantId = HttpContext.Items["TenantId"]?.ToString();
        var cacheKey = $"product_{tenantId}_{id}";

        // Try cache first
        if (!_cache.TryGetValue(cacheKey, out ProductDto product))
        {
            product = await _productService.GetProductByIdAsync(tenantId, id);

            if (product == null)
                return NotFound();

            // Cache for 5 minutes
            _cache.Set(cacheKey, product, TimeSpan.FromMinutes(5));
        }

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    [Authorize(Policy = "RequireProductCreatePermission")]
    [ProducesResponseType(typeof(ApiResponse<ProductDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        var tenantId = HttpContext.Items["TenantId"]?.ToString();
        var product = await _productService.CreateProductAsync(tenantId, request);

        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.Id },
            product);
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    [Authorize(Policy = "RequireProductUpdatePermission")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
    {
        var tenantId = HttpContext.Items["TenantId"]?.ToString();
        await _productService.UpdateProductAsync(tenantId, id, request);

        // Invalidate cache
        _cache.Remove($"product_{tenantId}_{id}");

        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    [Authorize(Policy = "RequireProductDeletePermission")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var tenantId = HttpContext.Items["TenantId"]?.ToString();
        await _productService.DeleteProductAsync(tenantId, id);

        _cache.Remove($"product_{tenantId}_{id}");

        return NoContent();
    }
}

// ==================== STARTUP CONFIGURATION ====================

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline
        ConfigurePipeline(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Controllers with filters
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiResponseFilter>();
            options.Filters.Add<GlobalExceptionFilter>();
        });

        // Scoped services (per request)
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPermissionService, PermissionService>();

        // Singleton services
        services.AddSingleton<IEncryptionService, EncryptionService>();

        // Memory cache
        services.AddMemoryCache();

        // HTTP Context Accessor
        services.AddHttpContextAccessor();

        // JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

        // Authorization policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireProductCreatePermission", policy =>
                policy.Requirements.Add(new TenantPermissionRequirement("product.create")));

            options.AddPolicy("RequireProductUpdatePermission", policy =>
                policy.Requirements.Add(new TenantPermissionRequirement("product.update")));

            options.AddPolicy("RequireProductDeletePermission", policy =>
                policy.Requirements.Add(new TenantPermissionRequirement("product.delete")));
        });

        // Authorization handlers
        services.AddSingleton<IAuthorizationHandler, TenantPermissionHandler>();

        // API Versioning
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SaaS API", Version = "v1" });

            // JWT Authentication in Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins("https://app.example.com")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        // Response Compression
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { "application/json" });
        });
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        // Development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Security headers
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
            await next();
        });

        app.UseHttpsRedirection();
        app.UseResponseCompression();
        app.UseCors("AllowSpecificOrigins");

        // Custom middleware (ORDER MATTERS!)
        app.UseMiddleware<ApiLoggingMiddleware>();
        app.UseMiddleware<TenantMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }
}

// ==================== SUPPORTING CLASSES ====================

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string Error { get; set; }
    public string StackTrace { get; set; }
    public DateTime Timestamp { get; set; }
    public string RequestId { get; set; }
}

public class Tenant
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string DatabaseConnection { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class ProductSearchRequest
{
    public string SearchTerm { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class CreateProductRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}

public class UpdateProductRequest
{
    [StringLength(100)]
    public string Name { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }

    [Range(0, int.MaxValue)]
    public int? Stock { get; set; }
}

// Custom exceptions
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

// Service interfaces
public interface ITenantService
{
    Task<Tenant> GetTenantAsync(string tenantId);
}

public interface IProductService
{
    Task<List<ProductDto>> SearchProductsAsync(string tenantId, ProductSearchRequest request);
    Task<ProductDto> GetProductByIdAsync(string tenantId, int id);
    Task<ProductDto> CreateProductAsync(string tenantId, CreateProductRequest request);
    Task UpdateProductAsync(string tenantId, int id, UpdateProductRequest request);
    Task DeleteProductAsync(string tenantId, int id);
}

public interface IPermissionService
{
    Task<bool> UserHasPermissionAsync(string userId, string tenantId, string permission);
}

public interface IEncryptionService
{
    int DecryptId(string encryptedId);
    string EncryptId(int id);
}
```

**Key Real-Life Takeaways:**
1. **Multi-tenancy**: Tenant isolation using middleware
2. **Middleware Pipeline**: Logging, authentication, custom processing
3. **Authorization**: Custom handlers for complex permission logic
4. **Filters**: Global exception handling, response wrapping
5. **Model Binding**: Custom binders for encrypted IDs, complex types
6. **Caching**: In-memory cache with invalidation strategy
7. **API Responses**: Consistent response format across all endpoints
8. **Security**: CORS, security headers, JWT authentication

---

## Entity Framework Core

### 1. **Lazy Loading vs Eager Loading vs Explicit Loading**
**Q: Explain different loading strategies in EF Core?**

```csharp
// Eager Loading (Include)
var users = context.Users
    .Include(u => u.Orders)
        .ThenInclude(o => o.OrderItems)
    .ToList();

// Explicit Loading
var user = context.Users.Find(1);
context.Entry(user)
    .Collection(u => u.Orders)
    .Load();

// Lazy Loading (requires Microsoft.EntityFrameworkCore.Proxies)
// Configure in OnConfiguring
optionsBuilder.UseLazyLoadingProxies();

// Virtual properties enable lazy loading
public class User
{
    public int Id { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}
```

### 2. **Change Tracking and No-Tracking Queries**
**Q: When should you use no-tracking queries?**

**Answer:**
Use `.AsNoTracking()` for read-only scenarios to improve performance.

```csharp
// No-tracking (better performance for read-only)
var users = context.Users
    .AsNoTracking()
    .Where(u => u.IsActive)
    .ToList();

// Tracking (for updates)
var user = context.Users.Find(1);
user.Email = "newemail@example.com";
context.SaveChanges(); // Detected by change tracker

// Track graph manually
context.Attach(user);
context.Entry(user).State = EntityState.Modified;
```

### 3. **Global Query Filters**
**Q: How to implement soft delete using global query filters?**

```csharp
// Entity
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
}

// DbContext configuration
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Global query filter
    modelBuilder.Entity<Product>()
        .HasQueryFilter(p => !p.IsDeleted);
}

// Usage
var products = context.Products.ToList(); // Automatically filters IsDeleted = false

// Ignore filter when needed
var allProducts = context.Products
    .IgnoreQueryFilters()
    .ToList();
```

### 4. **Transactions and Concurrency**
**Q: How to handle transactions and concurrency in EF Core?**

```csharp
// Transaction
using var transaction = context.Database.BeginTransaction();
try
{
    var user = new User { Name = "John" };
    context.Users.Add(user);
    await context.SaveChangesAsync();

    var order = new Order { UserId = user.Id };
    context.Orders.Add(order);
    await context.SaveChangesAsync();

    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}

// Optimistic Concurrency using RowVersion
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}

// Handling concurrency
try
{
    await context.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException ex)
{
    var entry = ex.Entries.Single();
    var databaseValues = await entry.GetDatabaseValuesAsync();

    if (databaseValues == null)
    {
        // Entity was deleted
    }
    else
    {
        // Resolve conflict
        entry.OriginalValues.SetValues(databaseValues);
    }
}
```

### 5. **Raw SQL and Stored Procedures**
**Q: How to execute raw SQL safely in EF Core?**

```csharp
// Parameterized query (safe from SQL injection)
var userId = 1;
var users = context.Users
    .FromSqlRaw("SELECT * FROM Users WHERE Id = {0}", userId)
    .ToList();

// Or using interpolation (automatically parameterized)
var users = context.Users
    .FromSqlInterpolated($"SELECT * FROM Users WHERE Id = {userId}")
    .ToList();

// Execute stored procedure
var results = context.Users
    .FromSqlRaw("EXEC GetActiveUsers")
    .ToList();

// Non-query commands
var rowsAffected = context.Database
    .ExecuteSqlRaw("UPDATE Users SET IsActive = 1 WHERE CreatedDate < {0}",
        DateTime.Now.AddYears(-1));
```

### 6. **Performance Optimization**
**Q: What are best practices for EF Core performance?**

```csharp
// 1. Select only needed columns
var users = context.Users
    .Select(u => new { u.Id, u.Name })
    .ToList();

// 2. Use compiled queries for frequently executed queries
private static Func<ApplicationDbContext, int, User> _getUserById =
    EF.CompileQuery((ApplicationDbContext context, int id) =>
        context.Users.FirstOrDefault(u => u.Id == id));

var user = _getUserById(context, 1);

// 3. Batch operations
context.Users.AddRange(userList); // Better than multiple Add() calls
await context.SaveChangesAsync();

// 4. Split queries for multiple includes
var users = context.Users
    .Include(u => u.Orders)
    .Include(u => u.Addresses)
    .AsSplitQuery() // Generates multiple SQL queries
    .ToList();

// 5. Use AsAsyncEnumerable for large datasets
await foreach (var user in context.Users.AsAsyncEnumerable())
{
    // Process one at a time without loading all into memory
}
```

---

## Design Patterns & Architecture

### 1. **Repository Pattern**
**Q: Implement a generic repository pattern?**

```csharp
// Generic repository interface
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}

// Implementation
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    // ... other methods
}

// Unit of Work pattern
public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Order> Orders { get; }
    Task<int> CompleteAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IRepository<User> Users { get; private set; }
    public IRepository<Order> Orders { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new Repository<User>(_context);
        Orders = new Repository<Order>(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
```

### 2. **CQRS Pattern**
**Q: Explain and implement CQRS pattern?**

**Answer:**
CQRS (Command Query Responsibility Segregation) separates read and write operations.

```csharp
// Command (write)
public class CreateUserCommand
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CreateUserCommandHandler
{
    private readonly IRepository<User> _repository;

    public CreateUserCommandHandler(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<int> HandleAsync(CreateUserCommand command)
    {
        var user = new User
        {
            Name = command.Name,
            Email = command.Email
        };

        await _repository.AddAsync(user);
        return user.Id;
    }
}

// Query (read)
public class GetUserQuery
{
    public int UserId { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class GetUserQueryHandler
{
    private readonly ApplicationDbContext _context;

    public GetUserQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> HandleAsync(GetUserQuery query)
    {
        return await _context.Users
            .Where(u => u.Id == query.UserId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            })
            .FirstOrDefaultAsync();
    }
}

// Using MediatR for CQRS
public class CreateUserCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IRepository<User> _repository;

    public CreateUserCommandHandler(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User { Name = request.Name, Email = request.Email };
        await _repository.AddAsync(user);
        return user.Id;
    }
}
```

### 3. **Decorator Pattern**
**Q: Implement decorator pattern for adding logging to a service?**

```csharp
// Base interface
public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderRequest request);
}

// Core implementation
public class OrderService : IOrderService
{
    private readonly IRepository<Order> _repository;

    public OrderService(IRepository<Order> repository)
    {
        _repository = repository;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
    {
        var order = new Order
        {
            CustomerId = request.CustomerId,
            TotalAmount = request.TotalAmount
        };

        await _repository.AddAsync(order);
        return order;
    }
}

// Logging decorator
public class LoggingOrderServiceDecorator : IOrderService
{
    private readonly IOrderService _innerService;
    private readonly ILogger<LoggingOrderServiceDecorator> _logger;

    public LoggingOrderServiceDecorator(
        IOrderService innerService,
        ILogger<LoggingOrderServiceDecorator> logger)
    {
        _innerService = innerService;
        _logger = logger;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
    {
        _logger.LogInformation($"Creating order for customer {request.CustomerId}");

        try
        {
            var result = await _innerService.CreateOrderAsync(request);
            _logger.LogInformation($"Order {result.Id} created successfully");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create order");
            throw;
        }
    }
}

// Registration with Scrutor
services.AddScoped<IOrderService, OrderService>();
services.Decorate<IOrderService, LoggingOrderServiceDecorator>();
```

### 4. **Strategy Pattern**
**Q: Implement strategy pattern for payment processing?**

```csharp
// Strategy interface
public interface IPaymentStrategy
{
    Task<PaymentResult> ProcessPaymentAsync(decimal amount, PaymentDetails details);
}

// Concrete strategies
public class CreditCardPaymentStrategy : IPaymentStrategy
{
    public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, PaymentDetails details)
    {
        // Credit card processing logic
        return new PaymentResult { Success = true, TransactionId = Guid.NewGuid().ToString() };
    }
}

public class PayPalPaymentStrategy : IPaymentStrategy
{
    public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, PaymentDetails details)
    {
        // PayPal processing logic
        return new PaymentResult { Success = true, TransactionId = Guid.NewGuid().ToString() };
    }
}

// Context
public class PaymentProcessor
{
    private readonly Dictionary<PaymentMethod, IPaymentStrategy> _strategies;

    public PaymentProcessor(IEnumerable<IPaymentStrategy> strategies)
    {
        _strategies = new Dictionary<PaymentMethod, IPaymentStrategy>
        {
            { PaymentMethod.CreditCard, strategies.OfType<CreditCardPaymentStrategy>().First() },
            { PaymentMethod.PayPal, strategies.OfType<PayPalPaymentStrategy>().First() }
        };
    }

    public async Task<PaymentResult> ProcessAsync(
        PaymentMethod method,
        decimal amount,
        PaymentDetails details)
    {
        if (!_strategies.TryGetValue(method, out var strategy))
            throw new NotSupportedException($"Payment method {method} not supported");

        return await strategy.ProcessPaymentAsync(amount, details);
    }
}
```

### 5. **Clean Architecture / Onion Architecture**
**Q: Describe the layers in Clean Architecture?**

**Answer:**
```
┌─────────────────────────────────┐
│   Presentation (API/UI)         │
├─────────────────────────────────┤
│   Application (Use Cases)       │
├─────────────────────────────────┤
│   Domain (Entities, Rules)      │
└─────────────────────────────────┘
         Infrastructure
     (Data Access, External)
```

**Key Principles:**
1. Domain layer has no dependencies
2. Dependencies point inward
3. Application layer contains business logic
4. Infrastructure implements interfaces from domain/application

```csharp
// Domain Layer
public class Order
{
    public int Id { get; private set; }
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }

    public void MarkAsShipped()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Only paid orders can be shipped");

        Status = OrderStatus.Shipped;
    }
}

// Application Layer
public class CreateOrderUseCase
{
    private readonly IOrderRepository _repository;
    private readonly IEventPublisher _eventPublisher;

    public async Task<int> ExecuteAsync(CreateOrderCommand command)
    {
        var order = new Order(command.CustomerId, command.Items);
        await _repository.AddAsync(order);

        await _eventPublisher.PublishAsync(new OrderCreatedEvent(order.Id));

        return order.Id;
    }
}

// Infrastructure Layer
public class OrderRepository : IOrderRepository
{
    private readonly DbContext _context;

    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
}
```

---

## Asynchronous Programming

### 1. **async/await Best Practices**
**Q: What are common mistakes with async/await?**

```csharp
// ❌ BAD: Blocking on async code (causes deadlock in UI apps)
var result = SomeAsyncMethod().Result;
var result2 = SomeAsyncMethod().GetAwaiter().GetResult();

// ✅ GOOD: Async all the way
var result = await SomeAsyncMethod();

// ❌ BAD: Async void (except event handlers)
public async void ProcessData() { }

// ✅ GOOD: Return Task
public async Task ProcessDataAsync() { }

// ❌ BAD: Unnecessary async/await
public async Task<int> GetValueAsync()
{
    return await repository.GetValueAsync();
}

// ✅ GOOD: Return task directly when no processing needed
public Task<int> GetValueAsync()
{
    return repository.GetValueAsync();
}

// ❌ BAD: Not using ConfigureAwait in library code
await SomeMethodAsync();

// ✅ GOOD: Use ConfigureAwait(false) in library code
await SomeMethodAsync().ConfigureAwait(false);
```

### 2. **Task Parallelism**
**Q: How to run tasks in parallel?**

```csharp
// Run multiple tasks in parallel
var task1 = Service1.GetDataAsync();
var task2 = Service2.GetDataAsync();
var task3 = Service3.GetDataAsync();

await Task.WhenAll(task1, task2, task3);

var result1 = task1.Result;
var result2 = task2.Result;
var result3 = task3.Result;

// Parallel.ForEachAsync (.NET 6+)
var urls = GetUrls();
await Parallel.ForEachAsync(urls, new ParallelOptions
{
    MaxDegreeOfParallelism = 5
},
async (url, cancellationToken) =>
{
    var data = await httpClient.GetStringAsync(url, cancellationToken);
    ProcessData(data);
});

// Process with SemaphoreSlim for throttling
var semaphore = new SemaphoreSlim(5); // Max 5 concurrent
var tasks = urls.Select(async url =>
{
    await semaphore.WaitAsync();
    try
    {
        return await httpClient.GetStringAsync(url);
    }
    finally
    {
        semaphore.Release();
    }
});

var results = await Task.WhenAll(tasks);
```

### 3. **CancellationToken**
**Q: How to properly implement cancellation?**

```csharp
// Accepting cancellation token
public async Task<List<Data>> ProcessDataAsync(CancellationToken cancellationToken)
{
    var results = new List<Data>();

    foreach (var item in items)
    {
        // Check for cancellation
        cancellationToken.ThrowIfCancellationRequested();

        var data = await FetchDataAsync(item, cancellationToken);
        results.Add(data);

        // Optional: check periodically in long operations
        if (cancellationToken.IsCancellationRequested)
            break;
    }

    return results;
}

// Usage with timeout
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
try
{
    var result = await ProcessDataAsync(cts.Token);
}
catch (OperationCanceledException)
{
    // Handle cancellation
}

// Linked cancellation tokens
var cts1 = new CancellationTokenSource();
var cts2 = new CancellationTokenSource(TimeSpan.FromMinutes(5));
var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token);

await LongRunningOperationAsync(linkedCts.Token);
```

### 4. **ValueTask vs Task**
**Q: When to use ValueTask instead of Task?**

**Answer:**
Use `ValueTask<T>` when:
- Result may be available synchronously
- Method is called frequently (hot path)
- Want to avoid allocation

```csharp
// Example: Caching scenario
private Dictionary<int, User> _cache = new();

public ValueTask<User> GetUserAsync(int id)
{
    // Synchronous path - no allocation
    if (_cache.TryGetValue(id, out var user))
        return new ValueTask<User>(user);

    // Asynchronous path
    return new ValueTask<User>(FetchUserFromDatabaseAsync(id));
}

private async Task<User> FetchUserFromDatabaseAsync(int id)
{
    var user = await _repository.GetByIdAsync(id);
    _cache[id] = user;
    return user;
}

// ⚠️ Important: ValueTask should only be awaited once!
// ❌ BAD
var valueTask = GetUserAsync(1);
await valueTask;
await valueTask; // Don't do this!

// ✅ GOOD
var result = await GetUserAsync(1);
```

---

## Memory Management & Performance

### 1. **Garbage Collection**
**Q: Explain GC generations and optimization strategies?**

**Answer:**
- **Gen 0**: Short-lived objects (most collections happen here)
- **Gen 1**: Buffer between Gen 0 and Gen 2
- **Gen 2**: Long-lived objects (expensive to collect)
- **LOH**: Large Object Heap (objects > 85KB)

```csharp
// Check GC statistics
Console.WriteLine($"Gen 0 collections: {GC.CollectionCount(0)}");
Console.WriteLine($"Gen 1 collections: {GC.CollectionCount(1)}");
Console.WriteLine($"Gen 2 collections: {GC.CollectionCount(2)}");

// Force collection (rarely needed)
GC.Collect();
GC.WaitForPendingFinalizers();

// Object pooling to reduce allocations
var arrayPool = ArrayPool<byte>.Shared;
byte[] buffer = arrayPool.Rent(1024);
try
{
    // Use buffer
}
finally
{
    arrayPool.Return(buffer);
}
```

### 2. **IDisposable and Using Statement**
**Q: Implement IDisposable pattern correctly?**

```csharp
// Full disposable pattern
public class ResourceHolder : IDisposable
{
    private bool _disposed = false;
    private IntPtr _unmanagedResource;
    private Stream _managedResource;

    public ResourceHolder()
    {
        _unmanagedResource = // allocate unmanaged resource
        _managedResource = new FileStream("file.txt", FileMode.Open);
    }

    // Public dispose method
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Prevent finalizer from running
    }

    // Protected dispose method
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            // Dispose managed resources
            _managedResource?.Dispose();
        }

        // Free unmanaged resources
        if (_unmanagedResource != IntPtr.Zero)
        {
            // Free unmanaged resource
            _unmanagedResource = IntPtr.Zero;
        }

        _disposed = true;
    }

    // Finalizer (only if you have unmanaged resources)
    ~ResourceHolder()
    {
        Dispose(false);
    }
}

// Using statement variations
// Traditional
using (var resource = new ResourceHolder())
{
    // Use resource
}

// C# 8+ using declaration
using var resource = new ResourceHolder();
// Disposed at end of scope

// Multiple resources
using var resource1 = new ResourceHolder();
using var resource2 = new ResourceHolder();
```

### 3. **Memory Leaks**
**Q: What causes memory leaks in .NET and how to prevent them?**

**Answer:**
Common causes:
1. Event handlers not unsubscribed
2. Static references to objects
3. Timers not disposed
4. Finalizers preventing collection

```csharp
// ❌ BAD: Event handler leak
public class Publisher
{
    public event EventHandler DataChanged;
}

public class Subscriber
{
    public Subscriber(Publisher publisher)
    {
        publisher.DataChanged += OnDataChanged; // LEAK if not unsubscribed
    }

    private void OnDataChanged(object sender, EventArgs e) { }
}

// ✅ GOOD: Unsubscribe
public class Subscriber : IDisposable
{
    private readonly Publisher _publisher;

    public Subscriber(Publisher publisher)
    {
        _publisher = publisher;
        _publisher.DataChanged += OnDataChanged;
    }

    public void Dispose()
    {
        _publisher.DataChanged -= OnDataChanged;
    }

    private void OnDataChanged(object sender, EventArgs e) { }
}

// ✅ BETTER: Weak event pattern
public class WeakEventManager
{
    private readonly List<WeakReference<EventHandler>> _handlers = new();

    public void AddHandler(EventHandler handler)
    {
        _handlers.Add(new WeakReference<EventHandler>(handler));
    }

    public void Raise(object sender, EventArgs e)
    {
        foreach (var weakHandler in _handlers.ToList())
        {
            if (weakHandler.TryGetTarget(out var handler))
                handler(sender, e);
            else
                _handlers.Remove(weakHandler); // Remove dead reference
        }
    }
}

// ❌ BAD: Timer leak
var timer = new System.Timers.Timer(1000);
timer.Elapsed += (s, e) => Console.WriteLine("Tick");
timer.Start();
// Timer should be disposed!

// ✅ GOOD
using var timer = new System.Timers.Timer(1000);
timer.Elapsed += (s, e) => Console.WriteLine("Tick");
timer.Start();
```

### 4. **Benchmarking with BenchmarkDotNet**
**Q: How to benchmark code performance?**

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class StringBenchmarks
{
    private const int Iterations = 1000;

    [Benchmark]
    public string StringConcatenation()
    {
        string result = "";
        for (int i = 0; i < Iterations; i++)
            result += "test";
        return result;
    }

    [Benchmark]
    public string StringBuilder()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Iterations; i++)
            sb.Append("test");
        return sb.ToString();
    }

    [Benchmark(Baseline = true)]
    public string StringCreate()
    {
        return string.Create(Iterations * 4, Iterations, (span, count) =>
        {
            for (int i = 0; i < count; i++)
            {
                "test".AsSpan().CopyTo(span.Slice(i * 4, 4));
            }
        });
    }
}

// Run benchmarks
public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<StringBenchmarks>();
    }
}
```

---

## Security

### 1. **SQL Injection Prevention**
```csharp
// ❌ VULNERABLE
string query = $"SELECT * FROM Users WHERE Username = '{username}'";

// ✅ SAFE: Parameterized queries
string query = "SELECT * FROM Users WHERE Username = @username";
command.Parameters.AddWithValue("@username", username);

// ✅ SAFE: EF Core (automatically parameterized)
var user = context.Users.FirstOrDefault(u => u.Username == username);

// ✅ SAFE: Dapper
var user = connection.QueryFirstOrDefault<User>(
    "SELECT * FROM Users WHERE Username = @Username",
    new { Username = username });
```

### 2. **XSS Prevention**
```csharp
// In Razor views, @ automatically HTML encodes
<p>@Model.UserInput</p> <!-- Safe -->

// For JavaScript contexts, use JSON encoding
<script>
    var data = @Html.Raw(Json.Serialize(Model.Data));
</script>

// Content Security Policy
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self'; script-src 'self' 'unsafe-inline'");
    await next();
});
```

### 3. **Secrets Management**
```csharp
// ❌ NEVER hardcode secrets
string apiKey = "sk_live_xxxxxxxxxxxxx";

// ✅ Use User Secrets (development)
// dotnet user-secrets init
// dotnet user-secrets set "ApiKey" "value"

// ✅ Use environment variables
string apiKey = Environment.GetEnvironmentVariable("API_KEY");

// ✅ Use Azure Key Vault (production)
var client = new SecretClient(
    new Uri("https://myvault.vault.azure.net/"),
    new DefaultAzureCredential());

KeyVaultSecret secret = await client.GetSecretAsync("ApiKey");
string apiKey = secret.Value;

// Configuration
builder.Configuration.AddAzureKeyVault(
    new Uri("https://myvault.vault.azure.net/"),
    new DefaultAzureCredential());
```

### 4. **CSRF Protection**
```csharp
// Automatic in ASP.NET Core for forms
@Html.AntiForgeryToken()

// Validate in controller
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult SubmitForm(FormModel model)
{
    // Safe from CSRF
}

// For AJAX
services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

// In JavaScript
fetch('/api/data', {
    method: 'POST',
    headers: {
        'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]').value
    }
});
```

### 5. **Password Hashing**
```csharp
// Use ASP.NET Core Identity's password hasher
public class PasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordService()
    {
        _passwordHasher = new PasswordHasher<User>();
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}

// Or use BCrypt
using BCrypt.Net;

string hashedPassword = BCrypt.HashPassword("password123");
bool isValid = BCrypt.Verify("password123", hashedPassword);
```

---

## Microservices & Distributed Systems

### 1. **Service Communication**
**Q: Compare synchronous vs asynchronous communication in microservices?**

```csharp
// Synchronous: HTTP with HttpClient
public class OrderService
{
    private readonly HttpClient _httpClient;

    public async Task<Product> GetProductAsync(int productId)
    {
        var response = await _httpClient.GetAsync($"api/products/{productId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Product>();
    }
}

// Typed HTTP client (better approach)
services.AddHttpClient<IProductService, ProductService>(client =>
{
    client.BaseAddress = new Uri("https://api.products.com");
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy());

// Asynchronous: Message Queue (RabbitMQ)
public class OrderCreatedEventPublisher
{
    private readonly IConnection _connection;

    public async Task PublishAsync(OrderCreatedEvent @event)
    {
        using var channel = _connection.CreateModel();

        channel.QueueDeclare(
            queue: "order-created",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

        channel.BasicPublish(
            exchange: "",
            routingKey: "order-created",
            basicProperties: null,
            body: body);
    }
}
```

### 2. **Resilience Patterns with Polly**
```csharp
// Retry policy
var retryPolicy = Policy
    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
    .WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

// Circuit breaker
var circuitBreakerPolicy = Policy
    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
    .CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 3,
        durationOfBreak: TimeSpan.FromSeconds(30));

// Combine policies
var combinedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

// Usage
var response = await combinedPolicy.ExecuteAsync(() =>
    _httpClient.GetAsync("api/products"));

// Timeout policy
var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
    TimeSpan.FromSeconds(10));

// Fallback policy
var fallbackPolicy = Policy<HttpResponseMessage>
    .Handle<Exception>()
    .FallbackAsync(new HttpResponseMessage
    {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(JsonSerializer.Serialize(GetDefaultProduct()))
    });
```

### 3. **API Gateway Pattern**
**Q: What is an API Gateway and how to implement with Ocelot?**

```json
// ocelot.json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/products/{id}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "product-service",
          "Port": 80
        }
      ],
      "UpstreamPathTemplate": "/products/{id}",
      "UpstreamHttpMethod": [ "Get" ],
      "RateLimitOptions": {
        "EnableRateLimiting": true,
        "Period": "1s",
        "Limit": 10
      }
    },
    {
      "DownstreamPathTemplate": "/api/orders",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "order-service",
          "Port": 80
        }
      ],
      "UpstreamPathTemplate": "/orders",
      "UpstreamHttpMethod": [ "Get", "Post" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer"
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://api.mycompany.com"
  }
}
```

```csharp
// Program.cs
builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot();

app.UseOcelot().Wait();
```

### 4. **Distributed Tracing**
```csharp
// Using OpenTelemetry
services.AddOpenTelemetry()
    .WithTracing(builder =>
    {
        builder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "jaeger";
                options.AgentPort = 6831;
            });
    });

// Custom activity
using var activity = Activity.StartActivity("ProcessOrder");
activity?.SetTag("order.id", orderId);
activity?.SetTag("customer.id", customerId);

try
{
    await ProcessOrderAsync(orderId);
    activity?.SetStatus(ActivityStatusCode.Ok);
}
catch (Exception ex)
{
    activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
    throw;
}
```

### 5. **Saga Pattern**
**Q: How to implement distributed transactions?**

```csharp
// Orchestration-based Saga
public class OrderSaga
{
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly IInventoryService _inventoryService;

    public async Task<SagaResult> ExecuteAsync(CreateOrderRequest request)
    {
        var sagaState = new SagaState();

        try
        {
            // Step 1: Create order
            var order = await _orderService.CreateOrderAsync(request);
            sagaState.OrderId = order.Id;

            // Step 2: Reserve inventory
            await _inventoryService.ReserveAsync(order.Items);
            sagaState.InventoryReserved = true;

            // Step 3: Process payment
            var payment = await _paymentService.ProcessAsync(order.TotalAmount);
            sagaState.PaymentProcessed = true;

            // Step 4: Confirm order
            await _orderService.ConfirmAsync(order.Id);

            return SagaResult.Success(order.Id);
        }
        catch (Exception ex)
        {
            // Compensating transactions
            await CompensateAsync(sagaState);
            return SagaResult.Failed(ex.Message);
        }
    }

    private async Task CompensateAsync(SagaState state)
    {
        if (state.PaymentProcessed)
            await _paymentService.RefundAsync(state.OrderId);

        if (state.InventoryReserved)
            await _inventoryService.ReleaseAsync(state.OrderId);

        if (state.OrderId.HasValue)
            await _orderService.CancelAsync(state.OrderId.Value);
    }
}
```

---

## Testing

### 1. **Unit Testing Best Practices**
```csharp
using Xunit;
using Moq;
using FluentAssertions;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockRepository;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly OrderService _sut; // System Under Test

    public OrderServiceTests()
    {
        _mockRepository = new Mock<IOrderRepository>();
        _mockEmailService = new Mock<IEmailService>();
        _sut = new OrderService(_mockRepository.Object, _mockEmailService.Object);
    }

    [Fact]
    public async Task CreateOrder_WithValidData_ShouldReturnOrderId()
    {
        // Arrange
        var request = new CreateOrderRequest
        {
            CustomerId = 1,
            Items = new List<OrderItem> { new() { ProductId = 1, Quantity = 2 } }
        };

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(new Order { Id = 123 });

        // Act
        var result = await _sut.CreateOrderAsync(request);

        // Assert
        result.Should().Be(123);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        _mockEmailService.Verify(
            e => e.SendOrderConfirmationAsync(It.IsAny<int>()),
            Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CreateOrder_WithInvalidCustomerId_ShouldThrowException(int customerId)
    {
        // Arrange
        var request = new CreateOrderRequest { CustomerId = customerId };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.CreateOrderAsync(request));
    }

    [Fact]
    public async Task CreateOrder_WhenRepositoryFails_ShouldNotSendEmail()
    {
        // Arrange
        var request = new CreateOrderRequest { CustomerId = 1 };
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _sut.CreateOrderAsync(request));
        _mockEmailService.Verify(
            e => e.SendOrderConfirmationAsync(It.IsAny<int>()),
            Times.Never);
    }
}
```

### 2. **Integration Testing**
```csharp
public class OrdersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public OrdersControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove real database
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Add in-memory database
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Build service provider
                var sp = services.BuildServiceProvider();

                // Seed test data
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedDatabase(db);
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetOrders_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/orders");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task CreateOrder_WithValidData_ReturnsCreated()
    {
        // Arrange
        var order = new CreateOrderRequest
        {
            CustomerId = 1,
            Items = new List<OrderItem>
            {
                new() { ProductId = 1, Quantity = 2 }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", order);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    private void SeedDatabase(ApplicationDbContext db)
    {
        db.Products.Add(new Product { Id = 1, Name = "Test Product", Price = 10 });
        db.SaveChanges();
    }
}
```

### 3. **Test Data Builders**
```csharp
// Builder pattern for test data
public class OrderBuilder
{
    private int _customerId = 1;
    private List<OrderItem> _items = new();
    private OrderStatus _status = OrderStatus.Pending;

    public OrderBuilder WithCustomerId(int customerId)
    {
        _customerId = customerId;
        return this;
    }

    public OrderBuilder WithItem(int productId, int quantity)
    {
        _items.Add(new OrderItem { ProductId = productId, Quantity = quantity });
        return this;
    }

    public OrderBuilder WithStatus(OrderStatus status)
    {
        _status = status;
        return this;
    }

    public Order Build()
    {
        return new Order
        {
            CustomerId = _customerId,
            Items = _items,
            Status = _status,
            CreatedDate = DateTime.UtcNow
        };
    }
}

// Usage in tests
[Fact]
public void Test_OrderWithMultipleItems()
{
    var order = new OrderBuilder()
        .WithCustomerId(1)
        .WithItem(productId: 1, quantity: 2)
        .WithItem(productId: 2, quantity: 1)
        .WithStatus(OrderStatus.Paid)
        .Build();

    // Test with order
}
```

---

## Advanced Scenarios & Problem Solving

### 1. **Rate Limiting**
```csharp
// Using AspNetCoreRateLimit
services.AddMemoryCache();
services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
services.AddInMemoryRateLimiting();
services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Or simple implementation
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly Dictionary<string, Queue<DateTime>> _requests = new();
    private static readonly SemaphoreSlim _semaphore = new(1);

    public async Task InvokeAsync(HttpContext context)
    {
        var clientId = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        await _semaphore.WaitAsync();
        try
        {
            if (!_requests.ContainsKey(clientId))
                _requests[clientId] = new Queue<DateTime>();

            var queue = _requests[clientId];

            // Remove requests older than 1 minute
            while (queue.Count > 0 && queue.Peek() < now.AddMinutes(-1))
                queue.Dequeue();

            if (queue.Count >= 100) // 100 requests per minute
            {
                context.Response.StatusCode = 429; // Too Many Requests
                await context.Response.WriteAsync("Rate limit exceeded");
                return;
            }

            queue.Enqueue(now);
        }
        finally
        {
            _semaphore.Release();
        }

        await _next(context);
    }
}
```

### 2. **Caching Strategies**
```csharp
// Memory cache
public class ProductService
{
    private readonly IMemoryCache _cache;
    private readonly IProductRepository _repository;

    public async Task<Product> GetProductAsync(int id)
    {
        return await _cache.GetOrCreateAsync($"product_{id}", async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(5);
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            return await _repository.GetByIdAsync(id);
        });
    }
}

// Distributed cache (Redis)
public class CachedOrderService
{
    private readonly IDistributedCache _cache;
    private readonly IOrderService _orderService;

    public async Task<Order> GetOrderAsync(int id)
    {
        var cacheKey = $"order_{id}";
        var cachedOrder = await _cache.GetStringAsync(cacheKey);

        if (cachedOrder != null)
            return JsonSerializer.Deserialize<Order>(cachedOrder);

        var order = await _orderService.GetOrderAsync(id);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(order),
            options);

        return order;
    }

    public async Task InvalidateOrderCacheAsync(int id)
    {
        await _cache.RemoveAsync($"order_{id}");
    }
}

// Cache-aside pattern with lazy loading
public class CacheAsideService<T>
{
    private readonly IDistributedCache _cache;

    public async Task<T> GetOrSetAsync(
        string key,
        Func<Task<T>> factory,
        TimeSpan? expiration = null)
    {
        var cached = await _cache.GetStringAsync(key);
        if (cached != null)
            return JsonSerializer.Deserialize<T>(cached);

        var value = await factory();

        await _cache.SetStringAsync(
            key,
            JsonSerializer.Serialize(value),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
            });

        return value;
    }
}
```

### 3. **Background Jobs**
```csharp
// Using Hangfire
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHangfire(config =>
            config.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection")));

        services.AddHangfireServer();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseHangfireDashboard();

        // Recurring job
        RecurringJob.AddOrUpdate<IEmailService>(
            "send-daily-report",
            service => service.SendDailyReportAsync(),
            Cron.Daily);
    }
}

// Using IHostedService
public class OrderProcessingBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderProcessingBackgroundService> _logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order processing service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                await orderService.ProcessPendingOrdersAsync();

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing orders");
            }
        }
    }
}

// Registration
services.AddHostedService<OrderProcessingBackgroundService>();
```

### 4. **File Upload with Streaming**
```csharp
[HttpPost("upload")]
[RequestSizeLimit(100_000_000)] // 100 MB
public async Task<IActionResult> UploadLargeFile()
{
    var boundary = Request.GetMultipartBoundary();
    var reader = new MultipartReader(boundary, Request.Body);

    MultipartSection section;
    while ((section = await reader.ReadNextSectionAsync()) != null)
    {
        var hasContentDisposition = ContentDispositionHeaderValue.TryParse(
            section.ContentDisposition, out var contentDisposition);

        if (hasContentDisposition && contentDisposition.IsFileDisposition())
        {
            var fileName = contentDisposition.FileName.Value;
            var filePath = Path.Combine("uploads", fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await section.Body.CopyToAsync(stream);
        }
    }

    return Ok();
}

// With progress tracking
public async Task<IActionResult> UploadWithProgress()
{
    var file = Request.Form.Files[0];
    var filePath = Path.Combine("uploads", file.FileName);

    using var fileStream = new FileStream(filePath, FileMode.Create);

    var buffer = new byte[81920]; // 80 KB buffer
    int bytesRead;
    long totalRead = 0;

    while ((bytesRead = await file.OpenReadStream().ReadAsync(buffer, 0, buffer.Length)) > 0)
    {
        await fileStream.WriteAsync(buffer, 0, bytesRead);
        totalRead += bytesRead;

        var progress = (int)((totalRead * 100) / file.Length);
        // Report progress (e.g., via SignalR)
        await _hubContext.Clients.All.SendAsync("UploadProgress", progress);
    }

    return Ok();
}
```

### 5. **Custom JSON Serialization**
```csharp
// Custom JSON converter
public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd HH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString(), Format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}

// Registration
services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Polymorphic serialization
[JsonDerivedType(typeof(CreditCardPayment), "credit-card")]
[JsonDerivedType(typeof(PayPalPayment), "paypal")]
public abstract class Payment
{
    public decimal Amount { get; set; }
}

public class CreditCardPayment : Payment
{
    public string CardNumber { get; set; }
}

public class PayPalPayment : Payment
{
    public string Email { get; set; }
}
```

---

## Common Interview Questions

### Technical Questions

1. **Q: What's the difference between IEnumerable and IQueryable?**
   - IEnumerable: In-memory collection, LINQ to Objects
   - IQueryable: Query provider (DB), deferred execution with expression trees

2. **Q: Explain the difference between First() and Single()?**
   - First(): Returns first element, throws if empty
   - Single(): Returns only element, throws if empty OR more than one

3. **Q: What is the difference between Task.Run() and Task.Factory.StartNew()?**
   - Task.Run(): Simplified, always uses TaskScheduler.Default
   - Task.Factory.StartNew(): More options, but complex

4. **Q: How does garbage collection work in .NET?**
   - Generational collection (Gen 0, 1, 2)
   - Mark and sweep algorithm
   - Background GC for Gen 2

5. **Q: What is the difference between abstract class and interface?**
   - Abstract class: Can have implementation, single inheritance
   - Interface: Contract only (C# 8+ can have default implementations), multiple inheritance

### Design Questions

1. **Q: Design a URL shortener service**
2. **Q: Design a rate limiter**
3. **Q: How would you handle high-traffic scenarios?**
4. **Q: Design a caching layer for an e-commerce application**
5. **Q: How would you implement a notification system?**

### Behavioral Questions

1. **Q: Describe a challenging bug you fixed**
2. **Q: How do you stay updated with .NET technologies?**
3. **Q: Tell me about a time you improved application performance**
4. **Q: How do you handle technical debt?**

---

## Resources for Further Study

1. **Official Documentation**
   - Microsoft .NET Documentation
   - ASP.NET Core Documentation
   - Entity Framework Core Documentation

2. **Books**
   - "C# in Depth" by Jon Skeet
   - "CLR via C#" by Jeffrey Richter
   - "Pro ASP.NET Core" by Adam Freeman

3. **Online Platforms**
   - Pluralsight
   - Microsoft Learn
   - GitHub repositories

4. **Practice**
   - LeetCode for algorithms
   - Build real projects
   - Contribute to open source

---

## Tips for Interview Success

1. **Preparation**
   - Review fundamentals regularly
   - Practice coding problems
   - Understand system design principles
   - Prepare questions to ask interviewer

2. **During Interview**
   - Think out loud
   - Ask clarifying questions
   - Consider edge cases
   - Write clean, readable code
   - Test your solution

3. **Communication**
   - Explain your thought process
   - Discuss trade-offs
   - Be honest about what you don't know
   - Show willingness to learn

4. **Follow-up**
   - Send thank you email
   - Reflect on what went well/poorly
   - Continue learning regardless of outcome

Good luck with your .NET interviews!
