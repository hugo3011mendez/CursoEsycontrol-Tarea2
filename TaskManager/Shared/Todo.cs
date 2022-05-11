using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Shared
{
    public class Todo // Representa una tarea
    {
        [Key]
        public Guid Id { get; set; } // ID de la tarea

        [Required]
        public string Name { get; set; } // Nombre de la tarea

        [Required]
        public string Description { get; set; } // Descripción de la tarea

        public bool Done { get; set; } // Booleana indicando si está realizada

        public DateTime Timestamp { get; set; } // Momento en el que se creó, o se modificó

        public Guid ParentID { get; set; } // Si se trata de una subtarea, referenciará al ID de su tarea padre

        public Todo() { } // Constructor vacío
    }
}
