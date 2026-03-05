using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IProyectRepository
{
    Task<IEnumerable<Proyect>> GetAll();
    Task<Proyect?> GetById(int id);
    Task<int> Create(Proyect proyect);
    Task<bool> Update(Proyect proyect);
    Task<bool> Delete(int id);
    Task<bool> Exists(int id);
}
