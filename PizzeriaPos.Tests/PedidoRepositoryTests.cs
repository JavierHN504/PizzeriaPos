using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzeriaPos.Core.Entities;
using PizzeriaPos.Infrastructure.Data;
using PizzeriaPos.Infrastructure.Repositories;

namespace PizzeriaPos.Tests
{
    public class PedidoRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_DebeAgregarClienteCorrectamente()
        {
            using var context = GetDbContext();
            var repo = new ClienteRepository(context);

            var cliente = new Cliente
            {
                Nombre = "Javier",
                Apellido = "Diaz",
                Telefono = "88993514",
                Email = "javierjosuediazrio26@email.com"
            };

            var resultado = await repo.AddAsync(cliente);

            Assert.NotNull(resultado);
            Assert.True(resultado.Id > 0);
            Assert.Equal("Javier", resultado.Nombre);
        }

        [Fact]
        public async Task AddAsync_DebeAgregarDireccionACliente()
        {
            using var context = GetDbContext();
            var clienteRepo = new ClienteRepository(context);
            var direccionRepo = new DireccionRepository(context);

            var cliente = await clienteRepo.AddAsync(new Cliente { Nombre = "Maria", Apellido = "Lopez" });

            var direccion = new Direccion
            {
                Calle = "Colonia Las Brisas, Bloque 3",
                Ciudad = "Tegucigalpa",
                Departamento = "Francisco Morazan",
                EsPrincipal = true,
                ClienteId = cliente.Id
            };

            var resultado = await direccionRepo.AddAsync(direccion);

            Assert.NotNull(resultado);
            Assert.Equal(cliente.Id, resultado.ClienteId);
            Assert.True(resultado.EsPrincipal);
        }

        [Fact]
        public async Task AddAsync_DebeAgregarPedidoConDetalle()
        {
            using var context = GetDbContext();
            var clienteRepo = new ClienteRepository(context);
            var productoRepo = new ProductoRepository(context);
            var pedidoRepo = new PedidoRepository(context);

            var cliente = await clienteRepo.AddAsync(new Cliente { Nombre = "Carlos", Apellido = "Gomez" });
            var producto = await productoRepo.AddAsync(new Producto { Nombre = "Pizza BBQ", Precio = 180.00m, Categoria = "Pizza" });

            var pedido = new PedidoCabecera
            {
                ClienteId = cliente.Id,
                Estado = "Pendiente",
                Total = 360.00m,
                Detalles = new List<PedidoDetalle>
                {
                    new PedidoDetalle
                    {
                        ProductoId = producto.Id,
                        Cantidad = 2,
                        PrecioUnitario = 180.00m
                    }
                }
            };

            var resultado = await pedidoRepo.AddAsync(pedido);

            Assert.NotNull(resultado);
            Assert.Equal(cliente.Id, resultado.ClienteId);
            Assert.Single(resultado.Detalles);
        }

        [Fact]
        public async Task GetByClienteIdAsync_DebeRetornarPedidosDelCliente()
        {
            using var context = GetDbContext();
            var clienteRepo = new ClienteRepository(context);
            var pedidoRepo = new PedidoRepository(context);

            var cliente = await clienteRepo.AddAsync(new Cliente { Nombre = "Ana", Apellido = "Martinez" });

            await pedidoRepo.AddAsync(new PedidoCabecera { ClienteId = cliente.Id, Estado = "Completado", Total = 150.00m });
            await pedidoRepo.AddAsync(new PedidoCabecera { ClienteId = cliente.Id, Estado = "Pendiente", Total = 200.00m });

            var pedidos = await pedidoRepo.GetByClienteIdAsync(cliente.Id);

            Assert.Equal(2, pedidos.Count);
        }

        [Fact]
        public async Task DeleteAsync_DebeMarcaPedidoComoEliminado()
        {
            using var context = GetDbContext();
            var clienteRepo = new ClienteRepository(context);
            var pedidoRepo = new PedidoRepository(context);

            var cliente = await clienteRepo.AddAsync(new Cliente { Nombre = "Luis", Apellido = "Torres" });
            var pedido = await pedidoRepo.AddAsync(new PedidoCabecera { ClienteId = cliente.Id, Estado = "Pendiente", Total = 175.00m });

            var resultado = await pedidoRepo.DeleteAsync(pedido.Id);
            var pedidos = await pedidoRepo.GetAllAsync();

            Assert.True(resultado);
            Assert.Empty(pedidos);
        }
    }
}
