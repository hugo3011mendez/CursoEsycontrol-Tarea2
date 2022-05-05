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

        private ToDoTask _task = new(); // Una nueva tarea vacía, para subir sus datos a la BBDD cuando se añada


        // Consigue las tareas pendientes y las tareas realizadas, y las guarda en las listas anteriormente declaradas
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await LoadFinishedTodosAsync();
            await LoadPendingTodosAsync();
        }

        // Consigue las tareas pendientes conectándose a la BBDD, y las guarda en su lista previamente declarada
        private async Task LoadPendingTodosAsync()
        {
            var pendingResponse = await HttpClient.GetAsync("todo/pending");

            if (pendingResponse.IsSuccessStatusCode) // Si la respuesta es correcta y se obtienen los datos
            {
                string content = await pendingResponse.Content.ReadAsStringAsync(); // Consigo los datos como JSON

                // Deserializo los datos JSON y los convierto a Lista de ToDoTask
                _pendingTasks = JsonConvert.DeserializeObject<List<ToDoTask>>(content);
            }
        }

        // Consigue las tareas realizadas conectándose a la BBDD, y las guarda en su lista previamente declarada
        private async Task LoadFinishedTodosAsync()
        {
            var finishedResponse = await HttpClient.GetAsync("todo/finished");

            if (finishedResponse.IsSuccessStatusCode) // Si la respuesta es correcta y se obtienen los datos
            {
                string content = await finishedResponse.Content.ReadAsStringAsync(); // Consigo los datos como JSON

                // Deserializo los datos JSON y los convierto a Lista de ToDoTask
                _finishedTasks = JsonConvert.DeserializeObject<List<ToDoTask>>(content);
            }
        }

        // Llama al controlador para que añada la nueva tarea a la BBDD
        private async Task HandleValidSubmitAsync()
        {
            string content = JsonConvert.SerializeObject(_task); // Convierte el objeto en JSON

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            // Para indicar que es contenido JSON
            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            await HttpClient.PostAsync("todo/update", byteContent); // Llama al método Update() del controlador
        }
	}
}
