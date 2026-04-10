using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IActressAdultRepository
{
    Task<int> CreateActressAdult(ActressAdult actress);
    Task<bool> UpdateActressAdult(ActressAdult actress);
    Task<bool> UpdateActressAdultImage(int id, string imageUrl);
    Task<IEnumerable<ActressAdult>> GetAllActressAdults();
    Task<ActressAdult?> GetActressAdultById(int id);
    Task<ActressAdultDto?> GetActressAdultWithTagsAndImageById(int id);
    Task<ActressAdult?> GetActressAdultByName(string name);
    Task<bool> CheckActressNameExists(string name);
    Task<IEnumerable<ActressAdultDto>> GetAllActressAdultsWithFirstImage();
}
