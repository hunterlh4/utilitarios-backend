using BackofficeCore.Domain.Models;

namespace BackofficeCore.Domain.Interfaces;

public interface IUserDetailRepository
{
    Task<UserDetail?> GetUserDetailByUserId(int userId);
}
