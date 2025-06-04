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
    [Authorize(Roles = "Admin,Manager")]

    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAll()
        {
            var depts = await _context.Departments
                .Select(d => new DepartmentDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description
                }).ToListAsync();

            return Ok(depts);
        }
        // Add this to your DepartmentController.cs
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> Get(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            return Ok(dept);
        }


        [HttpPost]
        public async Task<ActionResult> Create(DepartmentDTO dto)
        {
            var dept = new Department { Name = dto.Name, Description = dto.Description };
            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Department created." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, DepartmentDTO dto)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            dept.Name = dto.Name;
            dept.Description = dto.Description;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Department updated." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Department deleted." });
        }
    }
}
