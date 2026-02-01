using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace BackofficeApi.Shared.Filters;

/// <summary>
/// Atributo de autorización que utiliza el esquema de autenticación JWT.
/// </summary>
public class AuthorizeJwtAttribute : AuthorizeAttribute
{
    public AuthorizeJwtAttribute() : base(JwtBearerDefaults.AuthenticationScheme)
    {
    }
}
