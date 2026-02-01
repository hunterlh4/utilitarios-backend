using BackofficeCore.Application.Web.Auth.Dtos;
using BackofficeCore.Domain.Interfaces;
using BackofficeCore.Shared.Requests;
using BackofficeCore.Shared.Responses;
using MediatR;

namespace BackofficeCore.Application.Web.Auth.Actions;

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