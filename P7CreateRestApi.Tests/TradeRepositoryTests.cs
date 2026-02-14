using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Xunit;

namespace P7CreateRestApi.Tests;

public class TradeRepositoryTests
{

    // Test de création et de récupération d'une entité
    [Fact]
    public async Task Add_Then_FindById_ReturnsEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Add_Then_FindById_ReturnsEntity));
        var repo = new TradeRepository(ctx);

        var entity = new Trade
        {
            Account = "ACC1",
            AccountType = "TypeA",
            BuyQuantity = 10,
            BuyPrice = 100,
            TradeSecurity = "SEC1",
            CreationName = "test",
            RevisionName = "test"
        };

        await repo.Add(entity);

        var found = await repo.FindById(entity.TradeId);

        Assert.NotNull(found);
        Assert.Equal("ACC1", found!.Account);
        Assert.Equal("TypeA", found.AccountType);
    }

    // Test de mise à jour d'une entité
    [Fact]
    public async Task Update_ReturnsTrue_WhenEntityExists()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Update_ReturnsTrue_WhenEntityExists));
        var repo = new TradeRepository(ctx);

        var entity = new Trade
        {
            Account = "ACC1",
            AccountType = "TypeA",
            CreationName = "test",
            RevisionName = "test"
        };

        await repo.Add(entity);

        var ok = await repo.Update(entity.TradeId, new Trade
        {
            Account = "ACC2",
            AccountType = "TypeB",
            RevisionName = "upd"
        });

        Assert.True(ok);

        var updated = await repo.FindById(entity.TradeId);
        Assert.NotNull(updated);
        Assert.Equal("ACC2", updated!.Account);
        Assert.Equal("TypeB", updated.AccountType);
    }

    // Test de suppression d'une entité
    [Fact]
    public async Task Delete_RemovesEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Delete_RemovesEntity));
        var repo = new TradeRepository(ctx);

        var entity = new Trade
        {
            Account = "ACC1",
            AccountType = "TypeA",
            CreationName = "test",
            RevisionName = "test"
        };

        await repo.Add(entity);

        var existing = await repo.FindById(entity.TradeId);
        Assert.NotNull(existing);

        await repo.Delete(existing!);

        var after = await repo.FindById(entity.TradeId);
        Assert.Null(after);
    }
}
