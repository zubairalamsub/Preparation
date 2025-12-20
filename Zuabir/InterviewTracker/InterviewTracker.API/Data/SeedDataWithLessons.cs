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
}
