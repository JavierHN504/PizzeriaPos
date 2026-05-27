using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaPos.Core.Entities
{
    [Table("PedidosDetalle")]
    public class PedidoDetalle : BaseEntity
    {
        [Required]
        public int PedidoCabeceraId { get; set; }

        [ForeignKey("PedidoCabeceraId")]
        public PedidoCabecera PedidoCabecera { get; set; } = null!;

        [Required]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } = null!;

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
