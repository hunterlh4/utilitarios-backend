using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface ISteamItemRepository
{
    Task<IEnumerable<SteamItem>> GetAll();
    Task<SteamItem?> GetById(int id);
    Task<int> Create(SteamItem item);
    Task<bool> Update(SteamItem item);
    Task<bool> Delete(int id);
    Task<bool> Exists(int id);
}
