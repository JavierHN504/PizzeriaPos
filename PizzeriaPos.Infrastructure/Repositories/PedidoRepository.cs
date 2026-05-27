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
    // Maneja pedidos completos: cabecera + detalles + productos relacionados.
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context; // Contexto de base de datos inyectado

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todos los pedidos activos con cliente, direccion y detalles de productos
        public async Task<List<PedidoCabecera>> GetAllAsync()
        {
            return await _context.PedidosCabecera
                .Include(p => p.Cliente)
                .Include(p => p.Direccion)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto) // Carga el producto dentro de cada detalle
                .ToListAsync();
        }

        // Obtiene un pedido por ID con todos sus datos relacionados
        public async Task<PedidoCabecera?> GetByIdAsync(int id)
        {
            return await _context.PedidosCabecera
                .Include(p => p.Cliente)
                .Include(p => p.Direccion)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Obtiene todos los pedidos de un cliente especifico
        public async Task<List<PedidoCabecera>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.PedidosCabecera
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

        // Registra un nuevo pedido con sus detalles en la base de datos
        public async Task<PedidoCabecera> AddAsync(PedidoCabecera pedido)
        {
            pedido.CreatedAt = DateTime.Now;
            _context.PedidosCabecera.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        // Actualiza el estado u otros datos de un pedido existente
        public async Task<PedidoCabecera> UpdateAsync(PedidoCabecera pedido)
        {
            pedido.UpdatedAt = DateTime.Now;
            _context.PedidosCabecera.Update(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        // Soft delete: marca el pedido como eliminado sin borrarlo fisicamente
        public async Task<bool> DeleteAsync(int id)
        {
            PedidoCabecera? pedido = await GetByIdAsync(id);
            if (pedido == null) return false;

            pedido.Deleted = true;
            pedido.UpdatedAt = DateTime.Now;
            _context.PedidosCabecera.Update(pedido);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
