namespace PizzeriaPos.Api.DTOs
{
    // Datos que el cliente envia para crear o actualizar un cliente
    public class ClienteDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }
}