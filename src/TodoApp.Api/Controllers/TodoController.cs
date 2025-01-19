using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.DTOs;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces.Repositories;

namespace TodoApp.Api.Controllers;

[ApiController]

[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;

    public TodosController(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    [HttpPost]
    public async Task<ActionResult<TodoDto>> Create(CreateTodoDto createTodoDto)
    {
        var todo = new Todo
        {
            Title = createTodoDto.Title,
            Description = createTodoDto.Description,
            DueDate = createTodoDto.DueDate,
            Priority = createTodoDto.Priority,
            IsCompleted = false
        };

        var created = await _todoRepository.CreateAsync(todo);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToDto(created));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetAll([FromQuery] TodoFilterDto? filter)
    {
        var todos = await _todoRepository.GetAllAsync(filter);
        return Ok(todos.Select(ToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoDto>> GetById(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null) return NotFound();
        
        return Ok(ToDto(todo));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoDto>> Update(int id, UpdateTodoDto updateTodoDto)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null) return NotFound();

        todo.Title = updateTodoDto.Title;
        todo.Description = updateTodoDto.Description;
        todo.DueDate = updateTodoDto.DueDate;
        todo.Priority = updateTodoDto.Priority;

        var updated = await _todoRepository.UpdateAsync(todo);
        return Ok(ToDto(updated));
    }

    [HttpPatch("{id}/complete")]
    public async Task<ActionResult<TodoDto>> MarkAsCompleted(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null) return NotFound();

        todo.IsCompleted = true;
        todo.CompletedAt = DateTime.UtcNow;

        var updated = await _todoRepository.UpdateAsync(todo);
        return Ok(ToDto(updated));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _todoRepository.DeleteAsync(id);
        if (!result) return NotFound();
        
        return NoContent();
    }

    [HttpGet("completed")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetCompleted()
    {
        var todos = await _todoRepository.GetCompletedAsync();
        return Ok(todos.Select(ToDto));
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetPending()
    {
        var todos = await _todoRepository.GetPendingAsync();
        return Ok(todos.Select(ToDto));
    }

    [HttpDelete("completed")]
    public async Task<ActionResult> DeleteCompleted()
    {
        var count = await _todoRepository.DeleteCompletedAsync();
        return Ok(new { DeletedCount = count });
    }

    private static TodoDto ToDto(Todo todo) => new(
        todo.Id,
        todo.Title,
        todo.Description,
        todo.IsCompleted,
        todo.DueDate,
        todo.CreatedAt,
        todo.CompletedAt,
        todo.UpdatedAt,
        todo.Priority
    );
}