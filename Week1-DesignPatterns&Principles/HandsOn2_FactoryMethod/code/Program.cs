using System;

namespace FactoryMethodPatternExample
{
    public interface IDocument
    {
        string GetDocumentType();
        void Open();
        void Save();
        void Print();
    }
    public class WordDocument : IDocument
    {
        public string GetDocumentType() => "Word Document";

        public void Open()
        {
            Console.WriteLine("Opening Word Document...");
        }

        public void Save()
        {
            Console.WriteLine("Saving Word Document...");
        }

        public void Print()
        {
            Console.WriteLine("Printing Word Document...");
        }
    }
    public class PdfDocument : IDocument
    {
        public string GetDocumentType() => "PDF Document";

        public void Open()
        {
            Console.WriteLine("Opening PDF Document...");
        }

        public void Save()
        {
            Console.WriteLine("Saving PDF Document...");
        }

        public void Print()
        {
            Console.WriteLine("Printing PDF Document...");
        }
    }
    public class ExcelDocument : IDocument
    {
        public string GetDocumentType() => "Excel Document";

        public void Open()
        {
            Console.WriteLine("Opening Excel Document...");
        }

        public void Save()
        {
            Console.WriteLine("Saving Excel Document...");
        }

        public void Print()
        {
            Console.WriteLine("Printing Excel Document...");
        }
    }
    public abstract class DocumentFactory
    {
        public abstract IDocument CreateDocument();
        public void PerformDocumentOperations()
        {
            Console.WriteLine($"\n--- Using factory {this.GetType().Name} ---");
            IDocument document = CreateDocument(); // Calls the concrete factory's implementation
            Console.WriteLine($"Created: {document.GetDocumentType()}");
            document.Open();
            document.Save();
            document.Print();
            Console.WriteLine("--- Document operations completed ---\n");
        }
    }
    public class WordDocumentFactory : DocumentFactory
    {
        public override IDocument CreateDocument()
        {
            return new WordDocument();
        }
    }
    public class PdfDocumentFactory : DocumentFactory
    {
        public override IDocument CreateDocument()
        {
            return new PdfDocument();
        }
    }
    public class ExcelDocumentFactory : DocumentFactory
    {
        public override IDocument CreateDocument()
        {
            return new ExcelDocument();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Factory Method Pattern Example: Document Management System ---\n");

            // Create a Word document using its factory
            Console.WriteLine("Creating Word Document...");
            DocumentFactory wordFactory = new WordDocumentFactory();
            wordFactory.PerformDocumentOperations();

            // Create a PDF document using its factory
            Console.WriteLine("Creating PDF Document...");
            DocumentFactory pdfFactory = new PdfDocumentFactory();
            pdfFactory.PerformDocumentOperations();

            // Create an Excel document using its factory
            Console.WriteLine("Creating Excel Document...");
            DocumentFactory excelFactory = new ExcelDocumentFactory();
            excelFactory.PerformDocumentOperations();

            // Demonstrating direct creation using the factory method
            Console.WriteLine("\n--- Direct Creation via Factory Method ---");
            IDocument newWordDoc = new WordDocumentFactory().CreateDocument();
            Console.WriteLine($"Directly created: {newWordDoc.GetDocumentType()}");
            newWordDoc.Open();

            IDocument newPdfDoc = new PdfDocumentFactory().CreateDocument();
            Console.WriteLine($"Directly created: {newPdfDoc.GetDocumentType()}");
            newPdfDoc.Print();

            Console.WriteLine("\n--- Factory Method Pattern Example Finished ---");
            Console.ReadKey();
        }
    }
}
