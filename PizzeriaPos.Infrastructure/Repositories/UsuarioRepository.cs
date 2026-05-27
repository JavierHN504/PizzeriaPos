using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzeriaPos.Core.Entities;
using PizzeriaPos.Core.Interfaces;
using PizzeriaPos.Infrastructure.Data;

namespace PizzeriaPos.Infrastructure.Repositories
{
    // Maneja el acceso a datos de los usuarios del sistema POS.
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context; // Contexto de base de datos inyectado

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        // Busca un usuario por nombre de usuario (usado en el login)
        public async Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Activo);
        }

        // Obtiene un usuario por su ID
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.Activo);
        }

        // Verifica si ya existe un usuario con ese nombre (para evitar duplicados)
        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.NombreUsuario == nombreUsuario && !u.Deleted);
        }

        // Registra un nuevo usuario en la base de datos
        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            usuario.CreatedAt = DateTime.Now;
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
