using BackofficeApi.Shared.Extensions;
using BackofficeApi.Shared.Filters;
using BackofficeCore.Application.Features.Auth.Actions;
using BackofficeCore.Application.Features.Auth.Dtos;
using BackofficeCore.Application.Features.Permissions.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackofficeApi.Controllers;

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
