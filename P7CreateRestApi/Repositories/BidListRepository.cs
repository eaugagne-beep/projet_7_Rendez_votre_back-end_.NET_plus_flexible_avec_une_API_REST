using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class BidListRepository
    {
        public LocalDbContext DbContext { get; }

        public BidListRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<BidList>> FindAll()
        {
            return await DbContext.BidLists.ToListAsync();
        }

        public async Task<BidList?> FindById(int id)
        {
            return await DbContext.BidLists.FirstOrDefaultAsync(b => b.BidListId == id);
        }

        public async Task Add(BidList bidList)
        {
            DbContext.BidLists.Add(bidList);
            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> Update(int id, BidList input)
        {
            var existing = await DbContext.BidLists.FindAsync(id);
            if (existing == null) return false;

            existing.Account = input.Account;
            existing.BidType = input.BidType;
            existing.BidQuantity = input.BidQuantity;
            existing.AskQuantity = input.AskQuantity;
            existing.Bid = input.Bid;
            existing.Ask = input.Ask;
            existing.Benchmark = input.Benchmark;
            existing.BidListDate = input.BidListDate;
            existing.Commentary = input.Commentary;
            existing.BidSecurity = input.BidSecurity;
            existing.BidStatus = input.BidStatus;
            existing.Trader = input.Trader;
            existing.Book = input.Book;
            existing.DealName = input.DealName;
            existing.DealType = input.DealType;
            existing.SourceListId = input.SourceListId;
            existing.Side = input.Side;

            // serveur
            existing.RevisionDate = input.RevisionDate ?? DateTime.UtcNow;

            await DbContext.SaveChangesAsync();
            return true;
        }



        public async Task Delete(BidList bidList)
        {
            DbContext.BidLists.Remove(bidList);
            await DbContext.SaveChangesAsync();
        }
    }
}
