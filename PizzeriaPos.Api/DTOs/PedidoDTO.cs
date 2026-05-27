namespace PizzeriaPos.Api.DTOs
{
    // Datos para crear un pedido completo (cabecera + detalles)
    public class PedidoDTO
    {
        public int ClienteId { get; set; }
        public int? DireccionId { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string? Observaciones { get; set; }
        public List<PedidoDetalleDTO> Detalles { get; set; } = new List<PedidoDetalleDTO>();
    }

    // Datos de cada linea de detalle dentro de un pedido
    public class PedidoDetalleDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}