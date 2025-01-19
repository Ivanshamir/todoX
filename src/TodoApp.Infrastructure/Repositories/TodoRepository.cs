using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;
using TodoApp.Core.DTOs;
using TodoApp.Core.Interfaces.Repositories;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly ApplicationDbContext _context;

    public TodoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Todo> CreateAsync(Todo todo)
    {
        todo.CreatedAt = DateTime.UtcNow;
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public async Task<Todo?> GetByIdAsync(int id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public async Task<IEnumerable<Todo>> GetAllAsync(TodoFilterDto? filter = null)
    {
        var query = _context.Todos.AsQueryable();

        if (filter != null)
        {
            if (filter.IsCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == filter.IsCompleted.Value);

            if (filter.Priority.HasValue)
                query = query.Where(t => t.Priority == filter.Priority.Value);

            if (filter.DueBefore.HasValue)
                query = query.Where(t => t.DueDate <= filter.DueBefore.Value);

            if (filter.DueAfter.HasValue)
                query = query.Where(t => t.DueDate >= filter.DueAfter.Value);
        }

        return await query.OrderByDescending(t => t.Priority)
                         .ThenBy(t => t.DueDate)
                         .ToListAsync();
    }

    public async Task<Todo> UpdateAsync(Todo todo)
    {
        todo.UpdatedAt = DateTime.UtcNow;
        _context.Todos.Update(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null) return false;

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Todo>> GetCompletedAsync()
    {
        return await _context.Todos
            .Where(t => t.IsCompleted)
            .OrderByDescending(t => t.CompletedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Todo>> GetPendingAsync()
    {
        return await _context.Todos
            .Where(t => !t.IsCompleted)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<int> DeleteCompletedAsync()
    {
        var completed = await _context.Todos
            .Where(t => t.IsCompleted)
            .ToListAsync();

        _context.Todos.RemoveRange(completed);
        return await _context.SaveChangesAsync();
    }
}