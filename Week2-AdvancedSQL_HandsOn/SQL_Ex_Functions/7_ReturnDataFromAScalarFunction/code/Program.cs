using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

class Program
{
    private static string _connectionString = "Data Source=RICHA;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;";

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

    /// <param name="employeeId">The ID of the employee to get the annual salary for.</param>
    static void ExecuteScalarFunctionForEmployee(int employeeId)
    {
        Console.WriteLine($"\nRetrieving annual salary for EmployeeID: {employeeId}...");
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection opened successfully.");
                string sqlQuery = $@"
                    SELECT
                        E.FirstName,
                        E.LastName,
                        E.Salary,
                        dbo.fn_CalculateAnnualSalary(E.Salary) AS AnnualSalary
                    FROM
                        Employees AS E
                    WHERE
                        E.EmployeeID = @EmployeeID;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                            decimal monthlySalary = reader.GetDecimal(reader.GetOrdinal("Salary"));
                            decimal annualSalary = reader.GetDecimal(reader.GetOrdinal("AnnualSalary"));

                            Console.WriteLine($"Employee: {firstName} {lastName} (ID: {employeeId})");
                            Console.WriteLine($"Monthly Salary: {monthlySalary:C}");
                            Console.WriteLine($"Annual Salary: {annualSalary:C}");
                        }
                        else
                        {
                            Console.WriteLine($"No employee found with EmployeeID: {employeeId}");
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Error while executing function for EmployeeID {employeeId}: {ex.Message}");
            Console.WriteLine($"Error Code: {ex.Number}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred for EmployeeID {employeeId}: {ex.Message}");
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("--- Exercise 7: Return Data from a Scalar Function ---");

        string[] createFunctionSqlCommands = new string[]
        {
            @"
            IF OBJECT_ID('fn_CalculateAnnualSalary', 'FN') IS NOT NULL
                DROP FUNCTION fn_CalculateAnnualSalary;
            ",
            @"
            CREATE FUNCTION fn_CalculateAnnualSalary
            (
                @Salary DECIMAL(10,2)
            )
            RETURNS DECIMAL(10,2)
            AS
            BEGIN
                RETURN @Salary * 12;
            END;
            "
        };
        ExecuteDdlCommand("Create fn_CalculateAnnualSalary", createFunctionSqlCommands);
        Console.WriteLine("\n--- Executing Exercise 7: Return Data from a Scalar Function ---");
        ExecuteScalarFunctionForEmployee(1);

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }
}