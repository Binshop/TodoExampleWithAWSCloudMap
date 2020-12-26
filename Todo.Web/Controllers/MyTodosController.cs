using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Todo.Models;

namespace Todo.Web.Controllers
{
    [Route("api/[controller]")]
    public class MyTodosController : ControllerBase
    {
        private static readonly TodoList todoList = Seed();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(todoList);
        }

        [HttpGet("{id:Guid}")]
        public IActionResult GetById(Guid id)
        {
            var todo = todoList.Todos.FirstOrDefault(x => x.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPut]
        public IActionResult Put([FromBody] TodoItem todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var todoInStor = todoList.Todos.FirstOrDefault(x => x.Id == todo.Id);

            if (todoInStor == null)
            {
                return BadRequest();
            }

            todoInStor.Completed = todo.Completed;
            todoInStor.DueDate = todo.DueDate;
            todoInStor.Task = todo.Task;

            return Ok(todoInStor);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TodoItem todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var exist = todoList.Todos.Any(x => x.Task == todo.Task);
            if (exist)
            {
                return Conflict();
            }

            todo.Id = Guid.NewGuid();

            todoList.Todos.Add(todo);

            return CreatedAtAction(nameof(GetById), todo.Id, todo);
        }

        private static TodoList Seed()
        {
            var mytodos = new TodoList
            {
                Owner = "Demo",
            };
            mytodos.Todos.Add(new TodoItem { Id = Guid.NewGuid(), Task = "Create a Demo App for AWS Cloud Map." });
            mytodos.Todos.Add(new TodoItem { Id = Guid.NewGuid(), Task = "Show the Demo." });
            mytodos.Todos.Add(new TodoItem { Id = Guid.NewGuid(), Task = "Prepare the PPT." });
            return mytodos;
        }
    }
}
