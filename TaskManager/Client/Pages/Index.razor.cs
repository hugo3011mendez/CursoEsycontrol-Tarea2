using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TaskManager.Shared;


namespace TaskManager.Client.Pages
{
    public partial class Index // Aquí podemos poner el código dentro de la parte @code en Index
    {
		[Inject] public HttpClient HttpClient { get; set; } // Cliente HTTP

		private List<ToDoTask> _pendingTasks { get; set; } = new(); // Lista de tareas pendientes
		private List<ToDoTask> _finishedTasks { get; set; } = new(); // Lista de tareas realizadas

		// TODO : Aprender a hacer esta función bien
   //     protected override async ToDoTask OnInitializedAsync()
   //     {
			//await Task.Delay(1);
   //         await base.OnInitializedAsync();

			//var response = await HttpClient.GetAsync("todoTask/finished"); // Devuelve el resultado de la consulta

   //         if (response.IsSuccessStatusCode)
   //         {
			//	var content = response.Content.ReadAsStringAsync();

			//	_finishedTasks = JsonConvert.DeserializeObject(<List<ToDoTask>>(content));
   //         }
   //     }


        static string NewTaskName = ""; // Para el nombre de la nueva tarea
		static string NewTaskDescription = ""; // Para la descripción de la nueva tarea
		static int countTaskId = 0; // Contador del ID de las tareas 

		// Comprueba y añade una nueva tarea si la info escrita por el usuario procede 
		void addNewTask()
		{
			if (NewTaskName != "" && NewTaskDescription != "")
			{
				ToDoTask newTask = new(NewTaskName, NewTaskDescription);
				countTaskId++;
			}
		}

	}
}
