using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtWebApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // This requires authentication for all actions
    public class EmployeeController : ControllerBase
    {
        private static readonly List<Employee> Employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "John Doe", Position = "Developer", Department = "IT" },
            new Employee { Id = 2, Name = "Jane Smith", Position = "Manager", Department = "HR" },
            new Employee { Id = 3, Name = "Bob Johnson", Position = "Analyst", Department = "Finance" }
        };

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            return Ok(new 
            { 
                Message = "Employees retrieved successfully",
                RequestedBy = $"User ID: {userId}, Role: {userRole}",
                Data = Employees 
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            var employee = Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound(new { Message = "Employee not found" });
            }

            var userId = User.FindFirst("UserId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new 
            { 
                Message = "Employee retrieved successfully",
                RequestedBy = $"User ID: {userId}, Role: {userRole}",
                Data = employee 
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admin can create employees
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            employee.Id = Employees.Max(e => e.Id) + 1;
            Employees.Add(employee);

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can update employees
        public IActionResult UpdateEmployee(int id, [FromBody] Employee employee)
        {
            var existingEmployee = Employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee == null)
            {
                return NotFound(new { Message = "Employee not found" });
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Position = employee.Position;
            existingEmployee.Department = employee.Department;

            return Ok(new { Message = "Employee updated successfully", Data = existingEmployee });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can delete employees
        public IActionResult DeleteEmployee(int id)
        {
            var employee = Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound(new { Message = "Employee not found" });
            }

            Employees.Remove(employee);
            return Ok(new { Message = "Employee deleted successfully" });
        }

        // Example of role-based authorization with multiple roles
        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok(new { Message = "This endpoint is only accessible by Admin users" });
        }

        [HttpGet("admin-or-poc")]
        [Authorize(Roles = "Admin,POC")] // Either Admin or POC can access
        public IActionResult AdminOrPocEndpoint()
        {
            return Ok(new { Message = "This endpoint is accessible by Admin or POC users" });
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }
}