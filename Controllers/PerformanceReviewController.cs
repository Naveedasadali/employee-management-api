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
    public class PerformanceReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PerformanceReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerformanceReviewDTO>>> GetAll()
        {
            var reviews = await _context.PerformanceReviews
                .Include(r => r.Employee)
                .Select(r => new PerformanceReviewDTO
                {
                    Id = r.Id,
                    EmployeeId = r.EmployeeId,
                    Reviewer = r.Reviewer,
                    ReviewDate = r.ReviewDate,
                    Comments = r.Comments,
                    Rating = r.Rating,
                    Score = r.Score // ✅ Include Score in GET
                }).ToListAsync();

            return Ok(reviews);
        }

        [HttpPost]
        public async Task<ActionResult> Create(PerformanceReviewDTO dto)
        {
            var employee = await _context.Employees.FindAsync(dto.EmployeeId);
            if (employee == null)
                return BadRequest(new { message = "Invalid EmployeeId" });

            var review = new PerformanceReview
            {
                EmployeeId = dto.EmployeeId,
                Employee = employee,
                Reviewer = dto.Reviewer,
                ReviewDate = dto.ReviewDate,
                Comments = dto.Comments,
                Rating = dto.Rating,
                Score = dto.Score // ✅ Save Score in DB
            };

            _context.PerformanceReviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Performance review added." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, PerformanceReviewDTO dto)
        {
            var review = await _context.PerformanceReviews
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null) return NotFound();

            review.Reviewer = dto.Reviewer;
            review.ReviewDate = dto.ReviewDate;
            review.Comments = dto.Comments;
            review.Rating = dto.Rating;
            review.Score = dto.Score; // ✅ Update Score too

            await _context.SaveChangesAsync();
            return Ok(new { message = "Performance review updated." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var review = await _context.PerformanceReviews.FindAsync(id);
            if (review == null) return NotFound();

            _context.PerformanceReviews.Remove(review);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Performance review deleted." });
        }
    }
}
