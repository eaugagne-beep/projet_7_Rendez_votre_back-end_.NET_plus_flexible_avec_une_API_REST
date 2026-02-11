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
    public class BidListController : ControllerBase
    {
        private readonly BidListRepository _repo;
        private readonly ILogger<BidListController> _logger;

        public BidListController(BidListRepository repo, ILogger<BidListController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/BidList called by {User}",
                User.Identity?.Name ?? "anonymous");

            var items = await _repo.FindAll();

            _logger.LogInformation("GET /api/BidList returned {Count} items", items.Count);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/BidList/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var item = await _repo.FindById(id);
            if (item == null)
            {
                _logger.LogWarning("BidList not found: Id={Id}", id);
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBidListDto dto)
        {
            // [ApiController] => 400 automatique si dto invalide (ModelState)
            _logger.LogInformation("POST /api/BidList called by {User} | Account={Account} BidType={BidType}",
                User.Identity?.Name ?? "anonymous",
                dto.Account,
                dto.BidType);


            var username = User.Identity?.Name ?? "system";

            var now = DateTime.UtcNow;

            var entity = new BidList
            {
                Account = dto.Account,
                BidType = dto.BidType,
                BidQuantity = dto.BidQuantity,
                AskQuantity = dto.AskQuantity,
                Bid = dto.Bid,
                Ask = dto.Ask,
                Benchmark = dto.Benchmark,
                BidListDate = dto.BidListDate,
                Commentary = dto.Commentary,
                BidSecurity = dto.BidSecurity,
                BidStatus = dto.BidStatus,
                Trader = dto.Trader,
                Book = dto.Book,
                DealName = dto.DealName,
                DealType = dto.DealType,
                SourceListId = dto.SourceListId,
                Side = dto.Side,

                //  audit serveur (création)
                CreationName = username,
                CreationDate = now,

                //  audit serveur (révision initiale = création)
                RevisionName = username,
                RevisionDate = now
            };


            await _repo.Add(entity);

            _logger.LogInformation("BidList created: Id={Id}", entity.BidListId);

            return CreatedAtAction(nameof(GetById), new { id = entity.BidListId }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBidListDto dto)
        {
            _logger.LogInformation("PUT /api/BidList/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var ok = await _repo.Update(id, new BidList
            {
                Account = dto.Account,
                BidType = dto.BidType,
                BidQuantity = dto.BidQuantity,
                AskQuantity = dto.AskQuantity,
                Bid = dto.Bid,
                Ask = dto.Ask,
                Benchmark = dto.Benchmark,
                BidListDate = dto.BidListDate,
                Commentary = dto.Commentary,
                BidSecurity = dto.BidSecurity,
                BidStatus = dto.BidStatus,
                Trader = dto.Trader,
                Book = dto.Book,
                DealName = dto.DealName,
                DealType = dto.DealType,
                SourceListId = dto.SourceListId,
                Side = dto.Side,

                // serveur
                RevisionName = User.Identity?.Name ?? "system",
                RevisionDate = DateTime.UtcNow

            });

            if (!ok)
            {
                _logger.LogWarning("BidList not found for update: Id={Id}", id);
                return NotFound();
            }

            _logger.LogInformation("BidList updated: Id={Id}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/BidList/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var existing = await _repo.FindById(id);
            if (existing == null)
            {
                _logger.LogWarning("BidList not found for delete: Id={Id}", id);
                return NotFound();
            }

            await _repo.Delete(existing);

            _logger.LogInformation("BidList deleted: Id={Id}", id);
            return NoContent();
        }
    }
}
