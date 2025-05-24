using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        public required string FullName { get; set; }
        public required string Email { get; set; }
        public string? JobTitle { get; set; }


        [Required]
        public decimal Salary { get; set; }

      

        [Required]
        public int DepartmentId { get; set; }
    }
}
