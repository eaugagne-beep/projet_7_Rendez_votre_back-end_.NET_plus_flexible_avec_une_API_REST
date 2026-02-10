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
    public class TradeController : ControllerBase
    {
        private readonly TradeRepository _repo;
        private readonly ILogger<TradeController> _logger;

        public TradeController(TradeRepository repo, ILogger<TradeController> logger)
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
            _logger.LogInformation("GET /api/Trade by {UserName} ({UserId}) roles={Roles}",
                userName, userId, roles);

            var items = await _repo.FindAll();

            _logger.LogInformation("GET /api/Trade -> returned {Count} items", items.Count);
            return Ok(items);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogInformation("GET /api/Trade/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, userName, userId, roles);

            var item = await _repo.FindById(id);
            if (item == null)
            {
                _logger.LogWarning("GET /api/Trade/{Id} -> NotFound by {UserName} ({UserId})",
                    id, userName, userId);
                return NotFound();
            }

            return Ok(item);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Trade trade)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogInformation("POST /api/Trade (Create) by {UserName} ({UserId}) roles={Roles}",
                userName, userId, roles);

            if (trade == null) return BadRequest();

            
            trade.TradeId = 0;

            await _repo.Add(trade);

            _logger.LogInformation("POST /api/Trade -> Created id={Id} by {UserName} ({UserId})",
                trade.TradeId, userName, userId);

            return CreatedAtAction(nameof(GetById), new { id = trade.TradeId }, trade);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Trade trade)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogInformation("PUT /api/Trade/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, userName, userId, roles);

            if (trade == null) return BadRequest();

           
            if (trade.TradeId != 0 && trade.TradeId != id)
            {
                _logger.LogWarning("PUT /api/Trade/{Id} -> BadRequest (Id mismatch body={BodyId}) by {UserName} ({UserId})",
                    id, trade.TradeId, userName, userId);
                return BadRequest("Id mismatch");
            }

            var ok = await _repo.Update(id, trade);
            if (!ok)
            {
                _logger.LogWarning("PUT /api/Trade/{Id} -> NotFound by {UserName} ({UserId})",
                    id, userName, userId);
                return NotFound();
            }

            _logger.LogInformation("PUT /api/Trade/{Id} -> Updated by {UserName} ({UserId})",
                id, userName, userId);

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogWarning("DELETE /api/Trade/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, userName, userId, roles);

            var existing = await _repo.FindById(id);
            if (existing == null)
            {
                _logger.LogWarning("DELETE /api/Trade/{Id} -> NotFound by {UserName} ({UserId})",
                    id, userName, userId);
                return NotFound();
            }

            await _repo.Delete(existing);

            _logger.LogInformation("DELETE /api/Trade/{Id} -> Deleted by {UserName} ({UserId})",
                id, userName, userId);

            return NoContent();
        }
    }
}
