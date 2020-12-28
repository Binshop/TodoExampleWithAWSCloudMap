using Microsoft.EntityFrameworkCore;
using Todo.WebApi.Models;

namespace Todo.WebApi.Data
{
    public class TodoContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Todo;Integrated Security=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
