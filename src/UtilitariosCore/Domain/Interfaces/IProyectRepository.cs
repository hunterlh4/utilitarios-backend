using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAll();
    Task<Project?> GetById(int id);
    Task<int> Create(Project project);
    Task<bool> Update(Project project);
    Task<bool> Delete(int id);
    Task<bool> Exists(int id);
}
