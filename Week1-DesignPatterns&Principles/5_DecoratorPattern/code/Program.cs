using System;

namespace DecoratorPatternExample
{
    public interface INotifier
    {
        void Send(string message);
    }

    public class EmailNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine($"Sending Email Notification: {message}");
        }
    }

    public abstract class NotifierDecorator : INotifier
    {
        protected INotifier _wrappedNotifier; 
        public NotifierDecorator(INotifier notifier)
        {
            _wrappedNotifier = notifier;
        }

        public virtual void Send(string message)
        {
            _wrappedNotifier.Send(message);
        }
    }

    public class SMSNotifierDecorator : NotifierDecorator
    {
        public SMSNotifierDecorator(INotifier notifier) : base(notifier) { }

        public override void Send(string message)
        {
            base.Send(message); 
            Console.WriteLine($"Sending SMS Notification: {message}"); 
        }
    }

    public class SlackNotifierDecorator : NotifierDecorator
    {
        public SlackNotifierDecorator(INotifier notifier) : base(notifier) { }

        public override void Send(string message)
        {
            base.Send(message); 
            Console.WriteLine($"Sending Slack Notification: {message}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Decorator Pattern Example: Notification System ---\n");

            // Scenario 1: Basic Email Notification
            Console.WriteLine("Scenario 1: Basic Email Notification");
            INotifier emailNotifier = new EmailNotifier();
            emailNotifier.Send("Your order has been placed!");
            Console.WriteLine("\n----------------------------------\n");

            // Scenario 2: Email + SMS Notification
            Console.WriteLine("Scenario 2: Email + SMS Notification");
            // Start with EmailNotifier, then decorate it with SMS
            INotifier emailAndSmsNotifier = new SMSNotifierDecorator(new EmailNotifier());
            emailAndSmsNotifier.Send("Your package has shipped!");
            Console.WriteLine("\n----------------------------------\n");

            // Scenario 3: Email + SMS + Slack Notification
            Console.WriteLine("Scenario 3: Email + SMS + Slack Notification");
            // Start with EmailNotifier, then decorate with SMS, then decorate with Slack
            INotifier allChannelNotifier = new SlackNotifierDecorator(
                                                new SMSNotifierDecorator(
                                                    new EmailNotifier()
                                                )
                                            );
            allChannelNotifier.Send("New critical alert!");
            Console.WriteLine("\n----------------------------------\n");


            // Scenario 4: SMS-only (assuming SMS is a core component now, or you want to start with it)
            Console.WriteLine("Scenario 4: Starting with SMS functionality, then adding Email (conceptual reverse)");
            INotifier smsBaseNotifier = new SMSNotifierWrapper();
            smsBaseNotifier.Send("Reminder: Meeting at 10 AM.");
            Console.WriteLine("\n----------------------------------\n");

            Console.WriteLine("--- Decorator Pattern Example Finished ---");
            Console.ReadKey(); 
        }

        public class SMSWrapper : INotifier
        {
            public void Send(string message)
            {
                Console.WriteLine($"Sending SMS (base) Notification: {message}");
            }
        }
        public class SMSNotifierWrapper : INotifier
        {
            public void Send(string message)
            {
                Console.WriteLine($"Sending SMS (base) Notification: {message}");
            }
        }
    }
}
