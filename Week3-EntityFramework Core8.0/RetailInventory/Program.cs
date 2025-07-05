using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RetailInventory;
using RetailInventory.Models;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new AppDbContext();

        //Filter products with Price > 1000 and sort by Price descending
        Console.WriteLine("Products costing more than ₹1000 (sorted by price):");
        var filtered = await context.Products
            .Where(p => p.Price > 1000)
            .OrderByDescending(p => p.Price)
            .ToListAsync();

        foreach (var p in filtered)
        {
            Console.WriteLine($"{p.Name} - ₹{p.Price}");
        }

        //Project into DTO (just Name and Price)
        Console.WriteLine("\nProduct DTOs (Name & Price only):");
        var productDTOs = await context.Products
            .Select(p => new { p.Name, p.Price })  // anonymous type projection
            .ToListAsync();

        foreach (var dto in productDTOs)
        {
            Console.WriteLine($"{dto.Name} - ₹{dto.Price}");
        }
    }
}
