using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TaskManager.Shared;

namespace TaskManager.Client.Pages
{
    public partial class EditTodo
    {
        [Inject] public HttpClient HttpClient { get; set; }

        // Para una vez realizadas las acciones, volver al Index
        [Inject] public NavigationManager NavigationManager { get; set; }

        // ID de la tarea correspondiente
        [Parameter] public Guid Id { get; set; }

        // Booleana indicando si se creará una subtarea
        [Parameter] public bool Hijo { get; set; }


        private Todo _todo = new(); // Creo una nueva tarea vacía

        private string _texto = ""; // Para la info que se mostrará en pantalla

        protected override async Task OnParametersSetAsync() // Compruebo el parámetro de la tarea pasada
        {
            await base.OnParametersSetAsync();

            if (Id != Guid.Empty && Hijo != true) // Compruebo que sea una tarea con datos
            {
                await LoadTodoAsync(); // En ese caso, ejecuto la función que carga la tarea en cuestión
                _texto = "Tarea";
            }
            else if(Hijo == true) // Si es una subtarea a crear
            {
                _texto = "Subtarea";
                if(Id != Guid.Empty) // En el caso de ser una subtarea a editar
                {
                    await LoadTodoAsync(); // En ese caso, ejecuto la función que carga la tarea en cuestión
                    if (Id == _todo.Id) // Compruebo que no haya cargado la Tarea, en vez de la Subtarea
                    {
                        _todo = new();
                    }
                }
            }
            else // En el caso de que se quiera añadir una nueva tarea
            {
                _texto = "Tarea";
            }
        }

        private async Task LoadTodoAsync() // Carga la info de la tarea pasada a esta página
        {
            // Realiza la consulta pertinente al controlador, función Get(id)
            var todoResponse = await HttpClient.GetAsync($"todo/get/{Id}");

            if (todoResponse.IsSuccessStatusCode) // Si la petición resulta satisfactoria
            {
                string content = await todoResponse.Content.ReadAsStringAsync(); // Consigo la info de la tarea en JSON

                _todo = JsonConvert.DeserializeObject<Todo>(content); // Deserializo la info JSON y la convierto a tipo tarea
            }
            else // Si la petición resulta con algún error
            {
                NavigationManager.NavigateTo("/"); // Vuelvo al Index
            }
        }

        private async Task HandleValidSubmitAsync() // Maneja el evento submit del formulario para crear o editar una tarea
        {
            if(Hijo == true && Id != _todo.ParentID) // Compruebo que, en el caso que queramos crear una subtarea
            {
                _todo.ParentID = Id; // Establecer su campo ParentID
            }

            string content = JsonConvert.SerializeObject(_todo); // Convierto la tarea en JSON

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // Añado la cabecera al JSON

            // Realiza la consulta pertinente al controlador, función Update()
            _ = await HttpClient.PostAsync("todo/update", byteContent);

            _todo = new(); // Termino creanddo otra instancia vacía de la tarea anteriormente creada

            NavigationManager.NavigateTo("/"); // Vuelvo al Index
        }
    }
}
