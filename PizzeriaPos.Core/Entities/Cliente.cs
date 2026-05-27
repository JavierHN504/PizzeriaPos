using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaPos.Core.Entities
{
    // Representa un cliente de la pizzeria. Tabla: Clientes
    [Table("Clientes")]
    public class Cliente : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty; // Nombre del cliente (obligatorio)

        [MaxLength(100)]
        public string? Apellido { get; set; } // Apellido (opcional)

        [MaxLength(15)]
        public string? Telefono { get; set; } // Telefono de contacto (opcional)

        [MaxLength(150)]
        public string? Email { get; set; } // Correo electronico (opcional)

        // Un cliente puede tener muchas direcciones de entrega
        public ICollection<Direccion> Direcciones { get; set; } = new List<Direccion>();
        // Un cliente puede tener muchos pedidos
        public ICollection<PedidoCabecera> Pedidos { get; set; } = new List<PedidoCabecera>();
    }
}
