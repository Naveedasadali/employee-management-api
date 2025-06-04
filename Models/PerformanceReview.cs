using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class PerformanceReview
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

      

        [Required]
        public DateTime ReviewDate { get; set; }


        [Required]
        [Range(0, 100)]
        public int Score { get; set; } // ✅ Add this line
        public int Rating { get; set; } // 1 to 5

        public required string Reviewer { get; set; }
        public required string Comments { get; set; }
        public required Employee Employee { get; set; }

    }
}
