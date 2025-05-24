using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs
{
    public class FileUploadDTO
    {
        public required IFormFile File { get; set; }


        public int? EmployeeId { get; set; }
        public int? UserId { get; set; }
    }
}
