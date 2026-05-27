using Newtonsoft.Json;
using System.Text;

namespace PizzeriaPos.WinForms.Services
{
    // Servicio central de comunicacion con la API REST.
    // Todos los formularios usan esta clase para hacer peticiones HTTP.
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7039/api/";

        // Token JWT del usuario autenticado (se asigna al hacer login)
        public static string? Token { get; set; }

        public ApiService()
        {
            // Ignorar errores de certificado SSL en desarrollo
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        // Agrega el token JWT al header de cada peticion
        private void SetAuthHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        }

        // Peticion GET — obtener datos
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return default;

            // La API devuelve { success, message, data }
            // Necesitamos extraer solo el campo "data"
            var wrapper = JsonConvert.DeserializeObject<ApiResult<T>>(content);
            return wrapper != null ? wrapper.Data : default;
        }

        // Peticion POST — crear datos
        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            SetAuthHeader();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<ApiResult<T>>(responseContent);
                throw new Exception(error?.Message ?? "Error en la peticion.");
            }

            var result = JsonConvert.DeserializeObject<ApiResult<T>>(responseContent);
            return result != null ? result.Data : default;
        }

        // Peticion PUT — actualizar datos
        public async Task<T?> PutAsync<T>(string endpoint, object data)
        {
            SetAuthHeader();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<ApiResult<T>>(responseContent);
                throw new Exception(error?.Message ?? "Error en la peticion.");
            }

            var result = JsonConvert.DeserializeObject<ApiResult<T>>(responseContent);
            return result != null ? result.Data : default;
        }

        // Peticion DELETE — eliminar datos
        public async Task<bool> DeleteAsync(string endpoint)
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
    }

    // Clase auxiliar para deserializar la respuesta estandar de la API
    public class ApiResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}