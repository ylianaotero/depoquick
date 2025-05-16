# DepoQuick 🏬

**DepoQuick** es una plataforma web para la gestión de depósitos de almacenamiento temporal. Permite a los usuarios reservar depósitos según disponibilidad, aplicar promociones, registrar valoraciones y realizar pagos. Además, ofrece herramientas para la administración completa del sistema, como control de usuarios, generación de reportes y gestión de logs.

Este proyecto fue desarrollado como parte del curso _Diseño de Aplicaciones 1_ en la Universidad ORT Uruguay, aplicando metodologías modernas de desarrollo y buenas prácticas de ingeniería de software.

## 🔍 Características principales

- **Roles de usuario diferenciados**
  - Administrador único (primera cuenta registrada)
  - Múltiples clientes
- **Gestión de depósitos**: alta, baja, configuración de fechas disponibles y precios variables según tamaño y climatización.
- **Promociones**: creación y asociación a depósitos, con validación de fechas y reglas de negocio.
- **Reservas**: solicitud, aprobación o rechazo, control de superposición de fechas y pagos asociados.
- **Valoraciones y comentarios** por parte de clientes.
- **Logs de actividad** para trazabilidad del sistema.
- **Notificaciones** internas.
- **Exportación de reportes** de reservas en distintos formatos (ej: CSV, TXT).
- **Estadísticas** sobre el uso del sistema.

## 🧠 Decisiones de diseño y arquitectura

- **Framework**: .NET 6 + Blazor Server
- **Lenguaje**: C#
- **Persistencia**: SQL Server mediante Entity Framework Core
- **Arquitectura en capas** dividida en 4 proyectos principales:
  - `DepoQuick`: clases del dominio (POCOs y validaciones).
  - `BusinessLogic`: lógica de negocio, controladores, repositorios, reportes.
  - `DepoQuickTests`: pruebas unitarias con cobertura total.
  - `Interface`: frontend en Blazor Server.
- **TDD (Test Driven Development)**: metodología aplicada desde el inicio, garantizando cobertura del 100%.
- **Inyección de dependencias** y patrón **Repository** con interfaz genérica `IRepository<T>`.
- **Principios aplicados**:
  - **SOLID**
  - **GRASP**
  - **Clean Code**
  - **Responsabilidad Única y Alta Cohesión**
- **Patrón Template Method**: utilizado en la generación de reportes exportables.

## 🛠️ Tecnologías

| Tecnología         | Descripción                                |
|--------------------|---------------------------------------------|
| .NET 6             | Framework base del backend y frontend       |
| Blazor Server      | Interfaz interactiva SPA                    |
| Entity Framework   | ORM para persistencia y migraciones         |
| SQL Server         | Motor de base de datos relacional           |
| Docker             | Contenedor para la base de datos            |
| xUnit              | Framework de testing                        |

## ⚙️ Ejecución local

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/IngSoft-DA1-2023-2/231810_280070_301178.git
   ```
2. Dirigirse a la carpeta `Scripts/` y ejecutar el setup:
   ```bash
   ./container_setup.sh
   ```
3. Esperar ~1 minuto a que el contenedor de SQL Server esté listo.
4. Ejecutar el script `CreateSchema.sql` dentro del contenedor o mediante DBeaver.
5. En la carpeta `publish/`, ejecutar el binario `Interface`:
   ```bash
   ./Interface
   ```
6. (Opcional) Ejecutar `InsertData.sql` para precargar datos de prueba (usuarios, depósitos, reservas, etc.).

## 🧪 Testing

El sistema fue desarrollado con TDD desde el inicio, utilizando `xUnit`. Todos los módulos tienen pruebas unitarias y se cubren:

- Validaciones de dominio
- Lógica de cálculo de precios y descuentos
- Reglas de reserva y disponibilidad
- Restricciones de roles
- Excepciones personalizadas

## 👥 Equipo

Proyecto desarrollado en equipo como parte de la asignatura:

- Angelina Maverino
- Yliana Otero
- María Belén Rywaczuk
