using Microsoft.EntityFrameworkCore;
using PizzeriaPos.Core.Entities;
using PizzeriaPos.Infrastructure.Data;
using PizzeriaPos.Infrastructure.Repositories;

namespace PizzeriaPos.Tests
{
    public class ProductoRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_DebeAgregarProductoCorrectamente()
        {
            // Arrange
            using var context = GetDbContext();
            var repo = new ProductoRepository(context);
            var producto = new Producto
            {
                Nombre = "Pizza Margarita",
                Descripcion = "Pizza clasica con tomate y mozzarella",
                Precio = 150.00m,
                Categoria = "Pizza",
                Disponible = true
            };

            // Act
            var resultado = await repo.AddAsync(producto);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Id > 0);
            Assert.Equal("Pizza Margarita", resultado.Nombre);
        }

        [Fact]
        public async Task GetAllAsync_DebeRetornarProductosNoEliminados()
        {
            // Arrange
            using var context = GetDbContext();
            var repo = new ProductoRepository(context);

            await repo.AddAsync(new Producto { Nombre = "Pizza Hawaiana", Precio = 160.00m, Categoria = "Pizza" });
            await repo.AddAsync(new Producto { Nombre = "Coca Cola", Precio = 35.00m, Categoria = "Bebida" });

            // Act
            var productos = await repo.GetAllAsync();

            // Assert
            Assert.Equal(2, productos.Count);
        }

        [Fact]
        public async Task GetByIdAsync_DebeRetornarProductoCorrecto()
        {
            // Arrange
            using var context = GetDbContext();
            var repo = new ProductoRepository(context);
            var nuevo = await repo.AddAsync(new Producto { Nombre = "Pizza BBQ", Precio = 175.00m, Categoria = "Pizza" });

            // Act
            var resultado = await repo.GetByIdAsync(nuevo.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Pizza BBQ", resultado.Nombre);
        }

        [Fact]
        public async Task UpdateAsync_DebeActualizarProductoCorrectamente()
        {
            // Arrange
            using var context = GetDbContext();
            var repo = new ProductoRepository(context);
            var nuevo = await repo.AddAsync(new Producto { Nombre = "Pizza Pepperoni", Precio = 170.00m, Categoria = "Pizza" });

            // Act
            nuevo.Precio = 185.00m;
            var actualizado = await repo.UpdateAsync(nuevo);

            // Assert
            Assert.Equal(185.00m, actualizado.Precio);
            Assert.NotNull(actualizado.UpdatedAt);
        }

        [Fact]
        public async Task DeleteAsync_DebeMarcaProductoComoEliminado()
        {
            // Arrange
            using var context = GetDbContext();
            var repo = new ProductoRepository(context);
            var nuevo = await repo.AddAsync(new Producto { Nombre = "Pizza Veggie", Precio = 145.00m, Categoria = "Pizza" });

            // Act
            var resultado = await repo.DeleteAsync(nuevo.Id);
            var productosBD = await repo.GetAllAsync();

            // Assert
            Assert.True(resultado);
            Assert.Empty(productosBD); // El soft delete lo oculta del GetAll
        }
    }
}