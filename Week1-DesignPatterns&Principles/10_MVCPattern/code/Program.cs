using System;

namespace MVCPatternExample
{
    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }

        public Student(string id, string name, string grade)
        {
            Id = id;
            Name = name;
            Grade = grade;
        }
    }

    public class StudentView
    {
        
        public void DisplayStudentDetails(string studentId, string studentName, string studentGrade)
        {
            Console.WriteLine("\n--- Student Details ---");
            Console.WriteLine($"ID: {studentId}");
            Console.WriteLine($"Name: {studentName}");
            Console.WriteLine($"Grade: {studentGrade}");
            Console.WriteLine("-----------------------");
        }
    }

    public class StudentController
    {
        private Student _model; 
        private StudentView _view;  

       
        public StudentController(Student model, StudentView view)
        {
            _model = model;
            _view = view;
        }

        
        public void SetStudentName(string name)
        {
            Console.WriteLine($"Controller: Updating student name from '{_model.Name}' to '{name}'");
            _model.Name = name;
        }

        public void SetStudentGrade(string grade)
        {
            Console.WriteLine($"Controller: Updating student grade from '{_model.Grade}' to '{grade}'");
            _model.Grade = grade;
        }

        public string GetStudentId()
        {
            return _model.Id;
        }

        public string GetStudentName()
        {
            return _model.Name;
        }

        public string GetStudentGrade()
        {
            return _model.Grade;
        }

        public void UpdateView()
        {
            Console.WriteLine("Controller: Instructing View to display updated details.");
            _view.DisplayStudentDetails(_model.Id, _model.Name, _model.Grade);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- MVC Pattern Example: Student Record Management ---\n");

            
            Console.WriteLine("Client: Creating initial Student Model...");
            Student student = new Student("S101", "Alice Smith", "A-");

            
            Console.WriteLine("Client: Creating Student View...");
            StudentView view = new StudentView();

            Console.WriteLine("Client: Creating Student Controller, linking Model and View...");
            StudentController controller = new StudentController(student, view);

            
            Console.WriteLine("\nClient: Displaying initial student details:");
            controller.UpdateView();

            
            Console.WriteLine("\nClient: Simulating user interaction to update student name and grade...");
            controller.SetStudentName("Alice Wonderland");
            controller.SetStudentGrade("A+");

            
            Console.WriteLine("\nClient: Displaying updated student details:");
            controller.UpdateView();

            
            Console.WriteLine("\nClient: Simulating user interaction to change only student grade...");
            controller.SetStudentGrade("B");

            Console.WriteLine("\nClient: Displaying final student details:");
            controller.UpdateView();


            Console.WriteLine("\n--- MVC Pattern Example Finished ---");
            Console.ReadKey(); 
        }
    }
}
