using FluentValidation;
using MediatR;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.Accounts.Actions;

public record CreateAccountPropertyRequest(string Key, string Value);
public record CreateAccountRenewalRequest(int Day, int Month, int Year);

public record CreateAccountCommand(
    AccountType Type,
    string Name,
    string? Username,
    string? Password,
    string? ProfileUrl,
    string? PhoneNumber,
    string? RecoveryEmail,
    DateTime? LastConnection,
    List<CreateAccountPropertyRequest>? Properties,
    List<CreateAccountRenewalRequest>? Renewals
) : IRequest<Result<int>>;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Type).IsInEnum().WithMessage("Type no es un valor válido.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Username).MaximumLength(200).When(x => x.Username is not null);
        RuleFor(x => x.Password).MaximumLength(200).When(x => x.Password is not null);
        RuleFor(x => x.ProfileUrl).MaximumLength(1000).When(x => x.ProfileUrl is not null);
        RuleFor(x => x.PhoneNumber).MaximumLength(20).When(x => x.PhoneNumber is not null);
        RuleFor(x => x.RecoveryEmail).MaximumLength(200).When(x => x.RecoveryEmail is not null);

        RuleForEach(x => x.Properties).ChildRules(p =>
        {
            p.RuleFor(x => x.Key).NotEmpty().MaximumLength(100);
            p.RuleFor(x => x.Value).NotEmpty().MaximumLength(500);
        }).When(x => x.Properties is not null);

        RuleForEach(x => x.Renewals).ChildRules(r =>
        {
            r.RuleFor(x => x.Day).InclusiveBetween(1, 31).WithMessage("Day debe ser entre 1 y 31.");
            r.RuleFor(x => x.Month).InclusiveBetween(1, 12).WithMessage("Month debe ser entre 1 y 12.");
            r.RuleFor(x => x.Year).GreaterThan(2000).WithMessage("Year debe ser mayor a 2000.");
        }).When(x => x.Renewals is not null);
    }
}

internal sealed class CreateAccountCommandHandler(IAccountRepository accountRepository)
    : IRequestHandler<CreateAccountCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = new Account
        {
            Type = request.Type,
            Name = request.Name,
            Username = request.Username,
            Password = request.Password,
            ProfileUrl = request.ProfileUrl,
            PhoneNumber = request.PhoneNumber,
            RecoveryEmail = request.RecoveryEmail,
            LastConnection = request.LastConnection,
            CreatedAt = DateTime.Now
        };

        var properties = (request.Properties ?? [])
            .Select(p => new AccountProperty { Key = p.Key, Value = p.Value })
            .ToList();

        var renewals = (request.Renewals ?? [])
            .Select(r => new AccountRenewal { Day = r.Day, Month = r.Month, Year = r.Year })
            .ToList();

        int id = await accountRepository.Create(account, properties, renewals);
        return id;
    }
}
