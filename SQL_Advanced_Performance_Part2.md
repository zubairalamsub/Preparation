# SQL Server Advanced Performance Guide - Part 2
## Index Optimization, Query Tuning, and Advanced Performance Topics

---

## 5. Index Optimization Strategies

### 5.1 Index Design Best Practices

```sql
-- Rule 1: Choose appropriate key columns
-- Good: High selectivity (many unique values)
CREATE INDEX IX_Email ON Employees(Email);  -- Email is unique

-- Bad: Low selectivity (few unique values)
CREATE INDEX IX_Gender ON Employees(Gender);  -- Only M/F/O

-- Rule 2: Column order matters in composite indexes
-- Index on (A, B, C) can be used for:
-- - WHERE A = x
-- - WHERE A = x AND B = y
-- - WHERE A = x AND B = y AND C = z
-- But NOT for:
-- - WHERE B = x
-- - WHERE C = x

-- Example
CREATE INDEX IX_Composite ON Employees(DepartmentID, JobTitle, Salary);

-- Uses index
SELECT * FROM Employees WHERE DepartmentID = 10;
SELECT * FROM Employees WHERE DepartmentID = 10 AND JobTitle = 'Manager';
SELECT * FROM Employees WHERE DepartmentID = 10 AND JobTitle = 'Manager' AND Salary > 70000;

-- Does NOT use index efficiently
SELECT * FROM Employees WHERE JobTitle = 'Manager';  -- Missing DepartmentID
SELECT * FROM Employees WHERE Salary > 70000;  -- Missing DepartmentID and JobTitle

-- Rule 3: Use INCLUDE for covering indexes
CREATE INDEX IX_LastName_Covering
ON Employees(LastName)
INCLUDE (FirstName, Email, Salary);

-- This query is "covered" (no key lookup needed)
SELECT FirstName, Email, Salary
FROM Employees
WHERE LastName = 'Smith';

-- Rule 4: Use filtered indexes for subset queries
CREATE INDEX IX_ActiveEmployees
ON Employees(HireDate)
WHERE IsActive = 1;

-- Smaller index, only for active employees
SELECT * FROM Employees
WHERE IsActive = 1 AND HireDate > '2020-01-01';
```

### 5.2 Index Maintenance Strategies

```sql
-- Check index fragmentation
SELECT
    OBJECT_NAME(ips.object_id) AS TableName,
    i.name AS IndexName,
    ips.index_type_desc,
    ips.avg_fragmentation_in_percent,
    ips.page_count,
    ips.avg_page_space_used_in_percent,
    CASE
        WHEN ips.avg_fragmentation_in_percent < 10 THEN 'No action needed'
        WHEN ips.avg_fragmentation_in_percent < 30 THEN 'Reorganize'
        ELSE 'Rebuild'
    END AS Recommendation
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'DETAILED') ips
INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
WHERE ips.page_count > 1000  -- Only indexes with significant size
ORDER BY ips.avg_fragmentation_in_percent DESC;

-- Reorganize index (online, less intensive)
ALTER INDEX IX_LastName ON Employees REORGANIZE;

-- Rebuild index (offline by default, more thorough)
ALTER INDEX IX_LastName ON Employees REBUILD;

-- Rebuild with options
ALTER INDEX IX_LastName ON Employees REBUILD
WITH (
    FILLFACTOR = 80,           -- Leave 20% free space
    ONLINE = ON,               -- Keep table accessible
    SORT_IN_TEMPDB = ON,       -- Use tempdb for sorting
    MAXDOP = 4,                -- Max 4 parallel threads
    DATA_COMPRESSION = PAGE    -- Compress index pages
);

-- Rebuild all indexes on table
ALTER INDEX ALL ON Employees REBUILD;

-- Update statistics after rebuild
UPDATE STATISTICS Employees WITH FULLSCAN;
```

### 5.3 Automated Index Maintenance

```sql
-- Create maintenance procedure
CREATE PROCEDURE sp_MaintainIndexes
    @FragmentationThresholdReorg FLOAT = 10,
    @FragmentationThresholdRebuild FLOAT = 30,
    @MinPageCount INT = 1000
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @TableName NVARCHAR(255);
    DECLARE @IndexName NVARCHAR(255);
    DECLARE @Fragmentation FLOAT;

    DECLARE index_cursor CURSOR FOR
    SELECT
        OBJECT_NAME(ips.object_id) AS TableName,
        i.name AS IndexName,
        ips.avg_fragmentation_in_percent
    FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'SAMPLED') ips
    INNER JOIN sys.indexes i ON ips.object_id = i.object_id AND ips.index_id = i.index_id
    WHERE ips.page_count > @MinPageCount
      AND ips.avg_fragmentation_in_percent > @FragmentationThresholdReorg
      AND i.name IS NOT NULL;  -- Skip heaps

    OPEN index_cursor;
    FETCH NEXT FROM index_cursor INTO @TableName, @IndexName, @Fragmentation;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @Fragmentation >= @FragmentationThresholdRebuild
        BEGIN
            SET @SQL = 'ALTER INDEX ' + QUOTENAME(@IndexName) +
                      ' ON ' + QUOTENAME(@TableName) +
                      ' REBUILD WITH (ONLINE = ON, SORT_IN_TEMPDB = ON)';
            PRINT 'Rebuilding: ' + @TableName + '.' + @IndexName +
                  ' (Frag: ' + CAST(@Fragmentation AS VARCHAR(10)) + '%)';
        END
        ELSE
        BEGIN
            SET @SQL = 'ALTER INDEX ' + QUOTENAME(@IndexName) +
                      ' ON ' + QUOTENAME(@TableName) + ' REORGANIZE';
            PRINT 'Reorganizing: ' + @TableName + '.' + @IndexName +
                  ' (Frag: ' + CAST(@Fragmentation AS VARCHAR(10)) + '%)';
        END

        EXEC sp_executesql @SQL;

        FETCH NEXT FROM index_cursor INTO @TableName, @IndexName, @Fragmentation;
    END

    CLOSE index_cursor;
    DEALLOCATE index_cursor;
END;
GO

-- Schedule to run nightly
EXEC sp_MaintainIndexes;
```

### 5.4 Identifying Unused Indexes

