using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface ISeriesRepository
{
    Task<int> CreateSeries(Series item);
    Task<bool> UpdateSeries(Series item);
    Task<bool> DeleteSeries(int id);
    Task<Series?> GetSeriesById(int id);
    Task<Series?> GetSeriesByImdbId(string imdbId);
    Task<IEnumerable<Series>> GetAllSeries();
}
