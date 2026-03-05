using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Infrastructure.Persistence;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class DotaTreasureRepository(MssqlContext context) : IDotaTreasureRepository
{
    public async Task<IEnumerable<DotaTreasure>> GetAll()
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<DotaTreasure>(
            "SELECT Id, Name, Image, ImagePresentation, Year, Type, CreatedAt FROM DotaTreasure ORDER BY Year DESC, Name ASC");
    }

    public async Task<DotaTreasure?> GetById(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<DotaTreasure>(
            "SELECT Id, Name, Image, ImagePresentation, Year, Type, CreatedAt FROM DotaTreasure WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<int> Create(DotaTreasure treasure)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(@"
            INSERT INTO DotaTreasure (Name, Image, ImagePresentation, Year, Type, CreatedAt)
            VALUES (@Name, @Image, @ImagePresentation, @Year, @Type, @CreatedAt);
            SELECT SCOPE_IDENTITY();", treasure);
    }

    public async Task<bool> Update(DotaTreasure treasure)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync(@"
            UPDATE DotaTreasure SET Name = @Name, Image = @Image, ImagePresentation = @ImagePresentation,
            Year = @Year, Type = @Type WHERE Id = @Id", treasure);
        return rows > 0;
    }

    public async Task<bool> Delete(int id)
    {
        var db = context.CreateDefaultConnection();
        int rows = await db.ExecuteAsync("DELETE FROM DotaTreasure WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public async Task<bool> Exists(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM DotaTreasure WHERE Id = @Id", new { Id = id }) > 0;
    }
}
