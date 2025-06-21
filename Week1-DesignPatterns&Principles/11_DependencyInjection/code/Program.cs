using System;
using System.Collections.Generic; // Required for Dictionary<TKey, TValue>

namespace DependencyInjectionExample
{
   
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Customer(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return $"Customer [ID: {Id}, Name: {Name}, Email: {Email}]";
        }
    }

    
    public interface ICustomerRepository
    {
        Customer? FindCustomerById(string id); // Using nullable reference type
    }

    
    public class CustomerRepositoryImpl : ICustomerRepository
    {
        private readonly Dictionary<string, Customer> _customers;

        public CustomerRepositoryImpl()
        {
            _customers = new Dictionary<string, Customer>
            {
                { "C101", new Customer("C101", "Alice Johnson", "alice@example.com") },
                { "C102", new Customer("C102", "Bob Williams", "bob@example.com") },
                { "C103", new Customer("C103", "Charlie Brown", "charlie@example.com") }
            };
            Console.WriteLine("CustomerRepositoryImpl: Initialized with sample data.");
        }

        
        public Customer? FindCustomerById(string id)
        {
            Console.WriteLine($"CustomerRepositoryImpl: Searching for customer with ID '{id}'.");
            _customers.TryGetValue(id, out var customer);
            return customer;
        }
    }

    
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            Console.WriteLine("CustomerService: Initialized with injected Customer Repository.");
        }

        public Customer? GetCustomerDetails(string customerId)
        {
            Console.WriteLine($"CustomerService: Requesting customer details for ID '{customerId}'.");
            return _customerRepository.FindCustomerById(customerId);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Dependency Injection Example: Customer Management ---\n");

            Console.WriteLine("Client/Composition Root: Creating CustomerRepositoryImpl instance.");
            ICustomerRepository repository = new CustomerRepositoryImpl();

            Console.WriteLine("\nClient/Composition Root: Creating CustomerService instance, injecting repository.");
            CustomerService customerService = new CustomerService(repository);

            Console.WriteLine("\nClient: Using CustomerService to find a customer.");
            Customer? foundCustomer1 = customerService.GetCustomerDetails("C101");
            if (foundCustomer1 != null)
            {
                Console.WriteLine($"Client: Found customer: {foundCustomer1}");
            }
            else
            {
                Console.WriteLine("Client: Customer C101 not found.");
            }

            Console.WriteLine("\n--- Attempting to find a non-existent customer ---");
            Customer? foundCustomer2 = customerService.GetCustomerDetails("C999");
            if (foundCustomer2 != null)
            {
                Console.WriteLine($"Client: Found customer: {foundCustomer2}");
            }
            else
            {
                Console.WriteLine("Client: Customer C999 not found (as expected).");
            }

            Console.WriteLine("\n--- Dependency Injection Example Finished ---");
            Console.ReadKey(); 
        }
    }
}
