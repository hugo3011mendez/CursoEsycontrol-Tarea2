using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TaskManager.Server.Infrastructure;
using TaskManager.Shared;

namespace TaskManager.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _todoDbContext; // Instancia del contexto de la conexión con la BBDD

        public TodoController(TodoDbContext todoDbContext) => _todoDbContext = todoDbContext; // Constructor del controlador

        [HttpGet]
        public IActionResult Finished() // Consulta que devuelve las tareas finalizadas
        {
            var query = _todoDbContext.Todos.Where(x => x.Done == true && x.ParentID == Guid.Empty); // Armo la query

            return Ok(query.ToList()); // Ejecuto la query y devuelvo su resultado como lista
        }

        [HttpGet]
        public IActionResult Pending() // Consulta que devuelve las tareas pendientes
        {
            var query = _todoDbContext.Todos.Where(x => x.Done == false && x.ParentID == Guid.Empty); // Armo la query

            return Ok(query.ToList()); // Y devuelvo su resultado como lista
        }


        // Devuelve las subtareas finalizadas, según la ID de su padre
        [HttpGet("{id}")]
        public IActionResult FinishedSubTasks(Guid ID)
        {
            var query = _todoDbContext.Todos.Where(x => x.Done == true && x.ParentID == ID); // Armo la query

            return Ok(query.ToList()); // Ejecuto la query y devuelvo su resultado como lista
        }

        // Devuelve las subtareas pendientes, según la ID de su padre
        [HttpGet("{id}")]
        public IActionResult PendingSubTasks(Guid ID)
        {
            var query = _todoDbContext.Todos.Where(x => x.Done == false && x.ParentID == ID); // Armo la query

            return Ok(query.ToList()); // Y devuelvo su resultado como lista
        }


        [HttpGet("{id}")]
        public IActionResult Get(Guid id) // Consulta que consigue una tarea por su ID
        {
            var todo = _todoDbContext.Todos.FirstOrDefault(x => x.Id == id); // Armo una tarea según la info de la query

            if (todo is null) // Compruebo si la tarea está vacía
            {
                return NotFound(); // Y en caso afirmativo, devuelvo un NotFound()
            }

            return Ok(todo); // Devuelvo la tarea resultado de la query si todo el procedimiento ha ido bien
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id) // Elimino una tarea buscando por su ID
        {
            var todo = _todoDbContext.Todos.FirstOrDefault(x => x.Id == id); // Creo una tarea según la info de la query

            if (todo is null) // Compruebo si la tarea está vacía
            {
                return NotFound(); // Y en caso afirmativo, devuelvo un NotFound()
            }

            _ = _todoDbContext.Todos.Remove(todo); // Si todo ha ido bien, elimino la tarea de la BBDD
            _ = _todoDbContext.SaveChanges(); // Y finalmente guardo los cambios

            return Ok(); // Termino devolviendo el resultado de la query
        }

        [HttpPost]
        public IActionResult Update(Todo newTodo) // Crea o edita una tarea, pasándola como parámetro
        {
            if (newTodo is null) // Comprueba si la tarea parámetro está vacía
            {
                return BadRequest(); // Y en caso afirmativo, devuelve un BadRequest()
            }

            DateTime now = DateTime.Now; // Asigno el valor del momento actual en una variable

            if (newTodo.Id == Guid.Empty) // Compruebo si la ID de la tarea parámetro está vacía
            { // En este caso, sería una nueva tarea a crear
                newTodo.Timestamp = now; // Establezco el momento guardado al timestamp de la nueva tarea
                _ = _todoDbContext.Todos.Add(newTodo); // Y añado la nueva tarea a la colección de tareas del contexto
            }
            else // Si la tarea tiene ID, significa que hay que modificarla
            {
                // Busco la tarea en el contexto de la BBDD
                var dbTodo = _todoDbContext.Todos.FirstOrDefault(x => x.Id == newTodo.Id);
                if (dbTodo is null) // Compruebo si el resultado de la query existe
                {
                    newTodo.Timestamp = now; // Establezco el momento de la tarea al anteriormente guardado
                    _ = _todoDbContext.Todos.Add(newTodo); // Y añado dicha tarea a su colección en el contexto de la BBDD
                }
                else // Si la query devuelve un resultado
                {
                    if (dbTodo.Timestamp > newTodo.Timestamp) // Compruebo que la tarea no se haya actualizado mientras tanto
                    {
                        return BadRequest("The entity is not updated"); // En ese caso, no la edito y devuelvo un error
                    }

                    // Finalmente, establezco las propiedades de la nueva tarea
                    dbTodo.Name = newTodo.Name;
                    dbTodo.Description = newTodo.Description;
                    dbTodo.Done = newTodo.Done;
                    dbTodo.Timestamp = now;
                    // TODO : Implementar para establecer también el ParentID
                }
            }

            _ = _todoDbContext.SaveChanges(); // Y por último, guardo los cambios en la BBDD

            return Ok(); // Termino devolviendo un Ok()
        }

        // TODO : Realizar 2 funciones más => Al eliminar un padre, eliminar los hijos; y Listar todos los hijos del padre;

    }
}