# Design Patterns Complete Guide
## Common Design Patterns from Beginner to Advanced

---

## Table of Contents

### Part 1: Creational Patterns
1. [Singleton Pattern](#1-singleton-pattern)
2. [Factory Pattern](#2-factory-pattern)
3. [Abstract Factory Pattern](#3-abstract-factory-pattern)
4. [Builder Pattern](#4-builder-pattern)
5. [Prototype Pattern](#5-prototype-pattern)

### Part 2: Structural Patterns
6. [Adapter Pattern](#6-adapter-pattern)
7. [Decorator Pattern](#7-decorator-pattern)
8. [Facade Pattern](#8-facade-pattern)
9. [Proxy Pattern](#9-proxy-pattern)
10. [Composite Pattern](#10-composite-pattern)
11. [Bridge Pattern](#11-bridge-pattern)
12. [Flyweight Pattern](#12-flyweight-pattern)

### Part 3: Behavioral Patterns
13. [Strategy Pattern](#13-strategy-pattern)
14. [Observer Pattern](#14-observer-pattern)
15. [Command Pattern](#15-command-pattern)
16. [Chain of Responsibility](#16-chain-of-responsibility)
17. [Template Method](#17-template-method)
18. [Iterator Pattern](#18-iterator-pattern)
19. [Mediator Pattern](#19-mediator-pattern)
20. [Memento Pattern](#20-memento-pattern)
21. [State Pattern](#21-state-pattern)
22. [Visitor Pattern](#22-visitor-pattern)

### Part 4: Architectural Patterns
23. [Repository Pattern](#23-repository-pattern)
24. [Unit of Work Pattern](#24-unit-of-work-pattern)
25. [CQRS Pattern](#25-cqrs-pattern)
26. [MVC Pattern](#26-mvc-pattern)
27. [MVVM Pattern](#27-mvvm-pattern)
28. [Dependency Injection](#28-dependency-injection)
29. [Service Locator](#29-service-locator)

### Part 5: Modern Patterns
30. [Specification Pattern](#30-specification-pattern)
31. [Options Pattern](#31-options-pattern)
32. [Result Pattern](#32-result-pattern)
33. [Null Object Pattern](#33-null-object-pattern)

---

## PART 1: CREATIONAL PATTERNS

## 1. Singleton Pattern

**Intent**: Ensure a class has only one instance and provide global access to it.

**When to Use:**
- Need exactly one instance (e.g., configuration, logging)
- Global access point needed
- Lazy initialization desired

### Basic Implementation

```csharp
// Thread-safe Singleton (Lazy<T>)
public sealed class DatabaseConnection
{
    private static readonly Lazy<DatabaseConnection> _instance =
        new Lazy<DatabaseConnection>(() => new DatabaseConnection());

    private DatabaseConnection()
    {
        // Private constructor prevents external instantiation
        ConnectionString = "Server=localhost;Database=MyDb;";
    }

    public static DatabaseConnection Instance => _instance.Value;

    public string ConnectionString { get; private set; }

    public void Connect()
    {
        Console.WriteLine($"Connecting to: {ConnectionString}");
    }
}

// Usage
var db1 = DatabaseConnection.Instance;
var db2 = DatabaseConnection.Instance;
Console.WriteLine(db1 == db2); // True - same instance
```

### Thread-Safe Implementations

```csharp
// Method 1: Double-Check Locking
public sealed class Logger
{
    private static Logger _instance;
    private static readonly object _lock = new object();

    private Logger() { }

    public static Logger Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger();
                    }
                }
            }
            return _instance;
        }
    }

    public void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now}] {message}");
    }
}

// Method 2: Static Constructor (Recommended)
public sealed class Configuration
{
    private static readonly Configuration _instance = new Configuration();

    // Static constructor ensures thread safety
    static Configuration() { }

    private Configuration()
    {
        Settings = new Dictionary<string, string>();
    }

    public static Configuration Instance => _instance;

    public Dictionary<string, string> Settings { get; }
}

// Method 3: Lazy<T> (Best for most cases)
public sealed class CacheManager
{
    private static readonly Lazy<CacheManager> _lazy =
        new Lazy<CacheManager>(() => new CacheManager());

    private CacheManager()
    {
        Cache = new Dictionary<string, object>();
    }

    public static CacheManager Instance => _lazy.Value;

    public Dictionary<string, object> Cache { get; }
}
```

### Singleton with Dependency Injection

```csharp
// Register as Singleton in DI container
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Singleton - created once per application lifetime
        services.AddSingleton<IConfiguration, Configuration>();

        // Alternative: use instance
        services.AddSingleton<ILogger>(new Logger());
    }
}
```

### Anti-Pattern Warning

```csharp
// ❌ BAD: Singleton with mutable state
public class BadSingleton
{
    private static BadSingleton _instance = new BadSingleton();
    public static BadSingleton Instance => _instance;

    public int Counter { get; set; } // Mutable state - dangerous!
}

// ✅ GOOD: Singleton with immutable configuration
public class GoodSingleton
{
    private static readonly GoodSingleton _instance = new GoodSingleton();
    public static GoodSingleton Instance => _instance;

    public IReadOnlyDictionary<string, string> Settings { get; }

    private GoodSingleton()
    {
        Settings = LoadSettings();
    }
}
```

---

## 2. Factory Pattern

**Intent**: Define an interface for creating objects, but let subclasses decide which class to instantiate.

**When to Use:**
- Object creation is complex
- Client shouldn't know concrete classes
- Decoupling object creation from usage

### Simple Factory

```csharp
// Product
public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount);
}

public class CreditCardProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} via Credit Card");
    }
}

public class PayPalProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} via PayPal");
    }
}

public class CryptoProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing ${amount} via Cryptocurrency");
    }
}

// Simple Factory
public class PaymentProcessorFactory
{
    public static IPaymentProcessor Create(PaymentMethod method)
    {
        return method switch
        {
            PaymentMethod.CreditCard => new CreditCardProcessor(),
            PaymentMethod.PayPal => new PayPalProcessor(),
            PaymentMethod.Crypto => new CryptoProcessor(),
            _ => throw new ArgumentException("Invalid payment method")
        };
    }
}

// Usage
var processor = PaymentProcessorFactory.Create(PaymentMethod.CreditCard);
processor.ProcessPayment(100.00m);
```

### Factory Method Pattern

```csharp
// Abstract Creator
public abstract class DocumentCreator
{
    // Factory method
    public abstract IDocument CreateDocument();

    // Template method using factory method
    public void NewDocument()
    {
        var document = CreateDocument();
        document.Open();
        document.Save();
    }
}

// Product
public interface IDocument
{
    void Open();
    void Save();
}

// Concrete Products
public class PdfDocument : IDocument
{
    public void Open() => Console.WriteLine("Opening PDF document");
    public void Save() => Console.WriteLine("Saving PDF document");
}

public class WordDocument : IDocument
{
    public void Open() => Console.WriteLine("Opening Word document");
    public void Save() => Console.WriteLine("Saving Word document");
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
creator.NewDocument();
```

### Factory with Dependency Injection

```csharp
public interface INotificationFactory
{
    INotification Create(NotificationType type);
}

public class NotificationFactory : INotificationFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public INotification Create(NotificationType type)
    {
        return type switch
        {
            NotificationType.Email => _serviceProvider.GetRequiredService<EmailNotification>(),
            NotificationType.SMS => _serviceProvider.GetRequiredService<SmsNotification>(),
            NotificationType.Push => _serviceProvider.GetRequiredService<PushNotification>(),
            _ => throw new ArgumentException("Invalid notification type")
        };
    }
}

// Register in DI
services.AddTransient<EmailNotification>();
services.AddTransient<SmsNotification>();
services.AddTransient<PushNotification>();
services.AddSingleton<INotificationFactory, NotificationFactory>();
```

---

## 3. Abstract Factory Pattern

**Intent**: Provide an interface for creating families of related objects without specifying concrete classes.

**When to Use:**
- System needs to be independent of how objects are created
- System needs to work with multiple families of related products
- You want to enforce constraints on which products can work together

```csharp
// Abstract Products
public interface IButton
{
    void Render();
}

public interface ICheckbox
{
    void Render();
}

// Windows Products
public class WindowsButton : IButton
{
    public void Render() => Console.WriteLine("Rendering Windows button");
}

public class WindowsCheckbox : ICheckbox
{
    public void Render() => Console.WriteLine("Rendering Windows checkbox");
}

// Mac Products
public class MacButton : IButton
{
    public void Render() => Console.WriteLine("Rendering Mac button");
}

public class MacCheckbox : ICheckbox
{
    public void Render() => Console.WriteLine("Rendering Mac checkbox");
}

// Abstract Factory
public interface IUIFactory
{
    IButton CreateButton();
    ICheckbox CreateCheckbox();
}

// Concrete Factories
public class WindowsUIFactory : IUIFactory
{
    public IButton CreateButton() => new WindowsButton();
    public ICheckbox CreateCheckbox() => new WindowsCheckbox();
}

public class MacUIFactory : IUIFactory
{
    public IButton CreateButton() => new MacButton();
    public ICheckbox CreateCheckbox() => new MacCheckbox();
}

// Client
public class Application
{
    private readonly IUIFactory _factory;
    private IButton _button;
    private ICheckbox _checkbox;

    public Application(IUIFactory factory)
    {
        _factory = factory;
    }

    public void CreateUI()
    {
        _button = _factory.CreateButton();
        _checkbox = _factory.CreateCheckbox();
    }

    public void RenderUI()
    {
        _button.Render();
        _checkbox.Render();
    }
}

// Usage
IUIFactory factory = OperatingSystem.IsWindows()
    ? new WindowsUIFactory()
    : new MacUIFactory();

var app = new Application(factory);
app.CreateUI();
app.RenderUI();
```

### Real-World Example: Database Provider

```csharp
// Abstract Products
public interface IConnection
{
    void Open();
    void Close();
}

public interface ICommand
{
    void Execute(string sql);
}

// SQL Server Products
public class SqlConnection : IConnection
{
    public void Open() => Console.WriteLine("Opening SQL Server connection");
    public void Close() => Console.WriteLine("Closing SQL Server connection");
}

public class SqlCommand : ICommand
{
    public void Execute(string sql) => Console.WriteLine($"Executing SQL: {sql}");
}

// PostgreSQL Products
public class PostgresConnection : IConnection
{
    public void Open() => Console.WriteLine("Opening PostgreSQL connection");
    public void Close() => Console.WriteLine("Closing PostgreSQL connection");
}

public class PostgresCommand : ICommand
{
    public void Execute(string sql) => Console.WriteLine($"Executing PostgreSQL: {sql}");
}

// Abstract Factory
public interface IDatabaseFactory
{
    IConnection CreateConnection();
    ICommand CreateCommand();
}

// Concrete Factories
public class SqlServerFactory : IDatabaseFactory
{
    public IConnection CreateConnection() => new SqlConnection();
    public ICommand CreateCommand() => new SqlCommand();
}

public class PostgresFactory : IDatabaseFactory
{
    public IConnection CreateConnection() => new PostgresConnection();
    public ICommand CreateCommand() => new PostgresCommand();
}

// Usage
public class DataAccess
{
    private readonly IDatabaseFactory _factory;

    public DataAccess(IDatabaseFactory factory)
    {
        _factory = factory;
    }

    public void ExecuteQuery(string sql)
    {
        using var connection = _factory.CreateConnection();
        connection.Open();

        var command = _factory.CreateCommand();
        command.Execute(sql);

        connection.Close();
    }
}
```

---

## 4. Builder Pattern

**Intent**: Separate construction of complex object from its representation.

**When to Use:**
- Object construction is complex with many optional parameters
- Want to create different representations of an object
- Avoid telescoping constructor anti-pattern

### Classic Builder

```csharp
// Product
public class Pizza
{
    public string Dough { get; set; }
    public string Sauce { get; set; }
    public List<string> Toppings { get; set; } = new();
    public bool Cheese { get; set; }
    public PizzaSize Size { get; set; }

    public override string ToString()
    {
        return $"{Size} pizza with {Dough} dough, {Sauce} sauce, " +
               $"cheese: {Cheese}, toppings: {string.Join(", ", Toppings)}";
    }
}

// Builder
public class PizzaBuilder
{
    private readonly Pizza _pizza = new();

    public PizzaBuilder SetSize(PizzaSize size)
    {
        _pizza.Size = size;
        return this;
    }

    public PizzaBuilder SetDough(string dough)
    {
        _pizza.Dough = dough;
        return this;
    }

    public PizzaBuilder SetSauce(string sauce)
    {
        _pizza.Sauce = sauce;
        return this;
    }

    public PizzaBuilder AddCheese()
    {
        _pizza.Cheese = true;
        return this;
    }

    public PizzaBuilder AddTopping(string topping)
    {
        _pizza.Toppings.Add(topping);
        return this;
    }

    public Pizza Build()
    {
        // Validation
        if (string.IsNullOrEmpty(_pizza.Dough))
            throw new InvalidOperationException("Dough is required");

        return _pizza;
    }
}

// Usage
var pizza = new PizzaBuilder()
    .SetSize(PizzaSize.Large)
    .SetDough("Thin crust")
    .SetSauce("Tomato")
    .AddCheese()
    .AddTopping("Pepperoni")
    .AddTopping("Mushrooms")
    .Build();

Console.WriteLine(pizza);
```

### Fluent Builder with Director

```csharp
// Builder Interface
public interface ICarBuilder
{
    ICarBuilder SetEngine(string engine);
    ICarBuilder SetWheels(int wheels);
    ICarBuilder SetColor(string color);
    ICarBuilder AddFeature(string feature);
    Car Build();
}

// Concrete Builder
public class CarBuilder : ICarBuilder
{
    private readonly Car _car = new();

    public ICarBuilder SetEngine(string engine)
    {
        _car.Engine = engine;
        return this;
    }

    public ICarBuilder SetWheels(int wheels)
    {
        _car.Wheels = wheels;
        return this;
    }

    public ICarBuilder SetColor(string color)
    {
        _car.Color = color;
        return this;
    }

    public ICarBuilder AddFeature(string feature)
    {
        _car.Features.Add(feature);
        return this;
    }

    public Car Build() => _car;
}

// Director (optional)
public class CarDirector
{
    public Car ConstructSportsCar(ICarBuilder builder)
    {
        return builder
            .SetEngine("V8")
            .SetWheels(4)
            .SetColor("Red")
            .AddFeature("Turbo")
            .AddFeature("Sport seats")
            .Build();
    }

    public Car ConstructFamilyCar(ICarBuilder builder)
    {
        return builder
            .SetEngine("V4")
            .SetWheels(4)
            .SetColor("Blue")
            .AddFeature("Air conditioning")
            .AddFeature("Child seats")
            .Build();
    }
}

// Usage
var director = new CarDirector();
var sportsCar = director.ConstructSportsCar(new CarBuilder());
```

### Modern C# Builder (Records)

```csharp
// Using records and with expressions
public record User
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public int Age { get; init; }
    public string Address { get; init; }
}

public class UserBuilder
{
    private User _user = new User();

    public UserBuilder WithName(string firstName, string lastName)
    {
        _user = _user with { FirstName = firstName, LastName = lastName };
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _user = _user with { Email = email };
        return this;
    }

    public UserBuilder WithAge(int age)
    {
        _user = _user with { Age = age };
        return this;
    }

    public UserBuilder WithAddress(string address)
    {
        _user = _user with { Address = address };
        return this;
    }

    public User Build() => _user;
}

// Usage
var user = new UserBuilder()
    .WithName("John", "Doe")
    .WithEmail("john@example.com")
    .WithAge(30)
    .WithAddress("123 Main St")
    .Build();
```

---

## 5. Prototype Pattern

**Intent**: Create new objects by copying existing objects (cloning).

**When to Use:**
- Object creation is expensive
- Want to avoid subclasses of object creator
- Runtime specification of objects to create

```csharp
// Prototype Interface
public interface IPrototype<T>
{
    T Clone();
}

// Shallow Copy Example
public class Person : IPrototype<Person>
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }

    public Person Clone()
    {
        // Shallow copy - Address reference is shared
        return (Person)MemberwiseClone();
    }
}

// Deep Copy Example
public class Employee : IPrototype<Employee>
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }
    public List<string> Skills { get; set; } = new();

    public Employee Clone()
    {
        // Deep copy - new instances created
        return new Employee
        {
            Name = Name,
            Age = Age,
            Address = Address?.Clone(),
            Skills = new List<string>(Skills)
        };
    }
}

public class Address : IPrototype<Address>
{
    public string Street { get; set; }
    public string City { get; set; }

    public Address Clone()
    {
        return new Address
        {
            Street = Street,
            City = City
        };
    }
}

// Usage
var original = new Employee
{
    Name = "John",
    Age = 30,
    Address = new Address { Street = "123 Main", City = "NYC" },
    Skills = new List<string> { "C#", "SQL" }
};

var clone = original.Clone();
clone.Name = "Jane";
clone.Skills.Add("Azure");

Console.WriteLine(original.Name); // John
Console.WriteLine(clone.Name);    // Jane
Console.WriteLine(clone.Skills.Count); // 3
Console.WriteLine(original.Skills.Count); // 2 (deep copy worked)
```

### Prototype Registry

```csharp
public class ShapeCache
{
    private static readonly Dictionary<string, IShape> _shapeMap = new();

    public static void LoadCache()
    {
        var circle = new Circle { Radius = 5, Color = "Red" };
        _shapeMap.Add("circle", circle);

        var rectangle = new Rectangle { Width = 10, Height = 5, Color = "Blue" };
        _shapeMap.Add("rectangle", rectangle);
    }

    public static IShape GetShape(string shapeId)
    {
        var cachedShape = _shapeMap[shapeId];
        return cachedShape.Clone();
    }
}

// Usage
ShapeCache.LoadCache();

var circle1 = ShapeCache.GetShape("circle");
var circle2 = ShapeCache.GetShape("circle");

// Different instances but same initial state
Console.WriteLine(circle1 == circle2); // False
```

---

## PART 2: STRUCTURAL PATTERNS

## 6. Adapter Pattern

**Intent**: Convert interface of a class into another interface clients expect.

**When to Use:**
- Want to use existing class with incompatible interface
- Need to create reusable class that cooperates with unrelated classes
- Need to use several existing subclasses but impractical to adapt their interface by subclassing

```csharp
// Target Interface (what client expects)
public interface IMediaPlayer
{
    void Play(string audioType, string fileName);
}

// Adaptee (existing incompatible interface)
public class AdvancedMediaPlayer
{
    public void PlayVlc(string fileName)
    {
        Console.WriteLine($"Playing VLC file: {fileName}");
    }

    public void PlayMp4(string fileName)
    {
        Console.WriteLine($"Playing MP4 file: {fileName}");
    }
}

// Adapter
public class MediaAdapter : IMediaPlayer
{
    private readonly AdvancedMediaPlayer _advancedPlayer;

    public MediaAdapter(string audioType)
    {
        _advancedPlayer = new AdvancedMediaPlayer();
    }

    public void Play(string audioType, string fileName)
    {
        if (audioType.Equals("vlc", StringComparison.OrdinalIgnoreCase))
        {
            _advancedPlayer.PlayVlc(fileName);
        }
        else if (audioType.Equals("mp4", StringComparison.OrdinalIgnoreCase))
        {
            _advancedPlayer.PlayMp4(fileName);
        }
    }
}

// Client
public class AudioPlayer : IMediaPlayer
{
    private IMediaPlayer _mediaAdapter;

    public void Play(string audioType, string fileName)
    {
        // Built-in support for mp3
        if (audioType.Equals("mp3", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Playing MP3 file: {fileName}");
        }
        // Use adapter for other formats
        else if (audioType.Equals("vlc", StringComparison.OrdinalIgnoreCase) ||
                 audioType.Equals("mp4", StringComparison.OrdinalIgnoreCase))
        {
            _mediaAdapter = new MediaAdapter(audioType);
            _mediaAdapter.Play(audioType, fileName);
        }
        else
        {
            Console.WriteLine($"Invalid media type: {audioType}");
        }
    }
}

// Usage
var player = new AudioPlayer();
player.Play("mp3", "song.mp3");
player.Play("mp4", "video.mp4");
player.Play("vlc", "movie.vlc");
```

### Real-World Example: Third-Party Payment Gateway

```csharp
// Your system's payment interface
public interface IPaymentProcessor
{
    PaymentResult ProcessPayment(decimal amount, string currency);
}

// Third-party payment gateway (can't modify)
public class StripePaymentGateway
{
    public StripeResponse Charge(int amountInCents, string currencyCode, string token)
    {
        Console.WriteLine($"Stripe: Charging {amountInCents} cents in {currencyCode}");
        return new StripeResponse { Success = true, TransactionId = "stripe_" + Guid.NewGuid() };
    }
}

// Adapter
public class StripePaymentAdapter : IPaymentProcessor
{
    private readonly StripePaymentGateway _stripeGateway;

    public StripePaymentAdapter(StripePaymentGateway stripeGateway)
    {
        _stripeGateway = stripeGateway;
    }

    public PaymentResult ProcessPayment(decimal amount, string currency)
    {
        // Convert amount to cents
        int amountInCents = (int)(amount * 100);

        // Call third-party API
        var stripeResponse = _stripeGateway.Charge(amountInCents, currency, "token_xxx");

        // Convert response to our format
        return new PaymentResult
        {
            Success = stripeResponse.Success,
            TransactionId = stripeResponse.TransactionId,
            Amount = amount,
            Currency = currency
        };
    }
}

// Usage
IPaymentProcessor processor = new StripePaymentAdapter(new StripePaymentGateway());
var result = processor.ProcessPayment(100.50m, "USD");
```

---

## 7. Decorator Pattern

**Intent**: Attach additional responsibilities to an object dynamically.

**When to Use:**
- Add responsibilities to individual objects without affecting other objects
- Responsibilities can be withdrawn
- Extension by subclassing is impractical

```csharp
// Component
public interface ICoffee
{
    string GetDescription();
    decimal GetCost();
}

// Concrete Component
public class SimpleCoffee : ICoffee
{
    public string GetDescription() => "Simple coffee";
    public decimal GetCost() => 2.00m;
}

// Base Decorator
public abstract class CoffeeDecorator : ICoffee
{
    protected readonly ICoffee _coffee;

    protected CoffeeDecorator(ICoffee coffee)
    {
        _coffee = coffee;
    }

    public virtual string GetDescription() => _coffee.GetDescription();
    public virtual decimal GetCost() => _coffee.GetCost();
}

// Concrete Decorators
public class MilkDecorator : CoffeeDecorator
{
    public MilkDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => _coffee.GetDescription() + ", milk";
    public override decimal GetCost() => _coffee.GetCost() + 0.50m;
}

public class SugarDecorator : CoffeeDecorator
{
    public SugarDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => _coffee.GetDescription() + ", sugar";
    public override decimal GetCost() => _coffee.GetCost() + 0.25m;
}

public class WhipDecorator : CoffeeDecorator
{
    public WhipDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => _coffee.GetDescription() + ", whipped cream";
    public override decimal GetCost() => _coffee.GetCost() + 0.75m;
}

// Usage
ICoffee coffee = new SimpleCoffee();
Console.WriteLine($"{coffee.GetDescription()} = ${coffee.GetCost()}");
// Simple coffee = $2.00

coffee = new MilkDecorator(coffee);
Console.WriteLine($"{coffee.GetDescription()} = ${coffee.GetCost()}");
// Simple coffee, milk = $2.50

coffee = new SugarDecorator(coffee);
coffee = new WhipDecorator(coffee);
Console.WriteLine($"{coffee.GetDescription()} = ${coffee.GetCost()}");
// Simple coffee, milk, sugar, whipped cream = $3.50
```

### Real-World Example: Stream Decorators

```csharp
// ASP.NET Core uses decorator pattern for stream processing
public class LoggingStreamDecorator : Stream
{
    private readonly Stream _innerStream;
    private readonly ILogger _logger;

    public LoggingStreamDecorator(Stream innerStream, ILogger logger)
    {
        _innerStream = innerStream;
        _logger = logger;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        _logger.LogInformation($"Reading {count} bytes");
        var bytesRead = _innerStream.Read(buffer, offset, count);
        _logger.LogInformation($"Actually read {bytesRead} bytes");
        return bytesRead;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _logger.LogInformation($"Writing {count} bytes");
        _innerStream.Write(buffer, offset, count);
    }

    // Delegate all other members to inner stream
    public override bool CanRead => _innerStream.CanRead;
    public override bool CanSeek => _innerStream.CanSeek;
    public override bool CanWrite => _innerStream.CanWrite;
    public override long Length => _innerStream.Length;
    public override long Position
    {
        get => _innerStream.Position;
        set => _innerStream.Position = value;
    }

    public override void Flush() => _innerStream.Flush();
    public override long Seek(long offset, SeekOrigin origin) => _innerStream.Seek(offset, origin);
    public override void SetLength(long value) => _innerStream.SetLength(value);
}

// Usage
Stream stream = File.OpenRead("file.txt");
stream = new LoggingStreamDecorator(stream, logger);
stream = new BufferedStream(stream);
stream = new GZipStream(stream, CompressionMode.Decompress);
```

---

## 8. Facade Pattern

**Intent**: Provide unified interface to set of interfaces in subsystem.

**When to Use:**
- Want simple interface to complex subsystem
- Many dependencies between clients and implementation classes
- Want to layer your subsystems

```csharp
// Complex subsystem classes
public class VideoFile
{
    private string _name;
    public VideoFile(string name) => _name = name;
}

public class OggCompressionCodec { }
public class MPEG4CompressionCodec { }

public class CodecFactory
{
    public static object Extract(VideoFile file)
    {
        // Complex logic to determine codec
        return new MPEG4CompressionCodec();
    }
}

public class BitrateReader
{
    public static string Read(VideoFile file, object codec)
    {
        return "buffer data";
    }

    public static string Convert(string buffer, object codec)
    {
        return "converted buffer";
    }
}

public class AudioMixer
{
    public string Fix(string result)
    {
        return "fixed audio";
    }
}

// Facade - Simplified interface
public class VideoConverter
{
    public string Convert(string filename, string format)
    {
        Console.WriteLine("VideoConverter: conversion started.");

        var file = new VideoFile(filename);
        var sourceCodec = CodecFactory.Extract(file);

        object destinationCodec = format switch
        {
            "mp4" => new MPEG4CompressionCodec(),
            "ogg" => new OggCompressionCodec(),
            _ => throw new ArgumentException("Unsupported format")
        };

        var buffer = BitrateReader.Read(file, sourceCodec);
        var result = BitrateReader.Convert(buffer, destinationCodec);

        result = new AudioMixer().Fix(result);

        Console.WriteLine("VideoConverter: conversion completed.");
        return result;
    }
}

// Usage - Simple!
var converter = new VideoConverter();
var mp4 = converter.Convert("video.ogg", "mp4");
```

### Real-World Example: Order Processing Facade

```csharp
// Complex subsystems
public class InventoryService
{
    public bool CheckStock(int productId, int quantity)
    {
        Console.WriteLine($"Checking stock for product {productId}");
        return true;
    }

    public void ReserveStock(int productId, int quantity)
    {
        Console.WriteLine($"Reserving {quantity} units of product {productId}");
    }
}

public class PaymentService
{
    public string ProcessPayment(decimal amount, string cardNumber)
    {
        Console.WriteLine($"Processing payment of ${amount}");
        return "payment_" + Guid.NewGuid();
    }
}

public class ShippingService
{
    public string CreateShipment(string address)
    {
        Console.WriteLine($"Creating shipment to {address}");
        return "shipment_" + Guid.NewGuid();
    }
}

public class NotificationService
{
    public void SendOrderConfirmation(string email, string orderId)
    {
        Console.WriteLine($"Sending confirmation email to {email}");
    }
}

// Facade
public class OrderFacade
{
    private readonly InventoryService _inventoryService;
    private readonly PaymentService _paymentService;
    private readonly ShippingService _shippingService;
    private readonly NotificationService _notificationService;

    public OrderFacade()
    {
        _inventoryService = new InventoryService();
        _paymentService = new PaymentService();
        _shippingService = new ShippingService();
        _notificationService = new NotificationService();
    }

    public OrderResult PlaceOrder(OrderRequest request)
    {
        try
        {
            // Check inventory
            if (!_inventoryService.CheckStock(request.ProductId, request.Quantity))
                return OrderResult.Failed("Out of stock");

            // Reserve stock
            _inventoryService.ReserveStock(request.ProductId, request.Quantity);

            // Process payment
            var paymentId = _paymentService.ProcessPayment(
                request.TotalAmount,
                request.CardNumber);

            // Create shipment
            var shipmentId = _shippingService.CreateShipment(request.ShippingAddress);

            // Send notification
            _notificationService.SendOrderConfirmation(
                request.Email,
                $"order_{Guid.NewGuid()}");

            return OrderResult.Success(paymentId, shipmentId);
        }
        catch (Exception ex)
        {
            return OrderResult.Failed(ex.Message);
        }
    }
}

// Usage - Complex process simplified!
var orderFacade = new OrderFacade();
var result = orderFacade.PlaceOrder(new OrderRequest
{
    ProductId = 123,
    Quantity = 2,
    TotalAmount = 99.99m,
    CardNumber = "1234-5678-9012-3456",
    ShippingAddress = "123 Main St",
    Email = "customer@example.com"
});
```

---

## 9. Proxy Pattern

**Intent**: Provide surrogate or placeholder for another object to control access to it.

**When to Use:**
- Lazy initialization (virtual proxy)
- Access control (protection proxy)
- Local representation of remote object (remote proxy)
- Logging, caching (smart reference)

### Virtual Proxy (Lazy Loading)

```csharp
// Subject
public interface IImage
{
    void Display();
}

// Real Subject (expensive to create)
public class RealImage : IImage
{
    private readonly string _filename;

    public RealImage(string filename)
    {
        _filename = filename;
        LoadFromDisk();
    }

    private void LoadFromDisk()
    {
        Console.WriteLine($"Loading image from disk: {_filename}");
        Thread.Sleep(2000); // Simulate expensive operation
    }

    public void Display()
    {
        Console.WriteLine($"Displaying image: {_filename}");
    }
}

// Proxy (lazy loading)
public class ImageProxy : IImage
{
    private readonly string _filename;
    private RealImage _realImage;

    public ImageProxy(string filename)
    {
        _filename = filename;
    }

    public void Display()
    {
        // Load image only when needed
        if (_realImage == null)
        {
            _realImage = new RealImage(_filename);
        }
        _realImage.Display();
    }
}

// Usage
IImage image = new ImageProxy("photo.jpg");
Console.WriteLine("Image created (not loaded yet)");

// Image loaded only when displayed
image.Display(); // Loads from disk
image.Display(); // Already loaded, just displays
```

### Protection Proxy (Access Control)

```csharp
public interface IDocument
{
    void View();
    void Edit(string content);
    void Delete();
}

public class Document : IDocument
{
    private string _content;

    public void View()
    {
        Console.WriteLine($"Viewing document: {_content}");
    }

    public void Edit(string content)
    {
        _content = content;
        Console.WriteLine("Document edited");
    }

    public void Delete()
    {
        Console.WriteLine("Document deleted");
    }
}

public class DocumentProxy : IDocument
{
    private readonly Document _document;
    private readonly string _userRole;

    public DocumentProxy(string userRole)
    {
        _document = new Document();
        _userRole = userRole;
    }

    public void View()
    {
        // Everyone can view
        _document.View();
    }

    public void Edit(string content)
    {
        if (_userRole == "Admin" || _userRole == "Editor")
        {
            _document.Edit(content);
        }
        else
        {
            Console.WriteLine("Access denied: Insufficient permissions to edit");
        }
    }

    public void Delete()
    {
        if (_userRole == "Admin")
        {
            _document.Delete();
        }
        else
        {
            Console.WriteLine("Access denied: Only admins can delete");
        }
    }
}

// Usage
IDocument adminDoc = new DocumentProxy("Admin");
adminDoc.Edit("New content");
adminDoc.Delete();

IDocument viewerDoc = new DocumentProxy("Viewer");
viewerDoc.View();
viewerDoc.Edit("Try to edit"); // Access denied
viewerDoc.Delete(); // Access denied
```

### Caching Proxy

```csharp
public interface IDataService
{
    string GetData(int id);
}

public class RealDataService : IDataService
{
    public string GetData(int id)
    {
        Console.WriteLine($"Fetching data from database for ID: {id}");
        Thread.Sleep(1000); // Simulate slow database query
        return $"Data for ID {id}";
    }
}

public class CachingProxy : IDataService
{
    private readonly RealDataService _realService;
    private readonly Dictionary<int, string> _cache;

    public CachingProxy()
    {
        _realService = new RealDataService();
        _cache = new Dictionary<int, string>();
    }

    public string GetData(int id)
    {
        if (_cache.ContainsKey(id))
        {
            Console.WriteLine($"Returning cached data for ID: {id}");
            return _cache[id];
        }

        var data = _realService.GetData(id);
        _cache[id] = data;
        return data;
    }
}

// Usage
IDataService service = new CachingProxy();
service.GetData(1); // Fetches from database
service.GetData(1); // Returns from cache
service.GetData(2); // Fetches from database
```

---

**[Continuing in next part with remaining patterns...]**

## Quick Pattern Reference

### When to Use Each Pattern

**Creational:**
- **Singleton**: One instance needed globally
- **Factory**: Hide creation logic, many variants
- **Abstract Factory**: Families of related objects
- **Builder**: Complex object construction
- **Prototype**: Expensive object creation

**Structural:**
- **Adapter**: Make incompatible interfaces work together
- **Decorator**: Add responsibilities dynamically
- **Facade**: Simplify complex subsystem
- **Proxy**: Control access, lazy loading, caching

**Behavioral:**
- **Strategy**: Interchangeable algorithms
- **Observer**: One-to-many notifications
- **Command**: Encapsulate requests
- **Template Method**: Algorithm skeleton with steps
- **State**: Object behavior changes with state

### Most Common in .NET Development
1. Dependency Injection
2. Repository Pattern
3. Factory Pattern
4. Strategy Pattern
5. Observer Pattern (Events/Delegates)
6. Decorator Pattern
7. Singleton Pattern
8. Builder Pattern
