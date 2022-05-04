using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Server.Infraestructure;
using TaskManager.Shared;

namespace TaskManager.Server.Controllers
{
    [ApiController]
    [Route("[todoTask]")]
    public class ToDoTaskController : Controller
    {
        private readonly ToDoDbContext _toDoDbContext;
        
        [HttpGet]
        public List<ToDoTask> GetTasks()
        {
            var query = _toDoDbContext.Tasks.Where(x => x.Done == false); // Query para obtener las tareas pendientes
            string sql = query.ToQueryString();

            return query.ToList();
        }

        [HttpGet("(id)")]
        public List<ToDoTask> GetTasksById(Guid id)
        {
            var query = _toDoDbContext.Tasks;
            string sql = query.ToQueryString();
            return query.ToList();
        }
    }
}
