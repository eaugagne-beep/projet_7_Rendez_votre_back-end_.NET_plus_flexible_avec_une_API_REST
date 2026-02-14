using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository
    {
        // Injection du DbContext pour accéder à la base de données
        public LocalDbContext DbContext { get; }

        public UserRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        // Trouve une entité par son nom d'utilisateur, retourne null si elle n'existe pas
        public User? FindByUserName(string userName)
        {
            return DbContext.Users
                            .FirstOrDefault(user => user.UserName == userName);
        }

        //  Trouve toutes les entités, retourne une liste vide si aucune n'existe
        public async Task<List<User>> FindAll()
        {
            return await DbContext.Users.ToListAsync();
        }

        // Trouve une entité par son identifiant, retourne null si elle n'existe pas
        public async Task<User?> FindById(string id)
        {
            return await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        // Ajoute une nouvelle entité à la base de données
        public async Task Add(User user)
        {
            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();
        }

        // Met à jour une entité existante, retourne false si l'entité n'existe pas
        public async Task<bool> Update(string id, User input)
        {
            var existing = await DbContext.Users.FindAsync(id);
            if (existing == null) return false;

            existing.UserName = input.UserName;
            

            await DbContext.SaveChangesAsync();
            return true;
        }

        // Supprime une entité de la base de données
        public async Task Delete(User user)
        {
            DbContext.Users.Remove(user);
            await DbContext.SaveChangesAsync();
        }
    }
}
