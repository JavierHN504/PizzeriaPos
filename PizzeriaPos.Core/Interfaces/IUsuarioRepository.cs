using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzeriaPos.Core.Entities;

namespace PizzeriaPos.Core.Interfaces
{
    // Define las operaciones de autenticacion y gestion de usuarios.
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario); // Buscar usuario por nombre (para login)
        Task<Usuario?> GetByIdAsync(int id); // Obtener usuario por ID
        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario); // Verificar si el nombre de usuario ya existe
        Task<Usuario> AddAsync(Usuario usuario); // Registrar un nuevo usuario
    }
}
