using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
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

        private List<Todo> _pendingSubTasks = new(); // Lista donde recoger las subtareas pendientes
        private List<Todo> _finishedSubTasks = new(); // Lista donde recoger las subtareas finalizadas

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
            // Realiza la consulta pertinente al controlador, función Pending()
            var pendingResponse = await HttpClient.GetAsync("todo/pending");

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
            // Realiza la consulta pertinente al controlador, función Finished()
            var finishedResponse = await HttpClient.GetAsync("todo/finished");

            if (finishedResponse.IsSuccessStatusCode) // Si la consulta resulta satisfactoria
            {
                // Consigo el contenido del resultado en formato JSON
                string content = await finishedResponse.Content.ReadAsStringAsync();

                // Deserializo la info JSON y la meto en su lista correspondiente
                _finishedTasks = JsonConvert.DeserializeObject<List<Todo>>(content);
            }
        }


        // Muestra la info de las subtareas, cuando se clicke en una tarea
        private async void TodoOnClick(Guid IDParent)
        {
            // Llamo a las funciones para que carguen las subtareas de la tarea clickada
            await LoadPendingSubtasksAsync(IDParent);
            await LoadFinishedSubtasksAsync(IDParent);
        }

        // Establece la lista de subtareas pendientes de la tarea clickada
        private async Task LoadPendingSubtasksAsync(Guid IDParent)
        {
            // Realiza la consulta pertinente al controlador, función PendingSubTasks(ID)
            var pendingResponse = await HttpClient.GetAsync($"todo/pendingsubtasks/{IDParent}");

            if (pendingResponse.IsSuccessStatusCode) // Si la petición resulta satisfactoria
            {
                // Consigo el contenido de la consulta en JSON
                string content = await pendingResponse.Content.ReadAsStringAsync();

                // Deserializo la info JSON y la meto en su lista correspondiente
                _pendingSubTasks = JsonConvert.DeserializeObject<List<Todo>>(content);
            }
        }

        // Establece la lista de subtareas finalizadas de la tarea clickada
        private async Task LoadFinishedSubtasksAsync(Guid IDParent)
        {
            // Realiza la consulta pertinente al controlador,función FinishedSubTasks(ID)
            var finishedResponse = await HttpClient.GetAsync($"todo/finishedsubtasks/{IDParent}");

            if (finishedResponse.IsSuccessStatusCode) // Si la consulta resulta satisfactoria
            {
                // Consigo el contenido del resultado en formato JSON
                string content = await finishedResponse.Content.ReadAsStringAsync();

                // Deserializo la info JSON y la meto en su lista correspondiente
                _finishedSubTasks = JsonConvert.DeserializeObject<List<Todo>>(content);
            }
        }
    }
}
