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
    // Maneja las direcciones de entrega asociadas a cada cliente.
    public class DireccionRepository : IDireccionRepository
    {
        private readonly AppDbContext _context; // Contexto de base de datos inyectado

        public DireccionRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todas las direcciones activas de un cliente especifico
        public async Task<List<Direccion>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.Direcciones
                .Where(d => d.ClienteId == clienteId)
                .ToListAsync();
        }

        // Obtiene una direccion por ID incluyendo los datos del cliente
        public async Task<Direccion?> GetByIdAsync(int id)
        {
            return await _context.Direcciones
                .Include(d => d.Cliente) // Carga el cliente relacionado
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        // Agrega una nueva direccion a un cliente
        public async Task<Direccion> AddAsync(Direccion direccion)
        {
            direccion.CreatedAt = DateTime.Now;
            _context.Direcciones.Add(direccion);
            await _context.SaveChangesAsync();
            return direccion;
        }

        // Actualiza una direccion existente
        public async Task<Direccion> UpdateAsync(Direccion direccion)
        {
            direccion.UpdatedAt = DateTime.Now;
            _context.Direcciones.Update(direccion);
            await _context.SaveChangesAsync();
            return direccion;
        }

        // Soft delete: marca la direccion como eliminada sin borrarla fisicamente
        public async Task<bool> DeleteAsync(int id)
        {
            Direccion? direccion = await GetByIdAsync(id);
            if (direccion == null) return false;

            direccion.Deleted = true;
            direccion.UpdatedAt = DateTime.Now;
            _context.Direcciones.Update(direccion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
