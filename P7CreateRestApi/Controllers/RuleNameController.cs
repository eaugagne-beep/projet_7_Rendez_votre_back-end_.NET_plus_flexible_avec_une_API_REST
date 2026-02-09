using Dot.Net.WebApi.Domain;
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

        public RuleNameController(RuleNameRepository repo) => _repo = repo;

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
        public async Task<IActionResult> Create([FromBody] RuleName ruleName)
        {
            if (ruleName == null) return BadRequest();
            ruleName.Id = 0;

            await _repo.Add(ruleName);
            return CreatedAtAction(nameof(GetById), new { id = ruleName.Id }, ruleName);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RuleName ruleName)
        {
            if (ruleName == null) return BadRequest();
            if (ruleName.Id != 0 && ruleName.Id != id) return BadRequest("Id mismatch");

            var ok = await _repo.Update(id, ruleName);
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
