using EmployeeManagementSystem.DTOs.Auth;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    /// <summary>
    /// Public authentication controller for registering and logging in users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user (Admin-only if protecting creation of Admins).
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous] // Optional: restrict to Admins if needed
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (result == "Email already exists")
                return Conflict(new { message = result });

            return Ok(new { message = result });
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginAsync(model);

            if (token == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new { token });
        }

        /// <summary>
        /// Example secured route to check token validity (Admin role required).
        /// </summary>
        [HttpGet("me")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult GetCurrentUser()
        {
            var name = User.Identity?.Name ?? "Unknown";
            var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Unknown";
            var email = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? "Unknown";

            return Ok(new
            {
                message = "Authenticated user",
                name,
                email,
                role
            });
        }

    }
}
