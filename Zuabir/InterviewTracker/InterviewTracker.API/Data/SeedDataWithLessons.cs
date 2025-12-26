using InterviewTracker.API.Models;

namespace InterviewTracker.API.Data;

public static class SeedDataWithLessons
{
    public static List<OOPTopic> GetOOPTopics()
    {
        return new List<OOPTopic>
        {
            // SOLID Principles
            new() {
                Title = "Single Responsibility Principle (SRP)",
                Category = "SOLID Principles",
                Difficulty = "Easy",
                KeyConcepts = "A class should have only one reason to change. Each class should do one thing well.",
                Lesson = @"# Single Responsibility Principle (SRP)

## Definition
The Single Responsibility Principle states that a class should have only one reason to change, meaning it should have only one job or responsibility.

## Why It Matters
- **Maintainability**: When a class has one responsibility, changes are localized
- **Testability**: Smaller, focused classes are easier to test
- **Reusability**: Single-purpose classes can be reused in different contexts
- **Readability**: Code is easier to understand when each class has a clear purpose

## Bad Example (Violating SRP)
```csharp
public class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }

    // Responsibility 1: Employee data
    public void UpdateName(string name) { Name = name; }

    // Responsibility 2: Database operations (WRONG!)
    public void SaveToDatabase() { /* SQL code here */ }

    // Responsibility 3: Report generation (WRONG!)
    public string GeneratePaySlip() { return $""Pay slip for {Name}""; }

    // Responsibility 4: Email sending (WRONG!)
    public void SendPaySlipEmail() { /* Email code here */ }
}
```

## Good Example (Following SRP)
```csharp
// Responsibility 1: Employee data only
public class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }
}

// Responsibility 2: Database operations
public class EmployeeRepository
{
    public void Save(Employee employee) { /* SQL code */ }
    public Employee GetById(int id) { /* SQL code */ }
}

// Responsibility 3: Report generation
public class PaySlipGenerator
{
    public string Generate(Employee employee)
    {
        return $""Pay slip for {employee.Name}: {employee.Salary}"";
    }
}

// Responsibility 4: Email sending
public class EmailService
{
    public void SendEmail(string to, string body) { /* Email code */ }
}
```

## How to Identify SRP Violations
1. **Multiple 'and' in description**: If you describe a class with 'and' (e.g., 'handles users AND sends emails'), it likely violates SRP
2. **Many imports**: If a class imports many unrelated namespaces
3. **Large classes**: Classes with hundreds of lines often have multiple responsibilities
4. **Difficulty naming**: If you can't give a clear, concise name to a class

## Interview Tips
- Be prepared to refactor a class that violates SRP
- Explain trade-offs: too many small classes can increase complexity
- Mention that SRP applies at different levels (methods, classes, modules)",
                CodeExample = @"// Bad: UserService doing too much
public class UserService
{
    public void CreateUser(User user) { }
    public void SendWelcomeEmail(User user) { }  // Should be EmailService
    public void LogUserActivity(User user) { }   // Should be LoggingService
    public string GenerateReport() { }           // Should be ReportService
}

// Good: Single responsibility
public class UserService
{
    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository) => _repository = repository;
    public void CreateUser(User user) => _repository.Add(user);
    public User GetUser(int id) => _repository.GetById(id);
}",
                Tags = new() { "SOLID", "Design", "Clean Code" }
            },

            new() {
                Title = "Open/Closed Principle (OCP)",
                Category = "SOLID Principles",
                Difficulty = "Medium",
                KeyConcepts = "Open for extension, closed for modification. Use abstraction and inheritance.",
                Lesson = @"# Open/Closed Principle (OCP)

## Definition
Software entities (classes, modules, functions) should be **open for extension** but **closed for modification**.

## What This Means
- **Open for extension**: You can add new functionality
- **Closed for modification**: You shouldn't change existing, tested code

## Why It Matters
- Reduces risk of breaking existing functionality
- Promotes use of abstractions
- Makes code more maintainable and scalable

## Bad Example (Violating OCP)
```csharp
public class AreaCalculator
{
    public double Calculate(object shape)
    {
        // Every new shape requires modifying this method!
        if (shape is Rectangle r)
            return r.Width * r.Height;
        else if (shape is Circle c)
            return Math.PI * c.Radius * c.Radius;
        else if (shape is Triangle t)  // Added later - MODIFICATION!
            return 0.5 * t.Base * t.Height;
        // What about Pentagon? Hexagon? More modifications needed!

        throw new ArgumentException(""Unknown shape"");
    }
}
```

## Good Example (Following OCP)
```csharp
// Define abstraction
public interface IShape
{
    double CalculateArea();
}

// Each shape implements its own area calculation
public class Rectangle : IShape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double CalculateArea() => Width * Height;
}

public class Circle : IShape
{
    public double Radius { get; set; }
    public double CalculateArea() => Math.PI * Radius * Radius;
}

// Adding new shapes doesn't require modifying existing code!
public class Triangle : IShape  // EXTENSION, not modification
{
    public double Base { get; set; }
    public double Height { get; set; }
    public double CalculateArea() => 0.5 * Base * Height;
}

// Calculator doesn't need to change for new shapes
public class AreaCalculator
{
    public double Calculate(IShape shape) => shape.CalculateArea();

    public double CalculateTotal(IEnumerable<IShape> shapes)
        => shapes.Sum(s => s.CalculateArea());
}
```

## Techniques to Achieve OCP
1. **Strategy Pattern**: Encapsulate algorithms
2. **Template Method**: Define skeleton, let subclasses fill in
3. **Decorator Pattern**: Add behavior dynamically
4. **Dependency Injection**: Inject different implementations

## Real-World Example: Payment Processing
```csharp
public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount);
}

public class CreditCardProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount) { /* Credit card logic */ }
}

public class PayPalProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount) { /* PayPal logic */ }
}

// Adding Bitcoin? Just create new class, no modifications needed!
public class BitcoinProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount) { /* Bitcoin logic */ }
}

public class PaymentService
{
    private readonly IPaymentProcessor _processor;

    public PaymentService(IPaymentProcessor processor)
    {
        _processor = processor;
    }

    public void Pay(decimal amount) => _processor.ProcessPayment(amount);
}
```

## Interview Tips
- Explain the relationship between OCP and polymorphism
- Discuss when it's acceptable to modify code (bug fixes, refactoring)
- Show how OCP relates to other SOLID principles",
                CodeExample = @"// Strategy Pattern for OCP
public interface IDiscountStrategy
{
    decimal CalculateDiscount(decimal price);
}

public class RegularDiscount : IDiscountStrategy
{
    public decimal CalculateDiscount(decimal price) => price * 0.1m;
}

public class PremiumDiscount : IDiscountStrategy
{
    public decimal CalculateDiscount(decimal price) => price * 0.2m;
}

public class PriceCalculator
{
    public decimal Calculate(decimal price, IDiscountStrategy strategy)
        => price - strategy.CalculateDiscount(price);
}",
                Tags = new() { "SOLID", "Extensibility", "Abstraction" }
            },

            new() {
                Title = "Liskov Substitution Principle (LSP)",
                Category = "SOLID Principles",
                Difficulty = "Medium",
                KeyConcepts = "Subtypes must be substitutable for their base types without altering program correctness.",
                Lesson = @"# Liskov Substitution Principle (LSP)

## Definition
Objects of a superclass should be replaceable with objects of its subclasses without affecting the correctness of the program.

## Named After
Barbara Liskov, who introduced this concept in 1987.

## The Rule
If S is a subtype of T, then objects of type T can be replaced with objects of type S without altering any of the desirable properties of the program.

## Classic Violation: Square/Rectangle Problem
```csharp
// This seems logical but violates LSP!
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public int Area => Width * Height;
}

public class Square : Rectangle
{
    private int _side;

    public override int Width
    {
        get => _side;
        set => _side = value;  // Also changes height!
    }

    public override int Height
    {
        get => _side;
        set => _side = value;  // Also changes width!
    }
}

// This code breaks with Square!
public void TestRectangle(Rectangle rect)
{
    rect.Width = 5;
    rect.Height = 4;

    // Expected: 20, but Square gives 16!
    Debug.Assert(rect.Area == 20); // FAILS for Square!
}
```

## Correct Approach
```csharp
public interface IShape
{
    int Area { get; }
}

public class Rectangle : IShape
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Area => Width * Height;
}

public class Square : IShape
{
    public int Side { get; set; }
    public int Area => Side * Side;
}
```

## LSP Rules
1. **Preconditions cannot be strengthened** in a subtype
2. **Postconditions cannot be weakened** in a subtype
3. **Invariants must be preserved** in subtypes
4. **History constraint**: Subtypes shouldn't modify state in unexpected ways

## More Violations

### Throwing Unexpected Exceptions
```csharp
public class Bird
{
    public virtual void Fly() { /* fly */ }
}

public class Penguin : Bird
{
    public override void Fly()
    {
        throw new NotSupportedException(""Penguins can't fly!"");
        // Violates LSP - callers expect all Birds to fly
    }
}
```

### Better Design
```csharp
public interface IBird { }

public interface IFlyingBird : IBird
{
    void Fly();
}

public class Sparrow : IFlyingBird
{
    public void Fly() { /* fly */ }
}

public class Penguin : IBird
{
    public void Swim() { /* swim */ }
}
```

## How to Detect LSP Violations
1. Type checking with `is` or `as` in client code
2. Empty method implementations
3. Throwing `NotImplementedException` or `NotSupportedException`
4. Overridden methods that do nothing

## Interview Tips
- Use the Square/Rectangle example
- Explain behavioral subtyping vs structural subtyping
- Show how LSP violations lead to fragile code",
                CodeExample = @"// LSP-compliant design
public interface IReadableStream
{
    byte[] Read(int count);
}

public interface IWritableStream
{
    void Write(byte[] data);
}

public interface IStream : IReadableStream, IWritableStream { }

// ReadOnlyStream only implements IReadableStream
public class ReadOnlyFileStream : IReadableStream
{
    public byte[] Read(int count) { /* read */ return new byte[count]; }
}

// Full stream implements both
public class FileStream : IStream
{
    public byte[] Read(int count) { return new byte[count]; }
    public void Write(byte[] data) { /* write */ }
}",
                Tags = new() { "SOLID", "Inheritance", "Polymorphism" }
            },

            new() {
                Title = "Interface Segregation Principle (ISP)",
                Category = "SOLID Principles",
                Difficulty = "Medium",
                KeyConcepts = "Clients shouldn't depend on interfaces they don't use. Prefer small, specific interfaces.",
                Lesson = @"# Interface Segregation Principle (ISP)

## Definition
No client should be forced to depend on interfaces it does not use. Many specific interfaces are better than one general-purpose interface.

## The Problem with Fat Interfaces
```csharp
// Fat interface - violates ISP
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}

// Robot can't eat or sleep!
public class Robot : IWorker
{
    public void Work() { /* work */ }
    public void Eat() { throw new NotImplementedException(); }  // Problem!
    public void Sleep() { throw new NotImplementedException(); } // Problem!
}

public class Human : IWorker
{
    public void Work() { /* work */ }
    public void Eat() { /* eat */ }
    public void Sleep() { /* sleep */ }
}
```

## Solution: Segregated Interfaces
```csharp
public interface IWorkable
{
    void Work();
}

public interface IFeedable
{
    void Eat();
}

public interface ISleepable
{
    void Sleep();
}

// Robot only implements what it can do
public class Robot : IWorkable
{
    public void Work() { /* work */ }
}

// Human implements all relevant interfaces
public class Human : IWorkable, IFeedable, ISleepable
{
    public void Work() { /* work */ }
    public void Eat() { /* eat */ }
    public void Sleep() { /* sleep */ }
}
```

## Real-World Example: Printer Interfaces
```csharp
// Bad: Fat interface
public interface IMachine
{
    void Print(Document d);
    void Scan(Document d);
    void Fax(Document d);
    void Staple(Document d);
}

// Old printer can only print!
public class OldPrinter : IMachine
{
    public void Print(Document d) { /* print */ }
    public void Scan(Document d) => throw new NotSupportedException();
    public void Fax(Document d) => throw new NotSupportedException();
    public void Staple(Document d) => throw new NotSupportedException();
}
```

```csharp
// Good: Segregated interfaces
public interface IPrinter { void Print(Document d); }
public interface IScanner { void Scan(Document d); }
public interface IFax { void Fax(Document d); }
public interface IStapler { void Staple(Document d); }

// Compose interfaces as needed
public interface IMultiFunctionDevice : IPrinter, IScanner, IFax { }

public class OldPrinter : IPrinter
{
    public void Print(Document d) { /* print */ }
}

public class ModernPrinter : IMultiFunctionDevice
{
    public void Print(Document d) { /* print */ }
    public void Scan(Document d) { /* scan */ }
    public void Fax(Document d) { /* fax */ }
}
```

## Benefits of ISP
1. **Decoupling**: Classes only depend on what they use
2. **Flexibility**: Easy to add new implementations
3. **Maintainability**: Changes in one interface don't affect unrelated classes
4. **Testability**: Smaller interfaces are easier to mock

## Signs of ISP Violation
- Methods that throw `NotImplementedException`
- Empty method implementations
- Interfaces with many methods (> 5-7 is a code smell)
- Classes that only use a few methods of an interface

## Interview Tips
- Compare to SRP (ISP is SRP for interfaces)
- Discuss role interfaces vs header interfaces
- Explain the relationship with Dependency Inversion",
                CodeExample = @"// Repository pattern with ISP
public interface IReadRepository<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
}

public interface IWriteRepository<T>
{
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> { }

// Read-only service only needs read operations
public class ReportService
{
    private readonly IReadRepository<Order> _repository;

    public ReportService(IReadRepository<Order> repository)
    {
        _repository = repository;
    }
}",
                Tags = new() { "SOLID", "Interfaces", "Decoupling" }
            },

            new() {
                Title = "Dependency Inversion Principle (DIP)",
                Category = "SOLID Principles",
                Difficulty = "Medium",
                KeyConcepts = "High-level modules shouldn't depend on low-level modules. Both should depend on abstractions.",
                Lesson = @"# Dependency Inversion Principle (DIP)

## Definition
1. High-level modules should not depend on low-level modules. Both should depend on abstractions.
2. Abstractions should not depend on details. Details should depend on abstractions.

## Understanding the Layers
- **High-level modules**: Business logic, policies, workflows
- **Low-level modules**: Database access, file I/O, network calls
- **Abstractions**: Interfaces, abstract classes

## The Problem Without DIP
```csharp
// Low-level module
public class SqlDatabase
{
    public void Save(string data) { /* SQL save */ }
}

// High-level module directly depends on low-level
public class UserService
{
    private SqlDatabase _database = new SqlDatabase(); // Tight coupling!

    public void CreateUser(string name)
    {
        _database.Save(name);  // Can't change database without modifying this
    }
}
```

## Problems:
- Can't switch to MongoDB without changing UserService
- Can't unit test UserService without a real database
- Changes in SqlDatabase might break UserService

## Solution with DIP
```csharp
// Abstraction (owned by high-level module)
public interface IDatabase
{
    void Save(string data);
}

// Low-level module depends on abstraction
public class SqlDatabase : IDatabase
{
    public void Save(string data) { /* SQL implementation */ }
}

public class MongoDatabase : IDatabase
{
    public void Save(string data) { /* MongoDB implementation */ }
}

// High-level module depends on abstraction
public class UserService
{
    private readonly IDatabase _database;

    public UserService(IDatabase database)  // Dependency Injection
    {
        _database = database;
    }

    public void CreateUser(string name)
    {
        _database.Save(name);
    }
}
```

## The Dependency Flow

### Without DIP:
```
UserService (high-level) → SqlDatabase (low-level)
```

### With DIP:
```
UserService (high-level) → IDatabase (abstraction) ← SqlDatabase (low-level)
```

The dependency arrow is INVERTED!

## Dependency Injection Types
```csharp
// 1. Constructor Injection (preferred)
public class OrderService
{
    private readonly IRepository _repo;
    public OrderService(IRepository repo) => _repo = repo;
}

// 2. Property Injection
public class OrderService
{
    public IRepository Repository { get; set; }
}

// 3. Method Injection
public class OrderService
{
    public void ProcessOrder(Order order, IRepository repo)
    {
        repo.Save(order);
    }
}
```

## DI Container Example (ASP.NET Core)
```csharp
// Startup.cs / Program.cs
services.AddScoped<IUserRepository, SqlUserRepository>();
services.AddScoped<IEmailService, SmtpEmailService>();
services.AddScoped<IUserService, UserService>();

// UserService automatically gets dependencies injected
public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IEmailService _email;

    public UserService(IUserRepository repo, IEmailService email)
    {
        _repo = repo;
        _email = email;
    }
}
```

## Benefits
1. **Loose coupling**: Easy to swap implementations
2. **Testability**: Inject mocks for unit testing
3. **Flexibility**: Configure dependencies at runtime
4. **Parallel development**: Teams work on interfaces

## Interview Tips
- Explain DIP vs DI vs IoC (Inversion of Control)
- Show how to use DI containers
- Discuss service lifetimes (Singleton, Scoped, Transient)",
                CodeExample = @"// Complete DIP example with testing
public interface INotificationService
{
    void Send(string message);
}

public class EmailNotification : INotificationService
{
    public void Send(string message) => Console.WriteLine($""Email: {message}"");
}

public class SmsNotification : INotificationService
{
    public void Send(string message) => Console.WriteLine($""SMS: {message}"");
}

public class OrderProcessor
{
    private readonly INotificationService _notification;

    public OrderProcessor(INotificationService notification)
    {
        _notification = notification;
    }

    public void Process(Order order)
    {
        // Process order...
        _notification.Send($""Order {order.Id} processed"");
    }
}

// In tests - inject mock
var mockNotification = new Mock<INotificationService>();
var processor = new OrderProcessor(mockNotification.Object);",
                Tags = new() { "SOLID", "DI", "IoC", "Abstraction" }
            },

            // Four Pillars of OOP
            new() {
                Title = "Encapsulation",
                Category = "Four Pillars",
                Difficulty = "Easy",
                KeyConcepts = "Bundling data and methods, hiding internal state. Access modifiers: public, private, protected, internal.",
                Lesson = @"# Encapsulation

## Definition
Encapsulation is the bundling of data (fields) and methods that operate on that data within a single unit (class), and restricting direct access to some of the object's components.

## Two Aspects
1. **Data Hiding**: Keep implementation details private
2. **Data Bundling**: Group related data and behavior together

## Access Modifiers in C#
| Modifier | Same Class | Same Assembly | Derived Class | Everywhere |
|----------|------------|---------------|---------------|------------|
| private | ✓ | ✗ | ✗ | ✗ |
| protected | ✓ | ✗ | ✓ | ✗ |
| internal | ✓ | ✓ | ✗ | ✗ |
| protected internal | ✓ | ✓ | ✓ | ✗ |
| private protected | ✓ | ✗ | ✓* | ✗ |
| public | ✓ | ✓ | ✓ | ✓ |

*Only in same assembly

## Bad Example (No Encapsulation)
```csharp
public class BankAccount
{
    public decimal Balance;  // Public field - anyone can modify!
}

// Problems:
var account = new BankAccount();
account.Balance = -1000;  // Invalid state!
account.Balance = 999999999;  // No validation!
```

## Good Example (Proper Encapsulation)
```csharp
public class BankAccount
{
    private decimal _balance;  // Private field

    public decimal Balance => _balance;  // Read-only property

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException(""Amount must be positive"");
        _balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException(""Amount must be positive"");
        if (amount > _balance)
            return false;  // Insufficient funds
        _balance -= amount;
        return true;
    }
}
```

## Properties in C#
```csharp
public class Person
{
    // Auto-property
    public string Name { get; set; }

    // Read-only property
    public DateTime CreatedAt { get; } = DateTime.Now;

    // Property with backing field
    private int _age;
    public int Age
    {
        get => _age;
        set
        {
            if (value < 0 || value > 150)
                throw new ArgumentException(""Invalid age"");
            _age = value;
        }
    }

    // Init-only property (C# 9+)
    public string Id { get; init; }

    // Computed property
    public bool IsAdult => Age >= 18;
}
```

## Benefits of Encapsulation
1. **Control**: You control how data is accessed/modified
2. **Validation**: Enforce business rules
3. **Flexibility**: Change internal implementation without affecting users
4. **Security**: Hide sensitive data
5. **Maintainability**: Reduce complexity

## Interview Tips
- Explain difference between fields and properties
- Discuss when to use each access modifier
- Show validation in property setters",
                CodeExample = @"public class Employee
{
    // Private fields
    private string _email;
    private decimal _salary;

    // Public property with validation
    public string Email
    {
        get => _email;
        set
        {
            if (!value.Contains(""@""))
                throw new ArgumentException(""Invalid email"");
            _email = value;
        }
    }

    // Protected - accessible in derived classes
    protected decimal Salary
    {
        get => _salary;
        set => _salary = value > 0 ? value : throw new ArgumentException();
    }

    // Internal - accessible in same assembly
    internal string EmployeeCode { get; set; }
}",
                Tags = new() { "OOP", "Fundamentals", "Access Modifiers" }
            },

            new() {
                Title = "Abstraction",
                Category = "Four Pillars",
                Difficulty = "Easy",
                KeyConcepts = "Hiding complex implementation details, showing only essential features. Abstract classes and interfaces.",
                Lesson = @"# Abstraction

## Definition
Abstraction is the process of hiding complex implementation details and showing only the necessary features of an object. It reduces complexity by exposing only relevant information.

## Real-World Analogy
- **Car**: You know how to drive (steering, pedals, gear) but don't need to know how the engine works internally
- **TV Remote**: Press buttons to change channels without knowing signal transmission details
- **ATM**: Withdraw money without knowing banking backend systems

## Abstraction in C#

### Using Abstract Classes
```csharp
public abstract class Shape
{
    public string Color { get; set; }

    // Abstract method - no implementation, must be overridden
    public abstract double CalculateArea();

    // Concrete method - has implementation
    public void Display()
    {
        Console.WriteLine($""Shape color: {Color}, Area: {CalculateArea()}"");
    }
}

public class Circle : Shape
{
    public double Radius { get; set; }

    public override double CalculateArea()
    {
        return Math.PI * Radius * Radius;
    }
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public override double CalculateArea()
    {
        return Width * Height;
    }
}
```

### Using Interfaces
```csharp
public interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount);
    void Refund(string transactionId);
}

// Implementation details hidden
public class StripePaymentProcessor : IPaymentProcessor
{
    private readonly StripeClient _client;

    public bool ProcessPayment(decimal amount)
    {
        // Complex Stripe API calls hidden from user
        var charge = _client.CreateCharge(amount);
        return charge.Status == ""succeeded"";
    }

    public void Refund(string transactionId)
    {
        _client.CreateRefund(transactionId);
    }
}
```

## Abstract Class vs Interface

| Feature | Abstract Class | Interface |
|---------|---------------|-----------|
| Multiple inheritance | No | Yes |
| Constructor | Yes | No |
| Fields | Yes | No (constants only) |
| Access modifiers | Any | Public by default |
| Default implementation | Yes | Yes (C# 8+) |
| When to use | IS-A + shared code | CAN-DO behavior |

## Levels of Abstraction
```csharp
// High-level abstraction
public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}

// Mid-level abstraction
public abstract class EmailServiceBase : IEmailService
{
    protected abstract Task SendInternalAsync(EmailMessage message);

    public async Task SendAsync(string to, string subject, string body)
    {
        var message = new EmailMessage(to, subject, body);
        ValidateMessage(message);
        await SendInternalAsync(message);
        LogSent(message);
    }

    private void ValidateMessage(EmailMessage msg) { /* validation */ }
    private void LogSent(EmailMessage msg) { /* logging */ }
}

// Low-level implementation
public class SmtpEmailService : EmailServiceBase
{
    protected override async Task SendInternalAsync(EmailMessage message)
    {
        using var client = new SmtpClient();
        // SMTP-specific code
        await client.SendMailAsync(/* ... */);
    }
}
```

## Benefits
1. Reduces complexity
2. Enables code reuse
3. Provides security (hides implementation)
4. Supports loose coupling

## Interview Tips
- Explain difference from Encapsulation
- Show when to use abstract class vs interface
- Discuss abstraction levels in architecture",
                CodeExample = @"// Database abstraction example
public interface IRepository<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

// SQL Server implementation
public class SqlRepository<T> : IRepository<T>
{
    private readonly DbContext _context;
    // Implementation using Entity Framework
}

// MongoDB implementation
public class MongoRepository<T> : IRepository<T>
{
    private readonly IMongoCollection<T> _collection;
    // Implementation using MongoDB driver
}

// Service doesn't know or care about database details
public class ProductService
{
    private readonly IRepository<Product> _repo;
    public ProductService(IRepository<Product> repo) => _repo = repo;
}",
                Tags = new() { "OOP", "Fundamentals", "Design" }
            },

            new() {
                Title = "Inheritance",
                Category = "Four Pillars",
                Difficulty = "Easy",
                KeyConcepts = "Creating new classes from existing ones. IS-A relationship. Base and derived classes.",
                Lesson = @"# Inheritance

## Definition
Inheritance is a mechanism where a new class (derived/child) inherits properties and methods from an existing class (base/parent). It establishes an IS-A relationship.

## Basic Syntax
```csharp
// Base class (parent)
public class Animal
{
    public string Name { get; set; }

    public void Eat()
    {
        Console.WriteLine($""{Name} is eating"");
    }

    public virtual void MakeSound()
    {
        Console.WriteLine(""Some sound"");
    }
}

// Derived class (child)
public class Dog : Animal
{
    public string Breed { get; set; }

    public void Fetch()
    {
        Console.WriteLine($""{Name} is fetching"");
    }

    public override void MakeSound()
    {
        Console.WriteLine(""Woof!"");
    }
}

// Usage
var dog = new Dog { Name = ""Buddy"", Breed = ""Golden Retriever"" };
dog.Eat();       // Inherited from Animal
dog.MakeSound(); // Overridden in Dog
dog.Fetch();     // Defined in Dog
```

## Types of Inheritance

### Single Inheritance
```csharp
public class Animal { }
public class Dog : Animal { }  // Dog inherits from Animal
```

### Multilevel Inheritance
```csharp
public class Animal { }
public class Mammal : Animal { }
public class Dog : Mammal { }  // Dog → Mammal → Animal
```

### Hierarchical Inheritance
```csharp
public class Animal { }
public class Dog : Animal { }
public class Cat : Animal { }  // Both inherit from Animal
```

### Multiple Inheritance (via Interfaces)
```csharp
public interface ISwimmable { void Swim(); }
public interface IFlyable { void Fly(); }

public class Duck : ISwimmable, IFlyable
{
    public void Swim() { }
    public void Fly() { }
}
```

## Important Keywords

### `virtual` and `override`
```csharp
public class Shape
{
    public virtual double Area() => 0;  // Can be overridden
}

public class Circle : Shape
{
    public double Radius { get; set; }
    public override double Area() => Math.PI * Radius * Radius;
}
```

### `sealed` - Prevent further inheritance
```csharp
public sealed class FinalClass { }  // Cannot be inherited
// public class Child : FinalClass { }  // Compile error!

public class Parent
{
    public virtual void Method() { }
}

public class Child : Parent
{
    public sealed override void Method() { }  // Cannot be overridden further
}
```

### `base` - Access parent members
```csharp
public class Employee
{
    public virtual decimal CalculateSalary() => 50000;
}

public class Manager : Employee
{
    public override decimal CalculateSalary()
    {
        return base.CalculateSalary() + 20000;  // Call parent method
    }
}
```

## Constructor Chaining
```csharp
public class Person
{
    public string Name { get; }

    public Person(string name)
    {
        Name = name;
    }
}

public class Employee : Person
{
    public string Department { get; }

    public Employee(string name, string department)
        : base(name)  // Call parent constructor
    {
        Department = department;
    }
}
```

## When to Use Inheritance
- ✅ Clear IS-A relationship (Dog IS-A Animal)
- ✅ Share common functionality
- ✅ Polymorphic behavior needed

## When NOT to Use
- ❌ Just to reuse code (use composition)
- ❌ HAS-A relationship (Car HAS-A Engine)
- ❌ Deep inheritance hierarchies (>3 levels)

## Favor Composition Over Inheritance
```csharp
// Instead of inheriting...
public class Car : Engine { }  // Wrong! Car is not an Engine

// Use composition
public class Car
{
    private readonly Engine _engine;
    public Car(Engine engine) => _engine = engine;
}
```",
                CodeExample = @"// Real-world inheritance example
public abstract class Employee
{
    public string Name { get; set; }
    public decimal BaseSalary { get; set; }

    public abstract decimal CalculateBonus();

    public decimal GetTotalCompensation()
    {
        return BaseSalary + CalculateBonus();
    }
}

public class Developer : Employee
{
    public string ProgrammingLanguage { get; set; }

    public override decimal CalculateBonus()
    {
        return BaseSalary * 0.15m;  // 15% bonus
    }
}

public class Manager : Employee
{
    public int TeamSize { get; set; }

    public override decimal CalculateBonus()
    {
        return BaseSalary * 0.20m + (TeamSize * 1000);
    }
}",
                Tags = new() { "OOP", "Fundamentals", "Reusability" }
            },

            new() {
                Title = "Polymorphism",
                Category = "Four Pillars",
                Difficulty = "Medium",
                KeyConcepts = "Same interface, different implementations. Method overriding, method overloading, virtual methods.",
                Lesson = @"# Polymorphism

## Definition
Polymorphism means ""many forms"". It allows objects of different classes to be treated as objects of a common base class. The same method call can behave differently depending on the object that receives it.

## Types of Polymorphism

### 1. Compile-Time (Static) Polymorphism
Resolved at compile time through method overloading and operator overloading.

```csharp
public class Calculator
{
    // Method Overloading - same name, different parameters
    public int Add(int a, int b) => a + b;
    public double Add(double a, double b) => a + b;
    public int Add(int a, int b, int c) => a + b + c;
    public string Add(string a, string b) => a + b;
}

var calc = new Calculator();
calc.Add(1, 2);           // Calls int version
calc.Add(1.5, 2.5);       // Calls double version
calc.Add(1, 2, 3);        // Calls three-parameter version
```

### 2. Run-Time (Dynamic) Polymorphism
Resolved at runtime through method overriding using `virtual` and `override`.

```csharp
public class Animal
{
    public virtual void Speak()
    {
        Console.WriteLine(""Animal speaks"");
    }
}

public class Dog : Animal
{
    public override void Speak()
    {
        Console.WriteLine(""Woof!"");
    }
}

public class Cat : Animal
{
    public override void Speak()
    {
        Console.WriteLine(""Meow!"");
    }
}

// Runtime polymorphism in action
Animal[] animals = { new Dog(), new Cat(), new Animal() };
foreach (var animal in animals)
{
    animal.Speak();  // Output depends on actual object type
}
// Output: Woof! Meow! Animal speaks
```

## Virtual Table (vtable)
When you use `virtual` methods, C# creates a virtual method table for each class. At runtime, the CLR looks up the correct method to call based on the actual object type.

## Polymorphism with Interfaces
```csharp
public interface IShape
{
    double CalculateArea();
}

public class Circle : IShape
{
    public double Radius { get; set; }
    public double CalculateArea() => Math.PI * Radius * Radius;
}

public class Rectangle : IShape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double CalculateArea() => Width * Height;
}

// Polymorphic usage
public double TotalArea(IEnumerable<IShape> shapes)
{
    return shapes.Sum(s => s.CalculateArea());
}
```

## Method Hiding vs Overriding
```csharp
public class Parent
{
    public virtual void VirtualMethod() => Console.WriteLine(""Parent Virtual"");
    public void NormalMethod() => Console.WriteLine(""Parent Normal"");
}

public class Child : Parent
{
    public override void VirtualMethod() => Console.WriteLine(""Child Override"");
    public new void NormalMethod() => Console.WriteLine(""Child New"");  // Hiding
}

Child child = new Child();
Parent parent = child;

child.VirtualMethod();   // ""Child Override""
parent.VirtualMethod();  // ""Child Override"" (polymorphism!)

child.NormalMethod();    // ""Child New""
parent.NormalMethod();   // ""Parent Normal"" (no polymorphism!)
```

## Operator Overloading
```csharp
public class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException(""Cannot add different currencies"");
        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static bool operator ==(Money a, Money b)
        => a.Amount == b.Amount && a.Currency == b.Currency;

    public static bool operator !=(Money a, Money b) => !(a == b);
}

var total = new Money(10, ""USD"") + new Money(20, ""USD"");  // Money(30, ""USD"")
```

## Benefits of Polymorphism
1. **Flexibility**: Write code that works with base types
2. **Extensibility**: Add new types without changing existing code
3. **Maintainability**: Reduce conditional logic
4. **Testability**: Easy to mock and substitute

## Interview Questions
- Difference between overloading and overriding?
- What is the virtual table (vtable)?
- Can you override a static method?
- Difference between `new` and `override`?",
                CodeExample = @"// Polymorphism eliminating if-else chains
// Without polymorphism (bad)
public decimal CalculateDiscount(string customerType, decimal amount)
{
    if (customerType == ""Regular"") return amount * 0.1m;
    if (customerType == ""Premium"") return amount * 0.2m;
    if (customerType == ""VIP"") return amount * 0.3m;
    return 0;
}

// With polymorphism (good)
public interface ICustomer
{
    decimal GetDiscountRate();
}

public class RegularCustomer : ICustomer
{
    public decimal GetDiscountRate() => 0.1m;
}

public class PremiumCustomer : ICustomer
{
    public decimal GetDiscountRate() => 0.2m;
}

public class VipCustomer : ICustomer
{
    public decimal GetDiscountRate() => 0.3m;
}

public decimal CalculateDiscount(ICustomer customer, decimal amount)
{
    return amount * customer.GetDiscountRate();
}",
                Tags = new() { "OOP", "Fundamentals", "Runtime" }
            }
        };
    }

