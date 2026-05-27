using System.Security.Cryptography;
using System.Text;

namespace PizzeriaPos.Api.Helpers
{
    // Utilidad para hashear y verificar contraseñas usando HMACSHA512.
    // Las contraseñas NO se guardan en texto plano en la base de datos.
    public static class PasswordHelper
    {
        // Clave secreta fija para el hash (en produccion deberia estar en configuracion)
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("PizzeriaPosSecretKey2024!");

        // Genera el hash de una contraseña en texto plano
        public static string Hash(string password)
        {
            using var hmac = new HMACSHA512(Key);
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = hmac.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // Verifica si una contraseña en texto plano coincide con su hash almacenado
        public static bool Verificar(string password, string hash)
        {
            return Hash(password) == hash;
        }
    }
}
