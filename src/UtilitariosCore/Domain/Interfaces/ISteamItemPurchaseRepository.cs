using UtilitariosCore.Application.Features.SteamItemPurchases.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface ISteamItemPurchaseRepository
{
    Task<IEnumerable<SteamItemPurchaseDto>> GetAll();
    Task<SteamItemPurchaseDto?> GetById(int id);
    Task<int> Create(SteamItemPurchase purchase);
    Task<bool> Update(SteamItemPurchase purchase);
    Task<bool> Delete(int id);
    Task<bool> Exists(int id);
}
