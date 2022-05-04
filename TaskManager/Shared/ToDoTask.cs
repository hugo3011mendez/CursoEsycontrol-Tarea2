using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Shared
{
    public class ToDoTask
    {
		// Elementos necesarios para la parte con BBDD
		[Key]
		public Guid Id { get; set; }

		// Propiedades de las tareas
		public string TaskName { get; set; }
		public string DescriptionTask { get; set; }
		public bool Done { get; set; }
		public int TaskID { get; set; }

		// Constructor para crear una nueva tarea
		public ToDoTask(int id, string name, string desc)
		{
			TaskID = id;
			Done = false;
			TaskName = name;
			DescriptionTask = desc;
		}
	}
}
