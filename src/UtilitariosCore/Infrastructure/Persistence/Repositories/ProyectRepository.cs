using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class ProyectRepository(MssqlContext context) : IProyectRepository
{
    public async Task<IEnumerable<Proyect>> GetAll()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<Proyect>(
            "SELECT Id, Name, Description, Url, CreatedAt FROM Proyect ORDER BY CreatedAt DESC");
    }

    public async Task<Proyect?> GetById(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<Proyect>(
            "SELECT Id, Name, Description, Url, CreatedAt FROM Proyect WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<int> Create(Proyect proyect)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO Proyect (Name, Description, Url, CreatedAt)
            VALUES (@Name, @Description, @Url, @CreatedAt);
            SELECT SCOPE_IDENTITY();",
            proyect);
    }

    public async Task<bool> Update(Proyect proyect)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(@"
            UPDATE Proyect SET Name = @Name, Description = @Description, Url = @Url
            WHERE Id = @Id",
            proyect);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM Proyect WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> Exists(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM Proyect WHERE Id = @Id", new { Id = id }) > 0;
    }
}
