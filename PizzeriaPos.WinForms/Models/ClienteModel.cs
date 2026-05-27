using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaPos.WinForms.Models
{
    // Modelo que representa un Cliente recibido desde la API
    public class ClienteModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }

    // Modelo que representa un Producto recibido desde la API
    public class ProductoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public bool Disponible { get; set; } // true = disponible para ordenar
    }

    // Modelo que representa la cabecera de un Pedido recibido desde la API
    public class PedidoCabeceraModel
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public ClienteModel? Cliente { get; set; } // Datos del cliente del pedido
        public string Estado { get; set; } = string.Empty; // Pendiente, En Proceso, Completado
        public decimal Total { get; set; }
        public string? Observaciones { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PedidoDetalleModel> Detalles { get; set; } = new(); // Productos del pedido
    }

    // Modelo que representa una linea de detalle dentro de un pedido
    public class PedidoDetalleModel
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public ProductoModel? Producto { get; set; } // Producto ordenado
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}