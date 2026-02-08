using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using School.Infrastructure.Context;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected AppDbContext _dbContext = null!;
    private SqliteConnection _connection = null!;

    public async ValueTask InitializeAsync()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _dbContext = new AppDbContext(options);
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
