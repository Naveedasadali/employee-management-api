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
    [Authorize(Roles = $"{Role.Admin},{Role.Manager},{Role.Employee}")]
    public class LeaveController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaveController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveDTO>>> GetAll()
        {
            var leaves = await _context.Leaves
                .Include(l => l.Employee)
                .Select(l => new LeaveDTO
                {
                    Id = l.Id,
                    EmployeeId = l.EmployeeId,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Reason = l.Reason,
                    Status = l.Status
                }).ToListAsync();

            return Ok(leaves);
        }

        [HttpPost]
        public async Task<ActionResult> Create(LeaveDTO dto)
        {
            var employee = await _context.Employees.FindAsync(dto.EmployeeId);
            if (employee == null)
                return BadRequest(new { message = "Invalid EmployeeId" });

            var leave = new Leave
            {
                EmployeeId = dto.EmployeeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                Status = dto.Status,
                Employee = employee // ✅ Set required navigation property
            };

            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Leave applied." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, LeaveDTO dto)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null) return NotFound();

            leave.StartDate = dto.StartDate;
            leave.EndDate = dto.EndDate;
            leave.Reason = dto.Reason;
            leave.Status = dto.Status;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Leave updated." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null) return NotFound();

            _context.Leaves.Remove(leave);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Leave deleted." });
        }
    }
}
