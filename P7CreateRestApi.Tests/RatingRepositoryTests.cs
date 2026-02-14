using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Xunit;

namespace P7CreateRestApi.Tests;

public class RatingRepositoryTests
{
    
    [Fact]
    public async Task Add_Then_FindById_ReturnsEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Add_Then_FindById_ReturnsEntity));
        var repo = new RatingRepository(ctx);

        var entity = new Rating
        {
            MoodysRating = "A1",
            SandPRating = "AA",
            FitchRating = "A+",
            OrderNumber = 1
        };

        await repo.Add(entity);

        var found = await repo.FindById(entity.Id);

        Assert.NotNull(found);
        Assert.Equal("A1", found!.MoodysRating);
        Assert.Equal("AA", found.SandPRating);
        Assert.Equal("A+", found.FitchRating);
    }

    
    [Fact]
    public async Task Update_ReturnsTrue_WhenEntityExists()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Update_ReturnsTrue_WhenEntityExists));
        var repo = new RatingRepository(ctx);

        var entity = new Rating
        {
            MoodysRating = "A1",
            SandPRating = "AA",
            FitchRating = "A+",
            OrderNumber = 1
        };

        await repo.Add(entity);

        var ok = await repo.Update(entity.Id, new Rating
        {
            MoodysRating = "B1",
            SandPRating = "BB",
            FitchRating = "B+",
            OrderNumber = 2
        });

        Assert.True(ok);

        var updated = await repo.FindById(entity.Id);
        Assert.NotNull(updated);
        Assert.Equal("B1", updated!.MoodysRating);
        Assert.Equal("BB", updated.SandPRating);
    }


    
    [Fact]
    public async Task Delete_RemovesEntity()
    {
        using var ctx = TestDbFactory.CreateContext(nameof(Delete_RemovesEntity));
        var repo = new RatingRepository(ctx);

        var entity = new Rating
        {
            MoodysRating = "A1",
            SandPRating = "AA",
            FitchRating = "A+",
            OrderNumber = 1
        };

        await repo.Add(entity);

        var existing = await repo.FindById(entity.Id);
        Assert.NotNull(existing);

        await repo.Delete(existing!);

        var after = await repo.FindById(entity.Id);
        Assert.Null(after);
    }
}
