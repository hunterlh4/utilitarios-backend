using UtilitariosCore.Application.Features.Accounts.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IAccountRepository
{
    #region account-email
    Task<IEnumerable<AccountEmailDto>> GetEmails();
    Task<int> CreateEmail(AccountEmail account);
    Task<bool> UpdateEmail(AccountEmail account);
    Task<bool> DeleteEmail(int id);
    #endregion
    
    #region account-steam
    Task<IEnumerable<AccountSteamDto>> GetSteams();
    Task<int> CreateSteam(AccountSteam account);
    Task<bool> UpdateSteam(AccountSteam account);
    Task<bool> DeleteSteam(int id);
    Task<bool> UpdateSteamLastPlay(int id, DateTime lastPlay);
    Task<int> ClearWeeklyLastPlay();
    #endregion
    
    #region account-github
    Task<IEnumerable<AccountGitHubDto>> GetGitHubs();
    Task<int> CreateGitHub(AccountGitHub account);
    Task<bool> UpdateGitHub(AccountGitHub account);
    Task<bool> DeleteGitHub(int id);
    #endregion
    
    #region account-general
    Task<IEnumerable<AccountGeneralDto>> GetGenerals();
    Task<int> CreateGeneral(AccountGeneral account);
    Task<bool> UpdateGeneral(AccountGeneral account);
    Task<bool> DeleteGeneral(int id);
    #endregion
    
    #region account-kiro
    Task<IEnumerable<AccountKiroDto>> GetKiro();
    Task<int> CreateKiro(AccountKiro account);
    Task<bool> UpdateKiro(AccountKiro account);
    Task<bool> UseKiro(int id);
    Task<int> ResetKiro(DateTime threshold);
    #endregion
}
