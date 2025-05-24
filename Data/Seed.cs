using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagementSystem.Data
{
    /// <summary>
    /// Seeds the database with initial roles and admin user.
    /// </summary>
    public static class Seed
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure DB is created and apply migrations
            await context.Database.MigrateAsync();

            // If Admin already exists, skip seeding
            if (context.Users.Any()) return;

            // Seed default Admin user
            var adminUser = new User
            {
                FullName = "System Administrator",
                Email = "admin@ems.com",
                Role = Role.Admin,
                PasswordHash = HashPassword("Admin@123") // Default password
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Securely hashes the password using SHA256 (simple example, use Identity for production).
        /// </summary>
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
