using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IActressJavRepository
{
    Task<int> CreateActress(ActressJav actress);
    Task<bool> UpdateActress(ActressJav actress);
    Task<ActressJav?> GetActressById(int id);
    Task<ActressJav?> GetActressByName(string name);
    Task<IEnumerable<ActressJav>> GetAllActresses();
    Task<IEnumerable<ActressJavDto>> GetAllActressesWithFirstImage();
}
