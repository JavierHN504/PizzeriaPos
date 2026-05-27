using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzeriaPos.Core.Entities;

namespace PizzeriaPos.Core.Interfaces
{
    public interface IDireccionRepository
    {
        Task<List<Direccion>> GetByClienteIdAsync(int clienteId);
        Task<Direccion?> GetByIdAsync(int id);
        Task<Direccion> AddAsync(Direccion direccion);
        Task<Direccion> UpdateAsync(Direccion direccion);
        Task<bool> DeleteAsync(int id);
    }
}