using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IActressRepository
{
    Task<int> CreateActress(Actress actress);
    Task<bool> UpdateActress(Actress actress);
    Task<Actress?> GetActressById(int id);
    Task<Actress?> GetActressByName(string name);
    Task<IEnumerable<Actress>> GetAllActresses();
    Task<IEnumerable<ActressJavDto>> GetAllActressesWithFirstImage();
}
