using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly RatingRepository _repo;

        public RatingController(RatingRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repo.FindAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repo.FindById(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Rating rating)
        {
            if (rating == null) return BadRequest();
            rating.Id = 0;

            await _repo.Add(rating);
            return CreatedAtAction(nameof(GetById), new { id = rating.Id }, rating);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Rating rating)
        {
            if (rating == null) return BadRequest();
            if (rating.Id != 0 && rating.Id != id) return BadRequest("Id mismatch");

            var ok = await _repo.Update(id, rating);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.FindById(id);
            if (existing == null) return NotFound();

            await _repo.Delete(existing);
            return NoContent();
        }
    }
}
