using System;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string Category { get; set; }

    public Product(int id, string name, string category)
    {
        ProductId = id;
        ProductName = name;
        Category = category;
    }
}

public class SearchAlgorithms
{
    // Linear Search: O(n) time complexity
    public static Product LinearSearch(Product[] products, int targetId)
    {
        foreach (var product in products)
        {
            if (product.ProductId == targetId)
                return product;
        }
        return null;
    }

    // Binary Search: O(log n) time complexity (requires sorted array)
    public static Product BinarySearch(Product[] sortedProducts, int targetId)
    {
        int left = 0;
        int right = sortedProducts.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (sortedProducts[mid].ProductId == targetId)
                return sortedProducts[mid];
                
            if (sortedProducts[mid].ProductId < targetId)
                left = mid + 1;
            else
                right = mid - 1;
        }
        return null;
    }
}

public class Program
{
    public static void Main()
    {
        // Sample product data (unsorted)
        Product[] products = {
            new Product(102, "Keyboard", "Electronics"),
            new Product(105, "Mouse", "Electronics"),
            new Product(101, "Monitor", "Electronics"),
            new Product(104, "Desk Lamp", "Home"),
            new Product(103, "Notebook", "Stationery")
        };

        // Sorted array for binary search (by ProductId)
        Product[] sortedProducts = (Product[])products.Clone();
        Array.Sort(sortedProducts, (a,b) => a.ProductId.CompareTo(b.ProductId));

        // Search demonstrations
        int searchId = 103;
        
        Console.WriteLine("Linear Search Result:");
        Product linearResult = SearchAlgorithms.LinearSearch(products, searchId);
        Console.WriteLine(linearResult != null 
            ? $"Found: {linearResult.ProductName}" 
            : "Product not found");

        Console.WriteLine("\nBinary Search Result:");
        Product binaryResult = SearchAlgorithms.BinarySearch(sortedProducts, searchId);
        Console.WriteLine(binaryResult != null 
            ? $"Found: {binaryResult.ProductName}" 
            : "Product not found");
    }
}
