using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Infrastructure.Persistence;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class DotaHeroRepository(MssqlContext context) : IDotaHeroRepository
{
    public async Task<IEnumerable<DotaHero>> GetAll()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<DotaHero>(
            "SELECT Id, Name, Image, CreatedAt FROM DotaHero ORDER BY Name ASC");
    }

    public async Task<DotaHero?> GetById(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<DotaHero>(
            "SELECT Id, Name, Image, CreatedAt FROM DotaHero WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> Create(DotaHero hero)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO DotaHero (Name, Image, CreatedAt)
            VALUES (@Name, @Image, @CreatedAt);
            SELECT SCOPE_IDENTITY();", hero);
    }

    public async Task<bool> Update(DotaHero hero)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(
            "UPDATE DotaHero SET Name = @Name, Image = @Image WHERE Id = @Id", hero);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM DotaHero WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> Exists(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM DotaHero WHERE Id = @Id", new { Id = id }) > 0;
    }
}