```sql
-- Find indexes that are never used
SELECT
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc,
    ius.user_seeks,
    ius.user_scans,
    ius.user_lookups,
    ius.user_updates,
    (ius.user_seeks + ius.user_scans + ius.user_lookups) AS TotalReads,
    ps.used_page_count * 8 / 1024 AS SizeMB
FROM sys.indexes i
LEFT JOIN sys.dm_db_index_usage_stats ius
    ON i.object_id = ius.object_id AND i.index_id = ius.index_id
    AND ius.database_id = DB_ID()
INNER JOIN sys.dm_db_partition_stats ps
    ON i.object_id = ps.object_id AND i.index_id = ps.index_id
WHERE i.type_desc = 'NONCLUSTERED'
  AND i.is_primary_key = 0
  AND i.is_unique_constraint = 0
  AND OBJECT_SCHEMA_NAME(i.object_id) <> 'sys'
  AND (ius.user_seeks + ius.user_scans + ius.user_lookups = 0
       OR ius.user_seeks IS NULL)
  AND ius.user_updates > 0  -- Index is being maintained but not used
ORDER BY ius.user_updates DESC;

-- Drop unused indexes (after verification!)
-- DROP INDEX IX_UnusedIndex ON TableName;
```

### 5.5 Duplicate and Overlapping Indexes

```sql
-- Find duplicate indexes (same columns, same order)
WITH IndexColumns AS (
    SELECT
        i.object_id,
        i.index_id,
        i.name AS IndexName,
        STUFF((
            SELECT ',' + c.name
            FROM sys.index_columns ic
            INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
            WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id
            ORDER BY ic.key_ordinal
            FOR XML PATH('')
        ), 1, 1, '') AS KeyColumns
    FROM sys.indexes i
    WHERE i.type IN (1, 2)  -- Clustered and nonclustered
)
SELECT
    OBJECT_NAME(ic1.object_id) AS TableName,
    ic1.IndexName AS Index1,
    ic2.IndexName AS Index2,
    ic1.KeyColumns
FROM IndexColumns ic1
INNER JOIN IndexColumns ic2
    ON ic1.object_id = ic2.object_id
    AND ic1.KeyColumns = ic2.KeyColumns
    AND ic1.index_id < ic2.index_id;  -- Avoid duplicates in results

-- Find overlapping indexes
-- Index on (A, B) overlaps with (A)
-- Consider combining or using INCLUDE
```

---

## 6. Query Performance Tuning

### 6.1 Common Performance Anti-Patterns

```sql
-- ANTI-PATTERN 1: SELECT *
-- Bad
SELECT * FROM Employees WHERE DepartmentID = 10;

-- Good
SELECT EmployeeID, FirstName, LastName, Salary
FROM Employees
WHERE DepartmentID = 10;

-- ANTI-PATTERN 2: Functions on indexed columns
-- Bad (can't use index)
SELECT * FROM Orders
WHERE YEAR(OrderDate) = 2023;

-- Good (uses index)
SELECT * FROM Orders
WHERE OrderDate >= '2023-01-01' AND OrderDate < '2024-01-01';

-- ANTI-PATTERN 3: Leading wildcards
-- Bad
SELECT * FROM Employees WHERE LastName LIKE '%smith%';

-- Good (can use index)
SELECT * FROM Employees WHERE LastName LIKE 'smith%';

-- ANTI-PATTERN 4: OR conditions on different columns
-- Bad (can't use indexes efficiently)
SELECT * FROM Employees
WHERE FirstName = 'John' OR LastName = 'Smith';

-- Good (uses UNION ALL)
SELECT * FROM Employees WHERE FirstName = 'John'
UNION ALL
SELECT * FROM Employees WHERE LastName = 'Smith' AND FirstName <> 'John';

-- ANTI-PATTERN 5: NOT IN with nullable columns
-- Bad (can return unexpected results)
SELECT * FROM Employees
WHERE DepartmentID NOT IN (SELECT DepartmentID FROM Departments WHERE Location = 'Remote');

-- Good (handles NULLs correctly)
SELECT * FROM Employees e
WHERE NOT EXISTS (
    SELECT 1 FROM Departments d
    WHERE d.DepartmentID = e.DepartmentID AND d.Location = 'Remote'
);

-- ANTI-PATTERN 6: Implicit conversions
-- Bad (VARCHAR compared to INT)
CREATE TABLE Test (ID VARCHAR(10));
SELECT * FROM Test WHERE ID = 123;  -- Implicit conversion

-- Good
SELECT * FROM Test WHERE ID = '123';

-- ANTI-PATTERN 7: Scalar UDFs in SELECT
-- Bad (row-by-row execution)
CREATE FUNCTION dbo.GetDepartmentName(@DeptID INT)
RETURNS NVARCHAR(50)
AS
BEGIN
    RETURN (SELECT DepartmentName FROM Departments WHERE DepartmentID = @DeptID);
END;

SELECT EmployeeID, dbo.GetDepartmentName(DepartmentID)
FROM Employees;  -- Calls function for EVERY row!

-- Good (use JOIN)
SELECT e.EmployeeID, d.DepartmentName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID;

-- ANTI-PATTERN 8: Cursors for set-based operations
-- Bad
DECLARE @EmployeeID INT, @Salary DECIMAL(10,2);
DECLARE emp_cursor CURSOR FOR SELECT EmployeeID, Salary FROM Employees;
OPEN emp_cursor;
FETCH NEXT FROM emp_cursor INTO @EmployeeID, @Salary;
WHILE @@FETCH_STATUS = 0
BEGIN
    UPDATE Employees SET Salary = @Salary * 1.1 WHERE EmployeeID = @EmployeeID;
    FETCH NEXT FROM emp_cursor INTO @EmployeeID, @Salary;
END
CLOSE emp_cursor;
DEALLOCATE emp_cursor;

-- Good (set-based)
UPDATE Employees SET Salary = Salary * 1.1;
```

### 6.2 Optimizing Joins

```sql
-- Join optimization strategies

-- 1. Index join columns
CREATE INDEX IX_DepartmentID ON Employees(DepartmentID);
CREATE INDEX IX_DepartmentID_Dept ON Departments(DepartmentID);

-- 2. Join on same data types (avoid implicit conversion)
-- Bad
CREATE TABLE Table1 (ID VARCHAR(10));
CREATE TABLE Table2 (ID INT);
SELECT * FROM Table1 t1 INNER JOIN Table2 t2 ON t1.ID = t2.ID;  -- Conversion!

-- Good
ALTER TABLE Table1 ALTER COLUMN ID INT;

-- 3. Use appropriate join type
-- INNER JOIN - Most efficient, only matching rows
SELECT e.FirstName, d.DepartmentName
FROM Employees e
INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID;

-- LEFT JOIN - When you need all left rows
SELECT e.FirstName, d.DepartmentName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID;

-- CROSS APPLY - For function/subquery results
SELECT e.FirstName, t.TopSalary
FROM Employees e
CROSS APPLY (
    SELECT TOP 1 Salary AS TopSalary
    FROM Employees
    WHERE DepartmentID = e.DepartmentID
    ORDER BY Salary DESC
) t;

-- 4. Consider join order (SQL Server usually optimizes this)
-- Generally: smaller table first, but optimizer decides

-- 5. Use query hints when optimizer is wrong
SELECT e.FirstName, d.DepartmentName
FROM Employees e
INNER LOOP JOIN Departments d ON e.DepartmentID = d.DepartmentID;
-- Forces nested loop join
```

