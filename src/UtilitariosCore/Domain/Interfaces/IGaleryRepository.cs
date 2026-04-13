using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IGaleryRepository
{
    #region anime-galery
    Task<int> CreateAnimeGalery(AnimeGalery item);
    Task<bool> UpdateAnimeGalery(AnimeGalery item);
    Task<bool> UpdateAnimeGaleryImage(int id, string imageUrl);
    Task<bool> DeleteAnimeGalery(int id);
    Task<AnimeGalery> GetAnimeGaleryById(int id);
    Task<AnimeGalery> GetAnimeGaleryByName(string name);
    Task<IEnumerable<AnimeGalery>> GetAllAnimeGaleries();
    #endregion
    
    #region girls-galery
    Task<int> CreateGirlGalery(GirlGalery item);
    Task<bool> UpdateGirlGalery(GirlGalery item);
    Task<bool> UpdateGirlGaleryImage(int id, string imageUrl);
    Task<bool> DeleteGirlGalery(int id);
    Task<GirlGalery> GetGirlGaleryById(int id);
    Task<GirlGalery> GetGirlGaleryByName(string name);
    Task<IEnumerable<GirlGalery>> GetAllGirlGaleries();
    #endregion
}
