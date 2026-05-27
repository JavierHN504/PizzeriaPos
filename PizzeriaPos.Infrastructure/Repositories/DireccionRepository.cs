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
    public class DireccionRepository : IDireccionRepository
    {
        private readonly AppDbContext _context;

        public DireccionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Direccion>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.Direcciones
                .Where(d => d.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<Direccion?> GetByIdAsync(int id)
        {
            return await _context.Direcciones
                .Include(d => d.Cliente)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Direccion> AddAsync(Direccion direccion)
        {
            direccion.CreatedAt = DateTime.Now;
            _context.Direcciones.Add(direccion);
            await _context.SaveChangesAsync();
            return direccion;
        }

        public async Task<Direccion> UpdateAsync(Direccion direccion)
        {
            direccion.UpdatedAt = DateTime.Now;
            _context.Direcciones.Update(direccion);
            await _context.SaveChangesAsync();
            return direccion;
        }

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
