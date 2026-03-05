using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IAccountRepository
{
    Task<IEnumerable<AccountDto>> GetAll(AccountType? type = null);
    Task<AccountDto?> GetById(int id);
    Task<int> Create(Account account, List<AccountProperty> properties, List<AccountRenewal> renewals);
    Task<bool> Update(Account account, List<AccountProperty> properties, List<AccountRenewal> renewals);
    Task<bool> Delete(int id);
    Task<bool> Exists(int id);
}
