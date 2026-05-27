using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaPos.Api.DTOs;
using PizzeriaPos.Core.Entities;
using PizzeriaPos.Core.Interfaces;

namespace PizzeriaPos.Api.Controllers
{
    // Controller de Clientes. Requiere JWT para todos los endpoints.
    [Authorize]
    public class ClienteController : BaseController
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        // GET api/Cliente
        // Obtiene todos los clientes activos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return ResponseOk(clientes, "Clientes obtenidos exitosamente.");
        }

        // GET api/Cliente/{id}
        // Obtiene un cliente por ID con sus direcciones
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
                return ResponseNotFound($"Cliente con ID {id} no encontrado.");

            return ResponseOk(cliente, "Cliente encontrado.");
        }

        // POST api/Cliente
        // Crea un nuevo cliente
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteDTO dto)
        {
            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Telefono = dto.Telefono,
                Email = dto.Email
            };

            var resultado = await _clienteRepository.AddAsync(cliente);
            return ResponseCreated(resultado, "Cliente creado exitosamente.");
        }

        // PUT api/Cliente/{id}
        // Actualiza un cliente existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteDTO dto)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
                return ResponseNotFound($"Cliente con ID {id} no encontrado.");

            cliente.Nombre = dto.Nombre;
            cliente.Apellido = dto.Apellido;
            cliente.Telefono = dto.Telefono;
            cliente.Email = dto.Email;

            var resultado = await _clienteRepository.UpdateAsync(cliente);
            return ResponseOk(resultado, "Cliente actualizado exitosamente.");
        }

        // DELETE api/Cliente/{id}
        // Elimina logicamente un cliente
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _clienteRepository.DeleteAsync(id);
            if (!resultado)
                return ResponseNotFound($"Cliente con ID {id} no encontrado.");

            return ResponseOk(resultado, "Cliente eliminado exitosamente.");
        }
    }
}