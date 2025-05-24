using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs.Auth
{
    /// <summary>
    /// DTO used for logging in a user.
    /// </summary>
    public class LoginRequestDTO
    {
        /// <summary>
        /// The user's email address used for login.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        /// <summary>
        /// The user's password used for login.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public required string Password { get; set; }
    }
}
