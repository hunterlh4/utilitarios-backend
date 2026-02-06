namespace UtilitariosCore.Domain.Models;

public class Person
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}
