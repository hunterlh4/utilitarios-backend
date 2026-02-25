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
        INSERT INTO Jav (Code, ActressId, Image, Status, CreatedAt)
        VALUES (@Code, @ActressId, @Image, @Status, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, item);
        return result;
    }

    public async Task<bool> UpdateJav(Jav item)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Jav
        SET Code = @Code, ActressId = @ActressId, Image = @Image
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, item);
        return result > 0;
    }

    public async Task<bool> UpdateJavStatus(int id, Domain.Enums.ContentStatus status)
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
        string sql = "SELECT Id, Code, ActressId, Image, Status, CreatedAt FROM Jav WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<Jav>(sql, new { Id = id });
        return result;
    }

    public async Task<Jav?> GetJavByCode(string code)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Code, ActressId, Image, Status, CreatedAt FROM Jav WHERE Code = @Code";
        var result = await db.QueryFirstOrDefaultAsync<Jav>(sql, new { Code = code });
        return result;
    }

    public async Task<IEnumerable<Jav>> GetAllJavs()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Code, ActressId, Image, Status, CreatedAt FROM Jav ORDER BY CreatedAt DESC";
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

    public async Task<JavWithDetails?> GetJavWithDetailsById(int id)
    {
        var db = context.CreateDefaultConnection();

        string sql = $@"
        SELECT 
            j.Id, j.Code, j.ActressId, j.Image, j.Status, j.CreatedAt,
            a.Id, a.Name, a.Image, a.CreatedAt,
            l.Id, l.Type, l.RefId, l.Name, l.Url, l.OrderIndex, l.CreatedAt
        FROM Jav j
        LEFT JOIN Actress a ON j.ActressId = a.Id
        LEFT JOIN Link l ON (l.RefId = j.Id AND l.Type = {(int)LinkType.Jav}) OR (l.RefId = a.Id AND l.Type = {(int)LinkType.ActressJav})
        WHERE j.Id = @Id
        ORDER BY l.Type, l.Id
        ";

        var javDict = new Dictionary<int, JavWithDetails>();

        await db.QueryAsync<Jav, ActressJav?, Link?, JavWithDetails>(
            sql,
            (jav, actress, link) =>
            {
                if (!javDict.TryGetValue(jav.Id, out var javDetails))
                {
                    javDetails = new JavWithDetails
                    {
                        Jav = jav,
                        Actress = actress
                    };
                    javDict.Add(jav.Id, javDetails);
                }

                if (link != null)
                {
                    if (link.Type == LinkType.Jav)
                    {
                        javDetails.JavLinks.Add(link);
                    }
                    else if (link.Type == LinkType.ActressJav)
                    {
                        javDetails.ActressLinks.Add(link);
                    }
                }

                return javDetails;
            },
            new { Id = id },
            splitOn: "Id,Id,Id"
        );

        return javDict.Values.FirstOrDefault();
    }

    public async Task<IEnumerable<JavWithDetails>> GetAllJavsWithDetails()
    {
        var db = context.CreateDefaultConnection();

        string sql = $@"
        SELECT 
            j.Id, j.Code, j.ActressId, j.Image, j.Status, j.CreatedAt,
            a.Id, a.Name, a.Image, a.CreatedAt,
            l.Id, l.Type, l.RefId, l.Name, l.Url, l.OrderIndex, l.CreatedAt
        FROM Jav j
        LEFT JOIN Actress a ON j.ActressId = a.Id
        LEFT JOIN Link l ON (l.RefId = j.Id AND l.Type = {(int)LinkType.Jav}) OR (l.RefId = a.Id AND l.Type = {(int)LinkType.ActressJav})
        ORDER BY j.CreatedAt DESC, l.Type, l.Id
        ";

        var javDict = new Dictionary<int, JavWithDetails>();

        await db.QueryAsync<Jav, ActressJav?, Link?, JavWithDetails>(
            sql,
            (jav, actress, link) =>
            {
                if (!javDict.TryGetValue(jav.Id, out var javDetails))
                {
                    javDetails = new JavWithDetails
                    {
                        Jav = jav,
                        Actress = actress
                    };
                    javDict.Add(jav.Id, javDetails);
                }

                if (link != null)
                {
                    if (link.Type == LinkType.Jav && !javDetails.JavLinks.Any(l => l.Id == link.Id))
                    {
                        javDetails.JavLinks.Add(link);
                    }
                    else if (link.Type == LinkType.ActressJav && !javDetails.ActressLinks.Any(l => l.Id == link.Id))
                    {
                        javDetails.ActressLinks.Add(link);
                    }
                }

                return javDetails;
            },
            splitOn: "Id,Id,Id"
        );

        return javDict.Values;
    }
}
