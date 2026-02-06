using Dapper;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class ActressRepository(MssqlContext context) : IActressRepository
{
    public async Task<int> CreateActress(Actress actress)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        INSERT INTO Actress (Name, Image, CreatedAt)
        VALUES (@Name, @Image, @CreatedAt)
        SELECT SCOPE_IDENTITY()
        ";

        var result = await db.QuerySingleAsync<int>(sql, actress);
        return result;
    }

    public async Task<bool> UpdateActress(Actress actress)
    {
        var db = context.CreateDefaultConnection();

        string sql = @"
        UPDATE Actress
        SET Name = @Name, Image = @Image
        WHERE Id = @Id
        ";

        var result = await db.ExecuteAsync(sql, actress);
        return result > 0;
    }

    public async Task<Actress?> GetActressById(int id)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM Actress WHERE Id = @Id";
        var result = await db.QueryFirstOrDefaultAsync<Actress>(sql, new { Id = id });
        return result;
    }

    public async Task<Actress?> GetActressByName(string name)
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM Actress WHERE Name = @Name";
        var result = await db.QueryFirstOrDefaultAsync<Actress>(sql, new { Name = name });
        return result;
    }

    public async Task<IEnumerable<Actress>> GetAllActresses()
    {
        var db = context.CreateDefaultConnection();
        string sql = "SELECT Id, Name, Image, CreatedAt FROM Actress ORDER BY Name";
        var result = await db.QueryAsync<Actress>(sql);
        return result;
    }
}
