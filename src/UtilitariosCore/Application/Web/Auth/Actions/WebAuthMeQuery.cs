using UtilitariosCore.Application.Web.Auth.Dtos;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Shared.Requests;
using UtilitariosCore.Shared.Responses;
using MediatR;

namespace UtilitariosCore.Application.Web.Auth.Actions;

public class WebAuthMeQuery : IRequest<Result<WebAuthMeDto>>
{
    internal sealed class Handler(IUserRepository userRepository, IAuthContext authContext) : IRequestHandler<WebAuthMeQuery, Result<WebAuthMeDto>>
    {
        public async Task<Result<WebAuthMeDto>> Handle(WebAuthMeQuery request, CancellationToken cancellationToken)
        {
            var userId = authContext.UserId;

            var user = await userRepository.GetUserById(userId);

            if (user == null)
            {
                return Errors.NotFound();
            }

            var userDetail = await userRepository.GetUserDetailById(user.Id);

            return new WebAuthMeDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = userDetail?.FirstName
            };
        }
    }
}