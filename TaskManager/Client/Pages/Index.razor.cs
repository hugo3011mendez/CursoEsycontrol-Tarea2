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

        private ToDoTask _task = new(); // Una nueva tarea vacía

        // TODO : Aprender a hacer esta función bien
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var finishedResponse = await HttpClient.GetAsync("todoTask/finished"); // Devuelve el resultado de la consulta

            if (finishedResponse.IsSuccessStatusCode)
            {
                var content = await finishedResponse.Content.ReadAsStringAsync();

                _finishedTasks = JsonConvert.DeserializeObject<List<ToDoTask>>(content);
            }
        }

        private async Task LoadPendingTodosTask() // Aquí se agrupará el código para cargar todas las tareas pendientes
        {

        }
        private async Task LoadFinishedTodosTask() // Aquí se agrupará el código para cargar todas las tareas realizadas
        {

        }


        // Para manejar los datos provenientes del formulario que establece datos a la tarea vacía
        private async Task HandleValidSubmitAsync()
        {
            string content = JsonConvert.SerializeObject(_task);

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            await HttpClient.PostAsync("todo/update", byteContent);
        }


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
