using UtilitariosApi.Shared.Extensions;
using UtilitariosApi.Shared.Filters;
using UtilitariosCore.Application.Features.Auth.Actions;
using UtilitariosCore.Application.Features.Auth.Dtos;
using UtilitariosCore.Application.Features.Permissions.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UtilitariosApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginDto>> Login([FromBody] LoginCommand command)
    {
        var response = await sender.Send(command);

        return response.ToActionResult();
    }

    [HttpGet("me")]
    [AuthorizeJwt]
    public async Task<ActionResult<AuthMeDto>> Me()
    {
        var response = await sender.Send(new AuthMeQuery());

        return response.ToActionResult();
    }

    [HttpPost("mapping-permissions")]
    [AuthorizeJwt]
    public async Task<ActionResult<IEnumerable<MappingPermissionDto>>> MappingPermissions([FromBody] MappingPermissionsCommand command)
    {
        var response = await sender.Send(command);

        return response.ToActionResult();
    }
}
