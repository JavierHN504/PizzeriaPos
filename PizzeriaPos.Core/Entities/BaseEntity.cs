using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaPos.Core.Entities
{
    // Clase base para todas las entidades. Define campos comunes en cada tabla.
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; } // Llave primaria autoincremental
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Fecha de creacion
        public DateTime? UpdatedAt { get; set; } // Fecha de ultima modificacion (null si nunca se edito)
        public bool Deleted { get; set; } = false; // Soft delete: true = eliminado logicamente, no fisicamente
    }
}