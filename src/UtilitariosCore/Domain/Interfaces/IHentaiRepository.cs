using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IHentaiRepository
{
    Task<int> CreateHentai(Hentai item);
    Task<bool> UpdateHentai(Hentai item);
    Task<bool> DeleteHentai(int id);
    Task<Hentai?> GetHentaiById(int id);
    Task<Hentai?> GetHentaiByApiId(string apiId);
    Task<IEnumerable<Hentai>> GetAllHentais();
    Task<HentaiWithTags?> GetHentaiWithTagsById(int id);
    Task<IEnumerable<HentaiWithTags>> GetAllHentaisWithTags();
}
