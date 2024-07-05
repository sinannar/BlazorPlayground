
using ApiApplication.Persistance;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var sqlConnectionString = builder.Configuration.GetConnectionString("tododatabase");
        builder.AddServiceDefaults();
        builder.Services.AddDbContext<TodoDbContext>(options => {
            options.UseSqlServer(sqlConnectionString, sqlOptions => {
                sqlOptions.EnableRetryOnFailure();
                sqlOptions.ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c));
            });
        });
        builder.Services.AddProblemDetails();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddHealthChecks()
            .AddSqlServer(sqlConnectionString!);

        var app = builder.Build();

        using var scope = app.Services.CreateScope();
        var dbcontext = scope.ServiceProvider.GetService<TodoDbContext>()!;
        dbcontext.Database.Migrate();

        app.MapDefaultEndpoints();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapDefaultEndpoints();
        app.MapControllers();
        app.Run();
    }
}
