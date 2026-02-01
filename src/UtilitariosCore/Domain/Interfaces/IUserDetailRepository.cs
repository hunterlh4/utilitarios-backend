using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IUserDetailRepository
{
    Task<UserDetail?> GetUserDetailByUserId(int userId);
}
