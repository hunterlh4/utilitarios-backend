namespace UtilitariosCore.Application.Features.Shared.Dtos;

public class ReorderMediaRequest
{
    public List<MediaOrderItem> Items { get; set; } = new();
}

public class MediaOrderItem
{
    public int MediaId { get; set; }
    public int OrderIndex { get; set; }
}
