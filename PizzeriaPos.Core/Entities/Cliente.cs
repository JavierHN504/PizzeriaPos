using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaPos.Core.Entities
{
    [Table("Clientes")]
    public class Cliente : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Apellido { get; set; }

        [MaxLength(15)]
        public string? Telefono { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        // Relacion: un cliente tiene muchas direcciones
        public ICollection<Direccion> Direcciones { get; set; } = new List<Direccion>();

        // Relacion: un cliente tiene muchos pedidos
        public ICollection<PedidoCabecera> Pedidos { get; set; } = new List<PedidoCabecera>();
    }
}
