using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Media.Actions;
using UtilitariosCore.Application.Features.Media.Dtos;
using UtilitariosCore.Application.Features.Projects.Actions;
using UtilitariosCore.Application.Features.Projects.Dtos;
using UtilitariosCore.Domain.Enums;

namespace UtilitariosApi.Controllers;

[ApiController]
[Route("api/project")]
public class ProjectController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
    {
        var result = await sender.Send(new GetAllProjectsQuery());
        return result.ToActionResult();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDetailDto>> GetById(int id)
    {
        var result = await sender.Send(new GetProjectByIdQuery(id));
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateProjectCommand command)
    {
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateProjectCommand command)
    {
        command.Id = id;
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id:int}/links")]
    public async Task<ActionResult> UpdateLinks(int id, [FromBody] UpdateProjectLinksCommand command)
    {
        command.ProjectId = id;
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await sender.Send(new DeleteProjectCommand(id));
        return result.ToActionResult();
    }

    [HttpPost("{id:int}/images")]
    public async Task<ActionResult<IEnumerable<UploadImageDto>>> UploadImages(
        [FromRoute] int id,
        [FromForm] List<IFormFile> images)
    {
        if (images is null || images.Count == 0)
            return BadRequest("Se requiere al menos una imagen.");

        var results = new List<UploadImageDto>();
        foreach (var image in images)
        {
            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            var response = await sender.Send(new UploadImageCommand(ms.ToArray(), image.FileName, MediaType.Project, id));
            if (response.IsSuccess && response.Value is not null)
                results.Add(response.Value);
        }

        return Ok(results);
    }

    [HttpPost("{id:int}/images/url")]
    public async Task<ActionResult<int>> AddImageUrl([FromRoute] int id, [FromBody] AddProjectImageUrlCommand command)
    {
        command.ProjectId = id;
        var result = await sender.Send(command);
        return result.ToActionResult();
    }

    [HttpDelete("media/{mediaId:int}")]
    public async Task<ActionResult> DeleteMedia([FromRoute] int mediaId)
    {
        var response = await sender.Send(new DeleteMediaCommand(mediaId));
        return response.ToActionResult();
    }
}
