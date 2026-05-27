using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaPos.Api.DTOs;
using PizzeriaPos.Core.Entities;
using PizzeriaPos.Core.Interfaces;

namespace PizzeriaPos.Api.Controllers
{
    // Controller de Productos. Requiere JWT para todos los endpoints.
    [Authorize]
    public class ProductoController : BaseController
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        // GET api/Producto
        // Obtiene todos los productos activos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _productoRepository.GetAllAsync();
            return ResponseOk(productos, "Productos obtenidos exitosamente.");
        }

        // GET api/Producto/{id}
        // Obtiene un producto por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
                return ResponseNotFound($"Producto con ID {id} no encontrado.");

            return ResponseOk(producto, "Producto encontrado.");
        }

        // POST api/Producto
        // Crea un nuevo producto
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoDTO dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Categoria = dto.Categoria,
                Disponible = dto.Disponible
            };

            var resultado = await _productoRepository.AddAsync(producto);
            return ResponseCreated(resultado, "Producto creado exitosamente.");
        }

        // PUT api/Producto/{id}
        // Actualiza un producto existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoDTO dto)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null)
                return ResponseNotFound($"Producto con ID {id} no encontrado.");

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;
            producto.Categoria = dto.Categoria;
            producto.Disponible = dto.Disponible;

            var resultado = await _productoRepository.UpdateAsync(producto);
            return ResponseOk(resultado, "Producto actualizado exitosamente.");
        }

        // DELETE api/Producto/{id}
        // Elimina logicamente un producto
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _productoRepository.DeleteAsync(id);
            if (!resultado)
                return ResponseNotFound($"Producto con ID {id} no encontrado.");

            return ResponseOk(resultado, "Producto eliminado exitosamente.");
        }
    }
}