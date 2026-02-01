using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Shared.Requests;
using UtilitariosCore.Shared.Responses;
using MediatR;

namespace UtilitariosCore.Application.Web.Auth.Actions;

public class WebUpdateProfileCommand : IRequest<Result>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    internal sealed class Handler(IUserRepository userRepository, IAuthContext authContext) : IRequestHandler<WebUpdateProfileCommand, Result>
    {
        public async Task<Result> Handle(WebUpdateProfileCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                return Errors.BadRequest();
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                return Errors.BadRequest();
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return Errors.BadRequest();
            }

            int userId = authContext.UserId;

            var userDetail = await userRepository.GetUserDetailById(userId);

            if (userDetail == null)
            {
                var newItem = new UserDetail
                {
                    UserId = userId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                await userRepository.CreateUserDetail(newItem);
            }
            else
            {
                userDetail.FirstName = request.FirstName;
                userDetail.LastName = request.LastName;
                userDetail.Email = request.Email;
                userDetail.PhoneNumber = request.PhoneNumber;
                userDetail.UpdatedAt = DateTimeOffset.UtcNow;

                await userRepository.UpdateUserDetail(userDetail);
            }

            return Results.NoContent();
        }
    }
}