    public static List<CSharpTopic> GetCSharpTopics()
    {
        return new List<CSharpTopic>
        {
            new() {
                Title = "Value Types vs Reference Types",
                Category = "Fundamentals",
                Difficulty = "Easy",
                KeyConcepts = "Stack vs heap allocation, struct vs class, boxing/unboxing, nullable value types",
                DotNetVersion = "1.0",
                Lesson = @"# Value Types vs Reference Types

## Overview
Understanding the difference between value types and reference types is fundamental to C# programming and affects performance, memory usage, and behavior.

## Value Types
Stored on the **stack** (or inline in containing type). Contains the actual data.

### Built-in Value Types
- `int`, `long`, `short`, `byte` (integers)
- `float`, `double`, `decimal` (floating-point)
- `bool` (boolean)
- `char` (character)
- `DateTime`, `TimeSpan`
- `Guid`

### Custom Value Types (struct)
```csharp
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}
```

## Reference Types
Stored on the **heap**. Variable holds a reference (pointer) to the data.

### Built-in Reference Types
- `string`
- `object`
- `dynamic`
- Arrays

### Custom Reference Types (class)
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
```

## Key Differences

| Feature | Value Type | Reference Type |
|---------|------------|----------------|
| Storage | Stack | Heap |
| Contains | Actual value | Reference to value |
| Default | 0/false/null | null |
| Assignment | Copies value | Copies reference |
| Comparison | By value | By reference |
| Inheritance | Cannot inherit | Can inherit |
| Can be null | No (unless nullable) | Yes |

## Assignment Behavior
```csharp
// Value Type - copies the value
int a = 10;
int b = a;  // b is a COPY of a
b = 20;
Console.WriteLine(a);  // Still 10!

// Reference Type - copies the reference
var person1 = new Person { Name = ""John"" };
var person2 = person1;  // person2 points to SAME object
person2.Name = ""Jane"";
Console.WriteLine(person1.Name);  // ""Jane"" - both changed!
```

## Boxing and Unboxing
Converting between value types and reference types.

```csharp
// Boxing: value type → object (heap allocation!)
int number = 42;
object boxed = number;  // Boxing occurs

// Unboxing: object → value type
int unboxed = (int)boxed;  // Unboxing occurs

// Performance impact - avoid in hot paths!
List<object> list = new List<object>();
for (int i = 0; i < 1000000; i++)
{
    list.Add(i);  // Boxing 1 million times!
}
```

## Nullable Value Types
```csharp
int? nullableInt = null;  // Nullable<int>
int? value = 42;

// Null checking
if (nullableInt.HasValue)
{
    int actual = nullableInt.Value;
}

// Null coalescing
int result = nullableInt ?? 0;

// Null-conditional
int? length = nullableInt?.ToString().Length;
```

## When to Use struct vs class

### Use struct when:
- Represents a single value (like int)
- Instance size < 16 bytes
- Immutable
- Won't be boxed frequently
- No inheritance needed

### Use class when:
- Larger objects
- Need inheritance
- Reference semantics needed
- Mutable state
- Need null value

## Record Types (C# 9+)
```csharp
// Record class (reference type with value semantics)
public record Person(string Name, int Age);

// Record struct (C# 10+)
public record struct Point(int X, int Y);
```

## Memory Visualization
```
Stack                    Heap
┌─────────────────┐     ┌─────────────────┐
│ int x = 42      │     │                 │
│ [42]            │     │                 │
├─────────────────┤     │                 │
│ Person p = new  │ ──► │ Person object   │
│ [reference]     │     │ Name: ""John""   │
│                 │     │ Age: 30         │
└─────────────────┘     └─────────────────┘
```",
                CodeExample = @"// Demonstrating value vs reference semantics
public struct PointStruct { public int X, Y; }
public class PointClass { public int X, Y; }

// Value type behavior
var structPoint1 = new PointStruct { X = 1, Y = 2 };
var structPoint2 = structPoint1;  // Copy
structPoint2.X = 100;
Console.WriteLine(structPoint1.X);  // 1 (unchanged)

// Reference type behavior
var classPoint1 = new PointClass { X = 1, Y = 2 };
var classPoint2 = classPoint1;  // Same reference
classPoint2.X = 100;
Console.WriteLine(classPoint1.X);  // 100 (changed!)

// Passing to methods
void ModifyStruct(PointStruct p) { p.X = 999; }
void ModifyClass(PointClass p) { p.X = 999; }

ModifyStruct(structPoint1);
Console.WriteLine(structPoint1.X);  // Still original

ModifyClass(classPoint1);
Console.WriteLine(classPoint1.X);  // 999!",
                Tags = new() { "Types", "Memory", "Fundamentals" }
            },

            new() {
                Title = "async/await Fundamentals",
                Category = "Async",
                Difficulty = "Medium",
                KeyConcepts = "Task, Task<T>, async methods, await keyword, ConfigureAwait, ValueTask",
                DotNetVersion = "5.0",
                Lesson = @"# async/await Fundamentals

## What is Asynchronous Programming?
Asynchronous programming allows your application to perform non-blocking operations, improving responsiveness and scalability.

## Synchronous vs Asynchronous
```csharp
// Synchronous - blocks the thread
public string GetDataSync()
{
    Thread.Sleep(2000);  // Thread blocked for 2 seconds
    return ""data"";
}

// Asynchronous - doesn't block
public async Task<string> GetDataAsync()
{
    await Task.Delay(2000);  // Thread released during wait
    return ""data"";
}
```

## Task and Task<T>
```csharp
// Task - no return value
public async Task DoWorkAsync()
{
    await Task.Delay(1000);
    Console.WriteLine(""Work done"");
}

// Task<T> - returns a value
public async Task<int> CalculateAsync()
{
    await Task.Delay(1000);
    return 42;
}
```

## async/await Keywords
```csharp
// async - marks method as asynchronous
// await - suspends execution until task completes
public async Task<string> FetchDataAsync()
{
    using var client = new HttpClient();

    // await suspends here, thread returns to pool
    string data = await client.GetStringAsync(""https://api.example.com/data"");

    // Execution resumes here when data is ready
    return data.ToUpper();
}
```

## Best Practices

### 1. Always await async methods
```csharp
// BAD - fire and forget (exceptions lost!)
public void BadMethod()
{
    DoWorkAsync();  // Not awaited!
}

// GOOD
public async Task GoodMethod()
{
    await DoWorkAsync();
}
```

### 2. Use async all the way
```csharp
// BAD - blocking on async (can cause deadlocks!)
public string Bad()
{
    return GetDataAsync().Result;  // Blocks!
}

// GOOD - async all the way up
public async Task<string> Good()
{
    return await GetDataAsync();
}
```

### 3. ConfigureAwait
```csharp
// In libraries, use ConfigureAwait(false)
public async Task<string> LibraryMethodAsync()
{
    var data = await httpClient.GetStringAsync(url)
        .ConfigureAwait(false);  // Don't capture context
    return data;
}

// In UI apps, omit or use true (need UI context)
public async Task ButtonClickAsync()
{
    var data = await GetDataAsync();  // Captures UI context
    label.Text = data;  // Safe to update UI
}
```

## Multiple Async Operations

### Sequential
```csharp
public async Task<int> SequentialAsync()
{
    var result1 = await Task1Async();  // Wait for this
    var result2 = await Task2Async();  // Then this
    return result1 + result2;
}
```

### Parallel
```csharp
public async Task<int> ParallelAsync()
{
    var task1 = Task1Async();  // Start both
    var task2 = Task2Async();

    await Task.WhenAll(task1, task2);  // Wait for both

    return task1.Result + task2.Result;
}

// Or with WhenAny
public async Task<string> FirstToCompleteAsync()
{
    var task1 = Api1Async();
    var task2 = Api2Async();

    var winner = await Task.WhenAny(task1, task2);
    return await winner;
}
```

## Cancellation
```csharp
public async Task<string> CancellableAsync(CancellationToken token)
{
    // Check for cancellation
    token.ThrowIfCancellationRequested();

    // Pass token to async operations
    var response = await httpClient.GetAsync(url, token);

    // Periodic check in loops
    for (int i = 0; i < 100; i++)
    {
        token.ThrowIfCancellationRequested();
        await ProcessItemAsync(i);
    }

    return ""done"";
}

// Usage
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
try
{
    await CancellableAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine(""Operation cancelled"");
}
```

## ValueTask (Performance)
```csharp
// Use when result is often cached/synchronous
public ValueTask<int> GetCachedValueAsync()
{
    if (_cache.TryGetValue(key, out int value))
        return new ValueTask<int>(value);  // No allocation!

    return new ValueTask<int>(FetchFromDbAsync());
}
```

## Common Mistakes
1. Using `.Result` or `.Wait()` (deadlocks!)
2. `async void` (only for event handlers)
3. Not using ConfigureAwait in libraries
4. Forgetting to await
5. Not handling exceptions properly",
                CodeExample = @"// Complete async example with error handling
public class DataService
{
    private readonly HttpClient _client;

    public async Task<Result<User>> GetUserAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var response = await _client
                .GetAsync($""/users/{id}"", ct)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return Result<User>.Fail($""HTTP {response.StatusCode}"");

            var user = await response.Content
                .ReadFromJsonAsync<User>(ct)
                .ConfigureAwait(false);

            return Result<User>.Success(user);
        }
        catch (OperationCanceledException)
        {
            return Result<User>.Fail(""Cancelled"");
        }
        catch (HttpRequestException ex)
        {
            return Result<User>.Fail(ex.Message);
        }
    }
}",
                Tags = new() { "Async", "TPL", "Performance" }
            },

            new() {
                Title = "LINQ Basics",
                Category = "LINQ",
                Difficulty = "Medium",
                KeyConcepts = "Query syntax vs method syntax, Where, Select, OrderBy, GroupBy, Join",
                DotNetVersion = "3.5",
                Lesson = @"# LINQ Basics

## What is LINQ?
Language Integrated Query (LINQ) provides a consistent query syntax for various data sources (collections, databases, XML, etc.).

## Two Syntaxes

### Query Syntax (SQL-like)
```csharp
var query = from person in people
            where person.Age > 18
            orderby person.Name
            select person.Name;
```

### Method Syntax (Lambda-based)
```csharp
var query = people
    .Where(p => p.Age > 18)
    .OrderBy(p => p.Name)
    .Select(p => p.Name);
```

Both produce identical results. Method syntax is more common.

## Core LINQ Methods

### Filtering: Where
```csharp
var adults = people.Where(p => p.Age >= 18);

// Multiple conditions
var seniorDevs = employees
    .Where(e => e.Age > 50 && e.Department == ""IT"");
```

### Projection: Select
```csharp
// Transform each element
var names = people.Select(p => p.Name);

// Create anonymous type
var summaries = people.Select(p => new { p.Name, p.Age });

// With index
var indexed = people.Select((p, i) => $""{i}: {p.Name}"");
```

### SelectMany: Flatten nested collections
```csharp
var allOrders = customers.SelectMany(c => c.Orders);

// With result selector
var orderDetails = customers.SelectMany(
    c => c.Orders,
    (customer, order) => new { customer.Name, order.Total }
);
```

### Ordering
```csharp
var sorted = people.OrderBy(p => p.Name);
var descending = people.OrderByDescending(p => p.Age);

// Multiple sort criteria
var multiSort = people
    .OrderBy(p => p.LastName)
    .ThenBy(p => p.FirstName);
```

### Grouping: GroupBy
```csharp
var byDepartment = employees.GroupBy(e => e.Department);

foreach (var group in byDepartment)
{
    Console.WriteLine($""Department: {group.Key}"");
    foreach (var emp in group)
        Console.WriteLine($""  {emp.Name}"");
}

// With element selector
var namesByDept = employees.GroupBy(
    e => e.Department,
    e => e.Name
);
```

### Joining
```csharp
// Inner Join
var query = orders.Join(
    customers,
    order => order.CustomerId,
    customer => customer.Id,
    (order, customer) => new { order.Total, customer.Name }
);

// Group Join (left join)
var leftJoin = customers.GroupJoin(
    orders,
    c => c.Id,
    o => o.CustomerId,
    (customer, orders) => new { customer.Name, OrderCount = orders.Count() }
);
```

### Aggregation
```csharp
var count = people.Count();
var adults = people.Count(p => p.Age >= 18);

var sum = orders.Sum(o => o.Total);
var avg = orders.Average(o => o.Total);
var max = orders.Max(o => o.Total);
var min = orders.Min(o => o.Total);

// Aggregate for custom operations
var product = numbers.Aggregate((a, b) => a * b);
```

### Element Operations
```csharp
var first = people.First();           // Throws if empty
var firstOrNull = people.FirstOrDefault();  // null if empty
var firstAdult = people.First(p => p.Age >= 18);

var single = people.Single(p => p.Id == 1);  // Throws if not exactly one
var singleOrNull = people.SingleOrDefault(p => p.Id == 1);

var last = people.Last();
```

### Set Operations
```csharp
var distinct = numbers.Distinct();
var union = list1.Union(list2);
var intersect = list1.Intersect(list2);
var except = list1.Except(list2);  // In list1 but not list2
```

### Quantifiers
```csharp
bool anyAdults = people.Any(p => p.Age >= 18);
bool allAdults = people.All(p => p.Age >= 18);
bool contains = numbers.Contains(42);
```

## Deferred Execution
LINQ queries don't execute until you iterate or call a terminal method.

```csharp
var query = people.Where(p => p.Age > 18);  // Not executed yet!

// Executes when iterating
foreach (var person in query) { }

// Or with terminal methods
var list = query.ToList();      // Executes
var array = query.ToArray();    // Executes
var count = query.Count();      // Executes
```

## Method Chaining
```csharp
var result = products
    .Where(p => p.InStock)
    .OrderByDescending(p => p.Rating)
    .Take(10)
    .Select(p => new { p.Name, p.Price })
    .ToList();
```",
                CodeExample = @"// Real-world LINQ examples
public class OrderService
{
    public decimal GetTotalRevenue(IEnumerable<Order> orders)
        => orders.Sum(o => o.Total);

    public IEnumerable<CustomerSummary> GetTopCustomers(
        IEnumerable<Order> orders, int count)
    {
        return orders
            .GroupBy(o => o.CustomerId)
            .Select(g => new CustomerSummary
            {
                CustomerId = g.Key,
                TotalSpent = g.Sum(o => o.Total),
                OrderCount = g.Count()
            })
            .OrderByDescending(c => c.TotalSpent)
            .Take(count);
    }

    public IEnumerable<Order> SearchOrders(
        IEnumerable<Order> orders,
        string? status = null,
        DateTime? fromDate = null)
    {
        var query = orders.AsEnumerable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(o => o.Status == status);

        if (fromDate.HasValue)
            query = query.Where(o => o.Date >= fromDate);

        return query.OrderByDescending(o => o.Date);
    }
}",
                Tags = new() { "LINQ", "Querying", "Collections" }
            },

            new() {
                Title = "Delegates and Events",
                Category = "Advanced",
                Difficulty = "Medium",
                KeyConcepts = "Delegates, multicast delegates, events, EventHandler, Action, Func, Predicate",
                DotNetVersion = "1.0",
                Lesson = @"# Delegates and Events

## What is a Delegate?
A delegate is a type-safe function pointer that holds a reference to a method.

## Defining Delegates
```csharp
// Custom delegate declaration
public delegate int MathOperation(int a, int b);

// Using the delegate
MathOperation add = (a, b) => a + b;
MathOperation multiply = (a, b) => a * b;

int result1 = add(5, 3);       // 8
int result2 = multiply(5, 3);  // 15
```

## Built-in Delegates

### Action - No return value
```csharp
// Action with no parameters
Action greet = () => Console.WriteLine(""Hello!"");

// Action with parameters
Action<string> greetPerson = name => Console.WriteLine($""Hello, {name}!"");
Action<int, int> printSum = (a, b) => Console.WriteLine(a + b);

greet();                    // Hello!
greetPerson(""John"");       // Hello, John!
printSum(5, 3);             // 8
```

### Func - Returns a value
```csharp
// Func<TResult> - no params, returns TResult
Func<int> getRandomNumber = () => new Random().Next(100);

// Func<T, TResult> - one param, returns TResult
Func<int, int> square = x => x * x;

// Func<T1, T2, TResult> - two params, returns TResult
Func<int, int, int> add = (a, b) => a + b;

int random = getRandomNumber();  // 0-99
int squared = square(5);         // 25
int sum = add(3, 4);            // 7
```

### Predicate - Returns bool
```csharp
Predicate<int> isEven = x => x % 2 == 0;
Predicate<string> isLong = s => s.Length > 10;

bool result1 = isEven(4);              // true
bool result2 = isLong(""Hello"");       // false
```

## Multicast Delegates
```csharp
Action<string> notify = null;

notify += message => Console.WriteLine($""Email: {message}"");
notify += message => Console.WriteLine($""SMS: {message}"");
notify += message => Console.WriteLine($""Push: {message}"");

notify(""Order shipped!"");
// Output:
// Email: Order shipped!
// SMS: Order shipped!
// Push: Order shipped!

// Remove a handler
notify -= message => Console.WriteLine($""SMS: {message}"");
```

## Events
Events are a special kind of delegate designed for the publisher-subscriber pattern.

```csharp
public class Button
{
    // Event declaration
    public event EventHandler<ClickEventArgs> Clicked;

    // Protected method to raise the event
    protected virtual void OnClicked(ClickEventArgs e)
    {
        Clicked?.Invoke(this, e);
    }

    public void Click()
    {
        OnClicked(new ClickEventArgs { ClickTime = DateTime.Now });
    }
}

public class ClickEventArgs : EventArgs
{
    public DateTime ClickTime { get; set; }
}

// Usage
var button = new Button();

// Subscribe to event
button.Clicked += (sender, e) =>
{
    Console.WriteLine($""Button clicked at {e.ClickTime}"");
};

button.Click();  // Raises the event
```

## Event vs Delegate
| Feature | Delegate | Event |
|---------|----------|-------|
| Can invoke from outside class | Yes | No (only owner can) |
| Can assign directly (=) | Yes | No (only += / -=) |
| Encapsulation | Less | More |
| Use case | Callbacks | Notifications |

## Real-World Example: Order Processing
```csharp
public class OrderProcessor
{
    public event EventHandler<OrderEventArgs> OrderPlaced;
    public event EventHandler<OrderEventArgs> OrderShipped;

    public void ProcessOrder(Order order)
    {
        // Process order logic...
        OnOrderPlaced(new OrderEventArgs { Order = order });
    }

    public void ShipOrder(Order order)
    {
        // Shipping logic...
        OnOrderShipped(new OrderEventArgs { Order = order });
    }

    protected virtual void OnOrderPlaced(OrderEventArgs e)
        => OrderPlaced?.Invoke(this, e);

    protected virtual void OnOrderShipped(OrderEventArgs e)
        => OrderShipped?.Invoke(this, e);
}

// Subscribers
var processor = new OrderProcessor();

processor.OrderPlaced += (s, e) =>
    emailService.SendOrderConfirmation(e.Order);

processor.OrderShipped += (s, e) =>
    smsService.SendShippingNotification(e.Order);
```",
                CodeExample = @"// Complete example with custom event args
public class StockMarket
{
    public event EventHandler<StockPriceChangedEventArgs> PriceChanged;

    private decimal _price;
    public decimal Price
    {
        get => _price;
        set
        {
            var oldPrice = _price;
            _price = value;
            OnPriceChanged(new StockPriceChangedEventArgs
            {
                Symbol = ""MSFT"",
                OldPrice = oldPrice,
                NewPrice = value,
                ChangePercent = oldPrice > 0 ? (value - oldPrice) / oldPrice * 100 : 0
            });
        }
    }

    protected virtual void OnPriceChanged(StockPriceChangedEventArgs e)
    {
        PriceChanged?.Invoke(this, e);
    }
}

public class StockPriceChangedEventArgs : EventArgs
{
    public string Symbol { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public decimal ChangePercent { get; set; }
}

// Usage
var market = new StockMarket();

market.PriceChanged += (sender, e) =>
{
    Console.WriteLine($""{e.Symbol}: ${e.OldPrice} -> ${e.NewPrice} ({e.ChangePercent:F2}%)"");
};

market.Price = 100m;  // MSFT: $0 -> $100 (0.00%)
market.Price = 105m;  // MSFT: $100 -> $105 (5.00%)",
                Tags = new() { "Delegates", "Events", "Callbacks", "Advanced" }
            },

            new() {
                Title = "Generics Deep Dive",
                Category = "Advanced",
                Difficulty = "Medium",
                KeyConcepts = "Generic classes, methods, constraints, covariance, contravariance, where clause",
                DotNetVersion = "2.0",
                Lesson = @"# Generics Deep Dive

## What are Generics?
Generics allow you to write type-safe code that works with any data type while maintaining compile-time type checking.

## Generic Classes
```csharp
public class Repository<T> where T : class
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);
    public T GetById(int index) => _items[index];
    public IEnumerable<T> GetAll() => _items;
    public int Count => _items.Count;
}

// Usage
var userRepo = new Repository<User>();
userRepo.Add(new User { Name = ""John"" });

var orderRepo = new Repository<Order>();
orderRepo.Add(new Order { Total = 99.99m });
```

## Generic Methods
```csharp
public class Utilities
{
    // Generic method
    public T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) > 0 ? a : b;
    }

    // Generic method with multiple type parameters
    public TResult Convert<TInput, TResult>(TInput input, Func<TInput, TResult> converter)
    {
        return converter(input);
    }
}

// Usage
var utils = new Utilities();
int maxInt = utils.Max(5, 10);           // 10
string maxStr = utils.Max(""apple"", ""banana"");  // ""banana""

string result = utils.Convert(42, x => x.ToString());  // ""42""
```

## Generic Constraints

### where T : class (Reference type)
```csharp
public class ReferenceRepository<T> where T : class
{
    public T FindOrDefault(Predicate<T> predicate)
    {
        // Can return null because T is a reference type
        return default;  // null
    }
}
```

### where T : struct (Value type)
```csharp
public class ValueContainer<T> where T : struct
{
    private T? _value;  // Nullable value type

    public void SetValue(T value) => _value = value;
    public T GetValueOrDefault() => _value ?? default;
}
```

### where T : new() (Parameterless constructor)
```csharp
public class Factory<T> where T : new()
{
    public T Create() => new T();  // Can instantiate T
}
```

### where T : BaseClass
```csharp
public class EntityRepository<T> where T : Entity
{
    public void Save(T entity)
    {
        entity.Id = Guid.NewGuid();  // Entity has Id property
        entity.CreatedAt = DateTime.UtcNow;
    }
}
```

### where T : IInterface
```csharp
public class Sorter<T> where T : IComparable<T>
{
    public T[] Sort(T[] items)
    {
        Array.Sort(items);  // Uses IComparable<T>
        return items;
    }
}
```

### Multiple Constraints
```csharp
public class Service<T> where T : class, IEntity, IValidatable, new()
{
    public T CreateAndValidate()
    {
        var entity = new T();
        entity.Validate();
        return entity;
    }
}
```

## Covariance and Contravariance

### Covariance (out) - Can use derived type
```csharp
public interface IReadOnlyRepository<out T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
}

// Animal is base, Dog is derived
IReadOnlyRepository<Dog> dogRepo = new DogRepository();
IReadOnlyRepository<Animal> animalRepo = dogRepo;  // Covariance allows this
```

### Contravariance (in) - Can use base type
```csharp
public interface IComparer<in T>
{
    int Compare(T x, T y);
}

// Can use AnimalComparer where DogComparer is expected
IComparer<Animal> animalComparer = new AnimalComparer();
IComparer<Dog> dogComparer = animalComparer;  // Contravariance allows this
```

## Generic Interfaces in .NET
```csharp
// Common generic interfaces
IEnumerable<T>      // Iteration
ICollection<T>      // Add, Remove, Count
IList<T>           // Index access
IDictionary<TKey, TValue>  // Key-value pairs
IComparable<T>     // Comparison
IEquatable<T>      // Equality
IComparer<T>       // Custom comparison
```",
                CodeExample = @"// Advanced generic pattern: Specification pattern
public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
    ISpecification<T> And(ISpecification<T> other);
    ISpecification<T> Or(ISpecification<T> other);
}

public abstract class Specification<T> : ISpecification<T>
{
    public abstract bool IsSatisfiedBy(T entity);

    public ISpecification<T> And(ISpecification<T> other)
        => new AndSpecification<T>(this, other);

    public ISpecification<T> Or(ISpecification<T> other)
        => new OrSpecification<T>(this, other);
}

public class AndSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override bool IsSatisfiedBy(T entity)
        => _left.IsSatisfiedBy(entity) && _right.IsSatisfiedBy(entity);
}

// Product specifications
public class InStockSpecification : Specification<Product>
{
    public override bool IsSatisfiedBy(Product product)
        => product.StockQuantity > 0;
}

public class PriceRangeSpecification : Specification<Product>
{
    private readonly decimal _min;
    private readonly decimal _max;

    public PriceRangeSpecification(decimal min, decimal max)
    {
        _min = min;
        _max = max;
    }

    public override bool IsSatisfiedBy(Product product)
        => product.Price >= _min && product.Price <= _max;
}

// Usage
var inStock = new InStockSpecification();
var affordable = new PriceRangeSpecification(10, 100);
var spec = inStock.And(affordable);

var matchingProducts = products.Where(p => spec.IsSatisfiedBy(p));",
                Tags = new() { "Generics", "Type Safety", "Constraints", "Advanced" }
            },

            new() {
                Title = "Reflection and Attributes",
                Category = "Advanced",
                Difficulty = "Hard",
                KeyConcepts = "Type inspection, GetType(), typeof(), custom attributes, Activator, dynamic loading",
                DotNetVersion = "1.0",
                Lesson = @"# Reflection and Attributes

## What is Reflection?
Reflection allows you to inspect and manipulate types, methods, properties, and other metadata at runtime.

## Getting Type Information

### Using typeof() and GetType()
```csharp
// typeof - compile-time, for type names
Type type1 = typeof(string);
Type type2 = typeof(List<int>);

// GetType() - runtime, for instances
string text = ""Hello"";
Type type3 = text.GetType();

// Type comparison
bool isString = type3 == typeof(string);  // true
```

## Inspecting Members

### Properties
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    private string Secret { get; set; }
}

Type type = typeof(Person);

// Get all public properties
PropertyInfo[] publicProps = type.GetProperties();
foreach (var prop in publicProps)
{
    Console.WriteLine($""{prop.Name}: {prop.PropertyType.Name}"");
}
// Output: Name: String, Age: Int32

// Get private properties too
PropertyInfo[] allProps = type.GetProperties(
    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
```

### Methods
```csharp
MethodInfo[] methods = type.GetMethods();
foreach (var method in methods)
{
    var parameters = method.GetParameters();
    Console.WriteLine($""{method.Name}({string.Join("", "", parameters.Select(p => p.ParameterType.Name))})"");
}
```

## Creating Instances Dynamically

### Using Activator
```csharp
// Create instance with parameterless constructor
object instance = Activator.CreateInstance(typeof(Person));

// Create instance with parameters
object instance2 = Activator.CreateInstance(
    typeof(Person),
    new object[] { ""John"", 30 });

// Generic version
Person person = Activator.CreateInstance<Person>();
```

### Invoking Methods
```csharp
Person person = new Person { Name = ""John"" };
Type type = typeof(Person);

// Get and invoke a method
MethodInfo method = type.GetMethod(""ToString"");
object result = method.Invoke(person, null);

// Set property value
PropertyInfo nameProp = type.GetProperty(""Name"");
nameProp.SetValue(person, ""Jane"");

// Get property value
string name = (string)nameProp.GetValue(person);
```

## Custom Attributes

### Creating Custom Attributes
```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
public class ValidationAttribute : Attribute
{
    public string ErrorMessage { get; set; }
    public bool Required { get; set; }

    public ValidationAttribute(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute : Attribute
{
    public int Length { get; }

    public MaxLengthAttribute(int length)
    {
        Length = length;
    }
}
```

### Applying Attributes
```csharp
[Validation(""User validation failed"")]
public class User
{
    [Validation(""Name is required"", Required = true)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Validation(""Email is required"", Required = true)]
    public string Email { get; set; }
}
```

### Reading Attributes at Runtime
```csharp
Type type = typeof(User);

// Get class-level attributes
var classAttrs = type.GetCustomAttributes<ValidationAttribute>();

// Get property-level attributes
foreach (var prop in type.GetProperties())
{
    var validation = prop.GetCustomAttribute<ValidationAttribute>();
    if (validation?.Required == true)
    {
        Console.WriteLine($""{prop.Name} is required"");
    }

    var maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
    if (maxLength != null)
    {
        Console.WriteLine($""{prop.Name} max length: {maxLength.Length}"");
    }
}
```

## Performance Considerations
```csharp
// Reflection is SLOW! Cache when possible
private static readonly PropertyInfo _nameProp = typeof(Person).GetProperty(""Name"");

// Better: Use compiled expressions
private static readonly Func<Person, string> _getName =
    (Func<Person, string>)Delegate.CreateDelegate(
        typeof(Func<Person, string>),
        typeof(Person).GetProperty(""Name"").GetGetMethod());

// Or use source generators in modern .NET
```",
                CodeExample = @"// Practical example: Simple object validator using reflection
public class Validator
{
    public ValidationResult Validate<T>(T obj)
    {
        var result = new ValidationResult();
        var type = typeof(T);

        foreach (var prop in type.GetProperties())
        {
            var value = prop.GetValue(obj);

            // Check Required
            var required = prop.GetCustomAttribute<RequiredAttribute>();
            if (required != null && value == null)
            {
                result.Errors.Add($""{prop.Name} is required"");
            }

            // Check MaxLength
            var maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLength != null && value is string str && str.Length > maxLength.Length)
            {
                result.Errors.Add($""{prop.Name} exceeds max length of {maxLength.Length}"");
            }

            // Check Range
            var range = prop.GetCustomAttribute<RangeAttribute>();
            if (range != null && value is IComparable comparable)
            {
                if (comparable.CompareTo(range.Min) < 0 || comparable.CompareTo(range.Max) > 0)
                {
                    result.Errors.Add($""{prop.Name} must be between {range.Min} and {range.Max}"");
                }
            }
        }

        return result;
    }
}

