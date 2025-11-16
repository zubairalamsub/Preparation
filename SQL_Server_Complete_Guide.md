# SQL Server Complete Guide
## Comprehensive Beginner to Advanced Database Topics

---

## Table of Contents
1. [SQL Server Fundamentals](#1-sql-server-fundamentals)
2. [Database Design & Normalization](#2-database-design--normalization)
3. [Basic SQL Queries](#3-basic-sql-queries)
4. [Intermediate Queries](#4-intermediate-queries)
5. [Advanced Queries](#5-advanced-queries)
6. [Joins in Depth](#6-joins-in-depth)
7. [Subqueries & CTEs](#7-subqueries--ctes)
8. [Stored Procedures](#8-stored-procedures)
9. [Functions](#9-functions)
10. [Triggers](#10-triggers)
11. [Views](#11-views)
12. [Indexes](#12-indexes)
13. [Transactions & Concurrency](#13-transactions--concurrency)
14. [Window Functions](#14-window-functions)
15. [Performance Tuning](#15-performance-tuning)
16. [Security](#16-security)
17. [Backup & Recovery](#17-backup--recovery)
18. [Advanced Topics](#18-advanced-topics)
19. [Interview Questions](#19-interview-questions)
20. [Practice Exercises](#20-practice-exercises)

---

## 1. SQL Server Fundamentals

### 1.1 What is SQL Server?
- Relational Database Management System (RDBMS) by Microsoft
- Used for storing and retrieving data
- Supports T-SQL (Transact-SQL)

### 1.2 Key Components
```
Database Engine    - Core service for storing/processing data
SQL Server Agent   - Job scheduling and automation
SSMS              - SQL Server Management Studio (GUI)
SQLCMD            - Command-line tool
SSIS              - Integration Services (ETL)
SSRS              - Reporting Services
SSAS              - Analysis Services
```

### 1.3 Data Types

#### Numeric Types
```sql
-- Exact Numeric
INT              -- -2,147,483,648 to 2,147,483,647
BIGINT           -- Very large integers
SMALLINT         -- -32,768 to 32,767
TINYINT          -- 0 to 255
DECIMAL(p,s)     -- Fixed precision (p=precision, s=scale)
NUMERIC(p,s)     -- Same as DECIMAL
MONEY            -- -922,337,203,685,477.5808 to 922,337,203,685,477.5807
SMALLMONEY       -- -214,748.3648 to 214,748.3647
BIT              -- 0, 1, or NULL

-- Approximate Numeric
FLOAT            -- -1.79E+308 to 1.79E+308
REAL             -- -3.40E+38 to 3.40E+38
```

#### String Types
```sql
-- Fixed Length
CHAR(n)          -- Fixed length, max 8,000 chars
NCHAR(n)         -- Unicode fixed length, max 4,000 chars

-- Variable Length
VARCHAR(n)       -- Variable length, max 8,000 chars
VARCHAR(MAX)     -- Variable length, max 2GB
NVARCHAR(n)      -- Unicode variable, max 4,000 chars
NVARCHAR(MAX)    -- Unicode variable, max 2GB
TEXT             -- Deprecated, use VARCHAR(MAX)
NTEXT            -- Deprecated, use NVARCHAR(MAX)
```

#### Date and Time
```sql
DATE             -- Date only (YYYY-MM-DD)
TIME             -- Time only (HH:MM:SS.nnnnnnn)
DATETIME         -- Date and time (1753-9999)
DATETIME2        -- Extended datetime (0001-9999)
SMALLDATETIME    -- Date and time (1900-2079)
DATETIMEOFFSET   -- Date, time with timezone
TIMESTAMP        -- Unique binary number
```

#### Binary Types
```sql
BINARY(n)        -- Fixed length binary
VARBINARY(n)     -- Variable length binary
VARBINARY(MAX)   -- Variable length binary, max 2GB
IMAGE            -- Deprecated, use VARBINARY(MAX)
```

#### Other Types
```sql
UNIQUEIDENTIFIER -- GUID (Globally Unique Identifier)
XML              -- XML data
CURSOR           -- Reference to cursor
TABLE            -- Result set for stored procedures
SQL_VARIANT      -- Stores values of various data types
```

### 1.4 Creating Database
```sql
-- Create Database
CREATE DATABASE CompanyDB;

-- Create Database with Options
CREATE DATABASE CompanyDB
ON PRIMARY
(
    NAME = CompanyDB_Data,
    FILENAME = 'C:\SQLData\CompanyDB.mdf',
    SIZE = 10MB,
    MAXSIZE = 100MB,
    FILEGROWTH = 5MB
)
LOG ON
(
    NAME = CompanyDB_Log,
    FILENAME = 'C:\SQLData\CompanyDB.ldf',
    SIZE = 5MB,
    MAXSIZE = 50MB,
    FILEGROWTH = 5MB
);

-- Use Database
USE CompanyDB;

-- Drop Database
DROP DATABASE CompanyDB;

-- Check existing databases
SELECT name FROM sys.databases;
```

---

## 2. Database Design & Normalization

### 2.1 Normalization Forms

#### First Normal Form (1NF)
```
Rules:
- Each column contains atomic values
- Each column contains values of same type
- Each column has unique name
- Order doesn't matter

Bad Example:
StudentID | Name  | Courses
1         | John  | Math, Science, English

Good Example:
StudentID | Name  | Course
1         | John  | Math
1         | John  | Science
1         | John  | English
```

#### Second Normal Form (2NF)
```
Rules:
- Must be in 1NF
- No partial dependencies (all non-key attributes fully dependent on primary key)

Bad Example:
OrderID | ProductID | ProductName | Quantity
1       | 101       | Laptop      | 2
1       | 102       | Mouse       | 5

Good Example:
Orders Table:
OrderID | ProductID | Quantity
1       | 101       | 2
1       | 102       | 5

Products Table:
ProductID | ProductName
101       | Laptop
102       | Mouse
```

#### Third Normal Form (3NF)
```
Rules:
- Must be in 2NF
- No transitive dependencies

Bad Example:
EmployeeID | Name | DeptID | DeptName
1          | John | 10     | IT
2          | Jane | 20     | HR

Good Example:
Employees Table:
EmployeeID | Name | DeptID
1          | John | 10
2          | Jane | 20

Departments Table:
DeptID | DeptName
10     | IT
20     | HR
```

#### Boyce-Codd Normal Form (BCNF)
```
Rules:
- Must be in 3NF
- For every dependency A → B, A should be a super key
```

### 2.2 Database Schema Design
```sql
-- Create Tables with Relationships

-- Departments Table
CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(50) NOT NULL UNIQUE,
    Location NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Employees Table
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE,
    Phone NVARCHAR(20),
    HireDate DATE NOT NULL,
    JobTitle NVARCHAR(50),
    Salary DECIMAL(10,2) CHECK (Salary > 0),
    DepartmentID INT,
    ManagerID INT,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),

    -- Foreign Keys
    CONSTRAINT FK_Emp_Dept FOREIGN KEY (DepartmentID)
        REFERENCES Departments(DepartmentID),
    CONSTRAINT FK_Emp_Manager FOREIGN KEY (ManagerID)
        REFERENCES Employees(EmployeeID)
);

-- Projects Table
CREATE TABLE Projects (
    ProjectID INT PRIMARY KEY IDENTITY(1,1),
    ProjectName NVARCHAR(100) NOT NULL,
    StartDate DATE,
    EndDate DATE,
    Budget DECIMAL(15,2),
    DepartmentID INT,
    Status NVARCHAR(20) DEFAULT 'Active',

    CONSTRAINT FK_Proj_Dept FOREIGN KEY (DepartmentID)
        REFERENCES Departments(DepartmentID),
    CONSTRAINT CHK_Dates CHECK (EndDate >= StartDate)
);

-- Employee_Projects (Many-to-Many)
CREATE TABLE EmployeeProjects (
    EmployeeID INT,
    ProjectID INT,
    Role NVARCHAR(50),
    HoursAllocated DECIMAL(5,2),

    PRIMARY KEY (EmployeeID, ProjectID),
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID)
);

-- Customers Table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    CompanyName NVARCHAR(100) NOT NULL,
    ContactName NVARCHAR(50),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(200),
    City NVARCHAR(50),
    State NVARCHAR(50),
    ZipCode NVARCHAR(10),
    Country NVARCHAR(50)
);

-- Orders Table
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    ShippedDate DATETIME,
    TotalAmount DECIMAL(10,2),
    Status NVARCHAR(20) DEFAULT 'Pending',

    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Products Table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    CategoryID INT,
    SupplierID INT,
    UnitPrice DECIMAL(10,2) NOT NULL,
    UnitsInStock INT DEFAULT 0,
    ReorderLevel INT DEFAULT 10,
    Discontinued BIT DEFAULT 0
);

-- OrderDetails Table
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    Discount DECIMAL(3,2) DEFAULT 0,

    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
```

---

## 3. Basic SQL Queries

### 3.1 SELECT Statement
```sql
-- Select all columns
SELECT * FROM Employees;

-- Select specific columns
SELECT FirstName, LastName, Email FROM Employees;

-- Select with alias
SELECT
    FirstName AS 'First Name',
    LastName AS 'Last Name',
    Salary AS 'Annual Salary'
FROM Employees;

-- Select distinct values
SELECT DISTINCT DepartmentID FROM Employees;

-- Select TOP
SELECT TOP 10 * FROM Employees;
SELECT TOP 10 PERCENT * FROM Employees;

-- Select with calculations
SELECT
    FirstName,
    LastName,
    Salary,
    Salary * 12 AS AnnualSalary,
    Salary * 0.1 AS Bonus
FROM Employees;
```

### 3.2 WHERE Clause
```sql
-- Basic filtering
SELECT * FROM Employees WHERE DepartmentID = 10;

-- Multiple conditions
SELECT * FROM Employees
WHERE DepartmentID = 10 AND Salary > 50000;

SELECT * FROM Employees
WHERE DepartmentID = 10 OR DepartmentID = 20;

-- NOT operator
SELECT * FROM Employees WHERE NOT DepartmentID = 10;

-- IN operator
SELECT * FROM Employees
WHERE DepartmentID IN (10, 20, 30);

-- BETWEEN operator
SELECT * FROM Employees
WHERE Salary BETWEEN 40000 AND 80000;

-- LIKE operator
SELECT * FROM Employees WHERE FirstName LIKE 'J%';     -- Starts with J
SELECT * FROM Employees WHERE FirstName LIKE '%n';     -- Ends with n
SELECT * FROM Employees WHERE FirstName LIKE '%oh%';   -- Contains 'oh'
SELECT * FROM Employees WHERE FirstName LIKE 'J_hn';   -- _ for single char

-- IS NULL / IS NOT NULL
SELECT * FROM Employees WHERE ManagerID IS NULL;
SELECT * FROM Employees WHERE Email IS NOT NULL;
```

### 3.3 ORDER BY
```sql
-- Ascending (default)
SELECT * FROM Employees ORDER BY LastName;
SELECT * FROM Employees ORDER BY LastName ASC;

-- Descending
SELECT * FROM Employees ORDER BY Salary DESC;

-- Multiple columns
SELECT * FROM Employees
ORDER BY DepartmentID ASC, Salary DESC;

-- Order by column position
SELECT FirstName, LastName, Salary
FROM Employees
ORDER BY 3 DESC;  -- Order by 3rd column (Salary)
```

### 3.4 INSERT
```sql
-- Insert single row
INSERT INTO Departments (DepartmentName, Location)
VALUES ('IT', 'New York');

-- Insert multiple rows
INSERT INTO Departments (DepartmentName, Location)
VALUES
    ('HR', 'Chicago'),
    ('Sales', 'Boston'),
    ('Marketing', 'Seattle');

-- Insert with all columns (no column list needed)
INSERT INTO Departments
VALUES ('Finance', 'Dallas', GETDATE());

-- Insert from SELECT
INSERT INTO ArchivedEmployees
SELECT * FROM Employees WHERE HireDate < '2010-01-01';

-- Insert with IDENTITY column
INSERT INTO Employees (FirstName, LastName, Email, HireDate, JobTitle, Salary)
VALUES ('John', 'Doe', 'john.doe@example.com', '2023-01-15', 'Developer', 75000);
```

### 3.5 UPDATE
```sql
-- Update single column
UPDATE Employees
SET Salary = 80000
WHERE EmployeeID = 1;

-- Update multiple columns
UPDATE Employees
SET Salary = 85000, JobTitle = 'Senior Developer'
WHERE EmployeeID = 1;

-- Update with calculation
UPDATE Employees
SET Salary = Salary * 1.1
WHERE DepartmentID = 10;

-- Update all rows (be careful!)
UPDATE Products
SET Discontinued = 0;

-- Update with subquery
UPDATE Employees
SET Salary = (SELECT AVG(Salary) FROM Employees)
WHERE EmployeeID = 1;
```

### 3.6 DELETE
```sql
-- Delete specific rows
DELETE FROM Employees WHERE EmployeeID = 100;

-- Delete with multiple conditions
DELETE FROM Employees
WHERE DepartmentID = 10 AND HireDate < '2010-01-01';

-- Delete all rows (be careful!)
DELETE FROM TempTable;

-- Truncate (faster, resets identity)
TRUNCATE TABLE TempTable;
```

### 3.7 Aggregate Functions
```sql
-- COUNT
SELECT COUNT(*) AS TotalEmployees FROM Employees;
SELECT COUNT(DISTINCT DepartmentID) AS UniqueDepartments FROM Employees;

-- SUM
SELECT SUM(Salary) AS TotalSalaries FROM Employees;

-- AVG
SELECT AVG(Salary) AS AverageSalary FROM Employees;

-- MIN and MAX
SELECT MIN(Salary) AS MinSalary FROM Employees;
SELECT MAX(Salary) AS MaxSalary FROM Employees;

-- Multiple aggregates
SELECT
    COUNT(*) AS TotalEmployees,
    AVG(Salary) AS AvgSalary,
    MIN(Salary) AS MinSalary,
    MAX(Salary) AS MaxSalary,
    SUM(Salary) AS TotalSalaries
FROM Employees;
```

### 3.8 GROUP BY
```sql
-- Group by single column
SELECT DepartmentID, COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentID;

-- Group by multiple columns
SELECT DepartmentID, JobTitle, COUNT(*) AS Count
FROM Employees
GROUP BY DepartmentID, JobTitle;

-- Group by with aggregate functions
SELECT
    DepartmentID,
    COUNT(*) AS EmployeeCount,
    AVG(Salary) AS AvgSalary,
    MAX(Salary) AS MaxSalary
FROM Employees
GROUP BY DepartmentID;

-- HAVING clause (filter groups)
SELECT DepartmentID, COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentID
HAVING COUNT(*) > 5;

SELECT DepartmentID, AVG(Salary) AS AvgSalary
FROM Employees
GROUP BY DepartmentID
HAVING AVG(Salary) > 60000;

-- WHERE vs HAVING
SELECT DepartmentID, COUNT(*) AS EmployeeCount
FROM Employees
WHERE IsActive = 1  -- Filter rows before grouping
GROUP BY DepartmentID
HAVING COUNT(*) > 5;  -- Filter groups after grouping
```

---

## 4. Intermediate Queries

### 4.1 String Functions
```sql
-- LEN - Length of string
SELECT FirstName, LEN(FirstName) AS NameLength FROM Employees;

-- UPPER / LOWER
SELECT UPPER(FirstName) AS UpperName FROM Employees;
SELECT LOWER(Email) AS LowerEmail FROM Employees;

-- SUBSTRING
SELECT SUBSTRING(FirstName, 1, 3) AS FirstThree FROM Employees;

-- LEFT / RIGHT
SELECT LEFT(FirstName, 3) AS LeftThree FROM Employees;
SELECT RIGHT(Phone, 4) AS LastFourDigits FROM Employees;

-- LTRIM / RTRIM / TRIM
SELECT LTRIM('  Hello  ') AS LeftTrimmed;
SELECT RTRIM('  Hello  ') AS RightTrimmed;
SELECT TRIM('  Hello  ') AS Trimmed;

-- REPLACE
SELECT REPLACE(Phone, '-', '.') AS ModifiedPhone FROM Employees;

-- CONCAT
SELECT CONCAT(FirstName, ' ', LastName) AS FullName FROM Employees;
SELECT FirstName + ' ' + LastName AS FullName FROM Employees;  -- Alternative

-- CHARINDEX / PATINDEX
SELECT CHARINDEX('@', Email) AS AtPosition FROM Employees;
SELECT PATINDEX('%@%.com', Email) AS PatternPosition FROM Employees;

-- STUFF
SELECT STUFF('Hello World', 7, 5, 'SQL') AS Result;  -- 'Hello SQL'

-- REVERSE
SELECT REVERSE(FirstName) AS ReversedName FROM Employees;

-- FORMAT
SELECT FORMAT(GETDATE(), 'yyyy-MM-dd') AS FormattedDate;
SELECT FORMAT(Salary, 'C', 'en-US') AS FormattedSalary FROM Employees;
```

### 4.2 Date Functions
```sql
-- GETDATE / SYSDATETIME
SELECT GETDATE() AS CurrentDateTime;
SELECT SYSDATETIME() AS CurrentDateTimeWithPrecision;

-- DATEPART
SELECT DATEPART(YEAR, HireDate) AS HireYear FROM Employees;
SELECT DATEPART(MONTH, HireDate) AS HireMonth FROM Employees;
SELECT DATEPART(DAY, HireDate) AS HireDay FROM Employees;

-- YEAR / MONTH / DAY
SELECT YEAR(HireDate) AS HireYear FROM Employees;
SELECT MONTH(HireDate) AS HireMonth FROM Employees;
SELECT DAY(HireDate) AS HireDay FROM Employees;

-- DATEADD
SELECT DATEADD(YEAR, 1, HireDate) AS OneYearLater FROM Employees;
SELECT DATEADD(MONTH, 6, HireDate) AS SixMonthsLater FROM Employees;
SELECT DATEADD(DAY, -30, GETDATE()) AS ThirtyDaysAgo;

-- DATEDIFF
SELECT DATEDIFF(YEAR, HireDate, GETDATE()) AS YearsEmployed FROM Employees;
SELECT DATEDIFF(DAY, OrderDate, ShippedDate) AS DaysToShip FROM Orders;

-- EOMONTH (End of Month)
SELECT EOMONTH(GETDATE()) AS LastDayOfMonth;

-- DATEFROMPARTS
SELECT DATEFROMPARTS(2023, 12, 25) AS ChristmasDay;

-- FORMAT
SELECT FORMAT(GETDATE(), 'dd/MM/yyyy') AS FormattedDate;
SELECT FORMAT(GETDATE(), 'MMMM dd, yyyy') AS LongDate;
```

### 4.3 Mathematical Functions
```sql
-- ROUND
SELECT ROUND(123.456, 2) AS Rounded;  -- 123.460
SELECT ROUND(123.456, 1) AS Rounded;  -- 123.500

-- CEILING / FLOOR
SELECT CEILING(123.45) AS RoundedUp;   -- 124
SELECT FLOOR(123.45) AS RoundedDown;   -- 123

-- ABS (Absolute value)
SELECT ABS(-100) AS AbsoluteValue;  -- 100

-- POWER / SQRT
SELECT POWER(2, 3) AS PowerResult;  -- 8
SELECT SQRT(16) AS SquareRoot;      -- 4

-- RAND (Random number)
SELECT RAND() AS RandomNumber;
SELECT FLOOR(RAND() * 100) AS RandomInt;  -- Random int 0-99

-- PI
SELECT PI() AS PiValue;  -- 3.14159265358979

-- SIN, COS, TAN
SELECT SIN(PI()/2) AS SineValue;
```

### 4.4 Conversion Functions
```sql
-- CAST
SELECT CAST('123' AS INT) AS IntValue;
SELECT CAST(123.456 AS INT) AS IntValue;  -- 123
SELECT CAST(GETDATE() AS DATE) AS DateOnly;

-- CONVERT
SELECT CONVERT(INT, '123') AS IntValue;
SELECT CONVERT(VARCHAR(10), GETDATE(), 101) AS USDate;  -- MM/DD/YYYY
SELECT CONVERT(VARCHAR(10), GETDATE(), 103) AS UKDate;  -- DD/MM/YYYY

-- TRY_CAST / TRY_CONVERT (returns NULL on error)
SELECT TRY_CAST('ABC' AS INT) AS Result;  -- NULL
SELECT TRY_CONVERT(INT, '123ABC') AS Result;  -- NULL

-- STR (Number to string)
SELECT STR(123.456, 6, 2) AS StringValue;  -- '123.46'
```

### 4.5 NULL Handling
```sql
-- ISNULL
SELECT FirstName, ISNULL(ManagerID, 0) AS ManagerID FROM Employees;

-- COALESCE (returns first non-null)
SELECT FirstName, COALESCE(Phone, Email, 'No Contact') AS Contact
FROM Employees;

-- NULLIF (returns NULL if equal)
SELECT NULLIF(10, 10) AS Result;  -- NULL
SELECT NULLIF(10, 20) AS Result;  -- 10
```

### 4.6 CASE Expressions
```sql
-- Simple CASE
SELECT
    FirstName,
    LastName,
    CASE DepartmentID
        WHEN 10 THEN 'IT'
        WHEN 20 THEN 'HR'
        WHEN 30 THEN 'Sales'
        ELSE 'Other'
    END AS DepartmentName
FROM Employees;

-- Searched CASE
SELECT
    FirstName,
    LastName,
    Salary,
    CASE
        WHEN Salary < 50000 THEN 'Low'
        WHEN Salary BETWEEN 50000 AND 80000 THEN 'Medium'
        WHEN Salary > 80000 THEN 'High'
        ELSE 'Unknown'
    END AS SalaryRange
FROM Employees;

-- CASE in ORDER BY
SELECT FirstName, LastName, DepartmentID
FROM Employees
ORDER BY
    CASE
        WHEN DepartmentID = 10 THEN 1
        WHEN DepartmentID = 20 THEN 2
        ELSE 3
    END;

-- CASE in aggregate
SELECT
    DepartmentID,
    COUNT(*) AS TotalEmployees,
    SUM(CASE WHEN Salary > 70000 THEN 1 ELSE 0 END) AS HighEarners
FROM Employees
GROUP BY DepartmentID;
```

### 4.7 IIF Function (SQL Server 2012+)
```sql
-- IIF (shorthand for simple CASE)
SELECT
    FirstName,
    LastName,
    IIF(Salary > 70000, 'High', 'Normal') AS SalaryLevel
FROM Employees;

SELECT
    ProductName,
    UnitsInStock,
    IIF(UnitsInStock < ReorderLevel, 'Reorder', 'OK') AS Status
FROM Products;
```

---

## 5. Advanced Queries

### 5.1 SET Operators
```sql
-- UNION (removes duplicates)
SELECT FirstName, LastName FROM Employees
UNION
SELECT ContactName, ContactName FROM Customers;

-- UNION ALL (keeps duplicates)
SELECT City FROM Employees
UNION ALL
SELECT City FROM Customers;

-- INTERSECT (common records)
SELECT City FROM Employees
INTERSECT
SELECT City FROM Customers;

-- EXCEPT (in first but not in second)
SELECT City FROM Employees
EXCEPT
SELECT City FROM Customers;
```

### 5.2 Pivoting Data
```sql
-- PIVOT
SELECT * FROM
(
    SELECT DepartmentID, JobTitle, Salary
    FROM Employees
) AS SourceTable
PIVOT
(
    AVG(Salary)
    FOR JobTitle IN ([Developer], [Manager], [Analyst])
) AS PivotTable;

-- Dynamic PIVOT
DECLARE @cols NVARCHAR(MAX), @query NVARCHAR(MAX);

SELECT @cols = STRING_AGG(QUOTENAME(JobTitle), ',')
FROM (SELECT DISTINCT JobTitle FROM Employees) AS Jobs;

SET @query = '
SELECT * FROM
(
    SELECT DepartmentID, JobTitle, Salary
    FROM Employees
) AS SourceTable
PIVOT
(
    AVG(Salary)
    FOR JobTitle IN (' + @cols + ')
) AS PivotTable';

EXEC(@query);

-- UNPIVOT
SELECT Department, Quarter, Sales
FROM QuarterlySales
UNPIVOT
(
    Sales FOR Quarter IN (Q1, Q2, Q3, Q4)
) AS UnpivotTable;
```

### 5.3 Temporary Tables
```sql
-- Local Temporary Table (visible only to current session)
CREATE TABLE #TempEmployees (
    EmployeeID INT,
    FullName NVARCHAR(100),
    Salary DECIMAL(10,2)
);

INSERT INTO #TempEmployees
SELECT EmployeeID, FirstName + ' ' + LastName, Salary
FROM Employees;

SELECT * FROM #TempEmployees;

DROP TABLE #TempEmployees;

-- Global Temporary Table (visible to all sessions)
CREATE TABLE ##GlobalTemp (
    ID INT,
    Value NVARCHAR(100)
);

-- Table Variable
DECLARE @EmployeeTable TABLE (
    EmployeeID INT,
    FullName NVARCHAR(100),
    Salary DECIMAL(10,2)
);

INSERT INTO @EmployeeTable
SELECT EmployeeID, FirstName + ' ' + LastName, Salary
FROM Employees;

SELECT * FROM @EmployeeTable;
```

### 5.4 Ranking Functions
```sql
-- ROW_NUMBER
SELECT
    ROW_NUMBER() OVER (ORDER BY Salary DESC) AS RowNum,
    FirstName,
    LastName,
    Salary
FROM Employees;

-- ROW_NUMBER with PARTITION
SELECT
    ROW_NUMBER() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS RowNum,
    DepartmentID,
    FirstName,
    LastName,
    Salary
FROM Employees;

-- RANK (gaps in ranking for ties)
SELECT
    RANK() OVER (ORDER BY Salary DESC) AS Rank,
    FirstName,
    LastName,
    Salary
FROM Employees;

-- DENSE_RANK (no gaps)
SELECT
    DENSE_RANK() OVER (ORDER BY Salary DESC) AS DenseRank,
    FirstName,
    LastName,
    Salary
FROM Employees;

-- NTILE (divide into N groups)
SELECT
    NTILE(4) OVER (ORDER BY Salary) AS Quartile,
    FirstName,
    LastName,
    Salary
FROM Employees;
```

---

## 6. Joins in Depth

### 6.1 INNER JOIN
```sql
-- Basic INNER JOIN
SELECT
    e.FirstName,
    e.LastName,
    d.DepartmentName
FROM Employees e
INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID;

-- Multiple INNER JOINs
SELECT
    e.FirstName,
    e.LastName,
    d.DepartmentName,
    p.ProjectName
FROM Employees e
INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
INNER JOIN EmployeeProjects ep ON e.EmployeeID = ep.EmployeeID
INNER JOIN Projects p ON ep.ProjectID = p.ProjectID;
```

### 6.2 LEFT JOIN (LEFT OUTER JOIN)
```sql
-- LEFT JOIN (returns all from left table)
SELECT
    e.FirstName,
    e.LastName,
    d.DepartmentName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID;

-- Find employees without departments
SELECT
    e.FirstName,
    e.LastName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE d.DepartmentID IS NULL;
```

### 6.3 RIGHT JOIN (RIGHT OUTER JOIN)
```sql
-- RIGHT JOIN (returns all from right table)
SELECT
    e.FirstName,
    e.LastName,
    d.DepartmentName
FROM Employees e
RIGHT JOIN Departments d ON e.DepartmentID = d.DepartmentID;

-- Find departments without employees
SELECT d.DepartmentName
FROM Employees e
RIGHT JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE e.EmployeeID IS NULL;
```

### 6.4 FULL OUTER JOIN
```sql
-- FULL OUTER JOIN (returns all from both tables)
SELECT
    e.FirstName,
    e.LastName,
    d.DepartmentName
FROM Employees e
FULL OUTER JOIN Departments d ON e.DepartmentID = d.DepartmentID;
```

### 6.5 CROSS JOIN
```sql
-- CROSS JOIN (Cartesian product)
SELECT
    e.FirstName,
    p.ProductName
FROM Employees e
CROSS JOIN Products p;

-- Practical use: Generate date ranges
SELECT
    d.Date,
    e.EmployeeName
FROM DateDimension d
CROSS JOIN Employees e
WHERE d.Date BETWEEN '2023-01-01' AND '2023-12-31';
```

### 6.6 SELF JOIN
```sql
-- Self JOIN (join table to itself)
SELECT
    e.FirstName + ' ' + e.LastName AS Employee,
    m.FirstName + ' ' + m.LastName AS Manager
FROM Employees e
LEFT JOIN Employees m ON e.ManagerID = m.EmployeeID;

-- Find employees in same department
SELECT
    e1.FirstName AS Employee1,
    e2.FirstName AS Employee2,
    e1.DepartmentID
FROM Employees e1
INNER JOIN Employees e2 ON e1.DepartmentID = e2.DepartmentID
WHERE e1.EmployeeID < e2.EmployeeID;
```

### 6.7 JOIN with Aggregates
```sql
-- Count employees per department
SELECT
    d.DepartmentName,
    COUNT(e.EmployeeID) AS EmployeeCount,
    AVG(e.Salary) AS AvgSalary
FROM Departments d
LEFT JOIN Employees e ON d.DepartmentID = e.DepartmentID
GROUP BY d.DepartmentName;

-- Orders with total amounts
SELECT
    o.OrderID,
    o.OrderDate,
    SUM(od.Quantity * od.UnitPrice) AS TotalAmount
FROM Orders o
INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
GROUP BY o.OrderID, o.OrderDate;
```

---

## 7. Subqueries & CTEs

### 7.1 Scalar Subqueries
```sql
-- Subquery returning single value
SELECT
    FirstName,
    LastName,
    Salary,
    (SELECT AVG(Salary) FROM Employees) AS AvgSalary
FROM Employees;

-- Subquery in WHERE
SELECT FirstName, LastName, Salary
FROM Employees
WHERE Salary > (SELECT AVG(Salary) FROM Employees);
```

### 7.2 Multi-Row Subqueries
```sql
-- IN operator
SELECT FirstName, LastName
FROM Employees
WHERE DepartmentID IN (
    SELECT DepartmentID
    FROM Departments
    WHERE Location = 'New York'
);

-- ANY / ALL
SELECT ProductName, UnitPrice
FROM Products
WHERE UnitPrice > ALL (
    SELECT UnitPrice FROM Products WHERE CategoryID = 1
);

SELECT ProductName, UnitPrice
FROM Products
WHERE UnitPrice > ANY (
    SELECT UnitPrice FROM Products WHERE CategoryID = 1
);
```

### 7.3 Correlated Subqueries
```sql
-- Subquery referencing outer query
SELECT
    e1.FirstName,
    e1.LastName,
    e1.Salary
FROM Employees e1
WHERE Salary > (
    SELECT AVG(Salary)
    FROM Employees e2
    WHERE e2.DepartmentID = e1.DepartmentID
);

-- EXISTS
SELECT DepartmentName
FROM Departments d
WHERE EXISTS (
    SELECT 1
    FROM Employees e
    WHERE e.DepartmentID = d.DepartmentID
);

-- NOT EXISTS (departments with no employees)
SELECT DepartmentName
FROM Departments d
WHERE NOT EXISTS (
    SELECT 1
    FROM Employees e
    WHERE e.DepartmentID = d.DepartmentID
);
```

### 7.4 Derived Tables
```sql
-- Subquery in FROM clause
SELECT
    DeptSalary.DepartmentID,
    DeptSalary.AvgSalary
FROM (
    SELECT
        DepartmentID,
        AVG(Salary) AS AvgSalary
    FROM Employees
    GROUP BY DepartmentID
) AS DeptSalary
WHERE DeptSalary.AvgSalary > 60000;
```

### 7.5 Common Table Expressions (CTEs)
```sql
-- Basic CTE
WITH EmployeeCTE AS (
    SELECT
        EmployeeID,
        FirstName,
        LastName,
        Salary,
        DepartmentID
    FROM Employees
    WHERE IsActive = 1
)
SELECT * FROM EmployeeCTE WHERE Salary > 50000;

-- Multiple CTEs
WITH
DeptAvg AS (
    SELECT
        DepartmentID,
        AVG(Salary) AS AvgSalary
    FROM Employees
    GROUP BY DepartmentID
),
HighEarners AS (
    SELECT
        e.FirstName,
        e.LastName,
        e.Salary,
        e.DepartmentID
    FROM Employees e
    WHERE e.Salary > 70000
)
SELECT
    h.FirstName,
    h.LastName,
    h.Salary,
    d.AvgSalary
FROM HighEarners h
INNER JOIN DeptAvg d ON h.DepartmentID = d.DepartmentID;

-- Recursive CTE (Employee Hierarchy)
WITH EmployeeHierarchy AS (
    -- Anchor member
    SELECT
        EmployeeID,
        FirstName,
        LastName,
        ManagerID,
        1 AS Level
    FROM Employees
    WHERE ManagerID IS NULL

    UNION ALL

    -- Recursive member
    SELECT
        e.EmployeeID,
        e.FirstName,
        e.LastName,
        e.ManagerID,
        eh.Level + 1
    FROM Employees e
    INNER JOIN EmployeeHierarchy eh ON e.ManagerID = eh.EmployeeID
)
SELECT * FROM EmployeeHierarchy;

-- Recursive CTE (Number series)
WITH NumberSeries AS (
    SELECT 1 AS Number
    UNION ALL
    SELECT Number + 1
    FROM NumberSeries
    WHERE Number < 100
)
SELECT * FROM NumberSeries;
```

---

## 8. Stored Procedures

### 8.1 Basic Stored Procedure
```sql
-- Create simple stored procedure
CREATE PROCEDURE GetAllEmployees
AS
BEGIN
    SELECT * FROM Employees;
END;

-- Execute
EXEC GetAllEmployees;
-- OR
EXECUTE GetAllEmployees;

-- Drop
DROP PROCEDURE GetAllEmployees;
```

### 8.2 Stored Procedure with Parameters
```sql
-- Input parameters
CREATE PROCEDURE GetEmployeesByDept
    @DepartmentID INT
AS
BEGIN
    SELECT * FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;

-- Execute with parameter
EXEC GetEmployeesByDept @DepartmentID = 10;

-- Multiple parameters
CREATE PROCEDURE GetEmployeesBySalaryRange
    @MinSalary DECIMAL(10,2),
    @MaxSalary DECIMAL(10,2)
AS
BEGIN
    SELECT FirstName, LastName, Salary
    FROM Employees
    WHERE Salary BETWEEN @MinSalary AND @MaxSalary
    ORDER BY Salary DESC;
END;

EXEC GetEmployeesBySalaryRange @MinSalary = 50000, @MaxSalary = 80000;

-- Default parameter values
CREATE PROCEDURE GetEmployeesByStatus
    @IsActive BIT = 1  -- Default value
AS
BEGIN
    SELECT * FROM Employees
    WHERE IsActive = @IsActive;
END;

EXEC GetEmployeesByStatus;  -- Uses default
EXEC GetEmployeesByStatus @IsActive = 0;
```

### 8.3 Output Parameters
```sql
CREATE PROCEDURE GetEmployeeCount
    @DepartmentID INT,
    @EmployeeCount INT OUTPUT
AS
BEGIN
    SELECT @EmployeeCount = COUNT(*)
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;

-- Execute with OUTPUT
DECLARE @Count INT;
EXEC GetEmployeeCount @DepartmentID = 10, @EmployeeCount = @Count OUTPUT;
SELECT @Count AS TotalEmployees;
```

### 8.4 Return Values
```sql
CREATE PROCEDURE CheckEmployeeExists
    @EmployeeID INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Employees WHERE EmployeeID = @EmployeeID)
        RETURN 1;
    ELSE
        RETURN 0;
END;

-- Execute and capture return value
DECLARE @ReturnValue INT;
EXEC @ReturnValue = CheckEmployeeExists @EmployeeID = 100;
SELECT @ReturnValue AS Exists;
```

### 8.5 Error Handling in Stored Procedures
```sql
CREATE PROCEDURE InsertEmployee
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @DepartmentID INT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Employees (FirstName, LastName, Email, DepartmentID, HireDate)
        VALUES (@FirstName, @LastName, @Email, @DepartmentID, GETDATE());

        SELECT 'Employee inserted successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT
            ERROR_NUMBER() AS ErrorNumber,
            ERROR_MESSAGE() AS ErrorMessage,
            ERROR_SEVERITY() AS ErrorSeverity,
            ERROR_STATE() AS ErrorState;
    END CATCH
END;
```

### 8.6 Advanced Stored Procedure
```sql
CREATE PROCEDURE UpdateEmployeeSalary
    @EmployeeID INT,
    @PercentageIncrease DECIMAL(5,2),
    @NewSalary DECIMAL(10,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;  -- Don't return row count

    DECLARE @CurrentSalary DECIMAL(10,2);
    DECLARE @ErrorMessage NVARCHAR(500);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Get current salary
        SELECT @CurrentSalary = Salary
        FROM Employees
        WHERE EmployeeID = @EmployeeID;

        IF @CurrentSalary IS NULL
        BEGIN
            SET @ErrorMessage = 'Employee not found';
            THROW 50001, @ErrorMessage, 1;
        END

        -- Calculate new salary
        SET @NewSalary = @CurrentSalary * (1 + @PercentageIncrease / 100);

        -- Update salary
        UPDATE Employees
        SET Salary = @NewSalary
        WHERE EmployeeID = @EmployeeID;

        -- Log the change
        INSERT INTO SalaryHistory (EmployeeID, OldSalary, NewSalary, ChangeDate)
        VALUES (@EmployeeID, @CurrentSalary, @NewSalary, GETDATE());

        COMMIT TRANSACTION;

        RETURN 0;  -- Success
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
        RETURN -1;  -- Error
    END CATCH
END;
```

---

## 9. Functions

### 9.1 Scalar Functions
```sql
-- Create scalar function
CREATE FUNCTION dbo.GetFullName
(
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50)
)
RETURNS NVARCHAR(101)
AS
BEGIN
    RETURN @FirstName + ' ' + @LastName;
END;

-- Use function
SELECT dbo.GetFullName(FirstName, LastName) AS FullName
FROM Employees;

-- Function with calculations
CREATE FUNCTION dbo.CalculateAnnualSalary
(
    @MonthlySalary DECIMAL(10,2),
    @BonusPercentage DECIMAL(5,2)
)
RETURNS DECIMAL(12,2)
AS
BEGIN
    DECLARE @AnnualSalary DECIMAL(12,2);
    SET @AnnualSalary = @MonthlySalary * 12 * (1 + @BonusPercentage / 100);
    RETURN @AnnualSalary;
END;

SELECT
    FirstName,
    Salary,
    dbo.CalculateAnnualSalary(Salary, 10) AS AnnualWithBonus
FROM Employees;
```

### 9.2 Table-Valued Functions (Inline)
```sql
-- Inline table-valued function
CREATE FUNCTION dbo.GetEmployeesByDept
(
    @DepartmentID INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        EmployeeID,
        FirstName,
        LastName,
        Salary
    FROM Employees
    WHERE DepartmentID = @DepartmentID
);

-- Use function
SELECT * FROM dbo.GetEmployeesByDept(10);

-- Join with function
SELECT
    d.DepartmentName,
    e.FirstName,
    e.LastName
FROM Departments d
CROSS APPLY dbo.GetEmployeesByDept(d.DepartmentID) e;
```

### 9.3 Multi-Statement Table-Valued Functions
```sql
CREATE FUNCTION dbo.GetEmployeeHierarchy
(
    @ManagerID INT
)
RETURNS @EmployeeTable TABLE
(
    EmployeeID INT,
    FullName NVARCHAR(101),
    Level INT
)
AS
BEGIN
    DECLARE @Level INT = 1;

    -- Insert direct reports
    INSERT INTO @EmployeeTable
    SELECT
        EmployeeID,
        FirstName + ' ' + LastName,
        @Level
    FROM Employees
    WHERE ManagerID = @ManagerID;

    -- Process hierarchy
    WHILE @@ROWCOUNT > 0
    BEGIN
        SET @Level = @Level + 1;

        INSERT INTO @EmployeeTable
        SELECT
            e.EmployeeID,
            e.FirstName + ' ' + e.LastName,
            @Level
        FROM Employees e
        INNER JOIN @EmployeeTable et ON e.ManagerID = et.EmployeeID
        WHERE et.Level = @Level - 1;
    END

    RETURN;
END;

-- Use function
SELECT * FROM dbo.GetEmployeeHierarchy(1);
```

### 9.4 System Functions
```sql
-- @@IDENTITY - Last inserted identity value
INSERT INTO Employees (...) VALUES (...);
SELECT @@IDENTITY AS LastID;

-- SCOPE_IDENTITY() - Last identity in current scope
SELECT SCOPE_IDENTITY() AS LastID;

-- @@ROWCOUNT - Rows affected by last statement
UPDATE Employees SET Salary = Salary * 1.1;
SELECT @@ROWCOUNT AS RowsUpdated;

-- @@ERROR - Error number of last statement
SELECT @@ERROR AS ErrorNumber;

-- @@TRANCOUNT - Active transaction count
SELECT @@TRANCOUNT AS ActiveTransactions;

-- @@VERSION - SQL Server version
SELECT @@VERSION;

-- @@SERVERNAME - Server name
SELECT @@SERVERNAME AS ServerName;

-- DB_NAME() - Current database name
SELECT DB_NAME() AS CurrentDatabase;

-- USER_NAME() - Current user
SELECT USER_NAME() AS CurrentUser;

-- OBJECT_ID() - Object ID
SELECT OBJECT_ID('Employees') AS TableID;

-- COL_LENGTH() - Column length
SELECT COL_LENGTH('Employees', 'FirstName') AS ColumnLength;
```

---

## 10. Triggers

### 10.1 DML Triggers (AFTER)
```sql
-- AFTER INSERT Trigger
CREATE TRIGGER trg_AfterInsertEmployee
ON Employees
AFTER INSERT
AS
BEGIN
    INSERT INTO EmployeeAudit (EmployeeID, Action, ActionDate)
    SELECT EmployeeID, 'INSERT', GETDATE()
    FROM inserted;
END;

-- AFTER UPDATE Trigger
CREATE TRIGGER trg_AfterUpdateEmployee
ON Employees
AFTER UPDATE
AS
BEGIN
    INSERT INTO EmployeeAudit (EmployeeID, OldSalary, NewSalary, ActionDate)
    SELECT
        i.EmployeeID,
        d.Salary AS OldSalary,
        i.Salary AS NewSalary,
        GETDATE()
    FROM inserted i
    INNER JOIN deleted d ON i.EmployeeID = d.EmployeeID
    WHERE i.Salary <> d.Salary;
END;

-- AFTER DELETE Trigger
CREATE TRIGGER trg_AfterDeleteEmployee
ON Employees
AFTER DELETE
AS
BEGIN
    INSERT INTO EmployeeAudit (EmployeeID, Action, ActionDate)
    SELECT EmployeeID, 'DELETE', GETDATE()
    FROM deleted;
END;

-- Multiple events
CREATE TRIGGER trg_EmployeeChanges
ON Employees
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
    BEGIN
        -- INSERT
        PRINT 'Insert occurred';
    END
    ELSE IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        -- UPDATE
        PRINT 'Update occurred';
    END
    ELSE IF NOT EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        -- DELETE
        PRINT 'Delete occurred';
    END
END;
```

### 10.2 INSTEAD OF Triggers
```sql
-- INSTEAD OF INSERT
CREATE TRIGGER trg_InsteadOfInsertEmployee
ON Employees
INSTEAD OF INSERT
AS
BEGIN
    -- Custom validation
    IF EXISTS (SELECT 1 FROM inserted WHERE Salary < 30000)
    BEGIN
        RAISERROR('Salary cannot be less than 30000', 16, 1);
        RETURN;
    END

    -- Perform insert
    INSERT INTO Employees (FirstName, LastName, Email, Salary, HireDate)
    SELECT FirstName, LastName, Email, Salary, GETDATE()
    FROM inserted;
END;

-- INSTEAD OF DELETE (Soft delete)
CREATE TRIGGER trg_InsteadOfDeleteEmployee
ON Employees
INSTEAD OF DELETE
AS
BEGIN
    UPDATE Employees
    SET IsActive = 0, TerminationDate = GETDATE()
    WHERE EmployeeID IN (SELECT EmployeeID FROM deleted);
END;
```

### 10.3 DDL Triggers
```sql
-- Database-level DDL trigger
CREATE TRIGGER trg_PreventTableDrop
ON DATABASE
FOR DROP_TABLE
AS
BEGIN
    PRINT 'Table drop is not allowed';
    ROLLBACK;
END;

-- Server-level DDL trigger
CREATE TRIGGER trg_AuditDatabaseCreation
ON ALL SERVER
FOR CREATE_DATABASE
AS
BEGIN
    INSERT INTO ServerAudit (EventType, EventData, EventDate)
    VALUES ('CREATE_DATABASE', EVENTDATA(), GETDATE());
END;
```

### 10.4 Trigger Management
```sql
-- Disable trigger
DISABLE TRIGGER trg_AfterInsertEmployee ON Employees;

-- Enable trigger
ENABLE TRIGGER trg_AfterInsertEmployee ON Employees;

-- Disable all triggers on table
DISABLE TRIGGER ALL ON Employees;

-- Enable all triggers on table
ENABLE TRIGGER ALL ON Employees;

-- Drop trigger
DROP TRIGGER trg_AfterInsertEmployee;

-- View trigger definition
EXEC sp_helptext 'trg_AfterInsertEmployee';

-- List all triggers
SELECT * FROM sys.triggers;
```

---

## 11. Views

### 11.1 Simple Views
```sql
-- Create view
CREATE VIEW vw_EmployeeDetails
AS
SELECT
    e.EmployeeID,
    e.FirstName + ' ' + e.LastName AS FullName,
    e.Email,
    e.JobTitle,
    e.Salary,
    d.DepartmentName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE e.IsActive = 1;

-- Query view
SELECT * FROM vw_EmployeeDetails;

-- Filter view
SELECT * FROM vw_EmployeeDetails
WHERE DepartmentName = 'IT';
```

### 11.2 Complex Views
```sql
-- View with aggregates
CREATE VIEW vw_DepartmentSummary
AS
SELECT
    d.DepartmentID,
    d.DepartmentName,
    COUNT(e.EmployeeID) AS EmployeeCount,
    AVG(e.Salary) AS AvgSalary,
    MAX(e.Salary) AS MaxSalary,
    MIN(e.Salary) AS MinSalary
FROM Departments d
LEFT JOIN Employees e ON d.DepartmentID = e.DepartmentID
GROUP BY d.DepartmentID, d.DepartmentName;

-- View with multiple joins
CREATE VIEW vw_OrderDetails
AS
SELECT
    o.OrderID,
    o.OrderDate,
    c.CompanyName,
    c.ContactName,
    p.ProductName,
    od.Quantity,
    od.UnitPrice,
    od.Quantity * od.UnitPrice AS LineTotal
FROM Orders o
INNER JOIN Customers c ON o.CustomerID = c.CustomerID
INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
INNER JOIN Products p ON od.ProductID = p.ProductID;
```

### 11.3 Updatable Views
```sql
-- Create updatable view
CREATE VIEW vw_ActiveEmployees
AS
SELECT
    EmployeeID,
    FirstName,
    LastName,
    Email,
    Salary
FROM Employees
WHERE IsActive = 1;

-- Update through view
UPDATE vw_ActiveEmployees
SET Salary = Salary * 1.1
WHERE EmployeeID = 1;

-- Insert through view
INSERT INTO vw_ActiveEmployees (FirstName, LastName, Email, Salary)
VALUES ('John', 'Doe', 'john@example.com', 50000);

-- Delete through view
DELETE FROM vw_ActiveEmployees WHERE EmployeeID = 100;
```

### 11.4 Indexed Views (Materialized Views)
```sql
-- Create view with SCHEMABINDING
CREATE VIEW vw_ProductSales
WITH SCHEMABINDING
AS
SELECT
    p.ProductID,
    p.ProductName,
    SUM(od.Quantity) AS TotalQuantity,
    COUNT_BIG(*) AS OrderCount
FROM dbo.Products p
INNER JOIN dbo.OrderDetails od ON p.ProductID = od.ProductID
GROUP BY p.ProductID, p.ProductName;

-- Create unique clustered index
CREATE UNIQUE CLUSTERED INDEX IX_vw_ProductSales
ON vw_ProductSales (ProductID);

-- Create nonclustered index
CREATE NONCLUSTERED INDEX IX_vw_ProductSales_TotalQty
ON vw_ProductSales (TotalQuantity);
```

### 11.5 View with CHECK OPTION
```sql
-- Prevent updates that don't meet view criteria
CREATE VIEW vw_HighSalaryEmployees
AS
SELECT *
FROM Employees
WHERE Salary > 70000
WITH CHECK OPTION;

-- This will fail because new salary doesn't meet criteria
UPDATE vw_HighSalaryEmployees
SET Salary = 60000
WHERE EmployeeID = 1;
```

### 11.6 View Management
```sql
-- Alter view
ALTER VIEW vw_EmployeeDetails
AS
SELECT
    e.EmployeeID,
    e.FirstName,
    e.LastName,
    e.Email,
    d.DepartmentName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID;

-- Drop view
DROP VIEW vw_EmployeeDetails;

-- View definition
EXEC sp_helptext 'vw_EmployeeDetails';

-- Refresh view metadata
EXEC sp_refreshview 'vw_EmployeeDetails';

-- List all views
SELECT * FROM sys.views;
```

---

## 12. Indexes

### 12.1 Clustered Index
```sql
-- Create clustered index (only one per table)
CREATE CLUSTERED INDEX IX_Employees_EmployeeID
ON Employees (EmployeeID);

-- Primary key creates clustered index by default
CREATE TABLE TestTable (
    ID INT PRIMARY KEY CLUSTERED,  -- Clustered index
    Name NVARCHAR(50)
);

-- Drop clustered index
DROP INDEX IX_Employees_EmployeeID ON Employees;
```

### 12.2 Non-Clustered Index
```sql
-- Create non-clustered index
CREATE NONCLUSTERED INDEX IX_Employees_LastName
ON Employees (LastName);

-- Composite index
CREATE NONCLUSTERED INDEX IX_Employees_Dept_Salary
ON Employees (DepartmentID, Salary);

-- Index with INCLUDE clause (covering index)
CREATE NONCLUSTERED INDEX IX_Employees_LastName_Inc
ON Employees (LastName)
INCLUDE (FirstName, Email, Salary);

-- Unique index
CREATE UNIQUE NONCLUSTERED INDEX IX_Employees_Email
ON Employees (Email);

-- Filtered index
CREATE NONCLUSTERED INDEX IX_Employees_Active
ON Employees (HireDate)
WHERE IsActive = 1;
```

### 12.3 Index Options
```sql
-- Index with options
CREATE NONCLUSTERED INDEX IX_Employees_Salary
ON Employees (Salary)
WITH (
    FILLFACTOR = 80,          -- Leave 20% free space
    PAD_INDEX = ON,           -- Apply fillfactor to index pages
    SORT_IN_TEMPDB = ON,      -- Use tempdb for sorting
    ONLINE = ON,              -- Keep table accessible during creation
    DATA_COMPRESSION = PAGE   -- Compress index pages
);

-- Rebuild index
ALTER INDEX IX_Employees_Salary ON Employees REBUILD;

-- Reorganize index
ALTER INDEX IX_Employees_Salary ON Employees REORGANIZE;

-- Disable index
ALTER INDEX IX_Employees_Salary ON Employees DISABLE;

-- Enable index (rebuild)
ALTER INDEX IX_Employees_Salary ON Employees REBUILD;

-- Drop index
DROP INDEX IX_Employees_Salary ON Employees;
```

### 12.4 Index Analysis
```sql
-- View index usage
SELECT
    OBJECT_NAME(s.object_id) AS TableName,
    i.name AS IndexName,
    s.user_seeks,
    s.user_scans,
    s.user_lookups,
    s.user_updates
FROM sys.dm_db_index_usage_stats s
INNER JOIN sys.indexes i ON s.object_id = i.object_id
    AND s.index_id = i.index_id
WHERE database_id = DB_ID()
ORDER BY s.user_seeks + s.user_scans + s.user_lookups DESC;

-- Find missing indexes
SELECT
    OBJECT_NAME(d.object_id) AS TableName,
    d.equality_columns,
    d.inequality_columns,
    d.included_columns,
    s.user_seeks,
    s.user_scans,
    s.avg_total_user_cost,
    s.avg_user_impact
FROM sys.dm_db_missing_index_details d
INNER JOIN sys.dm_db_missing_index_groups g ON d.index_handle = g.index_handle
INNER JOIN sys.dm_db_missing_index_group_stats s ON g.index_group_handle = s.group_handle
WHERE database_id = DB_ID()
ORDER BY s.avg_total_user_cost * s.avg_user_impact DESC;

-- Index fragmentation
SELECT
    OBJECT_NAME(ips.object_id) AS TableName,
    i.name AS IndexName,
    ips.index_type_desc,
    ips.avg_fragmentation_in_percent,
    ips.page_count
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'DETAILED') ips
INNER JOIN sys.indexes i ON ips.object_id = i.object_id
    AND ips.index_id = i.index_id
WHERE ips.avg_fragmentation_in_percent > 10
ORDER BY ips.avg_fragmentation_in_percent DESC;
```

### 12.5 Full-Text Index
```sql
-- Create full-text catalog
CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;

-- Create full-text index
CREATE FULLTEXT INDEX ON Documents (
    Title,
    Content
)
KEY INDEX PK_Documents
ON ftCatalog;

-- Search full-text index
SELECT *
FROM Documents
WHERE CONTAINS(Content, 'SQL Server');

-- Proximity search
SELECT *
FROM Documents
WHERE CONTAINS(Content, 'NEAR((SQL, Server), 10)');

-- Thesaurus search
SELECT *
FROM Documents
WHERE FREETEXT(Content, 'database management');
```

---

## 13. Transactions & Concurrency

### 13.1 Basic Transactions
```sql
-- Simple transaction
BEGIN TRANSACTION;

UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;
UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2;

COMMIT TRANSACTION;

-- Transaction with rollback
BEGIN TRANSACTION;

UPDATE Employees SET Salary = Salary * 1.1;

-- Something went wrong
ROLLBACK TRANSACTION;

-- Transaction with error handling
BEGIN TRY
    BEGIN TRANSACTION;

    UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;
    UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2;

    COMMIT TRANSACTION;
    PRINT 'Transaction committed successfully';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT 'Transaction rolled back due to error';
    PRINT ERROR_MESSAGE();
END CATCH;
```

### 13.2 Named Transactions and Savepoints
```sql
-- Named transaction
BEGIN TRANSACTION TransferFunds;

UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;

-- Create savepoint
SAVE TRANSACTION SavePoint1;

UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2;

-- Rollback to savepoint
ROLLBACK TRANSACTION SavePoint1;

-- Commit transaction
COMMIT TRANSACTION TransferFunds;
```

### 13.3 Isolation Levels
```sql
-- READ UNCOMMITTED (Dirty Read possible)
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
BEGIN TRANSACTION;
SELECT * FROM Employees;
COMMIT;

-- READ COMMITTED (Default, no dirty reads)
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRANSACTION;
SELECT * FROM Employees;
COMMIT;

-- REPEATABLE READ (No dirty reads, no non-repeatable reads)
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
BEGIN TRANSACTION;
SELECT * FROM Employees WHERE DepartmentID = 10;
-- Repeated reads will get same results
SELECT * FROM Employees WHERE DepartmentID = 10;
COMMIT;

-- SERIALIZABLE (No dirty, non-repeatable reads, no phantoms)
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;
SELECT * FROM Employees WHERE Salary > 50000;
COMMIT;

-- SNAPSHOT (Optimistic concurrency)
ALTER DATABASE CompanyDB SET ALLOW_SNAPSHOT_ISOLATION ON;

SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
BEGIN TRANSACTION;
SELECT * FROM Employees;
COMMIT;
```

### 13.4 Locking Hints
```sql
-- NOLOCK (same as READ UNCOMMITTED)
SELECT * FROM Employees WITH (NOLOCK);

-- ROWLOCK (force row-level locks)
UPDATE Employees WITH (ROWLOCK)
SET Salary = Salary * 1.1
WHERE EmployeeID = 1;

-- TABLOCK (force table-level lock)
SELECT * FROM Employees WITH (TABLOCK);

-- UPDLOCK (update lock, prevents deadlocks)
BEGIN TRANSACTION;
SELECT * FROM Employees WITH (UPDLOCK)
WHERE EmployeeID = 1;

UPDATE Employees
SET Salary = Salary * 1.1
WHERE EmployeeID = 1;
COMMIT;

-- XLOCK (exclusive lock)
SELECT * FROM Employees WITH (XLOCK)
WHERE EmployeeID = 1;

-- HOLDLOCK (hold lock until end of transaction)
BEGIN TRANSACTION;
SELECT * FROM Employees WITH (HOLDLOCK)
WHERE DepartmentID = 10;
COMMIT;
```

### 13.5 Deadlock Handling
```sql
-- Create deadlock retry logic
CREATE PROCEDURE TransferFunds
    @FromAccount INT,
    @ToAccount INT,
    @Amount DECIMAL(10,2)
AS
BEGIN
    DECLARE @RetryCount INT = 0;
    DECLARE @MaxRetries INT = 3;

    WHILE @RetryCount < @MaxRetries
    BEGIN
        BEGIN TRY
            BEGIN TRANSACTION;

            -- Lock accounts in consistent order to prevent deadlocks
            DECLARE @FirstAccount INT, @SecondAccount INT;

            IF @FromAccount < @ToAccount
            BEGIN
                SET @FirstAccount = @FromAccount;
                SET @SecondAccount = @ToAccount;
            END
            ELSE
            BEGIN
                SET @FirstAccount = @ToAccount;
                SET @SecondAccount = @FromAccount;
            END

            -- Lock first account
            UPDATE Accounts WITH (UPDLOCK)
            SET Balance = Balance
            WHERE AccountID = @FirstAccount;

            -- Lock second account
            UPDATE Accounts WITH (UPDLOCK)
            SET Balance = Balance
            WHERE AccountID = @SecondAccount;

            -- Perform transfer
            UPDATE Accounts
            SET Balance = Balance - @Amount
            WHERE AccountID = @FromAccount;

            UPDATE Accounts
            SET Balance = Balance + @Amount
            WHERE AccountID = @ToAccount;

            COMMIT TRANSACTION;
            BREAK;  -- Success, exit loop
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;

            IF ERROR_NUMBER() = 1205  -- Deadlock
            BEGIN
                SET @RetryCount = @RetryCount + 1;
                WAITFOR DELAY '00:00:00.100';  -- Wait 100ms
                CONTINUE;
            END
            ELSE
            BEGIN
                THROW;
            END
        END CATCH
    END
END;
```

---

## 14. Window Functions

### 14.1 Aggregate Window Functions
```sql
-- SUM OVER
SELECT
    EmployeeID,
    FirstName,
    Salary,
    SUM(Salary) OVER (PARTITION BY DepartmentID) AS DeptTotal,
    SUM(Salary) OVER () AS GrandTotal
FROM Employees;

-- AVG OVER
SELECT
    EmployeeID,
    FirstName,
    Salary,
    AVG(Salary) OVER (PARTITION BY DepartmentID) AS DeptAvg
FROM Employees;

-- Running total
SELECT
    OrderID,
    OrderDate,
    TotalAmount,
    SUM(TotalAmount) OVER (ORDER BY OrderDate) AS RunningTotal
FROM Orders;
```

### 14.2 Ranking Functions
```sql
-- ROW_NUMBER (unique rank, no ties)
SELECT
    ROW_NUMBER() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS RowNum,
    FirstName,
    LastName,
    DepartmentID,
    Salary
FROM Employees;

-- RANK (gaps for ties)
SELECT
    RANK() OVER (ORDER BY Salary DESC) AS Rank,
    FirstName,
    LastName,
    Salary
FROM Employees;

-- DENSE_RANK (no gaps)
SELECT
    DENSE_RANK() OVER (ORDER BY Salary DESC) AS DenseRank,
    FirstName,
    LastName,
    Salary
FROM Employees;

-- NTILE (divide into groups)
SELECT
    NTILE(4) OVER (ORDER BY Salary) AS Quartile,
    FirstName,
    LastName,
    Salary
FROM Employees;
```

### 14.3 Offset Functions
```sql
-- LAG (previous row)
SELECT
    OrderDate,
    TotalAmount,
    LAG(TotalAmount, 1, 0) OVER (ORDER BY OrderDate) AS PreviousAmount,
    TotalAmount - LAG(TotalAmount, 1, 0) OVER (ORDER BY OrderDate) AS Difference
FROM Orders;

-- LEAD (next row)
SELECT
    OrderDate,
    TotalAmount,
    LEAD(TotalAmount) OVER (ORDER BY OrderDate) AS NextAmount
FROM Orders;

-- FIRST_VALUE
SELECT
    EmployeeID,
    Salary,
    FIRST_VALUE(Salary) OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS HighestSalary
FROM Employees;

-- LAST_VALUE
SELECT
    EmployeeID,
    Salary,
    LAST_VALUE(Salary) OVER (
        PARTITION BY DepartmentID
        ORDER BY Salary
        ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING
    ) AS HighestSalary
FROM Employees;
```

### 14.4 Frame Specification
```sql
-- Rows frame
SELECT
    OrderDate,
    TotalAmount,
    AVG(TotalAmount) OVER (
        ORDER BY OrderDate
        ROWS BETWEEN 2 PRECEDING AND CURRENT ROW
    ) AS MovingAvg3
FROM Orders;

-- Range frame
SELECT
    OrderDate,
    TotalAmount,
    SUM(TotalAmount) OVER (
        ORDER BY OrderDate
        RANGE BETWEEN INTERVAL '7' DAY PRECEDING AND CURRENT ROW
    ) AS Last7DaysTotal
FROM Orders;

-- Full frame specification
SELECT
    EmployeeID,
    Salary,
    AVG(Salary) OVER (
        PARTITION BY DepartmentID
        ORDER BY Salary
        ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING
    ) AS DeptAvg
FROM Employees;
```

---

## 15. Performance Tuning

### 15.1 Execution Plans
```sql
-- Show estimated execution plan
SET SHOWPLAN_TEXT ON;
GO
SELECT * FROM Employees WHERE DepartmentID = 10;
GO
SET SHOWPLAN_TEXT OFF;
GO

-- Show actual execution plan
SET STATISTICS PROFILE ON;
GO
SELECT * FROM Employees WHERE DepartmentID = 10;
GO
SET STATISTICS PROFILE OFF;
GO

-- Include actual execution plan (SSMS)
-- Ctrl + M or Query > Include Actual Execution Plan

-- Statistics IO
SET STATISTICS IO ON;
SELECT * FROM Employees WHERE DepartmentID = 10;
SET STATISTICS IO OFF;

-- Statistics TIME
SET STATISTICS TIME ON;
SELECT * FROM Employees WHERE DepartmentID = 10;
SET STATISTICS TIME OFF;
```

### 15.2 Query Optimization Techniques
```sql
-- Use EXISTS instead of IN for large datasets
-- Slow
SELECT * FROM Customers
WHERE CustomerID IN (SELECT CustomerID FROM Orders);

-- Faster
SELECT * FROM Customers c
WHERE EXISTS (SELECT 1 FROM Orders o WHERE o.CustomerID = c.CustomerID);

-- Avoid functions on indexed columns
-- Slow (can't use index)
SELECT * FROM Employees
WHERE YEAR(HireDate) = 2020;

-- Faster (can use index)
SELECT * FROM Employees
WHERE HireDate >= '2020-01-01' AND HireDate < '2021-01-01';

-- Use covering indexes
CREATE NONCLUSTERED INDEX IX_Employees_Covering
ON Employees (DepartmentID)
INCLUDE (FirstName, LastName, Salary);

SELECT FirstName, LastName, Salary
FROM Employees
WHERE DepartmentID = 10;  -- Index covers all columns

-- Avoid SELECT *
-- Slow
SELECT * FROM Employees;

-- Faster
SELECT EmployeeID, FirstName, LastName FROM Employees;

-- Use WHERE instead of HAVING when possible
-- Slow
SELECT DepartmentID, COUNT(*)
FROM Employees
GROUP BY DepartmentID
HAVING DepartmentID = 10;

-- Faster
SELECT DepartmentID, COUNT(*)
FROM Employees
WHERE DepartmentID = 10
GROUP BY DepartmentID;

-- Use UNION ALL instead of UNION when duplicates don't matter
-- Slow (removes duplicates)
SELECT City FROM Employees
UNION
SELECT City FROM Customers;

-- Faster (keeps duplicates)
SELECT City FROM Employees
UNION ALL
SELECT City FROM Customers;
```

### 15.3 Index Tuning
```sql
-- Update statistics
UPDATE STATISTICS Employees;

-- Update statistics with full scan
UPDATE STATISTICS Employees WITH FULLSCAN;

-- Auto update statistics
ALTER DATABASE CompanyDB SET AUTO_UPDATE_STATISTICS ON;

-- Rebuild all indexes
ALTER INDEX ALL ON Employees REBUILD;

-- Rebuild fragmented indexes
DECLARE @TableName NVARCHAR(255);
DECLARE @IndexName NVARCHAR(255);
DECLARE @Fragmentation FLOAT;

DECLARE index_cursor CURSOR FOR
SELECT
    OBJECT_NAME(ips.object_id),
    i.name,
    ips.avg_fragmentation_in_percent
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'DETAILED') ips
INNER JOIN sys.indexes i ON ips.object_id = i.object_id
    AND ips.index_id = i.index_id
WHERE ips.avg_fragmentation_in_percent > 30
    AND ips.page_count > 1000;

OPEN index_cursor;

FETCH NEXT FROM index_cursor INTO @TableName, @IndexName, @Fragmentation;

WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT 'Rebuilding ' + @IndexName + ' on ' + @TableName;
    EXEC('ALTER INDEX ' + @IndexName + ' ON ' + @TableName + ' REBUILD');

    FETCH NEXT FROM index_cursor INTO @TableName, @IndexName, @Fragmentation;
END

CLOSE index_cursor;
DEALLOCATE index_cursor;
```

### 15.4 Query Store
```sql
-- Enable Query Store
ALTER DATABASE CompanyDB
SET QUERY_STORE = ON;

-- Configure Query Store
ALTER DATABASE CompanyDB
SET QUERY_STORE (
    OPERATION_MODE = READ_WRITE,
    DATA_FLUSH_INTERVAL_SECONDS = 900,
    INTERVAL_LENGTH_MINUTES = 60,
    MAX_STORAGE_SIZE_MB = 1024,
    QUERY_CAPTURE_MODE = AUTO
);

-- View top resource consuming queries
SELECT TOP 10
    q.query_id,
    qt.query_sql_text,
    rs.avg_duration / 1000 AS avg_duration_ms,
    rs.avg_cpu_time / 1000 AS avg_cpu_ms,
    rs.avg_logical_io_reads,
    rs.count_executions
FROM sys.query_store_query q
INNER JOIN sys.query_store_query_text qt ON q.query_text_id = qt.query_text_id
INNER JOIN sys.query_store_plan p ON q.query_id = p.query_id
INNER JOIN sys.query_store_runtime_stats rs ON p.plan_id = rs.plan_id
ORDER BY rs.avg_duration DESC;

-- Clear Query Store
ALTER DATABASE CompanyDB
SET QUERY_STORE CLEAR ALL;
```

### 15.5 Monitoring and DMVs
```sql
-- Currently executing queries
SELECT
    s.session_id,
    r.status,
    r.command,
    r.cpu_time,
    r.total_elapsed_time,
    t.text AS query_text
FROM sys.dm_exec_requests r
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) t
INNER JOIN sys.dm_exec_sessions s ON r.session_id = s.session_id
WHERE s.is_user_process = 1;

-- Top CPU consuming queries
SELECT TOP 10
    qs.execution_count,
    qs.total_worker_time / 1000000 AS total_cpu_seconds,
    qs.total_worker_time / qs.execution_count / 1000 AS avg_cpu_ms,
    SUBSTRING(qt.text, qs.statement_start_offset/2 + 1,
        (CASE WHEN qs.statement_end_offset = -1
            THEN LEN(CONVERT(NVARCHAR(MAX), qt.text)) * 2
            ELSE qs.statement_end_offset
        END - qs.statement_start_offset)/2) AS query_text
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
ORDER BY qs.total_worker_time DESC;

-- Table sizes
SELECT
    t.name AS TableName,
    p.rows AS RowCount,
    SUM(a.total_pages) * 8 / 1024 AS TotalSpaceMB,
    SUM(a.used_pages) * 8 / 1024 AS UsedSpaceMB
FROM sys.tables t
INNER JOIN sys.indexes i ON t.object_id = i.object_id
INNER JOIN sys.partitions p ON i.object_id = p.object_id AND i.index_id = p.index_id
INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
WHERE t.is_ms_shipped = 0
GROUP BY t.name, p.rows
ORDER BY SUM(a.total_pages) DESC;

-- Wait statistics
SELECT TOP 20
    wait_type,
    wait_time_ms / 1000.0 AS wait_time_seconds,
    waiting_tasks_count,
    wait_time_ms / waiting_tasks_count AS avg_wait_ms
FROM sys.dm_os_wait_stats
WHERE waiting_tasks_count > 0
ORDER BY wait_time_ms DESC;
```

---

## 16. Security

### 16.1 User Management
```sql
-- Create login (server-level)
CREATE LOGIN john_doe
WITH PASSWORD = 'StrongP@ssw0rd!';

-- Create user (database-level)
USE CompanyDB;
CREATE USER john_doe FOR LOGIN john_doe;

-- Alter user
ALTER USER john_doe WITH NAME = john_smith;

-- Drop user
DROP USER john_doe;

-- Drop login
DROP LOGIN john_doe;

-- Create user from Windows authentication
CREATE LOGIN [DOMAIN\username] FROM WINDOWS;
CREATE USER [DOMAIN\username] FOR LOGIN [DOMAIN\username];
```

### 16.2 Roles and Permissions
```sql
-- Create database role
CREATE ROLE DataReaders;

-- Add user to role
ALTER ROLE DataReaders ADD MEMBER john_doe;

-- Grant permissions to role
GRANT SELECT ON Employees TO DataReaders;
GRANT SELECT ON Departments TO DataReaders;

-- Grant specific permissions
GRANT SELECT, INSERT, UPDATE ON Customers TO john_doe;
GRANT EXECUTE ON dbo.GetEmployeesByDept TO john_doe;

-- Grant with grant option
GRANT SELECT ON Employees TO john_doe WITH GRANT OPTION;

-- Deny permission
DENY DELETE ON Employees TO john_doe;

-- Revoke permission
REVOKE SELECT ON Employees FROM john_doe;

-- Server-level roles
ALTER SERVER ROLE sysadmin ADD MEMBER john_doe;
ALTER SERVER ROLE dbcreator ADD MEMBER john_doe;

-- Database-level roles
ALTER ROLE db_owner ADD MEMBER john_doe;
ALTER ROLE db_datareader ADD MEMBER john_doe;
ALTER ROLE db_datawriter ADD MEMBER john_doe;
ALTER ROLE db_ddladmin ADD MEMBER john_doe;

-- View permissions
SELECT
    pr.name AS RoleName,
    pm.class_desc,
    pm.permission_name,
    pm.state_desc,
    OBJECT_NAME(pm.major_id) AS ObjectName
FROM sys.database_principals pr
INNER JOIN sys.database_permissions pm ON pr.principal_id = pm.grantee_principal_id
WHERE pr.name = 'DataReaders';
```

### 16.3 Row-Level Security
```sql
-- Create security function
CREATE FUNCTION dbo.fn_SecurityPredicate(@DepartmentID AS INT)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN SELECT 1 AS Result
WHERE @DepartmentID = USER_NAME() OR IS_MEMBER('Managers') = 1;

-- Create security policy
CREATE SECURITY POLICY EmployeeSecurityPolicy
ADD FILTER PREDICATE dbo.fn_SecurityPredicate(DepartmentID) ON dbo.Employees,
ADD BLOCK PREDICATE dbo.fn_SecurityPredicate(DepartmentID) ON dbo.Employees AFTER INSERT
WITH (STATE = ON);

-- Alter security policy
ALTER SECURITY POLICY EmployeeSecurityPolicy
WITH (STATE = OFF);

-- Drop security policy
DROP SECURITY POLICY EmployeeSecurityPolicy;
```

### 16.4 Dynamic Data Masking
```sql
-- Add masked column
ALTER TABLE Employees
ADD Email NVARCHAR(100) MASKED WITH (FUNCTION = 'email()');

-- Alter existing column with mask
ALTER TABLE Employees
ALTER COLUMN Phone NVARCHAR(20) MASKED WITH (FUNCTION = 'partial(0,"XXX-XXX-",4)');

-- Different masking functions
-- default(): Full masking
-- email(): j***@****.com
-- random(start, end): Random number
-- partial(prefix, padding, suffix): Custom masking

-- Grant unmask permission
GRANT UNMASK TO john_doe;

-- Revoke unmask permission
REVOKE UNMASK TO john_doe;

-- Remove mask
ALTER TABLE Employees
ALTER COLUMN Email DROP MASKED;
```

### 16.5 Always Encrypted
```sql
-- Create column master key
CREATE COLUMN MASTER KEY CMK1
WITH (
    KEY_STORE_PROVIDER_NAME = 'MSSQL_CERTIFICATE_STORE',
    KEY_PATH = 'CurrentUser/My/thumbprint'
);

-- Create column encryption key
CREATE COLUMN ENCRYPTION KEY CEK1
WITH VALUES (
    COLUMN_MASTER_KEY = CMK1,
    ALGORITHM = 'RSA_OAEP',
    ENCRYPTED_VALUE = 0x...
);

-- Create table with encrypted columns
CREATE TABLE SecureEmployees (
    EmployeeID INT PRIMARY KEY,
    FirstName NVARCHAR(50),
    SSN NVARCHAR(11) COLLATE Latin1_General_BIN2
        ENCRYPTED WITH (
            COLUMN_ENCRYPTION_KEY = CEK1,
            ENCRYPTION_TYPE = DETERMINISTIC,
            ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256'
        ),
    Salary DECIMAL(10,2)
        ENCRYPTED WITH (
            COLUMN_ENCRYPTION_KEY = CEK1,
            ENCRYPTION_TYPE = RANDOMIZED,
            ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256'
        )
);
```

### 16.6 Auditing
```sql
-- Create server audit
CREATE SERVER AUDIT ServerAudit
TO FILE (
    FILEPATH = 'C:\SQLAudit\',
    MAXSIZE = 100 MB,
    MAX_ROLLOVER_FILES = 10
);

-- Enable audit
ALTER SERVER AUDIT ServerAudit WITH (STATE = ON);

-- Create database audit specification
CREATE DATABASE AUDIT SPECIFICATION DatabaseAuditSpec
FOR SERVER AUDIT ServerAudit
ADD (SELECT, INSERT, UPDATE, DELETE ON Employees BY public);

-- Enable specification
ALTER DATABASE AUDIT SPECIFICATION DatabaseAuditSpec WITH (STATE = ON);

-- View audit logs
SELECT
    event_time,
    action_id,
    succeeded,
    session_server_principal_name,
    database_name,
    statement
FROM sys.fn_get_audit_file('C:\SQLAudit\*.sqlaudit', DEFAULT, DEFAULT);
```

---

## 17. Backup & Recovery

### 17.1 Backup Types
```sql
-- Full backup
BACKUP DATABASE CompanyDB
TO DISK = 'C:\Backups\CompanyDB_Full.bak'
WITH FORMAT, COMPRESSION, STATS = 10;

-- Differential backup
BACKUP DATABASE CompanyDB
TO DISK = 'C:\Backups\CompanyDB_Diff.bak'
WITH DIFFERENTIAL, COMPRESSION;

-- Transaction log backup
BACKUP LOG CompanyDB
TO DISK = 'C:\Backups\CompanyDB_Log.trn'
WITH COMPRESSION;

-- File/Filegroup backup
BACKUP DATABASE CompanyDB
FILE = 'CompanyDB_Data'
TO DISK = 'C:\Backups\CompanyDB_File.bak';

-- Copy-only backup
BACKUP DATABASE CompanyDB
TO DISK = 'C:\Backups\CompanyDB_CopyOnly.bak'
WITH COPY_ONLY, COMPRESSION;

-- Backup to multiple files (striping)
BACKUP DATABASE CompanyDB
TO DISK = 'C:\Backups\CompanyDB1.bak',
   DISK = 'C:\Backups\CompanyDB2.bak',
   DISK = 'C:\Backups\CompanyDB3.bak'
WITH FORMAT, COMPRESSION;
```

### 17.2 Restore Operations
```sql
-- Simple restore
RESTORE DATABASE CompanyDB
FROM DISK = 'C:\Backups\CompanyDB_Full.bak'
WITH REPLACE;

-- Restore with move
RESTORE DATABASE CompanyDB
FROM DISK = 'C:\Backups\CompanyDB_Full.bak'
WITH MOVE 'CompanyDB_Data' TO 'D:\Data\CompanyDB.mdf',
     MOVE 'CompanyDB_Log' TO 'E:\Logs\CompanyDB.ldf',
     REPLACE;

-- Restore with NORECOVERY (for log restores)
RESTORE DATABASE CompanyDB
FROM DISK = 'C:\Backups\CompanyDB_Full.bak'
WITH NORECOVERY;

RESTORE DATABASE CompanyDB
FROM DISK = 'C:\Backups\CompanyDB_Diff.bak'
WITH NORECOVERY;

RESTORE LOG CompanyDB
FROM DISK = 'C:\Backups\CompanyDB_Log.trn'
WITH RECOVERY;

-- Point-in-time restore
RESTORE DATABASE CompanyDB
FROM DISK = 'C:\Backups\CompanyDB_Full.bak'
WITH NORECOVERY;

RESTORE LOG CompanyDB
FROM DISK = 'C:\Backups\CompanyDB_Log.trn'
WITH STOPAT = '2023-12-25 10:30:00',
     RECOVERY;

-- Restore to different database
RESTORE DATABASE CompanyDB_Test
FROM DISK = 'C:\Backups\CompanyDB_Full.bak'
WITH MOVE 'CompanyDB_Data' TO 'D:\Data\CompanyDB_Test.mdf',
     MOVE 'CompanyDB_Log' TO 'E:\Logs\CompanyDB_Test.ldf';

-- Verify backup
RESTORE VERIFYONLY
FROM DISK = 'C:\Backups\CompanyDB_Full.bak';

-- View backup contents
RESTORE HEADERONLY
FROM DISK = 'C:\Backups\CompanyDB_Full.bak';

RESTORE FILELISTONLY
FROM DISK = 'C:\Backups\CompanyDB_Full.bak';
```

### 17.3 Recovery Models
```sql
-- Set recovery model
ALTER DATABASE CompanyDB SET RECOVERY FULL;
ALTER DATABASE CompanyDB SET RECOVERY SIMPLE;
ALTER DATABASE CompanyDB SET RECOVERY BULK_LOGGED;

-- View recovery model
SELECT name, recovery_model_desc
FROM sys.databases
WHERE name = 'CompanyDB';
```

### 17.4 Backup Strategy
```sql
-- Create maintenance plan script

-- Sunday: Full backup
IF DATEPART(WEEKDAY, GETDATE()) = 1
BEGIN
    BACKUP DATABASE CompanyDB
    TO DISK = 'C:\Backups\CompanyDB_Full_' +
              CONVERT(VARCHAR(8), GETDATE(), 112) + '.bak'
    WITH COMPRESSION, INIT;
END

-- Monday-Saturday: Differential backup
IF DATEPART(WEEKDAY, GETDATE()) BETWEEN 2 AND 7
BEGIN
    BACKUP DATABASE CompanyDB
    TO DISK = 'C:\Backups\CompanyDB_Diff_' +
              CONVERT(VARCHAR(8), GETDATE(), 112) + '.bak'
    WITH DIFFERENTIAL, COMPRESSION, INIT;
END

-- Every hour: Transaction log backup
BACKUP LOG CompanyDB
TO DISK = 'C:\Backups\CompanyDB_Log_' +
          CONVERT(VARCHAR(14), GETDATE(), 112) + '.trn'
WITH COMPRESSION;

-- Delete old backups (older than 7 days)
EXECUTE master.dbo.xp_delete_file 0,
    N'C:\Backups',
    N'bak',
    N'2023-12-18T00:00:00';
```

---

## 18. Advanced Topics

### 18.1 XML Operations
```sql
-- Create XML column
CREATE TABLE Documents (
    DocID INT PRIMARY KEY,
    DocData XML
);

-- Insert XML
INSERT INTO Documents VALUES (1, '
<Employee>
    <ID>1</ID>
    <Name>John Doe</Name>
    <Department>IT</Department>
</Employee>
');

-- Query XML
SELECT
    DocID,
    DocData.value('(/Employee/Name)[1]', 'NVARCHAR(50)') AS EmployeeName
FROM Documents;

-- XQuery
SELECT
    DocData.query('/Employee/Department') AS Department
FROM Documents;

-- exist() method
SELECT *
FROM Documents
WHERE DocData.exist('/Employee[Department="IT"]') = 1;

-- nodes() method
SELECT
    T.c.value('ID[1]', 'INT') AS EmployeeID,
    T.c.value('Name[1]', 'NVARCHAR(50)') AS Name
FROM Documents
CROSS APPLY DocData.nodes('/Employee') AS T(c);

-- FOR XML
SELECT EmployeeID, FirstName, LastName
FROM Employees
FOR XML RAW;

SELECT EmployeeID, FirstName, LastName
FROM Employees
FOR XML AUTO;

SELECT EmployeeID, FirstName, LastName
FROM Employees
FOR XML PATH('Employee'), ROOT('Employees');
```

### 18.2 JSON Operations (SQL Server 2016+)
```sql
-- Create table with JSON
CREATE TABLE JsonData (
    ID INT PRIMARY KEY,
    Data NVARCHAR(MAX)
);

-- Insert JSON
INSERT INTO JsonData VALUES (1, N'{
    "Employee": {
        "ID": 1,
        "Name": "John Doe",
        "Skills": ["SQL", "C#", "Python"]
    }
}');

-- JSON_VALUE (scalar value)
SELECT
    ID,
    JSON_VALUE(Data, '$.Employee.Name') AS EmployeeName
FROM JsonData;

-- JSON_QUERY (object or array)
SELECT
    ID,
    JSON_QUERY(Data, '$.Employee.Skills') AS Skills
FROM JsonData;

-- OPENJSON
SELECT *
FROM OPENJSON('["SQL", "C#", "Python"]');

SELECT *
FROM OPENJSON('{
    "Name": "John",
    "Age": 30,
    "City": "New York"
}');

-- FOR JSON
SELECT EmployeeID, FirstName, LastName
FROM Employees
FOR JSON AUTO;

SELECT EmployeeID, FirstName, LastName
FROM Employees
FOR JSON PATH, ROOT('Employees');

-- JSON_MODIFY
UPDATE JsonData
SET Data = JSON_MODIFY(Data, '$.Employee.Name', 'Jane Doe')
WHERE ID = 1;

-- ISJSON
SELECT ISJSON('{"Name": "John"}');  -- Returns 1
SELECT ISJSON('Invalid');            -- Returns 0
```

### 18.3 Partitioning
```sql
-- Create partition function
CREATE PARTITION FUNCTION YearPartitionFunction (DATE)
AS RANGE RIGHT FOR VALUES
('2021-01-01', '2022-01-01', '2023-01-01', '2024-01-01');

-- Create partition scheme
CREATE PARTITION SCHEME YearPartitionScheme
AS PARTITION YearPartitionFunction
ALL TO ([PRIMARY]);

-- Create partitioned table
CREATE TABLE SalesHistory (
    SaleID INT,
    SaleDate DATE,
    Amount DECIMAL(10,2)
) ON YearPartitionScheme(SaleDate);

-- View partition information
SELECT
    p.partition_number,
    p.rows,
    rv.value AS boundary_value
FROM sys.partitions p
INNER JOIN sys.indexes i ON p.object_id = i.object_id AND p.index_id = i.index_id
LEFT JOIN sys.partition_range_values rv ON p.partition_number = rv.boundary_id
WHERE p.object_id = OBJECT_ID('SalesHistory')
ORDER BY p.partition_number;

-- Switch partition
ALTER TABLE SalesHistory
SWITCH PARTITION 1 TO SalesArchive PARTITION 1;
```

### 18.4 Temporal Tables (System-Versioned)
```sql
-- Create temporal table
CREATE TABLE Employees_Temporal (
    EmployeeID INT PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Salary DECIMAL(10,2),
    ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL,
    ValidTo DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.Employees_History));

-- Query current data
SELECT * FROM Employees_Temporal;

-- Query historical data
SELECT * FROM Employees_Temporal
FOR SYSTEM_TIME AS OF '2023-01-01';

SELECT * FROM Employees_Temporal
FOR SYSTEM_TIME BETWEEN '2023-01-01' AND '2023-12-31';

SELECT * FROM Employees_Temporal
FOR SYSTEM_TIME FROM '2023-01-01' TO '2023-12-31';

SELECT * FROM Employees_Temporal
FOR SYSTEM_TIME CONTAINED IN ('2023-01-01', '2023-12-31');

SELECT * FROM Employees_Temporal
FOR SYSTEM_TIME ALL;

-- Disable system versioning
ALTER TABLE Employees_Temporal SET (SYSTEM_VERSIONING = OFF);

-- Enable system versioning
ALTER TABLE Employees_Temporal
SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.Employees_History));
```

### 18.5 Graph Database Features (SQL Server 2017+)
```sql
-- Create node tables
CREATE TABLE Person (
    ID INT PRIMARY KEY,
    Name NVARCHAR(100)
) AS NODE;

CREATE TABLE City (
    ID INT PRIMARY KEY,
    CityName NVARCHAR(100)
) AS NODE;

-- Create edge table
CREATE TABLE LivesIn AS EDGE;
CREATE TABLE Knows AS EDGE;

-- Insert nodes
INSERT INTO Person VALUES (1, 'John');
INSERT INTO Person VALUES (2, 'Jane');
INSERT INTO City VALUES (1, 'New York');

-- Insert edges
INSERT INTO LivesIn
VALUES ((SELECT $node_id FROM Person WHERE ID = 1),
        (SELECT $node_id FROM City WHERE ID = 1));

INSERT INTO Knows
VALUES ((SELECT $node_id FROM Person WHERE ID = 1),
        (SELECT $node_id FROM Person WHERE ID = 2));

-- Query graph
SELECT
    Person1.Name AS Person,
    City.CityName
FROM Person AS Person1, LivesIn, City
WHERE MATCH(Person1-(LivesIn)->City);

-- Multi-hop query
SELECT
    Person1.Name AS Person1,
    Person2.Name AS Person2
FROM Person AS Person1, Knows AS K1, Person AS Person2
WHERE MATCH(Person1-(K1)->Person2);
```

### 18.6 In-Memory OLTP
```sql
-- Create memory-optimized filegroup
ALTER DATABASE CompanyDB
ADD FILEGROUP MemOptFG CONTAINS MEMORY_OPTIMIZED_DATA;

ALTER DATABASE CompanyDB
ADD FILE (
    NAME = MemOptFile,
    FILENAME = 'C:\Data\MemOptFile'
) TO FILEGROUP MemOptFG;

-- Create memory-optimized table
CREATE TABLE dbo.MemOptTable (
    ID INT NOT NULL PRIMARY KEY NONCLUSTERED,
    Value NVARCHAR(100) NOT NULL,
    INDEX IX_Value HASH (Value) WITH (BUCKET_COUNT = 1000000)
)
WITH (MEMORY_OPTIMIZED = ON, DURABILITY = SCHEMA_AND_DATA);

-- Natively compiled stored procedure
CREATE PROCEDURE dbo.InsertMemOptData
    @ID INT,
    @Value NVARCHAR(100)
WITH NATIVE_COMPILATION, SCHEMABINDING
AS
BEGIN ATOMIC WITH (
    TRANSACTION ISOLATION LEVEL = SNAPSHOT,
    LANGUAGE = N'us_english'
)
    INSERT INTO dbo.MemOptTable VALUES (@ID, @Value);
END;
```

---

## 19. Interview Questions

### 19.1 Basic Level
```
Q1: What is the difference between DELETE and TRUNCATE?
A: DELETE removes rows one by one, can have WHERE clause, can be rolled back,
   triggers fire. TRUNCATE removes all rows, faster, can't be rolled back
   (unless in transaction), doesn't fire triggers, resets identity.

Q2: What is a PRIMARY KEY?
A: Uniquely identifies each record, cannot be NULL, table can have only one.

Q3: What is a FOREIGN KEY?
A: Creates relationship between tables, references PRIMARY KEY of another table.

Q4: Difference between WHERE and HAVING?
A: WHERE filters rows before grouping, HAVING filters groups after aggregation.

Q5: What is normalization?
A: Process of organizing data to reduce redundancy and improve data integrity.

Q6: What are the types of JOINs?
A: INNER JOIN, LEFT/RIGHT/FULL OUTER JOIN, CROSS JOIN, SELF JOIN.

Q7: What is an INDEX?
A: Database object that improves query performance by allowing faster data retrieval.

Q8: Difference between UNION and UNION ALL?
A: UNION removes duplicates, UNION ALL keeps all rows including duplicates.

Q9: What is a VIEW?
A: Virtual table based on SELECT query, doesn't store data itself.

Q10: What is a STORED PROCEDURE?
A: Precompiled set of SQL statements stored in database.
```

### 19.2 Intermediate Level
```
Q1: Explain query execution order?
A: FROM > WHERE > GROUP BY > HAVING > SELECT > ORDER BY

Q2: What is the difference between clustered and non-clustered index?
A: Clustered sorts and stores data rows, one per table. Non-clustered creates
   separate structure with pointers, multiple allowed.

Q3: What are window functions?
A: Functions that perform calculations across a set of rows related to current row.

Q4: Explain isolation levels?
A: READ UNCOMMITTED, READ COMMITTED, REPEATABLE READ, SERIALIZABLE, SNAPSHOT.
   Control how transactions see changes made by other transactions.

Q5: What is a deadlock?
A: When two transactions block each other, each waiting for the other to release locks.

Q6: Difference between RANK() and DENSE_RANK()?
A: RANK() creates gaps in ranking for ties, DENSE_RANK() doesn't.

Q7: What is a CTE?
A: Common Table Expression, temporary result set for use in a query.

Q8: Explain ACID properties?
A: Atomicity, Consistency, Isolation, Durability - properties of transactions.

Q9: What is normalization vs denormalization?
A: Normalization reduces redundancy, denormalization adds redundancy for performance.

Q10: What are triggers and types?
A: Code that executes automatically on events. Types: AFTER, INSTEAD OF, DDL.
```

### 19.3 Advanced Level
```
Q1: How to find Nth highest salary?
A:
SELECT Salary
FROM (
    SELECT Salary, DENSE_RANK() OVER (ORDER BY Salary DESC) AS Rank
    FROM Employees
) AS Ranked
WHERE Rank = N;

Q2: Explain query optimization techniques?
A: Proper indexing, avoid SELECT *, use EXISTS instead of IN,
   avoid functions on indexed columns, use covering indexes,
   update statistics, analyze execution plans.

Q3: What is index fragmentation?
A: When index pages are not physically contiguous, causing performance degradation.
   Fix with REBUILD or REORGANIZE.

Q4: Explain execution plan?
A: Visual representation of how SQL Server executes a query.
   Shows operators, costs, and data flow.

Q5: What is SNAPSHOT isolation?
A: Optimistic concurrency using row versioning in tempdb.
   Readers don't block writers, writers don't block readers.

Q6: How to handle large table updates?
A: Batch updates, use indexes, consider partitioning,
   update during off-peak hours, use NOLOCK hint for reads.

Q7: What is query hint?
A: Directives to override query optimizer decisions.
   Examples: OPTION (RECOMPILE), WITH (NOLOCK), OPTION (MAXDOP 1).

Q8: Explain parameter sniffing?
A: SQL Server creates execution plan based on first parameter values.
   Can cause poor performance if data distribution varies.
   Fix: OPTION (RECOMPILE), OPTION (OPTIMIZE FOR), local variables.

Q9: What is columnstore index?
A: Column-based storage for data warehousing.
   Excellent compression and query performance for analytics.

Q10: Explain Always On Availability Groups?
A: High availability and disaster recovery solution.
   Multiple replica copies with automatic failover.
```

---

## 20. Practice Exercises

### Exercise Set 1: Basic Queries
```sql
-- 1. Find all employees earning more than average salary
SELECT * FROM Employees
WHERE Salary > (SELECT AVG(Salary) FROM Employees);

-- 2. Count employees per department
SELECT DepartmentID, COUNT(*) AS EmployeeCount
FROM Employees
GROUP BY DepartmentID;

-- 3. Find employees hired in last year
SELECT * FROM Employees
WHERE HireDate >= DATEADD(YEAR, -1, GETDATE());

-- 4. Get top 5 highest paid employees
SELECT TOP 5 * FROM Employees
ORDER BY Salary DESC;

-- 5. Find duplicate emails
SELECT Email, COUNT(*) AS Count
FROM Employees
GROUP BY Email
HAVING COUNT(*) > 1;
```

### Exercise Set 2: Intermediate Queries
```sql
-- 1. Second highest salary per department
WITH RankedSalaries AS (
    SELECT
        DepartmentID,
        Salary,
        DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS Rank
    FROM Employees
)
SELECT DepartmentID, Salary
FROM RankedSalaries
WHERE Rank = 2;

-- 2. Employees with no manager
SELECT * FROM Employees
WHERE ManagerID IS NULL;

-- 3. Running total of salaries by hire date
SELECT
    FirstName,
    LastName,
    HireDate,
    Salary,
    SUM(Salary) OVER (ORDER BY HireDate) AS RunningTotal
FROM Employees;

-- 4. Find departments with more than 5 employees
SELECT
    d.DepartmentName,
    COUNT(e.EmployeeID) AS EmployeeCount
FROM Departments d
INNER JOIN Employees e ON d.DepartmentID = e.DepartmentID
GROUP BY d.DepartmentName
HAVING COUNT(e.EmployeeID) > 5;

-- 5. Employees earning more than their department average
SELECT e.*
FROM Employees e
WHERE e.Salary > (
    SELECT AVG(Salary)
    FROM Employees
    WHERE DepartmentID = e.DepartmentID
);
```

### Exercise Set 3: Advanced Queries
```sql
-- 1. Employee hierarchy (recursive CTE)
WITH EmployeeHierarchy AS (
    SELECT
        EmployeeID,
        FirstName,
        LastName,
        ManagerID,
        0 AS Level
    FROM Employees
    WHERE ManagerID IS NULL

    UNION ALL

    SELECT
        e.EmployeeID,
        e.FirstName,
        e.LastName,
        e.ManagerID,
        eh.Level + 1
    FROM Employees e
    INNER JOIN EmployeeHierarchy eh ON e.ManagerID = eh.EmployeeID
)
SELECT * FROM EmployeeHierarchy
ORDER BY Level, EmployeeID;

-- 2. Pivot department salaries by job title
SELECT *
FROM (
    SELECT DepartmentID, JobTitle, Salary
    FROM Employees
) AS SourceTable
PIVOT (
    AVG(Salary)
    FOR JobTitle IN ([Developer], [Manager], [Analyst], [Designer])
) AS PivotTable;

-- 3. Gap and island problem (find consecutive sequences)
WITH NumberedRows AS (
    SELECT
        EmployeeID,
        HireDate,
        ROW_NUMBER() OVER (ORDER BY HireDate) -
        ROW_NUMBER() OVER (PARTITION BY DATEPART(YEAR, HireDate) ORDER BY HireDate) AS Grp
    FROM Employees
)
SELECT
    MIN(HireDate) AS StartDate,
    MAX(HireDate) AS EndDate,
    COUNT(*) AS ConsecutiveHires
FROM NumberedRows
GROUP BY Grp;

-- 4. Running difference between consecutive rows
WITH OrderedEmployees AS (
    SELECT
        EmployeeID,
        Salary,
        LAG(Salary) OVER (ORDER BY EmployeeID) AS PrevSalary
    FROM Employees
)
SELECT
    EmployeeID,
    Salary,
    PrevSalary,
    Salary - ISNULL(PrevSalary, 0) AS Difference
FROM OrderedEmployees;

-- 5. Find employees in same department with higher salary
SELECT
    e1.FirstName AS Employee,
    e1.Salary AS EmployeeSalary,
    e2.FirstName AS HigherPaidColleague,
    e2.Salary AS ColleagueSalary
FROM Employees e1
INNER JOIN Employees e2
    ON e1.DepartmentID = e2.DepartmentID
    AND e2.Salary > e1.Salary;
```

---

## Conclusion

This guide covers SQL Server from beginner to advanced level. Key takeaways:

1. **Master the Fundamentals**: SELECT, WHERE, JOINs, GROUP BY
2. **Understand Data Structures**: Proper normalization and relationships
3. **Performance Matters**: Indexes, execution plans, query optimization
4. **Security**: User management, roles, encryption
5. **Advanced Features**: CTEs, Window Functions, Temporal Tables
6. **Practice Regularly**: Hands-on experience is essential

### Next Steps:
1. Practice with real databases
2. Study execution plans
3. Learn performance tuning
4. Understand backup strategies
5. Explore advanced features
6. Work on real projects

**Good luck with your SQL Server journey!**
