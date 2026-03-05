using Dapper;
using UtilitariosCore.Application.Features.Payments.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Infrastructure.Persistence.Repositories;

public class PaymentRepository(MssqlContext context) : IPaymentRepository
{
    private static bool IsPositive(int type) => type == 1 || type == 3;

    public async Task<IEnumerable<PaymentDetailDto>> GetAllPayments()
    {
        var db = context.CreateDefaultConnection();

        // Query 1: todos los payments con balance calculado en SQL
        string sqlPayments = @"
        SELECT
            p.Id,
            p.PersonName,
            p.Amount,
            p.CreatedAt,
            ISNULL(SUM(
                CASE
                    WHEN pd.Type IN (1, 3) THEN pd.Amount
                    WHEN pd.Type IN (2, 4) THEN -pd.Amount
                    ELSE 0
                END
            ), 0) AS Balance
        FROM Payment p
        LEFT JOIN PaymentDetail pd ON pd.PaymentId = p.Id
        GROUP BY p.Id, p.PersonName, p.Amount, p.CreatedAt
        ORDER BY p.CreatedAt DESC";

        var payments = (await db.QueryAsync<PaymentDetailDto>(sqlPayments)).ToList();

        if (payments.Count == 0) return payments;

        // Query 2: todos los detalles de esos payments en una sola consulta
        var paymentIds = payments.Select(p => p.Id).ToList();
        var details = (await db.QueryAsync<PaymentDetailItemDto>(
            @"SELECT Id, PaymentId, Type, Amount, Date, Description, CreatedAt
              FROM PaymentDetail
              WHERE PaymentId IN @Ids
              ORDER BY PaymentId, Date, CreatedAt",
            new { Ids = paymentIds })).ToList();

        // Mapeo en memoria agrupando por PaymentId
        var detailsByPayment = details.GroupBy(d => d.PaymentId)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var p in payments)
            p.Details = detailsByPayment.TryGetValue(p.Id, out var list) ? list : [];

        return payments;
    }

    public async Task<PaymentDetailDto?> GetPaymentById(int id)
    {
        var db = context.CreateDefaultConnection();

        var payment = await db.QueryFirstOrDefaultAsync<Payment>(
            "SELECT Id, PersonName, Amount, CreatedAt FROM Payment WHERE Id = @Id",
            new { Id = id });

        if (payment is null) return null;

        var details = (await db.QueryAsync<PaymentDetail>(
            "SELECT Id, PaymentId, Type, Amount, Date, Description, CreatedAt FROM PaymentDetail WHERE PaymentId = @Id ORDER BY Date, CreatedAt",
            new { Id = id })).ToList();

        decimal balance = details.Sum(d => IsPositive(d.Type) ? d.Amount : -d.Amount);

        return new PaymentDetailDto
        {
            Id = payment.Id,
            PersonName = payment.PersonName,
            Amount = payment.Amount,
            Balance = balance,
            CreatedAt = payment.CreatedAt,
            Details = details.Select(d => new PaymentDetailItemDto
            {
                Id = d.Id,
                Type = d.Type,
                Amount = d.Amount,
                Date = d.Date,
                Description = d.Description,
                CreatedAt = d.CreatedAt
            }).ToList()
        };
    }

    public async Task<int> CreatePayment(Payment payment)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
        INSERT INTO Payment (PersonName, Amount, CreatedAt)
        VALUES (@PersonName, @Amount, @CreatedAt);
        SELECT SCOPE_IDENTITY();
        ";
        return await db.QuerySingleAsync<int>(sql, payment);
    }

    public async Task<bool> DeletePayment(int id)
    {
        var db = context.CreateDefaultConnection();
        var result = await db.ExecuteAsync("DELETE FROM Payment WHERE Id = @Id", new { Id = id });
        return result > 0;
    }

    public async Task<int> AddDetail(PaymentDetail detail)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
        INSERT INTO PaymentDetail (PaymentId, Type, Amount, Date, Description, CreatedAt)
        VALUES (@PaymentId, @Type, @Amount, @Date, @Description, @CreatedAt);
        SELECT SCOPE_IDENTITY();
        ";
        return await db.QuerySingleAsync<int>(sql, detail);
    }

    public async Task<bool> UpdateDetail(PaymentDetail detail)
    {
        var db = context.CreateDefaultConnection();
        string sql = @"
        UPDATE PaymentDetail
        SET Type = @Type, Amount = @Amount, Date = @Date, Description = @Description
        WHERE Id = @Id
        ";
        var result = await db.ExecuteAsync(sql, detail);
        return result > 0;
    }

    public async Task<bool> DeleteDetail(int detailId)
    {
        var db = context.CreateDefaultConnection();
        var result = await db.ExecuteAsync("DELETE FROM PaymentDetail WHERE Id = @Id", new { Id = detailId });
        return result > 0;
    }

    public async Task<PaymentDetail?> GetDetailById(int id)
    {
        var db = context.CreateDefaultConnection();
        return await db.QueryFirstOrDefaultAsync<PaymentDetail>(
            "SELECT Id, PaymentId, Type, Amount, Date, Description, CreatedAt FROM PaymentDetail WHERE Id = @Id",
            new { Id = id });
    }
}
