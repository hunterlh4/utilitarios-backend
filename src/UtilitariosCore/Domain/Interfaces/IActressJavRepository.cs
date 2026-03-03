using UtilitariosCore.Application.Features.Actresses.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IActressJavRepository
{
    Task<int> CreateActressJav(ActressJav actress);
    Task<bool> UpdateActressJav(ActressJav actress);
    Task<ActressJav?> GetActressJavById(int id);
    Task<ActressJavWithTagsDto?> GetActressJavWithTagsById(int id);
    Task<ActressJav?> GetActressJavByName(string name);
    Task<IEnumerable<ActressJav>> GetAllActressJav();
    Task<IEnumerable<ActressJavDto>> GetAllActressJavWithFirstImage();
}
