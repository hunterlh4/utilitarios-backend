using Dapper;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class ActressAdultRepository(MssqlContext context) : IActressAdultRepository
{
    public async Task<int> CreateActressAdult(ActressAdult actress)
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

    public async Task<bool> UpdateActressAdult(ActressAdult actress)
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

    public async Task<ActressAdult?> GetActressAdultById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM ActressAdult WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<ActressAdult>(sql, new { Id = id });
        return result;
    }

    public async Task<ActressAdult?> GetActressAdultByName(string name)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM ActressAdult WHERE Name = @Name";
        var result = await db.QueryFirstOrDefaultAsync<ActressAdult>(sql, new { Name = name });
        return result;
    }

    public async Task<IEnumerable<ActressAdultDto>> GetAllActressAdultsWithFirstImage()
    {
        var db = context.CreateDefaultConnection();
        
        string sql = $@"
        SELECT 
            a.Id,
            a.Name,
            a.CreatedAt,
            (
                SELECT TOP 1 m.Url 
                FROM Media m 
                WHERE m.Type = {(int)MediaType.ActressAdult} 
                AND m.RefId = a.Id 
                ORDER BY m.OrderIndex
            ) as Image
        FROM ActressAdult a
        ORDER BY a.Name
        ";

        var result = await db.QueryAsync<ActressAdultDto>(sql);
        return result;
    }
}
