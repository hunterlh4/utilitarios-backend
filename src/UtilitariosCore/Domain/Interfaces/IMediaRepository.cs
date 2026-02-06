using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IMediaRepository
{
    Task<int> CreateMedia(Media item);
    Task<bool> DeleteMedia(int id);
    Task<Media?> GetMediaById(int id);
    Task<IEnumerable<Media>> GetMediaByRefId(int refId, MediaType type);
    Task<bool> UpdateMediaOrder(int id, int newOrderIndex);
}
