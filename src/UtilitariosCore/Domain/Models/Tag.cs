using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Tag
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public TagType Type { get; set; }
}

public class TagRelation
{
    public int TagId { get; set; }
    public int RefId { get; set; }
    public TagType Type { get; set; }
}
