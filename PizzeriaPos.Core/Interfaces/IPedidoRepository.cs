using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzeriaPos.Core.Entities;

namespace PizzeriaPos.Core.Interfaces
{
    public interface IPedidoRepository
    {
        Task<List<PedidoCabecera>> GetAllAsync();
        Task<PedidoCabecera?> GetByIdAsync(int id);
        Task<List<PedidoCabecera>> GetByClienteIdAsync(int clienteId);
        Task<PedidoCabecera> AddAsync(PedidoCabecera pedido);
        Task<PedidoCabecera> UpdateAsync(PedidoCabecera pedido);
        Task<bool> DeleteAsync(int id);
    }
}
