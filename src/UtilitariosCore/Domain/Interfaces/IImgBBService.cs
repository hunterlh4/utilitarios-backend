using UtilitariosCore.Application.Features.Media.Dtos;

namespace UtilitariosCore.Domain.Interfaces;

public interface IImgBBService
{
    Task<ImgBBUploadResponse?> UploadImageAsync(Stream fileStream, string fileName);
    Task DeleteImageAsync(string deleteUrl);
}
