using Dapper;
using UtilitariosCore.Application.Features.GirlGaleries.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class GirlGaleryRepository(MssqlContext context) : IGirlGaleryRepository
{
    public async Task<int> CreateGirlGalery(GirlGalery item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO GirlGalery (Name, CreatedAt)
        VALUES (@Name, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);
        return result;
    }

    public async Task<bool> UpdateGirlGalery(GirlGalery item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE GirlGalery
        SET Name = @Name
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);
        return result > 0;
    }

    public async Task<bool> DeleteGirlGalery(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM GirlGalery WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<GirlGalery?> GetGirlGaleryById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM GirlGalery WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<GirlGalery>(sql, new { Id = id });
        return result;
    }

    public async Task<IEnumerable<GirlGalery>> GetAllGirlGaleries()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, CreatedAt FROM GirlGalery ORDER BY CreatedAt DESC";
        var result = await db.QueryAsync<GirlGalery>(sql);
        return result;
    }

    public async Task<IEnumerable<GirlGaleryDto>> GetAllGirlGaleriesWithFirstImage()
    {
        var db = context.CreateDefaultConnection();

        string sql = $@"
        SELECT
            g.Id,
            g.Name,
            g.CreatedAt,
            (
                SELECT TOP 1 m.Url
                FROM Media m
                WHERE m.Type = {(int)MediaType.GirlGalery}
                AND m.RefId = g.Id
                ORDER BY m.OrderIndex
            ) AS FirstImageUrl
        FROM GirlGalery g
        ORDER BY g.CreatedAt DESC
        ";

        var result = await db.QueryAsync<GirlGaleryDto>(sql);
        return result;
    }
}
