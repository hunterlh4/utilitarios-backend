using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.Series.Dtos;

public class SeriesDto
{
    public int Id { get; set; }
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int? Year { get; set; }
    public decimal? Rating { get; set; }
    public string? Type { get; set; }
    public ContentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSeriesDto
{
    public int Id { get; set; }
}

public class UpdateSeriesDto
{
    public string ImdbId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int? Year { get; set; }
    public decimal? Rating { get; set; }
    public string? Type { get; set; }
}
