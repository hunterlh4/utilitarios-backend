using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetTagsByRefId(int refId, TagType type);
    Task<bool> ReplaceTagsForRefId(int refId, TagType type, List<string> tagNames);
}
