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
		// Elementos necesarios para la parte con BBDD
		[Key]
		public Guid Id { get; set; }

		// Propiedades de las tareas
		public string TaskName { get; set; }
		public string DescriptionTask { get; set; }
		public bool Done { get; set; }
		public int TaskID { get; set; }

		public DateTime TimeStamp { get; set; } = DateTime.Now; // Por defecto que sea el momento actual

        // Constructor para crear una nueva tarea
        public ToDoTask(string TaskName, string DescriptionTask)
		{
			Done = false;
			this.TaskName = TaskName;
			this.DescriptionTask = DescriptionTask;
		}
	}
}
