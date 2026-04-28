using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record CreateSteamAccountCommand : IRequest<Result<int>>
{
    public int? EmailId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ProfileUrl { get; set; }
    public string? ImageUrl { get; set; }
    public bool HasDota2 { get; set; }
    public bool HasCS2 { get; set; }
    public bool IsUnlimited { get; set; }
    public bool IsVacBanned { get; set; }
    public bool HasSteamMobile { get; set; }
    public DateTime? LastPurchaseDate { get; set; }

    public sealed class Validator : AbstractValidator<CreateSteamAccountCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Username).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(200);
        }
    }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<CreateSteamAccountCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateSteamAccountCommand r, CancellationToken ct)
        {
            var id = await repo.CreateSteam(new AccountSteam
            {
                EmailId = r.EmailId, 
                Username = r.Username, 
                Password = r.Password,
                Phone = r.Phone, 
                ProfileUrl = r.ProfileUrl,
                ImageUrl = r.ImageUrl,
                HasDota2 = r.HasDota2, 
                HasCS2 = r.HasCS2, 
                IsUnlimited = r.IsUnlimited,
                IsVacBanned = r.IsVacBanned, 
                HasSteamMobile = r.HasSteamMobile,
                LastPurchaseDate = r.LastPurchaseDate,
                CreatedAt = DateTime.Now
            });
            return Results.Created(id);
        }
    }
}
