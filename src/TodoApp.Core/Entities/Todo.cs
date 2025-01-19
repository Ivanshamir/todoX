using TodoApp.Core.Enums;

namespace TodoApp.Core.Entities;

public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Priority Priority { get; set; }
}