### 6.3 Optimizing Subqueries

```sql
-- Subquery optimization

-- 1. Use EXISTS instead of IN for better performance
-- Slower
SELECT * FROM Customers c
WHERE CustomerID IN (SELECT CustomerID FROM Orders WHERE OrderDate > '2023-01-01');

-- Faster
SELECT * FROM Customers c
WHERE EXISTS (SELECT 1 FROM Orders o WHERE o.CustomerID = c.CustomerID AND OrderDate > '2023-01-01');

-- 2. Use JOIN instead of correlated subquery
-- Slow (correlated subquery runs for each row)
SELECT
    e.FirstName,
    (SELECT DepartmentName FROM Departments d WHERE d.DepartmentID = e.DepartmentID) AS DeptName
FROM Employees e;

-- Fast (JOIN runs once)
SELECT e.FirstName, d.DepartmentName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID;

-- 3. Use CTE for readability and reusability
WITH HighEarners AS (
    SELECT DepartmentID, AVG(Salary) AS AvgSalary
    FROM Employees
    GROUP BY DepartmentID
)
SELECT e.FirstName, e.Salary, h.AvgSalary
FROM Employees e
INNER JOIN HighEarners h ON e.DepartmentID = h.DepartmentID
WHERE e.Salary > h.AvgSalary;

-- 4. Avoid nested correlated subqueries
-- Very slow
SELECT FirstName,
    (SELECT DepartmentName FROM Departments d
     WHERE d.DepartmentID = (SELECT DepartmentID FROM Employees e2 WHERE e2.EmployeeID = e.EmployeeID))
FROM Employees e;

-- Much faster
SELECT e.FirstName, d.DepartmentName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID;
```

### 6.4 Batch Processing for Large Updates

```sql
-- Update large table in batches to avoid lock escalation

-- Method 1: TOP with WHILE loop
DECLARE @BatchSize INT = 1000;
DECLARE @RowsAffected INT = @BatchSize;

WHILE @RowsAffected = @BatchSize
BEGIN
    UPDATE TOP (@BatchSize) Employees
    SET IsActive = 0
    WHERE LastLoginDate < DATEADD(YEAR, -1, GETDATE());

    SET @RowsAffected = @@ROWCOUNT;

    WAITFOR DELAY '00:00:00.100';  -- Small delay between batches
END;

-- Method 2: Use temporary table with IDs
CREATE TABLE #ToUpdate (EmployeeID INT);

INSERT INTO #ToUpdate
SELECT EmployeeID
FROM Employees
WHERE LastLoginDate < DATEADD(YEAR, -1, GETDATE());

DECLARE @BatchSize INT = 1000;

WHILE EXISTS (SELECT 1 FROM #ToUpdate)
BEGIN
    UPDATE Employees
    SET IsActive = 0
    FROM Employees e
    INNER JOIN (
        SELECT TOP (@BatchSize) EmployeeID
        FROM #ToUpdate
    ) t ON e.EmployeeID = t.EmployeeID;

    DELETE TOP (@BatchSize) FROM #ToUpdate;

    WAITFOR DELAY '00:00:00.100';
END;

DROP TABLE #ToUpdate;

-- Method 3: Delete in batches
DECLARE @Deleted INT = 1;
WHILE @Deleted > 0
BEGIN
    DELETE TOP (10000) FROM LargeTable
    WHERE CreatedDate < DATEADD(YEAR, -2, GETDATE());

    SET @Deleted = @@ROWCOUNT;
END;
```

### 6.5 Parameter Sniffing

```sql
-- Problem: Execution plan optimized for first parameter value
CREATE PROCEDURE GetEmployeesByDept
    @DepartmentID INT
AS
BEGIN
    SELECT * FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;

-- First call with DeptID = 10 (has 5 employees)
EXEC GetEmployeesByDept 10;  -- Creates plan for 5 rows (index seek)

-- Second call with DeptID = 20 (has 10,000 employees)
EXEC GetEmployeesByDept 20;  -- Uses same plan! (index seek for 10,000 rows = BAD)

-- Solution 1: OPTION (RECOMPILE)
CREATE PROCEDURE GetEmployeesByDept
    @DepartmentID INT
AS
BEGIN
    SELECT * FROM Employees
    WHERE DepartmentID = @DepartmentID
    OPTION (RECOMPILE);  -- New plan each time
END;

-- Solution 2: OPTIMIZE FOR hint
CREATE PROCEDURE GetEmployeesByDept
    @DepartmentID INT
AS
BEGIN
    SELECT * FROM Employees
    WHERE DepartmentID = @DepartmentID
    OPTION (OPTIMIZE FOR (@DepartmentID = 20));  -- Optimize for typical value
END;

-- Solution 3: Use local variable (disables parameter sniffing)
CREATE PROCEDURE GetEmployeesByDept
    @DepartmentID INT
AS
BEGIN
    DECLARE @Dept INT = @DepartmentID;

    SELECT * FROM Employees
    WHERE DepartmentID = @Dept;  -- Uses average statistics
END;

-- Solution 4: OPTIMIZE FOR UNKNOWN
CREATE PROCEDURE GetEmployeesByDept
    @DepartmentID INT
AS
BEGIN
    SELECT * FROM Employees
    WHERE DepartmentID = @DepartmentID
    OPTION (OPTIMIZE FOR (@DepartmentID UNKNOWN));  -- Uses average statistics
END;

-- Solution 5: Dynamic SQL (gets new plan each time)
CREATE PROCEDURE GetEmployeesByDept
    @DepartmentID INT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = N'SELECT * FROM Employees WHERE DepartmentID = @Dept';

    EXEC sp_executesql @SQL, N'@Dept INT', @Dept = @DepartmentID;
END;
```

---

## 7. Statistics and Cardinality Estimation

### 7.1 Understanding Statistics

```sql
-- Statistics tell optimizer about data distribution
-- Used for cardinality estimation (row count estimates)

-- View statistics on table
EXEC sp_helpstats 'Employees', 'ALL';

-- View detailed statistics
DBCC SHOW_STATISTICS('Employees', 'IX_LastName');

-- Output shows:
-- 1. Header (rows, pages, last update)
-- 2. Density (uniqueness)
-- 3. Histogram (distribution of values)

-- Create statistics manually
CREATE STATISTICS stat_DepartmentID ON Employees(DepartmentID);

-- Create multi-column statistics
CREATE STATISTICS stat_Dept_JobTitle ON Employees(DepartmentID, JobTitle);

-- Update statistics
UPDATE STATISTICS Employees;
UPDATE STATISTICS Employees WITH FULLSCAN;  -- More accurate, slower

-- Auto-update statistics (default: ON)
ALTER DATABASE YourDB SET AUTO_UPDATE_STATISTICS ON;
ALTER DATABASE YourDB SET AUTO_UPDATE_STATISTICS_ASYNC ON;  -- Update in background
```

