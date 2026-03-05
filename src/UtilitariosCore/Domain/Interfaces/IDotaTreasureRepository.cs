using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IDotaTreasureRepository
{
    Task<IEnumerable<DotaTreasure>> GetAll();
    Task<DotaTreasure?> GetById(int id);
    Task<int> Create(DotaTreasure treasure);
    Task<bool> Update(DotaTreasure treasure);
    Task<bool> Delete(int id);
    Task<bool> Exists(int id);
}
