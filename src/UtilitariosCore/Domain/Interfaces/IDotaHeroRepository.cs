using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IDotaHeroRepository
{
    Task<IEnumerable<DotaHero>> GetAll();
    Task<DotaHero?> GetById(int id);
    Task<int> Create(DotaHero hero);
    Task<bool> Update(DotaHero hero);
    Task<bool> Delete(int id);
    Task<bool> Exists(int id);
}
