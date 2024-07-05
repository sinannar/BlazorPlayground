using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Entities;

public class Todo
{
    [Key]
    public int Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
}
