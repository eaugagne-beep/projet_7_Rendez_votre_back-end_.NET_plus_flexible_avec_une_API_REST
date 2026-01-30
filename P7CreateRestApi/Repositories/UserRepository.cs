using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository
    {
        public LocalDbContext DbContext { get; }

        public UserRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public User? FindByUserName(string userName)
        {
            return DbContext.Users
                            .FirstOrDefault(user => user.UserName == userName);
        }

        public async Task<List<User>> FindAll()
        {
            return await DbContext.Users.ToListAsync();
        }

        public async Task<User?> FindById(string id)
        {
            return await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Add(User user)
        {
            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> Update(string id, User input)
        {
            var existing = await DbContext.Users.FindAsync(id);
            if (existing == null) return false;

            existing.UserName = input.UserName;
            

            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task Delete(User user)
        {
            DbContext.Users.Remove(user);
            await DbContext.SaveChangesAsync();
        }
    }
}
