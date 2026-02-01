using BackofficeCore.Domain.Interfaces;
using BackofficeCore.Shared.Requests;
using BackofficeCore.Shared.Responses;
using BackofficeCore.Shared.Utils;
using MediatR;

namespace BackofficeCore.Application.Web.Auth.Actions;

public class WebChangePasswordCommand : IRequest<Result>
{
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmNewPassword { get; set; }

    internal sealed class Handler(IUserRepository userRepository, IAuthContext authContext) : IRequestHandler<WebChangePasswordCommand, Result>
    {
        public async Task<Result> Handle(WebChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var validationError = Validate(request);

            if (validationError != null)
                return validationError;

            int userId = authContext.UserId;

            var user = await userRepository.GetUserById(userId);

            if (user == null)
                return Errors.NotFound("Usuario no encontrado.");

            bool isValid = PasswordUtil.VerifyPassword(request.CurrentPassword!, user.PasswordHash);

            if (!isValid)
                return Errors.BadRequest("La contraseña actual es incorrecta.");

            string newPasswordHash = PasswordUtil.HashPassword(request.NewPassword!);

            user.PasswordHash = newPasswordHash;
            user.UpdatedAt = DateTimeOffset.UtcNow;

            bool updated = await userRepository.UpdatePassword(user);

            if (!updated)
                return Errors.UnprocessableContent("No se pudo actualizar la contraseña.");

            return Results.NoContent();
        }

        private static Result? Validate(WebChangePasswordCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                return Errors.BadRequest("Debe ingresar su contraseña actual.");

            if (string.IsNullOrWhiteSpace(request.NewPassword))
                return Errors.BadRequest("Debe ingresar la nueva contraseña.");

            if (string.IsNullOrWhiteSpace(request.ConfirmNewPassword))
                return Errors.BadRequest("Debe confirmar la nueva contraseña.");

            if (request.NewPassword != request.ConfirmNewPassword)
                return Errors.BadRequest("Las contraseñas no coinciden.");

            if (request.NewPassword!.Length < 6)
                return Errors.BadRequest("La contraseña debe tener mínimo 6 caracteres.");

            return null;
        }
    }
}