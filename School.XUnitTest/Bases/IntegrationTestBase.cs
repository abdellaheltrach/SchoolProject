using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using School.Infrastructure.Bases.UnitOfWork;
using School.Infrastructure.Context;
using School.Infrastructure.Reposetries.Interfaces;
using School.Infrastructure.Repositories;
using School.Infrastructure.Repositories.Interfaces;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected AppDbContext _dbContext = null!;
    protected SqliteConnection _connection = null!;
    protected IServiceScope _scope = null!;
    protected IServiceProvider _serviceProvider = null!;

    public async ValueTask InitializeAsync()
    {
        var services = new ServiceCollection();

        // SQLite in-memory (real relational behavior)
        _connection = new SqliteConnection("Filename=:memory:");
        await _connection.OpenAsync();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(_connection));

        // Repositories
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<StudentRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<DepartmentRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Build DI
        var rootProvider = services.BuildServiceProvider();
        _scope = rootProvider.CreateScope();
        _serviceProvider = _scope.ServiceProvider;

        // Resolve DbContext from DI 
        _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
