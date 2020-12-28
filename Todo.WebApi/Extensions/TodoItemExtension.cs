using Todo.DTOs;

namespace Todo.WebApi.Models
{
    public static class TodoItemExtension
    {
        public static TodoItemDto ToDTO(this TodoItem item)
        {
            if (item == null)
            {
                return null;
            }

            return new TodoItemDto
            {
                Id = item.Id,
                Completed = item.Completed,
                Task = item.Task
            };
        }
    }
}
