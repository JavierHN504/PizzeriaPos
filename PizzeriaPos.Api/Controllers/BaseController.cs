using Microsoft.AspNetCore.Mvc;
using PizzeriaPos.Api.Responses;

namespace PizzeriaPos.Api.Controllers
{
    // Controlador base del que heredan todos los controllers de la API.
    // Centraliza los metodos de respuesta para mantener formato JSON consistente.
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        // Respuesta 200 OK con datos
        protected IActionResult ResponseOk<T>(T data, string message = "Operacion exitosa")
            => Ok(ApiResponse<T>.Ok(data, message));

        // Respuesta 201 Created con datos
        protected IActionResult ResponseCreated<T>(T data, string message = "Creado exitosamente")
            => StatusCode(201, ApiResponse<T>.Ok(data, message));

        // Respuesta 400 Bad Request con mensaje de error
        protected IActionResult ResponseBadRequest(string message)
            => BadRequest(ApiResponse<object>.Error(message));

        // Respuesta 404 Not Found con mensaje de error
        protected IActionResult ResponseNotFound(string message)
            => NotFound(ApiResponse<object>.Error(message));

        // Respuesta 409 Conflict cuando hay datos duplicados
        protected IActionResult ResponseConflict(string message)
            => Conflict(ApiResponse<object>.Error(message));

        // Respuesta 401 Unauthorized
        protected IActionResult ResponseUnauthorized(string message = "No autorizado")
            => Unauthorized(ApiResponse<object>.Error(message));
    }
}