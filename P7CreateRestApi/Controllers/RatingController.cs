using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Dtos;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly RatingRepository _repo;
        private readonly ILogger<RatingController> _logger;

        public RatingController(RatingRepository repo, ILogger<RatingController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/Rating called by {User}",
                User.Identity?.Name ?? "anonymous");

            var items = await _repo.FindAll();

            _logger.LogInformation("GET /api/Rating returned {Count} items", items.Count);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/Rating/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var item = await _repo.FindById(id);
            if (item == null)
            {
                _logger.LogWarning("Rating not found: Id={Id}", id);
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRatingDto dto)
        {
            _logger.LogInformation("POST /api/Rating called by {User}",
                User.Identity?.Name ?? "anonymous");

            var entity = new Rating
            {
                MoodysRating = dto.MoodysRating,
                SandPRating = dto.SandPRating,
                FitchRating = dto.FitchRating,
                OrderNumber = dto.OrderNumber
            };

            await _repo.Add(entity);

            _logger.LogInformation("Rating created: Id={Id}", entity.Id);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRatingDto dto)
        {
            _logger.LogInformation("PUT /api/Rating/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            // Si ton repository a Update(id, Rating input) (comme CurvePoint), on l’utilise.
            var ok = await _repo.Update(id, new Rating
            {
                MoodysRating = dto.MoodysRating,
                SandPRating = dto.SandPRating,
                FitchRating = dto.FitchRating,
                OrderNumber = dto.OrderNumber
            });

            if (!ok)
            {
                _logger.LogWarning("Rating not found for update: Id={Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Rating updated: Id={Id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/Rating/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var existing = await _repo.FindById(id);
            if (existing == null)
            {
                _logger.LogWarning("Rating not found for delete: Id={Id}", id);
                return NotFound();
            }

            await _repo.Delete(existing);

            _logger.LogInformation("Rating deleted: Id={Id}", id);
            return NoContent();
        }
    }
}
