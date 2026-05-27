using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzeriaPos.Core.Entities;

namespace PizzeriaPos.Core.Interfaces
{
    // Define las operaciones disponibles sin importar como se implementan.
    public interface IDireccionRepository
    {
        Task<List<Direccion>> GetByClienteIdAsync(int clienteId); // Obtener todas las direcciones de un cliente
        Task<Direccion?> GetByIdAsync(int id); // Obtener una direccion por su ID
        Task<Direccion> AddAsync(Direccion direccion); // Agregar una nueva direccion
        Task<Direccion> UpdateAsync(Direccion direccion); // Actualizar una direccion existente
        Task<bool> DeleteAsync(int id); // Eliminar logicamente una direccion (soft delete)
    }
}