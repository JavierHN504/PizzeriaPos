using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaPos.Core.Entities
{
    // Representa la cabecera de un pedido. Tabla: PedidosCabecera
    // Contiene los datos generales del pedido: cliente, direccion, estado y total.
    [Table("PedidosCabecera")]
    public class PedidoCabecera : BaseEntity
    {
        // Llave foranea: cliente que realizo el pedido
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; } = null!; // Navegacion hacia el cliente

        // Direccion de entrega (opcional)
        public int? DireccionId { get; set; }

        [ForeignKey("DireccionId")]
        public Direccion? Direccion { get; set; } // Navegacion hacia la direccion

        [Required]
        [MaxLength(50)]
        public string Estado { get; set; } = "Pendiente"; // Estado del pedido: Pendiente, En Proceso, Completado, Cancelado

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; } // Total a pagar por el pedido

        [MaxLength(250)]
        public string? Observaciones { get; set; } // Notas adicionales del pedido (opcional)

        // Un pedido tiene muchos detalles (productos ordenados)
        public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    }
}