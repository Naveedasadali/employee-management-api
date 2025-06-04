using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Helpers;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Role.Admin},{Role.Manager},{Role.Employee}")]
    public class FileUploadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FileUploadController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>
        /// Uploads a file for a user or employee.
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDTO dto)
        {

            try
            {
                if (dto.File == null || dto.File.Length == 0)
                    return BadRequest("File is required.");
                var rootPath = _env.WebRootPath ?? _env.ContentRootPath;

                var uploadsPath = Path.Combine(rootPath, "uploads");

                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }

                var fileUpload = new FileUpload
                {
                    FileName = dto.File.FileName,
                    FilePath = $"/uploads/{fileName}",
                    UploadedAt = DateTime.UtcNow,
                    UserId = dto.UserId,
                    EmployeeId = dto.EmployeeId
                };

                _context.FileUploads.Add(fileUpload);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "File uploaded successfully",
                    path = fileUpload.FilePath
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Lists all uploaded files.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public async Task<IActionResult> GetAllFiles()
        {
            var files = await _context.FileUploads
                .Include(f => f.User)
                .Include(f => f.Employee)
                .OrderByDescending(f => f.UploadedAt)
                .ToListAsync();

            return Ok(files.Select(f => new
            {
                f.Id,
                f.FileName,
                f.FilePath,
                f.UploadedAt,
                f.UserId,
                f.EmployeeId
            }));
        }

        /// <summary>
        /// Downloads a file by ID.
        /// </summary>
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var file = await _context.FileUploads.FindAsync(id);
            if (file == null)
                return NotFound("File not found.");

            var physicalPath = Path.Combine(_env.WebRootPath, file.FilePath.TrimStart('/'));

            var mimeType = "application/octet-stream";
            return PhysicalFile(physicalPath, mimeType, file.FileName);
        }
    }
}
