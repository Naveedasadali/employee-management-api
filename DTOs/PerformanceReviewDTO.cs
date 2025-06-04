using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class PerformanceReviewDTO
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public required string Reviewer { get; set; }
        public required string Comments { get; set; }



        [Required]
        public DateTime ReviewDate { get; set; }

        public int Score { get; set; }  // ✅ Added Score

        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
