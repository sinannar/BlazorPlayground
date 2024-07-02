using ApiApplication.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace ApiApplication.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly TodoDbContext _todoDbContext;

    public TodoController(ILogger<TodoController> logger, TodoDbContext todoDbContext)
    {
        _logger = logger;
        _todoDbContext = todoDbContext;
    }

    [HttpPut]
    public async Task<bool> CreateTodo([FromBody] CreateTodoRequestDto payload)
    {
        await _todoDbContext.Todos.AddAsync(new Todo{ 
            Title = payload.Title,
            Description = payload.Description,
            CreatedAt = DateTime.UtcNow
        }).ConfigureAwait(false);
        var savedN = await _todoDbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
        _logger.LogInformation("Created Todo", payload);
        return savedN == 1;
    }

    [HttpPost]
    public async Task<bool> UpdateTodo([FromBody] UpdateTodoRequestDto payload)
    {
        var existingTodo = await _todoDbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == payload.Id).ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(existingTodo);
        existingTodo.Title = payload.Title;
        existingTodo.Description = payload.Description;
        _todoDbContext.Todos.Update(existingTodo);
        var savedN = await _todoDbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
        _logger.LogInformation("Updated Todo", payload, existingTodo);
        return savedN == 1;
    }

    [HttpPost]
    public async Task<bool> CompleteTodo(int id)
    {
        var existingTodo = await _todoDbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == id).ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(existingTodo);
        _logger.LogInformation("Complete Todo", existingTodo);
        existingTodo.CompletedAt = DateTimeOffset.Now;
        _todoDbContext.Todos.Update(existingTodo);
        var savedN = await _todoDbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
        _logger.LogInformation("Complete Todo", existingTodo);
        return savedN == 1;
    }


    [HttpGet]
    public async Task<TodoDetailsDto> GetTodo(int id)
    {
        var existingTodo = await _todoDbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == id).ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(existingTodo);
        _logger.LogInformation("Get Todo", existingTodo);
        return new TodoDetailsDto(existingTodo.Id, existingTodo.Title, existingTodo.Description, existingTodo.CreatedAt, existingTodo.CompletedAt);
    }

    [HttpDelete]
    public async Task<bool> DeleteTodo(int id)
    {
        var existingTodo = await _todoDbContext.Todos.FirstOrDefaultAsync(todo => todo.Id == id).ConfigureAwait(false);
        ArgumentNullException.ThrowIfNull(existingTodo);
        _todoDbContext.Todos.Remove(existingTodo);
        var savedN = await _todoDbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
        _logger.LogInformation("Delete Todo", existingTodo);
        return savedN == 1;
    }

    [HttpGet]
    public async Task<List<TodoSummaryDto>> ListTodos()
    {
        var todos = await _todoDbContext.Todos.ToListAsync().ConfigureAwait(false);
        _logger.LogInformation("List Todos", todos);
        return todos.Select(todo => new TodoSummaryDto(todo.Id, todo.Title, todo.CompletedAt)).ToList();
    }
}
