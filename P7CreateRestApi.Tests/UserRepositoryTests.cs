using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Xunit;

namespace P7CreateRestApi.Tests;

public class UserRepositoryTests
{

    // test de l'ajout d'un utilisateur et de sa récupération par son id
    [Fact]
    public async Task Add_Then_FindById_ReturnsEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Add_Then_FindById_ReturnsEntity));
        var repo = new UserRepository(ctx);

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            FullName = "Admin User"
        };

        await repo.Add(user);

        var found = await repo.FindById(user.Id);

        Assert.NotNull(found);
        Assert.Equal("admin", found!.UserName);
        Assert.Equal("Admin User", found.FullName);
    }


    // test de la mise à jour d'un utilisateur
    [Fact]
    public async Task Update_ReturnsTrue_WhenUserExists()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Update_ReturnsTrue_WhenUserExists));
        var repo = new UserRepository(ctx);

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "user1",
            NormalizedUserName = "USER1",
            FullName = "User One"
        };

        await repo.Add(user);

        var ok = await repo.Update(user.Id, new User
        {
            UserName = "user2"
        });

        Assert.True(ok);

        var updated = await repo.FindById(user.Id);
        Assert.Equal("user2", updated!.UserName);
    }


    // suppression test de la suppression d'un utilisateur
    [Fact]
    public async Task Delete_RemovesUser()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Delete_RemovesUser));
        var repo = new UserRepository(ctx);

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "todelete",
            NormalizedUserName = "TODELETE",
            FullName = "Delete Me"
        };

        await repo.Add(user);

        var existing = await repo.FindById(user.Id);
        Assert.NotNull(existing);

        await repo.Delete(existing!);

        var after = await repo.FindById(user.Id);
        Assert.Null(after);
    }

    // test de la recherche d'un utilisateur par son nom d'utilisateur
    [Fact]
    public async Task FindByUserName_ReturnsUser()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(FindByUserName_ReturnsUser));
        var repo = new UserRepository(ctx);

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "lookup",
            NormalizedUserName = "LOOKUP",
            FullName = "Lookup User"
        };

        await repo.Add(user);

        var found = repo.FindByUserName("lookup");

        Assert.NotNull(found);
        Assert.Equal("Lookup User", found!.FullName);
    }
}