### 7.2 Statistics Maintenance

```sql
-- Check when statistics were last updated
SELECT
    OBJECT_NAME(s.object_id) AS TableName,
    s.name AS StatisticsName,
    sp.last_updated,
    sp.rows,
    sp.rows_sampled,
    sp.modification_counter,
    CAST(sp.modification_counter * 100.0 / sp.rows AS DECIMAL(10,2)) AS PercentChanged
FROM sys.stats s
CROSS APPLY sys.dm_db_stats_properties(s.object_id, s.stats_id) sp
WHERE s.object_id = OBJECT_ID('Employees')
ORDER BY sp.modification_counter DESC;

-- Update stale statistics
UPDATE STATISTICS Employees WITH FULLSCAN, NORECOMPUTE;

-- Automated statistics update script
CREATE PROCEDURE sp_UpdateStaleStatistics
    @ModificationThreshold DECIMAL(5,2) = 10.0  -- 10% changes
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @TableName NVARCHAR(255);
    DECLARE @StatsName NVARCHAR(255);

    DECLARE stats_cursor CURSOR FOR
    SELECT
        OBJECT_NAME(s.object_id),
        s.name
    FROM sys.stats s
    CROSS APPLY sys.dm_db_stats_properties(s.object_id, s.stats_id) sp
    WHERE sp.rows > 0
      AND CAST(sp.modification_counter * 100.0 / sp.rows AS DECIMAL(10,2)) > @ModificationThreshold;

    OPEN stats_cursor;
    FETCH NEXT FROM stats_cursor INTO @TableName, @StatsName;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @SQL = 'UPDATE STATISTICS ' + QUOTENAME(@TableName) + ' ' + QUOTENAME(@StatsName) + ' WITH FULLSCAN';
        PRINT 'Updating: ' + @TableName + '.' + @StatsName;
        EXEC sp_executesql @SQL;

        FETCH NEXT FROM stats_cursor INTO @TableName, @StatsName;
    END

    CLOSE stats_cursor;
    DEALLOCATE stats_cursor;
END;
```

### 7.3 Cardinality Estimation Issues

```sql
-- Problem: Optimizer estimates wrong number of rows

-- Example: Optimizer thinks query returns 1 row, actually returns 1000
SELECT * FROM Orders WHERE OrderDate = '2023-01-01' AND CustomerID = 100;

-- Check execution plan:
-- Estimated Rows: 1
-- Actual Rows: 1000
-- = BAD plan chosen!

-- Causes:
-- 1. Stale statistics
UPDATE STATISTICS Orders WITH FULLSCAN;

-- 2. Missing statistics on filtered columns
CREATE STATISTICS stat_OrderDate_CustomerID ON Orders(OrderDate, CustomerID);

-- 3. Local variable instead of parameter (no statistics)
-- Bad
DECLARE @Date DATE = '2023-01-01';
SELECT * FROM Orders WHERE OrderDate = @Date;  -- Uses average stats

-- Good
SELECT * FROM Orders WHERE OrderDate = '2023-01-01';  -- Uses histogram

-- 4. Complex predicates
-- Optimizer can't estimate correlation between columns
SELECT * FROM Orders
WHERE OrderDate > '2023-01-01'
  AND CustomerID IN (SELECT CustomerID FROM Customers WHERE Country = 'USA');

-- Solution: Use query hints or rewrite
SELECT * FROM Orders o
INNER JOIN Customers c ON o.CustomerID = c.CustomerID
WHERE o.OrderDate > '2023-01-01'
  AND c.Country = 'USA';
```

---

## 8. Tempdb Optimization

### 8.1 Tempdb Configuration

```sql
-- Check tempdb configuration
SELECT
    name,
    physical_name,
    size * 8 / 1024 AS SizeMB,
    max_size * 8 / 1024 AS MaxSizeMB,
    growth * 8 / 1024 AS GrowthMB
FROM sys.master_files
WHERE database_id = 2;  -- 2 = tempdb

-- Best practices:
-- 1. Number of files = number of CPU cores (up to 8)
-- 2. All files same size (prevents contention)
-- 3. Use instant file initialization
-- 4. Place on fast storage (SSD)

-- Add tempdb data files
USE master;
GO

-- Add file for each CPU core
DECLARE @DataPath NVARCHAR(255) = 'D:\SQLData\';
DECLARE @LogPath NVARCHAR(255) = 'E:\SQLLog\';
DECLARE @FileSize INT = 1024;  -- MB
DECLARE @FileGrowth INT = 512;  -- MB
DECLARE @NumCores INT = 8;
DECLARE @i INT = 2;  -- Start from 2 (tempdev already exists)

WHILE @i <= @NumCores
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = 'ALTER DATABASE tempdb ADD FILE (
        NAME = tempdev' + CAST(@i AS VARCHAR(2)) + ',
        FILENAME = ''' + @DataPath + 'tempdb' + CAST(@i AS VARCHAR(2)) + '.ndf'',
        SIZE = ' + CAST(@FileSize AS VARCHAR(10)) + 'MB,
        FILEGROWTH = ' + CAST(@FileGrowth AS VARCHAR(10)) + 'MB
    )';

    EXEC sp_executesql @SQL;
    SET @i = @i + 1;
END;

-- Resize existing tempdb files to match
ALTER DATABASE tempdb MODIFY FILE (
    NAME = tempdev,
    SIZE = 1024MB,
    FILEGROWTH = 512MB
);
```

### 8.2 Tempdb Contention

```sql
-- Check for tempdb contention
SELECT
    wait_type,
    waiting_tasks_count,
    wait_time_ms,
    max_wait_time_ms,
    signal_wait_time_ms
FROM sys.dm_os_wait_stats
WHERE wait_type LIKE 'PAGELATCH%'
  AND wait_type LIKE '%2:%'  -- tempdb = database_id 2
ORDER BY wait_time_ms DESC;

-- Check allocation contention
SELECT
    session_id,
    wait_type,
    wait_duration_ms,
    blocking_session_id,
    resource_description
FROM sys.dm_os_waiting_tasks
WHERE wait_type LIKE 'PAGELATCH%'
  AND resource_description LIKE '2:%';  -- tempdb pages

-- Solutions:
-- 1. Add more tempdb data files (done above)
-- 2. Enable trace flag 1117 (equal growth)
-- 3. Enable trace flag 1118 (uniform extent allocation)

-- SQL Server 2016+: These are default
```

### 8.3 Tempdb Usage Optimization

