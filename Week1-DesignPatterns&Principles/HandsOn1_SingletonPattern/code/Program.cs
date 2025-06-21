using System;
using System.Threading.Tasks;

namespace SingletonPatternExample
{
    public sealed class Logger
    {
        private static readonly Lazy<Logger> _lazyInstance = new Lazy<Logger>(() =>
        {
            Console.WriteLine("--- Logger instance created . ---");
            return new Logger();
        });

        public static Logger Instance
        {
            get
            {
                return _lazyInstance.Value;
            }
        }

        private Logger()
        {
            // Any initialization logic for the logger goes here.
        }

        public void Log(string message)
        {
           
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] LOG: {message}");
        }

       
        public void DemonstrateIdentity()
        {
            Console.WriteLine($"Logger instance Hash Code: {this.GetHashCode()}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Testing Singleton Pattern Example ---");

            Console.WriteLine("\n*** Test 1: Direct Access ***");
            Logger logger1 = Logger.Instance;
            logger1.Log("Application started.");
            logger1.DemonstrateIdentity();

            Logger logger2 = Logger.Instance;
            logger2.Log("Performing an operation.");
            logger2.DemonstrateIdentity();

            if (ReferenceEquals(logger1, logger2))
            {
                Console.WriteLine("logger1 and logger2 refer to the SAME instance (as expected).");
            }
            else
            {
                Console.WriteLine("ERROR: logger1 and logger2 refer to DIFFERENT instances.");
            }

            Console.WriteLine("\n*** Test 2: Access from Different Methods ***");
            CallLoggerFromMethod1();
            CallLoggerFromMethod2();

            Console.WriteLine("\n*** Test 3: Access from Multiple Threads ***");
            var tasks = new Task<Logger>[5];
            for (int i = 0; i < tasks.Length; i++)
            {
                int taskId = i + 1; 
                tasks[i] = Task.Run(() =>
                {
                    Logger threadLogger = Logger.Instance;
                    threadLogger.Log($"Message from Task {taskId}");
                    threadLogger.DemonstrateIdentity();
                    return threadLogger;
                });
            }

           
            Task.WaitAll(tasks);

            Logger firstTaskLogger = tasks[0].Result;
            bool allSameInstance = true;
            for (int i = 1; i < tasks.Length; i++)
            {
                if (!ReferenceEquals(firstTaskLogger, tasks[i].Result))
                {
                    allSameInstance = false;
                    break;
                }
            }

            if (allSameInstance)
            {
                Console.WriteLine("All threads received the SAME Logger instance (as expected).");
            }
            else
            {
                Console.WriteLine("ERROR: Different Logger instances were created across threads.");
            }

            Console.WriteLine("\n--- Singleton Pattern Example Finished ---");
            Console.ReadKey(); 
        }

        static void CallLoggerFromMethod1()
        {
            Logger.Instance.Log("Message from Method 1.");
            Logger.Instance.DemonstrateIdentity();
        }

        static void CallLoggerFromMethod2()
        {
            Logger.Instance.Log("Message from Method 2.");
            Logger.Instance.DemonstrateIdentity();
        }
    }
}
