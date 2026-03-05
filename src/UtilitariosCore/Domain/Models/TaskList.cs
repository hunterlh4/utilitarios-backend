using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Domain.Models;

public class Task
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public TaskListStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class TaskDetail
{
    public required string Id { get; set; }
    public required string TaskId { get; set; }
    public required string Title { get; set; }
    public TaskDetailStatus Status { get; set; }
    public DateTime? Date { get; set; }
}
