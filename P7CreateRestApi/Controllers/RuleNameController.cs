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
    public class RuleNameController : ControllerBase
    {
        private readonly RuleNameRepository _repo;
        private readonly ILogger<RuleNameController> _logger;

        public RuleNameController(RuleNameRepository repo, ILogger<RuleNameController> logger)
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
            _logger.LogInformation("GET /api/RuleName by {UserName} ({UserId}) roles={Roles}",
                userName, userId, roles);

            var items = await _repo.FindAll();

            _logger.LogInformation("GET /api/RuleName -> returned {Count} items", items.Count);
            return Ok(items);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogInformation("GET /api/RuleName/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, userName, userId, roles);

            var item = await _repo.FindById(id);
            if (item == null)
            {
                _logger.LogWarning("GET /api/RuleName/{Id} -> NotFound by {UserName} ({UserId})",
                    id, userName, userId);
                return NotFound();
            }

            return Ok(item);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RuleName ruleName)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogInformation("POST /api/RuleName (Create) by {UserName} ({UserId}) roles={Roles}",
                userName, userId, roles);

            if (ruleName == null) return BadRequest();

            
            ruleName.Id = 0;

            await _repo.Add(ruleName);

            _logger.LogInformation("POST /api/RuleName -> Created id={Id} by {UserName} ({UserId})",
                ruleName.Id, userName, userId);

            return CreatedAtAction(nameof(GetById), new { id = ruleName.Id }, ruleName);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RuleName ruleName)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogInformation("PUT /api/RuleName/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, userName, userId, roles);

            if (ruleName == null) return BadRequest();

          
            if (ruleName.Id != 0 && ruleName.Id != id)
            {
                _logger.LogWarning("PUT /api/RuleName/{Id} -> BadRequest (Id mismatch body={BodyId}) by {UserName} ({UserId})",
                    id, ruleName.Id, userName, userId);
                return BadRequest("Id mismatch");
            }

            var ok = await _repo.Update(id, ruleName);
            if (!ok)
            {
                _logger.LogWarning("PUT /api/RuleName/{Id} -> NotFound by {UserName} ({UserId})",
                    id, userName, userId);
                return NotFound();
            }

            _logger.LogInformation("PUT /api/RuleName/{Id} -> Updated by {UserName} ({UserId})",
                id, userName, userId);

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (userName, userId, roles) = GetCaller();
            _logger.LogWarning("DELETE /api/RuleName/{Id} by {UserName} ({UserId}) roles={Roles}",
                id, userName, userId, roles);

            var existing = await _repo.FindById(id);
            if (existing == null)
            {
                _logger.LogWarning("DELETE /api/RuleName/{Id} -> NotFound by {UserName} ({UserId})",
                    id, userName, userId);
                return NotFound();
            }

            await _repo.Delete(existing);

            _logger.LogInformation("DELETE /api/RuleName/{Id} -> Deleted by {UserName} ({UserId})",
                id, userName, userId);

            return NoContent();
        }
    }
}

