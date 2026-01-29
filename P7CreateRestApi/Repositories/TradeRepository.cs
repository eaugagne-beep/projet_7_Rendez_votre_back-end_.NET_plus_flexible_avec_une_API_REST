using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class TradeRepository
    {
        public LocalDbContext DbContext { get; }

        public TradeRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Trade>> FindAll()
        {
            return await DbContext.Trades.ToListAsync();
        }

        public async Task<Trade?> FindById(int id)
        {
            return await DbContext.Trades.FirstOrDefaultAsync(t => t.TradeId == id);
        }

        public async Task Add(Trade trade)
        {
            DbContext.Trades.Add(trade);
            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> Update(int id, Trade input)
        {
            var existing = await DbContext.Trades.FindAsync(id);
            if (existing == null) return false;

            existing.Account = input.Account;
            existing.AccountType = input.AccountType;
            existing.BuyQuantity = input.BuyQuantity;
            existing.SellQuantity = input.SellQuantity;
            existing.BuyPrice = input.BuyPrice;
            existing.SellPrice = input.SellPrice;
            existing.TradeDate = input.TradeDate;
            existing.TradeSecurity = input.TradeSecurity;
            existing.TradeStatus = input.TradeStatus;
            existing.Trader = input.Trader;
            existing.Benchmark = input.Benchmark;
            existing.Book = input.Book;
            existing.CreationName = input.CreationName;
            existing.CreationDate = input.CreationDate;
            existing.RevisionName = input.RevisionName;
            existing.RevisionDate = input.RevisionDate;
            existing.DealName = input.DealName;
            existing.DealType = input.DealType;
            existing.SourceListId = input.SourceListId;
            existing.Side = input.Side;

            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task Delete(Trade trade)
        {
            DbContext.Trades.Remove(trade);
            await DbContext.SaveChangesAsync();
        }
    }
}
