using Microsoft.EntityFrameworkCore;
using TaskManager.Shared;

namespace TaskManager.Server.Infraestructure
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDoTask> Todos { get; set; }
    }
}
