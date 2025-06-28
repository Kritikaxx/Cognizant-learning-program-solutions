using System;
using System.Data;
using Microsoft.Data.SqlClient; 

class Program
{
    private static string _connectionString = "Data Source=RICHA;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;";

    static void Main(string[] args)
    {
        Console.WriteLine("--- Exercise 5: Returning Scalar Data from a Stored Procedure ---");
        string[] createSpCountSqlCommands = new string[]
        {
            @"
            IF OBJECT_ID('sp_CountEmployeesInDepartment', 'P') IS NOT NULL
                DROP PROCEDURE sp_CountEmployeesInDepartment;
            ",
            @"
            CREATE PROCEDURE sp_CountEmployeesInDepartment
                @DepartmentID INT
            AS
            BEGIN
                SELECT
                    COUNT(EmployeeID) AS TotalEmployees
                FROM
                    Employees
                WHERE
                    DepartmentID = @DepartmentID;
            END;
            "
        };

        ExecuteDdlCommand("Create sp_CountEmployeesInDepartment", createSpCountSqlCommands);

        Console.WriteLine("\n--- Executing sp_CountEmployeesInDepartment ---");
        ExecuteStoredProcedureScalar("sp_CountEmployeesInDepartment", 1); // For HR
        ExecuteStoredProcedureScalar("sp_CountEmployeesInDepartment", 3); // For IT
        ExecuteStoredProcedureScalar("sp_CountEmployeesInDepartment", 99); // For a non-existent department

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
    /// <param name="spName">The name of the stored procedure to execute.</param>
    /// <param name="departmentId">The DepartmentID parameter for the stored procedure.</param>
    static void ExecuteStoredProcedureScalar(string spName, int departmentId)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine($"Connection opened for {spName}.");
                using (SqlCommand command = new SqlCommand(spName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DepartmentID", departmentId);

                    object result = command.ExecuteScalar(); 

                    if (result != null && result != DBNull.Value)
                    {
                        int totalEmployees = Convert.ToInt32(result); // Convert to int
                        Console.WriteLine($"Total Employees in Department {departmentId}: {totalEmployees}");
                    }
                    else
                    {
                        Console.WriteLine($"No result returned for Department {departmentId}.");
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