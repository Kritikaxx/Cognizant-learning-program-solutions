using System;

namespace CommandPatternExample
{
    
    public interface ICommand
    {
        void Execute();
    }

    
    public class Light
    {
        private string _location;

        public Light(string location)
        {
            _location = location;
        }

        public void TurnOn()
        {
            Console.WriteLine($"{_location} Light is ON.");
        }

        public void TurnOff()
        {
            Console.WriteLine($"{_location} Light is OFF.");
        }
    }

    
    public class LightOnCommand : ICommand
    {
        private Light _light; 

        public LightOnCommand(Light light)
        {
            _light = light;
        }

        
        public void Execute()
        {
            _light.TurnOn();
        }
    }

    
    public class LightOffCommand : ICommand
    {
        private Light _light; 

        public LightOffCommand(Light light)
        {
            _light = light;
        }

        
        public void Execute()
        {
            _light.TurnOff();
        }
    }

    
    public class RemoteControl
    {
        private ICommand _command; // Holds a reference to the command object

                public void SetCommand(ICommand command)
        {
            _command = command;
            Console.WriteLine($"Remote control command set to: {command.GetType().Name}");
        }

        
        public void PressButton()
        {
            if (_command != null)
            {
                Console.WriteLine("Remote Control: Button Pressed!");
                _command.Execute(); 
            }
            else
            {
                Console.WriteLine("Remote Control: No command set for the button.");
            }
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Command Pattern Example: Home Automation System ---\n");

            
            Light livingRoomLight = new Light("Living Room");
            Light kitchenLight = new Light("Kitchen");

            
            ICommand livingRoomLightOn = new LightOnCommand(livingRoomLight);
            ICommand livingRoomLightOff = new LightOffCommand(livingRoomLight);
            ICommand kitchenLightOn = new LightOnCommand(kitchenLight);
            ICommand kitchenLightOff = new LightOffCommand(kitchenLight);

            
            RemoteControl remote = new RemoteControl();

            Console.WriteLine("\n--- Scenario 1: Turning Living Room Light On ---");
            remote.SetCommand(livingRoomLightOn); 
            remote.PressButton(); 

            Console.WriteLine("\n--- Scenario 2: Turning Kitchen Light Off ---");
            remote.SetCommand(kitchenLightOff); 
            remote.PressButton(); 

            Console.WriteLine("\n--- Scenario 3: Turning Living Room Light Off ---");
            remote.SetCommand(livingRoomLightOff);
            remote.PressButton();

            Console.WriteLine("\n--- Scenario 4: Turning Kitchen Light On ---");
            remote.SetCommand(kitchenLightOn);
            remote.PressButton();

            Console.WriteLine("\n--- Command Pattern Example Finished ---");
            Console.ReadKey(); 
        }
    }
}
