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
        public IActionResult GetAll()
        {
            var query = _toDoDbContext.Todos; // Query para obtener todas las tareas

            return Ok(query.ToList());
        }


        [HttpGet("(id)")]
        public IActionResult Get(Guid id) // Para conseguir todas las tareas por ID
        {
            var task = _toDoDbContext.Todos.FirstOrDefault(x => x.Id == id);
            if(task is null)
            {
                return NotFound();
            }

            return Ok(task);
        }


        [HttpGet]
        public IActionResult Finished()
        {
            var query = _toDoDbContext.Todos.Where(x => x.Done == true); // Query para obtener las tareas realizadas

            return Ok(query.ToList());
        }


        [HttpGet]
        public IActionResult Pending()
        {
            var query = _toDoDbContext.Todos.Where(x => x.Done == false); // Query para obtener las tareas pendientes

            return Ok(query.ToList());
        }


        [HttpDelete("(id)")]
        public IActionResult Delete(Guid id) // Para eliminar una tarea por ID
        {
            var task = _toDoDbContext.Todos.FirstOrDefault(x => x.Id == id);

            if (task is null)
            {
                return NotFound();
            }

            _ = _toDoDbContext.Todos.Remove(task);
            _ = _toDoDbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("(NewTask)")]
        public IActionResult Update(ToDoTask newTask) // Para eliminar una tarea por ID
        {
            if (newTask is null)
            {
                return BadRequest(); // Devuelvo error si la nueva tarea no se encuentra presente
            }

            if(newTask.Id == Guid.Empty)
            {
                _toDoDbContext.Todos.Add(newTask);
            }
            else
            {
                var dbTask = _toDoDbContext.Todos.FirstOrDefault(x => x.Id == newTask.Id);
                if(dbTask is null)
                {
                    _ = _toDoDbContext.Todos.Add(newTask);
                }
                else
                {
                    if(dbTask.TimeStamp > newTask.TimeStamp)
                    {
                        return BadRequest("The entity is won't be updated");
                    }

                    // Asigno los valores pertinentes a las propiedades de la nueva tarea
                    dbTask.TaskName = newTask.TaskName;
                    dbTask.DescriptionTask = newTask.DescriptionTask;
                    dbTask.Done = newTask.Done;

                    dbTask.TimeStamp = DateTime.Now;
                    newTask.TimeStamp = dbTask.TimeStamp;
                }
            }
            _toDoDbContext.SaveChanges();

            return Ok();
        }
    }
}
