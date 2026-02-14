using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class RatingRepository
    {
        // Injection du DbContext pour accéder à la base de données
        public LocalDbContext DbContext { get; }

        public RatingRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        // Trouve toutes les entités
        public async Task<List<Rating>> FindAll()
        {
            return await DbContext.Ratings.ToListAsync();
        }

        // Trouve une entité par son ID
        public async Task<Rating?> FindById(int id)
        {
            return await DbContext.Ratings.FirstOrDefaultAsync(r => r.Id == id);
        }

        // Ajoute une nouvelle entité
        public async Task Add(Rating rating)
        {
            DbContext.Ratings.Add(rating);
            await DbContext.SaveChangesAsync();
        }

        // Met à jour une entité existante
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

        // Supprime une entité
        public async Task Delete(Rating rating)
        {
            DbContext.Ratings.Remove(rating);
            await DbContext.SaveChangesAsync();
        }
    }
}
