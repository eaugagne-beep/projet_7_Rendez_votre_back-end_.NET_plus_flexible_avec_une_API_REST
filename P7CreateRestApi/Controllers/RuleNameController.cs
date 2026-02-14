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
    public class RuleNameController : ControllerBase
    {
        private readonly RuleNameRepository _repo;
        private readonly ILogger<RuleNameController> _logger;


        // Injection du repository et du logger via le constructeur
        public RuleNameController(RuleNameRepository repo, ILogger<RuleNameController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // Seul un Admin peut voir la liste de toutes les RuleNames
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/RuleName called by {User}",
                User.Identity?.Name ?? "anonymous");

            var items = await _repo.FindAll();

            _logger.LogInformation("GET /api/RuleName returned {Count} items", items.Count);
            return Ok(items);
        }

        // Seul un Admin peut voir les détails d'une RuleName
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/RuleName/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var item = await _repo.FindById(id);
            if (item == null)
            {
                _logger.LogWarning("RuleName not found: Id={Id}", id);
                return NotFound();
            }

            return Ok(item);
        }

        // Seul un Admin peut créer une RuleName
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRuleNameDto dto)
        {
            _logger.LogInformation("POST /api/RuleName called by {User} | Name={Name}",
                User.Identity?.Name ?? "anonymous", dto.Name);

            var entity = new RuleName
            {
                Name = dto.Name,
                Description = dto.Description,
                Json = dto.Json,
                Template = dto.Template,
                SqlStr = dto.SqlStr,
                SqlPart = dto.SqlPart
            };

            await _repo.Add(entity);

            _logger.LogInformation("RuleName created: Id={Id}", entity.Id);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        // Seul un Admin peut mettre à jour une RuleName
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRuleNameDto dto)
        {
            _logger.LogInformation("PUT /api/RuleName/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var ok = await _repo.Update(id, new RuleName
            {
                Name = dto.Name,
                Description = dto.Description,
                Json = dto.Json,
                Template = dto.Template,
                SqlStr = dto.SqlStr,
                SqlPart = dto.SqlPart
            });

            if (!ok)
            {
                _logger.LogWarning("RuleName not found for update: Id={Id}", id);
                return NotFound();
            }

            _logger.LogInformation("RuleName updated: Id={Id}", id);
            return NoContent();
        }

        // Seul un Admin peut supprimer une RuleName
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/RuleName/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var existing = await _repo.FindById(id);
            if (existing == null)
            {
                _logger.LogWarning("RuleName not found for delete: Id={Id}", id);
                return NotFound();
            }

            await _repo.Delete(existing);

            _logger.LogInformation("RuleName deleted: Id={Id}", id);
            return NoContent();
        }
    }
}
