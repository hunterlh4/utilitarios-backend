using FluentValidation;
using MediatR;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record CreateGeneralAccountCommand : IRequest<Result<int>>
{
    public GeneralPlatform Platform { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int? EmailId { get; set; }
    public string? ProfileUrl { get; set; }

    public sealed class Validator : AbstractValidator<CreateGeneralAccountCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Platform).IsInEnum();
            RuleFor(x => x.Username).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(200);
        }
    }

    internal sealed class Handler(IAccountRepository repo) : IRequestHandler<CreateGeneralAccountCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateGeneralAccountCommand r, CancellationToken ct)
        {
            var id = await repo.CreateGeneral(new AccountGeneral
            {
                Platform = r.Platform, Username = r.Username, Password = r.Password,
                EmailId = r.EmailId, ProfileUrl = r.ProfileUrl, CreatedAt = DateTime.Now
            });
            return Results.Created(id);
        }
    }
}
