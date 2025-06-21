using System;

namespace StrategyPatternExample
{
    
    public interface IPaymentStrategy
    {
        void Pay(decimal amount);
    }

    
    public class CreditCardPayment : IPaymentStrategy
    {
        private string _cardNumber;
        private string _cardHolderName;
        private string _cvv;
        private string _expirationDate;

        public CreditCardPayment(string cardNumber, string cardHolderName, string cvv, string expirationDate)
        {
            _cardNumber = cardNumber;
            _cardHolderName = cardHolderName;
            _cvv = cvv;
            _expirationDate = expirationDate;
        }

        
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Processing Credit Card payment of {amount:C}...");
            Console.WriteLine($"Card Number: {_cardNumber}");
            Console.WriteLine($"Card Holder: {_cardHolderName}");
            
            Console.WriteLine("Credit Card payment processed successfully!");
        }
    }

    
    public class PayPalPayment : IPaymentStrategy
    {
        private string _email;
        private string _password; 

        public PayPalPayment(string email, string password)
        {
            _email = email;
            _password = password;
        }

        
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount:C}...");
            Console.WriteLine($"PayPal Email: {_email}");
            
            Console.WriteLine("PayPal payment processed successfully!");
        }
    }

    
    public class PaymentContext
    {
        private IPaymentStrategy _paymentStrategy;

        
        public void SetPaymentStrategy(IPaymentStrategy strategy)
        {
            _paymentStrategy = strategy;
            Console.WriteLine($"\nPayment method set to: {_paymentStrategy.GetType().Name}");
        }

        
        public void ExecutePayment(decimal amount)
        {
            if (_paymentStrategy == null)
            {
                Console.WriteLine("Error: No payment strategy has been set.");
                return;
            }
            Console.WriteLine($"Attempting to pay {amount:C}...");
            _paymentStrategy.Pay(amount); // Delegates the call to the selected strategy
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Strategy Pattern Example: Payment System ---\n");

           
            PaymentContext paymentProcessor = new PaymentContext();

            
            Console.WriteLine("Client wants to pay using Credit Card:");
            IPaymentStrategy creditCard = new CreditCardPayment("1234-5678-9012-3456", "John Doe", "123", "12/25");
            paymentProcessor.SetPaymentStrategy(creditCard);
            paymentProcessor.ExecutePayment(50.00m);

            Console.WriteLine("\n----------------------------------\n");

            
            Console.WriteLine("Client wants to pay using PayPal:");
            IPaymentStrategy payPal = new PayPalPayment("john.doe@example.com", "mysecurepassword"); // In real app, don't pass raw passwords
            paymentProcessor.SetPaymentStrategy(payPal);
            paymentProcessor.ExecutePayment(75.50m);

            Console.WriteLine("\n----------------------------------\n");

            
            Console.WriteLine("Client wants to pay again using Credit Card:");
            
            paymentProcessor.SetPaymentStrategy(creditCard);
            paymentProcessor.ExecutePayment(25.00m);

            Console.WriteLine("\n--- Strategy Pattern Example Finished ---");
            Console.ReadKey(); 
        }
    }
}
