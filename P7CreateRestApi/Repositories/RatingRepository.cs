using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class RatingRepository
    {
        public LocalDbContext DbContext { get; }

        public RatingRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Rating>> FindAll()
        {
            return await DbContext.Ratings.ToListAsync();
        }

        public async Task<Rating?> FindById(int id)
        {
            return await DbContext.Ratings.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task Add(Rating rating)
        {
            DbContext.Ratings.Add(rating);
            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> Update(int id, Rating input)
        {
            var existing = await DbContext.Ratings.FindAsync(id);
            if (existing == null) return false;

            existing.MoodysRating = input.MoodysRating;
            existing.SandPRating = input.SandPRating;
            existing.FitchRating = input.FitchRating;
            existing.OrderNumber = input.OrderNumber;

            await DbContext.SaveChangesAsync();
            return true;
        }


        public async Task Delete(Rating rating)
        {
            DbContext.Ratings.Remove(rating);
            await DbContext.SaveChangesAsync();
        }
    }
}
