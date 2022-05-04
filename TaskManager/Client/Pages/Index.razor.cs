using System.Collections.Generic;
using TaskManager.Shared;

namespace TaskManager.Client.Pages
{
    public partial class Index // Aquí podemos poner el código dentro de la parte @code en Index
    {
		private List<ToDoTask> pendingTasks { get; set; } = new(); // Lista de tareas pendientes
		private List<ToDoTask> finishedTasks { get; set; } = new(); // Lista de tareas realizadas


		static string NewTaskName = ""; // Para el nombre de la nueva tarea
		static string NewTaskDescription = ""; // Para la descripción de la nueva tarea
		static int countTaskId = 0; // Contador del ID de las tareas 

		// Comprueba y añade una nueva tarea si la info escrita por el usuario procede 
		void addNewTask()
		{
			if (NewTaskName != "" && NewTaskDescription != "")
			{
				ToDoTask newTask = new(countTaskId, NewTaskName, NewTaskDescription);
				countTaskId++;
			}
		}

	}
}
