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
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> GetAllAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Producto> AddAsync(Producto producto)
        {
            producto.CreatedAt = DateTime.Now;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto> UpdateAsync(Producto producto)
        {
            producto.UpdatedAt = DateTime.Now;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Producto? producto = await GetByIdAsync(id);

            if (producto == null) return false;

            // Soft delete: solo marcamos como eliminado
            producto.Deleted = true;
            producto.UpdatedAt = DateTime.Now;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
