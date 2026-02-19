using Dapper;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Interfaces;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class ActressAdultRepository(MssqlContext context) : IActressAdultRepository
{
    public async Task<int> CreateActressAdult(Domain.Models.ActressAdult actress)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO ActressAdult (Name, CreatedAt)
        VALUES (@Name, @CreatedAt);
        SELECT SCOPE_IDENTITY();
        ";

        var result = await db.QuerySingleAsync<int>(sql, actress);
        return result;
    }

    public async Task<bool> UpdateActressAdult(Domain.Models.ActressAdult actress)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE ActressAdult
        SET Name = @Name
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, actress);
        return result > 0;
    }

    public async Task<Domain.Models.ActressAdult?> GetActressAdultById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM ActressAdult WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<Domain.Models.ActressAdult>(sql, new { Id = id });
        return result;
    }

    public async Task<Domain.Models.ActressAdult?> GetActressAdultByName(string name)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM ActressAdult WHERE Name = @Name";
        var result = await db.QueryFirstOrDefaultAsync<Domain.Models.ActressAdult>(sql, new { Name = name });
        return result;
    }

    public async Task<IEnumerable<ActressAdultDto>> GetAllActressAdultsWithFirstImage()
    {
        var db = context.CreateDefaultConnection();
        
        string sql = @"
        SELECT 
            a.Id,
            a.Name,
            a.CreatedAt,
            (
                SELECT TOP 1 m.url 
                FROM Media m 
                WHERE m.type = '5' 
                AND m.refId = a.Id 
                ORDER BY m.orderIndex
            ) as FirstImageUrl
        FROM ActressAdult a
        ORDER BY a.Name
        ";

        var result = await db.QueryAsync<ActressAdultDto>(sql);
        return result;
    }
}
