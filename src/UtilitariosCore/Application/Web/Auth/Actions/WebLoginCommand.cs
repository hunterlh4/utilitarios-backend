using UtilitariosCore.Application.Web.Auth.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Responses;
using UtilitariosCore.Shared.Utils;
using MediatR;

namespace UtilitariosCore.Application.Web.Auth.Actions;

public class WebLoginCommand : IRequest<Result<WebLoginDto>>
{
    public string? Username { get; init; }
    public string? Password { get; init; }

    internal sealed class Handler(IUserRepository userRepository, IJwtUtil jwtUtil) : IRequestHandler<WebLoginCommand, Result<WebLoginDto>>
    {
        public async Task<Result<WebLoginDto>> Handle(WebLoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return Errors.BadRequest("El nombre de usuario es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return Errors.BadRequest("La contraseña es obligatoria.");
            }

            var user = await userRepository.GetUserByUsername(request.Username);

            if (user == null)
            {
                return Errors.Unauthorized("Usuario o contraseña incorrectos.");
            }

            bool isPasswordValid = PasswordUtil.VerifyPassword(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return Errors.Unauthorized("Usuario o contraseña incorrectos.");
            }

            int expiresIn = jwtUtil.GetExpiresIn();

            string token = jwtUtil.GenerateToken(user.Id.ToString());

            var response = new WebLoginDto
            {
                TokenType = "Bearer",
                ExpiresIn = expiresIn,
                AccessToken = token
            };

            return response;
        }
    }
}