using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Xunit;

namespace P7CreateRestApi.Tests;

public class BidListRepositoryTests
{
    // Test de la méthode Add pour vérifier qu'elle ajoute une entité et que FindById peut la retrouver
    [Fact]
    public async Task Add_Then_FindById_ReturnsEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Add_Then_FindById_ReturnsEntity));
        var repo = new BidListRepository(ctx);

        var entity = new BidList
        {
            Account = "ACC1",
            BidType = "TypeA",
            CreationName = "test",
            RevisionName = "test" 
        };

        await repo.Add(entity);

        var found = await repo.FindById(entity.BidListId);

        Assert.NotNull(found);
        Assert.Equal("ACC1", found!.Account);
    }

    // Test de la méthode Update pour vérifier qu'elle retourne true lorsque l'entité existe et que les modifications sont appliquées
    [Fact]
    public async Task Update_ReturnsTrue_WhenEntityExists()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Update_ReturnsTrue_WhenEntityExists));
        var repo = new BidListRepository(ctx);

        var entity = new BidList
        {
            Account = "ACC1",
            BidType = "TypeA",
            CreationName = "test",
            RevisionName = "test"
        };
        await repo.Add(entity);

        var ok = await repo.Update(entity.BidListId, new BidList
        {
            Account = "ACC2",
            BidType = "TypeB",
            RevisionName = "upd"
        });

        Assert.True(ok);

        var updated = await repo.FindById(entity.BidListId);
        Assert.Equal("ACC2", updated!.Account);
    }


    // Test de la méthode Update pour vérifier qu'elle retourne false lorsque l'entité n'existe pas
    [Fact]
    public async Task Delete_RemovesEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Delete_RemovesEntity));
        var repo = new BidListRepository(ctx);

        var entity = new BidList
        {
            Account = "ACC1",
            BidType = "TypeA",
            CreationName = "test",
            RevisionName = "test"
        };
        await repo.Add(entity);

        var existing = await repo.FindById(entity.BidListId);
        Assert.NotNull(existing);

        await repo.Delete(existing!);

        var after = await repo.FindById(entity.BidListId);
        Assert.Null(after);
    }
}
