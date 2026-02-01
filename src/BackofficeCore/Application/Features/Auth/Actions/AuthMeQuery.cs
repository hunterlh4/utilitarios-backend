using BackofficeCore.Application.Features.Auth.Dtos;
using BackofficeCore.Domain.Interfaces;
using BackofficeCore.Shared.Requests;
using BackofficeCore.Shared.Responses;
using MediatR;

namespace BackofficeCore.Application.Features.Auth.Actions;

public class AuthMeQuery : IRequest<Result<AuthMeDto>>
{
    internal sealed class Handler(
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        IAuthContext authContext) : IRequestHandler<AuthMeQuery, Result<AuthMeDto>>
    {
        public async Task<Result<AuthMeDto>> Handle(AuthMeQuery request, CancellationToken cancellationToken)
        {
            var userId = authContext.UserId;

            var user = await userRepository.GetUserById(userId);

            if (user == null)
            {
                return Errors.NotFound();
            }

            var permissions = await permissionRepository.GetPermissionsByUserId(userId);

            List<string> permissionsList = permissions.Select(x => x.ActionName).ToList();

            var response = new AuthMeDto
            {
                Id = user.Id,
                Username = user.Username,
                Permissions = permissionsList
            };

            return response;
        }
    }
}