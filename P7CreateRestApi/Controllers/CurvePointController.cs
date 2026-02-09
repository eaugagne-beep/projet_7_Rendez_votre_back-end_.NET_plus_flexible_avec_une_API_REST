using Dot.Net.WebApi.Domain;
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

        public CurveController(CurvePointRepository repo)
        {
            _repo = repo;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.FindAll();
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
            if (curvePoint == null) return BadRequest();

            
            curvePoint.Id = 0;

            await _repo.Add(curvePoint);

            
            return CreatedAtAction(nameof(GetById), new { id = curvePoint.Id }, curvePoint);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CurvePoint curvePoint)
        {
            if (curvePoint == null) return BadRequest();

            
            if (curvePoint.Id != 0 && curvePoint.Id != id)
                return BadRequest("Id mismatch");

            
            var ok = await _repo.Update(id, curvePoint);
            if (!ok) return NotFound();

            return NoContent(); 
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
