namespace UtilitariosCore.Domain.Models;

public class HentaiWithTags
{
    public Hentai Hentai { get; set; } = null!;
    public List<string> Tags { get; set; } = [];
}
