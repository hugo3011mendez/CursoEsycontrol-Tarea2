using Microsoft.EntityFrameworkCore;
using TaskManager.Shared;

namespace TaskManager.Server.Infraestructure
{
    public class ToDoDbContext : DbContext
    {
        public DbSet<ToDoTask> Todos { get; set; }

        public ToDoDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
