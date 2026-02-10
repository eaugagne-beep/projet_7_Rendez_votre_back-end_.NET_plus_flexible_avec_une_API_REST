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
    public class BidListController : ControllerBase
    {
        private readonly BidListRepository _repo;
        private readonly ILogger<BidListController> _logger;

        public BidListController(BidListRepository repo, ILogger<BidListController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        private (string UserName, string? UserId) CurrentUser()
        {
            var userName = User?.Identity?.Name ?? "anonymous";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return (userName, userId);
        }

  
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var (userName, userId) = CurrentUser();
            _logger.LogInformation("GET BidList -> GetAll by {UserName} ({UserId})", userName, userId);

            var items = await _repo.FindAll();

            _logger.LogInformation("GET BidList -> returned {Count} items", items.Count);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var (userName, userId) = CurrentUser();
            _logger.LogInformation("GET BidList/{Id} by {UserName} ({UserId})", id, userName, userId);

            var item = await _repo.FindById(id);
            if (item == null)
            {
                _logger.LogWarning("GET BidList/{Id} -> NotFound", id);
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BidList bidList)
        {
            var (userName, userId) = CurrentUser();
            _logger.LogInformation("POST BidList (Create) by {UserName} ({UserId})", userName, userId);

            if (bidList == null) return BadRequest();

            
            bidList.BidListId = 0;

            await _repo.Add(bidList);

            _logger.LogInformation("POST BidList -> Created id={Id}", bidList.BidListId);
            return CreatedAtAction(nameof(GetById), new { id = bidList.BidListId }, bidList);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BidList bidList)
        {
            var (userName, userId) = CurrentUser();
            _logger.LogInformation("PUT BidList/{Id} by {UserName} ({UserId})", id, userName, userId);

            if (bidList == null) return BadRequest();

            
            if (bidList.BidListId != 0 && bidList.BidListId != id)
            {
                _logger.LogWarning("PUT BidList/{Id} -> BadRequest (Id mismatch body={BodyId})", id, bidList.BidListId);
                return BadRequest("Id mismatch");
            }

            var ok = await _repo.Update(id, bidList);
            if (!ok)
            {
                _logger.LogWarning("PUT BidList/{Id} -> NotFound", id);
                return NotFound();
            }

            _logger.LogInformation("PUT BidList/{Id} -> Updated", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (userName, userId) = CurrentUser();
            _logger.LogInformation("DELETE BidList/{Id} by {UserName} ({UserId})", id, userName, userId);

            var existing = await _repo.FindById(id);
            if (existing == null)
            {
                _logger.LogWarning("DELETE BidList/{Id} -> NotFound", id);
                return NotFound();
            }

            await _repo.Delete(existing);

            _logger.LogInformation("DELETE BidList/{Id} -> Deleted", id);
            return NoContent();
        }

    }
}
