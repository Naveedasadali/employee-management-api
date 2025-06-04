using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    /// <summary>
    /// Admin-only controller for managing and retrieving audit logs.
    /// </summary>
    [Authorize(Roles = $"{Role.Admin},{Role.Manager}")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuditLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all audit logs.
        /// </summary>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AuditLogDTO>>> GetAllAuditLogs()
        {
            var logs = await _context.AuditLogs
                .Include(a => a.User)
                .OrderByDescending(a => a.Timestamp)
                .Select(a => new AuditLogDTO
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Action = a.Action,
                    Timestamp = a.Timestamp
                })
                .ToListAsync();

            return Ok(logs);
        }

        /// <summary>
        /// Retrieves audit logs for a specific user.
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AuditLogDTO>>> GetLogsByUserId(int userId)
        {
            var logs = await _context.AuditLogs
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Timestamp)
                .Select(a => new AuditLogDTO
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Action = a.Action,
                    Timestamp = a.Timestamp
                })
                .ToListAsync();

            if (logs is null || logs.Count == 0)
                return NotFound($"No logs found for user ID {userId}");

            return Ok(logs);
        }
            /// <summary>
            /// Retrieves a single audit log entry by its ID.
            /// </summary>
            [HttpGet("{id}")]
        public async Task<ActionResult<AuditLogDTO>> GetLogById(int id)
        {
            var log = await _context.AuditLogs
                .Include(a => a.User)
                .Where(a => a.Id == id)
                .Select(a => new AuditLogDTO
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Action = a.Action,
                    Timestamp = a.Timestamp
                })
                .FirstOrDefaultAsync();

            if (log == null)
                return NotFound($"Audit log with ID {id} not found.");

            return Ok(log);
        }
    }
}
