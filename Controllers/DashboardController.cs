
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    [Authorize(Roles = $"{Role.Admin},{Role.Manager}")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = $"{Role.Admin},{Role.Manager}")]
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var totalEmployees = await _context.Employees.CountAsync();
            var activeEmployees = await _context.Employees.CountAsync(e => e.Salary > 0);
            var inactiveEmployees = totalEmployees - activeEmployees;

            var leavesPending = await _context.Leaves.CountAsync(l => l.Status == "Pending");
            var leavesApproved = await _context.Leaves.CountAsync(l => l.Status == "Approved");
            var leavesRejected = await _context.Leaves.CountAsync(l => l.Status == "Rejected");

            var today = DateTime.Today;
            var loginsToday = await _context.AuditLogs.CountAsync(l => l.Action == "Login" && l.Timestamp.Date == today);
            var successfulLogins = await _context.AuditLogs.CountAsync(l => l.Action == "Login" && l.Timestamp.Date == today);
            var failedLogins = 1; // You can enhance this if you log failed attempts

            return Ok(new
            {
                totalEmployees,
                activeEmployees,
                inactiveEmployees,
                leavesPending,
                leavesApproved,
                leavesRejected,
                loginsToday,
                successfulLogins,
                failedLogins
            });
        }
        [Authorize(Roles = $"{Role.Admin},{Role.Manager}")]

        [HttpGet("performance-chart")]
        public async Task<IActionResult> GetPerformanceData()
        {
            var top = await _context.PerformanceReviews
                .Include(p => p.Employee)
                .GroupBy(p => p.Employee.FullName)
                .Select(g => new
                {
                    name = g.Key,
                    avgScore = g.Average(x => x.Score)
                })
                .OrderByDescending(x => x.avgScore)
                .Take(5)
                .ToListAsync();

            return Ok(top);
        }
    }
}