// Usage
var user = new User { Name = """", Email = null, Age = 150 };
var validator = new Validator();
var result = validator.Validate(user);

if (!result.IsValid)
{
    foreach (var error in result.Errors)
        Console.WriteLine(error);
}",
                Tags = new() { "Reflection", "Attributes", "Metadata", "Advanced" }
            },

            new() {
                Title = "Expression Trees",
                Category = "Advanced",
                Difficulty = "Hard",
                KeyConcepts = "Expression<T>, lambda to expression tree, building expressions, IQueryable",
                DotNetVersion = "3.5",
                Lesson = @"# Expression Trees

## What are Expression Trees?
Expression trees represent code as a data structure that can be examined, modified, or compiled at runtime.

## Expression vs Delegate
```csharp
// Delegate - compiled IL code
Func<int, int, int> addDelegate = (a, b) => a + b;

// Expression - data structure representing the code
Expression<Func<int, int, int>> addExpression = (a, b) => a + b;

// Expression can be compiled to delegate
Func<int, int, int> compiled = addExpression.Compile();
int result = compiled(2, 3);  // 5
```

## Why Use Expression Trees?
1. **LINQ to SQL/EF** - Translates C# to SQL
2. **Dynamic query building** - Build queries at runtime
3. **Code analysis** - Examine code structure
4. **Code generation** - Generate optimized code

## Examining Expression Trees
```csharp
Expression<Func<int, bool>> expr = x => x > 5;

// Examine the tree
Console.WriteLine(expr.Body);           // (x > 5)
Console.WriteLine(expr.Body.NodeType);  // GreaterThan
Console.WriteLine(expr.Parameters[0]);  // x

// Cast to binary expression
var binary = (BinaryExpression)expr.Body;
Console.WriteLine(binary.Left);   // x
Console.WriteLine(binary.Right);  // 5
```

## Building Expression Trees Manually
```csharp
// Build: x => x > 5
ParameterExpression param = Expression.Parameter(typeof(int), ""x"");
ConstantExpression constant = Expression.Constant(5, typeof(int));
BinaryExpression body = Expression.GreaterThan(param, constant);

Expression<Func<int, bool>> expr =
    Expression.Lambda<Func<int, bool>>(body, param);

// Compile and use
Func<int, bool> func = expr.Compile();
bool result = func(10);  // true
```

## Dynamic Query Building
```csharp
public class QueryBuilder<T>
{
    public Expression<Func<T, bool>> BuildFilter(
        string propertyName,
        object value,
        string operation = ""Equals"")
    {
        var param = Expression.Parameter(typeof(T), ""x"");
        var property = Expression.Property(param, propertyName);
        var constant = Expression.Constant(value);

        Expression body = operation switch
        {
            ""Equals"" => Expression.Equal(property, constant),
            ""NotEquals"" => Expression.NotEqual(property, constant),
            ""GreaterThan"" => Expression.GreaterThan(property, constant),
            ""LessThan"" => Expression.LessThan(property, constant),
            ""Contains"" => Expression.Call(property,
                typeof(string).GetMethod(""Contains"", new[] { typeof(string) }),
                constant),
            _ => throw new NotSupportedException()
        };

        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

// Usage
var builder = new QueryBuilder<Product>();
var filter = builder.BuildFilter(""Price"", 100m, ""GreaterThan"");
var expensiveProducts = products.AsQueryable().Where(filter);
```

## Entity Framework and IQueryable
```csharp
// This works because EF translates the expression to SQL
IQueryable<Product> query = dbContext.Products
    .Where(p => p.Price > 100 && p.Category == ""Electronics"");

// SQL generated:
// SELECT * FROM Products WHERE Price > 100 AND Category = 'Electronics'

// This would NOT translate (uses Func, not Expression)
Func<Product, bool> filter = p => p.Price > 100;
var result = dbContext.Products.Where(filter);  // Fetches ALL then filters in memory!
```

## Combining Expressions
```csharp
public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), ""x"");
        var body = Expression.AndAlso(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), ""x"");
        var body = Expression.OrElse(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

// Usage
Expression<Func<Product, bool>> priceFilter = p => p.Price > 50;
Expression<Func<Product, bool>> stockFilter = p => p.InStock;

var combined = priceFilter.And(stockFilter);
```",
                CodeExample = @"// Advanced: Building a dynamic sort expression
public static class QueryableExtensions
{
    public static IOrderedQueryable<T> OrderByProperty<T>(
        this IQueryable<T> source,
        string propertyName,
        bool descending = false)
    {
        var type = typeof(T);
        var property = type.GetProperty(propertyName);
        var parameter = Expression.Parameter(type, ""x"");
        var propertyAccess = Expression.Property(parameter, property);
        var keySelector = Expression.Lambda(propertyAccess, parameter);

        var methodName = descending ? ""OrderByDescending"" : ""OrderBy"";

        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(type, property.PropertyType);

        return (IOrderedQueryable<T>)method.Invoke(null, new object[] { source, keySelector });
    }
}

// Usage - sort by any property at runtime
var products = dbContext.Products.AsQueryable();

string sortBy = ""Price"";  // From user input
bool desc = true;

var sorted = products.OrderByProperty(sortBy, desc);
// Generates: SELECT * FROM Products ORDER BY Price DESC",
                Tags = new() { "Expressions", "LINQ", "Dynamic", "Advanced" }
            },

            new() {
                Title = "Span<T> and Memory<T>",
                Category = "Performance",
                Difficulty = "Hard",
                KeyConcepts = "Stack allocation, slicing without copying, ReadOnlySpan, Memory<T>, stackalloc",
                DotNetVersion = "7.0",
                Lesson = @"# Span<T> and Memory<T>

## What is Span<T>?
Span<T> is a stack-only type that provides a type-safe, memory-safe view over contiguous memory without copying.

## Why Use Span<T>?
1. **Zero allocations** - No heap allocation for slicing
2. **Performance** - Direct memory access
3. **Safety** - Bounds checking included
4. **Unified API** - Works with arrays, strings, native memory

## Basic Usage
```csharp
// Create from array
int[] array = { 1, 2, 3, 4, 5 };
Span<int> span = array;

// Slice without copying!
Span<int> slice = span.Slice(1, 3);  // { 2, 3, 4 }
slice[0] = 99;  // Modifies original array!

Console.WriteLine(array[1]);  // 99

// Create from string (ReadOnlySpan)
string text = ""Hello, World!"";
ReadOnlySpan<char> chars = text.AsSpan();
ReadOnlySpan<char> hello = chars.Slice(0, 5);  // ""Hello""
```

## Stack Allocation with stackalloc
```csharp
// Allocate on stack - no GC pressure!
Span<int> numbers = stackalloc int[100];

for (int i = 0; i < numbers.Length; i++)
{
    numbers[i] = i * i;
}

// Use Span methods
int sum = 0;
foreach (int n in numbers)
    sum += n;
```

## String Parsing Without Allocations
```csharp
// Traditional - allocates substrings
public (string key, string value) ParseTraditional(string input)
{
    int index = input.IndexOf('=');
    return (input.Substring(0, index), input.Substring(index + 1));
}

// With Span - zero allocations
public (ReadOnlySpan<char> key, ReadOnlySpan<char> value) ParseWithSpan(ReadOnlySpan<char> input)
{
    int index = input.IndexOf('=');
    return (input.Slice(0, index), input.Slice(index + 1));
}

// Even better - parse numbers directly
public int ParseNumber(ReadOnlySpan<char> input)
{
    return int.Parse(input);  // No string allocation!
}
```

## Memory<T> vs Span<T>
```csharp
// Span<T> - stack only, cannot be stored in fields, cannot be used in async
public ref struct SpanExample  // ref struct = stack only
{
    public Span<int> Data;  // OK - same restrictions
}

// Memory<T> - can be stored, used in async
public class MemoryExample
{
    private Memory<int> _data;  // OK - can store

    public async Task ProcessAsync(Memory<int> data)
    {
        _data = data;  // OK
        await Task.Delay(100);
        var span = _data.Span;  // Get Span when needed
        // Process...
    }
}
```

## Span Limitations
```csharp
// ❌ Cannot store Span in class field
public class Bad
{
    Span<int> _span;  // Compiler error!
}

// ❌ Cannot use Span in async methods
public async Task BadAsync(Span<int> span)  // Error!
{
    await Task.Delay(100);
}

// ❌ Cannot use Span in lambda captures
Span<int> span = stackalloc int[10];
var func = () => span[0];  // Error!

// ✅ Use Memory<T> for these scenarios
```

## ArrayPool for Reusable Buffers
```csharp
// Rent from pool instead of allocating
var pool = ArrayPool<byte>.Shared;
byte[] buffer = pool.Rent(1024);  // May be larger

try
{
    Span<byte> span = buffer.AsSpan(0, 1024);
    // Use span...
}
finally
{
    pool.Return(buffer);  // Return to pool
}
```

## Performance Comparison
```csharp
// Benchmark results (typical):
// Traditional substring parsing: 150ns, 64 bytes allocated
// Span-based parsing: 15ns, 0 bytes allocated
// 10x faster, zero allocations!
```",
                CodeExample = @"// High-performance CSV parser using Span
public class CsvParser
{
    public void ParseLine(ReadOnlySpan<char> line, Span<Range> fields)
    {
        int fieldIndex = 0;
        int start = 0;

        for (int i = 0; i < line.Length && fieldIndex < fields.Length; i++)
        {
            if (line[i] == ',')
            {
                fields[fieldIndex++] = start..i;
                start = i + 1;
            }
        }

        // Last field
        if (fieldIndex < fields.Length)
        {
            fields[fieldIndex] = start..line.Length;
        }
    }

    public void ProcessCsv(ReadOnlySpan<char> csv)
    {
        Span<Range> fields = stackalloc Range[10];  // Max 10 columns

        foreach (var lineRange in csv.Split('\n'))
        {
            var line = csv[lineRange];
            ParseLine(line, fields);

            // Access fields without allocations
            var firstName = csv[fields[0]];
            var lastName = csv[fields[1]];
            var ageSpan = csv[fields[2]];

            if (int.TryParse(ageSpan, out int age))
            {
                // Process...
            }
        }
    }
}

// Extension for splitting spans
public static class SpanExtensions
{
    public ref struct SpanSplitEnumerator
    {
        private ReadOnlySpan<char> _remaining;
        private readonly char _separator;

        public SpanSplitEnumerator(ReadOnlySpan<char> span, char separator)
        {
            _remaining = span;
            _separator = separator;
            Current = default;
        }

        public Range Current { get; private set; }

        public bool MoveNext()
        {
            if (_remaining.IsEmpty) return false;

            int index = _remaining.IndexOf(_separator);
            if (index == -1)
            {
                Current = ..;
                _remaining = default;
            }
            else
            {
                Current = ..index;
                _remaining = _remaining[(index + 1)..];
            }
            return true;
        }
    }
}",
                Tags = new() { "Performance", "Memory", "Span", "Advanced" }
            },

            new() {
                Title = "Pattern Matching",
                Category = "Modern C#",
                Difficulty = "Medium",
                KeyConcepts = "Type patterns, property patterns, switch expressions, when clauses, relational patterns",
                DotNetVersion = "7.0",
                Lesson = @"# Pattern Matching in C#

## Overview
Pattern matching allows you to test values against patterns and extract information when there's a match.

## Type Patterns
```csharp
object obj = ""Hello"";

// is expression with type pattern
if (obj is string s)
{
    Console.WriteLine(s.ToUpper());  // s is already typed
}

// Negation pattern
if (obj is not null)
{
    Console.WriteLine(obj.ToString());
}

// Switch with type patterns
string Describe(object obj) => obj switch
{
    int i => $""Integer: {i}"",
    string s => $""String: {s}"",
    List<int> list => $""List with {list.Count} items"",
    null => ""null"",
    _ => ""Unknown""
};
```

## Property Patterns
```csharp
public record Person(string Name, int Age, Address Address);
public record Address(string City, string Country);

// Property pattern in if statement
if (person is { Age: > 18, Address.Country: ""USA"" })
{
    Console.WriteLine(""Adult in USA"");
}

// Property pattern in switch
string Categorize(Person person) => person switch
{
    { Age: < 13 } => ""Child"",
    { Age: < 20 } => ""Teenager"",
    { Age: < 65 } => ""Adult"",
    { Age: >= 65 } => ""Senior"",
    _ => ""Unknown""
};

// Nested property patterns
string GetLocation(Person person) => person switch
{
    { Address: { City: ""NYC"", Country: ""USA"" } } => ""New York City"",
    { Address: { Country: ""USA"" } } => ""Somewhere in USA"",
    { Address: null } => ""No address"",
    _ => ""International""
};
```

## Relational Patterns
```csharp
string GetGrade(int score) => score switch
{
    >= 90 => ""A"",
    >= 80 => ""B"",
    >= 70 => ""C"",
    >= 60 => ""D"",
    < 60 => ""F""
};

// Combining with and/or
string Categorize(int value) => value switch
{
    > 0 and < 10 => ""Single digit positive"",
    >= 10 and <= 100 => ""Two digits"",
    < 0 or > 1000 => ""Out of range"",
    _ => ""Other""
};
```

## Tuple Patterns
```csharp
string GetQuadrant(int x, int y) => (x, y) switch
{
    ( > 0, > 0) => ""Q1"",
    ( < 0, > 0) => ""Q2"",
    ( < 0, < 0) => ""Q3"",
    ( > 0, < 0) => ""Q4"",
    (0, 0) => ""Origin"",
    (_, 0) => ""X-axis"",
    (0, _) => ""Y-axis""
};

// State machine with tuple pattern
string GetState(bool isConnected, bool hasData) => (isConnected, hasData) switch
{
    (false, _) => ""Disconnected"",
    (true, false) => ""Connected, no data"",
    (true, true) => ""Ready""
};
```

## List Patterns (C# 11+)
```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// Match specific elements
if (numbers is [1, 2, 3, 4, 5])
    Console.WriteLine(""Exact match"");

// Match with discard
if (numbers is [1, _, 3, _, 5])
    Console.WriteLine(""Alternating pattern"");

// Slice pattern
if (numbers is [var first, .. var middle, var last])
    Console.WriteLine($""First: {first}, Last: {last}, Middle count: {middle.Length}"");

// Empty and single element
string Describe(int[] arr) => arr switch
{
    [] => ""Empty"",
    [var single] => $""Single: {single}"",
    [var first, var second] => $""Pair: {first}, {second}"",
    [var head, .. var tail] => $""Head: {head}, Tail length: {tail.Length}""
};
```

## when Clauses (Guards)
```csharp
string Classify(object obj) => obj switch
{
    int i when i < 0 => ""Negative"",
    int i when i == 0 => ""Zero"",
    int i when i > 0 => ""Positive"",
    string s when s.Length == 0 => ""Empty string"",
    string s when s.Length < 10 => ""Short string"",
    string s => ""Long string"",
    _ => ""Other""
};

// Complex guard conditions
decimal CalculateDiscount(Order order) => order switch
{
    { Total: > 1000 } when order.Customer.IsPremium => 0.20m,
    { Total: > 1000 } => 0.10m,
    { Total: > 500 } when order.Customer.IsPremium => 0.10m,
    { Total: > 500 } => 0.05m,
    _ => 0m
};
```

## Combining Patterns
```csharp
bool IsValidUser(User user) => user is
{
    Name: not null and { Length: > 0 },
    Age: >= 18 and <= 120,
    Email: string email
} && email.Contains('@');

// Or pattern
if (obj is string or StringBuilder)
{
    Console.WriteLine(""Text type"");
}
```",
                CodeExample = @"// Real-world example: Order processing with pattern matching
public record Order(
    string OrderId,
    OrderStatus Status,
    decimal Total,
    Customer Customer,
    List<OrderItem> Items);

public record Customer(string Name, bool IsPremium, int LoyaltyPoints);
public record OrderItem(string ProductId, int Quantity, decimal Price);

public enum OrderStatus { Pending, Processing, Shipped, Delivered, Cancelled }

public class OrderProcessor
{
    public string ProcessOrder(Order order) => order switch
    {
        // Cancelled orders
        { Status: OrderStatus.Cancelled } => ""Order was cancelled"",

        // Empty orders
        { Items: [] } => ""Cannot process empty order"",

        // Premium customer with large order
        { Customer.IsPremium: true, Total: > 500 } =>
            $""Priority processing for premium customer. 20% discount applied."",

        // Large orders from regular customers
        { Total: > 1000, Customer.LoyaltyPoints: var points } when points > 100 =>
            $""Large order with {points} loyalty points. 15% discount."",

        // Standard processing
        { Status: OrderStatus.Pending, Items: [var single] } =>
            $""Processing single item order: {single.ProductId}"",

        { Status: OrderStatus.Pending, Items: [var first, .. var rest] } =>
            $""Processing order with {rest.Length + 1} items, starting with {first.ProductId}"",

        // Already processing
        { Status: OrderStatus.Processing or OrderStatus.Shipped } =>
            ""Order is already being processed"",

        _ => ""Standard processing""
    };

    public decimal CalculateFinalPrice(Order order) => order switch
    {
        { Customer.IsPremium: true, Total: var t } => t * 0.8m,  // 20% off
        { Customer.LoyaltyPoints: > 500, Total: var t } => t * 0.85m,  // 15% off
        { Total: > 1000 } => order.Total * 0.9m,  // 10% off
        { Total: > 500 } => order.Total * 0.95m,  // 5% off
        _ => order.Total
    };
}",
                Tags = new() { "Pattern Matching", "Switch", "Modern C#", "Advanced" }
            },

            new() {
                Title = "Records and Init-Only Properties",
                Category = "Modern C#",
                Difficulty = "Easy",
                KeyConcepts = "record types, positional records, init accessors, with expressions, value equality",
                DotNetVersion = "9.0",
                Lesson = @"# Records and Init-Only Properties

## What are Records?
Records are reference types that provide built-in value-based equality, immutability support, and concise syntax.

## Record Declaration
```csharp
// Positional record (recommended for simple DTOs)
public record Person(string Name, int Age);

// Equivalent to:
public record Person
{
    public string Name { get; init; }
    public int Age { get; init; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Deconstruct(out string name, out int age)
    {
        name = Name;
        age = Age;
    }
}
```

## Value Equality
```csharp
// Records compare by value, not reference
var person1 = new Person(""John"", 30);
var person2 = new Person(""John"", 30);

Console.WriteLine(person1 == person2);     // true (value equality)
Console.WriteLine(person1.Equals(person2)); // true

// Classes compare by reference
class PersonClass { public string Name; public int Age; }
var p1 = new PersonClass { Name = ""John"", Age = 30 };
var p2 = new PersonClass { Name = ""John"", Age = 30 };
Console.WriteLine(p1 == p2);  // false (different references)
```

## Init-Only Properties
```csharp
public class Product
{
    public string Name { get; init; }  // Can only be set during initialization
    public decimal Price { get; init; }
}

// Can set during object initialization
var product = new Product { Name = ""Widget"", Price = 9.99m };

// Cannot modify after
product.Name = ""New Name"";  // Compiler error!
```

## With Expressions (Non-Destructive Mutation)
```csharp
var original = new Person(""John"", 30);

// Create a copy with modified properties
var older = original with { Age = 31 };

Console.WriteLine(original.Age);  // 30 (unchanged)
Console.WriteLine(older.Age);     // 31

// Copy with multiple changes
var updated = original with { Name = ""Jane"", Age = 25 };
```

## Record Inheritance
```csharp
public record Person(string Name, int Age);
public record Employee(string Name, int Age, string Department) : Person(Name, Age);
public record Manager(string Name, int Age, string Department, int TeamSize)
    : Employee(Name, Age, Department);

// Equality includes type
var person = new Person(""John"", 30);
var employee = new Employee(""John"", 30, ""IT"");

Console.WriteLine(person == employee);  // false (different types)
```

## Record Structs (C# 10+)
```csharp
// Record struct - value type with record features
public readonly record struct Point(int X, int Y);

// Mutable record struct
public record struct MutablePoint(int X, int Y);

var p1 = new Point(1, 2);
var p2 = new Point(1, 2);
Console.WriteLine(p1 == p2);  // true

// Stack allocated, no heap allocation
Span<Point> points = stackalloc Point[100];
```

## Adding Methods to Records
```csharp
public record Person(string FirstName, string LastName, DateTime BirthDate)
{
    // Computed property
    public string FullName => $""{FirstName} {LastName}"";

    public int Age => DateTime.Now.Year - BirthDate.Year;

    // Methods
    public bool IsAdult() => Age >= 18;

    // Override ToString
    public override string ToString() => $""{FullName}, Age {Age}"";
}
```

## Primary Constructors (C# 12)
```csharp
// Primary constructor for classes (C# 12)
public class Service(ILogger logger, IRepository repo)
{
    public void DoWork()
    {
        logger.Log(""Starting work"");
        repo.Save(new Data());
    }
}

// Equivalent to:
public class Service
{
    private readonly ILogger _logger;
    private readonly IRepository _repo;

    public Service(ILogger logger, IRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }
}
```

## When to Use Records vs Classes

| Use Records When | Use Classes When |
|------------------|------------------|
| Immutable data | Mutable state |
| DTOs, Value objects | Entities with identity |
| Need value equality | Need reference equality |
| API responses | Services, controllers |
| Configuration | Stateful objects |",
                CodeExample = @"// Complete example: Domain modeling with records
public record Address(
    string Street,
    string City,
    string State,
    string ZipCode)
{
    public string FullAddress => $""{Street}, {City}, {State} {ZipCode}"";
}

public record Customer(
    Guid Id,
    string Name,
    string Email,
    Address? Address = null)
{
    public bool HasAddress => Address is not null;
}

public record OrderLine(
    string ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice)
{
    public decimal Total => Quantity * UnitPrice;
}

public record Order(
    Guid Id,
    Customer Customer,
    IReadOnlyList<OrderLine> Lines,
    DateTime OrderDate)
{
    public decimal Subtotal => Lines.Sum(l => l.Total);
    public decimal Tax => Subtotal * 0.08m;
    public decimal Total => Subtotal + Tax;

    // Factory method
    public static Order Create(Customer customer, params OrderLine[] lines)
        => new(Guid.NewGuid(), customer, lines, DateTime.UtcNow);
}

// Usage
var address = new Address(""123 Main St"", ""NYC"", ""NY"", ""10001"");
var customer = new Customer(Guid.NewGuid(), ""John Doe"", ""john@example.com"", address);

var order = Order.Create(customer,
    new OrderLine(""P001"", ""Widget"", 2, 19.99m),
    new OrderLine(""P002"", ""Gadget"", 1, 49.99m));

Console.WriteLine($""Order Total: {order.Total:C}"");

// Non-destructive mutation
var updatedCustomer = customer with { Email = ""newemail@example.com"" };
var newOrder = order with { Customer = updatedCustomer };",
                Tags = new() { "Records", "Immutability", "Modern C#", "C# 9" }
            },

            new() {
                Title = "Null Safety and Nullable Reference Types",
                Category = "Modern C#",
                Difficulty = "Medium",
                KeyConcepts = "nullable context, null-forgiving operator, null-conditional, null-coalescing, MaybeNull, NotNull",
                DotNetVersion = "8.0",
                Lesson = @"# Null Safety and Nullable Reference Types

## Enabling Nullable Reference Types
```xml
<!-- In .csproj -->
<PropertyGroup>
    <Nullable>enable</Nullable>
</PropertyGroup>
```

```csharp
// Or per-file
#nullable enable
```

## Nullable vs Non-Nullable
```csharp
#nullable enable

string nonNullable = ""Hello"";  // Cannot be null
string? nullable = null;         // Can be null

// Compiler warning:
nonNullable = null;  // Warning: Cannot assign null

// OK:
nullable = null;
nullable = ""World"";
```

## Null Checking Operators

### Null-Conditional (?.)
```csharp
string? name = person?.Name;  // null if person is null
int? length = name?.Length;   // null if name is null

// Chain multiple
string? city = person?.Address?.City;

// With methods
person?.Save();
```

### Null-Coalescing (??)
```csharp
string name = person?.Name ?? ""Unknown"";
int length = text?.Length ?? 0;

// Chaining
string result = first ?? second ?? third ?? ""default"";
```

### Null-Coalescing Assignment (??=)
```csharp
List<string>? items = null;
items ??= new List<string>();  // Assigns only if null

// Equivalent to:
if (items == null)
    items = new List<string>();
```

### Null-Forgiving Operator (!)
```csharp
// Tell compiler ""I know this isn't null""
string name = person!.Name;  // Suppresses warning

// Use carefully! Can cause NullReferenceException
string? maybeNull = null;
string definitelyNull = maybeNull!;  // No warning, but will fail at runtime
```

## Null Checking Patterns
```csharp
// Pattern matching (preferred)
if (person is not null)
{
    Console.WriteLine(person.Name);
}

if (name is { Length: > 0 })
{
    Console.WriteLine(name);
}

// Traditional null check
if (person != null)
{
    Console.WriteLine(person.Name);
}

// Throw if null
ArgumentNullException.ThrowIfNull(person);

// Or use null parameter check (C# 11)
public void Process(string name!!)  // Throws if null
{
    // name is guaranteed non-null
}
```

## Nullable Attributes
```csharp
using System.Diagnostics.CodeAnalysis;

public class Repository
{
    // May return null even though return type is non-nullable
    [return: MaybeNull]
    public T Find<T>(int id) where T : class
    {
        // May return null
    }

    // Guarantees non-null after call
    public bool TryGet(int id, [NotNullWhen(true)] out User? user)
    {
        user = FindUser(id);
        return user != null;
    }

    // Parameter must not be null
    public void Save([NotNull] User? user)
    {
        ArgumentNullException.ThrowIfNull(user);
        // user is not null after this point
    }

    // Output is not null if input is not null
    [return: NotNullIfNotNull(nameof(input))]
    public string? Transform(string? input)
    {
        return input?.ToUpper();
    }
}
```

## Required Members (C# 11)
```csharp
public class Person
{
    public required string Name { get; set; }  // Must be initialized
    public required int Age { get; set; }
    public string? Email { get; set; }  // Optional
}

// Must provide required members
var person = new Person { Name = ""John"", Age = 30 };

// Compiler error:
var invalid = new Person { Age = 30 };  // Missing Name!
```

## Best Practices
```csharp
// 1. Enable nullable for new projects
// 2. Use ? explicitly for nullable types
// 3. Avoid null-forgiving (!) unless absolutely necessary
// 4. Use ArgumentNullException.ThrowIfNull
// 5. Prefer pattern matching for null checks
// 6. Use [NotNull] attributes to help the compiler

// Good
public string GetDisplayName(User? user)
{
    if (user is null)
        return ""Anonymous"";

    return user.DisplayName ?? user.Email ?? ""Unknown"";
}

// Bad - too many null-forgiving operators
public string GetDisplayName(User? user)
{
    return user!.DisplayName!;  // Dangerous!
}
```",
                CodeExample = @"// Complete example: Null-safe service layer
public interface IUserRepository
{
    User? FindById(int id);
    Task<User?> FindByEmailAsync(string email);
    IEnumerable<User> GetAll();
}

public class UserService
{
    private readonly IUserRepository _repository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository repository, ILogger<UserService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public User GetUserOrThrow(int id)
    {
        var user = _repository.FindById(id);
        return user ?? throw new NotFoundException($""User {id} not found"");
    }

    public UserDto? GetUserDto(int id)
    {
        var user = _repository.FindById(id);

        if (user is null)
        {
            _logger.LogWarning(""User {Id} not found"", id);
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email ?? ""No email"",
            City = user.Address?.City ?? ""Unknown""
        };
    }

    public async Task<string> GetUserEmailAsync(int id)
    {
        var user = await _repository.FindByEmailAsync(id.ToString());

        return user switch
        {
            { Email: { Length: > 0 } email } => email,
            { Email: null or """" } => throw new InvalidOperationException(""User has no email""),
            null => throw new NotFoundException($""User {id} not found"")
        };
    }

    // Using TryGet pattern
    public bool TryGetUser(int id, [NotNullWhen(true)] out User? user)
    {
        user = _repository.FindById(id);
        return user is not null;
    }
}

// Usage
var service = new UserService(repo, logger);

if (service.TryGetUser(1, out var user))
{
    Console.WriteLine(user.Name);  // user is not null here
}

var dto = service.GetUserDto(1);
Console.WriteLine(dto?.Name ?? ""Not found"");",
                Tags = new() { "Null Safety", "Nullable", "Modern C#", "Best Practices" }
            },

            new() {
                Title = "Source Generators",
                Category = "Advanced",
                Difficulty = "Hard",
                KeyConcepts = "compile-time code generation, ISourceGenerator, incremental generators, partial classes",
                DotNetVersion = "5.0",
                Lesson = @"# Source Generators

## What are Source Generators?
Source generators run at compile time to generate additional C# source code that gets compiled with your project.

## Benefits
1. **Performance** - No runtime reflection
2. **Type safety** - Generated code is checked by compiler
3. **AOT friendly** - Works with Native AOT
4. **Debuggable** - Can step through generated code

## Common Use Cases
- JSON serialization (System.Text.Json)
- Dependency injection registration
- Mapping (AutoMapper alternatives)
- Logging
- Validation
- API clients

## Using Built-in Source Generators

### System.Text.Json Source Generation
```csharp
// Define a context
[JsonSerializable(typeof(Person))]
[JsonSerializable(typeof(List<Person>))]
public partial class AppJsonContext : JsonSerializerContext { }

public record Person(string Name, int Age);

// Usage - much faster than reflection-based
var json = JsonSerializer.Serialize(person, AppJsonContext.Default.Person);
var person = JsonSerializer.Deserialize(json, AppJsonContext.Default.Person);
```

### Regex Source Generation
```csharp
public partial class Validators
{
    [GeneratedRegex(@""^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"")]
    private static partial Regex EmailRegex();

    public static bool IsValidEmail(string email)
        => EmailRegex().IsMatch(email);
}
```

### Logging Source Generation
```csharp
public static partial class Log
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = ""User {UserId} logged in"")]
    public static partial void UserLoggedIn(ILogger logger, int userId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message = ""Failed to process order {OrderId}"")]
    public static partial void OrderProcessingFailed(ILogger logger, string orderId, Exception ex);
}

// Usage
Log.UserLoggedIn(_logger, user.Id);
Log.OrderProcessingFailed(_logger, order.Id, ex);
```

## Creating a Simple Source Generator
```csharp
// Generator project (.NET Standard 2.0)
[Generator]
public class HelloSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context) { }

    public void Execute(GeneratorExecutionContext context)
    {
        var source = @""
namespace Generated
{
    public static class Hello
    {
        public static string SayHello() => """"Hello from generated code!"""";
    }
}"";

        context.AddSource(""Hello.g.cs"", source);
    }
}
```

## Incremental Generators (Preferred)
```csharp
[Generator]
public class AutoNotifyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Register for syntax nodes
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (context, _) => GetClassInfo(context))
            .Where(info => info is not null);

        // Generate code
        context.RegisterSourceOutput(classDeclarations,
            (context, classInfo) => GenerateCode(context, classInfo!));
    }

    private static ClassInfo? GetClassInfo(GeneratorSyntaxContext context)
    {
        // Extract information from syntax
        var classDecl = (ClassDeclarationSyntax)context.Node;
        // ... analysis
        return new ClassInfo(classDecl.Identifier.Text);
    }

    private static void GenerateCode(SourceProductionContext context, ClassInfo info)
    {
        var source = $@""
public partial class {info.Name}
{{
    // Generated members
}}
"";
        context.AddSource($""{info.Name}.g.cs"", source);
    }
}
```

## Viewing Generated Code
```xml
<!-- In .csproj, to emit generated files -->
<PropertyGroup>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)\GeneratedFiles</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
```",
                CodeExample = @"// Example: Auto-implement ToString() generator
// First, define a marker attribute
[AttributeUsage(AttributeTargets.Class)]
public class AutoToStringAttribute : Attribute { }

// Use in your code
[AutoToString]
public partial class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

// Generator produces:
public partial class Person
{
    public override string ToString()
    {
        return $""Person {{ Name = {Name}, Age = {Age}, Email = {Email} }}"";
    }
}

// Example: Auto-register services generator
[AutoRegister(ServiceLifetime.Scoped)]
public class UserService : IUserService { }

[AutoRegister(ServiceLifetime.Singleton)]
public class CacheService : ICacheService { }

// Generator produces:
public static class ServiceRegistration
{
    public static IServiceCollection AddGeneratedServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<ICacheService, CacheService>();
        return services;
    }
}

// Usage in Program.cs
builder.Services.AddGeneratedServices();",
                Tags = new() { "Source Generators", "Code Generation", "Compile-time", "Advanced" }
            },

            // Memory Management & Performance
            new() {
                Title = "Garbage Collection and Memory Management",
                Category = "Memory",
                Difficulty = "Medium",
                DotNetVersion = "6.0+",
                KeyConcepts = "GC Generations, LOH, Memory Pressure, Finalizers, IDisposable",
                Lesson = @"# Garbage Collection in .NET

## How GC Works

The .NET Garbage Collector automatically manages memory allocation and deallocation. It uses a generational approach for efficiency.

### Generations
- **Gen 0**: Short-lived objects (local variables, temporary objects)
- **Gen 1**: Buffer between short and long-lived
- **Gen 2**: Long-lived objects (static data, application lifetime objects)

### Large Object Heap (LOH)
Objects >= 85KB go directly to LOH (treated as Gen 2).

## Common Issues

### Memory Leaks
Even with GC, you can leak memory:
- Event handlers not unsubscribed
- Static collections holding references
- Cached objects never removed

### Performance Problems
- Too many allocations trigger frequent GC
- Large objects fragment LOH
- Finalizers delay collection

## Best Practices

1. **Use `using` statements** for IDisposable
2. **Avoid finalizers** unless absolutely necessary
3. **Pool large objects** instead of allocating repeatedly
4. **Unsubscribe events** when done
5. **Use structs** for small, short-lived data",
                CodeExample = @"// ❌ BAD: Memory leak - event never unsubscribed
public class BadSubscriber
{
    public BadSubscriber(Publisher publisher)
    {
        publisher.OnDataReceived += HandleData;
        // Subscriber can never be GC'd while Publisher lives!
    }

    private void HandleData(object sender, EventArgs e) { }
}

// ✅ GOOD: Properly unsubscribe
public class GoodSubscriber : IDisposable
{
    private readonly Publisher _publisher;

    public GoodSubscriber(Publisher publisher)
    {
        _publisher = publisher;
        _publisher.OnDataReceived += HandleData;
    }

    public void Dispose()
    {
        _publisher.OnDataReceived -= HandleData;
    }

    private void HandleData(object sender, EventArgs e) { }
}

// Using IDisposable properly
using (var subscriber = new GoodSubscriber(publisher))
{
    // Use subscriber
} // Dispose called automatically

// Object pooling for performance
public class ObjectPool<T> where T : new()
{
    private readonly ConcurrentBag<T> _objects = new();

    public T Rent()
    {
        return _objects.TryTake(out T item) ? item : new T();
    }

    public void Return(T item)
    {
        _objects.Add(item);
    }
}

// Usage - avoid repeated allocations
var pool = new ObjectPool<StringBuilder>();
var sb = pool.Rent();
try
{
    sb.Append(""Hello"");
    // Use StringBuilder
}
finally
{
    sb.Clear();
    pool.Return(sb);
}

// Force GC (rarely needed, for demonstration)
GC.Collect();
GC.WaitForPendingFinalizers();
GC.Collect(); // Collect objects with finalizers",
                Tags = new() { "GC", "Memory", "Performance", "IDisposable" }
            },

            new() {
                Title = "Span<T> and Memory<T> for High-Performance Code",
                Category = "Memory",
                Difficulty = "Hard",
                DotNetVersion = "7.0+",
                KeyConcepts = "Stack allocation, Zero-copy, Slicing, ReadOnlySpan",
                Lesson = @"# Span<T> and Memory<T>

## What are they?

**Span<T>**: A stack-only type that provides a safe view over contiguous memory. Cannot be stored in heap (fields, async methods).

**Memory<T>**: Heap-allocated wrapper around Span<T>. Can be used in async methods and as fields.

## Why Use Them?

### Performance Benefits
- **Zero allocations**: No heap allocations for slicing
- **Stack-based**: Span<T> lives on stack
- **Zero-copy**: Views existing memory without copying
- **Unified API**: Works with arrays, strings, stack memory

## When to Use

- High-performance parsing
- String manipulation without allocations
- Working with binary data
- Memory-critical scenarios (games, parsers, serializers)",
                CodeExample = @"// ❌ OLD WAY: Multiple allocations
string ProcessString(string input)
{
    string trimmed = input.Trim(); // Allocation 1
    string lower = trimmed.ToLower(); // Allocation 2
    string sub = lower.Substring(0, 10); // Allocation 3
    return sub;
}

// ✅ NEW WAY: Zero allocations with Span
void ProcessStringSpan(ReadOnlySpan<char> input, Span<char> output)
{
    var trimmed = input.Trim();
    var first10 = trimmed.Slice(0, Math.Min(10, trimmed.Length));

    for (int i = 0; i < first10.Length; i++)
    {
        output[i] = char.ToLower(first10[i]);
    }
}

// Stack allocation - no heap usage!
Span<char> buffer = stackalloc char[100];
ProcessStringSpan(""  HELLO WORLD  "", buffer);

// Slicing without allocation
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
Span<int> allNumbers = numbers;
Span<int> firstFive = allNumbers.Slice(0, 5); // No allocation!
Span<int> lastFive = allNumbers.Slice(5); // No allocation!

// Parsing with zero allocations
ReadOnlySpan<char> ParseName(ReadOnlySpan<char> input)
{
    int commaIndex = input.IndexOf(',');
    if (commaIndex == -1) return input.Trim();

    return input.Slice(0, commaIndex).Trim();
}

// CSV parsing example
void ParseCsvLine(ReadOnlySpan<char> line)
{
    while (line.Length > 0)
    {
        int commaIndex = line.IndexOf(',');
        var field = commaIndex == -1
            ? line
            : line.Slice(0, commaIndex);

        ProcessField(field); // Zero allocation!

        line = commaIndex == -1
            ? ReadOnlySpan<char>.Empty
            : line.Slice(commaIndex + 1);
    }
}

// Memory<T> for async scenarios
async Task<int> ProcessDataAsync(Memory<byte> buffer)
{
    // Can use Memory<T> in async
    await SomeAsyncOperation(buffer);

    // Convert to Span when needed
    Span<byte> span = buffer.Span;
    return span.Length;
}

// Real-world example: Fast string parsing
public static bool TryParseInt32Fast(ReadOnlySpan<char> s, out int result)
{
    result = 0;
    if (s.IsEmpty) return false;

    int sign = 1;
    int i = 0;

    if (s[0] == '-')
    {
        sign = -1;
        i = 1;
    }

    for (; i < s.Length; i++)
    {
        if (s[i] < '0' || s[i] > '9') return false;
        result = result * 10 + (s[i] - '0');
    }

    result *= sign;
    return true;
}",
                Tags = new() { "Span", "Memory", "Performance", "Zero-allocation" }
            },

            new() {
                Title = "Threading and Task Parallel Library (TPL)",
                Category = "Async",
                Difficulty = "Hard",
                DotNetVersion = "6.0+",
                KeyConcepts = "Tasks, Thread Pool, Parallel.ForEach, async/await, Synchronization",
                Lesson = @"# Threading and TPL in .NET

## Task vs Thread

**Thread**: Low-level, expensive to create, dedicated OS thread
**Task**: High-level, uses thread pool, lightweight

## When to Use What

### Use Tasks for:
- I/O-bound operations (file, network, database)
- Parallel CPU-bound work
- Most async scenarios

### Use Threads for:
- Long-running CPU-intensive work
- Need dedicated thread (rare)

## Common Patterns

### CPU-Bound Parallelism
Use `Parallel.ForEach` or `Parallel.For` for data parallelism.

### I/O-Bound Async
Use `async/await` to avoid blocking threads.

## Synchronization

### Thread-Safe Collections
- `ConcurrentDictionary<TKey, TValue>`
- `ConcurrentQueue<T>`
- `ConcurrentBag<T>`

### Locking Primitives
- `lock` keyword (Monitor)
- `SemaphoreSlim` for async
- `ReaderWriterLockSlim` for read-heavy scenarios",
                CodeExample = @"// ❌ BAD: Creating threads manually
void BadParallelProcessing(List<string> items)
{
    foreach (var item in items)
    {
        new Thread(() => ProcessItem(item)).Start(); // Expensive!
    }
}

// ✅ GOOD: Use Task Parallel Library
void GoodParallelProcessing(List<string> items)
{
    Parallel.ForEach(items, item =>
    {
        ProcessItem(item);
    });
}

// CPU-bound work with degree of parallelism
var options = new ParallelOptions
{
    MaxDegreeOfParallelism = Environment.ProcessorCount
};

Parallel.ForEach(largeDataSet, options, item =>
{
    // Process each item in parallel
    var result = ExpensiveComputation(item);
});

// I/O-bound async operations
async Task ProcessMultipleFiles(string[] filePaths)
{
    // Start all reads concurrently
    var tasks = filePaths.Select(async path =>
    {
        var content = await File.ReadAllTextAsync(path);
        return ProcessContent(content);
    });

    // Wait for all to complete
    var results = await Task.WhenAll(tasks);
}

// Thread-safe dictionary
var cache = new ConcurrentDictionary<string, Data>();

// GetOrAdd is atomic
var data = cache.GetOrAdd(""key"", key =>
{
    // This delegate only runs if key doesn't exist
    return LoadExpensiveData(key);
});

// SemaphoreSlim for async throttling
var semaphore = new SemaphoreSlim(5); // Max 5 concurrent operations

async Task ProcessWithThrottling(IEnumerable<string> items)
{
    var tasks = items.Select(async item =>
    {
        await semaphore.WaitAsync();
        try
        {
            await ProcessItemAsync(item);
        }
        finally
        {
            semaphore.Release();
        }
    });

    await Task.WhenAll(tasks);
}

// Cancellation tokens
async Task LongRunningOperation(CancellationToken cancellationToken)
{
    for (int i = 0; i < 1000; i++)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await DoWorkAsync();
    }
}

