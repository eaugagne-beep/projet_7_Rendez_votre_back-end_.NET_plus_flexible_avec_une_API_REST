using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidListController : ControllerBase
    {
        private readonly BidListRepository _repo;

        public BidListController(BidListRepository repo) => _repo = repo;

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
        public async Task<IActionResult> Create([FromBody] BidList bidList)
        {
            if (bidList == null) return BadRequest();
            bidList.BidListId = 0;

            await _repo.Add(bidList);
            return CreatedAtAction(nameof(GetById), new { id = bidList.BidListId }, bidList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BidList bidList)
        {
            if (bidList == null) return BadRequest();
            if (bidList.BidListId != 0 && bidList.BidListId != id) return BadRequest("Id mismatch");

            var ok = await _repo.Update(id, bidList);
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
