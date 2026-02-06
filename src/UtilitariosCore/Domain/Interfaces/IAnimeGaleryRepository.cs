using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IAnimeGaleryRepository
{
    Task<int> CreateAnimeGalery(AnimeGalery item);
    Task<bool> UpdateAnimeGalery(AnimeGalery item);
    Task<bool> DeleteAnimeGalery(int id);
    Task<AnimeGalery?> GetAnimeGaleryById(int id);
    Task<IEnumerable<AnimeGalery>> GetAllAnimeGaleries();
}
