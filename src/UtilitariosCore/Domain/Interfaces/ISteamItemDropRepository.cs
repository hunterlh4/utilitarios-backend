using UtilitariosCore.Application.Features.SteamItemDrops.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface ISteamItemDropRepository
{
    Task<IEnumerable<SteamItemDropDto>> GetAll();
    Task<SteamItemDropDto?> GetById(int id);
    Task<int> Create(SteamItemDrop drop);
    Task<bool> Update(SteamItemDrop drop);
    Task<bool> Delete(int id);
    Task<bool> Exists(int id);
}
