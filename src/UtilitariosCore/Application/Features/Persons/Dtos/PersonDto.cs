namespace UtilitariosCore.Application.Features.Persons.Dtos;

public class PersonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreatePersonDto
{
    public string Name { get; set; } = string.Empty;
}

public class UpdatePersonDto
{
    public string? Name { get; set; }
}
