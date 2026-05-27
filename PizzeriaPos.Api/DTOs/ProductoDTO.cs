namespace PizzeriaPos.Api.DTOs
{
    // Datos que el cliente envia para crear o actualizar un producto
    public class ProductoDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public bool Disponible { get; set; } = true;
    }
}