using ApiApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication;

public class TodoDbContext : DbContext
{
    virtual public DbSet<Todo> Todos { get; set; }

    public TodoDbContext(DbContextOptions<TodoDbContext> opts) : base(opts)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
