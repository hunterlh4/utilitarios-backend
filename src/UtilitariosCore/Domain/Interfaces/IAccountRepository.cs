using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IAccountRepository
{
    Task<IEnumerable<AccountEmailDto>> GetEmails();
    Task<IEnumerable<AccountSteamDto>> GetSteams();
    Task<IEnumerable<AccountGitHubDto>> GetGitHubs();
    Task<IEnumerable<AccountGeneralDto>> GetGenerals();
    Task<AccountKiroDto?> GetKiro();

    Task<int> CreateEmail(AccountEmail account);
    Task<bool> UpdateEmail(AccountEmail account);
    Task<bool> DeleteEmail(int id);

    Task<int> CreateSteam(AccountSteam account);
    Task<bool> UpdateSteam(AccountSteam account);
    Task<bool> DeleteSteam(int id);

    Task<int> CreateGitHub(AccountGitHub account);
    Task<bool> UpdateGitHub(AccountGitHub account);
    Task<bool> DeleteGitHub(int id);

    Task<int> CreateGeneral(AccountGeneral account);
    Task<bool> UpdateGeneral(AccountGeneral account);
    Task<bool> DeleteGeneral(int id);

    Task<int> CreateKiro(AccountKiro account);
    Task<bool> UpdateKiro(AccountKiro account);
    Task<bool> UseKiro(int id);
    Task<int> ResetKiro(DateTime threshold);
}
