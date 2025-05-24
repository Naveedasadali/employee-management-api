using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs.Auth
{
    /// <summary>
    /// DTO used for registering a new user into the system.
    /// </summary>
    public class RegisterRequestDTO
    {
        /// <summary>
        /// Full name of the user.
        /// </summary>
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name must be under 100 characters")]
        public required string FullName { get; set; }

        /// <summary>
        /// Email address to be used as login credential.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public required string Email { get; set; }

        /// <summary>
        /// Password for the account.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public required string Password { get; set; }

        /// <summary>
        /// The role of the user: e.g., Admin, Manager, Employee.
        /// </summary>
        [Required(ErrorMessage = "Role is required")]
        public required string Role { get; set; }
    }
}
