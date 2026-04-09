using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Accounts.Actions;
using UtilitariosCore.Application.Features.Accounts.Dtos;

namespace UtilitariosApi.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController(ISender sender) : ControllerBase
{
    #region Account-email
    [HttpGet("email")]
    public async Task<ActionResult<IEnumerable<AccountEmailDto>>> GetEmails()
    {
        var response = await sender.Send(new GetAllEmailAccountsQuery());
        return response.ToActionResult();
    }

    [HttpPost("email")]
    public async Task<ActionResult<int>> CreateEmail([FromBody] CreateEmailAccountCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("email/{id:int}")]
    public async Task<ActionResult> UpdateEmail([FromRoute] int id, [FromBody] UpdateEmailAccountCommand command)
    {
        command.Id = id;
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpDelete("email/{id:int}")]
    public async Task<ActionResult> DeleteEmail([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteEmailAccountCommand(id));
        return response.ToActionResult();
    }
    #endregion

    #region Account-steam
    [HttpGet("steam")]
    public async Task<ActionResult<IEnumerable<AccountSteamDto>>> GetSteams()
    {
        var response = await sender.Send(new GetAllSteamAccountsQuery());
        return response.ToActionResult();
    }

    [HttpPost("steam")]
    public async Task<ActionResult<int>> CreateSteam([FromBody] CreateSteamAccountCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("steam/{id:int}")]
    public async Task<ActionResult> UpdateSteam([FromRoute] int id, [FromBody] UpdateSteamAccountCommand command)
    {
        command.Id = id;
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpDelete("steam/{id:int}")]
    public async Task<ActionResult> DeleteSteam([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteSteamAccountCommand(id));
        return response.ToActionResult();
    }
    #endregion

    #region Account-github
    [HttpGet("github")]
    public async Task<ActionResult<IEnumerable<AccountGitHubDto>>> GetGitHubs()
    {
        var response = await sender.Send(new GetAllGitHubAccountsQuery());
        return response.ToActionResult();
    }

    [HttpPost("github")]
    public async Task<ActionResult<int>> CreateGitHub([FromBody] CreateGitHubAccountCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("github/{id:int}")]
    public async Task<ActionResult> UpdateGitHub([FromRoute] int id, [FromBody] UpdateGitHubAccountCommand command)
    {
        command.Id = id;
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpDelete("github/{id:int}")]
    public async Task<ActionResult> DeleteGitHub([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteGitHubAccountCommand(id));
        return response.ToActionResult();
    }
    #endregion

    #region Account-general
    [HttpGet("general")]
    public async Task<ActionResult<IEnumerable<AccountGeneralDto>>> GetGenerals()
    {
        var response = await sender.Send(new GetAllGeneralAccountsQuery());
        return response.ToActionResult();
    }

    [HttpPost("general")]
    public async Task<ActionResult<int>> CreateGeneral([FromBody] CreateGeneralAccountCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("general/{id:int}")]
    public async Task<ActionResult> UpdateGeneral([FromRoute] int id, [FromBody] UpdateGeneralAccountCommand command)
    {
        command.Id = id;
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpDelete("general/{id:int}")]
    public async Task<ActionResult> DeleteGeneral([FromRoute] int id)
    {
        var response = await sender.Send(new DeleteGeneralAccountCommand(id));
        return response.ToActionResult();
    }
    #endregion
    
    #region Account-kiro
    [HttpGet("kiro")]
    public async Task<ActionResult<IEnumerable<AccountKiroDto>>> GetKiro()
    {
        var response = await sender.Send(new GetKiroAccountQuery());
        return response.ToActionResult();
    }

    [HttpPost("kiro")]
    public async Task<ActionResult<int>> CreateKiro([FromBody] CreateKiroAccountCommand command)
    {
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPut("kiro/{id:int}")]
    public async Task<ActionResult> UpdateKiro([FromRoute] int id, [FromBody] UpdateKiroAccountCommand command)
    {
        command.Id = id;
        var response = await sender.Send(command);
        return response.ToActionResult();
    }

    [HttpPatch("kiro/{id:int}/use")]
    public async Task<ActionResult> UseKiro([FromRoute] int id)
    {
        var response = await sender.Send(new UseKiroAccountCommand(id));
        return response.ToActionResult();
    }

    [HttpPost("kiro/reset")]
    public async Task<ActionResult<int>> ResetKiro()
    {
        var response = await sender.Send(new ResetKiroAccountCommand());
        return response.ToActionResult();
    }
    #endregion
}
