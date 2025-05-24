using EmployeeManagementSystem.DTOs.Auth;

namespace EmployeeManagementSystem.Interfaces
{
    /// <summary>
    /// Defines the contract for authentication operations like login and registration.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">The registration data.</param>
        /// <returns>Success message or error.</returns>
        Task<string> RegisterAsync(RegisterRequestDTO model);


        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <returns>JWT token if successful; null otherwise.</returns>
        Task<string?> LoginAsync(LoginRequestDTO model);

    }
}
