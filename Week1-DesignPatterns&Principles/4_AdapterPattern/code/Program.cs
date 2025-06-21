using System;

namespace AdapterPatternExample
{
    public interface IPaymentProcessor
    {
        void ProcessPayment(decimal amount);
    }

    public class OldPaymentGateway
    {
        public void MakePayment(double amountInDollars)
        {
            Console.WriteLine($"[OldPaymentGateway] Processing payment of ${amountInDollars:F2} using legacy system.");
            Console.WriteLine("[OldPaymentGateway] Transaction completed successfully via old system.");
        }
    }

    public class NewPaymentService
    {
        public void ExecuteTransaction(decimal transactionAmount, string currency)
        {
            Console.WriteLine($"[NewPaymentService] Executing transaction for {transactionAmount:F2} {currency}.");
            Console.WriteLine("[NewPaymentService] Transaction approved by new service.");
        }
    }

    public class OldPaymentGatewayAdapter : IPaymentProcessor
    {
        private readonly OldPaymentGateway _oldGateway;
        public OldPaymentGatewayAdapter(OldPaymentGateway oldGateway)
        {
            _oldGateway = oldGateway;
        }
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine("--- Using OldPaymentGatewayAdapter ---");
            _oldGateway.MakePayment((double)amount); 
            Console.WriteLine("--- OldPaymentGatewayAdapter finished ---\n");
        }
    }
    public class NewPaymentServiceAdapter : IPaymentProcessor
    {
        private readonly NewPaymentService _newService;

        public NewPaymentServiceAdapter(NewPaymentService newService)
        {
            _newService = newService;
        }
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine("--- Using NewPaymentServiceAdapter ---");
            _newService.ExecuteTransaction(amount, "USD"); 
            Console.WriteLine("--- NewPaymentServiceAdapter finished ---\n");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Adapter Pattern Example: Payment Processing System ---\n");

            // Scenario 1: Process payment using the Old Payment Gateway via its Adapter
            Console.WriteLine("Client wants to process payment via Old Payment Gateway:");
            OldPaymentGateway oldGateway = new OldPaymentGateway();
            IPaymentProcessor oldGatewayAdapter = new OldPaymentGatewayAdapter(oldGateway);
            oldGatewayAdapter.ProcessPayment(100.50m); 

            // Scenario 2: Process payment using the New Payment Service via its Adapter
            Console.WriteLine("Client wants to process payment via New Payment Service:");
            NewPaymentService newService = new NewPaymentService();
            IPaymentProcessor newServiceAdapter = new NewPaymentServiceAdapter(newService);
            newServiceAdapter.ProcessPayment(250.75m);

            // You can even put them in a list and process generically
            Console.WriteLine("Processing multiple payments generically through adapters:");
            var processors = new System.Collections.Generic.List<IPaymentProcessor>
            {
                new OldPaymentGatewayAdapter(new OldPaymentGateway()),
                new NewPaymentServiceAdapter(new NewPaymentService())
            };

            foreach (var processor in processors)
            {
                processor.ProcessPayment(50.00m);
            }

            Console.WriteLine("\n--- Adapter Pattern Example Finished ---");
            Console.ReadKey(); 
        }
    }
}
