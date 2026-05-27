using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaPos.Core.Entities
{
    [Table("Direcciones")]
    public class Direccion : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Calle { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Ciudad { get; set; }

        [MaxLength(100)]
        public string? Departamento { get; set; }

        [MaxLength(20)]
        public string? CodigoPostal { get; set; }

        public bool EsPrincipal { get; set; } = false;

        // Llave foranea hacia Cliente
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; } = null!;
    }
}
