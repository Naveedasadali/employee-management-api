using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
{
    /// <summary>
    /// ApplicationDbContext connects EF Core with our database and defines table mappings.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define all tables as DbSet<T>
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        /// <summary>
        /// Customize table configurations if needed using Fluent API.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: define relationships or constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            modelBuilder.Entity<Leave>()
                .HasOne(l => l.Employee)
                .WithMany(e => e.Leaves)
                .HasForeignKey(l => l.EmployeeId);

            modelBuilder.Entity<PerformanceReview>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.PerformanceReviews)
                .HasForeignKey(p => p.EmployeeId);

            modelBuilder.Entity<FileUpload>()
                .HasOne(f => f.Employee)
                .WithMany(e => e.FileUploads)
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FileUpload>()
                .HasOne(f => f.User)
                .WithMany(u => u.FileUploads)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId);
        }
    }
}
