using PizzeriaPos.Api;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios del proyecto (repositorios, DbContext, etc.)
builder.Services.AddProjectServices(builder.Configuration);

// Configurar autenticacion JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configurar Swagger con soporte JWT
builder.Services.AddSwaggerWithJwt();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Swagger disponible en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Autenticacion debe ir antes que autorizacion
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();