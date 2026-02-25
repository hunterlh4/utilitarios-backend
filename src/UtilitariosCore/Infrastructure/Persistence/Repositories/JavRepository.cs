using Dapper;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class JavRepository(MssqlContext context) : IJavRepository
{
    public async Task<int> CreateJav(Jav item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Jav (Code, Image, Status, CreatedAt)
        VALUES (@Code, @Image, @Status, @CreatedAt);
        SELECT SCOPE_IDENTITY();
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);
        return result;
    }

    public async Task<bool> UpdateJav(Jav item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Jav
        SET Code = @Code, Image = @Image
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);
        return result > 0;
    }

    public async Task<bool> UpdateJavStatus(int id, ContentStatus status)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Jav
        SET Status = @Status
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, new { Id = id, Status = status });
        return result > 0;
    }

    public async Task<bool> DeleteJav(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM Jav WHERE Id = @Id";
        var result = await db.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public async Task<Jav?> GetJavById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Code, Image, Status, CreatedAt FROM Jav WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<Jav>(sql, new { Id = id });
        return result;
    }

    public async Task<Jav?> GetJavByCode(string code)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Code, Image, Status, CreatedAt FROM Jav WHERE Code = @Code";
        var result = await db.QueryFirstOrDefaultAsync<Jav>(sql, new { Code = code });
        return result;
    }

    public async Task<IEnumerable<Jav>> GetAllJavs()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Code, Image, Status, CreatedAt FROM Jav ORDER BY CreatedAt DESC";
        var result = await db.QueryAsync<Jav>(sql);
        return result;
    }

    public async Task<IEnumerable<Jav>> GetJavsByActressId(int actressId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        SELECT j.Id, j.Code, j.Image, j.Status, j.CreatedAt
        FROM Jav j
        INNER JOIN JavActress ja ON j.Id = ja.JavId
        WHERE ja.ActressId = @ActressId
        ORDER BY j.CreatedAt DESC
        ";

        var result = await db.QueryAsync<Jav>(sql, new { ActressId = actressId });
        return result;
    }

    public async Task<bool> AddActressToJav(int javId, int actressId)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        IF NOT EXISTS (SELECT 1 FROM JavActress WHERE JavId = @JavId AND ActressId = @ActressId)
        BEGIN
            INSERT INTO JavActress (JavId, ActressId) VALUES (@JavId, @ActressId)
        END
        ";

        var rows = await db.ExecuteAsync(sql, new { JavId = javId, ActressId = actressId });
        return rows > 0;
    }

    public async Task<bool> RemoveActressesFromJav(int javId)
    {
        var db = context.CreateDefaultConnection();
        string sql = "DELETE FROM JavActress WHERE JavId = @JavId";
        var rows = await db.ExecuteAsync(sql, new { JavId = javId });
        return rows > 0;
    }

    public async Task<IEnumerable<int>> GetActressIdsByJavId(int javId)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryAsync<int>(
            "SELECT ActressId FROM JavActress WHERE JavId = @JavId",
            new { JavId = javId });
    }

    public async Task<bool> RemoveActressFromJav(int javId, int actressId)
    {
        var db = context.CreateDefaultConnection();
        var rows = await db.ExecuteAsync(
            "DELETE FROM JavActress WHERE JavId = @JavId AND ActressId = @ActressId",
            new { JavId = javId, ActressId = actressId });
        return rows > 0;
    }

    public async Task<JavWithDetails?> GetJavWithDetailsById(int id)
    {
        var db = context.CreateDefaultConnection();

        string sql = $@"
        SELECT
            j.Id, j.Code, j.Image, j.Status, j.CreatedAt,
            a.Id, a.Name, a.Image, a.CreatedAt,
            l.Id, l.Type, l.RefId, l.Name, l.Url, l.OrderIndex, l.CreatedAt
        FROM Jav j
        LEFT JOIN JavActress ja ON j.Id = ja.JavId
        LEFT JOIN Actress a ON ja.ActressId = a.Id
        LEFT JOIN Link l ON
            (l.RefId = j.Id AND l.Type = {(int)LinkType.Jav})
            OR (l.RefId = a.Id AND l.Type = {(int)LinkType.ActressJav})
        WHERE j.Id = @Id
        ORDER BY a.Id, l.Type, l.Id
        ";

        JavWithDetails? javDetails = null;

        await db.QueryAsync<Jav, ActressJav?, Link?, JavWithDetails>(
            sql,
            (jav, actress, link) =>
            {
                javDetails ??= new JavWithDetails { Jav = jav };

                if (actress != null)
                {
                    var actressEntry = javDetails.Actresses.FirstOrDefault(a => a.Actress.Id == actress.Id);
                    if (actressEntry == null)
                    {
                        actressEntry = new ActressWithLinks { Actress = actress };
                        javDetails.Actresses.Add(actressEntry);
                    }

                    if (link != null && link.Type == LinkType.ActressJav
                        && !actressEntry.Links.Any(l => l.Id == link.Id))
                    {
                        actressEntry.Links.Add(link);
                    }
                }

                if (link != null && link.Type == LinkType.Jav
                    && !javDetails.JavLinks.Any(l => l.Id == link.Id))
                {
                    javDetails.JavLinks.Add(link);
                }

                return javDetails;
            },
            new { Id = id },
            splitOn: "Id,Id,Id"
        );

        return javDetails;
    }

    public async Task<IEnumerable<JavWithDetails>> GetAllJavsWithDetails()
    {
        var db = context.CreateDefaultConnection();

        string sql = $@"
        SELECT
            j.Id, j.Code, j.Image, j.Status, j.CreatedAt,
            a.Id, a.Name, a.Image, a.CreatedAt,
            l.Id, l.Type, l.RefId, l.Name, l.Url, l.OrderIndex, l.CreatedAt
        FROM Jav j
        LEFT JOIN JavActress ja ON j.Id = ja.JavId
        LEFT JOIN Actress a ON ja.ActressId = a.Id
        LEFT JOIN Link l ON
            (l.RefId = j.Id AND l.Type = {(int)LinkType.Jav})
            OR (l.RefId = a.Id AND l.Type = {(int)LinkType.ActressJav})
        ORDER BY j.CreatedAt DESC, j.Id, a.Id, l.Type, l.Id
        ";

        var javDict = new Dictionary<int, JavWithDetails>();

        await db.QueryAsync<Jav, ActressJav?, Link?, JavWithDetails>(
            sql,
            (jav, actress, link) =>
            {
                if (!javDict.TryGetValue(jav.Id, out var javDetails))
                {
                    javDetails = new JavWithDetails { Jav = jav };
                    javDict.Add(jav.Id, javDetails);
                }

                if (actress != null)
                {
                    var actressEntry = javDetails.Actresses.FirstOrDefault(a => a.Actress.Id == actress.Id);
                    if (actressEntry == null)
                    {
                        actressEntry = new ActressWithLinks { Actress = actress };
                        javDetails.Actresses.Add(actressEntry);
                    }

                    if (link != null && link.Type == LinkType.ActressJav
                        && !actressEntry.Links.Any(l => l.Id == link.Id))
                    {
                        actressEntry.Links.Add(link);
                    }
                }

                if (link != null && link.Type == LinkType.Jav
                    && !javDetails.JavLinks.Any(l => l.Id == link.Id))
                {
                    javDetails.JavLinks.Add(link);
                }

                return javDetails;
            },
            splitOn: "Id,Id,Id"
        );

        return javDict.Values;
    }
}
