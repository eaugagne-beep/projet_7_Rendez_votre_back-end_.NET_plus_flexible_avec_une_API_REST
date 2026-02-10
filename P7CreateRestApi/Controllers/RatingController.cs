using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var user = User.Identity?.Name;

            _logger.LogInformation(
                "User {User} requested GET /api/Rating",
                user
            );

            return Ok(await _repo.FindAll());
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = User.Identity?.Name;

            _logger.LogInformation(
                "User {User} requested GET /api/Rating/{Id}",
                user,
                id
            );

            var rating = await _repo.FindById(id);
            if (rating == null)
            {
                _logger.LogWarning(
                    "User {User} requested non-existing Rating {Id}",
                    user,
                    id
                );
                return NotFound();
            }

            return Ok(rating);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Rating rating)
        {
            var user = User.Identity?.Name;

            if (rating == null) return BadRequest();

            _logger.LogInformation(
                "User {User} is creating a Rating",
                user
            );

            rating.Id = 0;
            await _repo.Add(rating);

            return CreatedAtAction(nameof(GetById), new { id = rating.Id }, rating);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Rating rating)
        {
            var user = User.Identity?.Name;

            if (rating == null) return BadRequest();
            if (rating.Id != 0 && rating.Id != id) return BadRequest("Id mismatch");

            _logger.LogInformation(
                "User {User} is updating Rating {Id}",
                user,
                id
            );

            var ok = await _repo.Update(id, rating);
            if (!ok) return NotFound();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = User.Identity?.Name;

            _logger.LogWarning(
                "User {User} is deleting Rating {Id}",
                user,
                id
            );

            var existing = await _repo.FindById(id);
            if (existing == null) return NotFound();

            await _repo.Delete(existing);
            return NoContent();
        }
    }
}