// Usage with timeout
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
try
{
    await LongRunningOperation(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine(""Operation timed out"");
}

// Producer-Consumer pattern
var channel = Channel.CreateUnbounded<WorkItem>();

// Producer
async Task Producer()
{
    for (int i = 0; i < 100; i++)
    {
        await channel.Writer.WriteAsync(new WorkItem(i));
    }
    channel.Writer.Complete();
}

// Consumer
async Task Consumer()
{
    await foreach (var item in channel.Reader.ReadAllAsync())
    {
        await ProcessWorkItem(item);
    }
}

// Start both
await Task.WhenAll(Producer(), Consumer());",
                Tags = new() { "Threading", "TPL", "Parallel", "async/await" }
            },

            new() {
                Title = "Dependency Injection in .NET",
                Category = "Modern C#",
                Difficulty = "Medium",
                DotNetVersion = "6.0+",
                KeyConcepts = "IoC, Service Lifetimes, Constructor Injection, DI Container",
                Lesson = @"# Dependency Injection in .NET

## What is DI?

Dependency Injection is a design pattern where objects receive their dependencies from external sources rather than creating them internally.

## Service Lifetimes

### Transient
Created each time they're requested. Best for lightweight, stateless services.

### Scoped
Created once per request (web apps) or scope. Best for per-request data.

### Singleton
Created once and reused for the application lifetime. Best for stateless, shared services.

## Constructor Injection

The preferred way to inject dependencies. Makes dependencies explicit and testable.

## When to Use Which Lifetime

- **Transient**: Lightweight services, no shared state
- **Scoped**: DbContext, per-request state, UnitOfWork
- **Singleton**: Configuration, caches, stateless services

## Common Patterns

- **Options Pattern**: `IOptions<T>` for configuration
- **Factory Pattern**: `IServiceProvider` or custom factories
- **Named Services**: Using `IServiceCollection` with keys",
                CodeExample = @"// ❌ BAD: Tight coupling, hard to test
public class OrderService
{
    private readonly SqlConnection _connection;

    public OrderService()
    {
        _connection = new SqlConnection(""connection-string""); // Hard-coded!
    }

    public void CreateOrder(Order order)
    {
        // Directly uses concrete SqlConnection
    }
}

// ✅ GOOD: Dependency injection, loose coupling
public interface IOrderRepository
{
    Task CreateOrderAsync(Order order);
}

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly ILogger<OrderService> _logger;

    // Dependencies injected via constructor
    public OrderService(
        IOrderRepository repository,
        ILogger<OrderService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task CreateOrderAsync(Order order)
    {
        _logger.LogInformation(""Creating order {OrderId}"", order.Id);
        await _repository.CreateOrderAsync(order);
    }
}

// Registration in Program.cs
var builder = WebApplication.CreateBuilder(args);

// Register services with appropriate lifetimes
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

// DbContext is Scoped by default
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Options pattern for configuration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection(""EmailSettings""));

// Using IOptions<T>
public class EmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public void SendEmail(string to, string body)
    {
        // Use _settings.SmtpServer, etc.
    }
}

// Factory pattern
public interface INotificationFactory
{
    INotificationSender Create(NotificationType type);
}

public class NotificationFactory : INotificationFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public INotificationSender Create(NotificationType type)
    {
        return type switch
        {
            NotificationType.Email => _serviceProvider.GetRequiredService<EmailSender>(),
            NotificationType.Sms => _serviceProvider.GetRequiredService<SmsSender>(),
            _ => throw new ArgumentException(""Unknown type"")
        };
    }
}

// Keyed services (.NET 8+)
builder.Services.AddKeyedScoped<INotificationSender, EmailSender>(""email"");
builder.Services.AddKeyedScoped<INotificationSender, SmsSender>(""sms"");

public class NotificationController : ControllerBase
{
    private readonly INotificationSender _emailSender;

    public NotificationController(
        [FromKeyedServices(""email"")] INotificationSender emailSender)
    {
        _emailSender = emailSender;
    }
}

// Scoped service example
public class RequestContext
{
    public string UserId { get; set; }
    public DateTime RequestTime { get; set; }
}

// Middleware sets context
app.Use(async (context, next) =>
{
    var requestContext = context.RequestServices.GetRequiredService<RequestContext>();
    requestContext.RequestTime = DateTime.UtcNow;
    requestContext.UserId = context.User.FindFirst(""sub"")?.Value;

    await next();
});",
                Tags = new() { "DI", "IoC", "Service Lifetimes", "Constructor Injection" }
            },

            new() {
                Title = "Records and Pattern Matching (Modern C#)",
                Category = "Modern C#",
                Difficulty = "Medium",
                DotNetVersion = "9.0+",
                KeyConcepts = "Records, Pattern Matching, Switch Expressions, Deconstruction",
                Lesson = @"# Modern C# Features: Records & Pattern Matching

## Records (C# 9+)

Records are reference types designed for immutable data models. They provide value-based equality and concise syntax.

### Benefits
- **Immutable by default**: `with` expressions for copying
- **Value equality**: Compare by value, not reference
- **Deconstruction**: Built-in support
- **Concise syntax**: Positional records

## Pattern Matching

Powerful feature for testing and extracting data from objects.

### Types of Patterns
- **Type patterns**: `obj is string s`
- **Property patterns**: `person is { Age: > 18 }`
- **Positional patterns**: `point is (0, 0)`
- **List patterns**: `array is [1, 2, ..]`

## When to Use

- **Records**: DTOs, value objects, immutable data
- **Pattern Matching**: Complex conditionals, state machines, parsing",
                CodeExample = @"// Record definition (concise)
public record Person(string Name, int Age, string Email);

// Equivalent to a class with:
// - Public properties
// - Constructor
// - Equals/GetHashCode based on values
// - Deconstruction
// - ToString() override

// Value-based equality
var person1 = new Person(""John"", 30, ""john@email.com"");
var person2 = new Person(""John"", 30, ""john@email.com"");

Console.WriteLine(person1 == person2); // True! (value equality)

// Immutability with 'with' expressions
var olderPerson = person1 with { Age = 31 };
// person1 unchanged, olderPerson is new instance

// Deconstruction
var (name, age, email) = person1;

// Record with body
public record Customer(string Id, string Name)
{
    public bool IsVip { get; init; }
    public List<Order> Orders { get; init; } = new();

    public decimal TotalSpent => Orders.Sum(o => o.Amount);
}

// Pattern matching - Type patterns
object obj = ""Hello"";

if (obj is string s)
{
    Console.WriteLine($""Length: {s.Length}"");
}

// Switch expressions
string GetDiscount(Customer customer) => customer switch
{
    { IsVip: true, TotalSpent: > 10000 } => ""20% off"",
    { IsVip: true } => ""15% off"",
    { TotalSpent: > 5000 } => ""10% off"",
    { TotalSpent: > 1000 } => ""5% off"",
    _ => ""No discount""
};

// Property patterns
bool IsAdultFromUSA(Person person, string country) =>
    person is { Age: >= 18 } && country is ""USA"";

// Positional patterns with records
record Point(int X, int Y);

string Classify(Point point) => point switch
{
    (0, 0) => ""Origin"",
    (var x, 0) => $""On X-axis at {x}"",
    (0, var y) => $""On Y-axis at {y}"",
    (var x, var y) when x == y => ""On diagonal"",
    _ => ""Somewhere else""
};

// List patterns (C# 11+)
int[] numbers = { 1, 2, 3, 4, 5 };

string DescribeArray(int[] arr) => arr switch
{
    [] => ""Empty"",
    [1] => ""Single element: 1"",
    [1, 2] => ""Starts with 1, 2"",
    [1, .., 5] => ""Starts with 1, ends with 5"",
    [.., var last] => $""Last element: {last}"",
    _ => ""Other""
};

// Complex pattern matching
record OrderRequest(string ProductId, int Quantity, decimal UnitPrice);

(bool IsValid, string Error) ValidateOrder(OrderRequest order) => order switch
{
    { Quantity: <= 0 }
        => (false, ""Quantity must be positive""),
    { UnitPrice: <= 0 }
        => (false, ""Price must be positive""),
    { Quantity: > 1000 }
        => (false, ""Quantity exceeds maximum""),
    { ProductId: null or """" }
        => (false, ""Product ID required""),
    _
        => (true, string.Empty)
};

// State machine with pattern matching
record State;
record Idle : State;
record Processing : State;
record Completed : State;
record Failed(string Error) : State;

string GetStatusMessage(State state) => state switch
{
    Idle => ""Ready to start"",
    Processing => ""Working..."",
    Completed => ""Done!"",
    Failed { Error: var err } => $""Error: {err}"",
    _ => ""Unknown state""
};

// Real-world example: API response handling
record ApiResponse(int StatusCode, string Body);

async Task<T> HandleResponse<T>(ApiResponse response) => response switch
{
    { StatusCode: 200, Body: var body }
        => JsonSerializer.Deserialize<T>(body),
    { StatusCode: 404 }
        => throw new NotFoundException(),
    { StatusCode: >= 400 and < 500 }
        => throw new BadRequestException(response.Body),
    { StatusCode: >= 500 }
        => throw new ServerErrorException(response.Body),
    _
        => throw new UnexpectedResponseException()
};",
                Tags = new() { "Records", "Pattern Matching", "Modern C#", "Immutability" }
            }
        };
    }

    public static List<DesignPatternTopic> GetDesignPatternTopics()
    {
        return new List<DesignPatternTopic>
        {
            new() {
                Title = "Singleton",
                Category = "Creational",
                Difficulty = "Easy",
                KeyConcepts = "Single instance, global access point, lazy initialization, thread safety",
                UseCases = "Logging, configuration, connection pools, caches",
                Lesson = @"# Singleton Pattern

## Intent
Ensure a class has only one instance and provide a global point of access to it.

## When to Use
- Exactly one instance of a class is needed
- The single instance should be accessible globally
- Examples: Logger, Configuration, Database connection pool

## Implementation Approaches

### 1. Basic (Not Thread-Safe)
```csharp
public sealed class Singleton
{
    private static Singleton _instance;

    private Singleton() { }  // Private constructor

    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Singleton();
            return _instance;
        }
    }
}
// Problem: Race condition in multi-threaded scenarios
```

### 2. Thread-Safe with Lock
```csharp
public sealed class Singleton
{
    private static Singleton _instance;
    private static readonly object _lock = new object();

    private Singleton() { }

    public static Singleton Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                    _instance = new Singleton();
                return _instance;
            }
        }
    }
}
// Problem: Lock on every access (performance)
```

### 3. Double-Check Locking
```csharp
public sealed class Singleton
{
    private static volatile Singleton _instance;
    private static readonly object _lock = new object();

    private Singleton() { }

    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new Singleton();
                }
            }
            return _instance;
        }
    }
}
```

### 4. Lazy<T> (Recommended)
```csharp
public sealed class Singleton
{
    private static readonly Lazy<Singleton> _lazy =
        new Lazy<Singleton>(() => new Singleton());

    private Singleton() { }

    public static Singleton Instance => _lazy.Value;
}
```

### 5. Static Constructor
```csharp
public sealed class Singleton
{
    private static readonly Singleton _instance = new Singleton();

    // Static constructor ensures thread safety
    static Singleton() { }

    private Singleton() { }

    public static Singleton Instance => _instance;
}
```

## Real-World Example: Logger
```csharp
public sealed class Logger
{
    private static readonly Lazy<Logger> _instance =
        new Lazy<Logger>(() => new Logger());

    private readonly object _lock = new object();
    private readonly string _logPath;

    private Logger()
    {
        _logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            ""app.log""
        );
    }

    public static Logger Instance => _instance.Value;

    public void Log(string message)
    {
        lock (_lock)
        {
            File.AppendAllText(_logPath,
                $""[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}"");
        }
    }
}

// Usage
Logger.Instance.Log(""Application started"");
```

## Singleton vs Static Class

| Singleton | Static Class |
|-----------|--------------|
| Can implement interfaces | Cannot |
| Can be passed as parameter | Cannot |
| Lazy initialization | Eager (at first access) |
| Can have state | Only static state |
| Can be mocked for testing | Difficult to mock |

## Testing Singletons
```csharp
// Problem: Hard to test code that uses singleton directly
public class OrderService
{
    public void ProcessOrder(Order order)
    {
        Logger.Instance.Log($""Processing order {order.Id}"");  // Tight coupling!
    }
}

// Solution: Inject the singleton
public class OrderService
{
    private readonly ILogger _logger;

    public OrderService(ILogger logger)
    {
        _logger = logger;
    }

    public void ProcessOrder(Order order)
    {
        _logger.Log($""Processing order {order.Id}"");
    }
}
```

## Criticisms
- Global state (can lead to tight coupling)
- Hidden dependencies
- Difficult to unit test
- Violates Single Responsibility Principle (manages own lifecycle)

Consider using Dependency Injection with a singleton lifetime instead.",
                CodeExample = @"// Modern singleton with DI support
public interface IConfigurationService
{
    string GetValue(string key);
}

public sealed class ConfigurationService : IConfigurationService
{
    private static readonly Lazy<ConfigurationService> _instance =
        new(() => new ConfigurationService());

    private readonly Dictionary<string, string> _settings;

    private ConfigurationService()
    {
        _settings = LoadFromFile();
    }

    public static ConfigurationService Instance => _instance.Value;

    public string GetValue(string key)
        => _settings.TryGetValue(key, out var value) ? value : null;

    private Dictionary<string, string> LoadFromFile()
    {
        // Load configuration
        return new Dictionary<string, string>();
    }
}

// In DI container
services.AddSingleton<IConfigurationService>(ConfigurationService.Instance);",
                Tags = new() { "Creational", "Instance Control", "GoF" }
            },

            new() {
                Title = "Factory Method",
                Category = "Creational",
                Difficulty = "Medium",
                KeyConcepts = "Virtual constructor, defers instantiation to subclasses, product interface",
                UseCases = "When class can't anticipate object types, frameworks, plugins",
                Lesson = @"# Factory Method Pattern

## Intent
Define an interface for creating an object, but let subclasses decide which class to instantiate. Factory Method lets a class defer instantiation to subclasses.

## Problem It Solves
```csharp
// Without Factory Method - tight coupling
public class DocumentProcessor
{
    public void ProcessDocument(string type)
    {
        Document doc;

        // Client knows about all concrete types!
        if (type == ""pdf"")
            doc = new PdfDocument();
        else if (type == ""word"")
            doc = new WordDocument();
        else if (type == ""excel"")
            doc = new ExcelDocument();

        // Process...
    }
}
```

## Structure
1. **Product**: Interface for objects the factory creates
2. **ConcreteProduct**: Implements the Product interface
3. **Creator**: Declares the factory method
4. **ConcreteCreator**: Overrides factory method to return ConcreteProduct

## Implementation
```csharp
// Product interface
public interface IDocument
{
    void Open();
    void Save();
    void Close();
}

// Concrete Products
public class PdfDocument : IDocument
{
    public void Open() => Console.WriteLine(""Opening PDF"");
    public void Save() => Console.WriteLine(""Saving PDF"");
    public void Close() => Console.WriteLine(""Closing PDF"");
}

public class WordDocument : IDocument
{
    public void Open() => Console.WriteLine(""Opening Word"");
    public void Save() => Console.WriteLine(""Saving Word"");
    public void Close() => Console.WriteLine(""Closing Word"");
}

// Creator (abstract)
public abstract class DocumentCreator
{
    // Factory Method
    public abstract IDocument CreateDocument();

    // Other operations that use the factory method
    public void ProcessDocument()
    {
        var doc = CreateDocument();  // Factory method call
        doc.Open();
        // Process...
        doc.Save();
        doc.Close();
    }
}

// Concrete Creators
public class PdfDocumentCreator : DocumentCreator
{
    public override IDocument CreateDocument() => new PdfDocument();
}

public class WordDocumentCreator : DocumentCreator
{
    public override IDocument CreateDocument() => new WordDocument();
}

// Usage
DocumentCreator creator = new PdfDocumentCreator();
creator.ProcessDocument();  // Works with PDF

creator = new WordDocumentCreator();
creator.ProcessDocument();  // Works with Word
```

## Variations

### Parameterized Factory Method
```csharp
public abstract class LoggerFactory
{
    public abstract ILogger CreateLogger(string category);
}

public class FileLoggerFactory : LoggerFactory
{
    public override ILogger CreateLogger(string category)
    {
        return new FileLogger(category, $""{category}.log"");
    }
}
```

### Static Factory Method
```csharp
public class Connection
{
    private Connection(string connectionString) { }

    // Static factory methods
    public static Connection ForSqlServer(string server, string database)
    {
        return new Connection($""Server={server};Database={database}"");
    }

    public static Connection ForPostgres(string host, string database)
    {
        return new Connection($""Host={host};Database={database}"");
    }
}

// Usage
var conn = Connection.ForSqlServer(""localhost"", ""mydb"");
```

## Real-World Example: Payment Processing
```csharp
public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount);
    void Refund(string transactionId);
}

public abstract class PaymentProcessorFactory
{
    public abstract IPaymentProcessor CreateProcessor();

    public void HandlePayment(decimal amount)
    {
        var processor = CreateProcessor();
        processor.ProcessPayment(amount);
    }
}

public class StripePaymentFactory : PaymentProcessorFactory
{
    private readonly string _apiKey;

    public StripePaymentFactory(string apiKey) => _apiKey = apiKey;

    public override IPaymentProcessor CreateProcessor()
        => new StripePaymentProcessor(_apiKey);
}

public class PayPalPaymentFactory : PaymentProcessorFactory
{
    public override IPaymentProcessor CreateProcessor()
        => new PayPalPaymentProcessor();
}
```

## Benefits
1. Avoids tight coupling between creator and products
2. Single Responsibility: Product creation in one place
3. Open/Closed: Add new products without modifying existing code

## When to Use
- When you don't know exact types beforehand
- When you want to delegate creation to subclasses
- When you want to provide users a way to extend components",
                CodeExample = @"// Notification Factory Example
public interface INotification
{
    void Send(string message, string recipient);
}

public class EmailNotification : INotification
{
    public void Send(string message, string recipient)
        => Console.WriteLine($""Email to {recipient}: {message}"");
}

public class SmsNotification : INotification
{
    public void Send(string message, string recipient)
        => Console.WriteLine($""SMS to {recipient}: {message}"");
}

public class PushNotification : INotification
{
    public void Send(string message, string recipient)
        => Console.WriteLine($""Push to {recipient}: {message}"");
}

// Factory with registration
public class NotificationFactory
{
    private readonly Dictionary<string, Func<INotification>> _creators = new();

    public void Register(string type, Func<INotification> creator)
        => _creators[type] = creator;

    public INotification Create(string type)
        => _creators.TryGetValue(type, out var creator)
            ? creator()
            : throw new ArgumentException($""Unknown type: {type}"");
}

// Setup
var factory = new NotificationFactory();
factory.Register(""email"", () => new EmailNotification());
factory.Register(""sms"", () => new SmsNotification());
factory.Register(""push"", () => new PushNotification());

// Usage
var notification = factory.Create(""email"");
notification.Send(""Hello"", ""user@example.com"");",
                Tags = new() { "Creational", "Polymorphism", "GoF" }
            },

            new() {
                Title = "Repository Pattern",
                Category = "Architectural",
                Difficulty = "Medium",
                KeyConcepts = "Abstracts data access, collection-like interface, decouples business logic from data",
                UseCases = "Data access abstraction, unit testing, multiple data sources",
                Lesson = @"# Repository Pattern

## Intent
Mediates between the domain and data mapping layers using a collection-like interface for accessing domain objects.

## Why Use It?
- **Abstraction**: Hide data access implementation details
- **Testability**: Easy to mock for unit testing
- **Single Responsibility**: Separate data access from business logic
- **Maintainability**: Change data source without affecting business code

## Basic Repository Interface
```csharp
public interface IRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
```

## Generic Repository Implementation
```csharp
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public T GetById(int id) => _dbSet.Find(id);

    public IEnumerable<T> GetAll() => _dbSet.ToList();

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        => _dbSet.Where(predicate).ToList();

    public void Add(T entity) => _dbSet.Add(entity);

    public void AddRange(IEnumerable<T> entities) => _dbSet.AddRange(entities);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Remove(T entity) => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
}
```

## Specific Repository (with custom methods)
```csharp
public interface IOrderRepository : IRepository<Order>
{
    IEnumerable<Order> GetOrdersByCustomer(int customerId);
    IEnumerable<Order> GetRecentOrders(int days);
    Order GetOrderWithItems(int orderId);
    decimal GetTotalRevenue(DateTime from, DateTime to);
}

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context) { }

    public IEnumerable<Order> GetOrdersByCustomer(int customerId)
        => _dbSet.Where(o => o.CustomerId == customerId).ToList();

    public IEnumerable<Order> GetRecentOrders(int days)
        => _dbSet.Where(o => o.OrderDate >= DateTime.Now.AddDays(-days)).ToList();

    public Order GetOrderWithItems(int orderId)
        => _dbSet.Include(o => o.Items)
                 .FirstOrDefault(o => o.Id == orderId);

    public decimal GetTotalRevenue(DateTime from, DateTime to)
        => _dbSet.Where(o => o.OrderDate >= from && o.OrderDate <= to)
                 .Sum(o => o.Total);
}
```

## Unit of Work Pattern
Coordinates multiple repositories and ensures they share the same context.

```csharp
public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    ICustomerRepository Customers { get; }
    IProductRepository Products { get; }

    Task<int> SaveChangesAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Orders = new OrderRepository(_context);
        Customers = new CustomerRepository(_context);
        Products = new ProductRepository(_context);
    }

    public IOrderRepository Orders { get; }
    public ICustomerRepository Customers { get; }
    public IProductRepository Products { get; }

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
```

## Usage in Services
```csharp
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> CreateOrderAsync(int customerId, List<OrderItem> items)
    {
        var customer = _unitOfWork.Customers.GetById(customerId);
        if (customer == null)
            throw new NotFoundException(""Customer not found"");

        var order = new Order
        {
            CustomerId = customerId,
            Items = items,
            OrderDate = DateTime.UtcNow,
            Total = items.Sum(i => i.Price * i.Quantity)
        };

        _unitOfWork.Orders.Add(order);
        await _unitOfWork.SaveChangesAsync();

        return order;
    }
}
```

## Testing with Repository
```csharp
[Fact]
public async Task CreateOrder_ValidCustomer_CreatesOrder()
{
    // Arrange
    var mockUnitOfWork = new Mock<IUnitOfWork>();
    var mockOrderRepo = new Mock<IOrderRepository>();
    var mockCustomerRepo = new Mock<ICustomerRepository>();

    mockCustomerRepo.Setup(r => r.GetById(1))
        .Returns(new Customer { Id = 1, Name = ""Test"" });

    mockUnitOfWork.Setup(u => u.Customers).Returns(mockCustomerRepo.Object);
    mockUnitOfWork.Setup(u => u.Orders).Returns(mockOrderRepo.Object);

    var service = new OrderService(mockUnitOfWork.Object);

    // Act
    var result = await service.CreateOrderAsync(1, new List<OrderItem>());

    // Assert
    mockOrderRepo.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
    mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
}
```

## Pros and Cons

### Pros
- Decouples business logic from data access
- Promotes testability
- Single place for data access logic
- Easy to switch data sources

### Cons
- Can add unnecessary abstraction over EF Core
- May lead to leaky abstractions
- Additional code to maintain",
                CodeExample = @"// Complete Repository Example with Async
public interface IAsyncRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

public class AsyncRepository<T> : IAsyncRepository<T> where T : class
{
    protected readonly DbContext _context;

    public AsyncRepository(DbContext context) => _context = context;

    public async Task<T> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);

    public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _context.Set<T>().ToListAsync();

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _context.Set<T>().Where(predicate).ToListAsync();

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}",
                Tags = new() { "Data Access", "Abstraction", "Architecture" }
            },

            new() {
                Title = "Abstract Factory",
                Category = "Creational",
                Difficulty = "Hard",
                KeyConcepts = "Family of related objects, platform independence, product variants, factory of factories",
                UseCases = "UI themes, cross-platform development, database providers",
                Lesson = @"# Abstract Factory Pattern

## Intent
Provide an interface for creating families of related or dependent objects without specifying their concrete classes.

## Structure
```
┌─────────────────────┐      ┌─────────────────────┐
│  AbstractFactory    │      │   AbstractProductA  │
├─────────────────────┤      └─────────────────────┘
│ CreateProductA()    │               ▲
│ CreateProductB()    │      ┌────────┴────────┐
└─────────────────────┘      │                 │
         ▲              ProductA1         ProductA2
    ┌────┴────┐
    │         │
Factory1   Factory2
```

## Implementation
```csharp
// Abstract Products
public interface IButton
{
    void Render();
    void OnClick(Action handler);
}

public interface ITextBox
{
    void Render();
    string GetText();
    void SetText(string text);
}

public interface ICheckBox
{
    void Render();
    bool IsChecked { get; set; }
}

// Abstract Factory
public interface IUIFactory
{
    IButton CreateButton();
    ITextBox CreateTextBox();
    ICheckBox CreateCheckBox();
}

// Windows Concrete Products
public class WindowsButton : IButton
{
    public void Render() => Console.WriteLine(""[Windows Button]"");
    public void OnClick(Action handler) => handler();
}

public class WindowsTextBox : ITextBox
{
    private string _text = """";
    public void Render() => Console.WriteLine($""[Windows TextBox: {_text}]"");
    public string GetText() => _text;
    public void SetText(string text) => _text = text;
}

public class WindowsCheckBox : ICheckBox
{
    public bool IsChecked { get; set; }
    public void Render() => Console.WriteLine($""[Windows CheckBox: {(IsChecked ? ""☑"" : ""☐"")}]"");
}

// Mac Concrete Products
public class MacButton : IButton
{
    public void Render() => Console.WriteLine(""(Mac Button)"");
    public void OnClick(Action handler) => handler();
}

public class MacTextBox : ITextBox
{
    private string _text = """";
    public void Render() => Console.WriteLine($""(Mac TextBox: {_text})"");
    public string GetText() => _text;
    public void SetText(string text) => _text = text;
}

public class MacCheckBox : ICheckBox
{
    public bool IsChecked { get; set; }
    public void Render() => Console.WriteLine($""(Mac CheckBox: {(IsChecked ? ""✓"" : ""○"")})"");
}

// Concrete Factories
public class WindowsUIFactory : IUIFactory
{
    public IButton CreateButton() => new WindowsButton();
    public ITextBox CreateTextBox() => new WindowsTextBox();
    public ICheckBox CreateCheckBox() => new WindowsCheckBox();
}

public class MacUIFactory : IUIFactory
{
    public IButton CreateButton() => new MacButton();
    public ITextBox CreateTextBox() => new MacTextBox();
    public ICheckBox CreateCheckBox() => new MacCheckBox();
}
```

## Usage
```csharp
public class Application
{
    private readonly IButton _button;
    private readonly ITextBox _textBox;
    private readonly ICheckBox _checkBox;

    public Application(IUIFactory factory)
    {
        // Client works with factory interface only
        _button = factory.CreateButton();
        _textBox = factory.CreateTextBox();
        _checkBox = factory.CreateCheckBox();
    }

    public void Render()
    {
        _button.Render();
        _textBox.Render();
        _checkBox.Render();
    }
}

// Factory selection based on environment
IUIFactory factory = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
    ? new WindowsUIFactory()
    : new MacUIFactory();

var app = new Application(factory);
app.Render();
```

## Real-World Example: Database Providers
```csharp
public interface IDbConnection { void Open(); void Close(); }
public interface IDbCommand { void Execute(string sql); }
public interface IDbTransaction { void Commit(); void Rollback(); }

public interface IDatabaseFactory
{
    IDbConnection CreateConnection(string connectionString);
    IDbCommand CreateCommand();
    IDbTransaction CreateTransaction();
}

public class SqlServerFactory : IDatabaseFactory { /* ... */ }
public class PostgresFactory : IDatabaseFactory { /* ... */ }
public class MySqlFactory : IDatabaseFactory { /* ... */ }
```

## Abstract Factory vs Factory Method
| Abstract Factory | Factory Method |
|------------------|----------------|
| Creates families of products | Creates single product |
| Multiple factory methods | Single factory method |
| Related products only | Any product |
| More complex | Simpler |",
                CodeExample = @"// Theme-based UI Factory
public interface ITheme
{
    IUIFactory CreateUIFactory();
    string PrimaryColor { get; }
    string SecondaryColor { get; }
}

public class LightTheme : ITheme
{
    public IUIFactory CreateUIFactory() => new LightUIFactory();
    public string PrimaryColor => ""#FFFFFF"";
    public string SecondaryColor => ""#000000"";
}

public class DarkTheme : ITheme
{
    public IUIFactory CreateUIFactory() => new DarkUIFactory();
    public string PrimaryColor => ""#1E1E1E"";
    public string SecondaryColor => ""#FFFFFF"";
}

// Usage with DI
public class ThemeService
{
    private readonly Dictionary<string, ITheme> _themes = new()
    {
        [""light""] = new LightTheme(),
        [""dark""] = new DarkTheme()
    };

    public ITheme GetTheme(string name)
        => _themes.TryGetValue(name, out var theme) ? theme : _themes[""light""];
}

// In Startup
services.AddSingleton<ThemeService>();
services.AddScoped<IUIFactory>(sp =>
{
    var themeService = sp.GetRequiredService<ThemeService>();
    var userPreference = GetUserPreference(); // From user settings
    return themeService.GetTheme(userPreference).CreateUIFactory();
});",
                Tags = new() { "Creational", "Family of Objects", "GoF" }
            },

            new() {
                Title = "Builder",
                Category = "Creational",
                Difficulty = "Medium",
                KeyConcepts = "Step-by-step construction, fluent interface, director, immutable objects",
                UseCases = "Complex object creation, configuration objects, test data builders",
                Lesson = @"# Builder Pattern

## Intent
Separate the construction of a complex object from its representation, allowing the same construction process to create different representations.

## When to Use
- Object has many optional parameters
- Object requires multiple steps to create
- Object should be immutable after creation
- Same construction process for different representations

## Basic Implementation
```csharp
public class Pizza
{
    public string Dough { get; }
    public string Sauce { get; }
    public List<string> Toppings { get; }
    public string Size { get; }
    public bool ExtraCheese { get; }

    // Private constructor - only Builder can create
    private Pizza(PizzaBuilder builder)
    {
        Dough = builder.Dough;
        Sauce = builder.Sauce;
        Toppings = builder.Toppings.ToList();
        Size = builder.Size;
        ExtraCheese = builder.ExtraCheese;
    }

    public class PizzaBuilder
    {
        public string Dough { get; private set; } = ""Regular"";
        public string Sauce { get; private set; } = ""Tomato"";
        public List<string> Toppings { get; } = new();
        public string Size { get; private set; } = ""Medium"";
        public bool ExtraCheese { get; private set; }

        public PizzaBuilder WithDough(string dough)
        {
            Dough = dough;
            return this;
        }

        public PizzaBuilder WithSauce(string sauce)
        {
            Sauce = sauce;
            return this;
        }

        public PizzaBuilder AddTopping(string topping)
        {
            Toppings.Add(topping);
            return this;
        }

        public PizzaBuilder WithSize(string size)
        {
            Size = size;
            return this;
        }

        public PizzaBuilder WithExtraCheese()
        {
            ExtraCheese = true;
            return this;
        }

        public Pizza Build() => new Pizza(this);
    }
}

// Usage - Fluent API
var pizza = new Pizza.PizzaBuilder()
    .WithDough(""Thin Crust"")
    .WithSauce(""BBQ"")
    .AddTopping(""Chicken"")
    .AddTopping(""Onions"")
    .AddTopping(""Peppers"")
    .WithSize(""Large"")
    .WithExtraCheese()
    .Build();
```

## With Director
```csharp
public class PizzaDirector
{
    public Pizza BuildMargherita(Pizza.PizzaBuilder builder)
    {
        return builder
            .WithDough(""Regular"")
            .WithSauce(""Tomato"")
            .AddTopping(""Mozzarella"")
            .AddTopping(""Basil"")
            .Build();
    }

    public Pizza BuildMeatLovers(Pizza.PizzaBuilder builder)
    {
        return builder
            .WithDough(""Thick"")
            .WithSauce(""Tomato"")
            .AddTopping(""Pepperoni"")
            .AddTopping(""Sausage"")
            .AddTopping(""Bacon"")
            .AddTopping(""Ham"")
            .WithExtraCheese()
            .Build();
    }
}

// Usage
var director = new PizzaDirector();
var margherita = director.BuildMargherita(new Pizza.PizzaBuilder());
```

## Real-World: HttpRequestMessage Builder
```csharp
public class HttpRequestBuilder
{
    private HttpMethod _method = HttpMethod.Get;
    private string _url;
    private readonly Dictionary<string, string> _headers = new();
    private HttpContent _content;
    private TimeSpan? _timeout;

    public HttpRequestBuilder WithMethod(HttpMethod method)
    {
        _method = method;
        return this;
    }

    public HttpRequestBuilder WithUrl(string url)
    {
        _url = url;
        return this;
    }

    public HttpRequestBuilder WithHeader(string name, string value)
    {
        _headers[name] = value;
        return this;
    }

    public HttpRequestBuilder WithBearerToken(string token)
    {
        _headers[""Authorization""] = $""Bearer {token}"";
        return this;
    }

    public HttpRequestBuilder WithJsonBody<T>(T body)
    {
        _content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            ""application/json"");
        return this;
    }

    public HttpRequestBuilder WithTimeout(TimeSpan timeout)
    {
        _timeout = timeout;
        return this;
    }

    public HttpRequestMessage Build()
    {
        var request = new HttpRequestMessage(_method, _url);

        foreach (var header in _headers)
            request.Headers.Add(header.Key, header.Value);

        if (_content != null)
            request.Content = _content;

        return request;
    }
}

// Usage
var request = new HttpRequestBuilder()
    .WithMethod(HttpMethod.Post)
    .WithUrl(""https://api.example.com/users"")
    .WithBearerToken(""my-token"")
    .WithJsonBody(new { Name = ""John"", Email = ""john@example.com"" })
    .Build();
```

## Validation in Builder
```csharp
public class EmailBuilder
{
    private string _to;
    private string _from;
    private string _subject;
    private string _body;
    private readonly List<string> _errors = new();

    public EmailBuilder To(string email)
    {
        if (!IsValidEmail(email))
            _errors.Add($""Invalid 'to' email: {email}"");
        _to = email;
        return this;
    }

    public Email Build()
    {
        if (_errors.Any())
            throw new InvalidOperationException(
                $""Cannot build email: {string.Join("", "", _errors)}"");

        return new Email(_to, _from, _subject, _body);
    }
}
```",
                CodeExample = @"// Test Data Builder Pattern
public class CustomerBuilder
{
    private int _id = 1;
    private string _name = ""John Doe"";
    private string _email = ""john@example.com"";
    private string _phone = ""555-1234"";
    private Address _address = new Address(""123 Main St"", ""NYC"", ""NY"", ""10001"");
    private CustomerType _type = CustomerType.Regular;
    private bool _isActive = true;
    private decimal _creditLimit = 1000m;

    public CustomerBuilder WithId(int id) { _id = id; return this; }
    public CustomerBuilder WithName(string name) { _name = name; return this; }
    public CustomerBuilder WithEmail(string email) { _email = email; return this; }
    public CustomerBuilder WithPhone(string phone) { _phone = phone; return this; }
    public CustomerBuilder WithAddress(Address address) { _address = address; return this; }
    public CustomerBuilder AsPremium() { _type = CustomerType.Premium; _creditLimit = 10000m; return this; }
    public CustomerBuilder AsInactive() { _isActive = false; return this; }
    public CustomerBuilder WithCreditLimit(decimal limit) { _creditLimit = limit; return this; }

    public Customer Build() => new Customer
    {
        Id = _id, Name = _name, Email = _email, Phone = _phone,
        Address = _address, Type = _type, IsActive = _isActive, CreditLimit = _creditLimit
    };

    // Factory methods for common scenarios
    public static CustomerBuilder Default() => new CustomerBuilder();
    public static CustomerBuilder Premium() => new CustomerBuilder().AsPremium();
    public static CustomerBuilder Inactive() => new CustomerBuilder().AsInactive();
}

// Usage in tests
[Fact]
public void Premium_Customer_Gets_Discount()
{
    var customer = CustomerBuilder.Premium()
        .WithName(""Jane Smith"")
        .Build();

    var discount = _discountService.CalculateDiscount(customer);

    Assert.Equal(0.20m, discount);
}",
                Tags = new() { "Creational", "Fluent Interface", "GoF" }
            },

            new() {
                Title = "Strategy",
                Category = "Behavioral",
                Difficulty = "Easy",
                KeyConcepts = "Interchangeable algorithms, runtime behavior change, composition over inheritance",
                UseCases = "Payment processing, sorting algorithms, validation rules, pricing strategies",
                Lesson = @"# Strategy Pattern

## Intent
Define a family of algorithms, encapsulate each one, and make them interchangeable. Strategy lets the algorithm vary independently from clients that use it.

## Structure
```
┌─────────────┐     ┌─────────────────┐
│   Context   │────▶│   IStrategy     │
├─────────────┤     ├─────────────────┤
│ SetStrategy │     │ Execute()       │
│ Execute()   │     └─────────────────┘
└─────────────┘              ▲
                    ┌────────┼────────┐
                    │        │        │
              StrategyA  StrategyB  StrategyC
```

## Implementation
```csharp
// Strategy interface
public interface IPaymentStrategy
{
    void Pay(decimal amount);
    bool ValidatePaymentDetails();
}

// Concrete strategies
public class CreditCardPayment : IPaymentStrategy
{
    private readonly string _cardNumber;
    private readonly string _cvv;
    private readonly string _expiry;

    public CreditCardPayment(string cardNumber, string cvv, string expiry)
    {
        _cardNumber = cardNumber;
        _cvv = cvv;
        _expiry = expiry;
    }

    public bool ValidatePaymentDetails()
        => _cardNumber.Length == 16 && _cvv.Length == 3;

    public void Pay(decimal amount)
    {
        Console.WriteLine($""Paid ${amount} using Credit Card ending in {_cardNumber[^4..]}"");
    }
}

public class PayPalPayment : IPaymentStrategy
{
    private readonly string _email;

    public PayPalPayment(string email) => _email = email;

    public bool ValidatePaymentDetails() => _email.Contains(""@"");

    public void Pay(decimal amount)
    {
        Console.WriteLine($""Paid ${amount} using PayPal account {_email}"");
    }
}

public class CryptoPayment : IPaymentStrategy
{
    private readonly string _walletAddress;

    public CryptoPayment(string walletAddress) => _walletAddress = walletAddress;

    public bool ValidatePaymentDetails() => _walletAddress.StartsWith(""0x"");

    public void Pay(decimal amount)
    {
        Console.WriteLine($""Paid ${amount} in crypto to {_walletAddress}"");
    }
}

// Context
public class PaymentProcessor
{
    private IPaymentStrategy _strategy;

    public void SetStrategy(IPaymentStrategy strategy)
    {
        _strategy = strategy;
    }

    public bool ProcessPayment(decimal amount)
    {
        if (_strategy == null)
            throw new InvalidOperationException(""Payment strategy not set"");

        if (!_strategy.ValidatePaymentDetails())
            return false;

        _strategy.Pay(amount);
        return true;
    }
}

// Usage
var processor = new PaymentProcessor();

// Pay with credit card
processor.SetStrategy(new CreditCardPayment(""1234567890123456"", ""123"", ""12/25""));
processor.ProcessPayment(99.99m);

// Switch to PayPal
processor.SetStrategy(new PayPalPayment(""user@example.com""));
processor.ProcessPayment(49.99m);
```

## With Dependency Injection
```csharp
// Register all strategies
services.AddTransient<CreditCardPayment>();
services.AddTransient<PayPalPayment>();
services.AddTransient<CryptoPayment>();

// Strategy resolver
services.AddTransient<Func<string, IPaymentStrategy>>(sp => key =>
{
    return key switch
    {
        ""credit"" => sp.GetRequiredService<CreditCardPayment>(),
        ""paypal"" => sp.GetRequiredService<PayPalPayment>(),
        ""crypto"" => sp.GetRequiredService<CryptoPayment>(),
        _ => throw new ArgumentException($""Unknown payment type: {key}"")
    };
});

// Usage in controller
public class PaymentController : ControllerBase
{
    private readonly Func<string, IPaymentStrategy> _strategyResolver;

    public PaymentController(Func<string, IPaymentStrategy> resolver)
    {
        _strategyResolver = resolver;
    }

    [HttpPost]
    public IActionResult Pay(string paymentType, decimal amount)
    {
        var strategy = _strategyResolver(paymentType);
        strategy.Pay(amount);
        return Ok();
    }
}
```

## Real-World: Sorting Strategies
```csharp
public interface ISortStrategy<T>
{
    IEnumerable<T> Sort(IEnumerable<T> items);
}

public class QuickSortStrategy<T> : ISortStrategy<T> where T : IComparable<T>
{
    public IEnumerable<T> Sort(IEnumerable<T> items)
    {
        var list = items.ToList();
        QuickSort(list, 0, list.Count - 1);
        return list;
    }
    // QuickSort implementation...
}

public class MergeSortStrategy<T> : ISortStrategy<T> where T : IComparable<T>
{
    public IEnumerable<T> Sort(IEnumerable<T> items)
    {
        // MergeSort implementation...
    }
}

// Choose strategy based on data size
public class AdaptiveSorter<T> where T : IComparable<T>
{
    public IEnumerable<T> Sort(IEnumerable<T> items)
    {
        ISortStrategy<T> strategy = items.Count() > 1000
            ? new QuickSortStrategy<T>()
            : new MergeSortStrategy<T>();

        return strategy.Sort(items);
    }
}
```",
                CodeExample = @"// Discount Strategy Example
public interface IDiscountStrategy
{
    decimal CalculateDiscount(Order order);
    string Description { get; }
}

public class NoDiscount : IDiscountStrategy
{
    public string Description => ""No discount"";
    public decimal CalculateDiscount(Order order) => 0;
}

public class PercentageDiscount : IDiscountStrategy
{
    private readonly decimal _percentage;
    public PercentageDiscount(decimal percentage) => _percentage = percentage;
    public string Description => $""{_percentage}% off"";
    public decimal CalculateDiscount(Order order) => order.Total * (_percentage / 100);
}

public class FlatDiscount : IDiscountStrategy
{
    private readonly decimal _amount;
    public FlatDiscount(decimal amount) => _amount = amount;
    public string Description => $""${_amount} off"";
    public decimal CalculateDiscount(Order order) => Math.Min(_amount, order.Total);
}

public class BuyXGetYFree : IDiscountStrategy
{
    private readonly int _buyCount;
    private readonly int _freeCount;
    public BuyXGetYFree(int buy, int free) { _buyCount = buy; _freeCount = free; }
    public string Description => $""Buy {_buyCount} Get {_freeCount} Free"";
    public decimal CalculateDiscount(Order order)
    {
        var cheapestItem = order.Items.Min(i => i.Price);
        var sets = order.Items.Count / (_buyCount + _freeCount);
        return cheapestItem * sets * _freeCount;
    }
}

// Strategy selection based on conditions
public class DiscountEngine
{
    public IDiscountStrategy SelectBestDiscount(Order order, Customer customer)
    {
        var strategies = new List<IDiscountStrategy>
        {
            new NoDiscount()
        };

        if (customer.IsPremium)
            strategies.Add(new PercentageDiscount(15));

        if (order.Total > 100)
            strategies.Add(new PercentageDiscount(10));

        if (order.Items.Count >= 3)
            strategies.Add(new BuyXGetYFree(2, 1));

        // Return strategy with highest discount
        return strategies.MaxBy(s => s.CalculateDiscount(order));
    }
}",
                Tags = new() { "Behavioral", "Algorithm", "GoF" }
            },

            new() {
                Title = "Observer",
                Category = "Behavioral",
                Difficulty = "Medium",
                KeyConcepts = "Publish-subscribe, event handling, loose coupling, one-to-many dependency",
                UseCases = "Event systems, UI updates, notifications, real-time data",
                Lesson = @"# Observer Pattern

## Intent
Define a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.

## Structure
```
┌─────────────┐      ┌─────────────────┐
│   Subject   │─────▶│    IObserver    │
├─────────────┤      ├─────────────────┤
│ Attach()    │      │ Update()        │
│ Detach()    │      └─────────────────┘
│ Notify()    │               ▲
└─────────────┘      ┌────────┼────────┐
                     │        │        │
               ObserverA  ObserverB  ObserverC
```

## Implementation
```csharp
// Observer interface
public interface IObserver<T>
{
    void Update(T data);
}

// Subject interface
public interface ISubject<T>
{
    void Attach(IObserver<T> observer);
    void Detach(IObserver<T> observer);
    void Notify();
}

// Concrete Subject
public class StockTicker : ISubject<StockPrice>
{
    private readonly List<IObserver<StockPrice>> _observers = new();
    private StockPrice _currentPrice;

    public StockPrice CurrentPrice
    {
        get => _currentPrice;
        set
        {
            _currentPrice = value;
            Notify();
        }
    }

    public void Attach(IObserver<StockPrice> observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver<StockPrice> observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_currentPrice);
        }
    }
}

public record StockPrice(string Symbol, decimal Price, DateTime Timestamp);

// Concrete Observers
public class StockDisplay : IObserver<StockPrice>
{
    private readonly string _name;

    public StockDisplay(string name) => _name = name;

    public void Update(StockPrice data)
    {
        Console.WriteLine($""[{_name}] {data.Symbol}: ${data.Price} at {data.Timestamp:HH:mm:ss}"");
    }
}

public class PriceAlertObserver : IObserver<StockPrice>
{
    private readonly string _symbol;
    private readonly decimal _targetPrice;
    private readonly Action<StockPrice> _alertAction;

    public PriceAlertObserver(string symbol, decimal targetPrice, Action<StockPrice> alertAction)
    {
        _symbol = symbol;
        _targetPrice = targetPrice;
        _alertAction = alertAction;
    }

    public void Update(StockPrice data)
    {
        if (data.Symbol == _symbol && data.Price >= _targetPrice)
        {
            _alertAction(data);
        }
    }
}

// Usage
var ticker = new StockTicker();

var display1 = new StockDisplay(""Main Display"");
var display2 = new StockDisplay(""Mobile App"");
var alert = new PriceAlertObserver(""AAPL"", 150m, p =>
    Console.WriteLine($""ALERT: {p.Symbol} reached ${p.Price}!""));

ticker.Attach(display1);
ticker.Attach(display2);
ticker.Attach(alert);

ticker.CurrentPrice = new StockPrice(""AAPL"", 145m, DateTime.Now);
ticker.CurrentPrice = new StockPrice(""AAPL"", 152m, DateTime.Now);

ticker.Detach(display2);
```

## Using .NET Events
```csharp
public class StockTickerWithEvents
{
    public event EventHandler<StockPrice> PriceChanged;

    private StockPrice _currentPrice;
    public StockPrice CurrentPrice
    {
        get => _currentPrice;
        set
        {
            _currentPrice = value;
            OnPriceChanged(value);
        }
    }

    protected virtual void OnPriceChanged(StockPrice price)
    {
        PriceChanged?.Invoke(this, price);
    }
}

// Usage
var ticker = new StockTickerWithEvents();

ticker.PriceChanged += (sender, price) =>
    Console.WriteLine($""{price.Symbol}: ${price.Price}"");

ticker.PriceChanged += (sender, price) =>
{
    if (price.Price > 150)
        SendNotification(price);
};
```

## Using IObservable<T> (Reactive)
```csharp
public class ReactiveStockTicker : IObservable<StockPrice>
{
    private readonly List<IObserver<StockPrice>> _observers = new();

    public IDisposable Subscribe(IObserver<StockPrice> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void PublishPrice(StockPrice price)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(price);
        }
    }

    public void Complete()
    {
        foreach (var observer in _observers)
        {
            observer.OnCompleted();
        }
    }

    private class Unsubscriber : IDisposable
    {
        private readonly List<IObserver<StockPrice>> _observers;
        private readonly IObserver<StockPrice> _observer;

        public Unsubscriber(List<IObserver<StockPrice>> observers, IObserver<StockPrice> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose() => _observers.Remove(_observer);
    }
}
```",
                CodeExample = @"// Real-world: Order Status Notification System
public interface IOrderObserver
{
    void OnOrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus);
}

public class Order
{
    private readonly List<IOrderObserver> _observers = new();
    private OrderStatus _status = OrderStatus.Created;

    public string OrderId { get; }
    public string CustomerEmail { get; }

    public Order(string orderId, string customerEmail)
    {
        OrderId = orderId;
        CustomerEmail = customerEmail;
    }

    public OrderStatus Status
    {
        get => _status;
        set
        {
            var oldStatus = _status;
            _status = value;
            NotifyObservers(oldStatus, value);
        }
    }

    public void Subscribe(IOrderObserver observer) => _observers.Add(observer);
    public void Unsubscribe(IOrderObserver observer) => _observers.Remove(observer);

    private void NotifyObservers(OrderStatus oldStatus, OrderStatus newStatus)
    {
        foreach (var observer in _observers)
            observer.OnOrderStatusChanged(this, oldStatus, newStatus);
    }
}

public class EmailNotifier : IOrderObserver
{
    private readonly IEmailService _emailService;

    public EmailNotifier(IEmailService emailService) => _emailService = emailService;

    public void OnOrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus)
    {
        var message = newStatus switch
        {
            OrderStatus.Confirmed => ""Your order has been confirmed!"",
            OrderStatus.Shipped => ""Your order has been shipped!"",
            OrderStatus.Delivered => ""Your order has been delivered!"",
            _ => null
        };

        if (message != null)
            _emailService.Send(order.CustomerEmail, $""Order {order.OrderId}"", message);
    }
}

public class InventoryUpdater : IOrderObserver
{
    public void OnOrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus)
    {
        if (newStatus == OrderStatus.Confirmed)
            ReserveInventory(order);
        else if (newStatus == OrderStatus.Cancelled)
            ReleaseInventory(order);
    }
}

public class AnalyticsTracker : IOrderObserver
{
    public void OnOrderStatusChanged(Order order, OrderStatus oldStatus, OrderStatus newStatus)
    {
        TrackEvent(""OrderStatusChanged"", new { order.OrderId, oldStatus, newStatus });
    }
}

// Setup
var order = new Order(""ORD-123"", ""customer@example.com"");
order.Subscribe(new EmailNotifier(emailService));
order.Subscribe(new InventoryUpdater());
order.Subscribe(new AnalyticsTracker());

// Status changes trigger all observers
order.Status = OrderStatus.Confirmed;
order.Status = OrderStatus.Shipped;",
                Tags = new() { "Behavioral", "Event Handling", "GoF" }
            },

            new() {
                Title = "Decorator",
                Category = "Structural",
                Difficulty = "Medium",
                KeyConcepts = "Wrapper, dynamic behavior addition, single responsibility, composition",
                UseCases = "Logging, caching, validation, compression, encryption",
                Lesson = @"# Decorator Pattern

## Intent
Attach additional responsibilities to an object dynamically. Decorators provide a flexible alternative to subclassing for extending functionality.

## Structure
```
        ┌─────────────────┐
        │   IComponent    │
        ├─────────────────┤
        │ Operation()     │
        └─────────────────┘
               ▲
      ┌────────┴────────┐
      │                 │
┌─────────────┐  ┌──────────────┐
│  Concrete   │  │  Decorator   │─────┐
│  Component  │  ├──────────────┤     │
└─────────────┘  │ _component   │◀────┘
                 │ Operation()  │
                 └──────────────┘
                        ▲
               ┌────────┴────────┐
               │                 │
         DecoratorA         DecoratorB
```

## Implementation
```csharp
// Component interface
public interface IDataSource
{
    void WriteData(string data);
    string ReadData();
}

// Concrete component
public class FileDataSource : IDataSource
{
    private readonly string _filename;

    public FileDataSource(string filename) => _filename = filename;

    public void WriteData(string data)
    {
        File.WriteAllText(_filename, data);
    }

    public string ReadData()
    {
        return File.Exists(_filename) ? File.ReadAllText(_filename) : """";
    }
}

// Base decorator
public abstract class DataSourceDecorator : IDataSource
{
    protected readonly IDataSource _wrappee;

    protected DataSourceDecorator(IDataSource source) => _wrappee = source;

    public virtual void WriteData(string data) => _wrappee.WriteData(data);
    public virtual string ReadData() => _wrappee.ReadData();
}

// Concrete decorators
public class EncryptionDecorator : DataSourceDecorator
{
    public EncryptionDecorator(IDataSource source) : base(source) { }

    public override void WriteData(string data)
    {
        var encrypted = Encrypt(data);
        base.WriteData(encrypted);
    }

    public override string ReadData()
    {
        var data = base.ReadData();
        return Decrypt(data);
    }

    private string Encrypt(string data) => Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
    private string Decrypt(string data) => Encoding.UTF8.GetString(Convert.FromBase64String(data));
}

public class CompressionDecorator : DataSourceDecorator
{
    public CompressionDecorator(IDataSource source) : base(source) { }

    public override void WriteData(string data)
    {
        var compressed = Compress(data);
        base.WriteData(compressed);
    }

    public override string ReadData()
    {
        var data = base.ReadData();
        return Decompress(data);
    }

    private string Compress(string data) { /* GZip compression */ return data; }
    private string Decompress(string data) { /* GZip decompression */ return data; }
}

// Usage - decorators can be stacked
IDataSource source = new FileDataSource(""data.txt"");
source = new CompressionDecorator(source);  // Add compression
source = new EncryptionDecorator(source);   // Add encryption on top

source.WriteData(""Sensitive data"");  // Encrypts, then compresses, then writes
var data = source.ReadData();         // Reads, decompresses, then decrypts
```

## Real-World: HTTP Handler Pipeline
```csharp
public interface IHttpHandler
{
    Task<HttpResponse> HandleAsync(HttpRequest request);
}

public class BaseHttpHandler : IHttpHandler
{
    public async Task<HttpResponse> HandleAsync(HttpRequest request)
    {
        // Actual HTTP processing
        return await SendRequestAsync(request);
    }
}

public class LoggingDecorator : IHttpHandler
{
    private readonly IHttpHandler _inner;
    private readonly ILogger _logger;

    public LoggingDecorator(IHttpHandler inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<HttpResponse> HandleAsync(HttpRequest request)
    {
        _logger.LogInformation($""Request: {request.Method} {request.Url}"");
        var stopwatch = Stopwatch.StartNew();

        var response = await _inner.HandleAsync(request);

        _logger.LogInformation($""Response: {response.StatusCode} in {stopwatch.ElapsedMilliseconds}ms"");
        return response;
    }
}

public class RetryDecorator : IHttpHandler
{
    private readonly IHttpHandler _inner;
    private readonly int _maxRetries;

    public RetryDecorator(IHttpHandler inner, int maxRetries = 3)
    {
        _inner = inner;
        _maxRetries = maxRetries;
    }

    public async Task<HttpResponse> HandleAsync(HttpRequest request)
    {
        for (int i = 0; i <= _maxRetries; i++)
        {
            try
            {
                return await _inner.HandleAsync(request);
            }
            catch when (i < _maxRetries)
            {
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, i)));
            }
        }
        throw new HttpRequestException(""Max retries exceeded"");
    }
}

