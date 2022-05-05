using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Shared
{
    public class ToDoTask
    {
		// Todas sus propiedades serán representadas en la BBDD
		[Key]
		public Guid Id { get; set; } // ID de la tarea, proporcionado automáticamente por la BBDD

		// Propiedades de las tareas
		public string TaskName { get; set; }
		public string DescriptionTask { get; set; }
		public bool Done { get; set; }

		public DateTime TimeStamp { get; set; } = DateTime.Now; // Fecha que indica cuándo ha sido creada o actualizada

		public Guid ParentID { get; set; } // ID de su tarea padre, puede ser null o vacío

		public ToDoTask() { } // Constructor vacío
	}
}
