namespace PizzeriaPos.Api.DTOs
{
    // Datos que el cliente envia para iniciar sesion
    public class LoginDTO
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}