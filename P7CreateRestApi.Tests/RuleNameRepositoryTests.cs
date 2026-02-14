using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Xunit;

namespace P7CreateRestApi.Tests;

public class RuleNameRepositoryTests
{

    // Test d'ajout et de recherche d'une entité
    [Fact]
    public async Task Add_Then_FindById_ReturnsEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Add_Then_FindById_ReturnsEntity));
        var repo = new RuleNameRepository(ctx);

        var entity = new RuleName
        {
            Name = "Rule1",
            Description = "Description",
            Json = "{}",
            Template = "tpl",
            SqlStr = "SELECT 1",
            SqlPart = "WHERE 1=1"
        };

        await repo.Add(entity);

        var found = await repo.FindById(entity.Id);

        Assert.NotNull(found);
        Assert.Equal("Rule1", found!.Name);
        Assert.Equal("Description", found.Description);
    }

    // Test de mise à jour d'une entité
    [Fact]
    public async Task Update_ReturnsTrue_WhenEntityExists()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Update_ReturnsTrue_WhenEntityExists));
        var repo = new RuleNameRepository(ctx);

        var entity = new RuleName
        {
            Name = "Rule1",
            Description = "Description"
        };

        await repo.Add(entity);

        var ok = await repo.Update(entity.Id, new RuleName
        {
            Name = "Rule2",
            Description = "Updated"
        });

        Assert.True(ok);

        var updated = await repo.FindById(entity.Id);
        Assert.NotNull(updated);
        Assert.Equal("Rule2", updated!.Name);
        Assert.Equal("Updated", updated.Description);
    }

    // Test de suppression d'une entité
    [Fact]
    public async Task Delete_RemovesEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Delete_RemovesEntity));
        var repo = new RuleNameRepository(ctx);

        var entity = new RuleName
        {
            Name = "Rule1",
            Description = "Description"
        };

        await repo.Add(entity);

        var existing = await repo.FindById(entity.Id);
        Assert.NotNull(existing);

        await repo.Delete(existing!);

        var after = await repo.FindById(entity.Id);
        Assert.Null(after);
    }
}
