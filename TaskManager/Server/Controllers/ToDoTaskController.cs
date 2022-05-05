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
    [Route("[controller] /[action]")]
    public class ToDoTaskController : Controller
    {
        private readonly ToDoDbContext _toDoDbContext; // Instancia haciendo referencia al DBContext de la BBDD
        
        [HttpGet]
        public IActionResult GetAll() // Consigue todas las tareas
        {
            var query = _toDoDbContext.Todos; // Realizo la query correspondiente

            return Ok(query.ToList()); // Devuelve el resultado de la ejecución de la query
        }


        [HttpGet("(id)")]
        public IActionResult Get(Guid id) // Busca una tarea por su ID
        {
            var task = _toDoDbContext.Todos.FirstOrDefault(x => x.Id == id); // Consigue la tarea buscada

            if(task is null) // Compruebo si se ha encontrado la tarea buscada
            {
                return NotFound(); // En caso negativo, devuelvo un NotFound()
            }

            return Ok(task); // Si se ha encontrado la tarea, devuelvo el resultado de la ejecución de la query
        }


        [HttpGet]
        public IActionResult Finished() // Obtiene las tareas realizadas
        {
            var query = _toDoDbContext.Todos.Where(x => x.Done == true); // Monto la query correspondiente

            return Ok(query.ToList()); // Devuelvo el resultado de la ejecución de la query
        }


        [HttpGet]
        public IActionResult Pending() // Obtiene las tareas pendientes
        {
            var query = _toDoDbContext.Todos.Where(x => x.Done == false); // Monto la query correspondiente

            return Ok(query.ToList()); // Devuelvo el resultado de la ejecución de la query
        }


        [HttpDelete("(id)")]
        public IActionResult Delete(Guid id) // Para eliminar una tarea por ID
        {
            var task = _toDoDbContext.Todos.FirstOrDefault(x => x.Id == id); // Obtengo la tarea buscada

            if (task is null) // Compruebo si se ha encontrado
            {
                return NotFound(); // En caso negativo, devuelve un NotFound()
            }

            // En caso afirmativo :
            _ = _toDoDbContext.Todos.Remove(task); // Elimina de la tabla a la tarea buscada
            _ = _toDoDbContext.SaveChanges(); // Y guarda los cambios en la BBDD

            return Ok(); // Devuelve el resultado de la consulta realizada
        }


        [HttpPost]
        public IActionResult Update(ToDoTask newTask) // Añade o actualiza una tarea en la BBDD
        {
            if (newTask is null)
            {
                return BadRequest(); // Devuelvo error si la tarea a añadir o actualizar está vacía
            }

            var now = DateTime.Now; // Establezco la fecha del momento actual

            if(newTask.Id == Guid.Empty) // Compruebo si la ID de la tarea buscada está vacía
            {
                _toDoDbContext.Todos.Add(newTask); // Y en ese caso, la añado al context para que le proporcione una ID
            }
            else // Si la tarea ya tiene ID, significa que vamos a actualizarla
            {
                // Consigo los datos guardados en la BBDD de la misma tarea
                var dbTask = _toDoDbContext.Todos.FirstOrDefault(x => x.Id == newTask.Id);
                if(dbTask is null) // Compruebo si la tarea está vacía
                {
                    _ = _toDoDbContext.Todos.Add(newTask); // Y en ese caso la añado al context
                }
                else // Si la tarea a actualizar no está vacía
                {
                    // Compruebo si la tarea no ha sido actualizada por otro mientras se realizaba esta actualización
                    if (dbTask.TimeStamp > newTask.TimeStamp)
                    {
                        return BadRequest("The entity is won't be updated"); // Y en ese caso, devuelvo un error y no la actualizo
                    }

                    // Asigno sus valores de la BBDD a las propiedades de tarea actualizada
                    dbTask.TaskName = newTask.TaskName;
                    dbTask.DescriptionTask = newTask.DescriptionTask;
                    dbTask.Done = newTask.Done;
                    dbTask.TimeStamp = now;

                    newTask.TimeStamp = dbTask.TimeStamp; // Y actualizo el timestamp de la tarea actualizada
                }
            }
            _toDoDbContext.SaveChanges(); // Finalmente, guardo los cambios

            return Ok(); // Devuelvo un Ok() como resultado de la ejecución de esta consulta
        }
    }
}