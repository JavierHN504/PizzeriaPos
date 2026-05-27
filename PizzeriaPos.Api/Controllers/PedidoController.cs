using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaPos.Api.DTOs;
using PizzeriaPos.Core.Entities;
using PizzeriaPos.Core.Interfaces;

namespace PizzeriaPos.Api.Controllers
{
    // Controller de Pedidos. Requiere JWT para todos los endpoints.
    [Authorize]
    public class PedidoController : BaseController
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoController(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        // GET api/Pedido
        // Obtiene todos los pedidos activos con sus detalles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            return ResponseOk(pedidos, "Pedidos obtenidos exitosamente.");
        }

        // GET api/Pedido/{id}
        // Obtiene un pedido por ID con todos sus detalles
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                return ResponseNotFound($"Pedido con ID {id} no encontrado.");

            return ResponseOk(pedido, "Pedido encontrado.");
        }

        // GET api/Pedido/cliente/{clienteId}
        // Obtiene todos los pedidos de un cliente especifico
        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> GetByCliente(int clienteId)
        {
            var pedidos = await _pedidoRepository.GetByClienteIdAsync(clienteId);
            return ResponseOk(pedidos, "Pedidos del cliente obtenidos exitosamente.");
        }

        // POST api/Pedido
        // Registra un nuevo pedido con sus detalles
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoDTO dto)
        {
            var pedido = new PedidoCabecera
            {
                ClienteId = dto.ClienteId,
                DireccionId = dto.DireccionId,
                Estado = dto.Estado,
                Observaciones = dto.Observaciones,
                Detalles = dto.Detalles.Select(d => new PedidoDetalle
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            };

            // Calcular el total sumando los subtotales de cada detalle
            pedido.Total = dto.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);

            var resultado = await _pedidoRepository.AddAsync(pedido);
            return ResponseCreated(resultado, "Pedido creado exitosamente.");
        }

        // PUT api/Pedido/{id}
        // Actualiza el estado de un pedido
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PedidoDTO dto)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                return ResponseNotFound($"Pedido con ID {id} no encontrado.");

            pedido.Estado = dto.Estado;
            pedido.Observaciones = dto.Observaciones;
            pedido.DireccionId = dto.DireccionId;

            var resultado = await _pedidoRepository.UpdateAsync(pedido);
            return ResponseOk(resultado, "Pedido actualizado exitosamente.");
        }

        // DELETE api/Pedido/{id}
        // Elimina logicamente un pedido
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _pedidoRepository.DeleteAsync(id);
            if (!resultado)
                return ResponseNotFound($"Pedido con ID {id} no encontrado.");

            return ResponseOk(resultado, "Pedido eliminado exitosamente.");
        }
    }
}