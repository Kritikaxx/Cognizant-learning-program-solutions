using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.IO;

class Program
{
    private static string dbFileName = "indexing_demo.db";
    private static string connectionString = $"Data Source={dbFileName}";

    static void Main(string[] args)
    {
        if (File.Exists(dbFileName))
        {
            File.Delete(dbFileName);
            Console.WriteLine($"Existing database file '{dbFileName}' deleted for a fresh start.");
        }

        InitializeDatabase();

        Console.WriteLine("\n--- Starting Indexing Exercises ---");
        Console.WriteLine("\n--- Exercise 1: Non-Clustered Index on ProductName ---");
        ExecuteExercise1();
        Console.WriteLine("\n--- Exercise 2: Clustered Index (SQLite Nuance) on OrderDate ---");
        ExecuteExercise2();
        Console.WriteLine("\n--- Exercise 3: Composite Index on CustomerID and OrderDate ---");
        ExecuteExercise3();

        Console.WriteLine("\n--- Indexing Demonstration Complete ---");
    }

    static void InitializeDatabase()
    {
        Console.WriteLine("Initializing database: Creating schema and inserting data...");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                // Database Schema
                command.CommandText = @"
                    CREATE TABLE Customers (
                        CustomerID INTEGER PRIMARY KEY, -- INTEGER PRIMARY KEY is implicitly AUTOINCREMENT and is the clustered index in SQLite
                        Name VARCHAR(100),
                        Region VARCHAR(50)
                    );

                    CREATE TABLE Products (
                        ProductID INTEGER PRIMARY KEY,
                        ProductName VARCHAR(100),
                        Category VARCHAR(50),
                        Price DECIMAL(10, 2)
                    );

                    CREATE TABLE Orders (
                        OrderID INTEGER PRIMARY KEY,
                        CustomerID INTEGER,
                        OrderDate DATE,
                        FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
                    );

                    CREATE TABLE OrderDetails (
                        OrderDetailID INTEGER PRIMARY KEY,
                        OrderID INTEGER,
                        ProductID INTEGER,
                        Quantity INTEGER,
                        FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
                        FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
                    );
                ";
                command.ExecuteNonQuery();

                // Sample Data
                command.CommandText = @"
                    INSERT INTO Customers (CustomerID, Name, Region) VALUES
                    (1, 'Alice', 'North'),
                    (2, 'Bob', 'South'),
                    (3, 'Charlie', 'East'),
                    (4, 'David', 'West');

                    INSERT INTO Products (ProductID, ProductName, Category, Price) VALUES
                    (1, 'Laptop', 'Electronics', 1200.00),
                    (2, 'Smartphone', 'Electronics', 800.00),
                    (3, 'Tablet', 'Electronics', 600.00),
                    (4, 'Headphones', 'Accessories', 150.00);

                    INSERT INTO Orders (OrderID, CustomerID, OrderDate) VALUES
                    (1, 1, '2023-01-15'),
                    (2, 2, '2023-02-20'),
                    (3, 3, '2023-03-25'),
                    (4, 4, '2023-04-30');

                    INSERT INTO OrderDetails (OrderDetailID, OrderID, ProductID, Quantity) VALUES
                    (1, 1, 1, 1),
                    (2, 2, 2, 2),
                    (3, 3, 3, 1),
                    (4, 4, 4, 3);
                ";
                command.ExecuteNonQuery();
            }
        }
        Console.WriteLine("Database initialized successfully.");
    }

    /// <param name="sql">The SQL query string.</param>
    /// <param name="description">A description of the query being executed.</param>
    /// <param name="readResults">If true, reads all results to simulate full query processing. Default is true.</param>
    static void ExecuteQuery(string sql, string description, bool readResults = true)
    {
        Console.WriteLine($"\n  Executing Query ({description}): {sql}");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                Stopwatch stopwatch = Stopwatch.StartNew();

                using (var reader = command.ExecuteReader())
                {
                    int rowCount = 0;
                    if (readResults)
                    {
                        while (reader.Read())
                        {
                            rowCount++;
                        }
                    }
                }

                stopwatch.Stop();
                Console.WriteLine($"  Query completed in: {stopwatch.Elapsed.TotalMilliseconds:F4} ms.");
            }
        }
    }

    /// <param name="sql">The SQL non-query string.</param>
    /// <param name="description">A description of the command being executed.</param>
    static void ExecuteNonQuery(string sql, string description)
    {
        Console.WriteLine($"\n  Executing Command ({description}): {sql}");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }
        Console.WriteLine($"  Command '{description}' executed successfully.");
    }

    /// <summary>
    /// Implements Exercise 1: Non-Clustered Index on ProductName.
    /// </summary>
    static void ExecuteExercise1()
    {
        ExecuteQuery("SELECT * FROM Products WHERE ProductName = 'Laptop';", "Before index on ProductName");

        ExecuteNonQuery("CREATE INDEX idx_productname ON Products (ProductName);", "Create non-clustered index on Products.ProductName");

        ExecuteQuery("SELECT * FROM Products WHERE ProductName = 'Laptop';", "After index on ProductName");
    }

    /// <summary>
    /// Implements Exercise 2: Clustered Index on OrderDate (with SQLite nuance).
    /// </summary>
    static void ExecuteExercise2()
    {
        Console.WriteLine("\n  (Note for SQLite: Explicit 'CLUSTERED' keyword is not supported. PRIMARY KEY implies clustering. For other columns, CREATE INDEX creates a non-clustered index. We are demonstrating the effect of an index on OrderDate.)");

        ExecuteQuery("SELECT * FROM Orders WHERE OrderDate = '2023-01-15';", "Before index on OrderDate");
        ExecuteNonQuery("CREATE INDEX idx_orderdate ON Orders (OrderDate);", "Create index on Orders.OrderDate");
        ExecuteQuery("SELECT * FROM Orders WHERE OrderDate = '2023-01-15';", "After index on OrderDate");
    }

    /// <summary>
    /// Implements Exercise 3: Composite Index on CustomerID and OrderDate.
    /// </summary>
    static void ExecuteExercise3()
    {
        ExecuteQuery("SELECT * FROM Orders WHERE CustomerID = 1 AND OrderDate = '2023-01-15';", "Before composite index on CustomerID, OrderDate");
        ExecuteNonQuery("CREATE INDEX idx_customerid_orderdate ON Orders (CustomerID, OrderDate);", "Create composite index on Orders (CustomerID, OrderDate)");
        ExecuteQuery("SELECT * FROM Orders WHERE CustomerID = 1 AND OrderDate = '2023-01-15';", "After composite index on CustomerID, OrderDate");
    }
}

