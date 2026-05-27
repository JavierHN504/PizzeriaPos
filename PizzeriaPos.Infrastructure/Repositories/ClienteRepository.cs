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
    // Aqui es donde se habla con la base de datos via Entity Framework.
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context; // Contexto de base de datos inyectado

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todos los clientes activos incluyendo sus direcciones
        public async Task<List<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .Include(c => c.Direcciones) // Carga las direcciones relacionadas
                .ToListAsync();
        }

        // Obtiene un cliente por ID incluyendo sus direcciones
        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Direcciones)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Agrega un nuevo cliente a la base de datos
        public async Task<Cliente> AddAsync(Cliente cliente)
        {
            cliente.CreatedAt = DateTime.Now;
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        // Actualiza los datos de un cliente existente
        public async Task<Cliente> UpdateAsync(Cliente cliente)
        {
            cliente.UpdatedAt = DateTime.Now;
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        // Soft delete: marca el cliente como eliminado sin borrarlo fisicamente
        public async Task<bool> DeleteAsync(int id)
        {
            Cliente? cliente = await GetByIdAsync(id);
            if (cliente == null) return false;

            cliente.Deleted = true;
            cliente.UpdatedAt = DateTime.Now;
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
