using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Leave
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

   

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public required string Reason { get; set; }
        public required Employee Employee { get; set; }

    }
}
