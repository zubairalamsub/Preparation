# SQL Server Advanced Performance Guide
## Deadlocks, Isolation Levels, and Performance Optimization

---

## Table of Contents
1. [Deadlocks - Complete Guide](#1-deadlocks---complete-guide)
2. [Isolation Levels - Deep Dive](#2-isolation-levels---deep-dive)
3. [Locking Mechanism](#3-locking-mechanism)
4. [Execution Plans - Mastery](#4-execution-plans---mastery)
5. [Index Optimization Strategies](#5-index-optimization-strategies)
6. [Query Performance Tuning](#6-query-performance-tuning)
7. [Statistics and Cardinality Estimation](#7-statistics-and-cardinality-estimation)
8. [Tempdb Optimization](#8-tempdb-optimization)
9. [Wait Statistics Analysis](#9-wait-statistics-analysis)
10. [Memory and Buffer Pool](#10-memory-and-buffer-pool)
11. [Advanced Concurrency Patterns](#11-advanced-concurrency-patterns)
12. [Real-World Performance Scenarios](#12-real-world-performance-scenarios)

---

## 1. Deadlocks - Complete Guide

### 1.1 What is a Deadlock?

A **deadlock** occurs when two or more transactions mutually block each other, with each transaction holding locks that the other needs, creating a circular dependency.

**Simple Example:**
```
Transaction 1          Transaction 2
-------------          -------------
Lock Table A           Lock Table B
Request Lock B  <--->  Request Lock A
[BLOCKED]              [BLOCKED]
```

Both transactions wait indefinitely for each other to release locks - this is a deadlock.

### 1.2 Deadlock Example in SQL

```sql
-- Setup tables
CREATE TABLE Accounts (
    AccountID INT PRIMARY KEY,
    Balance DECIMAL(10,2)
);

INSERT INTO Accounts VALUES (1, 1000), (2, 1000);

-- ========================================
-- SESSION 1 (Transaction 1)
-- ========================================
BEGIN TRANSACTION;

-- Step 1: Lock Account 1
UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;
WAITFOR DELAY '00:00:05';  -- Wait 5 seconds

-- Step 3: Try to lock Account 2 (will deadlock with Session 2)
UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2;

COMMIT;

-- ========================================
-- SESSION 2 (Transaction 2) - Run simultaneously
-- ========================================
BEGIN TRANSACTION;

-- Step 2: Lock Account 2
UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 2;
WAITFOR DELAY '00:00:05';  -- Wait 5 seconds

-- Step 4: Try to lock Account 1 (will deadlock with Session 1)
UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 1;

COMMIT;
```

**What happens:**
1. Transaction 1 locks Account 1
2. Transaction 2 locks Account 2
3. Transaction 1 tries to lock Account 2 → BLOCKED by Transaction 2
4. Transaction 2 tries to lock Account 1 → BLOCKED by Transaction 1
5. **DEADLOCK!** SQL Server kills one transaction (deadlock victim)

### 1.3 Deadlock Victim Selection

SQL Server automatically detects deadlocks and chooses a **victim** based on:

1. **DEADLOCK_PRIORITY** (configurable)
2. **Transaction cost** (rollback cost)
3. **Transaction duration**

```sql
-- Set deadlock priority (lower = more likely to be victim)
SET DEADLOCK_PRIORITY LOW;    -- -5
SET DEADLOCK_PRIORITY NORMAL; -- 0 (default)
SET DEADLOCK_PRIORITY HIGH;   -- 5

-- Or use numeric values (-10 to 10)
SET DEADLOCK_PRIORITY -10;  -- Most likely victim
SET DEADLOCK_PRIORITY 10;   -- Least likely victim
```

### 1.4 Detecting Deadlocks

#### Method 1: Enable Trace Flags
```sql
-- Enable deadlock graph in SQL Server Error Log
DBCC TRACEON(1222, -1);  -- Text output
DBCC TRACEON(1204, -1);  -- Detailed text output

-- View trace flag status
DBCC TRACESTATUS(1222);

-- Disable
DBCC TRACEOFF(1222, -1);
```

#### Method 2: Extended Events
```sql
-- Create Extended Event session for deadlocks
CREATE EVENT SESSION DeadlockCapture ON SERVER
ADD EVENT sqlserver.xml_deadlock_report
ADD TARGET package0.event_file(
    SET filename = N'C:\DeadlockLogs\Deadlocks.xel'
)
WITH (
    MAX_MEMORY = 4096 KB,
    EVENT_RETENTION_MODE = ALLOW_SINGLE_EVENT_LOSS,
    MAX_DISPATCH_LATENCY = 30 SECONDS,
    MAX_EVENT_SIZE = 0 KB,
    MEMORY_PARTITION_MODE = NONE,
    TRACK_CAUSALITY = OFF,
    STARTUP_STATE = ON
);

-- Start the session
ALTER EVENT SESSION DeadlockCapture ON SERVER STATE = START;

-- Query deadlock data
SELECT
    event_data.value('(event/@timestamp)[1]', 'datetime') AS DeadlockTime,
    event_data.value('(event/data[@name="xml_report"]/value)[1]', 'varchar(max)') AS DeadlockGraph
FROM (
    SELECT CAST(event_data AS XML) AS event_data
    FROM sys.fn_xe_file_target_read_file('C:\DeadlockLogs\Deadlocks*.xel', null, null, null)
) AS DeadlockData;
```

#### Method 3: System Health Session (Built-in)
```sql
-- Query system health session for deadlocks
SELECT
    XEvent.query('(event/data[@name="xml_report"]/value)[1]') AS DeadlockGraph,
    XEvent.value('(event/@timestamp)[1]', 'datetime2') AS DeadlockTime
FROM (
    SELECT CAST(target_data AS XML) AS TargetData
    FROM sys.dm_xe_session_targets st
    INNER JOIN sys.dm_xe_sessions s ON s.address = st.event_session_address
    WHERE s.name = 'system_health'
      AND st.target_name = 'ring_buffer'
) AS Data
CROSS APPLY TargetData.nodes('RingBufferTarget/event[@name="xml_deadlock_report"]') AS XEventData(XEvent)
ORDER BY DeadlockTime DESC;
```

### 1.5 Analyzing Deadlock Graphs

**Deadlock Graph Components:**
```xml
<deadlock>
  <victim-list>
    <victimProcess id="process123" />
  </victim-list>

  <process-list>
    <process id="process123" waitresource="KEY: 6:72057594038321152">
      <inputbuf>UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2</inputbuf>
      <executionStack>
        <frame procname="dbo.TransferFunds" line="15" />
      </executionStack>
    </process>

    <process id="process456" waitresource="KEY: 6:72057594038321153">
      <inputbuf>UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 1</inputbuf>
    </process>
  </process-list>

  <resource-list>
    <keylock hobtid="72057594038321152" dbid="6" objectname="Accounts"
             indexname="PK_Accounts" mode="X" />
  </resource-list>
</deadlock>
```

**Key Information:**
- **victim-list**: Which process was killed
- **process-list**: All processes involved
- **waitresource**: What resource each process is waiting for
- **inputbuf**: The SQL statement being executed
- **resource-list**: Resources involved in the deadlock

### 1.6 Preventing Deadlocks

#### Strategy 1: Access Resources in Same Order
```sql
-- BAD: Different order can cause deadlocks
-- Transaction 1: Updates Account 1, then Account 2
-- Transaction 2: Updates Account 2, then Account 1

-- GOOD: Always access in same order
CREATE PROCEDURE TransferFunds_Safe
    @FromAccount INT,
    @ToAccount INT,
    @Amount DECIMAL(10,2)
AS
BEGIN
    BEGIN TRANSACTION;

    -- Always lock accounts in ascending order
    DECLARE @FirstAccount INT = CASE WHEN @FromAccount < @ToAccount
                                     THEN @FromAccount ELSE @ToAccount END;
    DECLARE @SecondAccount INT = CASE WHEN @FromAccount < @ToAccount
                                      THEN @ToAccount ELSE @FromAccount END;

    -- Lock first account
    UPDATE Accounts WITH (UPDLOCK, ROWLOCK)
    SET Balance = Balance - CASE WHEN AccountID = @FromAccount THEN @Amount ELSE 0 END
    WHERE AccountID = @FirstAccount;

    -- Lock second account
    UPDATE Accounts WITH (UPDLOCK, ROWLOCK)
    SET Balance = Balance + CASE WHEN AccountID = @ToAccount THEN @Amount ELSE 0 END
    WHERE AccountID = @SecondAccount;

    COMMIT TRANSACTION;
END;
```

#### Strategy 2: Keep Transactions Short
```sql
-- BAD: Long transaction
BEGIN TRANSACTION;
    SELECT * FROM LargeTable;  -- Holds locks
    -- ... lots of processing ...
    WAITFOR DELAY '00:01:00';  -- Wait 1 minute
    UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;
COMMIT;

-- GOOD: Short transaction
-- Do processing outside transaction
SELECT * FROM LargeTable;
-- ... processing ...

BEGIN TRANSACTION;
    UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;
COMMIT;
```

#### Strategy 3: Use Appropriate Isolation Levels
```sql
-- Use READ COMMITTED SNAPSHOT to reduce blocking
ALTER DATABASE YourDB SET READ_COMMITTED_SNAPSHOT ON;

-- Or use SNAPSHOT isolation for read operations
SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
BEGIN TRANSACTION;
    SELECT * FROM Accounts;
COMMIT;
```

#### Strategy 4: Use UPDLOCK Hint
```sql
-- Prevent conversion deadlocks
BEGIN TRANSACTION;

    -- Get UPDLOCK immediately (instead of shared lock that converts to exclusive)
    SELECT Balance
    FROM Accounts WITH (UPDLOCK, ROWLOCK)
    WHERE AccountID = 1;

    -- Now we already have update lock, no conversion needed
    UPDATE Accounts
    SET Balance = Balance - 100
    WHERE AccountID = 1;

COMMIT;
```

#### Strategy 5: Use TRY-CATCH with Retry Logic
```sql
CREATE PROCEDURE TransferFunds_WithRetry
    @FromAccount INT,
    @ToAccount INT,
    @Amount DECIMAL(10,2)
AS
BEGIN
    DECLARE @Retries INT = 0;
    DECLARE @MaxRetries INT = 3;
    DECLARE @RetryDelay VARCHAR(8) = '00:00:01';

    WHILE @Retries < @MaxRetries
    BEGIN
        BEGIN TRY
            BEGIN TRANSACTION;

            UPDATE Accounts SET Balance = Balance - @Amount WHERE AccountID = @FromAccount;
            UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountID = @ToAccount;

            COMMIT TRANSACTION;

            RETURN 0;  -- Success
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;

            -- Check if it was a deadlock (error 1205)
            IF ERROR_NUMBER() = 1205
            BEGIN
                SET @Retries = @Retries + 1;

                IF @Retries < @MaxRetries
                BEGIN
                    -- Wait before retrying
                    WAITFOR DELAY @RetryDelay;
                    CONTINUE;
                END
            END

            -- Re-throw if not deadlock or max retries exceeded
            THROW;
        END CATCH
    END

    RETURN -1;  -- Failed after retries
END;
```

#### Strategy 6: Use Application-Level Locking
```sql
-- Use sp_getapplock for resource locking
CREATE PROCEDURE ProcessOrder
    @OrderID INT
AS
BEGIN
    DECLARE @Result INT;

    -- Get application lock
    EXEC @Result = sp_getapplock
        @Resource = 'OrderLock',
        @LockMode = 'Exclusive',
        @LockOwner = 'Transaction',
        @LockTimeout = 10000;  -- 10 seconds

    IF @Result < 0
    BEGIN
        RAISERROR('Could not acquire lock', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION;

    -- Process order
    UPDATE Orders SET Status = 'Processing' WHERE OrderID = @OrderID;

    COMMIT TRANSACTION;

    -- Lock released automatically at transaction end
END;
```

### 1.7 Deadlock Monitoring Query
```sql
-- Monitor deadlock frequency
SELECT
    CAST(target_data AS XML).value('(/RingBufferTarget/@truncated)[1]', 'bit') AS Truncated,
    CAST(target_data AS XML).value('count(/RingBufferTarget/event[@name="xml_deadlock_report"])', 'int') AS DeadlockCount,
    CAST(target_data AS XML).query('/RingBufferTarget/event[@name="xml_deadlock_report"]') AS DeadlockEvents
FROM sys.dm_xe_session_targets st
INNER JOIN sys.dm_xe_sessions s ON s.address = st.event_session_address
WHERE s.name = 'system_health'
  AND st.target_name = 'ring_buffer';
```

---

## 2. Isolation Levels - Deep Dive

### 2.1 Understanding Concurrency Problems

Before understanding isolation levels, know what problems they solve:

#### Problem 1: Dirty Read
Transaction reads uncommitted data from another transaction.
```sql
-- Transaction 1
BEGIN TRANSACTION;
UPDATE Accounts SET Balance = 500 WHERE AccountID = 1;
-- Not committed yet

-- Transaction 2 (at same time)
SELECT Balance FROM Accounts WHERE AccountID = 1;  -- Reads 500

-- Transaction 1
ROLLBACK;  -- Oops, Transaction 2 read wrong data!
```

#### Problem 2: Non-Repeatable Read
Reading the same data twice in a transaction returns different results.
```sql
-- Transaction 1
BEGIN TRANSACTION;
SELECT Balance FROM Accounts WHERE AccountID = 1;  -- Returns 1000

-- Transaction 2 (at same time)
UPDATE Accounts SET Balance = 500 WHERE AccountID = 1;
COMMIT;

-- Transaction 1
SELECT Balance FROM Accounts WHERE AccountID = 1;  -- Returns 500 (different!)
COMMIT;
```

#### Problem 3: Phantom Read
Query repeated in transaction returns different rows.
```sql
-- Transaction 1
BEGIN TRANSACTION;
SELECT COUNT(*) FROM Accounts WHERE Balance > 500;  -- Returns 5

-- Transaction 2 (at same time)
INSERT INTO Accounts VALUES (100, 1000);
COMMIT;

-- Transaction 1
SELECT COUNT(*) FROM Accounts WHERE Balance > 500;  -- Returns 6 (phantom row!)
COMMIT;
```

#### Problem 4: Lost Update
Two transactions update same data, one update is lost.
```sql
-- Transaction 1: Read balance
SELECT @Balance = Balance FROM Accounts WHERE AccountID = 1;  -- 1000
SET @Balance = @Balance - 100;  -- 900

-- Transaction 2: Read balance (at same time)
SELECT @Balance = Balance FROM Accounts WHERE AccountID = 1;  -- 1000
SET @Balance = @Balance - 200;  -- 800

-- Transaction 1: Write
UPDATE Accounts SET Balance = 900 WHERE AccountID = 1;
COMMIT;

-- Transaction 2: Write (overwrites Transaction 1's update!)
UPDATE Accounts SET Balance = 800 WHERE AccountID = 1;
COMMIT;

-- Result: Balance is 800, but should be 700 (1000-100-200)
```

### 2.2 Isolation Levels Comparison

| Isolation Level | Dirty Read | Non-Repeatable Read | Phantom Read | Lost Update | Performance |
|-----------------|------------|---------------------|--------------|-------------|-------------|
| READ UNCOMMITTED | ✓ Possible | ✓ Possible | ✓ Possible | ✓ Possible | ★★★★★ Fastest |
| READ COMMITTED | ✗ Prevented | ✓ Possible | ✓ Possible | ✓ Possible | ★★★★☆ Fast |
| REPEATABLE READ | ✗ Prevented | ✗ Prevented | ✓ Possible | ✗ Prevented | ★★★☆☆ Medium |
| SERIALIZABLE | ✗ Prevented | ✗ Prevented | ✗ Prevented | ✗ Prevented | ★★☆☆☆ Slow |
| SNAPSHOT | ✗ Prevented | ✗ Prevented | ✗ Prevented | * Special | ★★★★☆ Fast |

### 2.3 READ UNCOMMITTED (Isolation Level 0)

**Characteristics:**
- Lowest isolation level
- Reads uncommitted data (dirty reads)
- No shared locks acquired
- Not blocked by other transactions
- Fastest but least safe

**Use Case:** Reporting queries where approximate data is acceptable

```sql
-- Example demonstrating dirty read
-- Session 1
BEGIN TRANSACTION;
UPDATE Accounts SET Balance = 999 WHERE AccountID = 1;
-- Don't commit yet

-- Session 2
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SELECT Balance FROM Accounts WHERE AccountID = 1;  -- Returns 999 (uncommitted!)

-- Session 1
ROLLBACK;  -- Session 2 read incorrect data

-- Alternative syntax: NOLOCK hint
SELECT Balance FROM Accounts WITH (NOLOCK) WHERE AccountID = 1;
```

**Real-World Example:**
```sql
-- Dashboard query showing approximate counts
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT
    (SELECT COUNT(*) FROM Orders WITH (NOLOCK)) AS TotalOrders,
    (SELECT COUNT(*) FROM Orders WITH (NOLOCK) WHERE Status = 'Pending') AS PendingOrders,
    (SELECT SUM(TotalAmount) FROM Orders WITH (NOLOCK)) AS TotalRevenue;
```

**Warning:**
```sql
-- This can cause application errors!
CREATE PROCEDURE ProcessPayment
    @AccountID INT
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    -- Read balance (might be dirty!)
    DECLARE @Balance DECIMAL(10,2);
    SELECT @Balance = Balance FROM Accounts WHERE AccountID = @AccountID;

    IF @Balance >= 100
    BEGIN
        -- Deduct payment
        UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = @AccountID;
        -- Might cause negative balance if dirty read!
    END
END;
```

### 2.4 READ COMMITTED (Default - Isolation Level 1)

**Characteristics:**
- Default isolation level
- Prevents dirty reads
- Acquires shared locks for reads
- Releases shared locks immediately after read
- Allows non-repeatable reads and phantom reads

```sql
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

-- Session 1
BEGIN TRANSACTION;
UPDATE Accounts SET Balance = 500 WHERE AccountID = 1;
-- Not committed

-- Session 2
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
SELECT Balance FROM Accounts WHERE AccountID = 1;
-- BLOCKED until Session 1 commits or rolls back
-- Will NOT see uncommitted data

-- Session 1
COMMIT;

-- Now Session 2 can read the committed value
```

**Non-Repeatable Read Example:**
```sql
-- Session 1: Long running transaction
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRANSACTION;

SELECT Balance FROM Accounts WHERE AccountID = 1;  -- Returns 1000
WAITFOR DELAY '00:00:10';  -- Wait 10 seconds

-- Session 2 (meanwhile)
UPDATE Accounts SET Balance = 500 WHERE AccountID = 1;
COMMIT;

-- Session 1 continues
SELECT Balance FROM Accounts WHERE AccountID = 1;  -- Returns 500 (different!)
COMMIT;
```

**Best Practice - READ COMMITTED SNAPSHOT:**
```sql
-- Enable at database level (better than regular READ COMMITTED)
ALTER DATABASE YourDB SET READ_COMMITTED_SNAPSHOT ON;

-- Now READ COMMITTED uses row versioning (like SNAPSHOT)
-- Benefits:
-- - Readers don't block writers
-- - Writers don't block readers
-- - No dirty reads
-- - Better concurrency
```

### 2.5 REPEATABLE READ (Isolation Level 2)

**Characteristics:**
- Prevents dirty reads and non-repeatable reads
- Holds shared locks until end of transaction
- Other transactions can't modify data you've read
- Still allows phantom reads
- More locking = more blocking

```sql
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;

-- Session 1
BEGIN TRANSACTION;
SELECT Balance FROM Accounts WHERE AccountID = 1;  -- Locks this row

-- Session 2
UPDATE Accounts SET Balance = 500 WHERE AccountID = 1;
-- BLOCKED! Session 1 holds shared lock

-- Session 1
SELECT Balance FROM Accounts WHERE AccountID = 1;  -- Same value as before
COMMIT;  -- Now Session 2 can update
```

**Phantom Read Example:**
```sql
-- Session 1
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
BEGIN TRANSACTION;

SELECT COUNT(*) FROM Accounts WHERE Balance > 500;  -- Returns 5

-- Session 2
INSERT INTO Accounts VALUES (100, 1000);  -- NOT blocked!
COMMIT;

-- Session 1
SELECT COUNT(*) FROM Accounts WHERE Balance > 500;  -- Returns 6 (phantom!)
COMMIT;
```

**Use Case Example:**
```sql
-- Financial report that must be consistent
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
BEGIN TRANSACTION;

DECLARE @TotalDebit DECIMAL(15,2);
DECLARE @TotalCredit DECIMAL(15,2);

-- These must be consistent
SELECT @TotalDebit = SUM(Amount) FROM Transactions WHERE Type = 'Debit';
-- ... do some processing ...
SELECT @TotalCredit = SUM(Amount) FROM Transactions WHERE Type = 'Credit';

-- Values are guaranteed to be from same point in time
PRINT 'Difference: ' + CAST(@TotalDebit - @TotalCredit AS VARCHAR(20));

COMMIT;
```

### 2.6 SERIALIZABLE (Isolation Level 3)

**Characteristics:**
- Highest isolation level (pessimistic)
- Prevents all concurrency problems
- Holds range locks (prevents phantoms)
- Most blocking
- Worst performance
- Essentially makes transactions run sequentially

```sql
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

-- Session 1
BEGIN TRANSACTION;
SELECT COUNT(*) FROM Accounts WHERE Balance > 500;
-- Places range lock on all rows matching condition

-- Session 2
INSERT INTO Accounts VALUES (100, 1000);
-- BLOCKED! Session 1 has range lock

UPDATE Accounts SET Balance = 600 WHERE AccountID = 50;
-- BLOCKED! Even if row 50 doesn't exist yet

-- Session 1
SELECT COUNT(*) FROM Accounts WHERE Balance > 500;  -- Same count
COMMIT;  -- Now Session 2 can proceed
```

**Demonstration:**
```sql
-- Create test table
CREATE TABLE Products (ProductID INT PRIMARY KEY, Stock INT);
INSERT INTO Products VALUES (1, 100), (2, 200), (3, 50);

-- Session 1: SERIALIZABLE
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;
SELECT * FROM Products WHERE Stock > 60;  -- Returns rows 1 and 2
-- Range lock on Stock > 60

-- Session 2: Try to insert
INSERT INTO Products VALUES (4, 150);  -- BLOCKED! Falls in locked range

-- Session 2: Try to update
UPDATE Products SET Stock = 70 WHERE ProductID = 3;  -- BLOCKED! Moves into range

-- Session 1
COMMIT;  -- Session 2 can now proceed
```

**When to Use:**
```sql
-- Critical financial operations requiring absolute consistency
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;

-- Get next invoice number
DECLARE @NextInvoice INT;
SELECT @NextInvoice = MAX(InvoiceNumber) + 1 FROM Invoices;

-- No one else can insert invoices during this transaction
INSERT INTO Invoices (InvoiceNumber, CustomerID, Amount)
VALUES (@NextInvoice, 123, 500.00);

COMMIT;
```

### 2.7 SNAPSHOT (Isolation Level 4)

**Characteristics:**
- Optimistic concurrency
- Uses row versioning (stored in tempdb)
- Readers never block writers
- Writers never block readers
- Prevents dirty reads, non-repeatable reads, and phantoms
- No update conflicts (first writer wins)
- Good performance with high read concurrency

**Enable SNAPSHOT:**
```sql
-- Must enable at database level
ALTER DATABASE YourDB SET ALLOW_SNAPSHOT_ISOLATION ON;

-- Check if enabled
SELECT name, snapshot_isolation_state_desc, is_read_committed_snapshot_on
FROM sys.databases
WHERE name = 'YourDB';
```

**How It Works:**
```sql
-- Session 1: Start SNAPSHOT transaction
SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
BEGIN TRANSACTION;

-- Get current time
SELECT GETDATE() AS TransactionStartTime;  -- 2024-01-01 10:00:00

SELECT Balance FROM Accounts WHERE AccountID = 1;  -- Returns 1000

-- Session 2: Update data
UPDATE Accounts SET Balance = 500 WHERE AccountID = 1;
COMMIT;

-- Session 1: Read again
SELECT Balance FROM Accounts WHERE AccountID = 1;
-- Still returns 1000! (version from transaction start time)

COMMIT;
```

**Update Conflict Example:**
```sql
-- Session 1
SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
BEGIN TRANSACTION;

DECLARE @Balance DECIMAL(10,2);
SELECT @Balance = Balance FROM Accounts WHERE AccountID = 1;  -- 1000
SET @Balance = @Balance - 100;  -- 900

-- Session 2 (at same time)
SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
BEGIN TRANSACTION;

DECLARE @Balance DECIMAL(10,2);
SELECT @Balance = Balance FROM Accounts WHERE AccountID = 1;  -- 1000
SET @Balance = @Balance - 200;  -- 800

-- Session 2: Update first
UPDATE Accounts SET Balance = 800 WHERE AccountID = 1;
COMMIT;  -- Success

-- Session 1: Try to update
UPDATE Accounts SET Balance = 900 WHERE AccountID = 1;
-- ERROR 3960: Snapshot isolation transaction aborted due to update conflict
COMMIT;
```

**Handling Update Conflicts:**
```sql
CREATE PROCEDURE TransferFunds_Snapshot
    @FromAccount INT,
    @ToAccount INT,
    @Amount DECIMAL(10,2)
AS
BEGIN
    DECLARE @Retries INT = 0;
    DECLARE @MaxRetries INT = 3;

    WHILE @Retries < @MaxRetries
    BEGIN
        BEGIN TRY
            SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
            BEGIN TRANSACTION;

            UPDATE Accounts SET Balance = Balance - @Amount WHERE AccountID = @FromAccount;
            UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountID = @ToAccount;

            COMMIT;
            RETURN 0;  -- Success
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK;

            -- Check for snapshot conflict (error 3960)
            IF ERROR_NUMBER() = 3960
            BEGIN
                SET @Retries = @Retries + 1;
                IF @Retries < @MaxRetries
                    CONTINUE;
            END

            THROW;
        END CATCH
    END

    RAISERROR('Transaction failed after retries', 16, 1);
    RETURN -1;
END;
```

**READ COMMITTED SNAPSHOT (Hybrid):**
```sql
-- Enable at database level
ALTER DATABASE YourDB SET READ_COMMITTED_SNAPSHOT ON;

-- Now READ COMMITTED automatically uses snapshot
-- No need to change code!
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;  -- Uses snapshot underneath

BEGIN TRANSACTION;
SELECT * FROM Accounts;  -- No blocking, uses versioning
COMMIT;
```

### 2.8 Comparison with Examples

```sql
-- Setup
CREATE TABLE IsolationTest (ID INT PRIMARY KEY, Value INT);
INSERT INTO IsolationTest VALUES (1, 100);

-- ==========================================
-- READ UNCOMMITTED
-- ==========================================
-- Session 1
BEGIN TRANSACTION;
UPDATE IsolationTest SET Value = 999 WHERE ID = 1;

-- Session 2
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SELECT Value FROM IsolationTest WHERE ID = 1;  -- Returns 999 immediately

-- Session 1
ROLLBACK;

-- ==========================================
-- READ COMMITTED
-- ==========================================
-- Session 1
BEGIN TRANSACTION;
UPDATE IsolationTest SET Value = 999 WHERE ID = 1;

-- Session 2
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
SELECT Value FROM IsolationTest WHERE ID = 1;  -- BLOCKED until commit/rollback

-- Session 1
COMMIT;

-- ==========================================
-- REPEATABLE READ
-- ==========================================
-- Session 1
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
BEGIN TRANSACTION;
SELECT Value FROM IsolationTest WHERE ID = 1;  -- Locks row

-- Session 2
UPDATE IsolationTest SET Value = 999 WHERE ID = 1;  -- BLOCKED

-- Session 1
SELECT Value FROM IsolationTest WHERE ID = 1;  -- Same value
COMMIT;

-- ==========================================
-- SERIALIZABLE
-- ==========================================
-- Session 1
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;
SELECT * FROM IsolationTest WHERE Value > 50;  -- Range lock

-- Session 2
INSERT INTO IsolationTest VALUES (2, 200);  -- BLOCKED
UPDATE IsolationTest SET Value = 60 WHERE ID = 1;  -- BLOCKED

-- Session 1
COMMIT;

-- ==========================================
-- SNAPSHOT
-- ==========================================
ALTER DATABASE TestDB SET ALLOW_SNAPSHOT_ISOLATION ON;

-- Session 1
SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
BEGIN TRANSACTION;
SELECT Value FROM IsolationTest WHERE ID = 1;  -- 100

-- Session 2
UPDATE IsolationTest SET Value = 999 WHERE ID = 1;
COMMIT;

-- Session 1
SELECT Value FROM IsolationTest WHERE ID = 1;  -- Still 100!
COMMIT;
```

### 2.9 Choosing the Right Isolation Level

**Decision Tree:**

```
Is dirty read acceptable?
├── YES → READ UNCOMMITTED (reports, dashboards)
└── NO
    ├── High read concurrency needed?
    │   ├── YES → SNAPSHOT or READ COMMITTED SNAPSHOT
    │   └── NO
    │       ├── Need consistent reads within transaction?
    │       │   ├── YES
    │       │   │   ├── Need to prevent phantoms?
    │       │   │   │   ├── YES → SERIALIZABLE
    │       │   │   │   └── NO → REPEATABLE READ
    │       │   └── NO → READ COMMITTED
    │       └── Default → READ COMMITTED
```

**Common Scenarios:**

| Scenario | Recommended Level | Reason |
|----------|------------------|---------|
| Dashboard/Reports | READ UNCOMMITTED | Performance, approximate OK |
| OLTP Applications | READ COMMITTED SNAPSHOT | Good balance |
| Financial Transactions | SERIALIZABLE or SNAPSHOT | Accuracy critical |
| Inventory Management | REPEATABLE READ | Prevent lost updates |
| High-concurrency Reads | SNAPSHOT | No blocking |
| Audit Queries | SNAPSHOT | Consistent point-in-time |

---

## 3. Locking Mechanism

### 3.1 Lock Types

```sql
-- Lock Modes
/*
1. Shared (S)          - Reading data
2. Exclusive (X)       - Modifying data
3. Update (U)          - Intent to modify
4. Intent Shared (IS)  - Intent to read child resources
5. Intent Exclusive (IX) - Intent to modify child resources
6. Schema Modification (Sch-M) - DDL operations
7. Schema Stability (Sch-S) - Query compilation
8. Bulk Update (BU)    - Bulk insert operations
9. Key-Range          - Prevent phantoms
*/

-- Lock Compatibility Matrix
/*
          S    X    U    IS   IX
S         ✓    ✗    ✓    ✓    ✗
X         ✗    ✗    ✗    ✗    ✗
U         ✓    ✗    ✗    ✓    ✗
IS        ✓    ✗    ✓    ✓    ✓
IX        ✗    ✗    ✗    ✓    ✓
*/
```

### 3.2 Lock Granularity

```sql
-- Lock Hierarchy (top to bottom)
/*
Database
  ↓
Table
  ↓
Page (8KB)
  ↓
Row
  ↓
Key (index entry)
*/

-- View current locks
SELECT
    resource_type,
    resource_database_id,
    resource_description,
    request_mode,
    request_status,
    request_session_id
FROM sys.dm_tran_locks
WHERE request_session_id = @@SPID;
```

### 3.3 Lock Escalation

```sql
-- SQL Server escalates locks to reduce memory overhead
-- Row locks → Page locks → Table lock

-- Disable lock escalation on table
ALTER TABLE MyTable SET (LOCK_ESCALATION = DISABLE);

-- Set to AUTO (default)
ALTER TABLE MyTable SET (LOCK_ESCALATION = AUTO);

-- Set to TABLE (escalate to table, skip page)
ALTER TABLE MyTable SET (LOCK_ESCALATION = TABLE);

-- Monitor lock escalation
SELECT
    object_name(object_id) AS TableName,
    index_id,
    partition_number,
    lock_escalation_desc
FROM sys.tables
WHERE object_id = OBJECT_ID('MyTable');
```

### 3.4 Lock Hints (Table Hints)

```sql
-- NOLOCK (same as READ UNCOMMITTED)
SELECT * FROM Employees WITH (NOLOCK);

-- ROWLOCK (force row-level locks)
UPDATE Employees WITH (ROWLOCK)
SET Salary = Salary * 1.1
WHERE DepartmentID = 10;

-- PAGLOCK (force page-level locks)
UPDATE Employees WITH (PAGLOCK)
SET IsActive = 0
WHERE LastLoginDate < DATEADD(YEAR, -1, GETDATE());

-- TABLOCK (table-level lock)
SELECT * FROM Employees WITH (TABLOCK);

-- TABLOCKX (exclusive table lock)
TRUNCATE TABLE is essentially:
DELETE FROM Table WITH (TABLOCKX);

-- UPDLOCK (take update lock immediately)
-- Prevents deadlocks in read-then-update pattern
BEGIN TRANSACTION;
SELECT Balance
FROM Accounts WITH (UPDLOCK, ROWLOCK)
WHERE AccountID = 1;

UPDATE Accounts
SET Balance = Balance - 100
WHERE AccountID = 1;
COMMIT;

-- XLOCK (exclusive lock)
SELECT * FROM Employees WITH (XLOCK, ROWLOCK)
WHERE EmployeeID = 1;

-- HOLDLOCK (hold lock until end of transaction)
BEGIN TRANSACTION;
SELECT * FROM Employees WITH (HOLDLOCK)
WHERE DepartmentID = 10;
-- Locks held until commit
COMMIT;

-- READPAST (skip locked rows)
SELECT * FROM Orders WITH (READPAST)
WHERE Status = 'Pending';
-- Skips rows locked by other transactions

-- READCOMMITTEDLOCK (force locking even if RCSI enabled)
SELECT * FROM Employees WITH (READCOMMITTEDLOCK);
```

### 3.5 Monitoring Locks and Blocking

```sql
-- Find blocking sessions
SELECT
    blocking.session_id AS BlockingSessionId,
    blocked.session_id AS BlockedSessionId,
    blocking_text.text AS BlockingQuery,
    blocked_text.text AS BlockedQuery,
    blocked.wait_time AS WaitTimeMs,
    blocked.wait_type
FROM sys.dm_exec_requests blocked
INNER JOIN sys.dm_exec_requests blocking
    ON blocked.blocking_session_id = blocking.session_id
CROSS APPLY sys.dm_exec_sql_text(blocking.sql_handle) AS blocking_text
CROSS APPLY sys.dm_exec_sql_text(blocked.sql_handle) AS blocked_text;

-- Kill blocking session
KILL 53;  -- Use session_id from above

-- View all locks with details
SELECT
    tl.resource_type,
    DB_NAME(tl.resource_database_id) AS DatabaseName,
    CASE tl.resource_type
        WHEN 'OBJECT' THEN OBJECT_NAME(tl.resource_associated_entity_id)
        WHEN 'DATABASE' THEN 'DATABASE'
        ELSE CAST(tl.resource_associated_entity_id AS VARCHAR(20))
    END AS ObjectName,
    tl.request_mode AS LockMode,
    tl.request_status,
    tl.request_session_id,
    es.login_name,
    es.host_name,
    es.program_name,
    st.text AS Query
FROM sys.dm_tran_locks tl
LEFT JOIN sys.dm_exec_sessions es ON tl.request_session_id = es.session_id
LEFT JOIN sys.dm_exec_requests er ON es.session_id = er.session_id
OUTER APPLY sys.dm_exec_sql_text(er.sql_handle) st
WHERE tl.resource_database_id = DB_ID()
ORDER BY tl.request_session_id;

-- Lock timeout setting
SET LOCK_TIMEOUT 5000;  -- 5 seconds
SET LOCK_TIMEOUT -1;    -- Wait indefinitely (default)
SET LOCK_TIMEOUT 0;     -- Return immediately if locked

-- Example with lock timeout
BEGIN TRY
    SET LOCK_TIMEOUT 5000;

    UPDATE Accounts
    SET Balance = Balance - 100
    WHERE AccountID = 1;
END TRY
BEGIN CATCH
    IF ERROR_NUMBER() = 1222  -- Lock timeout
    BEGIN
        PRINT 'Could not acquire lock within timeout';
    END
END CATCH;
```

---

## 4. Execution Plans - Mastery

### 4.1 Understanding Execution Plans

**Execution Plan Types:**
1. **Estimated** - What optimizer thinks will happen
2. **Actual** - What actually happened

```sql
-- Show estimated execution plan
SET SHOWPLAN_XML ON;
GO
SELECT * FROM Employees WHERE DepartmentID = 10;
GO
SET SHOWPLAN_XML OFF;
GO

-- Show actual execution plan (must execute)
SET STATISTICS XML ON;
GO
SELECT * FROM Employees WHERE DepartmentID = 10;
GO
SET STATISTICS XML OFF;
GO

-- In SSMS: Ctrl+L (estimated), Ctrl+M (actual)
```

### 4.2 Reading Execution Plans

**Flow Direction:** Right to Left, Bottom to Top

```
Example Plan:

    SELECT (Cost: 0%)
         ↑
    Nested Loop Join (Cost: 45%)
         ↑
    ┌────┴────┐
    ↑         ↑
Index Seek  Table Scan
(Cost: 5%)  (Cost: 50%)
```

**Key Metrics to Check:**
- **Cost %** - Relative cost of each operator
- **Estimated vs Actual Rows** - Big difference = statistics problem
- **Warnings** - Yellow triangles indicate issues
- **Thick arrows** - More rows flowing
- **Operators** - What operation is performed

### 4.3 Common Operators

```sql
-- TABLE SCAN - Bad (reads entire table)
SELECT * FROM Employees WHERE FirstName = 'John';
-- No index on FirstName = Table Scan

-- INDEX SCAN - Better but still reads entire index
SELECT FirstName FROM Employees WHERE FirstName LIKE '%John%';
-- Can't use index seek with leading wildcard

-- INDEX SEEK - Good (uses index efficiently)
SELECT * FROM Employees WHERE EmployeeID = 100;
-- Uses clustered index

-- KEY LOOKUP (Bookmark Lookup) - Extra work
CREATE NONCLUSTERED INDEX IX_LastName ON Employees(LastName);

SELECT FirstName, LastName, Salary FROM Employees WHERE LastName = 'Smith';
-- Plan: Index Seek on IX_LastName + Key Lookup for Salary

-- Fix with covering index
CREATE NONCLUSTERED INDEX IX_LastName_Inc
ON Employees(LastName) INCLUDE (FirstName, Salary);

-- Now: Only Index Seek, no Key Lookup!

-- NESTED LOOP JOIN - Good for small datasets
SELECT e.FirstName, d.DepartmentName
FROM Employees e
INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE e.EmployeeID = 1;
-- Small result set = Nested Loop

-- MERGE JOIN - Good for large sorted datasets
SELECT e.FirstName, d.DepartmentName
FROM Employees e
INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
ORDER BY e.DepartmentID;
-- Both sorted by join key = Merge Join

-- HASH MATCH - Good for large unsorted datasets
SELECT e.FirstName, c.CityName
FROM Employees e
INNER JOIN Cities c ON e.CityID = c.CityID;
-- Large tables, no indexes = Hash Match

-- SORT - Expensive, avoid if possible
SELECT * FROM Employees ORDER BY FirstName;
-- No index on FirstName = Sort operator

-- FILTER - WHERE clause filtering
SELECT * FROM Employees WHERE Salary > 50000;

-- COMPUTE SCALAR - Calculations
SELECT FirstName, Salary * 12 AS AnnualSalary FROM Employees;
```

### 4.4 Execution Plan Warnings

```sql
-- WARNING 1: Missing Index
-- Yellow triangle with message: "Missing Index (Impact: XX%)"

SELECT * FROM Employees WHERE FirstName = 'John' AND LastName = 'Doe';

-- Get missing index details
SELECT
    mid.statement AS TableName,
    mid.equality_columns,
    mid.inequality_columns,
    mid.included_columns,
    migs.avg_user_impact,
    migs.user_seeks
FROM sys.dm_db_missing_index_details mid
INNER JOIN sys.dm_db_missing_index_groups mig ON mid.index_handle = mig.index_handle
INNER JOIN sys.dm_db_missing_index_group_stats migs ON mig.index_group_handle = migs.group_handle
WHERE database_id = DB_ID()
ORDER BY migs.avg_user_impact * migs.user_seeks DESC;

-- WARNING 2: Implicit Conversion
-- Different data types in join/where
CREATE TABLE Test1 (ID VARCHAR(10));
CREATE TABLE Test2 (ID INT);

SELECT * FROM Test1 t1
INNER JOIN Test2 t2 ON t1.ID = t2.ID;  -- Warning: Implicit conversion

-- Fix: Make data types match
ALTER TABLE Test1 ALTER COLUMN ID INT;

-- WARNING 3: Cardinality Estimate Mismatch
-- Estimated Rows: 100, Actual Rows: 10,000

-- Fix: Update statistics
UPDATE STATISTICS Employees WITH FULLSCAN;

-- WARNING 4: Unmatched Indexes
-- Index exists but can't be used
CREATE INDEX IX_Date ON Orders(OrderDate);

SELECT * FROM Orders
WHERE YEAR(OrderDate) = 2023;  -- Can't use index!

-- Fix: Don't use functions on indexed columns
SELECT * FROM Orders
WHERE OrderDate >= '2023-01-01' AND OrderDate < '2024-01-01';
```

### 4.5 Analyzing Expensive Queries

```sql
-- Find most expensive queries
SELECT TOP 20
    CAST(query_plan AS XML) AS ExecutionPlan,
    execution_count,
    total_worker_time / 1000000 AS total_cpu_seconds,
    total_elapsed_time / 1000000 AS total_elapsed_seconds,
    total_logical_reads,
    total_physical_reads,
    SUBSTRING(st.text, (qs.statement_start_offset/2) + 1,
        ((CASE qs.statement_end_offset
            WHEN -1 THEN DATALENGTH(st.text)
            ELSE qs.statement_end_offset
        END - qs.statement_start_offset)/2) + 1) AS query_text
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st
CROSS APPLY sys.dm_exec_query_plan(qs.plan_handle) qp
ORDER BY total_worker_time DESC;

-- Queries with most reads (I/O intensive)
SELECT TOP 20
    execution_count,
    total_logical_reads / execution_count AS avg_logical_reads,
    total_physical_reads / execution_count AS avg_physical_reads,
    SUBSTRING(st.text, (qs.statement_start_offset/2) + 1,
        ((CASE qs.statement_end_offset
            WHEN -1 THEN DATALENGTH(st.text)
            ELSE qs.statement_end_offset
        END - qs.statement_start_offset)/2) + 1) AS query_text
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st
ORDER BY total_logical_reads DESC;
```

### 4.6 Query Hints and Plan Guides

```sql
-- Force specific join type
SELECT e.FirstName, d.DepartmentName
FROM Employees e
INNER LOOP JOIN Departments d ON e.DepartmentID = d.DepartmentID;
-- Forces Nested Loop Join

-- Force index
SELECT * FROM Employees WITH (INDEX(IX_LastName))
WHERE LastName = 'Smith';

-- Force recompile (avoid parameter sniffing)
SELECT * FROM Employees WHERE DepartmentID = @Dept
OPTION (RECOMPILE);

-- Optimize for specific parameter value
SELECT * FROM Employees WHERE DepartmentID = @Dept
OPTION (OPTIMIZE FOR (@Dept = 10));

-- Force serial execution (no parallelism)
SELECT * FROM Employees
OPTION (MAXDOP 1);

-- Use Plan Guide for query you can't modify
EXEC sp_create_plan_guide
    @name = N'Guide1',
    @stmt = N'SELECT * FROM Employees WHERE DepartmentID = @Dept',
    @type = N'SQL',
    @module_or_batch = NULL,
    @params = N'@Dept INT',
    @hints = N'OPTION (MAXDOP 1, RECOMPILE)';
```

---

**[Continuing in next part due to length...]**

*This document continues with sections 5-12 covering Index Optimization, Query Tuning, Statistics, Tempdb, Wait Stats, Memory Management, Advanced Concurrency, and Real-World Scenarios. Would you like me to continue with the remaining sections?*

---

## Quick Reference Summary

### Deadlock Prevention Checklist
- [ ] Access resources in consistent order
- [ ] Keep transactions short
- [ ] Use appropriate isolation levels
- [ ] Implement retry logic
- [ ] Use UPDLOCK for read-then-update patterns
- [ ] Monitor with Extended Events

### Isolation Level Quick Guide
- **Reports/Dashboards** → READ UNCOMMITTED
- **OLTP Apps** → READ COMMITTED SNAPSHOT
- **Financial** → SERIALIZABLE or SNAPSHOT
- **High Concurrency** → SNAPSHOT
- **Default Safe Choice** → READ COMMITTED

### Performance Tuning Checklist
- [ ] Check execution plans
- [ ] Update statistics
- [ ] Add missing indexes
- [ ] Eliminate implicit conversions
- [ ] Avoid functions on indexed columns
- [ ] Use covering indexes
- [ ] Monitor wait statistics
- [ ] Optimize tempdb
- [ ] Check parameter sniffing
- [ ] Review isolation levels
