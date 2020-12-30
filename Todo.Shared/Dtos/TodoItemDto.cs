using System;

namespace Todo.DTOs
{
    public class TodoItemDto
    {
        public Guid Id { get; set; }

        public string Task { get; set; }

        public bool Completed { get; set; }
    }
}
