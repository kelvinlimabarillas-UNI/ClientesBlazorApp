# Aplicación Blazor - Administración de Clientes

Aplicación **Blazor Web App (interactiva en el servidor)** construida con **.NET 10**, que **consume la API REST de clientes** (`ClientesApi`) y **no accede directamente a la base de datos**. La interfaz usa **Bootstrap 5** (modales y validaciones).

## Requisito previo

Debes tener corriendo la API `ClientesApi` (la del proyecto anterior) en `http://localhost:5080`. Si la API está en otra URL, cambia el valor `ApiBaseUrl` en `appsettings.json` y `appsettings.Development.json`.

## Estructura del proyecto

```
ClientesBlazorApp/
├── Components/
│   ├── App.razor              # HTML raíz, incluye Bootstrap por CDN
│   ├── Routes.razor           # Enrutador de Blazor
│   ├── _Imports.razor
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   └── Pages/
│       ├── Home.razor
│       └── Clientes.razor     # Módulo principal: listado, modal crear/editar, modal eliminar
├── Models/
│   ├── ClienteDto.cs          # Igual a lo que devuelve la API
│   └── ClienteFormModel.cs    # Con [Required], [StringLength], etc. para las validaciones
├── Services/
│   ├── IClienteApiService.cs
│   ├── ClienteApiService.cs   # Único lugar donde se llama a la API (HttpClient)
│   └── ApiException.cs
├── wwwroot/css/app.css
├── appsettings.json
├── Program.cs
└── ClientesBlazorApp.csproj
```

## Cómo ejecutar

1. Asegúrate de que `ClientesApi` esté corriendo (`dotnet run` en esa carpeta), normalmente en `http://localhost:5080`.
2. En una terminal nueva, entra a la carpeta de este proyecto:
   ```bash
   cd ClientesBlazorApp
   dotnet restore
   dotnet run
   ```
3. Abre el navegador en:
   ```
   http://localhost:5180
   ```
4. Ve al menú **Clientes** para usar el módulo de administración.

## Qué hace el módulo de Clientes

- **Visualizar clientes**: al entrar a `/clientes`, se llama a `GET /api/clientes` y se muestra una tabla.
- **Agregar cliente**: botón "+ Nuevo cliente" abre un **modal de Bootstrap** con un formulario (`EditForm` + `DataAnnotationsValidator`), que valida CUI, NIT, nombres, apellidos y fecha de nacimiento antes de enviarlos con `POST /api/clientes`.
- **Modificar cliente**: botón "Editar" en cada fila abre el mismo modal, precargado con los datos del cliente, y al guardar llama a `PUT /api/clientes/{id}`.
- **Eliminar cliente**: botón "Eliminar" abre un modal de confirmación y, al aceptar, llama a `DELETE /api/clientes/{id}`.
- **Actualización automática**: después de cada creación, edición o eliminación exitosa, la tabla se vuelve a cargar automáticamente desde la API (no hace falta refrescar la página).

## Notas de diseño

- Toda la comunicación con los datos pasa por `ClienteApiService`, que usa `HttpClient` — la aplicación Blazor **no tiene ninguna referencia a Entity Framework ni a la base de datos**.
- Los mensajes de error que devuelve la API (por ejemplo, CUI duplicado con código 409) se muestran directamente dentro del modal.
- Los modales se controlan con variables booleanas de C# (`mostrarModalFormulario`, `clienteAEliminar`), sin necesidad de JavaScript adicional.
