using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurveController : ControllerBase
    {
        private readonly CurvePointRepository _repo;

        public CurveController(CurvePointRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Curve
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.FindAll();
            return Ok(items);
        }

        // GET: api/Curve/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repo.FindById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/Curve
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CurvePoint curvePoint)
        {
            if (curvePoint == null) return BadRequest();

            // ignore l'Id du body (DB le génère)
            curvePoint.Id = 0;

            await _repo.Add(curvePoint);

            // renvoie 201 + route vers GET by id
            return CreatedAtAction(nameof(GetById), new { id = curvePoint.Id }, curvePoint);
        }

        // PUT: api/Curve/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CurvePoint curvePoint)
        {
            if (curvePoint == null) return BadRequest();

            // Si Id est fourni dans le body, il doit matcher l'URL
            if (curvePoint.Id != 0 && curvePoint.Id != id)
                return BadRequest("Id mismatch");

            // ✅ appel correct : 2 paramètres (id + input)
            var ok = await _repo.Update(id, curvePoint);
            if (!ok) return NotFound();

            return NoContent(); // 204
        }

        // DELETE: api/Curve/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.FindById(id);
            if (existing == null) return NotFound();

            await _repo.Delete(existing);
            return NoContent(); // 204
        }
    }
}
