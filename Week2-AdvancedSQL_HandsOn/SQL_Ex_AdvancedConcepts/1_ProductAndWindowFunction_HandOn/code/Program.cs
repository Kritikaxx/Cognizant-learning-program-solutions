using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = null!; 
    public string Category { get; set; } = null!;  
    public decimal Price { get; set; }
    public int Rank { get; set; } 
}

class Program
{
    private static string _connectionString = "Data Source=.;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;"; // Moved to class level

    static void Main(string[] args)
    {
        string rowNumberQuery = @"
            SELECT
                ProductID,
                ProductName,
                Category,
                Price,
                CalculatedRank
            FROM (
                SELECT
                    ProductID,
                    ProductName,
                    Category,
                    Price,
                    ROW_NUMBER() OVER (PARTITION BY Category ORDER BY Price DESC) AS CalculatedRank
                FROM
                    Products
            ) AS RankedProducts
            WHERE CalculatedRank <= 3
            ORDER BY Category, CalculatedRank;";

        string rankQuery = @"
            SELECT
                ProductID,
                ProductName,
                Category,
                Price,
                CalculatedRank
            FROM (
                SELECT
                    ProductID,
                    ProductName,
                    Category,
                    Price,
                    RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS CalculatedRank
                FROM
                    Products
            ) AS RankedProducts
            WHERE CalculatedRank <= 3
            ORDER BY Category, CalculatedRank;";

        string denseRankQuery = @"
            SELECT
                ProductID,
                ProductName,
                Category,
                Price,
                CalculatedRank
            FROM (
                SELECT
                    ProductID,
                    ProductName,
                    Category,
                    Price,
                    DENSE_RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS CalculatedRank
                FROM
                    Products
            ) AS RankedProducts
            WHERE CalculatedRank <= 3
            ORDER BY Category, CalculatedRank;";

        ExecuteAndPrintResults("ROW_NUMBER()", rowNumberQuery);
        ExecuteAndPrintResults("RANK()", rankQuery);
        ExecuteAndPrintResults("DENSE_RANK()", denseRankQuery);

        Console.WriteLine("\n--- All ranking queries executed. Press any key to exit. ---");
        Console.ReadKey();
    } 
    static void ExecuteAndPrintResults(string queryName, string sqlQuery)
    {
        List<Product> products = new List<Product>();
        Console.WriteLine($"\n--- {queryName} (Top 3 Most Expensive Products per Category) ---");

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Category = reader.GetString(reader.GetOrdinal("Category")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Rank = (int)reader.GetInt64(reader.GetOrdinal("CalculatedRank"))
                            });
                        }
                    }
                }
            }

            // Print the results from the list
            if (products.Count > 0)
            {
                Console.WriteLine("{0,-12} {1,-28} {2,-18} {3,-12} {4,-8}", "Product ID", "Product Name", "Category", "Price", "Rank");
                Console.WriteLine("----------------------------------------------------------------------------------");
                foreach (var product in products)
                {
                    Console.WriteLine("{0,-12} {1,-28} {2,-18} {3,-12:C} {4,-8}",
                        product.ProductID, product.ProductName, product.Category, product.Price, product.Rank);
                }
            }
            else
            {
                Console.WriteLine("No products found or query returned no results.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Error during {queryName}: {ex.Message}");
            Console.WriteLine($"Error Code: {ex.Number}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during {queryName}: {ex.Message}");
        }
    } 
} 