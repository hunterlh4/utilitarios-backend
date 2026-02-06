using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class TaskList
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public TaskListStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Task
{
    public required string Id { get; set; }
    public required string TaskListId { get; set; }
    public required string Title { get; set; }
    public bool Completed { get; set; }
}
