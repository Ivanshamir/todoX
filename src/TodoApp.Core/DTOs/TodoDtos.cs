using TodoApp.Core.Enums;

namespace TodoApp.Core.DTOs;

public record CreateTodoDto(
    string Title,
    string? Description,
    DateTime DueDate,
    Priority Priority
);

public record UpdateTodoDto(
    string Title,
    string? Description,
    DateTime DueDate,
    Priority Priority
);

public record TodoDto(
    int Id,
    string Title,
    string? Description,
    bool IsCompleted,
    DateTime DueDate,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    DateTime? UpdatedAt,
    Priority Priority
);

public record TodoFilterDto(
    bool? IsCompleted = null,
    Priority? Priority = null,
    DateTime? DueBefore = null,
    DateTime? DueAfter = null
);