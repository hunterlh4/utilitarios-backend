using BackofficeCore.Application.Web.Auth.Dtos;
using BackofficeCore.Domain.Interfaces;
using BackofficeCore.Shared.Requests;
using BackofficeCore.Shared.Responses;
using MediatR;

namespace BackofficeCore.Application.Web.Auth.Actions;

public class WebGetProfileQuery : IRequest<Result<WebProfileDto>>
{
    internal sealed class Handler(IUserRepository userRepository, IAuthContext authContext) : IRequestHandler<WebGetProfileQuery, Result<WebProfileDto>>
    {
        public async Task<Result<WebProfileDto>> Handle(WebGetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = authContext.UserId;

            var user = await userRepository.GetUserById(userId);

            if (user == null)
            {
                return Errors.NotFound();
            }

            var detail = await userRepository.GetUserDetailById(user.Id);

            return new WebProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = detail?.FirstName,
                LastName = detail?.LastName,
                Email = detail?.Email,
                PhoneNumber = detail?.PhoneNumber
            };
        }
    }
}