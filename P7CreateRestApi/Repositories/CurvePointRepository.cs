using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class CurvePointRepository
    {
        public LocalDbContext DbContext { get; }

        public CurvePointRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<CurvePoint>> FindAll()
        {
            return await DbContext.CurvePoints.ToListAsync();
        }

        public async Task<CurvePoint?> FindById(int id)
        {
            return await DbContext.CurvePoints.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Add(CurvePoint curvePoint)
        {
            DbContext.CurvePoints.Add(curvePoint);
            await DbContext.SaveChangesAsync();
        }


        public async Task<bool> Update(int id, CurvePoint input)
        {
            var existing = await DbContext.CurvePoints.FindAsync(id);
            if (existing == null) return false;

            // copie des champs (sans toucher à Id)
            existing.CurveId = input.CurveId;
            existing.AsOfDate = input.AsOfDate;
            existing.Term = input.Term;
            existing.CurvePointValue = input.CurvePointValue;
            existing.CreationDate = input.CreationDate;

            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task Delete(CurvePoint curvePoint)
        {
            DbContext.CurvePoints.Remove(curvePoint);
            await DbContext.SaveChangesAsync();
        }
    }
}
