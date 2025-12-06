# Object-Oriented Programming Complete Guide
## From Fundamentals to Advanced Concepts

---

## Table of Contents

### Part 1: OOP Fundamentals
1. [What is OOP?](#1-what-is-oop)
2. [Classes and Objects](#2-classes-and-objects)
3. [Encapsulation](#3-encapsulation)
4. [Abstraction](#4-abstraction)
5. [Inheritance](#5-inheritance)
6. [Polymorphism](#6-polymorphism)

### Part 2: SOLID Principles
7. [Single Responsibility Principle](#7-single-responsibility-principle-srp)
8. [Open/Closed Principle](#8-openclosed-principle-ocp)
9. [Liskov Substitution Principle](#9-liskov-substitution-principle-lsp)
10. [Interface Segregation Principle](#10-interface-segregation-principle-isp)
11. [Dependency Inversion Principle](#11-dependency-inversion-principle-dip)

### Part 3: Advanced OOP Concepts
12. [Abstract Classes vs Interfaces](#12-abstract-classes-vs-interfaces)
13. [Composition vs Inheritance](#13-composition-vs-inheritance)
14. [Method Overloading and Overriding](#14-method-overloading-and-overriding)
15. [Access Modifiers](#15-access-modifiers)
16. [Static Members](#16-static-members)
17. [Properties and Indexers](#17-properties-and-indexers)

### Part 4: C# Specific Features
18. [Extension Methods](#18-extension-methods)
19. [Partial Classes](#19-partial-classes)
20. [Sealed Classes](#20-sealed-classes)
21. [Nested Classes](#21-nested-classes)
22. [Anonymous Types](#22-anonymous-types)
23. [Records (C# 9+)](#23-records-c-9)

### Part 5: Advanced Topics
24. [Generics](#24-generics)
25. [Delegates and Events](#25-delegates-and-events)
26. [Covariance and Contravariance](#26-covariance-and-contravariance)
27. [Reflection](#27-reflection)
28. [Object Lifecycle](#28-object-lifecycle)

### Part 6: Best Practices & Anti-Patterns
29. [Common OOP Anti-Patterns](#29-common-oop-anti-patterns)
30. [Best Practices](#30-best-practices)

---

## PART 1: OOP FUNDAMENTALS

## 1. What is OOP?

**Definition**: Object-Oriented Programming is a programming paradigm based on the concept of "objects" which contain data (fields) and code (methods).

**Four Main Pillars:**
1. **Encapsulation**: Bundling data and methods that work on that data
2. **Abstraction**: Hiding complex implementation details
3. **Inheritance**: Creating new classes from existing ones
4. **Polymorphism**: Objects taking many forms

### Why OOP?

**Benefits:**
- Code reusability through inheritance
- Modularity through encapsulation
- Flexibility through polymorphism
- Easier maintenance and updates
- Real-world modeling

```csharp
// Procedural approach (without OOP)
string customerName = "John Doe";
string customerEmail = "john@example.com";
decimal customerBalance = 1000m;

void ProcessPayment(decimal amount)
{
    if (customerBalance >= amount)
    {
        customerBalance -= amount;
        Console.WriteLine($"Payment processed: ${amount}");
    }
}

// OOP approach
public class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    private decimal _balance;

    public Customer(string name, string email, decimal balance)
    {
        Name = name;
        Email = email;
        _balance = balance;
    }

    public bool ProcessPayment(decimal amount)
    {
        if (_balance >= amount)
        {
            _balance -= amount;
            Console.WriteLine($"{Name} payment processed: ${amount}");
            return true;
        }
        return false;
    }

    public decimal GetBalance() => _balance;
}

// Usage
var customer = new Customer("John Doe", "john@example.com", 1000m);
customer.ProcessPayment(50m);
```

---

## 2. Classes and Objects

**Class**: Blueprint or template for creating objects
**Object**: Instance of a class

### Basic Class Structure

```csharp
public class Car
{
    // Fields (data members)
    private string _make;
    private string _model;
    private int _year;
    private decimal _price;

    // Constructor
    public Car(string make, string model, int year, decimal price)
    {
        _make = make;
        _model = model;
        _year = year;
        _price = price;
    }

    // Properties
    public string Make
    {
        get { return _make; }
        set { _make = value; }
    }

    public string Model
    {
        get { return _model; }
        set { _model = value; }
    }

    // Auto-implemented properties
    public int Mileage { get; set; }
    public string Color { get; set; }

    // Methods
    public void StartEngine()
    {
        Console.WriteLine($"{_make} {_model} engine started!");
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"{_year} {_make} {_model} - ${_price:N2}");
    }

    // Method with return value
    public decimal CalculateDepreciation(int years)
    {
        decimal depreciationRate = 0.15m;
        return _price * (decimal)Math.Pow(1 - (double)depreciationRate, years);
    }
}

// Creating objects
var myCar = new Car("Toyota", "Camry", 2022, 25000m);
myCar.Color = "Blue";
myCar.Mileage = 5000;

myCar.StartEngine();
myCar.DisplayInfo();

decimal valueAfter3Years = myCar.CalculateDepreciation(3);
Console.WriteLine($"Value after 3 years: ${valueAfter3Years:N2}");
```

### Multiple Constructors (Constructor Overloading)

```csharp
public class Employee
{
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Salary { get; set; }
    public string Department { get; set; }

    // Default constructor
    public Employee()
    {
        Department = "General";
        Salary = 30000m;
    }

    // Constructor with name
    public Employee(string name) : this()
    {
        Name = name;
    }

    // Constructor with name and email
    public Employee(string name, string email) : this(name)
    {
        Email = email;
    }

    // Full constructor
    public Employee(string name, string email, decimal salary, string department)
    {
        Name = name;
        Email = email;
        Salary = salary;
        Department = department;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"{Name} - {Department} - ${Salary:N2}");
    }
}

// Usage
var emp1 = new Employee();
var emp2 = new Employee("John Doe");
var emp3 = new Employee("Jane Smith", "jane@company.com");
var emp4 = new Employee("Bob Wilson", "bob@company.com", 75000m, "IT");
```

### Object Initializers

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public bool InStock { get; set; }
}

// Object initializer syntax
var product = new Product
{
    Id = 1,
    Name = "Laptop",
    Price = 999.99m,
    Category = "Electronics",
    InStock = true
};

// Collection initializer
var products = new List<Product>
{
    new Product { Id = 1, Name = "Laptop", Price = 999.99m },
    new Product { Id = 2, Name = "Mouse", Price = 29.99m },
    new Product { Id = 3, Name = "Keyboard", Price = 79.99m }
};
```

---

## 3. Encapsulation

**Definition**: Bundling data (fields) and methods that operate on that data within a single unit (class), and restricting access to internal details.

**Purpose:**
- Hide implementation details
- Protect data integrity
- Provide controlled access through properties/methods

### Example Without Encapsulation (Bad)

```csharp
// ❌ BAD: Direct field access
public class BankAccount
{
    public decimal balance; // Anyone can modify this!
}

// Problems:
var account = new BankAccount();
account.balance = -1000m; // Invalid balance allowed!
account.balance = 999999999m; // Direct manipulation possible
```

### Example With Encapsulation (Good)

```csharp
// ✅ GOOD: Encapsulated with validation
public class BankAccount
{
    private decimal _balance; // Private field
    private string _accountNumber;
    private string _accountHolderName;

    public BankAccount(string accountNumber, string accountHolderName)
    {
        _accountNumber = accountNumber;
        _accountHolderName = accountHolderName;
        _balance = 0m;
    }

    // Read-only property
    public decimal Balance => _balance;

    public string AccountNumber => _accountNumber;

    // Controlled method to modify balance
    public bool Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Deposit amount must be positive");
            return false;
        }

        _balance += amount;
        Console.WriteLine($"Deposited ${amount:N2}. New balance: ${_balance:N2}");
        return true;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Withdrawal amount must be positive");
            return false;
        }

        if (amount > _balance)
        {
            Console.WriteLine("Insufficient funds");
            return false;
        }

        _balance -= amount;
        Console.WriteLine($"Withdrew ${amount:N2}. New balance: ${_balance:N2}");
        return true;
    }

    public void DisplayAccountInfo()
    {
        Console.WriteLine($"Account: {_accountNumber}");
        Console.WriteLine($"Holder: {_accountHolderName}");
        Console.WriteLine($"Balance: ${_balance:N2}");
    }
}

// Usage
var account = new BankAccount("123456789", "John Doe");
account.Deposit(1000m);
account.Withdraw(500m);
// account._balance = -100m; // ❌ Compilation error - field is private
Console.WriteLine($"Current Balance: ${account.Balance}"); // ✅ Read-only access
```

### Property Encapsulation

```csharp
public class Person
{
    private string _name;
    private int _age;
    private string _email;

    // Property with validation
    public string Name
    {
        get { return _name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty");
            _name = value;
        }
    }

    // Property with validation
    public int Age
    {
        get { return _age; }
        set
        {
            if (value < 0 || value > 150)
                throw new ArgumentException("Age must be between 0 and 150");
            _age = value;
        }
    }

    // Property with validation
    public string Email
    {
        get { return _email; }
        set
        {
            if (!value.Contains("@"))
                throw new ArgumentException("Invalid email format");
            _email = value.ToLower();
        }
    }

    // Computed property
    public bool IsAdult => _age >= 18;

    // Read-only property
    public string DisplayName => $"{_name} ({_age} years old)";
}

// Usage
var person = new Person
{
    Name = "John Doe",
    Age = 30,
    Email = "John@Example.COM" // Will be converted to lowercase
};

Console.WriteLine(person.IsAdult); // True
Console.WriteLine(person.DisplayName); // John Doe (30 years old)
```

---

## 4. Abstraction

**Definition**: Hiding complex implementation details and showing only essential features.

**Purpose:**
- Reduce complexity
- Increase code reusability
- Focus on what an object does, not how it does it

### Abstraction with Abstract Classes

```csharp
// Abstract class - cannot be instantiated
public abstract class Shape
{
    public string Name { get; set; }
    public string Color { get; set; }

    // Abstract method - must be implemented by derived classes
    public abstract double CalculateArea();
    public abstract double CalculatePerimeter();

    // Concrete method - can be used by all derived classes
    public void DisplayInfo()
    {
        Console.WriteLine($"Shape: {Name}");
        Console.WriteLine($"Color: {Color}");
        Console.WriteLine($"Area: {CalculateArea():F2}");
        Console.WriteLine($"Perimeter: {CalculatePerimeter():F2}");
    }
}

public class Circle : Shape
{
    public double Radius { get; set; }

    public Circle(double radius, string color)
    {
        Radius = radius;
        Color = color;
        Name = "Circle";
    }

    public override double CalculateArea()
    {
        return Math.PI * Radius * Radius;
    }

    public override double CalculatePerimeter()
    {
        return 2 * Math.PI * Radius;
    }
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height, string color)
    {
        Width = width;
        Height = height;
        Color = color;
        Name = "Rectangle";
    }

    public override double CalculateArea()
    {
        return Width * Height;
    }

    public override double CalculatePerimeter()
    {
        return 2 * (Width + Height);
    }
}

// Usage
Shape circle = new Circle(5, "Red");
circle.DisplayInfo();

Shape rectangle = new Rectangle(4, 6, "Blue");
rectangle.DisplayInfo();

// Shape shape = new Shape(); // ❌ Error: Cannot create instance of abstract class
```

### Abstraction with Interfaces

```csharp
// Interface - contract that defines behavior
public interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount);
    bool RefundPayment(string transactionId);
    string GetPaymentStatus(string transactionId);
}

public interface IPaymentValidation
{
    bool ValidatePaymentMethod();
}

// Implementing interfaces
public class CreditCardProcessor : IPaymentProcessor, IPaymentValidation
{
    private string _cardNumber;
    private string _cvv;
    private DateTime _expiryDate;

    public CreditCardProcessor(string cardNumber, string cvv, DateTime expiryDate)
    {
        _cardNumber = cardNumber;
        _cvv = cvv;
        _expiryDate = expiryDate;
    }

    public bool ValidatePaymentMethod()
    {
        if (string.IsNullOrEmpty(_cardNumber) || _cardNumber.Length != 16)
            return false;

        if (_expiryDate < DateTime.Now)
            return false;

        return true;
    }

    public bool ProcessPayment(decimal amount)
    {
        if (!ValidatePaymentMethod())
        {
            Console.WriteLine("Invalid credit card");
            return false;
        }

        Console.WriteLine($"Processing ${amount} via Credit Card ending in {_cardNumber.Substring(12)}");
        // Implementation details hidden
        return true;
    }

    public bool RefundPayment(string transactionId)
    {
        Console.WriteLine($"Refunding transaction: {transactionId}");
        return true;
    }

    public string GetPaymentStatus(string transactionId)
    {
        return "Completed";
    }
}

public class PayPalProcessor : IPaymentProcessor, IPaymentValidation
{
    private string _email;
    private string _password;

    public PayPalProcessor(string email, string password)
    {
        _email = email;
        _password = password;
    }

    public bool ValidatePaymentMethod()
    {
        return !string.IsNullOrEmpty(_email) && _email.Contains("@");
    }

    public bool ProcessPayment(decimal amount)
    {
        if (!ValidatePaymentMethod())
        {
            Console.WriteLine("Invalid PayPal account");
            return false;
        }

        Console.WriteLine($"Processing ${amount} via PayPal ({_email})");
        return true;
    }

    public bool RefundPayment(string transactionId)
    {
        Console.WriteLine($"Refunding PayPal transaction: {transactionId}");
        return true;
    }

    public string GetPaymentStatus(string transactionId)
    {
        return "Pending";
    }
}

// Client code - works with abstraction
public class PaymentService
{
    public void ProcessOrder(IPaymentProcessor processor, decimal amount)
    {
        // We don't care about implementation details
        if (processor.ProcessPayment(amount))
        {
            Console.WriteLine("Payment successful!");
        }
        else
        {
            Console.WriteLine("Payment failed!");
        }
    }
}

// Usage - implementation details are abstracted away
var paymentService = new PaymentService();

IPaymentProcessor creditCard = new CreditCardProcessor(
    "1234567890123456", "123", DateTime.Now.AddYears(2));
paymentService.ProcessOrder(creditCard, 99.99m);

IPaymentProcessor paypal = new PayPalProcessor("user@example.com", "password");
paymentService.ProcessOrder(paypal, 99.99m);
```

---

## 5. Inheritance

**Definition**: Mechanism where a new class (derived/child) is created from an existing class (base/parent), inheriting its properties and methods.

**Purpose:**
- Code reusability
- Establish IS-A relationship
- Method overriding for polymorphism

### Basic Inheritance

```csharp
// Base class
public class Animal
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Species { get; set; }

    public virtual void MakeSound()
    {
        Console.WriteLine("Some generic animal sound");
    }

    public void Sleep()
    {
        Console.WriteLine($"{Name} is sleeping");
    }

    public void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }
}

// Derived class
public class Dog : Animal
{
    public string Breed { get; set; }

    public Dog()
    {
        Species = "Canine";
    }

    // Override base method
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says: Woof! Woof!");
    }

    // New method specific to Dog
    public void Fetch()
    {
        Console.WriteLine($"{Name} is fetching the ball");
    }
}

public class Cat : Animal
{
    public bool IsIndoor { get; set; }

    public Cat()
    {
        Species = "Feline";
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says: Meow!");
    }

    public void Scratch()
    {
        Console.WriteLine($"{Name} is scratching the furniture");
    }
}

// Usage
Dog dog = new Dog { Name = "Max", Age = 3, Breed = "Golden Retriever" };
dog.MakeSound(); // Max says: Woof! Woof!
dog.Eat();       // Max is eating (inherited method)
dog.Fetch();     // Max is fetching the ball (Dog-specific method)

Cat cat = new Cat { Name = "Whiskers", Age = 2, IsIndoor = true };
cat.MakeSound(); // Whiskers says: Meow!
cat.Sleep();     // Whiskers is sleeping (inherited method)
cat.Scratch();   // Whiskers is scratching the furniture
```

### Multi-Level Inheritance

```csharp
// Base class
public class Vehicle
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }

    public virtual void Start()
    {
        Console.WriteLine($"{Brand} {Model} is starting");
    }

    public void Stop()
    {
        Console.WriteLine($"{Brand} {Model} is stopping");
    }
}

// Intermediate derived class
public class Car : Vehicle
{
    public int NumberOfDoors { get; set; }
    public string FuelType { get; set; }

    public override void Start()
    {
        Console.WriteLine("Checking fuel level...");
        base.Start(); // Call parent method
    }

    public void OpenTrunk()
    {
        Console.WriteLine("Trunk is open");
    }
}

// Further derived class
public class ElectricCar : Car
{
    public int BatteryCapacity { get; set; }
    public int Range { get; set; }

    public ElectricCar()
    {
        FuelType = "Electric";
    }

    public override void Start()
    {
        Console.WriteLine($"Battery level: {BatteryCapacity}%");
        base.Start(); // Call Car's Start method
    }

    public void Charge()
    {
        Console.WriteLine($"Charging {Brand} {Model}...");
        BatteryCapacity = 100;
        Console.WriteLine("Fully charged!");
    }
}

// Usage
ElectricCar tesla = new ElectricCar
{
    Brand = "Tesla",
    Model = "Model 3",
    Year = 2023,
    NumberOfDoors = 4,
    BatteryCapacity = 80,
    Range = 350
};

tesla.Start();    // Uses ElectricCar's Start, which calls Car's Start, which calls Vehicle's Start
tesla.Charge();   // ElectricCar-specific method
tesla.OpenTrunk(); // Inherited from Car
tesla.Stop();      // Inherited from Vehicle
```

### Protected Access Modifier

```csharp
public class BankAccount
{
    protected decimal balance; // Accessible in derived classes
    private string accountNumber; // Only accessible in this class

    public BankAccount(string accountNumber, decimal initialBalance)
    {
        this.accountNumber = accountNumber;
        this.balance = initialBalance;
    }

    public virtual void Deposit(decimal amount)
    {
        balance += amount;
        Console.WriteLine($"Deposited: ${amount:N2}. Balance: ${balance:N2}");
    }

    public virtual bool Withdraw(decimal amount)
    {
        if (balance >= amount)
        {
            balance -= amount;
            Console.WriteLine($"Withdrew: ${amount:N2}. Balance: ${balance:N2}");
            return true;
        }
        return false;
    }

    public decimal GetBalance() => balance;
}

public class SavingsAccount : BankAccount
{
    private decimal interestRate;

    public SavingsAccount(string accountNumber, decimal initialBalance, decimal interestRate)
        : base(accountNumber, initialBalance)
    {
        this.interestRate = interestRate;
    }

    public void ApplyInterest()
    {
        // Can access 'balance' because it's protected
        decimal interest = balance * interestRate;
        balance += interest;
        Console.WriteLine($"Interest applied: ${interest:N2}. New balance: ${balance:N2}");
    }

    // Override withdraw with additional restriction
    public override bool Withdraw(decimal amount)
    {
        // Minimum balance requirement for savings
        if (balance - amount < 100m)
        {
            Console.WriteLine("Cannot withdraw: Minimum balance of $100 required");
            return false;
        }
        return base.Withdraw(amount);
    }
}

// Usage
SavingsAccount savings = new SavingsAccount("SAV-123", 1000m, 0.05m);
savings.Deposit(500m);
savings.ApplyInterest(); // Can access protected balance field
savings.Withdraw(200m);
savings.Withdraw(1500m); // Fails due to minimum balance requirement
```

### Base Keyword

```csharp
public class Employee
{
    public string Name { get; set; }
    public decimal BaseSalary { get; set; }

    public Employee(string name, decimal baseSalary)
    {
        Name = name;
        BaseSalary = baseSalary;
    }

    public virtual decimal CalculateSalary()
    {
        return BaseSalary;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Salary: ${CalculateSalary():N2}");
    }
}

public class Manager : Employee
{
    public decimal Bonus { get; set; }
    public int TeamSize { get; set; }

    // Constructor calls base constructor
    public Manager(string name, decimal baseSalary, decimal bonus, int teamSize)
        : base(name, baseSalary) // Call base constructor
    {
        Bonus = bonus;
        TeamSize = teamSize;
    }

    // Override and extend base method
    public override decimal CalculateSalary()
    {
        decimal baseSalary = base.CalculateSalary(); // Call base implementation
        return baseSalary + Bonus + (TeamSize * 100); // Add manager-specific calculation
    }

    // Override and extend base method
    public override void DisplayInfo()
    {
        base.DisplayInfo(); // Call base implementation
        Console.WriteLine($"Bonus: ${Bonus:N2}");
        Console.WriteLine($"Team Size: {TeamSize}");
        Console.WriteLine($"Total Compensation: ${CalculateSalary():N2}");
    }
}

// Usage
Manager manager = new Manager("Alice Johnson", 80000m, 15000m, 5);
manager.DisplayInfo();
```

---

## 6. Polymorphism

**Definition**: Ability of objects to take many forms. Same interface, different implementations.

**Types:**
1. **Compile-time (Static) Polymorphism**: Method overloading, Operator overloading
2. **Runtime (Dynamic) Polymorphism**: Method overriding through inheritance

### Method Overloading (Compile-time Polymorphism)

```csharp
public class Calculator
{
    // Same method name, different parameters
    public int Add(int a, int b)
    {
        Console.WriteLine("Adding two integers");
        return a + b;
    }

    public double Add(double a, double b)
    {
        Console.WriteLine("Adding two doubles");
        return a + b;
    }

    public int Add(int a, int b, int c)
    {
        Console.WriteLine("Adding three integers");
        return a + b + c;
    }

    public string Add(string a, string b)
    {
        Console.WriteLine("Concatenating two strings");
        return a + b;
    }

    // Different parameter order
    public void Display(string name, int age)
    {
        Console.WriteLine($"Name: {name}, Age: {age}");
    }

    public void Display(int age, string name)
    {
        Console.WriteLine($"Age: {age}, Name: {name}");
    }

    // Optional parameters
    public int Multiply(int a, int b, int c = 1)
    {
        return a * b * c;
    }

    // Params keyword
    public int Sum(params int[] numbers)
    {
        Console.WriteLine($"Summing {numbers.Length} numbers");
        return numbers.Sum();
    }
}

// Usage
Calculator calc = new Calculator();
Console.WriteLine(calc.Add(5, 10));           // Calls Add(int, int)
Console.WriteLine(calc.Add(5.5, 10.5));       // Calls Add(double, double)
Console.WriteLine(calc.Add(1, 2, 3));         // Calls Add(int, int, int)
Console.WriteLine(calc.Add("Hello", " World")); // Calls Add(string, string)

calc.Display("John", 25);                     // Calls Display(string, int)
calc.Display(25, "John");                     // Calls Display(int, string)

Console.WriteLine(calc.Multiply(2, 3));       // c defaults to 1
Console.WriteLine(calc.Multiply(2, 3, 4));    // All parameters provided

Console.WriteLine(calc.Sum(1, 2, 3, 4, 5));   // Params array
```

### Method Overriding (Runtime Polymorphism)

```csharp
public class Payment
{
    public string TransactionId { get; set; }
    public decimal Amount { get; set; }

    public virtual void ProcessPayment()
    {
        Console.WriteLine($"Processing payment of ${Amount:N2}");
    }

    public virtual void GenerateReceipt()
    {
        Console.WriteLine($"Receipt for transaction: {TransactionId}");
    }
}

public class CreditCardPayment : Payment
{
    public string CardNumber { get; set; }

    public override void ProcessPayment()
    {
        Console.WriteLine($"Processing credit card payment: ${Amount:N2}");
        Console.WriteLine($"Card: ****{CardNumber.Substring(12)}");
        // Credit card specific processing
    }

    public override void GenerateReceipt()
    {
        base.GenerateReceipt(); // Call base implementation
        Console.WriteLine($"Card used: ****{CardNumber.Substring(12)}");
    }
}

public class PayPalPayment : Payment
{
    public string Email { get; set; }

    public override void ProcessPayment()
    {
        Console.WriteLine($"Processing PayPal payment: ${Amount:N2}");
        Console.WriteLine($"PayPal account: {Email}");
        // PayPal specific processing
    }

    public override void GenerateReceipt()
    {
        base.GenerateReceipt();
        Console.WriteLine($"PayPal account: {Email}");
    }
}

public class CryptoPayment : Payment
{
    public string WalletAddress { get; set; }
    public string Cryptocurrency { get; set; }

    public override void ProcessPayment()
    {
        Console.WriteLine($"Processing {Cryptocurrency} payment: ${Amount:N2}");
        Console.WriteLine($"Wallet: {WalletAddress}");
        // Cryptocurrency specific processing
    }

    public override void GenerateReceipt()
    {
        base.GenerateReceipt();
        Console.WriteLine($"Cryptocurrency: {Cryptocurrency}");
        Console.WriteLine($"Wallet: {WalletAddress}");
    }
}

// Polymorphic behavior
public class PaymentProcessor
{
    public void Process(Payment payment) // Accepts any Payment type
    {
        payment.ProcessPayment(); // Calls appropriate overridden method
        payment.GenerateReceipt();
        Console.WriteLine(new string('-', 40));
    }
}

// Usage - Polymorphism in action
PaymentProcessor processor = new PaymentProcessor();

Payment[] payments = new Payment[]
{
    new CreditCardPayment
    {
        TransactionId = "CC-001",
        Amount = 99.99m,
        CardNumber = "1234567890123456"
    },
    new PayPalPayment
    {
        TransactionId = "PP-002",
        Amount = 149.99m,
        Email = "user@example.com"
    },
    new CryptoPayment
    {
        TransactionId = "BTC-003",
        Amount = 299.99m,
        WalletAddress = "1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa",
        Cryptocurrency = "Bitcoin"
    }
};

foreach (var payment in payments)
{
    processor.Process(payment); // Polymorphic call
}
```

### Virtual, Override, and New Keywords

```csharp
public class BaseClass
{
    public virtual void Method1()
    {
        Console.WriteLine("BaseClass Method1");
    }

    public virtual void Method2()
    {
        Console.WriteLine("BaseClass Method2");
    }

    public void Method3()
    {
        Console.WriteLine("BaseClass Method3");
    }
}

public class DerivedClass : BaseClass
{
    // Override - polymorphic behavior
    public override void Method1()
    {
        Console.WriteLine("DerivedClass Method1");
    }

    // New - hides base method (not polymorphic)
    public new void Method2()
    {
        Console.WriteLine("DerivedClass Method2");
    }

    // New - hides base method
    public new void Method3()
    {
        Console.WriteLine("DerivedClass Method3");
    }
}

// Usage - Understanding the difference
DerivedClass derived = new DerivedClass();
BaseClass baseRef = derived; // Polymorphic reference

derived.Method1();  // Output: DerivedClass Method1
baseRef.Method1();  // Output: DerivedClass Method1 (polymorphic)

derived.Method2();  // Output: DerivedClass Method2
baseRef.Method2();  // Output: BaseClass Method2 (not polymorphic!)

derived.Method3();  // Output: DerivedClass Method3
baseRef.Method3();  // Output: BaseClass Method3 (not polymorphic!)
```

---

## PART 2: SOLID PRINCIPLES

## 7. Single Responsibility Principle (SRP)

**Definition**: A class should have only one reason to change. Each class should have only one responsibility.

### Violation Example (Bad)

```csharp
// ❌ BAD: Class has multiple responsibilities
public class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }

    // Responsibility 1: Calculate salary
    public decimal CalculateSalary()
    {
        // Salary calculation logic
        return Salary * 12;
    }

    // Responsibility 2: Save to database
    public void SaveToDatabase()
    {
        // Database save logic
        Console.WriteLine("Saving employee to database...");
    }

    // Responsibility 3: Generate report
    public string GenerateReport()
    {
        return $"Employee Report: {Name}, Annual Salary: ${CalculateSalary()}";
    }

    // Responsibility 4: Send email
    public void SendEmail(string message)
    {
        Console.WriteLine($"Sending email to {Name}: {message}");
    }
}
```

### Following SRP (Good)

```csharp
// ✅ GOOD: Each class has single responsibility

// Responsibility: Employee data
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Salary { get; set; }
    public string Department { get; set; }
}

// Responsibility: Salary calculations
public class SalaryCalculator
{
    public decimal CalculateAnnualSalary(Employee employee)
    {
        return employee.Salary * 12;
    }

    public decimal CalculateWithBonus(Employee employee, decimal bonusPercentage)
    {
        return employee.Salary + (employee.Salary * bonusPercentage / 100);
    }

    public decimal CalculateTax(Employee employee)
    {
        decimal annualSalary = CalculateAnnualSalary(employee);
        // Tax calculation logic
        return annualSalary * 0.2m;
    }
}

// Responsibility: Database operations
public class EmployeeRepository
{
    public void Save(Employee employee)
    {
        Console.WriteLine($"Saving employee {employee.Name} to database");
        // Database save logic
    }

    public Employee GetById(int id)
    {
        Console.WriteLine($"Fetching employee with ID: {id}");
        // Database fetch logic
        return new Employee();
    }

    public void Delete(int id)
    {
        Console.WriteLine($"Deleting employee with ID: {id}");
        // Database delete logic
    }
}

// Responsibility: Report generation
public class EmployeeReportGenerator
{
    private readonly SalaryCalculator _salaryCalculator;

    public EmployeeReportGenerator(SalaryCalculator salaryCalculator)
    {
        _salaryCalculator = salaryCalculator;
    }

    public string GenerateSalaryReport(Employee employee)
    {
        decimal annualSalary = _salaryCalculator.CalculateAnnualSalary(employee);
        return $"Salary Report for {employee.Name}\n" +
               $"Monthly: ${employee.Salary:N2}\n" +
               $"Annual: ${annualSalary:N2}";
    }

    public string GenerateFullReport(Employee employee)
    {
        return $"Employee Report\n" +
               $"Name: {employee.Name}\n" +
               $"Department: {employee.Department}\n" +
               $"Email: {employee.Email}\n" +
               GenerateSalaryReport(employee);
    }
}

// Responsibility: Email notifications
public class EmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        Console.WriteLine($"Sending email to {to}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {body}");
    }

    public void SendWelcomeEmail(Employee employee)
    {
        SendEmail(employee.Email, "Welcome!", $"Welcome to the company, {employee.Name}!");
    }
}

// Usage
var employee = new Employee
{
    Id = 1,
    Name = "John Doe",
    Email = "john@company.com",
    Salary = 5000m,
    Department = "IT"
};

var salaryCalculator = new SalaryCalculator();
var repository = new EmployeeRepository();
var reportGenerator = new EmployeeReportGenerator(salaryCalculator);
var emailService = new EmailService();

repository.Save(employee);
string report = reportGenerator.GenerateFullReport(employee);
Console.WriteLine(report);
emailService.SendWelcomeEmail(employee);
```

---

## 8. Open/Closed Principle (OCP)

**Definition**: Software entities should be open for extension but closed for modification.

### Violation Example (Bad)

```csharp
// ❌ BAD: Need to modify class for each new discount type
public class DiscountCalculator
{
    public decimal CalculateDiscount(decimal price, string customerType)
    {
        if (customerType == "Regular")
        {
            return price * 0.1m; // 10% discount
        }
        else if (customerType == "Premium")
        {
            return price * 0.2m; // 20% discount
        }
        else if (customerType == "VIP")
        {
            return price * 0.3m; // 30% discount
        }
        // Adding new customer type requires modifying this class!
        else if (customerType == "Gold")
        {
            return price * 0.25m; // 25% discount
        }
        return 0;
    }
}
```

### Following OCP (Good)

```csharp
// ✅ GOOD: Open for extension, closed for modification

// Abstraction
public interface IDiscountStrategy
{
    decimal CalculateDiscount(decimal price);
    string Description { get; }
}

// Concrete implementations
public class RegularCustomerDiscount : IDiscountStrategy
{
    public string Description => "Regular Customer - 10% Off";

    public decimal CalculateDiscount(decimal price)
    {
        return price * 0.1m;
    }
}

public class PremiumCustomerDiscount : IDiscountStrategy
{
    public string Description => "Premium Customer - 20% Off";

    public decimal CalculateDiscount(decimal price)
    {
        return price * 0.2m;
    }
}

public class VIPCustomerDiscount : IDiscountStrategy
{
    public string Description => "VIP Customer - 30% Off";

    public decimal CalculateDiscount(decimal price)
    {
        return price * 0.3m;
    }
}

// New discount type - just add new class, no modification needed!
public class GoldCustomerDiscount : IDiscountStrategy
{
    public string Description => "Gold Customer - 25% Off";

    public decimal CalculateDiscount(decimal price)
    {
        return price * 0.25m;
    }
}

public class SeasonalDiscount : IDiscountStrategy
{
    private readonly decimal _discountPercentage;

    public SeasonalDiscount(decimal discountPercentage)
    {
        _discountPercentage = discountPercentage;
    }

    public string Description => $"Seasonal Sale - {_discountPercentage * 100}% Off";

    public decimal CalculateDiscount(decimal price)
    {
        return price * _discountPercentage;
    }
}

// Calculator uses abstraction
public class DiscountCalculator
{
    public decimal CalculateFinalPrice(decimal price, IDiscountStrategy discountStrategy)
    {
        decimal discount = discountStrategy.CalculateDiscount(price);
        decimal finalPrice = price - discount;

        Console.WriteLine($"Original Price: ${price:N2}");
        Console.WriteLine($"Discount: {discountStrategy.Description}");
        Console.WriteLine($"Discount Amount: ${discount:N2}");
        Console.WriteLine($"Final Price: ${finalPrice:N2}");

        return finalPrice;
    }
}

// Usage
var calculator = new DiscountCalculator();

decimal price = 100m;
calculator.CalculateFinalPrice(price, new RegularCustomerDiscount());
calculator.CalculateFinalPrice(price, new PremiumCustomerDiscount());
calculator.CalculateFinalPrice(price, new VIPCustomerDiscount());
calculator.CalculateFinalPrice(price, new GoldCustomerDiscount());
calculator.CalculateFinalPrice(price, new SeasonalDiscount(0.15m));
```

### Real-World Example: Logger

```csharp
// Abstraction
public interface ILogger
{
    void Log(string message);
}

// Concrete implementations
public class FileLogger : ILogger
{
    private readonly string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        Console.WriteLine($"Writing to file {_filePath}: {message}");
        // File.AppendAllText(_filePath, $"{DateTime.Now}: {message}\n");
    }
}

public class DatabaseLogger : ILogger
{
    private readonly string _connectionString;

    public DatabaseLogger(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Log(string message)
    {
        Console.WriteLine($"Writing to database: {message}");
        // Database insert logic
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {DateTime.Now}: {message}");
    }
}

// New logger type - no modification to existing code!
public class CloudLogger : ILogger
{
    private readonly string _cloudEndpoint;

    public CloudLogger(string cloudEndpoint)
    {
        _cloudEndpoint = cloudEndpoint;
    }

    public void Log(string message)
    {
        Console.WriteLine($"Sending to cloud {_cloudEndpoint}: {message}");
        // Cloud API call
    }
}

// Application uses abstraction
public class Application
{
    private readonly ILogger _logger;

    public Application(ILogger logger)
    {
        _logger = logger;
    }

    public void Run()
    {
        _logger.Log("Application started");
        // Business logic
        _logger.Log("Application finished");
    }
}

// Usage - easily switch logger implementations
var app1 = new Application(new FileLogger("app.log"));
app1.Run();

var app2 = new Application(new DatabaseLogger("connection_string"));
app2.Run();

var app3 = new Application(new ConsoleLogger());
app3.Run();

var app4 = new Application(new CloudLogger("https://logs.cloud.com"));
app4.Run();
```

---

## 9. Liskov Substitution Principle (LSP)

**Definition**: Objects of a superclass should be replaceable with objects of its subclasses without breaking the application.

### Violation Example (Bad)

```csharp
// ❌ BAD: Square breaks LSP when substituting Rectangle
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public int CalculateArea()
    {
        return Width * Height;
    }
}

public class Square : Rectangle
{
    private int _side;

    public override int Width
    {
        get { return _side; }
        set
        {
            _side = value;
            // Problem: Setting width also changes height!
        }
    }

    public override int Height
    {
        get { return _side; }
        set
        {
            _side = value;
            // Problem: Setting height also changes width!
        }
    }
}

// This breaks LSP
void TestRectangle(Rectangle rectangle)
{
    rectangle.Width = 5;
    rectangle.Height = 10;
    // Expected: Area = 50
    // With Rectangle: Area = 50 ✅
    // With Square: Area = 100 ❌ (Unexpected behavior!)
    Console.WriteLine($"Area: {rectangle.CalculateArea()}");
}

TestRectangle(new Rectangle()); // Works as expected: 50
TestRectangle(new Square());    // Doesn't work: 100 (LSP violated!)
```

### Following LSP (Good)

```csharp
// ✅ GOOD: Proper abstraction that doesn't violate LSP

// Abstract base
public abstract class Shape
{
    public abstract int CalculateArea();
}

public class Rectangle : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Rectangle(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public override int CalculateArea()
    {
        return Width * Height;
    }
}

public class Square : Shape
{
    public int Side { get; set; }

    public Square(int side)
    {
        Side = side;
    }

    public override int CalculateArea()
    {
        return Side * Side;
    }
}

// Both can be substituted without issues
void TestShape(Shape shape)
{
    Console.WriteLine($"Area: {shape.CalculateArea()}");
}

TestShape(new Rectangle(5, 10)); // Area: 50
TestShape(new Square(5));         // Area: 25
```

### Real-World Example: Bird Hierarchy

```csharp
// ❌ BAD: Violates LSP
public class Bird
{
    public virtual void Fly()
    {
        Console.WriteLine("Flying...");
    }
}

public class Penguin : Bird
{
    public override void Fly()
    {
        throw new NotSupportedException("Penguins can't fly!");
        // Violates LSP - can't substitute Bird with Penguin
    }
}

// ✅ GOOD: Follows LSP
public abstract class Bird
{
    public abstract void Move();
    public abstract void Eat();
}

public class FlyingBird : Bird
{
    public virtual void Fly()
    {
        Console.WriteLine("Flying through the air");
    }

    public override void Move()
    {
        Fly();
    }

    public override void Eat()
    {
        Console.WriteLine("Eating food");
    }
}

public class Eagle : FlyingBird
{
    public override void Fly()
    {
        Console.WriteLine("Eagle soaring high");
    }
}

public class Sparrow : FlyingBird
{
    public override void Fly()
    {
        Console.WriteLine("Sparrow flying quickly");
    }
}

public class FlightlessBird : Bird
{
    public virtual void Walk()
    {
        Console.WriteLine("Walking on ground");
    }

    public override void Move()
    {
        Walk();
    }

    public override void Eat()
    {
        Console.WriteLine("Eating food");
    }
}

public class Penguin : FlightlessBird
{
    public override void Walk()
    {
        Console.WriteLine("Penguin waddling");
    }

    public void Swim()
    {
        Console.WriteLine("Penguin swimming");
    }
}

public class Ostrich : FlightlessBird
{
    public override void Walk()
    {
        Console.WriteLine("Ostrich running fast");
    }
}

// Usage - LSP maintained
void MakeBirdMove(Bird bird)
{
    bird.Move(); // Works for all birds
    bird.Eat();
}

MakeBirdMove(new Eagle());
MakeBirdMove(new Penguin());
MakeBirdMove(new Ostrich());
```

---

## 10. Interface Segregation Principle (ISP)

**Definition**: Clients should not be forced to depend on interfaces they don't use. Many specific interfaces are better than one general interface.

### Violation Example (Bad)

```csharp
// ❌ BAD: Fat interface forces classes to implement unnecessary methods
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
    void GetPaid();
}

public class HumanWorker : IWorker
{
    public void Work()
    {
        Console.WriteLine("Human working");
    }

    public void Eat()
    {
        Console.WriteLine("Human eating");
    }

    public void Sleep()
    {
        Console.WriteLine("Human sleeping");
    }

    public void GetPaid()
    {
        Console.WriteLine("Human getting paid");
    }
}

public class RobotWorker : IWorker
{
    public void Work()
    {
        Console.WriteLine("Robot working");
    }

    // ❌ Robot doesn't eat!
    public void Eat()
    {
        throw new NotImplementedException("Robots don't eat");
    }

    // ❌ Robot doesn't sleep!
    public void Sleep()
    {
        throw new NotImplementedException("Robots don't sleep");
    }

    public void GetPaid()
    {
        Console.WriteLine("Charging robot battery");
    }
}
```

### Following ISP (Good)

```csharp
// ✅ GOOD: Segregated interfaces

public interface IWorkable
{
    void Work();
}

public interface IEatable
{
    void Eat();
}

public interface ISleepable
{
    void Sleep();
}

public interface IPayable
{
    void GetPaid();
}

// Human implements all interfaces
public class HumanWorker : IWorkable, IEatable, ISleepable, IPayable
{
    public void Work()
    {
        Console.WriteLine("Human working");
    }

    public void Eat()
    {
        Console.WriteLine("Human eating lunch");
    }

    public void Sleep()
    {
        Console.WriteLine("Human sleeping at night");
    }

    public void GetPaid()
    {
        Console.WriteLine("Human receiving salary");
    }
}

// Robot only implements relevant interfaces
public class RobotWorker : IWorkable, IPayable
{
    public void Work()
    {
        Console.WriteLine("Robot working 24/7");
    }

    public void GetPaid()
    {
        Console.WriteLine("Robot maintenance scheduled");
    }
}

// Contractor - works and gets paid but doesn't sleep at work
public class ContractorWorker : IWorkable, IPayable
{
    public void Work()
    {
        Console.WriteLine("Contractor working on project");
    }

    public void GetPaid()
    {
        Console.WriteLine("Contractor invoicing for work");
    }
}

// Manager class uses segregated interfaces
public class WorkManager
{
    public void ManageWork(IWorkable worker)
    {
        worker.Work();
    }

    public void ManagePayroll(IPayable worker)
    {
        worker.GetPaid();
    }

    public void ManageLunchBreak(IEatable worker)
    {
        worker.Eat();
    }
}

// Usage
var workManager = new WorkManager();

var human = new HumanWorker();
workManager.ManageWork(human);
workManager.ManageLunchBreak(human);
workManager.ManagePayroll(human);

var robot = new RobotWorker();
workManager.ManageWork(robot);
workManager.ManagePayroll(robot);
// workManager.ManageLunchBreak(robot); // Compile error - robot doesn't implement IEatable
```

### Real-World Example: Document Processor

```csharp
// ❌ BAD: Fat interface
public interface IDocument
{
    void Open();
    void Save();
    void Print();
    void Fax();
    void Scan();
    void Email();
}

// ✅ GOOD: Segregated interfaces
public interface IOpenable
{
    void Open();
}

public interface ISaveable
{
    void Save();
}

public interface IPrintable
{
    void Print();
}

public interface IFaxable
{
    void Fax();
}

public interface IScannable
{
    void Scan();
}

public interface IEmailable
{
    void Email();
}

// Modern document - supports everything
public class ModernDocument : IOpenable, ISaveable, IPrintable, IScannable, IEmailable
{
    public string FileName { get; set; }

    public void Open()
    {
        Console.WriteLine($"Opening {FileName}");
    }

    public void Save()
    {
        Console.WriteLine($"Saving {FileName}");
    }

    public void Print()
    {
        Console.WriteLine($"Printing {FileName}");
    }

    public void Scan()
    {
        Console.WriteLine($"Scanning {FileName}");
    }

    public void Email()
    {
        Console.WriteLine($"Emailing {FileName}");
    }
}

// Simple document - only basic operations
public class SimpleDocument : IOpenable, ISaveable
{
    public string FileName { get; set; }

    public void Open()
    {
        Console.WriteLine($"Opening simple document {FileName}");
    }

    public void Save()
    {
        Console.WriteLine($"Saving simple document {FileName}");
    }
}

// Read-only document
public class ReadOnlyDocument : IOpenable, IPrintable
{
    public string FileName { get; set; }

    public void Open()
    {
        Console.WriteLine($"Opening read-only {FileName}");
    }

    public void Print()
    {
        Console.WriteLine($"Printing read-only {FileName}");
    }
}
```

---

## 11. Dependency Inversion Principle (DIP)

**Definition**:
1. High-level modules should not depend on low-level modules. Both should depend on abstractions.
2. Abstractions should not depend on details. Details should depend on abstractions.

### Violation Example (Bad)

```csharp
// ❌ BAD: High-level class depends on low-level class directly

// Low-level class
public class EmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        Console.WriteLine($"Sending email to {to}: {subject}");
    }
}

// High-level class depends on concrete EmailService
public class UserRegistration
{
    private readonly EmailService _emailService; // Tight coupling!

    public UserRegistration()
    {
        _emailService = new EmailService(); // Creates dependency directly
    }

    public void RegisterUser(string email, string password)
    {
        // Registration logic
        Console.WriteLine($"Registering user: {email}");

        // Send welcome email
        _emailService.SendEmail(email, "Welcome", "Welcome to our service!");

        // Problem: Can't easily switch to SMS or other notification methods
        // Problem: Hard to test without sending real emails
    }
}
```

### Following DIP (Good)

```csharp
// ✅ GOOD: Both depend on abstraction

// Abstraction
public interface INotificationService
{
    void SendNotification(string to, string message);
}

// Low-level implementations
public class EmailService : INotificationService
{
    public void SendNotification(string to, string message)
    {
        Console.WriteLine($"Sending email to {to}: {message}");
        // Email sending logic
    }
}

public class SmsService : INotificationService
{
    public void SendNotification(string to, string message)
    {
        Console.WriteLine($"Sending SMS to {to}: {message}");
        // SMS sending logic
    }
}

public class PushNotificationService : INotificationService
{
    public void SendNotification(string to, string message)
    {
        Console.WriteLine($"Sending push notification to {to}: {message}");
        // Push notification logic
    }
}

// High-level class depends on abstraction
public class UserRegistration
{
    private readonly INotificationService _notificationService;

    // Dependency injected through constructor
    public UserRegistration(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public void RegisterUser(string contact, string password)
    {
        // Registration logic
        Console.WriteLine($"Registering user: {contact}");

        // Send notification using abstraction
        _notificationService.SendNotification(contact, "Welcome to our service!");
    }
}

// Usage - easily switch implementations
var emailRegistration = new UserRegistration(new EmailService());
emailRegistration.RegisterUser("user@example.com", "password123");

var smsRegistration = new UserRegistration(new SmsService());
smsRegistration.RegisterUser("+1234567890", "password123");

var pushRegistration = new UserRegistration(new PushNotificationService());
pushRegistration.RegisterUser("device_token_xyz", "password123");
```

### Real-World Example: Data Access

```csharp
// ❌ BAD: Business logic depends on concrete data access
public class SqlServerDataAccess
{
    public void SaveCustomer(Customer customer)
    {
        Console.WriteLine("Saving to SQL Server");
    }
}

public class CustomerService
{
    private readonly SqlServerDataAccess _dataAccess = new SqlServerDataAccess();

    public void CreateCustomer(Customer customer)
    {
        // Business logic
        _dataAccess.SaveCustomer(customer); // Tightly coupled to SQL Server
    }
}

// ✅ GOOD: Depend on abstraction
public interface ICustomerRepository
{
    void Save(Customer customer);
    Customer GetById(int id);
    IEnumerable<Customer> GetAll();
    void Delete(int id);
}

// Low-level implementations
public class SqlServerCustomerRepository : ICustomerRepository
{
    private readonly string _connectionString;

    public SqlServerCustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Save(Customer customer)
    {
        Console.WriteLine($"Saving customer to SQL Server: {customer.Name}");
        // SQL Server specific implementation
    }

    public Customer GetById(int id)
    {
        Console.WriteLine($"Fetching from SQL Server: {id}");
        return new Customer { Id = id };
    }

    public IEnumerable<Customer> GetAll()
    {
        Console.WriteLine("Fetching all from SQL Server");
        return new List<Customer>();
    }

    public void Delete(int id)
    {
        Console.WriteLine($"Deleting from SQL Server: {id}");
    }
}

public class MongoDbCustomerRepository : ICustomerRepository
{
    private readonly string _connectionString;

    public MongoDbCustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Save(Customer customer)
    {
        Console.WriteLine($"Saving customer to MongoDB: {customer.Name}");
        // MongoDB specific implementation
    }

    public Customer GetById(int id)
    {
        Console.WriteLine($"Fetching from MongoDB: {id}");
        return new Customer { Id = id };
    }

    public IEnumerable<Customer> GetAll()
    {
        Console.WriteLine("Fetching all from MongoDB");
        return new List<Customer>();
    }

    public void Delete(int id)
    {
        Console.WriteLine($"Deleting from MongoDB: {id}");
    }
}

public class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new List<Customer>();

    public void Save(Customer customer)
    {
        Console.WriteLine($"Saving customer to memory: {customer.Name}");
        _customers.Add(customer);
    }

    public Customer GetById(int id)
    {
        return _customers.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Customer> GetAll()
    {
        return _customers;
    }

    public void Delete(int id)
    {
        var customer = GetById(id);
        if (customer != null)
            _customers.Remove(customer);
    }
}

// High-level business logic depends on abstraction
public class CustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly INotificationService _notificationService;

    public CustomerService(
        ICustomerRepository repository,
        INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public void CreateCustomer(Customer customer)
    {
        // Business logic
        if (string.IsNullOrEmpty(customer.Email))
            throw new ArgumentException("Email is required");

        // Save using abstraction
        _repository.Save(customer);

        // Send notification using abstraction
        _notificationService.SendNotification(
            customer.Email,
            $"Welcome {customer.Name}!");

        Console.WriteLine("Customer created successfully");
    }

    public Customer GetCustomer(int id)
    {
        return _repository.GetById(id);
    }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

// Usage - Dependency Injection Container (e.g., in ASP.NET Core)
// services.AddScoped<ICustomerRepository, SqlServerCustomerRepository>();
// services.AddScoped<INotificationService, EmailService>();
// services.AddScoped<CustomerService>();

// Manual DI for demonstration
ICustomerRepository repository = new SqlServerCustomerRepository("connection_string");
INotificationService notificationService = new EmailService();
var customerService = new CustomerService(repository, notificationService);

var customer = new Customer
{
    Id = 1,
    Name = "John Doe",
    Email = "john@example.com"
};

customerService.CreateCustomer(customer);

// Easy to switch to different implementations
ICustomerRepository mongoRepo = new MongoDbCustomerRepository("mongo_connection");
INotificationService smsService = new SmsService();
var customerService2 = new CustomerService(mongoRepo, smsService);

customerService2.CreateCustomer(customer);

// Easy to test with in-memory repository
ICustomerRepository inMemoryRepo = new InMemoryCustomerRepository();
var testService = new CustomerService(inMemoryRepo, new EmailService());
testService.CreateCustomer(customer);
```

---

## PART 3: ADVANCED OOP CONCEPTS

## 12. Abstract Classes vs Interfaces

### When to Use Abstract Classes

- When you want to provide common implementation for derived classes
- When you need to define non-public members
- When you want to add new methods in the future without breaking existing code
- When classes share a strong "IS-A" relationship

### When to Use Interfaces

- When you want to define a contract for unrelated classes
- When you need multiple inheritance
- When you want complete abstraction
- When you want to achieve polymorphism across different class hierarchies

```csharp
// Abstract Class Example
public abstract class Vehicle
{
    // Fields
    protected string _brand;

    // Properties
    public string Model { get; set; }
    public int Year { get; set; }

    // Constructor
    protected Vehicle(string brand)
    {
        _brand = brand;
    }

    // Abstract methods (must be implemented)
    public abstract void Start();
    public abstract void Stop();
    public abstract double GetFuelEfficiency();

    // Concrete method (shared implementation)
    public void DisplayInfo()
    {
        Console.WriteLine($"{_brand} {Model} ({Year})");
        Console.WriteLine($"Fuel Efficiency: {GetFuelEfficiency()} MPG");
    }

    // Virtual method (can be overridden)
    public virtual void Honk()
    {
        Console.WriteLine("Beep beep!");
    }
}

public class Car : Vehicle
{
    public int NumberOfDoors { get; set; }

    public Car(string brand) : base(brand)
    {
    }

    public override void Start()
    {
        Console.WriteLine($"{_brand} car engine starting...");
    }

    public override void Stop()
    {
        Console.WriteLine($"{_brand} car engine stopping...");
    }

    public override double GetFuelEfficiency()
    {
        return 30.5; // MPG
    }

    public override void Honk()
    {
        Console.WriteLine($"{_brand} car: Horn!");
    }
}

// Interface Example
public interface IFlyable
{
    void TakeOff();
    void Land();
    double GetMaxAltitude();
}

public interface ISwimmable
{
    void Dive();
    void Surface();
    double GetMaxDepth();
}

// Class can implement multiple interfaces
public class Airplane : Vehicle, IFlyable
{
    public Airplane(string brand) : base(brand)
    {
    }

    public override void Start()
    {
        Console.WriteLine($"{_brand} airplane engines starting...");
    }

    public override void Stop()
    {
        Console.WriteLine($"{_brand} airplane engines stopping...");
    }

    public override double GetFuelEfficiency()
    {
        return 5.5; // Miles per gallon
    }

    // Implement IFlyable
    public void TakeOff()
    {
        Console.WriteLine($"{_brand} taking off...");
    }

    public void Land()
    {
        Console.WriteLine($"{_brand} landing...");
    }

    public double GetMaxAltitude()
    {
        return 35000; // feet
    }
}

// Unrelated class can implement same interface
public class Duck : IFlyable, ISwimmable
{
    public string Name { get; set; }

    public void TakeOff()
    {
        Console.WriteLine($"{Name} the duck is taking off");
    }

    public void Land()
    {
        Console.WriteLine($"{Name} the duck is landing");
    }

    public double GetMaxAltitude()
    {
        return 5000; // feet
    }

    public void Dive()
    {
        Console.WriteLine($"{Name} the duck is diving");
    }

    public void Surface()
    {
        Console.WriteLine($"{Name} the duck is surfacing");
    }

    public double GetMaxDepth()
    {
        return 20; // feet
    }
}

// Usage
Vehicle car = new Car("Toyota") { Model = "Camry", Year = 2023 };
car.DisplayInfo();
car.Start();
car.Honk();

IFlyable airplane = new Airplane("Boeing") { Model = "747", Year = 2020 };
airplane.TakeOff();

IFlyable duck1 = new Duck { Name = "Donald" };
duck1.TakeOff();

ISwimmable duck2 = new Duck { Name = "Daffy" };
duck2.Dive();
```

### Comparison Table

```csharp
/*
┌─────────────────────────┬──────────────────────────┬────────────────────────┐
│ Feature                 │ Abstract Class           │ Interface              │
├─────────────────────────┼──────────────────────────┼────────────────────────┤
│ Multiple Inheritance    │ No                       │ Yes                    │
│ Fields                  │ Yes                      │ No (C# 8+ can have)    │
│ Constructors            │ Yes                      │ No                     │
│ Access Modifiers        │ All modifiers            │ Public only (default)  │
│ Implementation          │ Can have                 │ No (C# 8+ can have)    │
│ Method Types            │ Abstract, Virtual,       │ Abstract (default)     │
│                         │ Concrete                 │ Default (C# 8+)        │
│ Properties              │ Yes                      │ Yes                    │
│ Performance             │ Faster                   │ Slightly slower        │
│ When to Use             │ IS-A relationship        │ CAN-DO relationship    │
│                         │ Share implementation     │ Contract definition    │
│                         │ Protected members needed │ Multiple inheritance   │
└─────────────────────────┴──────────────────────────┴────────────────────────┘
*/
```

---

## 13. Composition vs Inheritance

**Composition**: "HAS-A" relationship - object contains other objects
**Inheritance**: "IS-A" relationship - object is a type of another object

### Prefer Composition Over Inheritance

```csharp
// ❌ Inheritance approach (can lead to problems)
public class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }

    public virtual void Work()
    {
        Console.WriteLine($"{Name} is working");
    }
}

public class Manager : Employee
{
    public void ManageTeam()
    {
        Console.WriteLine($"{Name} is managing team");
    }
}

public class Programmer : Employee
{
    public void WriteCode()
    {
        Console.WriteLine($"{Name} is writing code");
    }
}

// ❌ Problem: What if someone is both Manager and Programmer?
// Can't do: public class TechLead : Manager, Programmer // Not allowed in C#

// ✅ Composition approach (flexible and maintainable)
public interface IWorkable
{
    void Work();
}

public class WorkBehavior : IWorkable
{
    private readonly string _personName;

    public WorkBehavior(string personName)
    {
        _personName = personName;
    }

    public void Work()
    {
        Console.WriteLine($"{_personName} is working");
    }
}

public class ManagementBehavior
{
    private readonly string _personName;

    public ManagementBehavior(string personName)
    {
        _personName = personName;
    }

    public void ManageTeam()
    {
        Console.WriteLine($"{_personName} is managing team");
    }

    public void ReviewPerformance()
    {
        Console.WriteLine($"{_personName} is reviewing team performance");
    }
}

public class ProgrammingBehavior
{
    private readonly string _personName;

    public ProgrammingBehavior(string personName)
    {
        _personName = personName;
    }

    public void WriteCode()
    {
        Console.WriteLine($"{_personName} is writing code");
    }

    public void CodeReview()
    {
        Console.WriteLine($"{_personName} is reviewing code");
    }
}

// Employee using composition
public class Employee2
{
    public string Name { get; set; }
    public decimal Salary { get; set; }

    // Composed behaviors
    private readonly List<object> _behaviors = new List<object>();

    public void AddBehavior(object behavior)
    {
        _behaviors.Add(behavior);
    }

    public T GetBehavior<T>() where T : class
    {
        return _behaviors.OfType<T>().FirstOrDefault();
    }

    public void Work()
    {
        Console.WriteLine($"{Name} is working");
    }
}

// Usage - Much more flexible!
var programmer = new Employee2 { Name = "Alice", Salary = 80000 };
programmer.AddBehavior(new ProgrammingBehavior(programmer.Name));

var manager = new Employee2 { Name = "Bob", Salary = 100000 };
manager.AddBehavior(new ManagementBehavior(manager.Name));

// Tech lead has BOTH behaviors!
var techLead = new Employee2 { Name = "Charlie", Salary = 120000 };
techLead.AddBehavior(new ProgrammingBehavior(techLead.Name));
techLead.AddBehavior(new ManagementBehavior(techLead.Name));

// Use behaviors
programmer.GetBehavior<ProgrammingBehavior>()?.WriteCode();

manager.GetBehavior<ManagementBehavior>()?.ManageTeam();

techLead.GetBehavior<ProgrammingBehavior>()?.WriteCode();
techLead.GetBehavior<ManagementBehavior>()?.ManageTeam();
```

### Real-World Example: Car Features

```csharp
// Features as separate classes (composition)
public class Engine
{
    public int Horsepower { get; set; }
    public string Type { get; set; }

    public void Start()
    {
        Console.WriteLine($"Starting {Type} engine with {Horsepower} HP");
    }

    public void Stop()
    {
        Console.WriteLine("Engine stopped");
    }
}

public class GPS
{
    public void Navigate(string destination)
    {
        Console.WriteLine($"Navigating to {destination}");
    }

    public string GetCurrentLocation()
    {
        return "Current Location: 123 Main St";
    }
}

public class AudioSystem
{
    public void PlayMusic(string song)
    {
        Console.WriteLine($"Playing: {song}");
    }

    public void AdjustVolume(int level)
    {
        Console.WriteLine($"Volume set to {level}");
    }
}

public class ClimateControl
{
    public void SetTemperature(int temperature)
    {
        Console.WriteLine($"Temperature set to {temperature}°F");
    }

    public void TurnOnAC()
    {
        Console.WriteLine("Air conditioning turned on");
    }
}

// Car composed of features
public class ComposedCar
{
    public string Brand { get; set; }
    public string Model { get; set; }

    // Composition - HAS-A relationships
    private readonly Engine _engine;
    private readonly GPS _gps;
    private readonly AudioSystem _audioSystem;
    private readonly ClimateControl _climateControl;

    public ComposedCar(
        string brand,
        string model,
        Engine engine,
        GPS gps = null,
        AudioSystem audioSystem = null,
        ClimateControl climateControl = null)
    {
        Brand = brand;
        Model = model;
        _engine = engine;
        _gps = gps;
        _audioSystem = audioSystem;
        _climateControl = climateControl;
    }

    public void Start()
    {
        Console.WriteLine($"Starting {Brand} {Model}");
        _engine.Start();
    }

    public void Navigate(string destination)
    {
        if (_gps != null)
        {
            _gps.Navigate(destination);
        }
        else
        {
            Console.WriteLine("GPS not available in this model");
        }
    }

    public void PlayMusic(string song)
    {
        if (_audioSystem != null)
        {
            _audioSystem.PlayMusic(song);
        }
        else
        {
            Console.WriteLine("Audio system not available");
        }
    }

    public void SetTemperature(int temp)
    {
        if (_climateControl != null)
        {
            _climateControl.SetTemperature(temp);
        }
        else
        {
            Console.WriteLine("Climate control not available");
        }
    }
}

// Usage
var basicEngine = new Engine { Horsepower = 150, Type = "4-cylinder" };
var basicCar = new ComposedCar("Toyota", "Corolla", basicEngine);
basicCar.Start();
basicCar.Navigate("Downtown"); // Not available

var luxuryEngine = new Engine { Horsepower = 300, Type = "V6" };
var luxuryCar = new ComposedCar(
    "BMW",
    "5 Series",
    luxuryEngine,
    new GPS(),
    new AudioSystem(),
    new ClimateControl()
);

luxuryCar.Start();
luxuryCar.Navigate("Airport");
luxuryCar.PlayMusic("Favorite Song");
luxuryCar.SetTemperature(72);
```

---

## 14. Method Overloading and Overriding

### Method Overloading (Compile-time Polymorphism)

Same method name, different parameters.

```csharp
public class MathOperations
{
    // Overloaded methods - same name, different parameters

    public int Add(int a, int b)
    {
        Console.WriteLine("Adding two integers");
        return a + b;
    }

    public double Add(double a, double b)
    {
        Console.WriteLine("Adding two doubles");
        return a + b;
    }

    public int Add(int a, int b, int c)
    {
        Console.WriteLine("Adding three integers");
        return a + b + c;
    }

    public string Add(string a, string b)
    {
        Console.WriteLine("Concatenating two strings");
        return a + b;
    }

    // Different parameter order
    public void Display(string name, int age)
    {
        Console.WriteLine($"Name: {name}, Age: {age}");
    }

    public void Display(int age, string name)
    {
        Console.WriteLine($"Age: {age}, Name: {name}");
    }

    // Optional parameters
    public int Multiply(int a, int b, int c = 1)
    {
        return a * b * c;
    }

    // Params keyword
    public int Sum(params int[] numbers)
    {
        Console.WriteLine($"Summing {numbers.Length} numbers");
        return numbers.Sum();
    }
}

// Usage
var math = new MathOperations();
Console.WriteLine(math.Add(5, 10));           // Calls Add(int, int)
Console.WriteLine(math.Add(5.5, 10.5));       // Calls Add(double, double)
Console.WriteLine(math.Add(1, 2, 3));         // Calls Add(int, int, int)
Console.WriteLine(math.Add("Hello", " World")); // Calls Add(string, string)

math.Display("John", 25);                     // Calls Display(string, int)
math.Display(25, "John");                     // Calls Display(int, string)

Console.WriteLine(math.Multiply(2, 3));       // c defaults to 1
Console.WriteLine(math.Multiply(2, 3, 4));    // All parameters provided

Console.WriteLine(math.Sum(1, 2, 3, 4, 5));   // Params array
```

### Method Overriding (Runtime Polymorphism)

```csharp
public class Animal
{
    public string Name { get; set; }

    // Virtual method - can be overridden
    public virtual void MakeSound()
    {
        Console.WriteLine($"{Name} makes a sound");
    }

    // Virtual method with implementation
    public virtual void Sleep()
    {
        Console.WriteLine($"{Name} is sleeping");
    }

    // Non-virtual method - cannot be overridden
    public void Breathe()
    {
        Console.WriteLine($"{Name} is breathing");
    }
}

public class Dog : Animal
{
    // Override virtual method
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} barks: Woof! Woof!");
    }

    // Override and call base implementation
    public override void Sleep()
    {
        Console.WriteLine($"{Name} circles before sleeping");
        base.Sleep(); // Call base implementation
    }

    // New method specific to Dog
    public void Fetch()
    {
        Console.WriteLine($"{Name} is fetching");
    }
}

public class Cat : Animal
{
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} meows: Meow!");
    }

    public override void Sleep()
    {
        Console.WriteLine($"{Name} finds a cozy spot");
        base.Sleep();
    }

    public void Purr()
    {
        Console.WriteLine($"{Name} is purring");
    }
}

// Usage - Runtime polymorphism
Animal[] animals = new Animal[]
{
    new Dog { Name = "Max" },
    new Cat { Name = "Whiskers" },
    new Animal { Name = "Generic Animal" }
};

foreach (var animal in animals)
{
    animal.MakeSound(); // Calls appropriate overridden method
    animal.Sleep();
    animal.Breathe();   // Calls base method
    Console.WriteLine();
}

// Output:
// Max barks: Woof! Woof!
// Max circles before sleeping
// Max is sleeping
// Max is breathing
//
// Whiskers meows: Meow!
// Whiskers finds a cozy spot
// Whiskers is sleeping
// Whiskers is breathing
//
// Generic Animal makes a sound
// Generic Animal is sleeping
// Generic Animal is breathing
```

### Sealed Methods

```csharp
public class Shape
{
    public virtual void Draw()
    {
        Console.WriteLine("Drawing shape");
    }
}

public class Circle : Shape
{
    // Override and seal - derived classes cannot override this
    public sealed override void Draw()
    {
        Console.WriteLine("Drawing circle");
    }
}

public class FilledCircle : Circle
{
    // ❌ Compilation error: Cannot override sealed method
    // public override void Draw()
    // {
    //     Console.WriteLine("Drawing filled circle");
    // }

    // ✅ Can use 'new' to hide the method
    public new void Draw()
    {
        Console.WriteLine("Drawing filled circle");
    }
}
```

---

## 15. Access Modifiers

Access modifiers control the visibility and accessibility of classes, methods, and members.

```csharp
// Class-level access modifiers
public class PublicClass { }           // Accessible from anywhere
internal class InternalClass { }       // Accessible only within same assembly
//private class PrivateClass { }       // ❌ Not allowed for top-level classes
//protected class ProtectedClass { }   // ❌ Not allowed for top-level classes

public class AccessModifiersExample
{
    // Public - accessible from anywhere
    public string PublicField = "public";
    public string PublicProperty { get; set; }
    public void PublicMethod()
    {
        Console.WriteLine("Public method");
    }

    // Private - accessible only within this class
    private string _privateField = "private";
    private string PrivateProperty { get; set; }
    private void PrivateMethod()
    {
        Console.WriteLine("Private method");
    }

    // Protected - accessible in this class and derived classes
    protected string ProtectedField = "protected";
    protected string ProtectedProperty { get; set; }
    protected void ProtectedMethod()
    {
        Console.WriteLine("Protected method");
    }

    // Internal - accessible within same assembly
    internal string InternalField = "internal";
    internal string InternalProperty { get; set; }
    internal void InternalMethod()
    {
        Console.WriteLine("Internal method");
    }

    // Protected Internal - accessible in same assembly OR derived classes
    protected internal string ProtectedInternalField = "protected internal";
    protected internal void ProtectedInternalMethod()
    {
        Console.WriteLine("Protected Internal method");
    }

    // Private Protected (C# 7.2+) - accessible in this class AND derived classes in same assembly
    private protected string PrivateProtectedField = "private protected";
    private protected void PrivateProtectedMethod()
    {
        Console.WriteLine("Private Protected method");
    }

    public void TestAccess()
    {
        // Can access all members within the same class
        Console.WriteLine(PublicField);
        Console.WriteLine(_privateField);
        Console.WriteLine(ProtectedField);
        Console.WriteLine(InternalField);
        Console.WriteLine(ProtectedInternalField);
        Console.WriteLine(PrivateProtectedField);
    }
}

public class DerivedClass : AccessModifiersExample
{
    public void TestDerivedAccess()
    {
        // ✅ Can access
        Console.WriteLine(PublicField);
        Console.WriteLine(ProtectedField);
        Console.WriteLine(InternalField);
        Console.WriteLine(ProtectedInternalField);
        Console.WriteLine(PrivateProtectedField); // Same assembly

        // ❌ Cannot access
        // Console.WriteLine(_privateField); // Compilation error
    }
}

public class UnrelatedClass
{
    public void TestUnrelatedAccess()
    {
        var obj = new AccessModifiersExample();

        // ✅ Can access
        Console.WriteLine(obj.PublicField);
        Console.WriteLine(obj.InternalField); // Same assembly

        // ❌ Cannot access
        // Console.WriteLine(obj._privateField);
        // Console.WriteLine(obj.ProtectedField);
        // Console.WriteLine(obj.PrivateProtectedField);
    }
}

// Access Modifiers Summary
/*
┌──────────────────────┬─────────┬─────────┬──────────┬────────────┬───────────────────┬────────────────────┐
│ Modifier             │ Class   │ Derived │ Assembly │ Derived in │ Derived in other  │ Other Assembly     │
│                      │         │ Class   │          │ Assembly   │ Assembly          │                    │
├──────────────────────┼─────────┼─────────┼──────────┼────────────┼───────────────────┼────────────────────┤
│ public               │ Yes     │ Yes     │ Yes      │ Yes        │ Yes               │ Yes                │
│ private              │ Yes     │ No      │ No       │ No         │ No                │ No                 │
│ protected            │ Yes     │ Yes     │ No       │ Yes        │ Yes               │ No                 │
│ internal             │ Yes     │ Yes     │ Yes      │ Yes        │ No                │ No                 │
│ protected internal   │ Yes     │ Yes     │ Yes      │ Yes        │ Yes               │ No                 │
│ private protected    │ Yes     │ Yes     │ No       │ Yes        │ No                │ No                 │
└──────────────────────┴─────────┴─────────┴──────────┴────────────┴───────────────────┴────────────────────┘
*/
```

### Property Access Modifiers

```csharp
public class Person
{
    // Different access for get and set
    public string Name { get; private set; } // Public read, private write

    public int Age { get; protected set; } // Public read, protected write

    public string Email { get; internal set; } // Public read, internal write

    // Read-only property (get only)
    public string FullInfo => $"{Name} - {Age} years old";

    // Constructor sets private properties
    public Person(string name, int age, string email)
    {
        Name = name;
        Age = age;
        Email = email;
    }

    public void UpdateAge(int newAge)
    {
        Age = newAge; // Can modify within class
    }
}

public class Employee : Person
{
    public Employee(string name, int age, string email)
        : base(name, age, email)
    {
    }

    public void IncrementAge()
    {
        Age = Age + 1; // ✅ Can access protected setter
        // Name = "New Name"; // ❌ Cannot access private setter
    }
}

// Usage
var person = new Person("John", 30, "john@example.com");
Console.WriteLine(person.Name);    // ✅ Can read
Console.WriteLine(person.Age);     // ✅ Can read
// person.Name = "Jane";           // ❌ Cannot write (private set)
// person.Age = 31;                // ❌ Cannot write (protected set)
person.UpdateAge(31);              // ✅ Can modify through method
```

---

## 16. Static Members

Static members belong to the class itself rather than to instances of the class.

```csharp
public class BankAccount
{
    // Instance members (each object has its own copy)
    public string AccountNumber { get; set; }
    public decimal Balance { get; private set; }

    // Static members (shared across all instances)
    private static int _accountCounter = 1000;
    private static decimal _interestRate = 0.05m;
    private static List<BankAccount> _allAccounts = new List<BankAccount>();

    // Static property
    public static int TotalAccounts => _allAccounts.Count;

    // Static read-only property
    public static decimal InterestRate
    {
        get { return _interestRate; }
        set
        {
            if (value >= 0 && value <= 1)
                _interestRate = value;
        }
    }

    // Constructor
    public BankAccount(decimal initialBalance)
    {
        AccountNumber = $"ACC{_accountCounter++}"; // Use static counter
        Balance = initialBalance;
        _allAccounts.Add(this); // Add to static list
    }

    // Instance method
    public void Deposit(decimal amount)
    {
        Balance += amount;
        Console.WriteLine($"Deposited ${amount}. Balance: ${Balance}");
    }

    // Instance method using static member
    public void ApplyInterest()
    {
        decimal interest = Balance * _interestRate;
        Balance += interest;
        Console.WriteLine($"Interest applied: ${interest}. New balance: ${Balance}");
    }

    // Static method
    public static void UpdateInterestRate(decimal newRate)
    {
        _interestRate = newRate;
        Console.WriteLine($"Interest rate updated to {newRate * 100}%");
    }

    // Static method
    public static decimal GetTotalBalance()
    {
        decimal total = 0;
        foreach (var account in _allAccounts)
        {
            total += account.Balance;
        }
        return total;
    }

    // Static method
    public static void DisplayAllAccounts()
    {
        Console.WriteLine($"Total Accounts: {TotalAccounts}");
        Console.WriteLine($"Current Interest Rate: {InterestRate * 100}%");
        Console.WriteLine($"Total Balance Across All Accounts: ${GetTotalBalance():N2}");
        Console.WriteLine("\nAccount Details:");
        foreach (var account in _allAccounts)
        {
            Console.WriteLine($"  {account.AccountNumber}: ${account.Balance:N2}");
        }
    }
}

// Usage
var account1 = new BankAccount(1000m);
var account2 = new BankAccount(2000m);
var account3 = new BankAccount(1500m);

account1.Deposit(500m);
account2.ApplyInterest();

// Access static members through class name
Console.WriteLine($"Total Accounts: {BankAccount.TotalAccounts}");
Console.WriteLine($"Interest Rate: {BankAccount.InterestRate * 100}%");

BankAccount.UpdateInterestRate(0.06m);
BankAccount.DisplayAllAccounts();
```

### Static Classes

```csharp
// Static class - cannot be instantiated, can only contain static members
public static class MathUtilities
{
    // Static field
    public static double PI = 3.14159265359;

    // Static method
    public static double CalculateCircleArea(double radius)
    {
        return PI * radius * radius;
    }

    public static double CalculateCircleCircumference(double radius)
    {
        return 2 * PI * radius;
    }

    public static int Factorial(int n)
    {
        if (n <= 1) return 1;
        return n * Factorial(n - 1);
    }

    public static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        for (int i = 3; i <= Math.Sqrt(number); i += 2)
        {
            if (number % i == 0) return false;
        }
        return true;
    }
}

// Usage - No need to create instance
Console.WriteLine(MathUtilities.CalculateCircleArea(5));
Console.WriteLine(MathUtilities.Factorial(5));
Console.WriteLine(MathUtilities.IsPrime(17));

// var util = new MathUtilities(); // ❌ Compilation error: Cannot create instance of static class
```

### Static Constructor

```csharp
public class Configuration
{
    public static string ConnectionString { get; private set; }
    public static string ApiKey { get; private set; }
    public static DateTime InitializedAt { get; private set; }

    // Static constructor - called automatically before first use
    static Configuration()
    {
        Console.WriteLine("Static constructor called");

        // Load configuration
        ConnectionString = "Server=localhost;Database=MyDB;";
        ApiKey = "secret_key_12345";
        InitializedAt = DateTime.Now;

        Console.WriteLine("Configuration loaded");
    }

    public static void DisplayConfig()
    {
        Console.WriteLine($"Connection String: {ConnectionString}");
        Console.WriteLine($"API Key: {ApiKey}");
        Console.WriteLine($"Initialized At: {InitializedAt}");
    }
}

// Usage - Static constructor runs before first access
Console.WriteLine("Before accessing Configuration");
Configuration.DisplayConfig(); // Static constructor runs here
Console.WriteLine("After accessing Configuration");
Configuration.DisplayConfig(); // Static constructor doesn't run again
```

---

## 17. Properties and Indexers

### Auto-Implemented Properties

```csharp
public class Product
{
    // Auto-implemented properties
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    // Auto-property with default value
    public bool InStock { get; set; } = true;

    // Read-only auto-property (can only be set in constructor)
    public DateTime CreatedDate { get; } = DateTime.Now;

    // Property with different access levels
    public int Quantity { get; private set; }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity >= 0)
            Quantity = newQuantity;
    }
}
```

### Properties with Backing Fields

```csharp
public class Person
{
    // Backing field
    private string _name;
    private int _age;
    private string _email;

    // Property with validation
    public string Name
    {
        get { return _name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty");
            _name = value;
        }
    }

    // Property with validation
    public int Age
    {
        get { return _age; }
        set
        {
            if (value < 0 || value > 150)
                throw new ArgumentException("Invalid age");
            _age = value;
        }
    }

    // Property with transformation
    public string Email
    {
        get { return _email; }
        set
        {
            if (!value.Contains("@"))
                throw new ArgumentException("Invalid email");
            _email = value.ToLower();
        }
    }

    // Computed property (read-only)
    public bool IsAdult => _age >= 18;

    // Expression-bodied property
    public string DisplayName => $"{_name} ({_age})";
}
```

### Indexers

Indexers allow objects to be indexed like arrays.

```csharp
// Simple indexer
public class ShoppingCart
{
    private List<string> _items = new List<string>();

    // Indexer
    public string this[int index]
    {
        get
        {
            if (index < 0 || index >= _items.Count)
                throw new IndexOutOfRangeException();
            return _items[index];
        }
        set
        {
            if (index < 0 || index >= _items.Count)
                throw new IndexOutOfRangeException();
            _items[index] = value;
        }
    }

    public int Count => _items.Count;

    public void Add(string item)
    {
        _items.Add(item);
    }
}

// Usage
var cart = new ShoppingCart();
cart.Add("Apple");
cart.Add("Banana");
cart.Add("Orange");

Console.WriteLine(cart[0]); // Access by index: Apple
cart[1] = "Mango";          // Modify by index
Console.WriteLine(cart[1]); // Mango

// Multiple indexers with different types
public class DataStore
{
    private Dictionary<int, string> _dataById = new Dictionary<int, string>();
    private Dictionary<string, string> _dataByName = new Dictionary<string, string>();

    // Integer indexer
    public string this[int id]
    {
        get
        {
            return _dataById.ContainsKey(id) ? _dataById[id] : null;
        }
        set
        {
            _dataById[id] = value;
        }
    }

    // String indexer
    public string this[string name]
    {
        get
        {
            return _dataByName.ContainsKey(name) ? _dataByName[name] : null;
        }
        set
        {
            _dataByName[name] = value;
        }
    }

    // Multi-parameter indexer
    public string this[int row, int column]
    {
        get
        {
            string key = $"{row},{column}";
            return _dataByName.ContainsKey(key) ? _dataByName[key] : null;
        }
        set
        {
            string key = $"{row},{column}";
            _dataByName[key] = value;
        }
    }
}

// Usage
var store = new DataStore();

// Integer indexer
store[1] = "Data 1";
Console.WriteLine(store[1]);

// String indexer
store["name"] = "John";
Console.WriteLine(store["name"]);

// Multi-parameter indexer
store[0, 0] = "Cell A1";
store[0, 1] = "Cell B1";
Console.WriteLine(store[0, 0]);
```

---

## PART 4: C# SPECIFIC FEATURES

## 18. Extension Methods

Extension methods allow you to add methods to existing types without modifying them.

```csharp
// Extension methods must be in a static class
public static class StringExtensions
{
    // First parameter with 'this' keyword indicates the type being extended
    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return email.Contains("@") && email.Contains(".");
    }

    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.Length <= maxLength
            ? value
            : value.Substring(0, maxLength) + "...";
    }

    public static int WordCount(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        return text.Split(new[] { ' ', '\t', '\n' },
            StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

// Usage - appears as if part of string class
string email = "user@example.com";
Console.WriteLine(email.IsValidEmail()); // True

string longText = "This is a very long text that needs truncation";
Console.WriteLine(longText.Truncate(20)); // This is a very long...

string text = "The quick brown fox jumps";
Console.WriteLine(text.WordCount()); // 5
```

---

## PART 5: ADVANCED TOPICS

## 24. Generics

Generics provide type safety and code reusability.

```csharp
// Generic class
public class GenericRepository<T> where T : class
{
    private List<T> _items = new List<T>();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T Get(int index)
    {
        return _items[index];
    }

    public IEnumerable<T> GetAll()
    {
        return _items;
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }

    public int Count => _items.Count;
}

// Generic method
public class Utilities
{
    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    public static T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) > 0 ? a : b;
    }
}

// Usage
var productRepo = new GenericRepository<Product>();
productRepo.Add(new Product { Id = 1, Name = "Laptop" });

int x = 5, y = 10;
Utilities.Swap(ref x, ref y);
Console.WriteLine($"x={x}, y={y}"); // x=10, y=5

Console.WriteLine(Utilities.Max(10, 20));        // 20
Console.WriteLine(Utilities.Max("apple", "banana")); // banana
```

---

## PART 6: BEST PRACTICES & ANTI-PATTERNS

## 29. Common OOP Anti-Patterns

### God Object
```csharp
// ❌ BAD: God object does everything
public class Application
{
    public void ProcessUser() { }
    public void ProcessOrder() { }
    public void SendEmail() { }
    public void LogError() { }
    public void SaveToDatabase() { }
    // ... 50 more methods
}

// ✅ GOOD: Separate responsibilities
public class UserService { }
public class OrderService { }
public class EmailService { }
public class Logger { }
public class Repository { }
```

### Anemic Domain Model
```csharp
// ❌ BAD: Data class with no behavior
public class Order
{
    public decimal Total { get; set; }
    public string Status { get; set; }
}

public class OrderService
{
    public void ProcessOrder(Order order)
    {
        order.Status = "Processed";
    }
}

// ✅ GOOD: Behavior in the model
public class Order
{
    public decimal Total { get; private set; }
    public string Status { get; private set; }

    public void Process()
    {
        Status = "Processed";
    }

    public void Cancel()
    {
        if (Status == "Processed")
            throw new InvalidOperationException("Cannot cancel processed order");
        Status = "Cancelled";
    }
}
```

## 30. Best Practices

### Use Meaningful Names
```csharp
// ❌ BAD
public class Mgr
{
    public void Proc(int x) { }
}

// ✅ GOOD
public class CustomerManager
{
    public void ProcessCustomerOrder(int orderId) { }
}
```

### Program to Interfaces
```csharp
// ✅ GOOD
IRepository<Customer> repository = new SqlRepository<Customer>();
IEmailService emailService = new SmtpEmailService();
```

### Key Principles to Remember:
1. **Favor Composition Over Inheritance**
2. **Keep Classes Focused (SRP)**
3. **Use Dependency Injection**
4. **Write Self-Documenting Code**
5. **Follow SOLID Principles**
6. **Prefer Immutability When Possible**
7. **Use Interfaces for Abstraction**
8. **Keep Methods Small and Focused**

---

## Summary

This guide covered:
- **OOP Fundamentals**: Classes, Objects, Encapsulation, Abstraction, Inheritance, Polymorphism
- **SOLID Principles**: SRP, OCP, LSP, ISP, DIP
- **Advanced Concepts**: Abstract classes vs Interfaces, Composition, Overloading/Overriding
- **C# Features**: Extension methods, Properties, Static members, Generics
- **Best Practices**: Design patterns, anti-patterns, clean code principles

Master these concepts to write maintainable, scalable, and robust object-oriented code!
