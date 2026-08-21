# Formatos de Respuesta de la Base de Datos

Este documento describe los formatos de datos que devuelven los diferentes repositorios y DTOs del sistema.

## JAV Repository

### GetAllJavsQuery / GetJavByIdQuery

Devuelve un array (o un objeto individual) de `JavDto` con la siguiente estructura:

```json
[
  {
    "id": 1,
    "code": "ABC-123",
    "actresses": [
      {
        "id": 5,
        "name": "Actriz Ejemplo 1",
        "createdAt": "2024-01-15T08:20:00Z"
      },
      {
        "id": 8,
        "name": "Actriz Ejemplo 2",
        "createdAt": "2024-02-10T12:15:00Z"
      }
    ],
    "tags": ["tag1", "tag2", "tag3"],
    "image": "https://example.com/image1.jpg",
    "status": 1,
    "links": [
      {
        "id": 20,
        "javId": 1,
        "url": "https://jav-video-link1.com",
        "orderIndex": 1,
        "createdAt": "2024-04-17T10:32:00Z"
      },
      {
        "id": 21,
        "javId": 1,
        "url": "https://jav-video-link2.com",
        "orderIndex": 2,
        "createdAt": "2024-04-17T10:33:00Z"
      }
    ],
    "createdAt": "2024-04-17T10:30:00Z"
  }
]
```

### Estructura de JavDto

**Nivel raíz**: Array de objetos `JavDto`

Cada elemento contiene:

#### 1. Propiedades del JAV
- `id` (int): ID único del JAV
- `code` (string): Código del JAV (ej: "ABC-123")
- `image` (string): URL de la imagen
- `status` (int): Estado del contenido (1=Pending, 2=Active, etc.)
- `createdAt` (DateTime): Fecha de creación del JAV
- `tags` (array de strings): Tags combinados del JAV y de todas sus actrices

#### 2. `actresses` (Array de ActressDto)
Cada actriz contiene:
- `id` (int): ID único de la actriz
- `name` (string): Nombre de la actriz
- `createdAt` (DateTime): Fecha de creación de la actriz

**Nota**: Los links de las actrices NO se incluyen en las consultas de JAV. Para obtener los links específicos de una actriz, se debe usar el endpoint de actrices por separado.

#### 3. `links` (Array de LinkJavDto)
Links específicos del JAV:
- `id` (int): ID único del link
- `javId` (int): ID del JAV relacionado
- `url` (string): URL del link
- `name` (string): Nombre descriptivo del link (opcional)
- `orderIndex` (int): Orden de visualización
- `createdAt` (DateTime): Fecha de creación del link

### Características del formato

- **Ordenamiento**: Los JAVs se ordenan por `CreatedAt DESC`
- **Links ordenados**: Los links mantienen su `OrderIndex` para preservar el orden
- **Tags combinados**: Incluye tags del JAV y de todas sus actrices en un solo array
- **DTOs específicos**: Usa `LinkJavDto` para links de JAV y `LinkActressJavDto` para links de actrices
- **Mapeo desde repositorio**: Se mapea desde `JavWithDetails` que usa las tablas `LinkJav` y `LinkActressJav`

### Métodos relacionados

Los siguientes métodos devuelven el mismo formato `JavDto`:

- `GetAllJavsQuery` - Todos los JAVs con detalles completos
- `GetJavByIdQuery` - Un JAV específico con detalles

### DTOs utilizados

- **JavDto**: Estructura principal del JAV
- **ActressDto**: Información básica de la actriz (sin links)
- **LinkJavDto**: Links específicos del JAV (mapeado desde `LinkJav`)

**Nota importante**: Los links de las actrices (`LinkActressJav`) no se incluyen en las consultas de JAV para mantener la respuesta ligera. Si se necesitan los links de una actriz específica, se debe consultar el endpoint de actrices por separado.

---

## Próximas secciones

_Aquí se pueden agregar otros formatos de respuesta de diferentes repositorios como ActressRepository, AnimeRepository, etc._