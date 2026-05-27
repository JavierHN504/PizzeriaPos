using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaPos.Core.Entities
{
    // Representa un usuario del sistema POS. Tabla: Usuarios
    // Los usuarios son el personal de la pizzeria que usa el sistema.
    [Table("Usuarios")]
    public class Usuario : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string NombreUsuario { get; set; } = string.Empty; // Nombre de usuario para login (unico)

        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; } = string.Empty; // Nombre completo del empleado

        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Contrasena hasheada

        public bool Activo { get; set; } = true; // Indica si el usuario puede iniciar sesion
    }
}