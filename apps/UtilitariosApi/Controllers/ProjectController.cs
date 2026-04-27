using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilitariosApi.Shared.Extensions;
using UtilitariosCore.Application.Features.Projects.Actions;
using UtilitariosCore.Application.Features.Projects.Dtos;

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
}