```sql
-- Minimize tempdb usage

-- 1. Avoid temp tables when table variables suffice
-- Temp table (uses tempdb, has statistics, can have indexes)
CREATE TABLE #TempData (ID INT, Value VARCHAR(100));

-- Table variable (less tempdb overhead, no statistics)
DECLARE @TableVar TABLE (ID INT, Value VARCHAR(100));

-- 2. Use WITH (NOLOCK) for temp data queries
SELECT * FROM #TempData WITH (NOLOCK);

-- 3. Drop temp tables explicitly
DROP TABLE #TempData;

-- 4. Avoid implicit conversions in temp tables
CREATE TABLE #Orders (OrderID INT, OrderDate DATETIME2);

-- Bad: Causes spool in tempdb
SELECT * FROM Orders
WHERE CONVERT(DATE, OrderDate) = '2023-01-01';

-- Good
SELECT * FROM Orders
WHERE OrderDate >= '2023-01-01' AND OrderDate < '2023-01-02';

-- 5. Use indexed views instead of materializing in tempdb
CREATE VIEW vw_OrderSummary
WITH SCHEMABINDING
AS
SELECT
    CustomerID,
    COUNT_BIG(*) AS OrderCount,
    SUM(TotalAmount) AS TotalSales
FROM dbo.Orders
GROUP BY CustomerID;

CREATE UNIQUE CLUSTERED INDEX IX_OrderSummary ON vw_OrderSummary(CustomerID);

-- Monitor tempdb space usage
SELECT
    SUM(unallocated_extent_page_count) * 8 AS UnallocatedSpaceKB,
    SUM(version_store_reserved_page_count) * 8 AS VersionStoreKB,
    SUM(internal_object_reserved_page_count) * 8 AS InternalObjectsKB,
    SUM(user_object_reserved_page_count) * 8 AS UserObjectsKB
FROM sys.dm_db_file_space_usage;

-- Find queries using most tempdb
SELECT
    t.session_id,
    t.request_id,
    t.task_alloc_pages * 8 / 1024 AS TaskAllocMB,
    s.task_dealloc_pages * 8 / 1024 AS TaskDeallocMB,
    r.command,
    r.status,
    r.cpu_time,
    DB_NAME(r.database_id) AS DatabaseName,
    SUBSTRING(st.text, (r.statement_start_offset/2) + 1,
        ((CASE r.statement_end_offset
            WHEN -1 THEN DATALENGTH(st.text)
            ELSE r.statement_end_offset
        END - r.statement_start_offset)/2) + 1) AS query_text
FROM (
    SELECT session_id, request_id, SUM(internal_objects_alloc_page_count) AS task_alloc_pages
    FROM sys.dm_db_task_space_usage
    GROUP BY session_id, request_id
) t
INNER JOIN (
    SELECT session_id, SUM(internal_objects_dealloc_page_count) AS task_dealloc_pages
    FROM sys.dm_db_task_space_usage
    GROUP BY session_id
) s ON t.session_id = s.session_id
INNER JOIN sys.dm_exec_requests r ON t.session_id = r.session_id AND t.request_id = r.request_id
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) st
ORDER BY t.task_alloc_pages DESC;
```

---

## 9. Wait Statistics Analysis

### 9.1 Understanding Wait Statistics

```sql
-- Wait statistics show WHERE SQL Server spends time waiting

-- Top wait types
SELECT TOP 20
    wait_type,
    wait_time_ms / 1000.0 / 60 AS wait_time_minutes,
    waiting_tasks_count,
    wait_time_ms / waiting_tasks_count AS avg_wait_ms,
    CAST(100.0 * wait_time_ms / SUM(wait_time_ms) OVER() AS DECIMAL(5,2)) AS PercentOfTotal
FROM sys.dm_os_wait_stats
WHERE wait_type NOT IN (
    -- Filter out benign waits
    'CLR_SEMAPHORE', 'LAZYWRITER_SLEEP', 'RESOURCE_QUEUE', 'SLEEP_TASK',
    'SLEEP_SYSTEMTASK', 'SQLTRACE_BUFFER_FLUSH', 'WAITFOR', 'LOGMGR_QUEUE',
    'CHECKPOINT_QUEUE', 'REQUEST_FOR_DEADLOCK_SEARCH', 'XE_TIMER_EVENT',
    'BROKER_TO_FLUSH', 'BROKER_TASK_STOP', 'CLR_MANUAL_EVENT', 'CLR_AUTO_EVENT',
    'DISPATCHER_QUEUE_SEMAPHORE', 'FT_IFTS_SCHEDULER_IDLE_WAIT', 'XE_DISPATCHER_WAIT',
    'XE_DISPATCHER_JOIN', 'SQLTRACE_INCREMENTAL_FLUSH_SLEEP', 'ONDEMAND_TASK_QUEUE',
    'BROKER_EVENTHANDLER', 'SLEEP_BPOOL_FLUSH', 'DIRTY_PAGE_POLL', 'HADR_FILESTREAM_IOMGR_IOCOMPLETION'
)
ORDER BY wait_time_ms DESC;

-- Reset wait statistics (use carefully!)
DBCC SQLPERF('sys.dm_os_wait_stats', CLEAR);
```

### 9.2 Common Wait Types and Solutions

