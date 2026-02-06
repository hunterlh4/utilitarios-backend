using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface ILinkRepository
{
    Task<int> CreateLink(Link link);
    Task<bool> UpdateLink(Link link);
    Task<bool> DeleteLink(int id);
    Task<Link?> GetLinkById(int id);
    Task<IEnumerable<Link>> GetLinksByRefId(int refId, LinkType type);
    Task<bool> DeleteLinksByRefId(int refId, LinkType type);
}
