namespace BackofficeCore.Application.Web.Auth.Dtos;

public record WebLoginDto
{
    public required string TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public required string AccessToken { get; set; }
}