using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IJavRepository
{
    Task<int> CreateJav(Jav item);
    Task<bool> UpdateJav(Jav item);
    Task<bool> UpdateJavStatus(int id, ContentStatus status);
    Task<bool> DeleteJav(int id);
    Task<Jav?> GetJavById(int id);
    Task<Jav?> GetJavByCode(string code);
    Task<IEnumerable<Jav>> GetAllJavs();
    Task<IEnumerable<Jav>> GetJavsByActressId(int actressId);
    Task<JavWithDetails?> GetJavWithDetailsById(int id);
    Task<IEnumerable<JavWithDetails>> GetAllJavsWithDetails();
}

public class JavWithDetails
{
    public Jav Jav { get; set; } = null!;
    public ActressJav? Actress { get; set; }
    public List<Link> ActressLinks { get; set; } = new();
    public List<Link> JavLinks { get; set; } = new();
}
