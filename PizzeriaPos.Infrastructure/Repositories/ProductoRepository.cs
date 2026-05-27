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
    // Maneja el catalogo de productos del menu de la pizzeria.
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context; // Contexto de base de datos inyectado

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todos los productos activos y disponibles
        public async Task<List<Producto>> GetAllAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        // Obtiene un producto por su ID
        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
        }

        // Agrega un nuevo producto al catalogo
        public async Task<Producto> AddAsync(Producto producto)
        {
            producto.CreatedAt = DateTime.Now;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        // Actualiza los datos de un producto existente
        public async Task<Producto> UpdateAsync(Producto producto)
        {
            producto.UpdatedAt = DateTime.Now;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        // Soft delete: marca el producto como eliminado sin borrarlo fisicamente
        public async Task<bool> DeleteAsync(int id)
        {
            Producto? producto = await GetByIdAsync(id);
            if (producto == null) return false;

            producto.Deleted = true;
            producto.UpdatedAt = DateTime.Now;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}