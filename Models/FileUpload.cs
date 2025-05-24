using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class FileUpload
    {
        public int Id { get; set; }

        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int? EmployeeId { get; set; }
        public int? UserId { get; set; }

        // Navigation
        public required string FileName { get; set; }
        public required string FilePath { get; set; }

        public Employee? Employee { get; set; }
        public User? User { get; set; }

    }
}
