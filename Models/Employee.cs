using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public required string FullName { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [StringLength(100)]
        public string? JobTitle { get; set; } // Nullable if not required

        [Required]
        public int DepartmentId { get; set; }

        // Navigation properties
        public required Department Department { get; set; }

        public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
        public ICollection<PerformanceReview> PerformanceReviews { get; set; } = new List<PerformanceReview>();
        public ICollection<FileUpload> FileUploads { get; set; } = new List<FileUpload>();
    }
}
