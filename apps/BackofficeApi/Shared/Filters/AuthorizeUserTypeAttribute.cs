using BackofficeCore.Domain.Enums;
using BackofficeCore.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace BackofficeApi.Shared.Filters;

/// <summary>
/// Atributo de autorización que valida el UserType del usuario autenticado.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AuthorizeUserTypeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly UserType[] _allowedUserTypes;

    public AuthorizeUserTypeAttribute(params UserType[] allowedUserTypes)
    {
        _allowedUserTypes = allowedUserTypes;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Obtener el servicio de UserRepository
        var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
        
        if (userRepository == null)
        {
            context.Result = new StatusCodeResult(500);
            return;
        }

        // Obtener el UserId del claim JWT (ClaimTypes.NameIdentifier)
        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Usuario no autenticado." });
            return;
        }

        // Obtener el usuario de la base de datos
        var user = await userRepository.GetUserById(userId);
        if (user == null)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Usuario no encontrado." });
            return;
        }

        // Validar si el UserType está permitido
        if (!_allowedUserTypes.Contains(user.UserType))
        {
            context.Result = new ObjectResult(new { message = "Usuario no autorizado para esta operación." })
            {
                StatusCode = 403
            };
            return;
        }
    }
}