public class CachingDecorator : IHttpHandler
{
    private readonly IHttpHandler _inner;
    private readonly ICache _cache;

    public CachingDecorator(IHttpHandler inner, ICache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<HttpResponse> HandleAsync(HttpRequest request)
    {
        if (request.Method == ""GET"")
        {
            var cached = _cache.Get<HttpResponse>(request.Url);
            if (cached != null) return cached;
        }

        var response = await _inner.HandleAsync(request);

        if (request.Method == ""GET"" && response.IsSuccess)
            _cache.Set(request.Url, response, TimeSpan.FromMinutes(5));

        return response;
    }
}

// Build pipeline
IHttpHandler handler = new BaseHttpHandler();
handler = new CachingDecorator(handler, cache);
handler = new RetryDecorator(handler, maxRetries: 3);
handler = new LoggingDecorator(handler, logger);
```",
                CodeExample = @"// Stream decorators in .NET
public class AuditingStream : Stream
{
    private readonly Stream _inner;
    private readonly ILogger _logger;
    private long _bytesRead;
    private long _bytesWritten;

    public AuditingStream(Stream inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var bytesRead = _inner.Read(buffer, offset, count);
        _bytesRead += bytesRead;
        _logger.LogDebug($""Read {bytesRead} bytes (total: {_bytesRead})"");
        return bytesRead;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _inner.Write(buffer, offset, count);
        _bytesWritten += count;
        _logger.LogDebug($""Wrote {count} bytes (total: {_bytesWritten})"");
    }

    // Delegate other members to _inner...
    public override bool CanRead => _inner.CanRead;
    public override bool CanSeek => _inner.CanSeek;
    public override bool CanWrite => _inner.CanWrite;
    public override long Length => _inner.Length;
    public override long Position
    {
        get => _inner.Position;
        set => _inner.Position = value;
    }
    public override void Flush() => _inner.Flush();
    public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
    public override void SetLength(long value) => _inner.SetLength(value);
}

// Usage with built-in .NET decorators
Stream stream = new FileStream(""data.bin"", FileMode.Create);
stream = new BufferedStream(stream);      // Add buffering
stream = new GZipStream(stream, CompressionMode.Compress);  // Add compression
stream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);  // Add encryption
stream = new AuditingStream(stream, logger);  // Add our custom auditing",
                Tags = new() { "Structural", "Wrapper", "GoF" }
            },

            new() {
                Title = "Adapter",
                Category = "Structural",
                Difficulty = "Easy",
                KeyConcepts = "Interface conversion, legacy integration, wrapper, bridge between incompatible interfaces",
                UseCases = "Legacy system integration, third-party libraries, API versioning",
                Lesson = @"# Adapter Pattern

## Intent
Convert the interface of a class into another interface clients expect. Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.

## Types of Adapters
1. **Object Adapter** - Uses composition (preferred)
2. **Class Adapter** - Uses inheritance (less flexible)

## Implementation
```csharp
// Target interface (what client expects)
public interface ILogger
{
    void Log(string message);
    void LogError(string message, Exception ex);
    void LogWarning(string message);
}

// Adaptee (legacy/third-party class with incompatible interface)
public class LegacyLogger
{
    public void WriteLog(int severity, string msg)
    {
        Console.WriteLine($""[{severity}] {msg}"");
    }
}

// Another adaptee
public class ThirdPartyLogger
{
    public void Write(LogEntry entry)
    {
        Console.WriteLine($""{entry.Timestamp}: {entry.Level} - {entry.Message}"");
    }
}

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
}

// Object Adapter for legacy logger
public class LegacyLoggerAdapter : ILogger
{
    private readonly LegacyLogger _legacyLogger;

    public LegacyLoggerAdapter(LegacyLogger legacyLogger)
    {
        _legacyLogger = legacyLogger;
    }

    public void Log(string message) => _legacyLogger.WriteLog(1, message);
    public void LogError(string message, Exception ex)
        => _legacyLogger.WriteLog(3, $""{message}: {ex.Message}"");
    public void LogWarning(string message) => _legacyLogger.WriteLog(2, message);
}

// Object Adapter for third-party logger
public class ThirdPartyLoggerAdapter : ILogger
{
    private readonly ThirdPartyLogger _logger;

    public ThirdPartyLoggerAdapter(ThirdPartyLogger logger)
    {
        _logger = logger;
    }

    public void Log(string message)
    {
        _logger.Write(new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = ""INFO"",
            Message = message
        });
    }

    public void LogError(string message, Exception ex)
    {
        _logger.Write(new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = ""ERROR"",
            Message = $""{message}: {ex.Message}""
        });
    }

    public void LogWarning(string message)
    {
        _logger.Write(new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = ""WARN"",
            Message = message
        });
    }
}

// Usage
ILogger logger = new LegacyLoggerAdapter(new LegacyLogger());
logger.Log(""Application started"");
logger.LogError(""Failed to connect"", new Exception(""Connection timeout""));
```

## Real-World: Payment Gateway Adapters
```csharp
// Our unified payment interface
public interface IPaymentGateway
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    Task<RefundResult> RefundAsync(string transactionId, decimal amount);
}

// Stripe's API (different interface)
public class StripeClient
{
    public async Task<StripeCharge> CreateChargeAsync(
        long amountInCents,
        string currency,
        string cardToken) { /* ... */ }

    public async Task<StripeRefund> CreateRefundAsync(
        string chargeId,
        long amountInCents) { /* ... */ }
}

// PayPal's API (completely different)
public class PayPalClient
{
    public async Task<PayPalPayment> ExecutePaymentAsync(
        PayPalOrder order) { /* ... */ }

    public async Task<PayPalRefund> RefundPaymentAsync(
        string saleId,
        PayPalAmount amount) { /* ... */ }
}

// Stripe Adapter
public class StripeAdapter : IPaymentGateway
{
    private readonly StripeClient _stripe;

    public StripeAdapter(StripeClient stripe) => _stripe = stripe;

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        var charge = await _stripe.CreateChargeAsync(
            (long)(request.Amount * 100),  // Convert to cents
            request.Currency,
            request.CardToken);

        return new PaymentResult
        {
            Success = charge.Status == ""succeeded"",
            TransactionId = charge.Id,
            Message = charge.Status
        };
    }

    public async Task<RefundResult> RefundAsync(string transactionId, decimal amount)
    {
        var refund = await _stripe.CreateRefundAsync(
            transactionId,
            (long)(amount * 100));

        return new RefundResult
        {
            Success = refund.Status == ""succeeded"",
            RefundId = refund.Id
        };
    }
}

// PayPal Adapter
public class PayPalAdapter : IPaymentGateway
{
    private readonly PayPalClient _paypal;

    public PayPalAdapter(PayPalClient paypal) => _paypal = paypal;

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        var order = new PayPalOrder
        {
            Amount = new PayPalAmount { Value = request.Amount, Currency = request.Currency }
        };

        var payment = await _paypal.ExecutePaymentAsync(order);

        return new PaymentResult
        {
            Success = payment.State == ""approved"",
            TransactionId = payment.Id,
            Message = payment.State
        };
    }

    public async Task<RefundResult> RefundAsync(string transactionId, decimal amount)
    {
        var refund = await _paypal.RefundPaymentAsync(
            transactionId,
            new PayPalAmount { Value = amount });

        return new RefundResult
        {
            Success = refund.State == ""completed"",
            RefundId = refund.Id
        };
    }
}
```",
                CodeExample = @"// Data Format Adapters
public interface IDataReader
{
    IEnumerable<Record> ReadRecords();
}

public class Record
{
    public Dictionary<string, object> Fields { get; set; }
}

// XML Reader (legacy format)
public class XmlDataReader
{
    public XDocument ReadXml(string path) => XDocument.Load(path);
}

// CSV Reader (another format)
public class CsvReader
{
    public IEnumerable<string[]> ReadRows(string path)
    {
        return File.ReadLines(path).Select(line => line.Split(','));
    }
}

// Adapters
public class XmlDataReaderAdapter : IDataReader
{
    private readonly XmlDataReader _xmlReader;
    private readonly string _path;

    public XmlDataReaderAdapter(XmlDataReader reader, string path)
    {
        _xmlReader = reader;
        _path = path;
    }

    public IEnumerable<Record> ReadRecords()
    {
        var doc = _xmlReader.ReadXml(_path);
        foreach (var element in doc.Root.Elements())
        {
            yield return new Record
            {
                Fields = element.Elements()
                    .ToDictionary(e => e.Name.LocalName, e => (object)e.Value)
            };
        }
    }
}

public class CsvDataReaderAdapter : IDataReader
{
    private readonly CsvReader _csvReader;
    private readonly string _path;

    public CsvDataReaderAdapter(CsvReader reader, string path)
    {
        _csvReader = reader;
        _path = path;
    }

    public IEnumerable<Record> ReadRecords()
    {
        var rows = _csvReader.ReadRows(_path).ToList();
        var headers = rows.First();

        foreach (var row in rows.Skip(1))
        {
            yield return new Record
            {
                Fields = headers.Zip(row, (h, v) => (h, v))
                    .ToDictionary(x => x.h, x => (object)x.v)
            };
        }
    }
}

// Usage - client code works with any format
public class DataImporter
{
    public void Import(IDataReader reader)
    {
        foreach (var record in reader.ReadRecords())
        {
            ProcessRecord(record);
        }
    }
}

var importer = new DataImporter();
importer.Import(new XmlDataReaderAdapter(new XmlDataReader(), ""data.xml""));
importer.Import(new CsvDataReaderAdapter(new CsvReader(), ""data.csv""));",
                Tags = new() { "Structural", "Interface Conversion", "GoF" }
            },

            new() {
                Title = "Command",
                Category = "Behavioral",
                Difficulty = "Medium",
                KeyConcepts = "Encapsulate request as object, undo/redo, queuing, logging",
                UseCases = "Undo operations, transaction management, task queues, macro recording",
                Lesson = @"# Command Pattern

## Intent
Encapsulate a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations.

## Structure
```
┌─────────────┐      ┌─────────────────┐
│   Invoker   │─────▶│    ICommand     │
└─────────────┘      ├─────────────────┤
                     │ Execute()       │
                     │ Undo()          │
                     └─────────────────┘
                              ▲
                     ┌────────┴────────┐
                     │                 │
              ConcreteCommandA  ConcreteCommandB
                     │                 │
                     ▼                 ▼
                 Receiver          Receiver
```

## Implementation
```csharp
// Command interface
public interface ICommand
{
    void Execute();
    void Undo();
}

// Receiver
public class Document
{
    public StringBuilder Content { get; } = new();

    public void InsertText(int position, string text)
    {
        Content.Insert(position, text);
    }

    public void DeleteText(int position, int length)
    {
        Content.Remove(position, length);
    }
}

// Concrete Commands
public class InsertTextCommand : ICommand
{
    private readonly Document _document;
    private readonly int _position;
    private readonly string _text;

    public InsertTextCommand(Document document, int position, string text)
    {
        _document = document;
        _position = position;
        _text = text;
    }

    public void Execute() => _document.InsertText(_position, _text);
    public void Undo() => _document.DeleteText(_position, _text.Length);
}

public class DeleteTextCommand : ICommand
{
    private readonly Document _document;
    private readonly int _position;
    private readonly int _length;
    private string _deletedText;

    public DeleteTextCommand(Document document, int position, int length)
    {
        _document = document;
        _position = position;
        _length = length;
    }

    public void Execute()
    {
        _deletedText = _document.Content.ToString(_position, _length);
        _document.DeleteText(_position, _length);
    }

    public void Undo() => _document.InsertText(_position, _deletedText);
}

// Invoker with undo/redo support
public class TextEditor
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();
    public Document Document { get; } = new();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear();  // Clear redo after new action
    }

    public void Undo()
    {
        if (_undoStack.Count > 0)
        {
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }
    }
}

// Usage
var editor = new TextEditor();
editor.ExecuteCommand(new InsertTextCommand(editor.Document, 0, ""Hello ""));
editor.ExecuteCommand(new InsertTextCommand(editor.Document, 6, ""World!""));
// Document: ""Hello World!""

editor.Undo();  // Document: ""Hello ""
editor.Undo();  // Document: """"
editor.Redo();  // Document: ""Hello ""
```

## Macro Commands (Composite)
```csharp
public class MacroCommand : ICommand
{
    private readonly List<ICommand> _commands = new();

    public void AddCommand(ICommand command) => _commands.Add(command);

    public void Execute()
    {
        foreach (var command in _commands)
            command.Execute();
    }

    public void Undo()
    {
        // Undo in reverse order
        for (int i = _commands.Count - 1; i >= 0; i--)
            _commands[i].Undo();
    }
}

// Usage
var macro = new MacroCommand();
macro.AddCommand(new InsertTextCommand(doc, 0, ""Header\n""));
macro.AddCommand(new InsertTextCommand(doc, 7, ""Content\n""));
macro.AddCommand(new InsertTextCommand(doc, 15, ""Footer""));

