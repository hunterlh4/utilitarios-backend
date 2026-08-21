using Dapper;
using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class VideoAdultRepository(MssqlContext context) : IVideoAdultRepository
{
    public async Task<int> CreateVideoAdult(VideoAdult videoAdult)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO VideoAdult (Source, ExternalId, VideoUrl, Title, ThumbnailUrl, EmbedHtml, Status, CreatedAt)
        VALUES (@Source, @ExternalId, @VideoUrl, @Title, @ThumbnailUrl, @EmbedHtml, @Status, @CreatedAt);
        SELECT SCOPE_IDENTITY();
        ";

        var result = await db.QuerySingleAsync<int>(sql, videoAdult);
        return result;
    }

    public async Task<bool> UpdateVideoAdult(VideoAdult videoAdult)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE VideoAdult
        SET Source = @Source, ExternalId = @ExternalId, VideoUrl = @VideoUrl, 
            Title = @Title, ThumbnailUrl = @ThumbnailUrl, EmbedHtml = @EmbedHtml, Status = @Status
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, videoAdult);
        return result > 0;
    }

    public async Task<bool> DeleteVideoAdult(int videoAdultId)
    {
        var db = context.CreateDefaultConnection();

        // Primero eliminar las relaciones con actrices
        string deleteRelationsSql = "DELETE FROM RelationActressVideo WHERE VideoAdultId = @VideoId";
        await db.ExecuteAsync(deleteRelationsSql, new { VideoId = videoAdultId });

        // Luego eliminar el video
        string deleteVideoSql = "DELETE FROM VideoAdult WHERE Id = @Id";
        var result = await db.ExecuteAsync(deleteVideoSql, new { Id = videoAdultId });
        
        return result > 0;
    }

    public async Task<VideoAdult?> GetVideoAdultById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Source, ExternalId, VideoUrl, Title, ThumbnailUrl, EmbedHtml, Status, CreatedAt FROM VideoAdult WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<VideoAdult>(sql, new { Id = id });
        return result;
    }

    public async Task<VideoAdult?> GetVideoAdultBySourceAndExternalId(string source, string externalId)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Source, ExternalId, VideoUrl, Title, ThumbnailUrl, EmbedHtml, Status, CreatedAt FROM VideoAdult WHERE Source = @Source AND ExternalId = @ExternalId";
        var result = await db.QueryFirstOrDefaultAsync<VideoAdult>(sql, new { Source = source, ExternalId = externalId });
        return result;
    }

    public async Task<IEnumerable<VideoAdult>> GetAllVideoAdults()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Source, ExternalId, VideoUrl, Title, ThumbnailUrl, EmbedHtml, Status, CreatedAt FROM VideoAdult ORDER BY CreatedAt DESC";
        var result = await db.QueryAsync<VideoAdult>(sql);
        return result;
    }

    public async Task<bool> AddActressToVideo(int videoAdultId, int actressId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        IF NOT EXISTS (SELECT 1 FROM RelationActressVideo WHERE VideoAdultId = @VideoId AND ActressAdultId = @ActressId)
        BEGIN
            INSERT INTO RelationActressVideo (VideoAdultId, ActressAdultId)
            VALUES (@VideoId, @ActressId)
        END
        ";

        var result = await db.ExecuteAsync(sql, new { VideoId = videoAdultId, ActressId = actressId });
        return result > 0;
    }

    public async Task<bool> RemoveActressFromVideo(int videoAdultId, int actressId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        DELETE FROM RelationActressVideo 
        WHERE VideoAdultId = @VideoId AND ActressAdultId = @ActressId
        ";

        var result = await db.ExecuteAsync(sql, new { VideoId = videoAdultId, ActressId = actressId });
        return result > 0;
    }

    public async Task<IEnumerable<int>> GetActressIdsByVideoId(int videoAdultId)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT ActressAdultId FROM RelationActressVideo WHERE VideoAdultId = @VideoId";
        var result = await db.QueryAsync<int>(sql, new { VideoId = videoAdultId });
        return result;
    }

    public async Task<IEnumerable<VideoAdult>> GetVideoAdultsByActressId(int actressId)
    {
        var db = context.CreateDefaultConnection();
        
        string sql = @"
        SELECT v.Id, v.Source, v.ExternalId, v.VideoUrl, v.Title, v.ThumbnailUrl, v.EmbedHtml, v.Status, v.CreatedAt
        FROM VideoAdult v
        INNER JOIN RelationActressVideo av ON v.Id = av.VideoAdultId
        WHERE av.ActressAdultId = @ActressId
        ORDER BY v.CreatedAt DESC
        ";

        var result = await db.QueryAsync<VideoAdult>(sql, new { ActressId = actressId });
        return result;
    }

    public async Task<IEnumerable<ActressJav>> GetActressesByVideoId(int videoAdultId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT a.Id, a.Name, a.CreatedAt
        FROM ActressAdult a
        INNER JOIN RelationActressVideo av ON a.Id = av.ActressAdultId
        WHERE av.VideoAdultId = @VideoId
        ORDER BY a.Name
        ";

        var result = await db.QueryAsync<ActressJav>(sql, new { VideoId = videoAdultId });
        return result;
    }

    public async Task<IEnumerable<VideoAdultGrouped>> GetVideoAdultsWithActressesByActressId(int actressId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT 
            v.Id as VideoId,
            v.Source,
            v.ExternalId,
            v.VideoUrl,
            v.Title,
            v.ThumbnailUrl,
            v.Status,
            v.CreatedAt as VideoCreatedAt,
            a.Id,
            a.Name
        FROM VideoAdult v
        INNER JOIN RelationActressVideo av1 ON v.Id = av1.VideoAdultId
        INNER JOIN RelationActressVideo av2 ON v.Id = av2.VideoAdultId
        INNER JOIN ActressAdult a ON av2.ActressAdultId = a.Id
        WHERE av1.ActressAdultId = @ActressId
        ORDER BY v.CreatedAt DESC, a.Name
        ";

        var videoDictionary = new Dictionary<int, VideoAdultGrouped>();

        await db.QueryAsync<VideoAdultGrouped, ActressInfo, VideoAdultGrouped>(
            sql,
            (video, actress) =>
            {
                if (!videoDictionary.TryGetValue(video.VideoId, out var videoEntry))
                {
                    videoEntry = video;
                    videoEntry.Actresses = new List<ActressInfo>();
                    videoDictionary.Add(video.VideoId, videoEntry);
                }

                if (actress != null && actress.Id > 0)
                {
                    videoEntry.Actresses.Add(actress);
                }
                
                return videoEntry;
            },
            new { ActressId = actressId },
            splitOn: "Id"
        );

        return videoDictionary.Values;
    }
}
