using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared;
public class TodoApiClient(HttpClient client)
{
    public async Task CreateTodo(CreateTodoRequestDto paylad)
    {
        var json = JsonConvert.SerializeObject(paylad);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await client
            .PutAsync($"api/Todo/CreateTodo", content)
            .ConfigureAwait(false);
    }

    public async Task UpdateTodo(UpdateTodoRequestDto paylad)
    {
        var json = JsonConvert.SerializeObject(paylad);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        await client
            .PostAsync($"api/Todo/UpdateTodo", content)
            .ConfigureAwait(false);
    }

    public async Task CompleteTodo(int id)
    {
        await client
            .PostAsync($"api/Todo/CompleteTodo?id={id}", null)
            .ConfigureAwait(false);
    }


    public async Task<TodoDetailsDto> GetTodo(int id)
    {
        var response = await client
            .GetAsync($"api/Todo/GetTodo?id={id}")
            .ConfigureAwait(false);
        var responseString = await response
            .Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);
        return JsonConvert.DeserializeObject<TodoDetailsDto>(responseString)!;
    }

    public async Task DeleteTodo(int id)
    {
        await client
            .DeleteAsync($"api/Todo/DeleteTodo?id={id}")
            .ConfigureAwait(false);
    }

    public async Task<List<TodoSummaryDto>> ListTodos()
    {
        var response = await client
            .GetAsync($"api/Todo/ListTodos")
            .ConfigureAwait(false);
        var responseString = await response
            .Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);
        return JsonConvert.DeserializeObject<List<TodoSummaryDto>>(responseString)!;
    }
}
