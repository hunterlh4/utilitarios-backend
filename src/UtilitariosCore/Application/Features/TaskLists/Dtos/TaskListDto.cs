using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.TaskLists.Dtos;

public class TaskListDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public TaskListStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateTaskListDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public TaskListStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UpdateTaskListDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public TaskListStatus? Status { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class TaskDto
{
    public string Id { get; set; } = string.Empty;
    public string TaskListId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }
}

public class CreateTaskDto
{
    public string Id { get; set; } = string.Empty;
    public string TaskListId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }
}

public class UpdateTaskDto
{
    public string? Id { get; set; }
    public string? TaskListId { get; set; }
    public string? Title { get; set; }
    public bool? Completed { get; set; }
}
