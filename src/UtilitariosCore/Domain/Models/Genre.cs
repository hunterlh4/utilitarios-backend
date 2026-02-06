namespace UtilitariosCore.Domain.Models;

public class Genre
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class HentaiGenre
{
    public int HentaiId { get; set; }
    public int GenreId { get; set; }
}
