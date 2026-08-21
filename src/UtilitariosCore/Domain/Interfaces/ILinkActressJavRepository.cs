using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface ILinkActressJavRepository
{
    Task<int> CreateLinkActressJav(LinkActressJav linkActressJav);
    Task<List<LinkActressJav>> GetLinkActressJavsByActressId(int actressId);
    Task<bool> DeleteLinkActressJavsByActressId(int actressId);
    Task<LinkActressJav?> GetLinkActressJavById(int id);
    Task<bool> UpdateLinkActressJav(LinkActressJav linkActressJav);
    Task<bool> DeleteLinkActressJav(int id);
}