---------------------------------- DSW-TP1 - API RESTful para Gestión de Órdenes ---------------------------------------

Este proyecto es una API RESTful desarrollada con C# y ASP.NET Core para gestionar órdenes en una plataforma de comercio electrónico.

Requisitos

- Visual Studio o Visual Studio Code
- Entity Framework Core
- [.NET 8 SDK]
- [SQL Server]

Configuración y Ejecución

1. Clonar el repositorio
   
En la consola ejecutar los siguientes comandos

   -> git clone "https://github.com/MarcosB007/DSW-TP1.git"

2. Abrir el proyecto en Visual Studio 2022:

 * Ir a Archivo > Abrir > Proyecto o Solución...
 * Seleccionar el archivo DSW-TP1.sln dentro del repositorio clonado.

3. Verificar o ajustar la cadena de conexión en appsettings.json:

 * Ubicado en el proyecto principal (DSW-TP1/appsettings.json).
 * Editar el archivo appsettings.json para que coincida con tu instancia de SQL Server:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DSW_TP1_DB;Trusted_Connection=True;TrustServerCertificate=True"
}

4. Aplicar las migraciones para crear la base de datos:

 * Abrir la Consola del Administrador de Paquetes (PMC) desde: Herramientas > Administrador de paquetes NuGet > Consola del Administrador de paquetes
 * Ejecutar el siguiente comando: Update-Database

5. Establecer el proyecto DSW-TP1 como proyecto de inicio:

 * Click derecho sobre el proyecto > Establecer como proyecto de inicio.

6. Ejecutar la aplicación:

 * Presionar F5 o click en el botón verde de "Iniciar".
 * Se abrirá el navegador con Swagger en una URL como: https://localhost:7118/swagger

7. Probar los endpoints desde Swagger:

 * Allí podrá ver todos los endpoints disponibles (GET, POST, PUT) y probar el comportamiento del sistema.

------ Endpoints Implementados ------

1 - Crear una nueva orden

Método: POST

Ruta: /api/orders

Descripción: Registra una nueva orden. Verifica el stock antes de confirmar la compra.

Cuerpo JSON de ejemplo:

{
  "customerId": "11111111-1111-1111-1111-111111111111",
  "shippingAddress": "Av. Siempre Viva 742",
  "billingAddress": "Av. Siempre Viva 742",
  "orderItems": [
    {
      "productId": "2AABD1AB-C722-4772-AE52-E257E4585DE9",
      "quantity": 2,
      "unitPrice": 1200.00
    }
  ],
  "notes": "Entrega rápida"
}


2 - Obtener todas las órdenes

Método: GET

Ruta: /api/orders

Descripción: Lista de todas las órdenes, con soporte para paginación y filtros opcionales (status, customerId).

3 - Obtener una orden por ID

Método: GET

Ruta: /api/orders/{id}

Descripción: Devuelve los detalles completos de una orden específica.

4 - Actualizar el estado de una orden
Método: PUT

Ruta: /api/orders/{id}/status

Cuerpo JSON:
{
  "newStatus": "Processing"
}

-- Notas

No se incluye lógica de pago.

El stock de productos se actualiza automáticamente al crear una orden.

Las validaciones aseguran que no se creen órdenes si no hay suficiente stock.