```sql
-- PAGEIOLATCH_* - I/O bottleneck
-- Cause: Slow disk I/O
-- Solutions:
-- 1. Add more memory (reduce physical reads)
-- 2. Faster storage (SSD)
-- 3. Better indexes
-- 4. Data compression

-- Check I/O statistics
SELECT
    DB_NAME(database_id) AS DatabaseName,
    FILE_NAME(file_id) AS FileName,
    num_of_reads,
    num_of_writes,
    io_stall_read_ms / NULLIF(num_of_reads, 0) AS avg_read_latency_ms,
    io_stall_write_ms / NULLIF(num_of_writes, 0) AS avg_write_latency_ms
FROM sys.dm_io_virtual_file_stats(NULL, NULL)
ORDER BY io_stall_read_ms DESC;

-- LCK_M_* - Locking/blocking
-- Cause: Transactions holding locks too long
-- Solutions:
-- 1. Optimize queries (shorter transactions)
-- 2. Better indexes
-- 3. READ COMMITTED SNAPSHOT isolation
-- 4. Review application logic

-- Find blocking queries
SELECT
    blocking.session_id AS BlockingSessionID,
    blocked.session_id AS BlockedSessionID,
    blocking_text.text AS BlockingQuery,
    blocked_text.text AS BlockedQuery,
    blocked.wait_time / 1000 AS WaitTimeSeconds,
    blocked.wait_type
FROM sys.dm_exec_requests blocked
INNER JOIN sys.dm_exec_requests blocking
    ON blocked.blocking_session_id = blocking.session_id
CROSS APPLY sys.dm_exec_sql_text(blocking.sql_handle) blocking_text
CROSS APPLY sys.dm_exec_sql_text(blocked.sql_handle) blocked_text;

-- CXPACKET / CXCONSUMER - Parallelism waits
-- Cause: Parallel query execution coordination
-- Solutions:
-- 1. Increase "cost threshold for parallelism" (default 5, try 50)
-- 2. Reduce MAXDOP (max degree of parallelism)
-- 3. Update statistics
-- 4. Rewrite query

EXEC sp_configure 'cost threshold for parallelism', 50;
EXEC sp_configure 'max degree of parallelism', 4;
RECONFIGURE;

-- SOS_SCHEDULER_YIELD - CPU pressure
-- Cause: High CPU usage
-- Solutions:
-- 1. Optimize queries (reduce CPU usage)
-- 2. Add more CPU cores
-- 3. Fix parameter sniffing
-- 4. Update statistics

-- Find CPU-intensive queries
SELECT TOP 20
    qs.total_worker_time / 1000000 AS total_cpu_seconds,
    qs.execution_count,
    qs.total_worker_time / qs.execution_count / 1000 AS avg_cpu_ms,
    SUBSTRING(st.text, (qs.statement_start_offset/2) + 1,
        ((CASE qs.statement_end_offset
            WHEN -1 THEN DATALENGTH(st.text)
            ELSE qs.statement_end_offset
        END - qs.statement_start_offset)/2) + 1) AS query_text
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st
ORDER BY total_cpu_seconds DESC;

-- WRITELOG - Transaction log write waits
-- Cause: Slow log file I/O
-- Solutions:
-- 1. Put log file on faster disk (dedicated, SSD)
-- 2. Batch smaller transactions
-- 3. Reduce transaction log size
-- 4. Use delayed durability (careful!)

-- ASYNC_NETWORK_IO - Client not consuming results fast enough
-- Cause: Application not reading data quickly
-- Solutions:
-- 1. Fix application (read data faster)
-- 2. Return less data (pagination)
-- 3. Use SELECT TOP
-- 4. Review application architecture

-- RESOURCE_SEMAPHORE - Memory grant waits
-- Cause: Queries waiting for memory to run
-- Solutions:
-- 1. Add more memory
-- 2. Optimize queries (reduce memory needs)
-- 3. Update statistics
-- 4. Create better indexes

-- Check memory grants
SELECT
    session_id,
    requested_memory_kb / 1024 AS requested_memory_mb,
    granted_memory_kb / 1024 AS granted_memory_mb,
    used_memory_kb / 1024 AS used_memory_mb,
    query_cost,
    SUBSTRING(st.text, (r.statement_start_offset/2) + 1,
        ((CASE r.statement_end_offset
            WHEN -1 THEN DATALENGTH(st.text)
            ELSE r.statement_end_offset
        END - r.statement_start_offset)/2) + 1) AS query_text
FROM sys.dm_exec_query_memory_grants qmg
INNER JOIN sys.dm_exec_requests r ON qmg.session_id = r.session_id
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) st
ORDER BY requested_memory_kb DESC;
```

### 9.3 Continuous Wait Stats Monitoring

```sql
-- Create wait stats baseline table
CREATE TABLE WaitStatsBaseline (
    CaptureTime DATETIME2 DEFAULT SYSDATETIME(),
    wait_type NVARCHAR(60),
    wait_time_ms BIGINT,
    waiting_tasks_count BIGINT
);

-- Capture baseline
INSERT INTO WaitStatsBaseline (wait_type, wait_time_ms, waiting_tasks_count)
SELECT wait_type, wait_time_ms, waiting_tasks_count
FROM sys.dm_os_wait_stats;

-- Compare current vs baseline
WITH CurrentWaits AS (
    SELECT wait_type, wait_time_ms, waiting_tasks_count
    FROM sys.dm_os_wait_stats
),
BaselineWaits AS (
    SELECT wait_type, MAX(wait_time_ms) AS wait_time_ms, MAX(waiting_tasks_count) AS waiting_tasks_count
    FROM WaitStatsBaseline
    WHERE CaptureTime = (SELECT MAX(CaptureTime) FROM WaitStatsBaseline)
    GROUP BY wait_type
)
SELECT TOP 20
    c.wait_type,
    (c.wait_time_ms - ISNULL(b.wait_time_ms, 0)) / 1000.0 AS wait_time_seconds_delta,
    c.waiting_tasks_count - ISNULL(b.waiting_tasks_count, 0) AS waiting_tasks_delta
FROM CurrentWaits c
LEFT JOIN BaselineWaits b ON c.wait_type = b.wait_type
WHERE c.wait_time_ms - ISNULL(b.wait_time_ms, 0) > 0
ORDER BY wait_time_seconds_delta DESC;
```

---

## 10. Memory and Buffer Pool

### 10.1 Memory Configuration

```sql
-- Check current memory settings
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'max server memory (MB)';
EXEC sp_configure 'min server memory (MB)';

-- Set max server memory (recommended: Total RAM - OS reserve)
-- For dedicated SQL Server:
-- 64GB RAM → 56-58GB for SQL Server
-- 32GB RAM → 28-29GB for SQL Server
-- 16GB RAM → 12-13GB for SQL Server

EXEC sp_configure 'max server memory (MB)', 57344;  -- 56GB
RECONFIGURE;

-- Set min server memory (prevents shrinking)
EXEC sp_configure 'min server memory (MB)', 40960;  -- 40GB
RECONFIGURE;
```

### 10.2 Buffer Pool Analysis

```sql
-- Buffer pool usage by database
SELECT
    DB_NAME(database_id) AS DatabaseName,
    COUNT(*) * 8 / 1024 AS BufferSizeMB,
    CAST(100.0 * COUNT(*) / (SELECT COUNT(*) FROM sys.dm_os_buffer_descriptors) AS DECIMAL(5,2)) AS PercentOfTotal
FROM sys.dm_os_buffer_descriptors
WHERE database_id <> 32767  -- Exclude ResourceDB
GROUP BY database_id
ORDER BY BufferSizeMB DESC;

-- Buffer pool usage by object
SELECT
    OBJECT_NAME(p.object_id) AS ObjectName,
    i.name AS IndexName,
    i.type_desc,
    COUNT(*) * 8 / 1024 AS BufferSizeMB
FROM sys.dm_os_buffer_descriptors bd
INNER JOIN sys.allocation_units au ON bd.allocation_unit_id = au.allocation_unit_id
INNER JOIN sys.partitions p ON au.container_id = p.hobt_id
INNER JOIN sys.indexes i ON p.object_id = i.object_id AND p.index_id = i.index_id
WHERE bd.database_id = DB_ID()
GROUP BY p.object_id, i.name, i.type_desc
ORDER BY BufferSizeMB DESC;

-- Page life expectancy (should be > 300 seconds)
SELECT
    object_name,
    counter_name,
    cntr_value AS PageLifeExpectancySeconds
FROM sys.dm_os_performance_counters
WHERE object_name LIKE '%Buffer Manager%'
  AND counter_name = 'Page life expectancy';

-- Buffer cache hit ratio (should be > 95%)
SELECT
    (a.cntr_value * 1.0 / b.cntr_value) * 100.0 AS BufferCacheHitRatio
FROM sys.dm_os_performance_counters a
JOIN (
    SELECT cntr_value, OBJECT_NAME
    FROM sys.dm_os_performance_counters
    WHERE counter_name = 'Buffer cache hit ratio base'
) b ON a.OBJECT_NAME = b.OBJECT_NAME
WHERE a.counter_name = 'Buffer cache hit ratio'
  AND a.OBJECT_NAME LIKE '%Buffer Manager%';
```

