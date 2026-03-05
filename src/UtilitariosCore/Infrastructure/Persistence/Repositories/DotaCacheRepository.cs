using Dapper;
using UtilitariosCore.Application.Features.DotaCaches.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Infrastructure.Persistence;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class DotaCacheRepository(MssqlContext context) : IDotaCacheRepository
{
    private const string SelectWithJoins = @"
        SELECT c.Id, c.TreasureId, t.Name AS TreasureName, c.HeroId,
               h.Name AS HeroName, h.Image AS HeroImage,
               c.Name, c.Photo, c.Price, c.Quantity, c.Total, c.Owner, c.CreatedAt
        FROM DotaCache c
        INNER JOIN DotaTreasure t ON t.Id = c.TreasureId
        INNER JOIN DotaHero h ON h.Id = c.HeroId";

    public async Task<IEnumerable<DotaCacheDto>> GetAll()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<DotaCacheDto>(SelectWithJoins + " ORDER BY t.Name, h.Name");
    }

    public async Task<IEnumerable<DotaCacheDto>> GetByTreasureId(int treasureId)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<DotaCacheDto>(
            SelectWithJoins + " WHERE c.TreasureId = @TreasureId ORDER BY h.Name",
            new { TreasureId = treasureId });
    }

    public async Task<DotaCacheDto?> GetById(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<DotaCacheDto>(
            SelectWithJoins + " WHERE c.Id = @Id", new { Id = id });
    }

    public async Task<int> Create(DotaCache cache)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO DotaCache (TreasureId, HeroId, Name, Photo, Price, Quantity, Total, Owner, CreatedAt)
            VALUES (@TreasureId, @HeroId, @Name, @Photo, @Price, @Quantity, @Total, @Owner, @CreatedAt);
            SELECT SCOPE_IDENTITY();", cache);
    }

    public async Task<bool> Update(DotaCache cache)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(@"
            UPDATE DotaCache SET TreasureId = @TreasureId, HeroId = @HeroId, Name = @Name,
            Photo = @Photo, Price = @Price, Quantity = @Quantity, Total = @Total, Owner = @Owner
            WHERE Id = @Id", cache);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM DotaCache WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> Exists(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM DotaCache WHERE Id = @Id", new { Id = id }) > 0;
    }
}
