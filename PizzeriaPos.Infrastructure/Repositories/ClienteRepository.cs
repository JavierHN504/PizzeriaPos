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
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .Include(c => c.Direcciones)
                .ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Direcciones)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente> AddAsync(Cliente cliente)
        {
            cliente.CreatedAt = DateTime.Now;
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> UpdateAsync(Cliente cliente)
        {
            cliente.UpdatedAt = DateTime.Now;
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

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
