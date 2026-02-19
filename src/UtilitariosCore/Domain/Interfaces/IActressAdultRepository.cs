using UtilitariosCore.Application.Features.ActressAdults.Dtos;

namespace UtilitariosCore.Domain.Interfaces;

public interface IActressAdultRepository
{
    Task<int> CreateActressAdult(Models.ActressAdult actress);
    Task<bool> UpdateActressAdult(Models.ActressAdult actress);
    Task<Models.ActressAdult?> GetActressAdultById(int id);
    Task<Models.ActressAdult?> GetActressAdultByName(string name);
    Task<IEnumerable<ActressAdultDto>> GetAllActressAdultsWithFirstImage();
}
