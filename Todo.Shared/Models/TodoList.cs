using System.Collections.Generic;

namespace Todo.Models
{
    public class TodoList
    {
        public string Owner { get; set; }

        public List<TodoItem> Todos { get; set; }

        public TodoList()
        {
            Todos = new List<TodoItem>();
        }
    }
}
