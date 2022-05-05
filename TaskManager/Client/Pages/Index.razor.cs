using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TaskManager.Shared;

namespace TaskManager.Client.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] public HttpClient HttpClient { get; set; }

        private List<Todo> _pendingTasks = new(); // Lista donde recoger las tareas pendientes
        private List<Todo> _finishedTasks = new(); // Lista donde recoger las tareas finalizadas

        // Carga las tareas pendientes y finalizadas en sus correspondientes listas
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await LoadFinishedTodosAsync();
            await LoadPendingTodosAsync();
        }

        // Carga las tareas pendientes en su lista correspondiente, _pendingTasks
        private async Task LoadPendingTodosAsync()
        {
            var pendingResponse = await HttpClient.GetAsync("todo/pending"); // Realiza la consulta pertinente a la BBDD

            if (pendingResponse.IsSuccessStatusCode) // Si la petición resulta satisfactoria
            {
                // Consigo el contenido de la consulta en JSON
                string content = await pendingResponse.Content.ReadAsStringAsync();

                // Deserializo la info JSON y la meto en su lista correspondiente
                _pendingTasks = JsonConvert.DeserializeObject<List<Todo>>(content);
            }
        }

        // Carga las tareas finalizadas en su lista correspondiente, _finishedTasks
        private async Task LoadFinishedTodosAsync()
        {
            var finishedResponse = await HttpClient.GetAsync("todo/finished"); // Realiza la consulta pertinente a la BBDD

            if (finishedResponse.IsSuccessStatusCode) // Si la consulta resulta satisfactoria
            {
                // Consigo el contenido del resultado en formato JSON
                string content = await finishedResponse.Content.ReadAsStringAsync();

                // Deserializo la info JSON y la meto en su lista correspondiente
                _finishedTasks = JsonConvert.DeserializeObject<List<Todo>>(content);
            }
        }
    }
}
