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
        INSERT INTO VideoAdult (Source, external_id, video_url, Title, thumbnail_url, embed_html, Status, CreatedAt)
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
        SET Source = @Source, external_id = @ExternalId, video_url = @VideoUrl, 
            Title = @Title, thumbnail_url = @ThumbnailUrl, embed_html = @EmbedHtml, Status = @Status
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, videoAdult);
        return result > 0;
    }

    public async Task<VideoAdult?> GetVideoAdultById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Source, external_id as ExternalId, video_url as VideoUrl, Title, thumbnail_url as ThumbnailUrl, embed_html as EmbedHtml, Status, CreatedAt FROM VideoAdult WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<VideoAdult>(sql, new { Id = id });
        return result;
    }

    public async Task<IEnumerable<VideoAdult>> GetAllVideoAdults()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Source, external_id as ExternalId, video_url as VideoUrl, Title, thumbnail_url as ThumbnailUrl, embed_html as EmbedHtml, Status, CreatedAt FROM VideoAdult ORDER BY CreatedAt DESC";
        var result = await db.QueryAsync<VideoAdult>(sql);
        return result;
    }

    public async Task<bool> AddActressToVideo(int videoAdultId, int actressId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        IF NOT EXISTS (SELECT 1 FROM ActressVideo WHERE video_id = @VideoId AND actress_id = @ActressId)
        BEGIN
            INSERT INTO ActressVideo (video_id, actress_id)
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
        DELETE FROM ActressVideo 
        WHERE video_id = @VideoId AND actress_id = @ActressId
        ";

        var result = await db.ExecuteAsync(sql, new { VideoId = videoAdultId, ActressId = actressId });
        return result > 0;
    }

    public async Task<IEnumerable<int>> GetActressIdsByVideoId(int videoAdultId)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT actress_id FROM ActressVideo WHERE video_id = @VideoId";
        var result = await db.QueryAsync<int>(sql, new { VideoId = videoAdultId });
        return result;
    }

    public async Task<IEnumerable<VideoAdult>> GetVideoAdultsByActressId(int actressId)
    {
        var db = context.CreateDefaultConnection();
        
        string sql = @"
        SELECT v.Id, v.Source, v.external_id as ExternalId, v.video_url as VideoUrl, v.Title, v.thumbnail_url as ThumbnailUrl, v.embed_html as EmbedHtml, v.Status, v.CreatedAt
        FROM VideoAdult v
        INNER JOIN ActressVideo av ON v.Id = av.video_id
        WHERE av.actress_id = @ActressId
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
        FROM Actress a
        INNER JOIN ActressVideo av ON a.Id = av.actress_id
        WHERE av.video_id = @VideoId
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
            v.external_id as ExternalId,
            v.video_url as VideoUrl,
            v.Title,
            v.thumbnail_url as ThumbnailUrl,
            v.Status,
            v.CreatedAt as VideoCreatedAt,
            a.Id,
            a.Name
        FROM VideoAdult v
        INNER JOIN ActressVideo av1 ON v.Id = av1.video_id
        INNER JOIN ActressVideo av2 ON v.Id = av2.video_id
        INNER JOIN ActressAdult a ON av2.actress_id = a.Id
        WHERE av1.actress_id = @ActressId
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
