using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PizzeriaPos.Api.DTOs;
using PizzeriaPos.Api.Helpers;
using PizzeriaPos.Core.Entities;
using PizzeriaPos.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PizzeriaPos.Api.Controllers
{
    // Controller de autenticacion.
    // Maneja registro de usuarios y login.
    public class AuthController : BaseController
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        // POST api/Auth/register
        // Registra un nuevo usuario en el sistema
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            // Verificar que el nombre de usuario no este en uso
            if (await _usuarioRepository.ExisteNombreUsuarioAsync(dto.NombreUsuario))
                return ResponseConflict($"El nombre de usuario '{dto.NombreUsuario}' ya esta en uso.");

            // Crear el usuario con la contrasena hasheada
            var usuario = new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                NombreCompleto = dto.NombreCompleto,
                PasswordHash = PasswordHelper.Hash(dto.Password),
                Activo = true
            };

            var resultado = await _usuarioRepository.AddAsync(usuario);

            return ResponseCreated(new
            {
                resultado.Id,
                resultado.NombreUsuario,
                resultado.NombreCompleto
            }, "Usuario registrado exitosamente.");
        }

        // POST api/Auth/login
        // Valida credenciales y retorna un token JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            // Buscar el usuario por nombre de usuario
            var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(dto.NombreUsuario);

            if (usuario == null)
                return ResponseUnauthorized("Credenciales invalidas.");

            // Verificar que la contrasena sea correcta
            if (!PasswordHelper.Verificar(dto.Password, usuario.PasswordHash))
                return ResponseUnauthorized("Credenciales invalidas.");

            // Generar el token JWT
            var token = GenerarToken(usuario);

            return ResponseOk(new
            {
                Token = token,
                usuario.NombreUsuario,
                usuario.NombreCompleto,
                Expiracion = DateTime.Now.AddMinutes(
                    double.Parse(_configuration["Jwt:ExpirationMinutes"]!))
            }, "Login exitoso.");
        }

        // Genera un token JWT con los datos del usuario
        private string GenerarToken(Usuario usuario)
        {
            var secretKey = _configuration["Jwt:SecretKey"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims: informacion del usuario que va dentro del token
            var claims = new[]
            {
                new Claim("Id", usuario.Id.ToString()),
                new Claim("NombreUsuario", usuario.NombreUsuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    double.Parse(_configuration["Jwt:ExpirationMinutes"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}