using System;

namespace Todo.WebApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }

        public string Task { get; set; }

        public DateTime Created { get; set; }

        public bool Completed { get; set; }
    }
}
