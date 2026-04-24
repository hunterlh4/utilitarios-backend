using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IComicRepository
{
    Task<int> CreateComic(Comic item);
    Task<bool> UpdateComic(Comic item);
    Task<bool> DeleteComic(int id);
    Task<Comic> GetComicById(int id);
    Task<IEnumerable<Comic>> GetAllComics();
}