editor.ExecuteCommand(macro);  // Executes all
editor.Undo();  // Undoes all
```

## Command Queue / Task Processing
```csharp
public interface IAsyncCommand
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public class CommandQueue
{
    private readonly Queue<IAsyncCommand> _queue = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public void Enqueue(IAsyncCommand command)
    {
        _queue.Enqueue(command);
    }

    public async Task ProcessAllAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            while (_queue.Count > 0)
            {
                var command = _queue.Dequeue();
                await command.ExecuteAsync(cancellationToken);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
```",
                CodeExample = @"// Order Management with Command Pattern
public interface IOrderCommand
{
    Task ExecuteAsync();
    Task UndoAsync();
    string Description { get; }
}

public class CreateOrderCommand : IOrderCommand
{
    private readonly IOrderRepository _repository;
    private readonly Order _order;

    public CreateOrderCommand(IOrderRepository repository, Order order)
    {
        _repository = repository;
        _order = order;
    }

    public string Description => $""Create Order {_order.Id}"";

    public async Task ExecuteAsync()
    {
        await _repository.AddAsync(_order);
    }

    public async Task UndoAsync()
    {
        await _repository.DeleteAsync(_order.Id);
    }
}

public class UpdateOrderStatusCommand : IOrderCommand
{
    private readonly IOrderRepository _repository;
    private readonly string _orderId;
    private readonly OrderStatus _newStatus;
    private OrderStatus _previousStatus;

    public UpdateOrderStatusCommand(IOrderRepository repository, string orderId, OrderStatus newStatus)
    {
        _repository = repository;
        _orderId = orderId;
        _newStatus = newStatus;
    }

    public string Description => $""Update Order {_orderId} to {_newStatus}"";

    public async Task ExecuteAsync()
    {
        var order = await _repository.GetByIdAsync(_orderId);
        _previousStatus = order.Status;
        order.Status = _newStatus;
        await _repository.UpdateAsync(order);
    }

    public async Task UndoAsync()
    {
        var order = await _repository.GetByIdAsync(_orderId);
        order.Status = _previousStatus;
        await _repository.UpdateAsync(order);
    }
}

// Command handler with audit logging
public class OrderCommandHandler
{
    private readonly Stack<IOrderCommand> _history = new();
    private readonly ILogger _logger;

    public OrderCommandHandler(ILogger logger) => _logger = logger;

    public async Task ExecuteAsync(IOrderCommand command)
    {
        _logger.LogInformation($""Executing: {command.Description}"");
        await command.ExecuteAsync();
        _history.Push(command);
        _logger.LogInformation($""Completed: {command.Description}"");
    }

    public async Task UndoLastAsync()
    {
        if (_history.Count > 0)
        {
            var command = _history.Pop();
            _logger.LogInformation($""Undoing: {command.Description}"");
            await command.UndoAsync();
        }
    }
}",
                Tags = new() { "Behavioral", "Undo/Redo", "GoF" }
            },

            new() {
                Title = "Facade",
                Category = "Structural",
                Difficulty = "Easy",
                KeyConcepts = "Simplified interface, subsystem encapsulation, reduced complexity",
                UseCases = "Library wrappers, API simplification, legacy system integration",
                Lesson = @"# Facade Pattern

## Intent
Provide a unified interface to a set of interfaces in a subsystem. Facade defines a higher-level interface that makes the subsystem easier to use.

## Structure
```
┌─────────────────────────────────────────────────┐
│                    Facade                        │
│  ┌──────────────────────────────────────────┐   │
│  │  SimpleOperation()                        │   │
│  │  AnotherOperation()                       │   │
│  └──────────────────────────────────────────┘   │
│        │           │           │                 │
│        ▼           ▼           ▼                 │
│  ┌─────────┐ ┌─────────┐ ┌─────────┐           │
│  │SubsysA  │ │SubsysB  │ │SubsysC  │           │
│  └─────────┘ └─────────┘ └─────────┘           │
└─────────────────────────────────────────────────┘
```

## Implementation
```csharp
// Complex subsystem classes
public class VideoFile
{
    public string Filename { get; }
    public string CodecType { get; }

    public VideoFile(string filename)
    {
        Filename = filename;
        CodecType = filename.EndsWith("".mp4"") ? ""mp4"" : ""ogg"";
    }
}

public class CodecFactory
{
    public ICodec GetCodec(string type) => type switch
    {
        ""mp4"" => new MPEG4Codec(),
        ""ogg"" => new OggCodec(),
        _ => throw new NotSupportedException()
    };
}

public interface ICodec { byte[] Decode(byte[] data); }
public class MPEG4Codec : ICodec { public byte[] Decode(byte[] data) => data; }
public class OggCodec : ICodec { public byte[] Decode(byte[] data) => data; }

public class BitrateReader
{
    public byte[] Read(string filename, ICodec codec)
    {
        var data = File.ReadAllBytes(filename);
        return codec.Decode(data);
    }
}

public class AudioMixer
{
    public byte[] MixAudio(byte[] video) => video; // Simplified
}

// Facade - simplifies video conversion
public class VideoConverterFacade
{
    public byte[] ConvertVideo(string filename, string format)
    {
        var file = new VideoFile(filename);
        var codecFactory = new CodecFactory();
        var sourceCodec = codecFactory.GetCodec(file.CodecType);

        var bitrateReader = new BitrateReader();
        var buffer = bitrateReader.Read(filename, sourceCodec);

        var audioMixer = new AudioMixer();
        var result = audioMixer.MixAudio(buffer);

        var destinationCodec = codecFactory.GetCodec(format);
        // Further conversion...

        return result;
    }
}

// Usage - client doesn't need to know about subsystems
var converter = new VideoConverterFacade();
var mp4Data = converter.ConvertVideo(""video.ogg"", ""mp4"");
```

## Real-World: Email Service Facade
```csharp
// Complex subsystems
public class SmtpClient
{
    public void Connect(string server, int port) { }
    public void Authenticate(string user, string password) { }
    public void SendMail(MailMessage message) { }
    public void Disconnect() { }
}

public class TemplateEngine
{
    public string Render(string template, object model)
    {
        // Complex template rendering
        return template;
    }
}

public class AttachmentProcessor
{
    public MailAttachment Process(string filePath)
    {
        var bytes = File.ReadAllBytes(filePath);
        return new MailAttachment(Path.GetFileName(filePath), bytes);
    }
}

public class EmailValidator
{
    public bool Validate(string email)
    {
        return Regex.IsMatch(email, @""^[\w\.-]+@[\w\.-]+\.\w+$"");
    }
}

// Facade
public class EmailServiceFacade
{
    private readonly SmtpClient _smtp;
    private readonly TemplateEngine _templateEngine;
    private readonly AttachmentProcessor _attachmentProcessor;
    private readonly EmailValidator _validator;
    private readonly EmailConfig _config;

    public EmailServiceFacade(EmailConfig config)
    {
        _config = config;
        _smtp = new SmtpClient();
        _templateEngine = new TemplateEngine();
        _attachmentProcessor = new AttachmentProcessor();
        _validator = new EmailValidator();
    }

    public async Task SendEmailAsync(
        string to,
        string subject,
        string templateName,
        object model,
        params string[] attachmentPaths)
    {
        // Validate
        if (!_validator.Validate(to))
            throw new ArgumentException(""Invalid email address"");

        // Render template
        var template = await LoadTemplateAsync(templateName);
        var body = _templateEngine.Render(template, model);

        // Process attachments
        var attachments = attachmentPaths
            .Select(path => _attachmentProcessor.Process(path))
            .ToList();

        // Build message
        var message = new MailMessage
        {
            From = _config.FromAddress,
            To = to,
            Subject = subject,
            Body = body,
            Attachments = attachments
        };

        // Send
        _smtp.Connect(_config.SmtpServer, _config.SmtpPort);
        _smtp.Authenticate(_config.Username, _config.Password);
        _smtp.SendMail(message);
        _smtp.Disconnect();
    }

    // Simple methods hide complexity
    public Task SendWelcomeEmailAsync(User user)
    {
        return SendEmailAsync(
            user.Email,
            ""Welcome!"",
            ""welcome"",
            new { user.Name, user.Email });
    }

    public Task SendOrderConfirmationAsync(Order order)
    {
        return SendEmailAsync(
            order.CustomerEmail,
            $""Order Confirmation #{order.Id}"",
            ""order-confirmation"",
            order,
            order.InvoicePath);
    }
}

// Usage - simple for clients
var emailService = new EmailServiceFacade(config);
await emailService.SendWelcomeEmailAsync(newUser);
await emailService.SendOrderConfirmationAsync(order);
```",
                CodeExample = @"// Home Automation Facade
public class Light { public void On() { } public void Off() { } public void Dim(int level) { } }
public class TV { public void On() { } public void Off() { } public void SetChannel(int ch) { } }
public class SoundSystem { public void On() { } public void Off() { } public void SetVolume(int vol) { } }
public class Thermostat { public void SetTemperature(int temp) { } }
public class SecuritySystem { public void Arm() { } public void Disarm() { } }

// Facade
public class SmartHomeFacade
{
    private readonly Light _livingRoomLight;
    private readonly Light _bedroomLight;
    private readonly TV _tv;
    private readonly SoundSystem _soundSystem;
    private readonly Thermostat _thermostat;
    private readonly SecuritySystem _security;

    public SmartHomeFacade()
    {
        _livingRoomLight = new Light();
        _bedroomLight = new Light();
        _tv = new TV();
        _soundSystem = new SoundSystem();
        _thermostat = new Thermostat();
        _security = new SecuritySystem();
    }

    public void MovieMode()
    {
        Console.WriteLine(""Setting up movie mode..."");
        _livingRoomLight.Dim(20);
        _tv.On();
        _soundSystem.On();
        _soundSystem.SetVolume(50);
        _thermostat.SetTemperature(72);
    }

    public void LeaveHome()
    {
        Console.WriteLine(""Leaving home..."");
        _livingRoomLight.Off();
        _bedroomLight.Off();
        _tv.Off();
        _soundSystem.Off();
        _thermostat.SetTemperature(65);
        _security.Arm();
    }

    public void ArriveHome()
    {
        Console.WriteLine(""Welcome home!"");
        _security.Disarm();
        _livingRoomLight.On();
        _thermostat.SetTemperature(72);
    }

    public void Goodnight()
    {
        Console.WriteLine(""Goodnight mode..."");
        _livingRoomLight.Off();
        _bedroomLight.Dim(10);
        _tv.Off();
        _soundSystem.Off();
        _thermostat.SetTemperature(68);
        _security.Arm();
    }
}

// Usage
var home = new SmartHomeFacade();
home.ArriveHome();
home.MovieMode();
home.Goodnight();",
                Tags = new() { "Structural", "Simplification", "GoF" }
            },

            new() {
                Title = "Unit of Work",
                Category = "Architectural",
                Difficulty = "Medium",
                KeyConcepts = "Transaction management, change tracking, batch persistence, consistency",
                UseCases = "Database transactions, ORM patterns, data consistency",
                Lesson = @"# Unit of Work Pattern

## Intent
Maintains a list of objects affected by a business transaction and coordinates the writing out of changes and the resolution of concurrency problems.

## Why Use It?
- **Consistency**: All changes committed or rolled back together
- **Performance**: Batch database operations
- **Simplicity**: Transaction management in one place
- **Testability**: Easy to mock for unit tests

## Implementation
```csharp
// Unit of Work interface
public interface IUnitOfWork : IDisposable
{
    IRepository<Customer> Customers { get; }
    IRepository<Order> Orders { get; }
    IRepository<Product> Products { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}

// Generic Repository interface
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}

// EF Core implementation
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction _transaction;

    private IRepository<Customer> _customers;
    private IRepository<Order> _orders;
    private IRepository<Product> _products;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<Customer> Customers =>
        _customers ??= new Repository<Customer>(_context);

    public IRepository<Order> Orders =>
        _orders ??= new Repository<Order>(_context);

    public IRepository<Product> Products =>
        _products ??= new Repository<Product>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

// Generic Repository implementation
public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => _dbSet.Remove(entity);
}
```

## Usage in Service Layer
```csharp
public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Get customer
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
            if (customer == null)
                throw new NotFoundException(""Customer not found"");

            // Create order
            var order = new Order
            {
                CustomerId = customer.Id,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            // Add order items and update inventory
            foreach (var item in request.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product.Stock < item.Quantity)
                    throw new InsufficientStockException(product.Name);

                product.Stock -= item.Quantity;
                _unitOfWork.Products.Update(product);

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return order;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
```

## Registration in DI Container
```csharp
// Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositories are created by UnitOfWork, not registered separately
```",
                CodeExample = @"// Advanced: Specification Pattern with Unit of Work
public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
}

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public Expression<Func<T, object>> OrderByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }

    protected void AddCriteria(Expression<Func<T, bool>> criteria) => Criteria = criteria;
    protected void AddInclude(Expression<Func<T, object>> include) => Includes.Add(include);
    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy) => OrderBy = orderBy;
    protected void ApplyPaging(int skip, int take) { Skip = skip; Take = take; }
}

// Example specification
public class OrdersByCustomerSpec : BaseSpecification<Order>
{
    public OrdersByCustomerSpec(int customerId, int page, int pageSize)
    {
        AddCriteria(o => o.CustomerId == customerId);
        AddInclude(o => o.Items);
        AddInclude(o => o.Customer);
        ApplyOrderBy(o => o.OrderDate);
        ApplyPaging((page - 1) * pageSize, pageSize);
    }
}

// Repository with specification support
public interface IRepository<T> where T : class
{
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
}

// Usage
var spec = new OrdersByCustomerSpec(customerId: 1, page: 1, pageSize: 10);
var orders = await _unitOfWork.Orders.ListAsync(spec);",
                Tags = new() { "Architectural", "Transaction", "Data Access" }
            },

            new() {
                Title = "Mediator",
                Category = "Behavioral",
                Difficulty = "Medium",
                KeyConcepts = "Loose coupling, centralized communication, CQRS, request/response",
                UseCases = "Chat applications, air traffic control, CQRS/MediatR",
                Lesson = @"# Mediator Pattern

## Intent
Define an object that encapsulates how a set of objects interact. Mediator promotes loose coupling by keeping objects from referring to each other explicitly.

## Structure
```
┌───────────┐    ┌───────────────┐    ┌───────────┐
│ColleagueA │───▶│   Mediator    │◀───│ColleagueB │
└───────────┘    └───────────────┘    └───────────┘
                        │
                        ▼
                 ┌───────────┐
                 │ColleagueC │
                 └───────────┘
```

## Basic Implementation
```csharp
// Mediator interface
public interface IChatMediator
{
    void SendMessage(string message, User sender);
    void RegisterUser(User user);
}

// Colleague
public abstract class User
{
    protected IChatMediator _mediator;
    public string Name { get; }

    protected User(IChatMediator mediator, string name)
    {
        _mediator = mediator;
        Name = name;
    }

    public abstract void Send(string message);
    public abstract void Receive(string message, User sender);
}

// Concrete Mediator
public class ChatRoom : IChatMediator
{
    private readonly List<User> _users = new();

    public void RegisterUser(User user)
    {
        _users.Add(user);
    }

    public void SendMessage(string message, User sender)
    {
        foreach (var user in _users)
        {
            if (user != sender)
            {
                user.Receive(message, sender);
            }
        }
    }
}

// Concrete Colleague
public class ChatUser : User
{
    public ChatUser(IChatMediator mediator, string name) : base(mediator, name) { }

    public override void Send(string message)
    {
        Console.WriteLine($""{Name} sends: {message}"");
        _mediator.SendMessage(message, this);
    }

    public override void Receive(string message, User sender)
    {
        Console.WriteLine($""{Name} receives from {sender.Name}: {message}"");
    }
}

// Usage
var chatRoom = new ChatRoom();

var john = new ChatUser(chatRoom, ""John"");
var jane = new ChatUser(chatRoom, ""Jane"");
var bob = new ChatUser(chatRoom, ""Bob"");

chatRoom.RegisterUser(john);
chatRoom.RegisterUser(jane);
chatRoom.RegisterUser(bob);

john.Send(""Hello everyone!"");
// Output:
// John sends: Hello everyone!
// Jane receives from John: Hello everyone!
// Bob receives from John: Hello everyone!
```

## MediatR Library (CQRS Pattern)
```csharp
// Request/Response
public record GetUserQuery(int Id) : IRequest<UserDto>;

public class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUserRepository _repository;

    public GetUserHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id);
        return user?.ToDto();
    }
}

// Command (no response)
public record CreateUserCommand(string Name, string Email) : IRequest<int>;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _repository;

    public CreateUserHandler(IUserRepository repository)
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

// Notifications (publish to many handlers)
public record UserCreatedNotification(int UserId, string Email) : INotification;

public class SendWelcomeEmailHandler : INotificationHandler<UserCreatedNotification>
{
    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        // Send welcome email
    }
}

public class CreateAuditLogHandler : INotificationHandler<UserCreatedNotification>
{
    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        // Create audit log
    }
}

// Controller usage
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet(""{id}"")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _mediator.Send(new GetUserQuery(id));
        return user != null ? Ok(user) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        var id = await _mediator.Send(command);
        await _mediator.Publish(new UserCreatedNotification(id, command.Email));
        return CreatedAtAction(nameof(Get), new { id }, null);
    }
}
```

## Pipeline Behaviors
```csharp
// Validation behavior
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
            throw new ValidationException(failures);

        return await next();
    }
}

// Logging behavior
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger _logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($""Handling {typeof(TRequest).Name}"");
        var response = await next();
        _logger.LogInformation($""Handled {typeof(TRequest).Name}"");
        return response;
    }
}

// Registration
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
```",
                CodeExample = @"// Complete CQRS Example with MediatR
// Commands
public record CreateOrderCommand(int CustomerId, List<OrderItemDto> Items) : IRequest<int>;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CreateOrderHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order { CustomerId = request.CustomerId };

        foreach (var item in request.Items)
        {
            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        // Publish domain event
        await _mediator.Publish(new OrderCreatedEvent(order.Id), cancellationToken);

        return order.Id;
    }
}

// Queries
public record GetOrdersQuery(int CustomerId, int Page, int PageSize) : IRequest<PagedResult<OrderDto>>;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, PagedResult<OrderDto>>
{
    private readonly IReadOnlyRepository<Order> _repository;

    public async Task<PagedResult<OrderDto>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _repository.GetPagedAsync(
            o => o.CustomerId == request.CustomerId,
            request.Page,
            request.PageSize);

        return orders.MapTo<OrderDto>();
    }
}

// Domain Events
public record OrderCreatedEvent(int OrderId) : INotification;

public class OrderCreatedEmailHandler : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Send confirmation email
    }
}

public class OrderCreatedInventoryHandler : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Update inventory
    }
}",
                Tags = new() { "Behavioral", "CQRS", "MediatR", "Loose Coupling" }
            }
        };
    }

    public static List<AspNetCoreTopic> GetAspNetCoreTopics()
    {
        return new List<AspNetCoreTopic>
        {
            new() {
                Title = "Dependency Injection in ASP.NET Core",
                Category = "Core Concepts",
                Difficulty = "Medium",
                KeyConcepts = "Built-in DI container, service lifetimes, constructor injection, IServiceCollection",
                Lesson = @"# Dependency Injection in ASP.NET Core

## What is Dependency Injection?
Dependency Injection (DI) is a design pattern where dependencies are provided to a class rather than created by it. ASP.NET Core has a built-in DI container.

## Service Lifetimes

### Transient
A new instance is created each time the service is requested.
```csharp
services.AddTransient<IMyService, MyService>();
```
**Use for:** Lightweight, stateless services

### Scoped
A single instance per HTTP request (or scope).
```csharp
services.AddScoped<IMyService, MyService>();
```
**Use for:** Services that should share state within a request (DbContext)

### Singleton
A single instance for the entire application lifetime.
```csharp
services.AddSingleton<IMyService, MyService>();
```
**Use for:** Stateless services, caching, configuration

## Registration in Program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(""Default"")));

var app = builder.Build();
```

## Constructor Injection
```csharp
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet(""{id}"")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        return user != null ? Ok(user) : NotFound();
    }
}
```

## Keyed Services (ASP.NET Core 8+)
```csharp
// Registration
builder.Services.AddKeyedScoped<IPaymentProcessor, StripeProcessor>(""stripe"");
builder.Services.AddKeyedScoped<IPaymentProcessor, PayPalProcessor>(""paypal"");

// Injection
public class PaymentController : ControllerBase
{
    public PaymentController(
        [FromKeyedServices(""stripe"")] IPaymentProcessor stripeProcessor,
        [FromKeyedServices(""paypal"")] IPaymentProcessor paypalProcessor)
    { }
}
```

## Common Mistakes
1. **Captive Dependencies**: Singleton depending on Scoped service
2. **Service Locator Pattern**: Using IServiceProvider directly
3. **Registering concrete types without interfaces**

## Interview Questions
- What are the three service lifetimes?
- When would you use Scoped vs Singleton?
- How do you register a service with multiple implementations?",
                CodeExample = @"// Complete DI setup example
public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderDto dto);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository repository,
        IPaymentService paymentService,
        ILogger<OrderService> logger)
    {
        _repository = repository;
        _paymentService = paymentService;
        _logger = logger;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderDto dto)
    {
        _logger.LogInformation(""Creating order for customer {CustomerId}"", dto.CustomerId);

        var order = new Order { CustomerId = dto.CustomerId };
        await _repository.AddAsync(order);
        await _paymentService.ProcessAsync(order.Total);

        return order;
    }
}

