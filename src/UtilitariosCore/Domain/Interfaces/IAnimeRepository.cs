using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IAnimeRepository
{
    Task<int> CreateAnime(Anime item);
    Task<bool> UpdateAnime(Anime item);
    Task<bool> DeleteAnime(int id);
    Task<Anime?> GetAnimeById(int id);
    Task<Anime?> GetAnimeByApiId(string apiId);
    Task<IEnumerable<Anime>> GetAllAnimes();
}
