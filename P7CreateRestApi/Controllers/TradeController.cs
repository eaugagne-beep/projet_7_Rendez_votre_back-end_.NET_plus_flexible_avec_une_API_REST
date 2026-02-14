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
    public class TradeController : ControllerBase
    {
        private readonly TradeRepository _repo;
        private readonly ILogger<TradeController> _logger;

        public TradeController(TradeRepository repo, ILogger<TradeController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // Seul un utilisateur authentifié peut voir la liste des Trades
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET /api/Trade called by {User}",
                User.Identity?.Name ?? "anonymous");

            var items = await _repo.FindAll();

            _logger.LogInformation("GET /api/Trade returned {Count} items", items.Count);
            return Ok(items);
        }
       
        // Seul un utilisateur authentifié peut voir les détails d'une Trade
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("GET /api/Trade/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var item = await _repo.FindById(id);
            if (item == null)
            {
                _logger.LogWarning("Trade not found: Id={Id}", id);
                return NotFound();
            }

            return Ok(item);
        }

        // Seul un utilisateur authentifié peut créer une Trade
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTradeDto dto)
        {
            _logger.LogInformation("POST /api/Trade by {User}", User.Identity?.Name ?? "anonymous");

            var username = User.Identity?.Name ?? "system";
            var now = DateTime.UtcNow;

            var entity = new Trade
            {
                Account = dto.Account,
                AccountType = dto.AccountType,
                BuyQuantity = dto.BuyQuantity,
                SellQuantity = dto.SellQuantity,
                BuyPrice = dto.BuyPrice,
                SellPrice = dto.SellPrice,
                TradeDate = dto.TradeDate,
                TradeSecurity = dto.TradeSecurity,
                TradeStatus = dto.TradeStatus,
                Trader = dto.Trader,
                Benchmark = dto.Benchmark,
                Book = dto.Book,
                DealName = dto.DealName,
                DealType = dto.DealType,
                SourceListId = dto.SourceListId,
                Side = dto.Side,

                // serveur
                CreationName = username,
                CreationDate = now,
                RevisionName = username,
                RevisionDate = now
            };

            await _repo.Add(entity);

            return CreatedAtAction(nameof(GetById), new { id = entity.TradeId }, entity);
        }

        // Seul un utilisateur authentifié peut mettre à jour une Trade
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTradeDto dto)
        {
            _logger.LogInformation("PUT /api/Trade/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var ok = await _repo.Update(id, new Trade
            {
                Account = dto.Account,
                AccountType = dto.AccountType,
                BuyQuantity = dto.BuyQuantity,
                SellQuantity = dto.SellQuantity,
                BuyPrice = dto.BuyPrice,
                SellPrice = dto.SellPrice,
                TradeDate = dto.TradeDate,
                TradeSecurity = dto.TradeSecurity,
                TradeStatus = dto.TradeStatus,
                Trader = dto.Trader,
                Benchmark = dto.Benchmark,
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
                _logger.LogWarning("Trade not found for update: Id={Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Trade updated: Id={Id}", id);
            return NoContent();
        }

        // Seul un utilisateur authentifié peut supprimer une Trade
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE /api/Trade/{Id} called by {User}",
                id, User.Identity?.Name ?? "anonymous");

            var existing = await _repo.FindById(id);
            if (existing == null)
            {
                _logger.LogWarning("Trade not found for delete: Id={Id}", id);
                return NotFound();
            }

            await _repo.Delete(existing);

            _logger.LogInformation("Trade deleted: Id={Id}", id);
            return NoContent();
        }
    }
}
