using BackofficeCore.Application.Features.Auth.Dtos;
using BackofficeCore.Domain.Interfaces;
using BackofficeCore.Shared.Responses;
using BackofficeCore.Shared.Utils;
using MediatR;

namespace BackofficeCore.Application.Features.Auth.Actions;

public record LoginCommand : IRequest<Result<LoginDto>>
{
    public required string Username { get; init; }
    public required string Password { get; init; }

    internal sealed class Handler(IUserRepository userRepository, IJwtUtil jwtUtil) : IRequestHandler<LoginCommand, Result<LoginDto>>
    {
        public async Task<Result<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByUsername(request.Username);

            if (user == null)
            {
                return Errors.NotFound();
            }

            bool compare = PasswordUtil.VerifyPassword(request.Password, user.PasswordHash);

            if (!compare)
            {
                return Errors.Unauthorized();
            }

            int expiresIn = jwtUtil.GetExpiresIn();
            
            var payload = new Dictionary<string, object>
            {
                ["sub"] = user.Id.ToString(),
                ["user"] = user.Username
            };

            string token = jwtUtil.GenerateToken(user.Id.ToString());

            var response = new LoginDto
            {
                TokenType = "Bearer",
                ExpiresIn = expiresIn,
                AccessToken = token
            };

            return response;
        }
    }
}