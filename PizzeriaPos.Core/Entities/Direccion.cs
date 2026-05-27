using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaPos.Core.Entities
{
    // Representa una direccion de entrega de un cliente. Tabla: Direcciones
    // Un cliente puede tener multiples direcciones registradas.
    [Table("Direcciones")]
    public class Direccion : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Calle { get; set; } = string.Empty; // Calle o descripcion de la direccion (obligatorio)

        [MaxLength(100)]
        public string? Ciudad { get; set; } // Ciudad (opcional)

        [MaxLength(100)]
        public string? Departamento { get; set; } // Departamento o estado (opcional)

        [MaxLength(20)]
        public string? CodigoPostal { get; set; } // Codigo postal (opcional)

        public bool EsPrincipal { get; set; } = false; // Indica si es la direccion principal del cliente

        // Llave foranea: referencia al cliente dueno de esta direccion
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; } = null!; // Navegacion hacia el cliente
    }
}