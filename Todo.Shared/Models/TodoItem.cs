using System;

namespace Todo.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        public string Task { get; set; }

        public DateTime? Deadline { get; set; }

        public bool IsDone { get; set; }
    }
}