// Program.cs registration
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPaymentService, StripePaymentService>();",
                Tags = new() { "DI", "IoC", "Services" }
            },

            new() {
                Title = "Middleware Pipeline",
                Category = "Core Concepts",
                Difficulty = "Medium",
                KeyConcepts = "Request pipeline, custom middleware, Use/Run/Map, order matters",
                Lesson = @"# Middleware Pipeline in ASP.NET Core

## What is Middleware?
Middleware is software assembled into an app pipeline to handle requests and responses. Each component:
- Chooses whether to pass the request to the next component
- Can perform work before and after the next component

## The Pipeline
```
Request → Middleware 1 → Middleware 2 → Middleware 3 → Endpoint
              ↓               ↓               ↓
Response ← Middleware 1 ← Middleware 2 ← Middleware 3 ← Endpoint
```

## Built-in Middleware Order (Important!)
```csharp
var app = builder.Build();

// 1. Exception handling (first to catch all exceptions)
app.UseExceptionHandler(""/Error"");
app.UseHsts();

// 2. HTTPS redirection
app.UseHttpsRedirection();

// 3. Static files (before routing)
app.UseStaticFiles();

// 4. Routing
app.UseRouting();

// 5. CORS (after routing, before auth)
app.UseCors();

// 6. Authentication
app.UseAuthentication();

// 7. Authorization
app.UseAuthorization();

// 8. Custom middleware

// 9. Endpoints
app.MapControllers();
```

## Creating Custom Middleware

### Inline Middleware
```csharp
app.Use(async (context, next) =>
{
    // Before next middleware
    var start = DateTime.UtcNow;

    await next();  // Call next middleware

    // After next middleware (response)
    var elapsed = DateTime.UtcNow - start;
    context.Response.Headers.Add(""X-Response-Time"", elapsed.TotalMilliseconds.ToString());
});
```

### Class-Based Middleware
```csharp
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
        _logger.LogInformation(""Request: {Method} {Path}"",
            context.Request.Method,
            context.Request.Path);

        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();
        _logger.LogInformation(""Response: {StatusCode} in {ElapsedMs}ms"",
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }
}

// Extension method
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}

// Usage
app.UseRequestLogging();
```

## Use vs Run vs Map

### Use - Passes to next
```csharp
app.Use(async (context, next) =>
{
    // Do something
    await next();  // Continue to next middleware
});
```

### Run - Terminal (doesn't call next)
```csharp
app.Run(async context =>
{
    await context.Response.WriteAsync(""Hello World"");
    // No next() - pipeline ends here
});
```

### Map - Branches pipeline
```csharp
app.Map(""/api"", apiApp =>
{
    apiApp.UseAuthentication();
    apiApp.UseAuthorization();
    apiApp.Run(async ctx => await ctx.Response.WriteAsync(""API Branch""));
});
```

## Interview Tips
- Explain why order matters
- Describe request/response flow
- Know built-in middleware order",
                CodeExample = @"// Rate limiting middleware example
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly int _maxRequests = 100;
    private readonly TimeSpan _window = TimeSpan.FromMinutes(1);

    public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? ""unknown"";
        var cacheKey = $""rate_limit_{ipAddress}"";

        var requestCount = _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _window;
            return 0;
        });

        if (requestCount >= _maxRequests)
        {
            context.Response.StatusCode = 429; // Too Many Requests
            await context.Response.WriteAsync(""Rate limit exceeded"");
            return;
        }

        _cache.Set(cacheKey, requestCount + 1, _window);
        await _next(context);
    }
}",
                Tags = new() { "Pipeline", "HTTP", "Request Processing" }
            },

            new() {
                Title = "Entity Framework Core Basics",
                Category = "Data Access",
                Difficulty = "Medium",
                KeyConcepts = "DbContext, DbSet, migrations, LINQ queries, change tracking",
                Lesson = @"# Entity Framework Core Basics

## What is EF Core?
Entity Framework Core is an ORM (Object-Relational Mapper) that enables .NET developers to work with databases using .NET objects.

## DbContext Setup
```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);
    }
}
```

## Registration
```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(""Default""))
           .EnableSensitiveDataLogging() // Development only
           .LogTo(Console.WriteLine));
```

## CRUD Operations

### Create
```csharp
var user = new User { Name = ""John"", Email = ""john@example.com"" };
_context.Users.Add(user);
await _context.SaveChangesAsync();
```

### Read
```csharp
// Find by primary key
var user = await _context.Users.FindAsync(id);

// Query with LINQ
var activeUsers = await _context.Users
    .Where(u => u.IsActive)
    .OrderBy(u => u.Name)
    .ToListAsync();

// Include related data (eager loading)
var userWithOrders = await _context.Users
    .Include(u => u.Orders)
        .ThenInclude(o => o.OrderItems)
    .FirstOrDefaultAsync(u => u.Id == id);
```

### Update
```csharp
var user = await _context.Users.FindAsync(id);
user.Name = ""Updated Name"";
await _context.SaveChangesAsync();  // Change tracking detects changes

// Or explicit update
_context.Entry(user).State = EntityState.Modified;
await _context.SaveChangesAsync();
```

### Delete
```csharp
var user = await _context.Users.FindAsync(id);
_context.Users.Remove(user);
await _context.SaveChangesAsync();
```

## Migrations
```bash
# Create migration
dotnet ef migrations add InitialCreate

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# Generate SQL script
dotnet ef migrations script
```

## Query Types

### No-Tracking Queries (Read-only, faster)
```csharp
var users = await _context.Users
    .AsNoTracking()
    .ToListAsync();
```

### Split Queries (Avoid cartesian explosion)
```csharp
var users = await _context.Users
    .Include(u => u.Orders)
    .AsSplitQuery()
    .ToListAsync();
```

### Raw SQL
```csharp
var users = await _context.Users
    .FromSqlRaw(""SELECT * FROM Users WHERE IsActive = 1"")
    .ToListAsync();

// With parameters (safe from SQL injection)
var users = await _context.Users
    .FromSqlInterpolated($""SELECT * FROM Users WHERE Email = {email}"")
    .ToListAsync();
```

## Best Practices
1. Use `AsNoTracking()` for read-only queries
2. Use `Include()` wisely - avoid over-fetching
3. Use pagination for large datasets
4. Handle concurrency with optimistic locking
5. Use transactions for multiple operations",
                CodeExample = @"// Complete repository with EF Core
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.IsActive)
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }
}",
                Tags = new() { "EF Core", "ORM", "Database" }
            }
        };
    }

    public static List<SqlServerTopic> GetSqlServerTopics()
    {
        return new List<SqlServerTopic>
        {
            new() {
                Title = "JOINs in SQL Server",
                Category = "Queries",
                Difficulty = "Medium",
                KeyConcepts = "INNER JOIN, LEFT JOIN, RIGHT JOIN, FULL OUTER JOIN, CROSS JOIN, self-join",
                Lesson = @"# JOINs in SQL Server

## What are JOINs?
JOINs combine rows from two or more tables based on a related column between them.

## Types of JOINs

### INNER JOIN
Returns only matching rows from both tables.
```sql
SELECT Orders.OrderId, Customers.CustomerName
FROM Orders
INNER JOIN Customers ON Orders.CustomerId = Customers.CustomerId;
```

### LEFT JOIN (LEFT OUTER JOIN)
Returns all rows from the left table, and matched rows from the right table. NULL for no match.
```sql
SELECT Customers.CustomerName, Orders.OrderId
FROM Customers
LEFT JOIN Orders ON Customers.CustomerId = Orders.CustomerId;
-- Shows all customers, even those without orders (OrderId = NULL)
```

### RIGHT JOIN (RIGHT OUTER JOIN)
Returns all rows from the right table, and matched rows from the left table.
```sql
SELECT Orders.OrderId, Customers.CustomerName
FROM Orders
RIGHT JOIN Customers ON Orders.CustomerId = Customers.CustomerId;
-- Shows all customers, orders without matching customer show NULL
```

### FULL OUTER JOIN
Returns all rows when there's a match in either table.
```sql
SELECT Customers.CustomerName, Orders.OrderId
FROM Customers
FULL OUTER JOIN Orders ON Customers.CustomerId = Orders.CustomerId;
-- Shows all customers and all orders, NULLs where no match
```

### CROSS JOIN
Returns the Cartesian product (all combinations).
```sql
SELECT Products.ProductName, Colors.ColorName
FROM Products
CROSS JOIN Colors;
-- Every product combined with every color
```

### Self-Join
Joining a table to itself.
```sql
-- Find employees and their managers
SELECT
    e.EmployeeName AS Employee,
    m.EmployeeName AS Manager
FROM Employees e
LEFT JOIN Employees m ON e.ManagerId = m.EmployeeId;
```

## Multiple JOINs
```sql
SELECT
    o.OrderId,
    c.CustomerName,
    p.ProductName,
    oi.Quantity
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.CustomerId
INNER JOIN OrderItems oi ON o.OrderId = oi.OrderId
INNER JOIN Products p ON oi.ProductId = p.ProductId
WHERE o.OrderDate >= '2024-01-01';
```

## JOIN vs Subquery
```sql
-- Using JOIN (often faster)
SELECT DISTINCT c.CustomerName
FROM Customers c
INNER JOIN Orders o ON c.CustomerId = o.CustomerId;

-- Using Subquery
SELECT CustomerName
FROM Customers
WHERE CustomerId IN (SELECT CustomerId FROM Orders);
```

## Performance Tips
1. Index the columns used in JOIN conditions
2. Start with the smallest table (SQL Server optimizer usually handles this)
3. Use appropriate JOIN types (don't use LEFT JOIN if INNER JOIN works)
4. Avoid JOINs in WHERE clause; use explicit JOIN syntax

## Interview Questions
- Difference between INNER and LEFT JOIN?
- When would you use CROSS JOIN?
- How do you find records in table A not in table B?",
                SqlExample = @"-- Find customers who haven't ordered in 30 days
SELECT c.CustomerId, c.CustomerName, MAX(o.OrderDate) AS LastOrder
FROM Customers c
LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
GROUP BY c.CustomerId, c.CustomerName
HAVING MAX(o.OrderDate) < DATEADD(day, -30, GETDATE())
    OR MAX(o.OrderDate) IS NULL;

-- Find products never ordered
SELECT p.ProductId, p.ProductName
FROM Products p
LEFT JOIN OrderItems oi ON p.ProductId = oi.ProductId
WHERE oi.ProductId IS NULL;

-- Employee hierarchy with level
WITH EmployeeCTE AS (
    SELECT EmployeeId, EmployeeName, ManagerId, 0 AS Level
    FROM Employees
    WHERE ManagerId IS NULL

    UNION ALL

    SELECT e.EmployeeId, e.EmployeeName, e.ManagerId, Level + 1
    FROM Employees e
    INNER JOIN EmployeeCTE cte ON e.ManagerId = cte.EmployeeId
)
SELECT * FROM EmployeeCTE ORDER BY Level;",
                Tags = new() { "JOIN", "Queries", "Fundamentals" }
            },

            new() {
                Title = "Indexes in SQL Server",
                Category = "Performance",
                Difficulty = "Medium",
                KeyConcepts = "Clustered index, non-clustered index, covering index, index seek vs scan",
                Lesson = @"# Indexes in SQL Server

## What are Indexes?
Indexes are database structures that improve the speed of data retrieval operations. They work like a book's index - instead of reading the whole book, you look up the topic and go directly to the page.

## Types of Indexes

### Clustered Index
- Determines the physical order of data in the table
- Only ONE per table (usually on Primary Key)
- The table data IS the clustered index
- Best for: Range queries, columns frequently used in ORDER BY

```sql
-- Created automatically with PRIMARY KEY
CREATE TABLE Users (
    UserId INT PRIMARY KEY,  -- Clustered index created
    Email VARCHAR(256),
    Name VARCHAR(100)
);

-- Or explicitly
CREATE CLUSTERED INDEX IX_Users_UserId ON Users(UserId);
```

### Non-Clustered Index
- Separate structure from table data
- Contains pointers to actual rows
- Can have many per table (up to 999)
- Best for: Columns used in WHERE, JOIN, or ORDER BY

```sql
CREATE NONCLUSTERED INDEX IX_Users_Email ON Users(Email);

-- Composite index
CREATE NONCLUSTERED INDEX IX_Orders_Customer_Date
ON Orders(CustomerId, OrderDate);
```

### Covering Index
A non-clustered index that includes all columns needed by a query (no lookup to table needed).

```sql
-- Query
SELECT Email, Name FROM Users WHERE Email = 'test@example.com';

-- Covering index
CREATE NONCLUSTERED INDEX IX_Users_Email_Incl
ON Users(Email)
INCLUDE (Name);  -- Included columns stored at leaf level
```

### Unique Index
```sql
CREATE UNIQUE INDEX IX_Users_Email ON Users(Email);
-- Same as: ALTER TABLE Users ADD CONSTRAINT UQ_Email UNIQUE(Email);
```

### Filtered Index
Index on a subset of rows.
```sql
CREATE NONCLUSTERED INDEX IX_Orders_Active
ON Orders(OrderDate)
WHERE Status = 'Active';
-- Smaller index, faster queries on active orders
```

## Index Seek vs Index Scan

### Index Seek (Good)
- Navigates the B-tree structure
- Finds specific rows quickly
- Uses: =, <, >, BETWEEN on indexed column

### Index Scan (Often Bad)
- Reads entire index
- Happens when: LIKE '%pattern', functions on indexed column, type mismatch

```sql
-- Seek (fast)
SELECT * FROM Users WHERE Email = 'test@example.com';

-- Scan (slow) - function on column
SELECT * FROM Users WHERE UPPER(Email) = 'TEST@EXAMPLE.COM';

-- Fixed: Use computed column or fix query
SELECT * FROM Users WHERE Email = 'test@example.com';
```

## When to Create Indexes
1. Columns in WHERE clauses
2. Columns in JOIN conditions
3. Columns in ORDER BY
4. Foreign key columns
5. Columns with high selectivity

## When NOT to Create Indexes
1. Small tables
2. Frequently updated columns
3. Low selectivity columns (Gender, Status with few values)
4. Tables with many INSERTs

## Index Maintenance
```sql
-- Rebuild index (removes fragmentation)
ALTER INDEX IX_Users_Email ON Users REBUILD;

-- Reorganize (lighter operation)
ALTER INDEX IX_Users_Email ON Users REORGANIZE;

-- Check fragmentation
SELECT
    i.name AS IndexName,
    s.avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('Users'), NULL, NULL, 'LIMITED') s
JOIN sys.indexes i ON s.object_id = i.object_id AND s.index_id = i.index_id;
```",
                SqlExample = @"-- Missing index query (find suggestions)
SELECT
    mig.index_group_handle,
    mid.statement AS TableName,
    mid.equality_columns,
    mid.inequality_columns,
    mid.included_columns,
    migs.avg_user_impact
FROM sys.dm_db_missing_index_groups mig
JOIN sys.dm_db_missing_index_group_stats migs ON mig.index_group_handle = migs.group_handle
JOIN sys.dm_db_missing_index_details mid ON mig.index_handle = mid.index_handle
ORDER BY migs.avg_user_impact DESC;

-- Index usage statistics
SELECT
    OBJECT_NAME(s.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc,
    s.user_seeks,
    s.user_scans,
    s.user_lookups,
    s.user_updates
FROM sys.dm_db_index_usage_stats s
JOIN sys.indexes i ON s.object_id = i.object_id AND s.index_id = i.index_id
WHERE OBJECTPROPERTY(s.object_id, 'IsUserTable') = 1
ORDER BY s.user_seeks + s.user_scans DESC;",
                Tags = new() { "Index", "Performance", "Optimization" }
            },

            new() {
                Title = "Stored Procedures",
                Category = "Programmability",
                Difficulty = "Medium",
                KeyConcepts = "Parameters, OUTPUT, error handling, transactions, security",
                Lesson = @"# Stored Procedures in SQL Server

## What is a Stored Procedure?
A stored procedure is a precompiled collection of SQL statements stored in the database. They provide:
- Code reusability
- Better performance (cached execution plans)
- Security (control access to data)
- Reduced network traffic

## Basic Syntax
```sql
CREATE PROCEDURE [schema].[ProcedureName]
    @Parameter1 DataType,
    @Parameter2 DataType = DefaultValue  -- Optional parameter
AS
BEGIN
    SET NOCOUNT ON;  -- Don't return row count messages

    -- SQL statements here
END;
```

## Parameters

### Input Parameters
```sql
CREATE PROCEDURE GetOrdersByCustomer
    @CustomerId INT,
    @StartDate DATE = NULL  -- Optional with default
AS
BEGIN
    SELECT * FROM Orders
    WHERE CustomerId = @CustomerId
    AND (@StartDate IS NULL OR OrderDate >= @StartDate);
END;

-- Execute
EXEC GetOrdersByCustomer @CustomerId = 123;
EXEC GetOrdersByCustomer 123, '2024-01-01';
```

### Output Parameters
```sql
CREATE PROCEDURE CreateOrder
    @CustomerId INT,
    @Total DECIMAL(18,2),
    @OrderId INT OUTPUT,
    @Message VARCHAR(200) OUTPUT
AS
BEGIN
    INSERT INTO Orders (CustomerId, Total, OrderDate)
    VALUES (@CustomerId, @Total, GETDATE());

    SET @OrderId = SCOPE_IDENTITY();
    SET @Message = 'Order created successfully';
END;

-- Execute
DECLARE @NewOrderId INT, @Msg VARCHAR(200);
EXEC CreateOrder
    @CustomerId = 123,
    @Total = 99.99,
    @OrderId = @NewOrderId OUTPUT,
    @Message = @Msg OUTPUT;

SELECT @NewOrderId AS OrderId, @Msg AS Message;
```

## Error Handling
```sql
CREATE PROCEDURE TransferMoney
    @FromAccountId INT,
    @ToAccountId INT,
    @Amount DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Withdraw
        UPDATE Accounts
        SET Balance = Balance - @Amount
        WHERE AccountId = @FromAccountId;

        IF @@ROWCOUNT = 0
            THROW 50001, 'Source account not found', 1;

        -- Check sufficient funds
        IF (SELECT Balance FROM Accounts WHERE AccountId = @FromAccountId) < 0
            THROW 50002, 'Insufficient funds', 1;

        -- Deposit
        UPDATE Accounts
        SET Balance = Balance + @Amount
        WHERE AccountId = @ToAccountId;

        IF @@ROWCOUNT = 0
            THROW 50003, 'Destination account not found', 1;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Re-throw with details
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
```

## Returning Data

### Return Value (integers only, for status)
```sql
CREATE PROCEDURE CheckUserExists
    @Email VARCHAR(256)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
        RETURN 1;  -- Exists
    RETURN 0;  -- Not found
END;

-- Execute
DECLARE @Result INT;
EXEC @Result = CheckUserExists 'test@example.com';
SELECT @Result;
```

### Result Sets
```sql
CREATE PROCEDURE GetDashboardData
    @UserId INT
AS
BEGIN
    -- Multiple result sets
    SELECT * FROM Users WHERE UserId = @UserId;
    SELECT * FROM Orders WHERE UserId = @UserId ORDER BY OrderDate DESC;
    SELECT COUNT(*) AS TotalOrders FROM Orders WHERE UserId = @UserId;
END;
```

## Security
```sql
-- Grant execute permission
GRANT EXECUTE ON GetOrdersByCustomer TO AppUser;

-- Execute as different user
CREATE PROCEDURE AdminProcedure
WITH EXECUTE AS 'AdminUser'
AS
BEGIN
    -- Runs with AdminUser permissions
END;
```

## Best Practices
1. Always use SET NOCOUNT ON
2. Use TRY/CATCH for error handling
3. Use transactions for multi-statement operations
4. Parameterize to prevent SQL injection
5. Use schema names (dbo.ProcedureName)
6. Comment your code",
                SqlExample = @"-- Complete CRUD stored procedures
CREATE PROCEDURE usp_User_GetById
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT UserId, Email, Name, CreatedAt
    FROM Users
    WHERE UserId = @UserId;
END;
GO

CREATE PROCEDURE usp_User_Create
    @Email VARCHAR(256),
    @Name VARCHAR(100),
    @UserId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
            THROW 50001, 'Email already exists', 1;

        INSERT INTO Users (Email, Name, CreatedAt)
        VALUES (@Email, @Name, GETUTCDATE());

        SET @UserId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE usp_User_Update
    @UserId INT,
    @Name VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET Name = @Name
    WHERE UserId = @UserId;

    IF @@ROWCOUNT = 0
        THROW 50001, 'User not found', 1;
END;
GO",
                Tags = new() { "Stored Procedure", "Programmability", "T-SQL" }
            }
        };
    }

    public static List<AzureTopic> GetAzureTopics()
    {
        return new List<AzureTopic>
        {
            new() {
                Title = "Azure App Service",
                Category = "Compute",
                Difficulty = "Easy",
                AzureService = "App Service",
                KeyConcepts = "Web apps, deployment slots, scaling, continuous deployment, custom domains",
                Lesson = @"# Azure App Service

## What is App Service?
Azure App Service is a fully managed platform for building, deploying, and scaling web applications. It supports multiple languages (.NET, Node.js, Python, Java, PHP) and frameworks.

## Key Features

### App Service Plans
Defines the compute resources for your app.

| Tier | Use Case | Features |
|------|----------|----------|
| Free/Shared | Dev/Test | Limited CPU, no custom domain |
| Basic | Low traffic | Custom domain, manual scale |
| Standard | Production | Auto-scale, slots, backups |
| Premium | High performance | Better scaling, VNet |
| Isolated | Enterprise | Dedicated environment |

### Deployment Slots
Staging environments that can be swapped with production.
```bash
# Deploy to staging
az webapp deployment slot create --name myapp --slot staging

# Swap staging to production (zero downtime)
az webapp deployment slot swap --name myapp --slot staging --target-slot production
```

### Continuous Deployment
```yaml
# GitHub Actions workflow
name: Deploy to Azure
on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0'

      - name: Build and publish
        run: |
          dotnet build --configuration Release
          dotnet publish -c Release -o ./publish

      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: 'my-app-name'
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: ./publish
```

### Scaling

#### Vertical Scaling (Scale Up)
Change to a higher App Service Plan tier.
```bash
az appservice plan update --name myplan --sku P1V2
```

#### Horizontal Scaling (Scale Out)
Add more instances.
```bash
# Manual scale to 3 instances
az webapp scale --name myapp --instance-count 3

# Autoscale based on CPU
az monitor autoscale create \
  --resource-group myRG \
  --resource myapp \
  --min-count 1 \
  --max-count 10 \
  --count 2
```

### Configuration
```bash
# Set environment variables
az webapp config appsettings set \
  --name myapp \
  --settings DatabaseConnection=""Server=..."" ASPNETCORE_ENVIRONMENT=""Production""

# Set connection strings
az webapp config connection-string set \
  --name myapp \
  --settings DefaultConnection=""..."" \
  --connection-string-type SQLServer
```

### Custom Domains & SSL
```bash
# Add custom domain
az webapp config hostname add --webapp-name myapp --hostname www.example.com

# Add SSL certificate
az webapp config ssl upload --name myapp --certificate-file cert.pfx --certificate-password pass
az webapp config ssl bind --name myapp --certificate-thumbprint ABC123 --ssl-type SNI
```

## Best Practices
1. Use deployment slots for zero-downtime deployments
2. Enable Application Insights for monitoring
3. Configure autoscaling for production workloads
4. Use managed identities instead of connection strings when possible
5. Enable always-on for production apps",
                CodeExample = @"// Accessing App Service configuration in ASP.NET Core
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuration is automatically loaded from:
        // 1. appsettings.json
        // 2. appsettings.{Environment}.json
        // 3. Environment variables (includes App Service settings)

        var connectionString = builder.Configuration.GetConnectionString(""DefaultConnection"");
        var apiKey = builder.Configuration[""ApiKey""];

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}

// Health check endpoint
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>()
    .AddUrlGroup(new Uri(""https://api.external.com/health""), ""external-api"");

app.MapHealthChecks(""/health"");",
                Tags = new() { "PaaS", "Web Apps", "Hosting" }
            },

            new() {
                Title = "Azure Blob Storage",
                Category = "Storage",
                Difficulty = "Easy",
                AzureService = "Blob Storage",
                KeyConcepts = "Containers, blobs, access tiers, SAS tokens, lifecycle management",
                Lesson = @"# Azure Blob Storage

## What is Blob Storage?
Azure Blob Storage is Microsoft's object storage solution for the cloud. It's optimized for storing massive amounts of unstructured data like images, documents, videos, and backups.

## Key Concepts

### Storage Account
The top-level namespace for your storage services.

### Containers
Like folders, containers organize blobs. A storage account can have unlimited containers.

### Blobs (Types)
- **Block Blobs**: For text and binary data up to 190.7 TB
- **Append Blobs**: Optimized for append operations (logs)
- **Page Blobs**: For random read/write operations (VM disks)

## Access Tiers

| Tier | Use Case | Cost | Access Latency |
|------|----------|------|----------------|
| Hot | Frequently accessed | Higher storage, lower access | Milliseconds |
| Cool | Infrequently accessed (30+ days) | Lower storage, higher access | Milliseconds |
| Cold | Rarely accessed (90+ days) | Even lower storage | Milliseconds |
| Archive | Long-term backup | Lowest storage | Hours to rehydrate |

## Working with Blob Storage in .NET

### Setup
```csharp
// NuGet: Azure.Storage.Blobs
builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration.GetConnectionString(""AzureStorage"")));
```

### Upload Blob
```csharp
public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
{
    var containerClient = _blobServiceClient.GetBlobContainerClient(""uploads"");
    await containerClient.CreateIfNotExistsAsync();

    var blobClient = containerClient.GetBlobClient(fileName);

    await blobClient.UploadAsync(fileStream, new BlobHttpHeaders
    {
        ContentType = ""application/octet-stream""
    });

    return blobClient.Uri.ToString();
}
```

### Download Blob
```csharp
public async Task<Stream> DownloadFileAsync(string fileName)
{
    var containerClient = _blobServiceClient.GetBlobContainerClient(""uploads"");
    var blobClient = containerClient.GetBlobClient(fileName);

    var response = await blobClient.DownloadAsync();
    return response.Value.Content;
}
```

### List Blobs
```csharp
public async Task<List<string>> ListBlobsAsync(string containerName)
{
    var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
    var blobs = new List<string>();

    await foreach (var blob in containerClient.GetBlobsAsync())
    {
        blobs.Add(blob.Name);
    }

    return blobs;
}
```

### Delete Blob
```csharp
public async Task DeleteFileAsync(string fileName)
{
    var containerClient = _blobServiceClient.GetBlobContainerClient(""uploads"");
    var blobClient = containerClient.GetBlobClient(fileName);
    await blobClient.DeleteIfExistsAsync();
}
```

## Shared Access Signatures (SAS)
Generate time-limited URLs for secure access.

```csharp
public string GenerateSasUrl(string blobName, int expiresInMinutes = 60)
{
    var containerClient = _blobServiceClient.GetBlobContainerClient(""uploads"");
    var blobClient = containerClient.GetBlobClient(blobName);

    var sasBuilder = new BlobSasBuilder
    {
        BlobContainerName = ""uploads"",
        BlobName = blobName,
        Resource = ""b"",  // b = blob
        ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(expiresInMinutes)
    };

    sasBuilder.SetPermissions(BlobSasPermissions.Read);

    return blobClient.GenerateSasUri(sasBuilder).ToString();
}
```

## Lifecycle Management
Automatically transition blobs between tiers.

```json
{
  ""rules"": [
    {
      ""name"": ""archiveOldLogs"",
      ""type"": ""Lifecycle"",
      ""definition"": {
        ""filters"": {
          ""prefixMatch"": [""logs/""]
        },
        ""actions"": {
          ""baseBlob"": {
            ""tierToCool"": { ""daysAfterModificationGreaterThan"": 30 },
            ""tierToArchive"": { ""daysAfterModificationGreaterThan"": 90 },
            ""delete"": { ""daysAfterModificationGreaterThan"": 365 }
          }
        }
      }
    }
  ]
}
```

## Best Practices
1. Use meaningful container and blob names
2. Implement lifecycle policies for cost optimization
3. Use SAS tokens instead of account keys
4. Enable soft delete for recovery
5. Use CDN for frequently accessed public content",
                CodeExample = @"// Complete Blob Storage service
public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream content, string fileName, string contentType);
    Task<Stream> DownloadAsync(string fileName);
    Task<bool> DeleteAsync(string fileName);
    string GetSasUrl(string fileName, int expiresInMinutes = 60);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _containerClient = blobServiceClient.GetBlobContainerClient(""files"");
        _containerClient.CreateIfNotExists();
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string contentType)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(content, new BlobHttpHeaders
        {
            ContentType = contentType
        });

        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string fileName)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        var response = await blobClient.DownloadStreamingAsync();
        return response.Value.Content;
    }

    public async Task<bool> DeleteAsync(string fileName)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        var response = await blobClient.DeleteIfExistsAsync();
        return response.Value;
    }

    public string GetSasUrl(string fileName, int expiresInMinutes = 60)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _containerClient.Name,
            BlobName = fileName,
            Resource = ""b"",
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(expiresInMinutes)
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        return blobClient.GenerateSasUri(sasBuilder).ToString();
    }
}",
                Tags = new() { "Storage", "Blobs", "Files" }
            },

            new() {
                Title = "Azure Functions",
                Category = "Compute",
                Difficulty = "Medium",
                AzureService = "Functions",
                KeyConcepts = "Serverless, triggers, bindings, durable functions, consumption plan",
                Lesson = @"# Azure Functions

## What are Azure Functions?
Azure Functions is a serverless compute service that lets you run event-triggered code without managing infrastructure. You only pay for the time your code runs.

## Triggers (What starts the function)

### HTTP Trigger
```csharp
[Function(""HttpExample"")]
public IActionResult Run(
    [HttpTrigger(AuthorizationLevel.Function, ""get"", ""post"")] HttpRequest req,
    FunctionContext context)
{
    var logger = context.GetLogger(""HttpExample"");
    logger.LogInformation(""Processing request"");

    string name = req.Query[""name""];
    return new OkObjectResult($""Hello, {name}"");
}
```

### Timer Trigger (Scheduled)
```csharp
[Function(""TimerExample"")]
public void Run(
    [TimerTrigger(""0 */5 * * * *"")] TimerInfo timer,  // Every 5 minutes
    FunctionContext context)
{
    var logger = context.GetLogger(""TimerExample"");
    logger.LogInformation($""Timer triggered at: {DateTime.Now}"");
}
```

### Blob Trigger
```csharp
[Function(""BlobExample"")]
public void Run(
    [BlobTrigger(""uploads/{name}"")] Stream blob,
    string name,
    FunctionContext context)
{
    var logger = context.GetLogger(""BlobExample"");
    logger.LogInformation($""Blob trigger: {name}, Size: {blob.Length} bytes"");
}
```

### Queue Trigger
```csharp
[Function(""QueueExample"")]
public void Run(
    [QueueTrigger(""myqueue"")] string message,
    FunctionContext context)
{
    var logger = context.GetLogger(""QueueExample"");
    logger.LogInformation($""Queue message: {message}"");
}
```

## Bindings (Input/Output)

### Output Binding (Write to Queue)
```csharp
[Function(""HttpToQueue"")]
public MultiResponse Run(
    [HttpTrigger(AuthorizationLevel.Function, ""post"")] HttpRequest req,
    FunctionContext context)
{
    return new MultiResponse
    {
        HttpResponse = new OkObjectResult(""Message queued""),
        QueueMessage = ""Hello from HTTP trigger""
    };
}

public class MultiResponse
{
    [HttpResult]
    public IActionResult HttpResponse { get; set; }

    [QueueOutput(""outputqueue"")]
    public string QueueMessage { get; set; }
}
```

### Input Binding (Read from Cosmos DB)
```csharp
[Function(""CosmosDbExample"")]
public IActionResult Run(
    [HttpTrigger(AuthorizationLevel.Function, ""get"")] HttpRequest req,
    [CosmosDBInput(
        databaseName: ""mydb"",
        containerName: ""items"",
        Connection = ""CosmosDbConnection"",
        Id = ""{id}"",
        PartitionKey = ""{partitionKey}"")] Item item)
{
    return new OkObjectResult(item);
}
```

## Hosting Plans

| Plan | Scale | Cold Start | Cost |
|------|-------|------------|------|
| Consumption | Auto (0-200 instances) | Yes | Pay per execution |
| Premium | Auto (10-100) | No | Pre-warmed instances |
| Dedicated | Manual/Auto | No | Fixed App Service Plan |

## Dependency Injection
```csharp
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        services.AddSingleton<IMyService, MyService>();
        services.AddDbContext<MyDbContext>();
    })
    .Build();

host.Run();
```

## Durable Functions
For long-running, stateful workflows.

```csharp
// Orchestrator
[Function(""OrderOrchestrator"")]
public async Task<string> RunOrchestrator(
    [OrchestrationTrigger] TaskOrchestrationContext context)
{
    var order = context.GetInput<Order>();

    await context.CallActivityAsync(""ValidateOrder"", order);
    await context.CallActivityAsync(""ProcessPayment"", order);
    await context.CallActivityAsync(""ShipOrder"", order);
    await context.CallActivityAsync(""SendNotification"", order);

    return ""Order completed"";
}

// Activity
[Function(""ProcessPayment"")]
public async Task ProcessPayment(
    [ActivityTrigger] Order order,
    FunctionContext context)
{
    // Process payment logic
}
```

## Best Practices
1. Keep functions small and focused
2. Use Durable Functions for complex workflows
3. Implement retry policies for transient failures
4. Use Application Insights for monitoring
5. Consider cold start impact on latency-sensitive apps",
                CodeExample = @"// Complete Azure Function example with DI
public class OrderFunction
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderFunction> _logger;

    public OrderFunction(IOrderService orderService, ILogger<OrderFunction> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [Function(""CreateOrder"")]
    public async Task<IActionResult> CreateOrder(
        [HttpTrigger(AuthorizationLevel.Function, ""post"")] HttpRequest req)
    {
        _logger.LogInformation(""CreateOrder function triggered"");

        var orderDto = await req.ReadFromJsonAsync<CreateOrderDto>();

        if (orderDto == null)
            return new BadRequestResult();

        try
        {
            var order = await _orderService.CreateAsync(orderDto);
            return new CreatedResult($""/orders/{order.Id}"", order);
        }
        catch (ValidationException ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }

    [Function(""ProcessOrder"")]
    public async Task ProcessOrder(
        [QueueTrigger(""orders-to-process"")] Order order)
    {
        _logger.LogInformation($""Processing order {order.Id}"");

        await _orderService.ProcessAsync(order);

        _logger.LogInformation($""Order {order.Id} processed successfully"");
    }
}",
                Tags = new() { "Serverless", "FaaS", "Event-Driven" }
            }
        };
    }

    public static List<EntityFrameworkTopic> GetEntityFrameworkTopics()
    {
        return new List<EntityFrameworkTopic>
        {
            // Beginner Level
            new() {
                Title = "What is Entity Framework Core?",
                Category = "Fundamentals",
                Difficulty = "Easy",
                EFVersion = "6.0+",
                KeyConcepts = "ORM, DbContext, DbSet, Database-First vs Code-First",
                Lesson = @"# What is Entity Framework Core?

## Definition
Entity Framework Core (EF Core) is a lightweight, extensible, open-source Object-Relational Mapper (ORM) for .NET. It enables developers to work with databases using .NET objects, eliminating the need for most data-access code.

## Key Features
- **Code-First**: Define your model using C# classes
- **Database-First**: Generate models from existing database
- **LINQ Support**: Query databases using C# LINQ
- **Change Tracking**: Automatically tracks changes to entities
- **Migrations**: Version control for your database schema

## Core Components

### DbContext
The primary class for interacting with the database. It represents a session with the database and can be used to query and save instances of entities.

### DbSet<T>
Represents a collection of entities in the database. Used to perform CRUD operations.

## Real-World Use Case
Imagine building an e-commerce application. Instead of writing raw SQL queries, EF Core allows you to:
- Define a Product class
- Use LINQ to query products
- Let EF Core handle SQL generation and database interaction",
                CodeExample = @"// Define your entity
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// Define your DbContext
public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(""connection-string"");
    }
}

// Use EF Core
using (var context = new AppDbContext())
{
    // Add a product
    context.Products.Add(new Product { Name = ""Laptop"", Price = 999 });
    await context.SaveChangesAsync();

    // Query products
    var products = await context.Products.Where(p => p.Price > 500).ToListAsync();
}",
                ProblemScenario = "You need to build a product catalog for an online store. Without EF Core, you'd write hundreds of lines of SQL and ADO.NET code. With EF Core, you can focus on business logic while EF handles database operations.",
                Tags = new() { "ORM", "DbContext", "Introduction" }
            },

            new() {
                Title = "Configuring DbContext",
                Category = "Fundamentals",
                Difficulty = "Easy",
                EFVersion = "6.0+",
                KeyConcepts = "Connection strings, Dependency Injection, DbContextOptions",
                Lesson = @"# Configuring DbContext

## Configuration Methods

### 1. OnConfiguring (Not Recommended for Production)
Override OnConfiguring in your DbContext for simple scenarios or testing.

### 2. Dependency Injection (Recommended)
Configure DbContext in your application startup using DI container.

## Connection Strings
Store connection strings in appsettings.json for security and flexibility.

## Best Practices
- Use DI for testability
- Never hardcode connection strings
- Use different configurations for dev/staging/production
- Enable sensitive data logging only in development",
                CodeExample = @"// appsettings.json
{
  ""ConnectionStrings"": {
    ""DefaultConnection"": ""Server=localhost;Database=MyDb;User=sa;Password=***;""
  }
}

// Program.cs (ASP.NET Core)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(""DefaultConnection"")));

// DbContext
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}

// Usage in Controller
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }
}",
                ProblemScenario = "Your application needs to connect to different databases in dev, staging, and production. Proper DbContext configuration allows you to manage these environments without code changes.",
                Tags = new() { "Configuration", "DI", "Connection String" }
            },

            new() {
                Title = "CRUD Operations",
                Category = "Fundamentals",
                Difficulty = "Easy",
                EFVersion = "6.0+",
                KeyConcepts = "Add, Update, Delete, Find, SaveChanges",
                Lesson = @"# CRUD Operations in EF Core

## Create (Add)
Use `Add()` or `AddRange()` to add entities to the context, then call `SaveChanges()`.

## Read (Query)
Use LINQ queries on DbSet to retrieve data.

## Update
Retrieve entity, modify properties, call SaveChanges(). EF tracks changes automatically.

## Delete
Use `Remove()` or `RemoveRange()` followed by SaveChanges().

## SaveChanges vs SaveChangesAsync
- `SaveChanges()`: Synchronous, blocks thread
- `SaveChangesAsync()`: Asynchronous, recommended for scalability

## Important Concepts
- **Change Tracking**: EF tracks entity state (Added, Modified, Deleted)
- **Unit of Work**: SaveChanges commits all changes in a transaction",
                CodeExample = @"// CREATE
var product = new Product { Name = ""Mouse"", Price = 25 };
context.Products.Add(product);
await context.SaveChangesAsync();

// READ
var product = await context.Products.FindAsync(1); // By primary key
var expensiveProducts = await context.Products
    .Where(p => p.Price > 100)
    .ToListAsync();

// UPDATE
var product = await context.Products.FindAsync(1);
product.Price = 30; // EF tracks this change
await context.SaveChangesAsync();

// DELETE
var product = await context.Products.FindAsync(1);
context.Products.Remove(product);
await context.SaveChangesAsync();

// BULK OPERATIONS
context.Products.AddRange(new[] { product1, product2 });
context.Products.RemoveRange(productsToDelete);
await context.SaveChangesAsync();",
                ProblemScenario = "You're building an inventory management system. You need to add new products, update stock levels, retrieve products by category, and remove discontinued items.",
                Tags = new() { "CRUD", "SaveChanges", "Tracking" }
            },

            // Intermediate Level
            new() {
                Title = "Relationships and Navigation Properties",
                Category = "Relationships",
                Difficulty = "Medium",
                EFVersion = "6.0+",
                KeyConcepts = "One-to-Many, Many-to-Many, Foreign Keys, Navigation Properties",
                Lesson = @"# Relationships in EF Core

## Types of Relationships

### One-to-Many
Most common relationship. One entity relates to multiple entities.
Example: One Category has many Products.

### One-to-One
Each entity relates to exactly one other entity.
Example: One User has one UserProfile.

### Many-to-Many
Entities on both sides can relate to multiple entities.
Example: Students and Courses (requires join table).

## Navigation Properties
Properties that reference related entities.

### Collection Navigation Property
For ""many"" side: `ICollection<T>` or `List<T>`

### Reference Navigation Property
For ""one"" side: Single entity reference

## Foreign Keys
EF Core can infer foreign keys by convention or you can specify explicitly.",
                CodeExample = @"// One-to-Many: Category -> Products
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Collection navigation property
    public ICollection<Product> Products { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Foreign key
    public int CategoryId { get; set; }

    // Reference navigation property
    public Category Category { get; set; }
}

// Many-to-Many (EF Core 5+)
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Course> Courses { get; set; }
}

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public ICollection<Student> Students { get; set; }
}

// Query with relationships
var category = await context.Categories
    .Include(c => c.Products)
    .FirstOrDefaultAsync(c => c.Id == 1);",
                ProblemScenario = "You're building an e-learning platform. Students enroll in multiple courses, and courses have multiple students. You need to efficiently query students with their enrolled courses.",
                Tags = new() { "Relationships", "Navigation", "Foreign Keys" }
            },

            new() {
                Title = "Lazy Loading vs Eager Loading vs Explicit Loading",
                Category = "Querying",
                Difficulty = "Medium",
                EFVersion = "6.0+",
                KeyConcepts = "Include, ThenInclude, Load, N+1 Problem",
                Lesson = @"# Loading Strategies in EF Core

## Eager Loading
Load related data as part of initial query using `Include()` and `ThenInclude()`.
**When to Use**: When you know you'll need related data.

## Lazy Loading
Related data is loaded automatically when navigation property is accessed.
**When to Use**: Rarely in web apps (can cause N+1 problem).

## Explicit Loading
Manually load related data when needed using `Load()`.
**When to Use**: When you sometimes need related data.

## The N+1 Problem
Without eager loading, accessing navigation properties in a loop causes N+1 database queries.
Example: Loading 100 orders, then accessing Order.Customer for each = 101 queries!

## Best Practices
- Use eager loading for known relationships
- Avoid lazy loading in web applications
- Project only needed fields with Select()",
                CodeExample = @"// EAGER LOADING (Best for known relationships)
var orders = await context.Orders
    .Include(o => o.Customer)
    .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
    .ToListAsync();

// EXPLICIT LOADING
var order = await context.Orders.FindAsync(1);
await context.Entry(order).Collection(o => o.OrderItems).LoadAsync();

// LAZY LOADING (requires lazy loading proxies)
// Install: Microsoft.EntityFrameworkCore.Proxies
// Enable in DbContext:
// options.UseLazyLoadingProxies()

public class Order
{
    public int Id { get; set; }
    public virtual Customer Customer { get; set; } // virtual enables lazy loading
}

// PROJECTION (Best for performance)
var orderSummaries = await context.Orders
    .Select(o => new OrderSummaryDto
    {
        OrderId = o.Id,
        CustomerName = o.Customer.Name,
        TotalItems = o.OrderItems.Count
    })
    .ToListAsync();

// BAD: N+1 Problem
var orders = await context.Orders.ToListAsync(); // 1 query
foreach (var order in orders)
{
    var customer = order.Customer.Name; // N queries (if lazy loading)
}",
                ProblemScenario = "Your e-commerce dashboard displays orders with customer names and item counts. Without proper loading strategy, displaying 100 orders could result in 200+ database queries, killing performance.",
                Tags = new() { "Include", "Performance", "N+1 Problem" }
            },

            new() {
                Title = "Migrations",
                Category = "Migrations",
                Difficulty = "Medium",
                EFVersion = "6.0+",
                KeyConcepts = "Add-Migration, Update-Database, Code-First",
                Lesson = @"# EF Core Migrations

## What are Migrations?
Migrations provide a way to incrementally update the database schema to keep it in sync with your application's data model.

## Migration Workflow
1. Modify your entity classes
2. Create a migration (generates code)
3. Review migration code
4. Apply migration to database

## Common Commands

### .NET CLI
- `dotnet ef migrations add InitialCreate`
- `dotnet ef database update`
- `dotnet ef migrations remove`
- `dotnet ef database update LastGoodMigration` (rollback)

### Package Manager Console
- `Add-Migration InitialCreate`
- `Update-Database`
- `Remove-Migration`

## Best Practices
- Name migrations descriptively (AddProductTable, AddPriceToProduct)
- Review generated SQL before applying
- Never modify applied migrations
- Use migrations in all environments (dev, staging, prod)
- Keep migrations in source control",
                CodeExample = @"// 1. Create entities
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// 2. Create migration
// dotnet ef migrations add AddProductTable

// Generated migration Up method:
public partial class AddProductTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: ""Products"",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation(""SqlServer:Identity"", ""1, 1""),
                Name = table.Column<string>(nullable: true),
                Price = table.Column<decimal>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(""PK_Products"", x => x.Id);
            });
    }
}

// 3. Apply migration
// dotnet ef database update

// 4. Add new property
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } // NEW
}

// 5. Create new migration
// dotnet ef migrations add AddDescriptionToProduct",
                ProblemScenario = "Your app is in production. You need to add a new 'Category' field to products without losing existing data or taking the app offline. Migrations allow safe, versioned schema changes.",
                Tags = new() { "Migrations", "Schema", "Code-First" }
            },

            // Advanced Level
            new() {
                Title = "Advanced LINQ Queries and Projections",
                Category = "Querying",
                Difficulty = "Hard",
                EFVersion = "6.0+",
                KeyConcepts = "GroupBy, Aggregations, Subqueries, Window Functions",
                Lesson = @"# Advanced Querying in EF Core

## Complex LINQ Queries
EF Core translates LINQ to SQL. Understanding what translates efficiently is crucial.

## Projections
Select only needed data to reduce database load and memory usage.

## Aggregations
Use Sum, Count, Average, Max, Min for analytical queries.

## GroupBy
Group data for reporting and analytics.

## Window Functions (EF Core 6+)
Perform calculations across rows related to current row.

## Subqueries
Queries within queries for complex filtering.

## Performance Tips
- Use AsNoTracking() for read-only queries
- Project to DTOs instead of loading full entities
- Use compiled queries for frequently executed queries",
                CodeExample = @"// PROJECTION to DTO
var productSummaries = await context.Products
    .Select(p => new ProductSummaryDto
    {
        Id = p.Id,
        Name = p.Name,
        CategoryName = p.Category.Name,
        OrderCount = p.OrderItems.Count
    })
    .ToListAsync();

// GROUPBY for reporting
var salesByCategory = await context.OrderItems
    .GroupBy(oi => oi.Product.Category.Name)
    .Select(g => new
    {
        Category = g.Key,
        TotalSales = g.Sum(oi => oi.Quantity * oi.Price),
        OrderCount = g.Count()
    })
    .ToListAsync();

// WINDOW FUNCTIONS (EF Core 6+)
var productsWithRank = await context.Products
    .Select(p => new
    {
        p.Name,
        p.Price,
        Rank = EF.Functions.RowNumber(
            EF.Functions.OrderBy(p.Price)
        )
    })
    .ToListAsync();

// SUBQUERY
var topCategories = await context.Categories
    .Where(c => c.Products.Any(p => p.Price > 100))
    .Select(c => new
    {
        c.Name,
        ExpensiveProductCount = c.Products.Count(p => p.Price > 100)
    })
    .ToListAsync();

// ASNOTRACKING for read-only
var products = await context.Products
    .AsNoTracking()
    .Where(p => p.Price > 50)
    .ToListAsync();

// COMPILED QUERIES for repeated queries
var getActiveProducts = EF.CompileAsyncQuery(
    (AppDbContext context, int minPrice) =>
        context.Products.Where(p => p.Price > minPrice));

var products = await getActiveProducts(context, 100);",
                ProblemScenario = "Your sales dashboard needs to show total revenue by category, top-selling products, and average order value - all from a database with millions of records. Efficient querying is critical.",
                Tags = new() { "LINQ", "Performance", "Projections" }
            },

            new() {
                Title = "Performance Optimization Techniques",
                Category = "Performance",
                Difficulty = "Hard",
                EFVersion = "6.0+",
                KeyConcepts = "AsNoTracking, Indexes, Batching, Connection Pooling",
                Lesson = @"# Performance Optimization in EF Core

## 1. AsNoTracking()
Disable change tracking for read-only queries. Can improve performance by 30-50%.

## 2. Indexes
Create indexes on frequently queried columns.

## 3. Batching
EF Core automatically batches multiple commands into single database round-trip.

## 4. Connection Pooling
Reuse database connections. Enabled by default.

## 5. Compiled Queries
Pre-compile LINQ queries for 10-30% performance gain on repeated queries.

## 6. Select vs Include
Project only needed data instead of loading entire entities.

## 7. Split Queries
For complex includes, split into multiple queries to avoid cartesian explosion.

## 8. Bulk Operations
Use libraries like EFCore.BulkExtensions for inserting/updating thousands of records.

## Monitoring
- Use logging to see generated SQL
- Profile database with SQL Server Profiler
- Monitor query execution time",
                CodeExample = @"// 1. AsNoTracking for read-only
var products = await context.Products
    .AsNoTracking()
    .ToListAsync();

// 2. Create Index in OnModelCreating
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>()
        .HasIndex(p => p.Name);

    modelBuilder.Entity<Order>()
        .HasIndex(o => new { o.CustomerId, o.OrderDate });
}

// 3. Batching (automatic in EF Core 7+)
var products = new List<Product> { /* ... */ };
context.Products.AddRange(products);
await context.SaveChangesAsync(); // All inserted in batches

// 4. Compiled Queries
private static readonly Func<AppDbContext, int, Task<List<Product>>>
    _getProductsByCategory = EF.CompileAsyncQuery(
        (AppDbContext context, int categoryId) =>
            context.Products.Where(p => p.CategoryId == categoryId).ToList());

