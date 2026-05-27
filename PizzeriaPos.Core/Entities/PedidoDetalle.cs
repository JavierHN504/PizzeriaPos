using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaPos.Core.Entities
{
    // Representa una linea de detalle dentro de un pedido. Tabla: PedidosDetalle
    // Cada detalle es un producto especifico con su cantidad y precio al momento del pedido.
    [Table("PedidosDetalle")]
    public class PedidoDetalle : BaseEntity
    {
        // Llave foranea: pedido al que pertenece este detalle
        public int PedidoCabeceraId { get; set; }

        [ForeignKey("PedidoCabeceraId")]
        public PedidoCabecera PedidoCabecera { get; set; } = null!; // Navegacion hacia la cabecera

        // Llave foranea: producto ordenado
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } = null!; // Navegacion hacia el producto

        [Required]
        public int Cantidad { get; set; } // Cantidad de unidades ordenadas

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; } // Precio del producto al momento del pedido

        // Subtotal calculado
        [NotMapped]
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
