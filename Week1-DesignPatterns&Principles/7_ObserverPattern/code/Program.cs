using System;
using System.Collections.Generic; // Required for List<T>

namespace ObserverPatternExample
{
    public interface IObserver
    {
        void Update(string stockName, decimal price);
    }

    
    public interface IStock
    {
        void RegisterObserver(IObserver observer);
        void DeregisterObserver(IObserver observer);
        void NotifyObservers();
    }

    
    public class StockMarket : IStock
    {
        private List<IObserver> _observers = new List<IObserver>();
        private string _stockName;
        private decimal _currentPrice;

        
        public StockMarket(string stockName, decimal initialPrice)
        {
            _stockName = stockName;
            _currentPrice = initialPrice;
            Console.WriteLine($"StockMarket created for '{_stockName}' with initial price: {initialPrice:C}");
        }

        
        public decimal CurrentPrice
        {
            get { return _currentPrice; }
            set
            {
                if (_currentPrice != value) 
                {
                    _currentPrice = value;
                    Console.WriteLine($"\n--- Stock Price for {_stockName} changed to {_currentPrice:C} ---");
                    NotifyObservers(); 
                }
            }
        }

        
        public void RegisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                Console.WriteLine($"+ Observer registered: {observer.GetType().Name}");
            }
        }

        
        public void DeregisterObserver(IObserver observer)
        {
            if (_observers.Remove(observer))
            {
                Console.WriteLine($"- Observer deregistered: {observer.GetType().Name}");
            }
        }

        
        public void NotifyObservers()
        {
            Console.WriteLine($"Notifying {_observers.Count} observers for {_stockName}...");
            foreach (var observer in _observers)
            {
                observer.Update(_stockName, _currentPrice);
            }
        }
    }

    
    public class MobileApp : IObserver
    {
        private string _appName;

        public MobileApp(string appName)
        {
            _appName = appName;
        }

        public void Update(string stockName, decimal price)
        {
            Console.WriteLine($"  [{_appName} Mobile App]: {stockName} updated to {price:C}. Check your alerts!");
        }
    }

    
    public class WebApp : IObserver
    {
        private string _appType;

        public WebApp(string appType)
        {
            _appType = appType;
        }

        public void Update(string stockName, decimal price)
        {
            Console.WriteLine($"  [{_appType} Web App]: {stockName} current price is {price:C}. Refresh your dashboard!");
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Observer Pattern Example: Stock Market Monitoring ---\n");

            
            StockMarket googleStock = new StockMarket("GOOG", 150.00m);

            
            MobileApp myPhoneApp = new MobileApp("MyPhoneStockTracker");
            WebApp dashboardWeb = new WebApp("FinancialDashboard");
            MobileApp partnersPhoneApp = new MobileApp("PartnerProApp");

            
            Console.WriteLine("\n--- Registering Observers ---");
            googleStock.RegisterObserver(myPhoneApp);
            googleStock.RegisterObserver(dashboardWeb);
            googleStock.RegisterObserver(partnersPhoneApp);

            
            Console.WriteLine("\n--- Simulating Stock Price Changes ---");

            googleStock.CurrentPrice = 152.50m; 
            googleStock.CurrentPrice = 151.80m; 
            googleStock.CurrentPrice = 151.80m; 
            googleStock.CurrentPrice = 155.00m; 

            
            Console.WriteLine("\n--- Deregistering an Observer ---");
            googleStock.DeregisterObserver(dashboardWeb);

            
            Console.WriteLine("\n--- Simulating Price Change after Deregistration ---");
            googleStock.CurrentPrice = 154.25m; 

            Console.WriteLine("\n--- Observer Pattern Example Finished ---");
            Console.ReadKey(); 
        }
    }
}
