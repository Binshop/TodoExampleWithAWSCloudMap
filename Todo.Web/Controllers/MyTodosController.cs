using Microsoft.AspNetCore.Mvc;
using Todo.Models;

namespace Todo.Web.Controllers
{
    [Route("api/[controller]")]
    public class MyTodosController : ControllerBase
    {
        [HttpGet]
        public TodoList Get()
        {
            var mytodos = new TodoList
            {
                Owner = "Demo",
            };
            mytodos.Todos.Add(new TodoItem { Task = "Demo AWS Cloud Map" });
            return mytodos;
        }
    }
}