### 10.3 Memory Pressure Detection

```sql
-- Check memory clerk usage
SELECT TOP 20
    type,
    SUM(pages_kb) / 1024 AS SizeMB,
    SUM(pages_kb) * 100.0 / (SELECT SUM(pages_kb) FROM sys.dm_os_memory_clerks) AS PercentOfTotal
FROM sys.dm_os_memory_clerks
GROUP BY type
ORDER BY SizeMB DESC;

-- Memory grants (queries waiting for memory)
SELECT
    session_id,
    wait_time_ms / 1000 AS wait_time_seconds,
    requested_memory_kb / 1024 AS requested_memory_mb,
    SUBSTRING(st.text, (r.statement_start_offset/2) + 1,
        ((CASE r.statement_end_offset
            WHEN -1 THEN DATALENGTH(st.text)
            ELSE r.statement_end_offset
        END - r.statement_start_offset)/2) + 1) AS query_text
FROM sys.dm_exec_query_memory_grants qmg
INNER JOIN sys.dm_exec_requests r ON qmg.session_id = r.session_id
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) st
WHERE wait_time_ms > 0
ORDER BY wait_time_ms DESC;

-- Memory pressure indicators
SELECT
    CASE
        WHEN memory_state_desc = 'AVAILABLE_PHYSICAL_MEMORY_LOW' THEN 'WARNING: Low physical memory'
        WHEN memory_state_desc = 'AVAILABLE_VIRTUAL_MEMORY_LOW' THEN 'WARNING: Low virtual memory'
        WHEN memory_state_desc = 'AVAILABLE_COMMITTED_MEMORY_LOW' THEN 'WARNING: Low committed memory'
        ELSE 'OK'
    END AS MemoryStatus,
    available_physical_memory_kb / 1024 AS AvailablePhysicalMemoryMB,
    available_committed_limit_kb / 1024 AS AvailableCommittedLimitMB
FROM sys.dm_os_sys_memory;
```

---

## 11. Advanced Concurrency Patterns

### 11.1 Optimistic Concurrency with Rowversion

```sql
-- Use rowversion (timestamp) for optimistic concurrency
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(100),
    Stock INT,
    Price DECIMAL(10,2),
    RowVersion ROWVERSION  -- Automatically updated
);

-- Application reads rowversion
DECLARE @OriginalVersion VARBINARY(8);

SELECT
    @OriginalVersion = RowVersion,
    @Stock = Stock
FROM Products
WHERE ProductID = 1;

-- Later, update only if version hasn't changed
UPDATE Products
SET Stock = Stock - 10
WHERE ProductID = 1
  AND RowVersion = @OriginalVersion;  -- Optimistic lock

IF @@ROWCOUNT = 0
BEGIN
    -- Someone else modified the row!
    RAISERROR('Record was modified by another user', 16, 1);
END;
```

### 11.2 Queue Processing Pattern

```sql
-- Efficient queue processing with READPAST
CREATE TABLE MessageQueue (
    MessageID INT IDENTITY PRIMARY KEY,
    Message NVARCHAR(MAX),
    ProcessedBy INT NULL,
    ProcessedAt DATETIME2 NULL,
    Status NVARCHAR(20) DEFAULT 'Pending'
);

-- Multiple workers can process queue without blocking
CREATE PROCEDURE ProcessNextMessage
AS
BEGIN
    DECLARE @MessageID INT;
    DECLARE @Message NVARCHAR(MAX);

    BEGIN TRANSACTION;

    -- Get next available message (skip locked rows)
    SELECT TOP 1
        @MessageID = MessageID,
        @Message = Message
    FROM MessageQueue WITH (ROWLOCK, READPAST)
    WHERE Status = 'Pending'
    ORDER BY MessageID;

    IF @MessageID IS NOT NULL
    BEGIN
        -- Mark as being processed
        UPDATE MessageQueue
        SET Status = 'Processing',
            ProcessedBy = @@SPID,
            ProcessedAt = SYSDATETIME()
        WHERE MessageID = @MessageID;

        COMMIT TRANSACTION;

        -- Process message (outside transaction)
        EXEC ProcessMessage @Message;

        -- Mark as complete
        UPDATE MessageQueue
        SET Status = 'Completed'
        WHERE MessageID = @MessageID;
    END
    ELSE
    BEGIN
        ROLLBACK TRANSACTION;
    END
END;
```

### 11.3 Application Locks

```sql
-- Use application-defined locks for custom concurrency control
CREATE PROCEDURE ProcessOrder
    @OrderID INT
AS
BEGIN
    DECLARE @LockResult INT;
    DECLARE @ResourceName NVARCHAR(255) = 'Order_' + CAST(@OrderID AS VARCHAR(10));

    -- Try to get exclusive lock
    EXEC @LockResult = sp_getapplock
        @Resource = @ResourceName,
        @LockMode = 'Exclusive',
        @LockOwner = 'Transaction',
        @LockTimeout = 10000;  -- Wait up to 10 seconds

    IF @LockResult < 0
    BEGIN
        -- Lock acquisition failed
        RAISERROR('Could not acquire lock on order', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION;

    -- Process order (guaranteed exclusive access)
    UPDATE Orders SET Status = 'Processing' WHERE OrderID = @OrderID;

    -- ... more processing ...

    COMMIT TRANSACTION;
    -- Lock automatically released
END;

-- Release lock explicitly if needed
EXEC sp_releaseapplock @Resource = 'Order_123', @LockOwner = 'Transaction';
```

---

## 12. Real-World Performance Scenarios

### Scenario 1: Slow Report Query

