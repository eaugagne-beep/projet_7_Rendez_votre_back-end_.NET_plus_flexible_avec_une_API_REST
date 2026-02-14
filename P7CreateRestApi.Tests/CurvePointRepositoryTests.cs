using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Xunit;

namespace P7CreateRestApi.Tests;

public class CurvePointRepositoryTests
{
    // Test de la méthode Add pour vérifier qu'une entité est ajoutée et que FindById peut la retrouver
    [Fact]
    public async Task Add_Then_FindById_ReturnsEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Add_Then_FindById_ReturnsEntity));
        var repo = new CurvePointRepository(ctx);

        var entity = new CurvePoint
        {
            CurveId = 1,
            Term = 10,
            CurvePointValue = 100,
            AsOfDate = DateTime.UtcNow,
            CreationDate = DateTime.UtcNow
        };

        await repo.Add(entity);

        var found = await repo.FindById(entity.Id);

        Assert.NotNull(found);
        Assert.Equal((byte)1, found!.CurveId);
        Assert.Equal(10, found.Term);
        Assert.Equal(100, found.CurvePointValue);
    }

    // Test de la méthode Update pour vérifier que les propriétés d'une entité sont mises à jour correctement
    [Fact]
    public async Task Update_ReturnsTrue_WhenEntityExists()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Update_ReturnsTrue_WhenEntityExists));
        var repo = new CurvePointRepository(ctx);

        var entity = new CurvePoint
        {
            CurveId = 1,
            Term = 10,
            CurvePointValue = 100
        };

        await repo.Add(entity);

        var ok = await repo.Update(entity.Id, new CurvePoint
        {
            CurveId = 2,
            Term = 20,
            CurvePointValue = 200,
            AsOfDate = DateTime.UtcNow,
            CreationDate = DateTime.UtcNow
        });

        Assert.True(ok);

        var updated = await repo.FindById(entity.Id);
        Assert.NotNull(updated);
        Assert.Equal((byte)2, updated!.CurveId);
        Assert.Equal(20, updated.Term);
        Assert.Equal(200, updated.CurvePointValue);
    }

    // Test de la méthode Delete pour vérifier qu'une entité est supprimée et que FindById ne la retrouve plus
    [Fact]
    public async Task Delete_RemovesEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Delete_RemovesEntity));
        var repo = new CurvePointRepository(ctx);

        var entity = new CurvePoint
        {
            CurveId = 1,
            Term = 10,
            CurvePointValue = 100
        };

        await repo.Add(entity);

        var existing = await repo.FindById(entity.Id);
        Assert.NotNull(existing);

        await repo.Delete(existing!);

        var after = await repo.FindById(entity.Id);
        Assert.Null(after);
    }
}

