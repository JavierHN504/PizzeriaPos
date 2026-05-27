namespace PizzeriaPos.Api.DTOs
{
    // Datos que el cliente envia para registrar un nuevo usuario
    public class RegisterDTO
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
