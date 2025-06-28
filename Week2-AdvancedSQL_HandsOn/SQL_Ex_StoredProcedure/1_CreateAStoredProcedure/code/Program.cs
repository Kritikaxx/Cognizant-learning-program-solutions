using System;
using System.Data;
using Microsoft.Data.SqlClient;

class Program
{
    private static string _connectionString = "Data Source=RICHA;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;";

    static void Main(string[] args)
    {
        Console.WriteLine("--- Exercise 1: Creating a Stored Procedure ---");
        string[] createSpSqlCommands = new string[]
        {
            @"
            IF OBJECT_ID('sp_GetEmployeesByDepartment', 'P') IS NOT NULL
                DROP PROCEDURE sp_GetEmployeesByDepartment;
            ",
            @"
            CREATE PROCEDURE sp_GetEmployeesByDepartment
                @DepartmentID INT
            AS
            BEGIN
                SELECT
                    EmployeeID,
                    FirstName,
                    LastName,
                    DepartmentID,
                    Salary,
                    JoinDate
                FROM
                    Employees
                WHERE
                    DepartmentID = @DepartmentID;
            END;
            "
        };

        ExecuteDdlCommand("Create sp_GetEmployeesByDepartment", createSpSqlCommands);

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }

    /// <param name="commandName">A descriptive name for the overall operation being executed.</param>
    /// <param name="sqlCommands">An array of SQL command strings to execute. Each string will be sent as a separate command.</param>
    static void ExecuteDdlCommand(string commandName, string[] sqlCommands)
    {
        Console.WriteLine($"\n--- Executing {commandName} ---");
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection opened successfully.");

                foreach (string sqlCommand in sqlCommands)
                {
                    if (string.IsNullOrWhiteSpace(sqlCommand))
                        continue;

                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        command.CommandTimeout = 30; 

                        command.ExecuteNonQuery();
                        Console.WriteLine($"  - Part of {commandName} executed: '{sqlCommand.Trim().Split('\n')[0].Trim()}'...");
                    }
                }
                Console.WriteLine($"{commandName} completed all parts successfully.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Error during {commandName}: {ex.Message}");
            Console.WriteLine($"Error Code: {ex.Number}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during {commandName}: {ex.Message}");
        }
    }
}