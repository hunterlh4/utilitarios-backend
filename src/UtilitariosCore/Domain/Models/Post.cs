using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public PostCategory Category { get; set; }
    public string? Subcategory { get; set; }
    public required string Slug { get; set; }
    public DateOnly Date { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PostContent
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public PostContentType Type { get; set; }
    public string? Text { get; set; }
    public string? Language { get; set; }
    public string? Url { get; set; }
    public string? Alt { get; set; }
    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PostContentItem
{
    public int Id { get; set; }
    public int PostContentId { get; set; }
    public required string Text { get; set; }
    public int OrderIndex { get; set; }
}
