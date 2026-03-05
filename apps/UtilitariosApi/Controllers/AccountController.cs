using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Accounts.Actions;
using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Enums;

namespace UtilitariosApi.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll([FromQuery] AccountType? type = null)
    {
        var response = await sender.Send(new GetAllAccountsQuery(type));
        return response.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountDto>> GetById([FromRoute] int id)
    {
        var response = await sender.Send(new GetAccountByIdQuery(id));
        return response.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateAccountCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateAccountCommand command)
    {
        var response = await sender.Send(command with { Id = id });
        return response.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteAccountCommand(id));
        return response.ToActionResult();
    }
}
