using Dot.Net.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Tests;

public static class TestDbFactory
{
    public static LocalDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new LocalDbContext(options);
    }
}
