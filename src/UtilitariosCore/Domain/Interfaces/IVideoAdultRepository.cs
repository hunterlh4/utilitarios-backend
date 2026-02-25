using UtilitariosCore.Application.Features.ActressAdults.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IVideoAdultRepository
{
    Task<int> CreateVideoAdult(VideoAdult videoAdult);
    Task<bool> UpdateVideoAdult(VideoAdult videoAdult);
    Task<VideoAdult?> GetVideoAdultById(int id);
    Task<IEnumerable<VideoAdult>> GetAllVideoAdults();
    Task<bool> AddActressToVideo(int videoAdultId, int actressId);
    Task<bool> RemoveActressFromVideo(int videoAdultId, int actressId);
    Task<IEnumerable<int>> GetActressIdsByVideoId(int videoAdultId);
    Task<IEnumerable<VideoAdult>> GetVideoAdultsByActressId(int actressId);
    Task<IEnumerable<ActressJav>> GetActressesByVideoId(int videoAdultId);
    Task<IEnumerable<VideoAdultGrouped>> GetVideoAdultsWithActressesByActressId(int actressId);
}
