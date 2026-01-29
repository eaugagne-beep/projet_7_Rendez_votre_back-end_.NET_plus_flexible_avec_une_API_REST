using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeController : ControllerBase
    {
        private readonly TradeRepository _repo;

        public TradeController(TradeRepository repo) => _repo = repo;

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
        public async Task<IActionResult> Create([FromBody] Trade trade)
        {
            if (trade == null) return BadRequest();
            trade.TradeId = 0;

            await _repo.Add(trade);
            return CreatedAtAction(nameof(GetById), new { id = trade.TradeId }, trade);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Trade trade)
        {
            if (trade == null) return BadRequest();
            if (trade.TradeId != 0 && trade.TradeId != id) return BadRequest("Id mismatch");

            var ok = await _repo.Update(id, trade);
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
