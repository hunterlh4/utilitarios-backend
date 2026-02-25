using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetTagsByRefId(int refId, TagType type);
    Task<IEnumerable<Tag>> GetAllTagsByType(TagType type);
    Task<bool> ReplaceTagsForRefId(int refId, TagType type, List<int> tagIds);
    Task<int> CreateTag(Tag tag);
    Task<bool> UpdateTag(Tag tag);
    Task<bool> DeleteTag(int id);
    Task<bool> IsTagInUse(int id);
    Task<Tag?> GetTagById(int id);
}
