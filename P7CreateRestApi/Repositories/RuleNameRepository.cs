using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class RuleNameRepository
    {
        public LocalDbContext DbContext { get; }

        public RuleNameRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<RuleName>> FindAll()
        {
            return await DbContext.RuleNames.ToListAsync();
        }

        public async Task<RuleName?> FindById(int id)
        {
            return await DbContext.RuleNames.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task Add(RuleName rule)
        {
            DbContext.RuleNames.Add(rule);
            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> Update(int id, RuleName input)
        {
            var existing = await DbContext.RuleNames.FindAsync(id);
            if (existing == null) return false;

            existing.Name = input.Name;
            existing.Description = input.Description;
            existing.Json = input.Json;
            existing.Template = input.Template;
            existing.SqlStr = input.SqlStr;
            existing.SqlPart = input.SqlPart;

            await DbContext.SaveChangesAsync();
            return true;
        }


        public async Task Delete(RuleName rule)
        {
            DbContext.RuleNames.Remove(rule);
            await DbContext.SaveChangesAsync();
        }
    }
}
