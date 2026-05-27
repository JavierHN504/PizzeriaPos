using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaPos.Api.DTOs;
using PizzeriaPos.Core.Entities;
using PizzeriaPos.Core.Interfaces;

namespace PizzeriaPos.Api.Controllers
{
    // Controller de Direcciones. Requiere JWT para todos los endpoints.
    [Authorize]
    public class DireccionController : BaseController
    {
        private readonly IDireccionRepository _direccionRepository;

        public DireccionController(IDireccionRepository direccionRepository)
        {
            _direccionRepository = direccionRepository;
        }

        // GET api/Direccion/cliente/{clienteId}
        // Obtiene todas las direcciones de un cliente especifico
        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> GetByCliente(int clienteId)
        {
            var direcciones = await _direccionRepository.GetByClienteIdAsync(clienteId);
            return ResponseOk(direcciones, "Direcciones obtenidas exitosamente.");
        }

        // GET api/Direccion/{id}
        // Obtiene una direccion por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var direccion = await _direccionRepository.GetByIdAsync(id);
            if (direccion == null)
                return ResponseNotFound($"Direccion con ID {id} no encontrada.");

            return ResponseOk(direccion, "Direccion encontrada.");
        }

        // POST api/Direccion
        // Crea una nueva direccion para un cliente
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DireccionDTO dto)
        {
            var direccion = new Direccion
            {
                Calle = dto.Calle,
                Ciudad = dto.Ciudad,
                Departamento = dto.Departamento,
                CodigoPostal = dto.CodigoPostal,
                EsPrincipal = dto.EsPrincipal,
                ClienteId = dto.ClienteId
            };

            var resultado = await _direccionRepository.AddAsync(direccion);
            return ResponseCreated(resultado, "Direccion creada exitosamente.");
        }

        // PUT api/Direccion/{id}
        // Actualiza una direccion existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DireccionDTO dto)
        {
            var direccion = await _direccionRepository.GetByIdAsync(id);
            if (direccion == null)
                return ResponseNotFound($"Direccion con ID {id} no encontrada.");

            direccion.Calle = dto.Calle;
            direccion.Ciudad = dto.Ciudad;
            direccion.Departamento = dto.Departamento;
            direccion.CodigoPostal = dto.CodigoPostal;
            direccion.EsPrincipal = dto.EsPrincipal;
            direccion.ClienteId = dto.ClienteId;

            var resultado = await _direccionRepository.UpdateAsync(direccion);
            return ResponseOk(resultado, "Direccion actualizada exitosamente.");
        }

        // DELETE api/Direccion/{id}
        // Elimina logicamente una direccion
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _direccionRepository.DeleteAsync(id);
            if (!resultado)
                return ResponseNotFound($"Direccion con ID {id} no encontrada.");

            return ResponseOk(resultado, "Direccion eliminada exitosamente.");
        }
    }
}