using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Interfaces
{
    /// <summary>
    /// Defines the contract for generating JWT tokens.
    /// </summary>
    public interface IJWTService
    {
        /// <summary>
        /// Generates a JWT token for the given user.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <returns>JWT token as a string.</returns>
        string GenerateToken(User user);
    }
}
