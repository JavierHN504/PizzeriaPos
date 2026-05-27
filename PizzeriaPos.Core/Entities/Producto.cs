using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaPos.Core.Entities
{
    // Representa un producto del menu de la pizzeria. Tabla: Productos
    [Table("Productos")]
    public class Producto : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty; // Nombre del producto (obligatorio)

        [MaxLength(250)]
        public string? Descripcion { get; set; } // Descripcion del producto (opcional)

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; } // Precio de venta del producto

        [Required]
        [MaxLength(50)]
        public string Categoria { get; set; } = string.Empty; // Categoria: Pizza, Bebida, Extra, etc.

        public bool Disponible { get; set; } = true; // Indica si el producto esta disponible para ordenar
    }
}
