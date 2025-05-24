using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Role.Admin},{Role.Manager}")]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAll()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Select(e => new EmployeeDTO
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Email = e.Email,
                    Salary = e.Salary,
                    JobTitle = e.JobTitle,
                    DepartmentId = e.DepartmentId
                }).ToListAsync();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> Get(int id)
        {
            var e = await _context.Employees.FindAsync(id);
            if (e == null) return NotFound();

            return new EmployeeDTO
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                Salary = e.Salary,
                JobTitle = e.JobTitle,
                DepartmentId = e.DepartmentId
            };
        }
        [HttpPost]
        public async Task<ActionResult> Create(EmployeeDTO dto)
        {
            var department = await _context.Departments.FindAsync(dto.DepartmentId);
            if (department == null)
                return BadRequest(new { message = "Invalid DepartmentId" });

            var e = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Salary = dto.Salary,
                JobTitle = dto.JobTitle,
                DepartmentId = dto.DepartmentId,
                Department = department // Set required navigation property
            };

            _context.Employees.Add(e);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Employee created successfully." });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, EmployeeDTO dto)
        {
            var e = await _context.Employees.FindAsync(id);
            if (e == null) return NotFound();

            e.FullName = dto.FullName;
            e.Email = dto.Email;
            e.Salary = dto.Salary;
            e.JobTitle = dto.JobTitle;
            e.DepartmentId = dto.DepartmentId;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Employee updated." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var e = await _context.Employees.FindAsync(id);
            if (e == null) return NotFound();

            _context.Employees.Remove(e);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Employee deleted." });
        }
    }
}
