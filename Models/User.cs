using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    /// <summary>
    /// Represents an authenticated user in the system.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }

        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
        public ICollection<FileUpload> FileUploads { get; set; } = new List<FileUpload>();
    }
}