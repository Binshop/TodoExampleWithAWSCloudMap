using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.DTOs;
using Todo.WebApi.Models;

namespace Todo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly IDynamoDBContext _dbContext;

        public TodosController(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
        {
            var result = _dbContext.ScanAsync<TodoItem>(null);
            var todos = new List<TodoItem>();
            do
            {
                var paritalItems = await result.GetNextSetAsync();
                todos.AddRange(paritalItems);
            }
            while (!result.IsDone);

            return todos.Select(x => x.ToDTO()).ToList();
        }

        // GET: api/Todos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItem(Guid id)
        {
            var todoItem = await _dbContext.LoadAsync<TodoItem>(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem.ToDTO();
        }

        // PUT: api/Todos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(Guid id, TodoItemDto todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _dbContext.LoadAsync<TodoItem>(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Task = todoItemDTO.Task;
            todoItem.Completed = todoItemDTO.Completed;
            await _dbContext.SaveAsync(todoItem);
            return Ok(todoItem.ToDTO());
        }

        // POST: api/Todos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> CreateTodoItem(TodoItemDto todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Completed = todoItemDTO.Completed,
                Task = todoItemDTO.Task,
                Created = DateTime.UtcNow
            };

            await _dbContext.SaveAsync(todoItem);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem.ToDTO());
        }

        // DELETE: api/Todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            var todoItem = await _dbContext.LoadAsync<TodoItem>(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            await _dbContext.DeleteAsync(id);

            return NoContent();
        }
    }
}