```sql
-- Problem: Monthly sales report takes 5 minutes
SELECT
    d.DepartmentName,
    COUNT(*) AS OrderCount,
    SUM(od.Quantity * od.UnitPrice) AS TotalSales
FROM Orders o
INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
INNER JOIN Products p ON od.ProductID = p.ProductID
INNER JOIN Employees e ON o.EmployeeID = e.EmployeeID
INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
WHERE o.OrderDate >= '2023-01-01' AND o.OrderDate < '2023-02-01'
GROUP BY d.DepartmentName;

-- Solution 1: Add indexes
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate) INCLUDE (EmployeeID);
CREATE INDEX IX_OrderDetails_OrderID ON OrderDetails(OrderID) INCLUDE (ProductID, Quantity, UnitPrice);

-- Solution 2: Use indexed view (materialized)
CREATE VIEW vw_OrderSummary
WITH SCHEMABINDING
AS
SELECT
    e.DepartmentID,
    YEAR(o.OrderDate) AS OrderYear,
    MONTH(o.OrderDate) AS OrderMonth,
    COUNT_BIG(*) AS OrderCount,
    SUM(od.Quantity * od.UnitPrice) AS TotalSales
FROM dbo.Orders o
INNER JOIN dbo.OrderDetails od ON o.OrderID = od.OrderID
INNER JOIN dbo.Employees e ON o.EmployeeID = e.EmployeeID
GROUP BY e.DepartmentID, YEAR(o.OrderDate), MONTH(o.OrderDate);

CREATE UNIQUE CLUSTERED INDEX IX_OrderSummary
ON vw_OrderSummary(DepartmentID, OrderYear, OrderMonth);

-- Now query is instant
SELECT
    d.DepartmentName,
    vs.OrderCount,
    vs.TotalSales
FROM vw_OrderSummary vs
INNER JOIN Departments d ON vs.DepartmentID = d.DepartmentID
WHERE OrderYear = 2023 AND OrderMonth = 1;
```

### Scenario 2: Application Timeout on Updates

```sql
-- Problem: UPDATE times out after 30 seconds
UPDATE Customers
SET LastPurchaseDate = GETDATE()
WHERE CustomerID IN (
    SELECT DISTINCT CustomerID
    FROM Orders
    WHERE OrderDate >= DATEADD(DAY, -1, GETDATE())
);

-- Solution 1: Rewrite to eliminate subquery
UPDATE c
SET LastPurchaseDate = GETDATE()
FROM Customers c
INNER JOIN (
    SELECT DISTINCT CustomerID
    FROM Orders
    WHERE OrderDate >= DATEADD(DAY, -1, GETDATE())
) o ON c.CustomerID = o.CustomerID;

-- Solution 2: Batch updates
DECLARE @BatchSize INT = 1000;
WHILE 1 = 1
BEGIN
    UPDATE TOP (@BatchSize) c
    SET LastPurchaseDate = GETDATE()
    FROM Customers c
    WHERE EXISTS (
        SELECT 1
        FROM Orders o
        WHERE o.CustomerID = c.CustomerID
          AND o.OrderDate >= DATEADD(DAY, -1, GETDATE())
    )
    AND c.LastPurchaseDate < DATEADD(DAY, -1, GETDATE());

    IF @@ROWCOUNT < @BatchSize BREAK;

    WAITFOR DELAY '00:00:00.100';
END;
```

### Scenario 3: Deadlocks in Order Processing

```sql
-- Problem: Frequent deadlocks between order creation and inventory update

-- Bad code (causes deadlocks)
-- Transaction 1: Create Order 100
BEGIN TRANSACTION;
INSERT INTO Orders (OrderID, CustomerID) VALUES (100, 1);
UPDATE Inventory SET Stock = Stock - 10 WHERE ProductID = 50;
COMMIT;

-- Transaction 2: Create Order 101 (same time)
BEGIN TRANSACTION;
INSERT INTO Orders (OrderID, CustomerID) VALUES (101, 2);
UPDATE Inventory SET Stock = Stock - 5 WHERE ProductID = 50;
COMMIT;
-- DEADLOCK on Inventory table!

-- Solution: Access resources in consistent order
CREATE PROCEDURE CreateOrder_Safe
    @OrderID INT,
    @CustomerID INT,
    @ProductID INT,
    @Quantity INT
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
    BEGIN TRANSACTION;

    -- Step 1: Lock inventory first (consistent order)
    UPDATE Inventory WITH (UPDLOCK, ROWLOCK)
    SET Stock = Stock - @Quantity
    WHERE ProductID = @ProductID;

    -- Check stock
    IF (SELECT Stock FROM Inventory WHERE ProductID = @ProductID) < 0
    BEGIN
        ROLLBACK;
        RAISERROR('Insufficient stock', 16, 1);
        RETURN;
    END

    -- Step 2: Create order
    INSERT INTO Orders (OrderID, CustomerID)
    VALUES (@OrderID, @CustomerID);

    COMMIT TRANSACTION;
END;
```

---

## Performance Tuning Checklist

### Database Level
- [ ] Set appropriate memory limits (max/min server memory)
- [ ] Configure tempdb properly (multiple files, SSD)
- [ ] Enable READ_COMMITTED_SNAPSHOT for OLTP
- [ ] Set cost threshold for parallelism (50+)
- [ ] Configure max degree of parallelism appropriately
- [ ] Enable instant file initialization
- [ ] Place data/log files on separate drives
- [ ] Set appropriate recovery model
- [ ] Configure backup retention policy

### Index Level
- [ ] Create indexes on foreign keys
- [ ] Create covering indexes for frequent queries
- [ ] Use filtered indexes for subset queries
- [ ] Remove unused indexes
- [ ] Consolidate duplicate/overlapping indexes
- [ ] Rebuild fragmented indexes (>30%)
- [ ] Reorganize moderately fragmented indexes (10-30%)
- [ ] Update statistics regularly
- [ ] Check for missing indexes (DMVs)

### Query Level
- [ ] Avoid SELECT *
- [ ] Avoid functions on indexed columns in WHERE
- [ ] Use EXISTS instead of IN for subqueries
- [ ] Use JOINS instead of correlated subqueries
- [ ] Avoid leading wildcards in LIKE
- [ ] Use appropriate data types (no implicit conversion)
- [ ] Keep transactions short
- [ ] Batch large updates/deletes
- [ ] Use appropriate isolation level
- [ ] Avoid cursors for set-based operations

### Monitoring
- [ ] Review execution plans regularly
- [ ] Monitor wait statistics
- [ ] Track blocking/deadlocks
- [ ] Monitor tempdb usage
- [ ] Check buffer cache hit ratio (>95%)
- [ ] Check page life expectancy (>300 sec)
- [ ] Review slow query log
- [ ] Monitor disk I/O latency
- [ ] Track CPU and memory usage
- [ ] Set up performance baseline

---

**End of Advanced Performance Guide**

For questions or more details on specific topics, consult SQL Server documentation or performance tuning resources.