var products = await _getProductsByCategory(context, 5);

// 5. Projection vs Include
// BAD: Loads all product data
var orders = await context.Orders
    .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
    .ToListAsync();

// GOOD: Only loads needed fields
var orders = await context.Orders
    .Select(o => new OrderDto
    {
        OrderId = o.Id,
        Items = o.OrderItems.Select(oi => new OrderItemDto
        {
            ProductName = oi.Product.Name,
            Quantity = oi.Quantity
        }).ToList()
    })
    .ToListAsync();

// 6. Split Queries (for large includes)
var blogs = await context.Blogs
    .Include(b => b.Posts)
    .Include(b => b.Contributors)
    .AsSplitQuery() // Prevents cartesian explosion
    .ToListAsync();

// 7. Enable query logging
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging()); // Dev only!",
                ProblemScenario = "Your application dashboard takes 5 seconds to load because it queries 10 tables with complex joins. Users are complaining. You need to optimize without changing functionality.",
                Tags = new() { "Performance", "Optimization", "Indexes" }
            },

            new() {
                Title = "Global Query Filters",
                Category = "Advanced Features",
                Difficulty = "Hard",
                EFVersion = "6.0+",
                KeyConcepts = "Soft Delete, Multi-Tenancy, HasQueryFilter",
                Lesson = @"# Global Query Filters

## What are Global Query Filters?
Filters automatically applied to all queries for an entity type. Defined once in OnModelCreating.

## Common Use Cases

### 1. Soft Delete
Instead of deleting records, mark them as deleted. Filter excludes them from all queries.

### 2. Multi-Tenancy
Automatically filter data by tenant/organization.

### 3. Data Partitioning
Filter by region, status, etc.

## How It Works
Define filter in OnModelCreating using HasQueryFilter(). Filter applied to ALL queries unless explicitly ignored.

## Ignoring Filters
Use `IgnoreQueryFilters()` when you need to see filtered data.",
                CodeExample = @"// Entity with soft delete
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
}

// Multi-tenant entity
public class Order
{
    public int Id { get; set; }
    public string TenantId { get; set; }
    public DateTime OrderDate { get; set; }
}

// Configure global filters in DbContext
public class AppDbContext : DbContext
{
    public string CurrentTenantId { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Soft delete filter
        modelBuilder.Entity<Product>()
            .HasQueryFilter(p => !p.IsDeleted);

        // Multi-tenancy filter
        modelBuilder.Entity<Order>()
            .HasQueryFilter(o => o.TenantId == CurrentTenantId);
    }
}

// Usage - filters applied automatically
var products = await context.Products.ToListAsync();
// Only non-deleted products returned

var orders = await context.Orders.ToListAsync();
// Only current tenant's orders returned

// Ignoring filters when needed
var allProducts = await context.Products
    .IgnoreQueryFilters()
    .ToListAsync();
// Returns all products including deleted

// Soft delete implementation
var product = await context.Products.FindAsync(1);
product.IsDeleted = true; // Don't actually delete
await context.SaveChangesAsync();

// Product now excluded from all queries automatically
var products = await context.Products.ToListAsync(); // Won't include soft-deleted product",
                ProblemScenario = "You're building a SaaS application serving 1000+ organizations. Each organization should only see their own data. Without global filters, you'd need to add TenantId checks to every single query.",
                Tags = new() { "Soft Delete", "Multi-Tenancy", "Security" }
            },

            new() {
                Title = "Concurrency Handling",
                Category = "Advanced Features",
                Difficulty = "Hard",
                EFVersion = "6.0+",
                KeyConcepts = "Concurrency Tokens, Row Version, Optimistic Concurrency",
                Lesson = @"# Concurrency in EF Core

## The Problem
Two users modify the same record simultaneously. Without concurrency handling, last write wins, potentially losing data.

## Optimistic Concurrency
Assume conflicts are rare. Detect conflicts when saving and let user resolve.

## Concurrency Token
A property that changes each time row is updated. EF checks token before updating.

### RowVersion (SQL Server)
Special byte[] property automatically managed by database.

### ConcurrencyCheck
Any property can be marked as concurrency token.

## Conflict Resolution
When DbUpdateConcurrencyException occurs:
1. Reload current database values
2. Show user what changed
3. Let user decide: overwrite, cancel, or merge

## Pessimistic Concurrency (Locking)
Lock records during edit. Prevents conflicts but reduces concurrency. Use sparingly.",
                CodeExample = @"// Using RowVersion (SQL Server)
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}

// Configure via Fluent API
modelBuilder.Entity<Product>()
    .Property(p => p.RowVersion)
    .IsRowVersion();

// Using any property as concurrency token
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }

    [ConcurrencyCheck]
    public DateTime LastModified { get; set; }
}

// Handling concurrency conflicts
try
{
    var product = await context.Products.FindAsync(1);
    product.Price = 299;
    await context.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException ex)
{
    var entry = ex.Entries.Single();
    var databaseValues = await entry.GetDatabaseValuesAsync();

    if (databaseValues == null)
    {
        // Record was deleted
        Console.WriteLine(""Product was deleted by another user"");
    }
    else
    {
        // Record was modified
        var databaseProduct = (Product)databaseValues.ToObject();

        Console.WriteLine($""Database price: {databaseProduct.Price}"");
        Console.WriteLine($""Your price: {product.Price}"");

        // Option 1: Overwrite database
        entry.OriginalValues.SetValues(databaseValues);
        await context.SaveChangesAsync();

        // Option 2: Refresh your values
        entry.Reload();

        // Option 3: Show user and let them decide
        // ... UI logic ...
    }
}

// Pessimistic locking (SQL Server)
using var transaction = await context.Database.BeginTransactionAsync();
var product = await context.Products
    .FromSqlRaw(""SELECT * FROM Products WITH (UPDLOCK, ROWLOCK) WHERE Id = {0}"", id)
    .FirstOrDefaultAsync();

// User now has exclusive lock on this product
product.Price = 299;
await context.SaveChangesAsync();
await transaction.CommitAsync();",
                ProblemScenario = "In your inventory system, two warehouse managers try to update the same product's stock simultaneously. Without concurrency handling, one update silently overwrites the other, causing inventory discrepancies.",
                Tags = new() { "Concurrency", "RowVersion", "Conflicts" }
            },

            new() {
                Title = "Raw SQL and Stored Procedures",
                Category = "Advanced Features",
                Difficulty = "Medium",
                EFVersion = "6.0+",
                KeyConcepts = "FromSqlRaw, ExecuteSqlRaw, Stored Procedures, Performance",
                Lesson = @"# Raw SQL in EF Core

## When to Use Raw SQL
- Complex queries EF can't translate
- Performance-critical queries
- Legacy stored procedures
- Database-specific features

## FromSqlRaw / FromSqlInterpolated
Execute raw SELECT queries, return entities.

## ExecuteSqlRaw / ExecuteSqlInterpolated
Execute INSERT, UPDATE, DELETE, or DDL commands.

## SQL Injection Prevention
ALWAYS use parameterized queries. Never concatenate user input into SQL strings.

## Stored Procedures
Call existing stored procedures from EF Core.

## Limitations
- Can only return entities defined in your model
- Can't use LINQ on raw SQL results directly (but can compose)",
                CodeExample = @"// FromSqlRaw - SELECT queries
var products = await context.Products
    .FromSqlRaw(""SELECT * FROM Products WHERE Price > {0}"", 100)
    .ToListAsync();

// FromSqlInterpolated - safer, prevents SQL injection
decimal minPrice = 100;
var products = await context.Products
    .FromSqlInterpolated($""SELECT * FROM Products WHERE Price > {minPrice}"")
    .ToListAsync();

// Can compose with LINQ
var filteredProducts = await context.Products
    .FromSqlRaw(""SELECT * FROM Products WHERE CategoryId = {0}"", categoryId)
    .Where(p => p.Price > 50) // Composed in SQL
    .OrderBy(p => p.Name)
    .ToListAsync();

// ExecuteSqlRaw - non-query commands
var rowsAffected = await context.Database
    .ExecuteSqlRawAsync(""UPDATE Products SET Price = Price * 1.1 WHERE CategoryId = {0}"", categoryId);

// ExecuteSqlInterpolated
await context.Database
    .ExecuteSqlInterpolatedAsync($""DELETE FROM Products WHERE IsDeleted = {true}"");

// Calling stored procedures
var products = await context.Products
    .FromSqlRaw(""EXEC GetProductsByCategory @CategoryId = {0}"", categoryId)
    .ToListAsync();

// Stored procedure with output parameter
var categoryIdParam = new SqlParameter
{
    ParameterName = ""@CategoryId"",
    SqlDbType = SqlDbType.Int,
    Value = 5
};

var totalParam = new SqlParameter
{
    ParameterName = ""@Total"",
    SqlDbType = SqlDbType.Decimal,
    Direction = ParameterDirection.Output
};

await context.Database.ExecuteSqlRawAsync(
    ""EXEC CalculateCategoryTotal @CategoryId, @Total OUTPUT"",
    categoryIdParam, totalParam);

var total = (decimal)totalParam.Value;

// WARNING: SQL Injection vulnerability
string userInput = ""'; DROP TABLE Products--"";
// NEVER DO THIS:
var bad = context.Products.FromSqlRaw($""SELECT * FROM Products WHERE Name = '{userInput}'"");

// SAFE: Use parameters
var safe = context.Products.FromSqlInterpolated($""SELECT * FROM Products WHERE Name = {userInput}"");",
                ProblemScenario = "Your database has complex reporting stored procedures built by the DBA team over years. You need to integrate these into your EF Core application without rewriting them in LINQ.",
                Tags = new() { "Raw SQL", "Stored Procedures", "Performance" }
            },

            new() {
                Title = "Value Conversions",
                Category = "Advanced Features",
                Difficulty = "Medium",
                EFVersion = "6.0+",
                KeyConcepts = "HasConversion, Enum to String, JSON columns, Encryption",
                Lesson = @"# Value Conversions in EF Core

## What are Value Conversions?
Transform property values when reading from or writing to database.

## Common Use Cases
1. **Enum to String**: Store enums as strings instead of ints
2. **JSON Columns**: Store complex objects as JSON
3. **Encryption**: Encrypt sensitive data
4. **Custom Types**: Convert between C# types and database types
5. **Date/Time**: Store UTC, convert to local

## Built-in Conversions
EF Core provides many built-in conversions.

## Custom Conversions
Create your own for specific needs.",
                CodeExample = @"// Enum to String conversion
public enum ProductStatus
{
    Active,
    Discontinued,
    OutOfStock
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProductStatus Status { get; set; }
}

// Configure in OnModelCreating
modelBuilder.Entity<Product>()
    .Property(p => p.Status)
    .HasConversion<string>(); // Stored as ""Active"", ""Discontinued"", etc.

// JSON column (EF Core 7+)
public class Order
{
    public int Id { get; set; }
    public Address ShippingAddress { get; set; } // Complex type
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
}

modelBuilder.Entity<Order>()
    .Property(o => o.ShippingAddress)
    .HasConversion(
        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
        v => JsonSerializer.Deserialize<Address>(v, (JsonSerializerOptions)null));

// Encryption conversion
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string SocialSecurityNumber { get; set; }
}

modelBuilder.Entity<User>()
    .Property(u => u.SocialSecurityNumber)
    .HasConversion(
        v => Encrypt(v),
        v => Decrypt(v));

// UTC DateTime conversion
modelBuilder.Entity<Order>()
    .Property(o => o.CreatedAt)
    .HasConversion(
        v => v.ToUniversalTime(),
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

// List to comma-separated string
public class Product
{
    public int Id { get; set; }
    public List<string> Tags { get; set; }
}

modelBuilder.Entity<Product>()
    .Property(p => p.Tags)
    .HasConversion(
        v => string.Join(',', v),
        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

// Value object conversion
public class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
}

public class Product
{
    public int Id { get; set; }
    public Money Price { get; set; }
}

modelBuilder.Entity<Product>()
    .Property(p => p.Price)
    .HasConversion(
        v => $""{v.Amount}|{v.Currency}"",
        v => new Money(
            decimal.Parse(v.Split('|')[0]),
            v.Split('|')[1]));",
                ProblemScenario = "Your application stores user preferences as a C# object, but the database needs them as JSON. You also need to encrypt Social Security Numbers at rest. Value conversions handle these automatically.",
                Tags = new() { "Conversions", "JSON", "Encryption" }
            },

            new() {
                Title = "Database Transactions",
                Category = "Advanced Features",
                Difficulty = "Medium",
                EFVersion = "6.0+",
                KeyConcepts = "BeginTransaction, Rollback, ACID, Isolation Levels",
                Lesson = @"# Transactions in EF Core

## What is a Transaction?
A unit of work that either completely succeeds or completely fails. Ensures data consistency.

## ACID Properties
- **Atomicity**: All or nothing
- **Consistency**: Data remains valid
- **Isolation**: Concurrent transactions don't interfere
- **Durability**: Committed data persists

## Default Behavior
SaveChanges() automatically wraps changes in a transaction.

## Manual Transactions
Use when you need to:
- Span multiple SaveChanges calls
- Include raw SQL commands
- Control isolation level
- Coordinate with external resources

## Isolation Levels
- **ReadUncommitted**: Fastest, but can read uncommitted changes (dirty reads)
- **ReadCommitted**: Default, prevents dirty reads
- **RepeatableRead**: Prevents non-repeatable reads
- **Serializable**: Strongest isolation, slowest
- **Snapshot**: SQL Server specific, good balance",
                CodeExample = @"// Default: SaveChanges uses implicit transaction
var order = new Order { /* ... */ };
context.Orders.Add(order);
await context.SaveChangesAsync(); // Atomic transaction

// Manual transaction spanning multiple SaveChanges
using var transaction = await context.Database.BeginTransactionAsync();
try
{
    // Create order
    var order = new Order { CustomerId = 1, Total = 100 };
    context.Orders.Add(order);
    await context.SaveChangesAsync();

    // Update inventory
    var product = await context.Products.FindAsync(1);
    product.Stock -= 5;
    await context.SaveChangesAsync();

    // Send email (external operation)
    await emailService.SendOrderConfirmationAsync(order);

    // All succeeded, commit
    await transaction.CommitAsync();
}
catch (Exception)
{
    // Something failed, rollback everything
    await transaction.RollbackAsync();
    throw;
}

// Mixing EF and raw SQL in transaction
using var transaction = await context.Database.BeginTransactionAsync();
try
{
    // EF operation
    var order = new Order { /* ... */ };
    context.Orders.Add(order);
    await context.SaveChangesAsync();

    // Raw SQL operation
    await context.Database.ExecuteSqlRawAsync(
        ""UPDATE Inventory SET Stock = Stock - {0} WHERE ProductId = {1}"",
        quantity, productId);

    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}

// Setting isolation level
using var transaction = await context.Database.BeginTransactionAsync(
    IsolationLevel.ReadCommitted);
// ... operations ...
await transaction.CommitAsync();

// Using transaction with different DbContext instances
var strategy = context.Database.CreateExecutionStrategy();
await strategy.ExecuteAsync(async () =>
{
    using var transaction = await context.Database.BeginTransactionAsync();

    // Use same transaction in different context
    using var context2 = new AppDbContext(options);
    await context2.Database.UseTransactionAsync(transaction.GetDbTransaction());

    // Operations on both contexts
    context.Orders.Add(new Order());
    await context.SaveChangesAsync();

    context2.Products.Add(new Product());
    await context2.SaveChangesAsync();

    await transaction.CommitAsync();
});",
                ProblemScenario = "Customer places an order. You need to: 1) Create order record, 2) Decrease product inventory, 3) Charge credit card, 4) Send confirmation email. If ANY step fails, everything must rollback to prevent inconsistent data.",
                Tags = new() { "Transactions", "ACID", "Consistency" }
            },

            new() {
                Title = "Owned Entity Types and Table Splitting",
                Category = "Advanced Features",
                Difficulty = "Hard",
                EFVersion = "6.0+",
                KeyConcepts = "OwnsOne, Value Objects, Table Splitting, DDD",
                Lesson = @"# Owned Entity Types in EF Core

## What are Owned Types?
Entity types that don't have their own identity and only exist as part of another entity. Based on Domain-Driven Design (DDD) value objects.

## When to Use
- Address, Money, DateRange (value objects)
- Data that doesn't make sense without parent entity
- Encapsulating related properties

## Configuration
Use `OwnsOne()` or `OwnsMany()` in OnModelCreating.

## Table Splitting
Multiple entity types mapped to same table.

## Navigation vs Owned Types
- **Navigation**: Separate entity with its own identity and table
- **Owned**: No separate identity, can share parent's table",
                CodeExample = @"// Value object
public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}

// Entity with owned types
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Owned type - no separate identity
    public Address BillingAddress { get; set; }
    public Address ShippingAddress { get; set; }
}

// Configure owned types
modelBuilder.Entity<Customer>()
    .OwnsOne(c => c.BillingAddress, address =>
    {
        address.Property(a => a.Street).HasColumnName(""BillingStreet"");
        address.Property(a => a.City).HasColumnName(""BillingCity"");
        address.Property(a => a.State).HasColumnName(""BillingState"");
        address.Property(a => a.ZipCode).HasColumnName(""BillingZip"");
    });

modelBuilder.Entity<Customer>()
    .OwnsOne(c => c.ShippingAddress, address =>
    {
        address.Property(a => a.Street).HasColumnName(""ShippingStreet"");
        address.Property(a => a.City).HasColumnName(""ShippingCity"");
        address.Property(a => a.State).HasColumnName(""ShippingState"");
        address.Property(a => a.ZipCode).HasColumnName(""ShippingZip"");
    });

// Results in single table:
// Customers: Id, Name, BillingStreet, BillingCity, ..., ShippingStreet, ShippingCity, ...

// OwnsMany - collection of owned types
public class Order
{
    public int Id { get; set; }
    public List<LineItem> LineItems { get; set; }
}

public class LineItem // Owned type
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

modelBuilder.Entity<Order>()
    .OwnsMany(o => o.LineItems, li =>
    {
        li.WithOwner().HasForeignKey(""OrderId"");
        li.Property<int>(""Id"");
        li.HasKey(""Id"");
    });

// Table Splitting - different entities, same table
public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderDetails Details { get; set; }
}

public class OrderDetails
{
    public int Id { get; set; }
    public string ShippingMethod { get; set; }
    public string Notes { get; set; }
}

modelBuilder.Entity<Order>()
    .HasOne(o => o.Details)
    .WithOne()
    .HasForeignKey<OrderDetails>(d => d.Id);

modelBuilder.Entity<Order>().ToTable(""Orders"");
modelBuilder.Entity<OrderDetails>().ToTable(""Orders""); // Same table!

// Querying owned types
var customer = await context.Customers
    .Where(c => c.BillingAddress.City == ""Seattle"")
    .FirstOrDefaultAsync();",
                ProblemScenario = "You're building an e-commerce system. Every customer has billing and shipping addresses. These addresses don't exist independently - they're always part of a customer. Owned types prevent creating unnecessary Address table and foreign keys.",
                Tags = new() { "Owned Types", "DDD", "Value Objects" }
            }
        };
    }

    public static List<SystemDesignTopic> GetSystemDesignTopicsWithLessons()
    {
        return new List<SystemDesignTopic>
        {
            // Fundamentals
            new() {
                Title = "CAP Theorem - The Foundation of Distributed Systems",
                Category = "Fundamentals",
                Difficulty = "Medium",
                KeyConcepts = "Consistency, Availability, Partition Tolerance - Choose only 2",
                Lesson = @"# CAP Theorem

## What is CAP?

**C**onsistency: All nodes see the same data at the same time
**A**vailability: Every request receives a response (success or failure)
**P**artition Tolerance: System continues despite network partitions

## The Core Principle

**You can only guarantee 2 out of 3** when a network partition occurs.

## Real-World Trade-offs

### CP Systems (Consistency + Partition Tolerance)
- **Example**: Banking systems, MongoDB (in certain configurations)
- **Trade-off**: May become unavailable during network issues
- **When**: Financial transactions, inventory management
- **Why**: Better to be unavailable than show wrong balance

### AP Systems (Availability + Partition Tolerance)
- **Example**: DNS, Cassandra, DynamoDB
- **Trade-off**: May return stale or different data on different nodes
- **When**: Social media feeds, product catalogs
- **Why**: Better to show slightly old data than be down

### CA Systems (Consistency + Availability)
- **Reality**: Can't truly exist in distributed systems
- **Example**: Single-node databases (not truly distributed)
- **Why**: Network partitions are inevitable in distributed systems

## Interview Insights

When asked ""Would you choose CP or AP?"":
1. Ask about use case
2. Discuss consistency requirements
3. Analyze failure scenarios
4. Consider business impact

## Example Scenario

**E-commerce Product Catalog**:
- **AP** for browsing (okay if price is 2 seconds old)
- **CP** for checkout (must have consistent inventory)
- **Solution**: Hybrid approach with different systems

## Common Misconceptions

❌ ""Choose 2 permanently"" → Actually, choose 2 **during partition**
❌ ""No system can be CA"" → Single-node systems can, but aren't distributed
✅ During normal operation, you get all 3",
                Resources = "https://www.youtube.com/watch?v=k-Yaq8AHlFA",
                Tags = new() { "Distributed Systems", "Theory", "Trade-offs" }
            },

            new() {
                Title = "Horizontal vs Vertical Scaling",
                Category = "Fundamentals",
                Difficulty = "Easy",
                KeyConcepts = "Scale Up vs Scale Out, Cost vs Complexity",
                Lesson = @"# Scaling Strategies

## Vertical Scaling (Scale Up)

**Add more power to existing machine** (more CPU, RAM, storage)

### Pros:
- ✅ Simpler code (no distributed complexity)
- ✅ No network latency
- ✅ ACID transactions easier
- ✅ Easier to maintain

### Cons:
- ❌ Physical limits (can't infinitely upgrade)
- ❌ Single point of failure
- ❌ Downtime during upgrades
- ❌ Eventually becomes very expensive

### When to Use:
- Startups/MVPs
- Databases that need ACID guarantees
- Applications not designed for distribution

## Horizontal Scaling (Scale Out)

**Add more machines** to distribute load

### Pros:
- ✅ Near-infinite scaling potential
- ✅ Better fault tolerance (redundancy)
- ✅ No downtime for upgrades (rolling deployment)
- ✅ Cost-effective at scale

### Cons:
- ❌ Complex code (distributed systems complexity)
- ❌ Data consistency challenges
- ❌ Network latency between nodes
- ❌ More infrastructure to manage

### When to Use:
- High traffic applications
- Global user base
- Unpredictable growth
- Need high availability

## Real-World Example

**Netflix**:
- **Vertically scaled**: Database servers (large instances)
- **Horizontally scaled**: Web servers, API servers (thousands of instances)

## Decision Matrix

| Factor | Vertical | Horizontal |
|--------|----------|------------|
| Initial cost | Lower | Higher |
| Long-term cost | Higher | Lower |
| Complexity | Low | High |
| Max scale | Limited | Unlimited |
| Downtime | Yes | No |
| Fault tolerance | Low | High |

## Interview Tip

Don't say ""horizontal is always better"". Explain:
1. Current scale and growth
2. Cost constraints
3. Team expertise
4. Specific use case requirements",
                Resources = "https://www.youtube.com/watch?v=xpDnVSmNFX0",
                Tags = new() { "Scaling", "Architecture", "Fundamentals" }
            },

            new() {
                Title = "Load Balancing - Distributing Traffic Efficiently",
                Category = "Load Balancing",
                Difficulty = "Medium",
                KeyConcepts = "Round Robin, Least Connections, Weighted, Health Checks",
                Lesson = @"# Load Balancing

## What is a Load Balancer?

Distributes incoming traffic across multiple servers to:
- Improve responsiveness
- Increase availability
- Prevent overload

## Load Balancing Algorithms

### 1. Round Robin
Distributes requests sequentially across servers.
- **Pros**: Simple, fair distribution
- **Cons**: Doesn't account for server load or capacity
- **When**: Servers have similar capacity

### 2. Least Connections
Sends requests to server with fewest active connections.
- **Pros**: Better for long-lived connections
- **Cons**: Slight overhead to track connections
- **When**: WebSockets, long-polling, video streaming

### 3. Weighted Round Robin
Servers with higher weights receive more requests.
- **Pros**: Accounts for different server capacities
- **Cons**: Need to configure weights manually
- **When**: Mixed server hardware

### 4. IP Hash / Sticky Sessions
Same client always goes to same server (based on IP hash).
- **Pros**: Session persistence without shared storage
- **Cons**: Uneven distribution if traffic is skewed
- **When**: Shopping carts, user sessions (legacy apps)

### 5. Least Response Time
Routes to server with fastest response time.
- **Pros**: Best user experience
- **Cons**: More complex to implement
- **When**: Performance-critical applications

## L4 vs L7 Load Balancing

### Layer 4 (Transport Layer)
- Routes based on IP and port
- Faster (less inspection)
- Cannot read application data
- **Example**: TCP/UDP load balancing

### Layer 7 (Application Layer)
- Routes based on HTTP headers, cookies, URL path
- Slower (more inspection)
- Can make intelligent routing decisions
- **Example**: Route /api to API servers, /static to CDN

## Health Checks

Load balancers ping servers periodically to check health:
- **Active checks**: Send requests to health endpoint
- **Passive checks**: Monitor real traffic for errors

If server fails health check → removed from pool

## Real-World Architecture

```
[Users] → [DNS] → [Global LB (L7)]
                       ↓
        ┌──────────────┼──────────────┐
        ↓              ↓              ↓
    [Regional LB]  [Regional LB]  [Regional LB]
        ↓              ↓              ↓
    [Servers]      [Servers]      [Servers]
```

## Common Interview Questions

**Q: How do you handle session persistence?**
A: Options:
1. Sticky sessions (IP hash)
2. Session store (Redis)
3. JWT tokens (stateless)

**Q: Load balancer is a single point of failure?**
A: Use multiple load balancers with:
- Active-passive (failover)
- Active-active (DNS round-robin)

**Q: How to add/remove servers without downtime?**
A: Graceful shutdown:
1. Stop accepting new requests
2. Wait for existing requests to finish
3. Remove from load balancer pool",
                Resources = "https://aws.amazon.com/what-is/load-balancing/",
                Tags = new() { "Load Balancing", "Availability", "Scalability" }
            },

            new() {
                Title = "Caching Strategies - Read Performance Optimization",
                Category = "Caching",
                Difficulty = "Medium",
                KeyConcepts = "Cache-Aside, Read-Through, Write-Through, Write-Behind",
                Lesson = @"# Caching Strategies

## Why Cache?

- **Speed**: RAM is 100,000x faster than disk
- **Reduce load**: Fewer database queries
- **Cost**: Less expensive than scaling database

## Caching Patterns

### 1. Cache-Aside (Lazy Loading)

**Flow**:
1. Check cache
2. If miss → fetch from DB
3. Update cache
4. Return data

**Pros**:
- ✅ Only cache what's needed
- ✅ Cache failures don't break system

**Cons**:
- ❌ First request is slow (cache miss)
- ❌ Stale data possible

**Code**:
```csharp
public async Task<User> GetUser(int id)
{
    // 1. Try cache
    var cached = await cache.GetAsync($""user:{id}"");
    if (cached != null) return cached;

    // 2. Cache miss - fetch from DB
    var user = await db.Users.FindAsync(id);

    // 3. Update cache
    await cache.SetAsync($""user:{id}"", user, TimeSpan.FromMinutes(10));

    return user;
}
```

### 2. Read-Through

Cache sits between application and database. Cache handles DB fetch automatically.

**Difference from Cache-Aside**: Application only talks to cache.

**Pros**:
- ✅ Simpler application code
- ✅ Consistent caching logic

**Cons**:
- ❌ Tighter coupling to cache
- ❌ Cache failure = system failure

### 3. Write-Through

**Flow**:
1. Write to cache
2. Cache writes to DB synchronously
3. Return success

**Pros**:
- ✅ Cache always up-to-date
- ✅ Data consistency

**Cons**:
- ❌ Slower writes (synchronous)
- ❌ Writes to data never read

**When**: Data must be consistent immediately

### 4. Write-Behind (Write-Back)

**Flow**:
1. Write to cache
2. Return success immediately
3. Cache writes to DB asynchronously (batched)

**Pros**:
- ✅ Very fast writes
- ✅ Can batch writes for efficiency
- ✅ Reduces DB load

**Cons**:
- ❌ Risk of data loss if cache fails before sync
- ❌ Complexity

**When**: High write throughput, can tolerate some data loss

### 5. Write-Around

Write directly to DB, bypass cache. Remove from cache on write.

**When**: Data written is rarely read again

## Cache Eviction Policies

### LRU (Least Recently Used)
Evict item not accessed for longest time.
- **Best for**: General purpose
- **Implementation**: Linked list + hash map

### LFU (Least Frequently Used)
Evict item accessed least often.
- **Best for**: Long-term access patterns

### FIFO (First In, First Out)
Evict oldest item.
- **Best for**: Time-sensitive data

### TTL (Time To Live)
Items expire after time period.
- **Best for**: Data with known freshness

## Cache Invalidation

**Two hardest problems in CS**:
1. Cache invalidation
2. Naming things
3. Off-by-one errors

### Strategies:

#### TTL-Based
Set expiration time. Simple but may serve stale data.

#### Event-Based
Invalidate cache on DB write.
```csharp
public async Task UpdateUser(User user)
{
    await db.SaveAsync(user);
    await cache.RemoveAsync($""user:{user.Id}""); // Invalidate
}
```

#### Tag-Based
Cache items with tags, invalidate all items with tag.

## Real-World Example: Social Media Feed

```
User requests feed:
1. Check cache for feed (key: ""feed:user123"")
2. If hit: Return cached feed ✅
3. If miss:
   - Fetch posts from DB
   - Apply filters/ranking
   - Cache result (TTL: 5 minutes)
   - Return feed

On new post:
- Invalidate feed caches for followers
- Or use write-behind to update caches asynchronously
```

## Common Pitfalls

❌ **Caching everything**: Wastes memory
❌ **No expiration**: Stale data forever
❌ **Cache stampede**: Many requests miss cache simultaneously, all hit DB
  - **Solution**: Use locks or pre-warm cache

## Interview Tips

**Q: How do you prevent cache stampede?**
A:
1. **Locking**: First request fetches, others wait
2. **Pre-warming**: Populate cache before expiration
3. **Probabilistic early expiration**: Refresh before actual expiration

**Q: Redis vs Memcached?**
A:
- **Redis**: Data structures, persistence, pub/sub
- **Memcached**: Simple, multi-threaded, pure cache",
                Resources = "https://aws.amazon.com/caching/best-practices/",
                Tags = new() { "Caching", "Performance", "Redis" }
            },

            new() {
                Title = "Database Sharding - Horizontal Partitioning at Scale",
                Category = "Database",
                Difficulty = "Hard",
                KeyConcepts = "Partition Key, Consistent Hashing, Cross-Shard Queries",
                Lesson = @"# Database Sharding

## What is Sharding?

Splitting a large database into smaller, faster, more manageable pieces (shards). Each shard is an independent database.

## Why Shard?

**Problem**: Single database can't handle:
- Billions of rows
- Thousands of writes/second
- Terabytes of data

**Solution**: Distribute data across multiple databases.

## Sharding Strategies

### 1. Range-Based Sharding

Partition by value ranges (e.g., user ID 1-1M → Shard 1, 1M-2M → Shard 2).

**Pros**:
- ✅ Simple to implement
- ✅ Range queries work well

**Cons**:
- ❌ Uneven distribution (hotspots)
- ❌ Hard to rebalance

**Example**: Users A-M → Shard 1, N-Z → Shard 2

### 2. Hash-Based Sharding

Use hash function on shard key: `shard = hash(user_id) % num_shards`

**Pros**:
- ✅ Even distribution
- ✅ No hotspots

**Cons**:
- ❌ Adding shards requires rehashing all data
- ❌ Range queries impossible

### 3. Consistent Hashing

Maps data and nodes to ring. Adding/removing nodes only affects adjacent data.

**Pros**:
- ✅ Minimal data movement when scaling
- ✅ Even distribution with virtual nodes

**Cons**:
- ❌ More complex
- ❌ Range queries still hard

### 4. Directory-Based Sharding

Lookup table maps shard keys to shards.

**Pros**:
- ✅ Flexible (can change mapping)
- ✅ Easy to rebalance

**Cons**:
- ❌ Lookup table is single point of failure
- ❌ Extra hop for every query

## Choosing a Shard Key

**Critical decision!** Hard to change later.

### Good Shard Key Characteristics:

1. **High cardinality**: Many unique values
   - ✅ `user_id` (millions of users)
   - ❌ `country` (200 countries)

2. **Even distribution**: No hotspots
   - ✅ `user_id` (assuming even signup)
   - ❌ `created_date` (recent data gets all writes)

3. **Matches query patterns**
   - If you query by `user_id` → shard by `user_id`

### Bad Shard Key Examples:

- **Timestamp**: Recent data gets all writes (hot shard)
- **Status** (active/inactive): Uneven distribution
- **Geography**: Some countries have way more users

## Challenges

### 1. Cross-Shard Queries

**Problem**: `SELECT * FROM users WHERE age > 25`
Must query all shards and merge results.

**Solutions**:
- Denormalize data
- Use separate analytics database
- Accept slower queries

### 2. Joins Across Shards

**Problem**: Can't join tables on different shards.

**Solutions**:
- Denormalize (duplicate data)
- Application-level joins
- Co-locate related data (shard by same key)

### 3. Transactions Across Shards

**Problem**: ACID transactions don't work across shards.

**Solutions**:
- Two-phase commit (slow, complex)
- Sagas (eventual consistency)
- Avoid cross-shard transactions by design

### 4. Rebalancing

**Problem**: Adding shards requires moving data.

**Solutions**:
- Use consistent hashing
- Plan for oversharding (create more shards than needed initially)

## Real-World Example: Instagram

**Shard Key**: `user_id`
- Photos sharded by user_id
- All user's photos on same shard
- Fast user timeline queries
- Cross-user queries (global search) use separate system

## Implementation Example

```python
# Simple hash-based sharding
def get_shard(user_id, num_shards=4):
    shard_id = hash(user_id) % num_shards
    return f""shard_{shard_id}""

# Route query to correct shard
user_id = 12345
shard = get_shard(user_id)
connection = connect_to_shard(shard)
result = connection.query(f""SELECT * FROM users WHERE id = {user_id}"")
```

## Interview Questions

**Q: When do you introduce sharding?**
A:
1. Single DB hitting limits (CPU, RAM, disk)
2. Can't vertically scale more
3. Have well-defined access patterns

**Q: Difference between sharding and partitioning?**
A:
- **Partitioning**: Splitting table within single DB
- **Sharding**: Splitting across multiple DBs

**Q: How to avoid cross-shard queries?**
A:
1. Denormalize data
2. Choose shard key matching query patterns
3. Use separate system for analytics",
                Resources = "https://www.mongodb.com/features/database-sharding-explained",
                Tags = new() { "Sharding", "Database", "Scalability", "Partitioning" }
            }
        };
    }
}
