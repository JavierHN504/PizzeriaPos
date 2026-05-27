namespace PizzeriaPos.Api.Responses
{
    // Estructura estandar de respuesta para todos los endpoints de la API.
    // Asi todas las respuestas tengan el mismo formato JSON.
    public class ApiResponse<T>
    {
        public bool Success { get; set; }      // true si la operacion fue exitosa
        public string Message { get; set; } = string.Empty; // Mensaje descriptivo del resultado
        public T? Data { get; set; }           // Datos retornados (null si hubo error)

        // Respuesta exitosa con datos
        public static ApiResponse<T> Ok(T data, string message = "Operacion exitosa")
            => new() { Success = true, Message = message, Data = data };

        // Respuesta de error sin datos
        public static ApiResponse<T> Error(string message)
            => new() { Success = false, Message = message, Data = default };
    }
}