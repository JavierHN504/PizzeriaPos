using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzeriaPos.Core.Entities;

namespace PizzeriaPos.Core.Interfaces
{
    // Define las operaciones disponibles sin importar como se implementan.
    public interface IProductoRepository
    {
        Task<List<Producto>> GetAllAsync(); // Obtener todos los productos activos
        Task<Producto?> GetByIdAsync(int id); // Obtener un producto por su ID
        Task<Producto> AddAsync(Producto producto); // Agregar un nuevo producto
        Task<Producto> UpdateAsync(Producto producto); // Actualizar un producto existente
        Task<bool> DeleteAsync(int id); // Eliminar logicamente un producto (soft delete)
    }
}

