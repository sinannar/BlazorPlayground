namespace Shared;

public record CreateTodoRequestDto(string Title, string Description);
public record UpdateTodoRequestDto(int Id, string Title, string Description);
public record TodoSummaryDto(int Id, string Title, DateTimeOffset? CompletedAt);
public record TodoDetailsDto(int Id, string Title, string Description, DateTimeOffset CreatedAt, DateTimeOffset? CompletedAt);
