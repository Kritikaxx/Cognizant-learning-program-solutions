using System;
using System.Threading; // Required for Thread.Sleep

namespace ProxyPatternExample
{
    
    public interface IImage
    {
        void Display();
    }

    
    public class RealImage : IImage
    {
        private string _fileName;

        
        public RealImage(string fileName)
        {
            _fileName = fileName;
            LoadImageFromRemoteServer(); 
        }

        
        private void LoadImageFromRemoteServer()
        {
            Console.WriteLine($"   RealImage: Loading '{_fileName}' from remote server... (Please wait)");
            Thread.Sleep(2000); 
            Console.WriteLine($"   RealImage: Loaded '{_fileName}' successfully.");
        }

        
        public void Display()
        {
            Console.WriteLine($"   RealImage: Displaying '{_fileName}'.");
        }
    }

    
    public class ProxyImage : IImage
    {
        private string _fileName;
        private RealImage? _realImage; 

        
        public ProxyImage(string fileName)
        {
            _fileName = fileName;
            Console.WriteLine($"ProxyImage: Created proxy for '{_fileName}'. Image not loaded yet.");
        }

        
        public void Display()
        {
            Console.WriteLine($"ProxyImage: Display method called for '{_fileName}'.");

            
            if (_realImage == null)
            {
                Console.WriteLine($"ProxyImage: RealImage for '{_fileName}' is null. Creating it now...");
                _realImage = new RealImage(_fileName); 
            }
            else
            {
                
                Console.WriteLine($"ProxyImage: RealImage for '{_fileName}' already loaded. Displaying cached version.");
            }

            
            _realImage.Display();
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Proxy Pattern Example: Image Viewer with Lazy Loading & Caching ---\n");

            
            Console.WriteLine("Client: Creating image 'photo1.jpg'...");
            IImage image1 = new ProxyImage("photo1.jpg");
            Console.WriteLine("Client: Image proxy created. Real image not loaded yet.\n");

            
            Console.WriteLine("Client: First display call for 'photo1.jpg'...");
            image1.Display();
            Console.WriteLine("Client: First display call for 'photo1.jpg' finished.\n");

            
            Console.WriteLine("Client: Second display call for 'photo1.jpg' (should be faster due to caching)...");
            image1.Display();
            Console.WriteLine("Client: Second display call for 'photo1.jpg' finished.\n");

            Console.WriteLine("---------------------------------------------------\n");

            
            Console.WriteLine("Client: Creating image 'document.png'...");
            IImage image2 = new ProxyImage("document.png");
            Console.WriteLine("Client: Image proxy created. Real image not loaded yet.\n");

            
            Console.WriteLine("Client: Display call for 'document.png'...");
            image2.Display();
            Console.WriteLine("Client: Display call for 'document.png' finished.\n");

            Console.WriteLine("\n--- Proxy Pattern Example Finished ---");
            Console.ReadKey(); 
        }
    }
}
