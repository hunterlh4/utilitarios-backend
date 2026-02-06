namespace UtilitariosCore.Domain.Models;

public class Seller
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Whatsapp { get; set; }
    public string? Products { get; set; }
    public DateTime CreatedAt { get; set; }
}
