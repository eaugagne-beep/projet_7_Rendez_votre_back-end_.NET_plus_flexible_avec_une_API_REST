using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class TradeRepository
    {
        // Injection du DbContext pour accéder à la base de données
        public LocalDbContext DbContext { get; }

        public TradeRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        // Trouve toutes les entités
        public async Task<List<Trade>> FindAll()
        {
            return await DbContext.Trades.ToListAsync();
        }

        // Trouve une entité par son ID, retourne null si elle n'existe pas
        public async Task<Trade?> FindById(int id)
        {
            return await DbContext.Trades.FirstOrDefaultAsync(t => t.TradeId == id);
        }

        // Ajoute une nouvelle entité à la base de données
        public async Task Add(Trade trade)
        {
            DbContext.Trades.Add(trade);
            await DbContext.SaveChangesAsync();
        }

        // Met à jour une entité existante, retourne false si l'entité n'existe pas
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
            existing.DealName = input.DealName;
            existing.DealType = input.DealType;
            existing.SourceListId = input.SourceListId;
            existing.Side = input.Side;

            existing.RevisionDate = input.RevisionDate ?? DateTime.UtcNow;

            await DbContext.SaveChangesAsync();
            return true;
        }

        // Supprime une entité de la base de données
        public async Task Delete(Trade trade)
        {
            DbContext.Trades.Remove(trade);
            await DbContext.SaveChangesAsync();
        }
    }
}
