using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Posts.Dtos;

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public PostCategory Category { get; set; }
    public string? Subcategory { get; set; }
    public string Slug { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public PostCategory Category { get; set; }
    public string? Subcategory { get; set; }
    public string Slug { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
}

public class UpdatePostDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public PostCategory? Category { get; set; }
    public string? Subcategory { get; set; }
    public string? Slug { get; set; }
    public DateOnly? Date { get; set; }
}
