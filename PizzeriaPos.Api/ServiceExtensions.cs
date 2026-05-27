using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PizzeriaPos.Core.Interfaces;
using PizzeriaPos.Infrastructure.Data;
using PizzeriaPos.Infrastructure.Repositories;
using System.Text;

namespace PizzeriaPos.Api
{
    // Clase de extension que registra todos los servicios de la aplicacion.
    public static class ServiceExtensions
    {
        public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Conexion a SQL Server via Entity Framework
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Registro de repositorios (inyeccion de dependencias)
            // Cada vez que un controller pida IProductoRepository, recibira ProductoRepository
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IDireccionRepository, DireccionRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Clave secreta para firmar y validar tokens JWT
            var secretKey = configuration["Jwt:SecretKey"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,  // Validar que el token fue firmado con nuestra clave
                        IssuerSigningKey = key,
                        ValidateIssuer = true,            // Validar quien emitio el token
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = true,          // Validar para quien es el token
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateLifetime = true,          // Validar que el token no haya expirado
                        ClockSkew = TimeSpan.Zero         // Sin margen de tolerancia en expiracion
                    };
                });
        }

        public static void AddSwaggerWithJwt(this IServiceCollection services)
        {
            // Configurar Swagger con soporte para autenticacion JWT
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pizzeria POS API",
                    Version = "v1",
                    Description = "API REST para el sistema POS de la pizzeria"
                });

                // Boton de autorizacion en Swagger para ingresar el token JWT
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT con el prefijo Bearer. Ejemplo: Bearer {token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }
    }
}