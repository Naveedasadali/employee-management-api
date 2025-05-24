using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

      
        public required string Action { get; set; }
        public required User User { get; set; }


        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Navigation
   
    }
}
