using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class ProjectRepository(MssqlContext context) : IProjectRepository
{
    public async Task<IEnumerable<Project>> GetAll()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<Project>(
            "SELECT Id, Name, Description, Url, CreatedAt FROM Project ORDER BY CreatedAt DESC");
    }

    public async Task<Project?> GetById(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<Project>(
            "SELECT Id, Name, Description, Url, CreatedAt FROM Project WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<int> Create(Project project)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO Project (Name, Description, Url, CreatedAt)
            VALUES (@Name, @Description, @Url, @CreatedAt);
            SELECT SCOPE_IDENTITY();",
            project);
    }

    public async Task<bool> Update(Project project)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(@"
            UPDATE Project SET Name = @Name, Description = @Description, Url = @Url
            WHERE Id = @Id",
            project);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM Project WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> Exists(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM Project WHERE Id = @Id", new { Id = id }) > 0;
    }
}
