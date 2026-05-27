using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzeriaPos.Core.Entities;

namespace PizzeriaPos.Core.Interfaces
{
    // Define las operaciones disponibles sin importar como se implementan.
    public interface IPedidoRepository
    {
        Task<List<PedidoCabecera>> GetAllAsync(); // Obtener todos los pedidos activos con sus detalles
        Task<PedidoCabecera?> GetByIdAsync(int id); // Obtener un pedido por su ID con sus detalles
        Task<List<PedidoCabecera>> GetByClienteIdAsync(int clienteId); // Obtener pedidos de un cliente especifico
        Task<PedidoCabecera> AddAsync(PedidoCabecera pedido); // Registrar un nuevo pedido
        Task<PedidoCabecera> UpdateAsync(PedidoCabecera pedido); // Actualizar un pedido existente
        Task<bool> DeleteAsync(int id); // Eliminar logicamente un pedido (soft delete)
    }
}
