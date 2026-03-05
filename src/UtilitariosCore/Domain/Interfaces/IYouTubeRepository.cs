using UtilitariosCore.Application.Features.YouTubes.Dtos;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IYouTubeRepository
{
    Task<IEnumerable<YouTubeDto>> GetAll(YouTubeCategory? category = null);
    Task<int> Create(YouTube item);
    Task<bool> Delete(int id);
    Task<bool> ExistsByUrl(string url);
}
