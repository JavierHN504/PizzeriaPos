namespace PizzeriaPos.Api.DTOs
{
    // Datos que el cliente envia para crear o actualizar una direccion
    public class DireccionDTO
    {
        public string Calle { get; set; } = string.Empty;
        public string? Ciudad { get; set; }
        public string? Departamento { get; set; }
        public string? CodigoPostal { get; set; }
        public bool EsPrincipal { get; set; } = false;
        public int ClienteId { get; set; }
    }
}