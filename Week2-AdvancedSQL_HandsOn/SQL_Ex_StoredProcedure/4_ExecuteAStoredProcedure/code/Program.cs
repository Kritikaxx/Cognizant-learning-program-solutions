using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

public class Employee
{
    public int EmployeeID { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int DepartmentID { get; set; }
    public decimal Salary { get; set; }
    public DateTime JoinDate { get; set; }
}

class Program
{
    private static string _connectionString = "Data Source=RICHA;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;";

    static void Main(string[] args)
    {
        Console.WriteLine("--- Exercise 4: Executing a Stored Procedure ---");
        ExecuteStoredProcedureWithResults("sp_GetEmployeesByDepartment", 1);
        ExecuteStoredProcedureWithResults("sp_GetEmployeesByDepartment", 3);

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }
    /// <param name="spName">The name of the stored procedure to execute.</param>
    /// <param name="departmentId">The DepartmentID parameter for the stored procedure.</param>
    static void ExecuteStoredProcedureWithResults(string spName, int departmentId)
    {
        Console.WriteLine($"\n--- Executing Stored Procedure: {spName} for DepartmentID: {departmentId} ---");
        List<Employee> employees = new List<Employee>();

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection opened successfully.");

                using (SqlCommand command = new SqlCommand(spName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@DepartmentID", departmentId); 

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No employees found for this department.");
                            return;
                        }

                        // Print header
                        Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,-15} {4,-12} {5,-15}",
                            "Emp ID", "First Name", "Last Name", "Dept ID", "Salary", "Join Date");
                        Console.WriteLine("----------------------------------------------------------------------------------");

                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                DepartmentID = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
                                JoinDate = reader.GetDateTime(reader.GetOrdinal("JoinDate"))
                            });

                            // Print directly from reader
                            Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,-15} {4,-12:C} {5,-15:yyyy-MM-dd}",
                                reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                reader.GetString(reader.GetOrdinal("FirstName")),
                                reader.GetString(reader.GetOrdinal("LastName")),
                                reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                reader.GetDecimal(reader.GetOrdinal("Salary")),
                                reader.GetDateTime(reader.GetOrdinal("JoinDate"))
                            );
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Error during {spName}: {ex.Message}");
            Console.WriteLine($"Error Code: {ex.Number}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during {spName}: {ex.Message}");
        }
    }
}