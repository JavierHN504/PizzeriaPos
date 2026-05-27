using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzeriaPos.Core.Entities;

namespace PizzeriaPos.Core.Interfaces
{
    // Define las operaciones disponibles sin importar como se implementan.
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetAllAsync(); // Obtener todos los clientes activos
        Task<Cliente?> GetByIdAsync(int id); // Obtener un cliente por su ID
        Task<Cliente> AddAsync(Cliente cliente); // Agregar un nuevo cliente
        Task<Cliente> UpdateAsync(Cliente cliente); // Actualizar un cliente existente
        Task<bool> DeleteAsync(int id); // Eliminar logicamente un cliente (soft delete)
    }
}
