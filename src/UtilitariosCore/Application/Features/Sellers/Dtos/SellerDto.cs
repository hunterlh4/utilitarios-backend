namespace UtilitariosCore.Application.Features.Sellers.Dtos;

public class SellerDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Whatsapp { get; set; }
    public string? Products { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSellerDto
{
    public string? Name { get; set; }
    public string? Whatsapp { get; set; }
    public string? Products { get; set; }
}

public class UpdateSellerDto
{
    public string? Name { get; set; }
    public string? Whatsapp { get; set; }
    public string? Products { get; set; }
}
