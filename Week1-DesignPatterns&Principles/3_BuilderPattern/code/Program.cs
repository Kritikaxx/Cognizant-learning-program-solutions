using System;

namespace BuilderPatternExample
{
    public class Computer
    {
        // Core components
        public string CPU { get; private set; }
        public int RAM_GB { get; private set; }
        public string Storage { get; private set; } 

        // Optional components
        public string GPU { get; private set; } = "Integrated"; 
        public string OperatingSystem { get; private set; } = "None"; 
        public string Monitor { get; private set; } = "None"; 
        public bool HasWebcam { get; private set; }
        public bool HasKeyboard { get; private set; }
        public bool HasMouse { get; private set; }

        private Computer(ComputerBuilder builder)
        {
            CPU = builder.cpu;
            RAM_GB = builder.ramGb;
            Storage = builder.storage;
            GPU = builder.gpu;
            OperatingSystem = builder.operatingSystem;
            Monitor = builder.monitor;
            HasWebcam = builder.hasWebcam;
            HasKeyboard = builder.hasKeyboard;
            HasMouse = builder.hasMouse;
        }

        public override string ToString()
        {
            var components = new System.Text.StringBuilder();
            components.AppendLine("--- Computer Configuration ---");
            components.AppendLine($"CPU: {CPU}");
            components.AppendLine($"RAM: {RAM_GB} GB");
            components.AppendLine($"Storage: {Storage}");
            components.AppendLine($"GPU: {GPU}");
            components.AppendLine($"OS: {OperatingSystem}");
            components.AppendLine($"Monitor: {Monitor}");
            components.AppendLine($"Webcam: {(HasWebcam ? "Yes" : "No")}");
            components.AppendLine($"Keyboard: {(HasKeyboard ? "Yes" : "No")}");
            components.AppendLine($"Mouse: {(HasMouse ? "Yes" : "No")}");
            components.AppendLine("----------------------------");
            return components.ToString();
        }

        public class ComputerBuilder
        {
            internal string cpu;
            internal int ramGb;
            internal string storage;
            internal string gpu = "Integrated"; 
            internal string operatingSystem = "None"; 
            internal string monitor = "None";
            internal bool hasWebcam = false;
            internal bool hasKeyboard = false;
            internal bool hasMouse = false;

            public ComputerBuilder(string cpu, int ramGb, string storage)
            {
                this.cpu = cpu;
                this.ramGb = ramGb;
                this.storage = storage;
            }

            public ComputerBuilder WithGpu(string gpu)
            {
                this.gpu = gpu;
                return this;
            }

            public ComputerBuilder WithOperatingSystem(string os)
            {
                this.operatingSystem = os;
                return this;
            }

            public ComputerBuilder WithMonitor(string monitor)
            {
                this.monitor = monitor;
                return this;
            }

            public ComputerBuilder AddWebcam()
            {
                this.hasWebcam = true;
                return this;
            }

            public ComputerBuilder AddKeyboard()
            {
                this.hasKeyboard = true;
                return this;
            }

            public ComputerBuilder AddMouse()
            {
                this.hasMouse = true;
                return this;
            }
            public Computer Build()
            {
                return new Computer(this); 
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Builder Pattern Example: Computer Assembly ---\n");

            // Example 1: Build a Basic Office Computer
            Console.WriteLine("Building a Basic Office Computer:");
            Computer officeComputer = new Computer.ComputerBuilder("Intel Core i5", 8, "256GB SSD")
                                          .WithOperatingSystem("Windows 10 Pro")
                                          .AddKeyboard()
                                          .AddMouse()
                                          .Build();
            Console.WriteLine(officeComputer);

            // Example 2: Build a High-End Gaming Computer
            Console.WriteLine("Building a High-End Gaming Computer:");
            Computer gamingComputer = new Computer.ComputerBuilder("AMD Ryzen 9", 32, "1TB NVMe SSD")
                                          .WithGpu("NVIDIA GeForce RTX 4080")
                                          .WithOperatingSystem("Windows 11 Home")
                                          .WithMonitor("27-inch 144Hz 1440p")
                                          .AddWebcam()
                                          .AddKeyboard()
                                          .AddMouse()
                                          .Build();
            Console.WriteLine(gamingComputer);

            // Example 3: Build a Compact Workstation (Minimal configuration)
            Console.WriteLine("Building a Compact Workstation:");
            Computer workstation = new Computer.ComputerBuilder("Intel Core i7", 16, "512GB SSD")
                                        .WithOperatingSystem("Ubuntu Linux")
                                        .Build(); 
            Console.WriteLine(workstation);

            // Example 4: A Computer with only essential components
            Console.WriteLine("Building a Barebones Computer:");
            Computer barebonesComputer = new Computer.ComputerBuilder("AMD Athlon", 4, "128GB SSD").Build();
            Console.WriteLine(barebonesComputer);

            Console.WriteLine("\n--- Builder Pattern Example Finished ---");
            Console.ReadKey(); 
        }
    }
}
