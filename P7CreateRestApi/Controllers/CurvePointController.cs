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
    public class CurveController : ControllerBase
    {
        private readonly CurvePointRepository _repo;
        private readonly ILogger<CurveController> _logger;

        public CurveController(CurvePointRepository repo, ILogger<CurveController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        private (string UserName, string? UserId, string Roles) GetCaller()
        {
            var userName = User?.Identity?.Name ?? "anonymous";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roles = string.Join(",", User.FindAll(ClaimTypes.Role).Select(r => r.Value));
            if (string.IsNullOrWhiteSpace(roles)) roles = "none";
            return (userName, userId, roles);
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogInformation("GET /api/Curve by {UserName} ({UserId}) roles={Roles}",
                userName, userId, roles);

            var items = await _repo.FindAll();
            _logger.LogInformation("GET /api/Curve returned {Count} items", items.Count);

            return Ok(items);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repo.FindById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CurvePoint curvePoint)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogInformation("POST /api/Curve by {UserName} ({UserId}) roles={Roles}",
                userName, userId, roles);

            if (curvePoint == null) return BadRequest();
            curvePoint.Id = 0;

            await _repo.Add(curvePoint);

            _logger.LogInformation("CurvePoint created id={Id} by {UserName} ({UserId})",
                curvePoint.Id, userName, userId);

            return CreatedAtAction(nameof(GetById), new { id = curvePoint.Id }, curvePoint);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CurvePoint curvePoint)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogInformation("PUT /api/Curve/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, userName, userId, roles);

            if (curvePoint == null) return BadRequest();

            if (curvePoint.Id != 0 && curvePoint.Id != id)
            {
                _logger.LogWarning("PUT /api/Curve/{Id} Id mismatch body={BodyId} by {UserName} ({UserId})",
                    id, curvePoint.Id, userName, userId);
                return BadRequest("Id mismatch");
            }

            var ok = await _repo.Update(id, curvePoint);
            if (!ok)
            {
                _logger.LogWarning("PUT /api/Curve/{Id} NotFound by {UserName} ({UserId})", id, userName, userId);
                return NotFound();
            }

            _logger.LogInformation("PUT /api/Curve/{Id} Updated by {UserName} ({UserId})", id, userName, userId);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogInformation("DELETE /api/Curve/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, userName, userId, roles);

            var existing = await _repo.FindById(id);
            if (existing == null)
            {
                _logger.LogWarning("DELETE /api/Curve/{Id} NotFound by {UserName} ({UserId})", id, userName, userId);
                return NotFound();
            }

            await _repo.Delete(existing);

            _logger.LogInformation("DELETE /api/Curve/{Id} Deleted by {UserName} ({UserId})", id, userName, userId);
            return NoContent();
        }

    }
}
