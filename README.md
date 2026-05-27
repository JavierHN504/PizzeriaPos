# 🍕 Pizzeria POS — Sistema de Punto de Venta

Sistema POS completo para una pizzería desarrollado en .NET 8 como prueba técnica para Albatros.

---

## 🏗️ Arquitectura del Proyecto
PizzeriaPos/
├── PizzeriaPos.Core/           → Entidades e interfaces (contratos)
├── PizzeriaPos.Infrastructure/ → DbContext, repositorios, acceso a datos
├── PizzeriaPos.Api/            → API REST con ASP.NET Core + JWT
├── PizzeriaPos.WinForms/       → Aplicacion de escritorio Windows Forms
├── PizzeriaPos.Tests/          → Pruebas unitarias con xUnit
└── database/                   → Scripts SQL de creacion e inicializacion

---

## ⚙️ Requisitos Previos

- .NET SDK 8.0 o superior
- SQL Server (local o remoto)
- SQL Server Management Studio (SSMS)
- Visual Studio 2022

---

## 🗄️ Configuracion de la Base de Datos

1. Abrir SSMS y conectarse al servidor local.
2. Ejecutar los scripts en orden:

```sql
-- Paso 1: Crear BD y tabla Productos
database/01_create_database.sql
-- Paso 2: Crear tablas Clientes, Direcciones y Pedidos
database/02_create_tables.sql
-- Paso 3: Crear tabla Usuarios
database/03_create_usuarios.sql
```

---

## 🔧 Configuracion de la API

1. Abrir `PizzeriaPos.Api/appsettings.json`
2. Verificar la cadena de conexion:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PizzeriaPosDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. Si su servidor SQL tiene un nombre diferente, reemplazar `localhost` por el nombre de su instancia.

---

## ▶️ Como Ejecutar

### Opcion 1 — Visual Studio 2022

1. Abrir `PizzeriaPos.sln` en Visual Studio.
2. Clic derecho en la Solucion → Configurar proyectos de inicio → Proyectos de inicio multiples.
3. Poner `PizzeriaPos.Api` y `PizzeriaPos.WinForms` en **Iniciar**.
4. Presionar **F5**.

### Opcion 2 — PowerShell (dos terminales)

Terminal 1 — API:
```powershell
cd PizzeriaPos.Api
dotnet run
```

Terminal 2 — WinForms:
```powershell
cd PizzeriaPos.WinForms
dotnet run
```

---

## 🔐 Primer Uso

1. Al abrir la aplicacion aparece la pantalla de Login.
2. Hacer clic en **¿No tienes cuenta? Registrate**.
3. Crear un usuario con nombre, usuario y contraseña.
4. Iniciar sesion con las credenciales creadas.

---

## 📋 Funcionalidades

### Productos
- Listar todos los productos activos
- Crear nuevo producto
- Editar producto existente (clic en la tabla)
- Eliminar producto (soft delete)

### Clientes
- Listar todos los clientes activos
- Crear nuevo cliente
- Editar cliente existente (clic en la tabla)
- Eliminar cliente (soft delete)

### Pedidos
- Listar todos los pedidos con cliente, estado y total
- Crear nuevo pedido seleccionando cliente y productos
- Ver detalle del pedido haciendo clic en la lista

### Autenticacion
- Registro de nuevos usuarios con contraseña hasheada (HMACSHA512)
- Login con generacion de token JWT
- Todos los endpoints de la API requieren JWT valido

---

## 🧪 Pruebas Unitarias

```powershell
dotnet test
```

10 pruebas unitarias cubriendo operaciones CRUD de Producto, Cliente, Direccion y Pedido usando base de datos en memoria.

---

## 📡 API REST — Swagger

Con la API corriendo, acceder a:

https://localhost:7039/swagger

Endpoints disponibles:
- `POST /api/Auth/register` — Registrar usuario
- `POST /api/Auth/login` — Iniciar sesion y obtener JWT
- `GET/POST/PUT/DELETE /api/Producto` — CRUD de productos
- `GET/POST/PUT/DELETE /api/Cliente` — CRUD de clientes
- `GET/POST/PUT/DELETE /api/Direccion` — CRUD de direcciones
- `GET/POST/PUT/DELETE /api/Pedido` — CRUD de pedidos

---

## 🛠️ Tecnologias Utilizadas

- **Backend:** ASP.NET Core 8, Entity Framework Core 9, JWT Bearer
- **Frontend:** Windows Forms .NET 8
- **Base de datos:** SQL Server
- **Pruebas:** xUnit, EF Core InMemory
- **Documentacion API:** Swagger / OpenAPI

---

## 👨‍💻 Desarrollador

**Javier Josue Diaz Rios**  
Prueba tecnica — Albatros  
2026