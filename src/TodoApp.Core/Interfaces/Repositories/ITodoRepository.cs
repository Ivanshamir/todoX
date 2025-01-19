using TodoApp.Core.DTOs;
using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces.Repositories;

public interface ITodoRepository
{
    Task<Todo> CreateAsync(Todo todo);
    Task<Todo?> GetByIdAsync(int id);
    Task<IEnumerable<Todo>> GetAllAsync(TodoFilterDto? filter = null);
    Task<Todo> UpdateAsync(Todo todo);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Todo>> GetCompletedAsync();
    Task<IEnumerable<Todo>> GetPendingAsync();
    Task<int> DeleteCompletedAsync();
}