using Dapper;
using UtilitariosCore.Application.Features.YouTubes.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class YouTubeRepository(MssqlContext context) : IYouTubeRepository
{
    public async Task<IEnumerable<YouTubeDto>> GetAll(YouTubeCategory? category = null)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT Id, Url, Title, AuthorName, AuthorUrl, Type, Height, Width,
               Version, ProviderName, ProviderUrl, ThumbnailHeight, ThumbnailWidth,
               ThumbnailUrl, Html, Category, CreatedAt
        FROM YouTube
        WHERE (@Category IS NULL OR Category = @Category)
        ORDER BY CreatedAt DESC";

        return await db.QueryAsync<YouTubeDto>(sql, new { Category = (int?)category });
    }

    public async Task<int> Create(YouTube item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO YouTube (Url, Title, AuthorName, AuthorUrl, Type, Height, Width,
                             Version, ProviderName, ProviderUrl, ThumbnailHeight, ThumbnailWidth,
                             ThumbnailUrl, Html, Category, CreatedAt)
        VALUES (@Url, @Title, @AuthorName, @AuthorUrl, @Type, @Height, @Width,
                @Version, @ProviderName, @ProviderUrl, @ThumbnailHeight, @ThumbnailWidth,
                @ThumbnailUrl, @Html, @Category, @CreatedAt);
        SELECT SCOPE_IDENTITY()";

        return await db.QuerySingleAsync<int>(sql, item);
    }

    public async Task<bool> Delete(int id)
    {
        var db = context.CreateDefaultConnection();
        var result = await db.ExecuteAsync("DELETE FROM YouTube WHERE Id = @Id", new { Id = id });
        return result > 0;
    }

    public async Task<bool> ExistsByUrl(string url)
    {
        var db = context.CreateDefaultConnection();
        var count = await db.QuerySingleAsync<int>(
            "SELECT COUNT(1) FROM YouTube WHERE Url = @Url", new { Url = url });
        return count > 0;
    }
}
