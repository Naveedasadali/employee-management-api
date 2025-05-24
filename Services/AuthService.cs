using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.DTOs.Auth;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{

    public class AuthService(ApplicationDbContext context, IJWTService jwtService) : IAuthService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IJWTService _jwtService = jwtService;

        /// <summary>
        /// Registers a new user if the email is not already taken.
        /// </summary>
        
        public async Task<string> RegisterAsync(RegisterRequestDTO model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                return "Email already exists";

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Role = model.Role,
                PasswordHash = HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Registration successful";
        }

        /// <summary>
        /// Logs in an existing user and returns a JWT token or null if login fails.
        /// </summary>
        public async Task<string?> LoginAsync(LoginRequestDTO model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
                return null;

            return _jwtService.GenerateToken(user);
        }

        // Static method to hash password using SHA256
        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        // Static method to verify password by comparing hashes
        private static bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
