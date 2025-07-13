using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeWebApi.Models;
using EmployeeWebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CustomAuthFilter]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public class EmployeeController : ControllerBase
    {
        private readonly List<Employee> _employees;
        
        public EmployeeController()
        {
            _employees = GetStandardEmployeeList();
        }
        
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<Employee>), 200)]
        [ProducesResponseType(500)]
        public ActionResult<List<Employee>> GetStandard()
        {
            // Uncomment the next line to test exception handling
            // throw new Exception("Test exception for demonstration");
            
            return Ok(_employees);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Employee> GetById(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            return Ok(employee);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Employee), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<Employee> Create([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee data is required");
            }
            
            employee.Id = _employees.Max(e => e.Id) + 1;
            _employees.Add(employee);
            
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Employee> Update(int id, [FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee data is required");
            }
            
            var existingEmployee = _employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            
            existingEmployee.Name = employee.Name;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.Permanent = employee.Permanent;
            existingEmployee.Department = employee.Department;
            existingEmployee.Skills = employee.Skills;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            
            return Ok(existingEmployee);
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult Delete(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            
            _employees.Remove(employee);
            return NoContent();
        }
        
        private List<Employee> GetStandardEmployeeList()
        {
            return new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "John Doe",
                    Salary = 50000,
                    Permanent = true,
                    DateOfBirth = new DateTime(1985, 6, 15),
                    Department = new Department { Id = 1, Name = "IT" },
                    Skills = new List<Skill>
                    {
                        new Skill { Id = 1, Name = "C#", Level = "Advanced" },
                        new Skill { Id = 2, Name = "ASP.NET", Level = "Intermediate" }
                    }
                },
                new Employee
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Salary = 60000,
                    Permanent = false,
                    DateOfBirth = new DateTime(1990, 3, 22),
                    Department = new Department { Id = 2, Name = "HR" },
                    Skills = new List<Skill>
                    {
                        new Skill { Id = 3, Name = "Communication", Level = "Expert" },
                        new Skill { Id = 4, Name = "Leadership", Level = "Advanced" }
                    }
                },
                new Employee
                {
                    Id = 3,
                    Name = "Bob Johnson",
                    Salary = 55000,
                    Permanent = true,
                    DateOfBirth = new DateTime(1988, 12, 10),
                    Department = new Department { Id = 1, Name = "IT" },
                    Skills = new List<Skill>
                    {
                        new Skill { Id = 5, Name = "JavaScript", Level = "Advanced" },
                        new Skill { Id = 6, Name = "React", Level = "Intermediate" }
                    }
                }
            };
        }
    }
}