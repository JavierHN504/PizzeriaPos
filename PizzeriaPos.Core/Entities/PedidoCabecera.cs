using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaPos.Core.Entities
{
    [Table("PedidosCabecera")]
    public class PedidoCabecera : BaseEntity
    {
        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; } = null!;

        public int? DireccionId { get; set; }

        [ForeignKey("DireccionId")]
        public Direccion? Direccion { get; set; }

        [Required]
        [MaxLength(50)]
        public string Estado { get; set; } = "Pendiente";

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [MaxLength(250)]
        public string? Observaciones { get; set; }

        // Relacion: un pedido tiene muchos detalles
        public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    }
}
