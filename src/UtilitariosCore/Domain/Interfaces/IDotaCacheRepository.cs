using UtilitariosCore.Application.Features.DotaCaches.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IDotaCacheRepository
{
    Task<IEnumerable<DotaCacheDto>> GetAll();
    Task<IEnumerable<DotaCacheDto>> GetByTreasureId(int treasureId);
    Task<DotaCacheDto?> GetById(int id);
    Task<int> Create(DotaCache cache);
    Task<bool> Update(DotaCache cache);
    Task<bool> Delete(int id);
    Task<bool> Exists(int id);
}
