using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Services
{
    public class AuditLogService(ApplicationDbContext context) : IAuditLogService
    {
        private readonly ApplicationDbContext _context = context;


        /// <summary>
        /// Saves a log describing the user's action.
        /// </summary>
        public async Task LogAsync(int userId, string action)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                Timestamp = DateTime.UtcNow,
                User = user // ✅ Required navigation property set
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
