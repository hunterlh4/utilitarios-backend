using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class SeriesRepository(MssqlContext context) : ISeriesRepository
{
    public async Task<int> CreateSeries(Series item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Series (ImdbId, Title, Image, Year, Rating, Type, Status, CreatedAt)
        VALUES (@ImdbId, @Title, @Image, @Year, @Rating, @Type, @Status, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);
        return result;
    }

    public async Task<bool> UpdateSeries(Series item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Series
        SET ImdbId = @ImdbId, Title = @Title, Image = @Image, Year = @Year, 
            Rating = @Rating, Type = @Type, Status=@Status
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);
        return result > 0;
    }

    public async Task<bool> DeleteSeries(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM Series WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<Series?> GetSeriesById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, ImdbId, Title, Image, Year, Rating, Type, Status, CreatedAt FROM Series WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<Series>(sql, new { Id = id });
        return result;
    }

    public async Task<Series?> GetSeriesByImdbId(string imdbId)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, ImdbId, Title, Image, Year, Rating, Type, Status, CreatedAt FROM Series WHERE ImdbId = @ImdbId";
        var result = await db.QueryFirstOrDefaultAsync<Series>(sql, new { ImdbId = imdbId });
        return result;
    }

    public async Task<IEnumerable<Series>> GetAllSeries()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, ImdbId, Title, Image, Year, Rating, Type, Status, CreatedAt FROM Series ORDER BY CreatedAt DESC";
        var result = await db.QueryAsync<Series>(sql);
        return result;
    }
}
