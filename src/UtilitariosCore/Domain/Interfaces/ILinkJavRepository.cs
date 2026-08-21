using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface ILinkJavRepository
{
    Task<int> CreateLinkJav(LinkJav linkJav);
    Task<List<LinkJav>> GetLinkJavsByJavId(int javId);
    Task<bool> DeleteLinkJavsByJavId(int javId);
    Task<LinkJav?> GetLinkJavById(int id);
    Task<bool> UpdateLinkJav(LinkJav linkJav);
    Task<bool> DeleteLinkJav(int id);
}