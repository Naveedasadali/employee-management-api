using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class FileUploadDTO
    {
        [Required]
        [FromForm(Name = "File")] // 👈 matches Angular: formData.append('File', ...)
        public required IFormFile File { get; set; }

        [FromForm(Name = "UserId")] // 👈 matches Angular: formData.append('UserId', ...)
        public int? UserId { get; set; }

        [FromForm(Name = "EmployeeId")] // 👈 matches Angular: formData.append('EmployeeId', ...)
        public int? EmployeeId { get; set; }
    }
}
