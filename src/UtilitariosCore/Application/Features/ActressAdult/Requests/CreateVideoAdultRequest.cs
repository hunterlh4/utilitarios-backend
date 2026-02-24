namespace UtilitariosCore.Application.Features.ActressAdults.Requests;

public record CreateVideoAdultRequest
{
    public string Source { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public List<int> ActressIds { get; set; } = [];
}
