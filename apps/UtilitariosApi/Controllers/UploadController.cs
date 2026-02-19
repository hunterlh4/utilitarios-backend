using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UtilitariosCore.Domain.Enums;
using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Domain.Models;
using UtilitariosCore.Infrastructure.Services.ImageUpload;

namespace UtilitariosApi.Controllers;

[Route("api/upload")]
[ApiController]
public class UploadController(IImgBBService imgBBService, IMediaRepository mediaRepository) : ControllerBase
{
    [HttpPost("image")]
    public async Task<ActionResult> UploadImage([FromForm] IFormFile image, [FromForm] int type, [FromForm] int refId)
    {
        try
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest(new { error = "Image is required" });
            }

            if (type < 1 || type > 5)
            {
                return BadRequest(new { error = "Invalid type. Must be 1 (GirlGalery), 2 (AnimeGalery), or 3 (Project), or 4 (actress), or 5 (actressadult)" });
            }

            if (refId <= 0)
            {
                return BadRequest(new { error = "RefId is required and must be greater than 0" });
            }

            // Subir imagen a ImgBB
            using var stream = image.OpenReadStream();
            var uploadResult = await imgBBService.UploadImageAsync(stream, image.FileName);

            if (uploadResult == null)
            {
                return StatusCode(500, new { error = "Failed to upload image" });
            }

            // Obtener el siguiente orderIndex
            var existingMedia = await mediaRepository.GetMediaByRefId(refId, (MediaType)type);
            var nextOrderIndex = existingMedia.Any() ? existingMedia.Max(m => m.OrderIndex) + 1 : 1;

            // Guardar en la base de datos
            var media = new Media
            {
                Type = (MediaType)type,
                RefId = refId,
                Url = uploadResult.Url,
                Thumbnail = uploadResult.Thumbnail,
                DeleteUrl = uploadResult.DeleteUrl,
                OrderIndex = nextOrderIndex,
                CreatedAt = DateTime.UtcNow
            };

            var mediaId = await mediaRepository.CreateMedia(media);

            return Ok(new
            {
                success = true,
                id = mediaId,
                url = uploadResult.Url,
                thumbnail = uploadResult.Thumbnail,
                deleteUrl = uploadResult.DeleteUrl,
                orderIndex = nextOrderIndex
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpDelete("media/{id:int}")]
    public async Task<ActionResult> DeleteMedia([FromRoute] int id)
    {
        try
        {
            var media = await mediaRepository.GetMediaById(id);

            if (media == null)
            {
                return NotFound(new { error = "Media not found" });
            }

            // Eliminar de la base de datos
            var deleted = await mediaRepository.DeleteMedia(id);

            if (!deleted)
            {
                return StatusCode(500, new { error = "Failed to delete media from database" });
            }

            return Ok(new { success = true, message = "Media deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
