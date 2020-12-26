using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        [Required]
        public string Task { get; set; }

        public DateTime? DueDate { get; set; }

        public bool Completed { get; set; }
    }
}
