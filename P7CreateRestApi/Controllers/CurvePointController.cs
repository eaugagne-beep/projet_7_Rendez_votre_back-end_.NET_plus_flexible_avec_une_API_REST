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
    public class CurveController : ControllerBase
    {
        private readonly CurvePointRepository _repo;
        private readonly ILogger<CurveController> _logger;

        public CurveController(CurvePointRepository repo, ILogger<CurveController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/Curve called by {User}",
                User.Identity?.Name ?? "anonymous");

            return Ok(await _repo.FindAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/Curve/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var item = await _repo.FindById(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCurvePointDto dto)
        {
            _logger.LogInformation("POST /api/Curve called by {User} | CurveId={CurveId}",
                User.Identity?.Name ?? "anonymous", dto.CurveId);

            var entity = new CurvePoint
            {
                CurveId = dto.CurveId,
                AsOfDate = dto.AsOfDate,
                Term = dto.Term,
                CurvePointValue = dto.CurvePointValue,
                CreationDate = DateTime.UtcNow
            };

            await _repo.Add(entity);

            _logger.LogInformation("CurvePoint created: Id={Id}", entity.Id);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCurvePointDto dto)
        {
            _logger.LogInformation("PUT /api/Curve/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var ok = await _repo.Update(id, new CurvePoint
            {
                CurveId = dto.CurveId,
                AsOfDate = dto.AsOfDate,
                Term = dto.Term,
                CurvePointValue = dto.CurvePointValue,
                CreationDate = DateTime.UtcNow
            });

            if (!ok)
            {
                _logger.LogWarning("CurvePoint not found: Id={Id}", id);
                return NotFound();
            }

            _logger.LogInformation("CurvePoint updated: Id={Id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/Curve/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var existing = await _repo.FindById(id);
            if (existing == null)
            {
                _logger.LogWarning("CurvePoint not found for delete: Id={Id}", id);
                return NotFound();
            }

            await _repo.Delete(existing);
            _logger.LogInformation("CurvePoint deleted: Id={Id}", id);
            return NoContent();
        }
    }
}
