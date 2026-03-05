# Estado de Endpoints

## Actress JAV — `api/actress-jav`

- [x] `GET /` — Lista todas las actrices con foto y tags
- [x] `GET /{id}` — Detalle de actriz: nombre, foto, tags, links, javs con tags + links
- [x] `GET /{id}/javs` — Javs de esa actriz con tags (jav + actrices) y links
- [x] `POST /` — Crear actriz
- [x] `PUT /{id}` — Actualizar nombre y tags de actriz
- [x] `PUT /{id}/links` — Actualizar links de actriz

---

## JAV — `api/jav`

- [x] `GET /` — Lista todos los javs con tags (jav + actrices), actrices y links
- [x] `GET /{id}` — Detalle para edición: tags propios del jav, actrices, links, foto
- [x] `GET /check/{code}` — Verificar si un código ya existe
- [x] `POST /` — Crear jav con actrices, links y tags
- [x] `POST /bulk` — Crear múltiples javs
- [x] `POST /bulk-import` — Importar javs en masa
- [x] `PUT /{id}` — Actualizar jav: código, actrices, links, tags
- [x] `PATCH /{id}/status` — Cambiar status (proximamente/completado/pendiente)
- [x] `DELETE /{id}` — Eliminar jav

---

## Actress Adult — `api/actress-adult`

- [x] `GET /` — Lista todas las actrices con foto y tags
- [x] `GET /{id}` — Detalle de actriz: nombre, foto, tags, links y videos con sus tags
- [x] `POST /` — Crear actriz
- [x] `PUT /{id}` — Actualizar nombre y tags de actriz
- [x] `PUT /{id}/links` — Actualizar links de actriz
- [x] `POST /video` — Crear video con actrices y tags
- [x] `PUT /video/{videoId}` — Actualizar actrices y tags del video
- [x] `PATCH /video/{videoId}/status` — Cambiar status del video

---

## Hentai — `api/hentai`

- [x] `GET /` — Lista todos los hentais con tags
- [x] `GET /{id}` — Detalle de hentai
- [x] `POST /` — Crear hentai desde MyAnimeList
- [x] `PUT /{id}/tags` — Actualizar tags
- [x] `DELETE /{id}` — Eliminar hentai

---

## Anime — `api/anime`

- [x] `GET /` — Lista todos los animes
- [x] `GET /{id}` — Detalle de anime
- [x] `POST /` — Crear anime desde MyAnimeList
- [ ] `PUT /{id}` — Actualizar anime *(comentado en controller)*
- [x] `DELETE /{id}` — Eliminar anime

---

## Series — `api/series`

- [x] `GET /` — Lista todas las series/películas
- [x] `GET /{id}` — Detalle de serie
- [x] `POST /` — Crear serie desde IMDB
- [x] `PATCH /{id}/status` — Cambiar status
- [x] `DELETE /{id}` — Eliminar serie

---

## Girl Galery — `api/girl-galery`

- [x] `GET /` — Lista todas las galerías
- [x] `GET /{id}` — Detalle con imágenes y links
- [x] `POST /` — Crear galería
- [x] `PUT /{id}` — Actualizar nombre
- [x] `PATCH /{id}/media/reorder` — Reordenar imágenes
- [x] `PUT /{id}/links` — Actualizar links
- [x] `DELETE /{id}` — Eliminar galería

---

## Anime Galery — `api/anime-galery`

- [x] `GET /` — Lista todas las galerías
- [x] `GET /{id}` — Detalle con imágenes y links
- [x] `POST /` — Crear galería
- [x] `PUT /{id}` — Actualizar nombre
- [x] `PATCH /{id}/media/reorder` — Reordenar imágenes
- [x] `PUT /{id}/links` — Actualizar links
- [x] `DELETE /{id}` — Eliminar galería

---

## Tags — `api/tags`

- [x] `GET /` — Lista todos los tags (filtrable por type)
- [x] `POST /` — Crear tag
- [x] `PUT /{id}` — Actualizar tag
- [x] `DELETE /{id}` — Eliminar tag

---

## Upload — `api/upload`

- [x] `POST /image` — Subir imagen
- [x] `DELETE /media/{id}` — Eliminar media

---

## Auth — `api/auth`

- [x] `POST /login` — Login con JWT
- [x] `GET /me` — Datos del usuario autenticado
- [x] `POST /mapping-permissions` — Mapear permisos

---

## Metadata — `api/metadata`

- [x] `GET /` — Metadata global (counts, enums, etc.)

---

---

# Por implementar

## YouTube — `api/youtube`

- [ ] `GET /` — Lista todos los videos (filtrable por category)
- [ ] `GET /{id}` — Detalle de video
- [ ] `POST /` — Agregar video (obtiene metadata oEmbed de YouTube)
- [ ] `DELETE /{id}` — Eliminar video

---

## Proyectos — `api/proyect`

- [ ] `GET /` — Lista todos los proyectos
- [ ] `GET /{id}` — Detalle con links y tags
- [ ] `POST /` — Crear proyecto
- [ ] `PUT /{id}` — Actualizar proyecto
- [ ] `DELETE /{id}` — Eliminar proyecto

---

## Posts — `api/post`

- [ ] `GET /` — Lista todos los posts
- [ ] `GET /{id}` — Detalle con contenido (bloques)
- [ ] `POST /` — Crear post con bloques de contenido
- [ ] `PUT /{id}` — Actualizar post
- [ ] `DELETE /{id}` — Eliminar post

---

## Task Lists — `api/task-list`

- [ ] `GET /` — Lista todas las listas de tareas
- [ ] `GET /{id}` — Detalle con tareas
- [ ] `POST /` — Crear lista
- [ ] `PUT /{id}` — Actualizar lista
- [ ] `PATCH /{id}/status` — Cambiar status
- [ ] `POST /{id}/task` — Agregar tarea
- [ ] `PATCH /{id}/task/{taskId}` — Completar/descompletar tarea
- [ ] `DELETE /{id}` — Eliminar lista
- [ ] `DELETE /{id}/task/{taskId}` — Eliminar tarea

---

## Events — `api/event` ✅

- [x] `GET /?year=&month=` — Lista eventos del mes (Google Calendar, default: mes actual)
- [x] `POST /` — Crear evento en Google Calendar
- [x] `DELETE /{eventId}` — Eliminar evento por ID de Google

---

## Accounts — `api/account`

- [ ] `GET /` — Lista todas las cuentas (filtrable por type)
- [ ] `GET /{id}` — Detalle con relaciones y propiedades
- [ ] `POST /` — Crear cuenta
- [ ] `PUT /{id}` — Actualizar cuenta
- [ ] `DELETE /{id}` — Eliminar cuenta

---

## Payments — `api/payment` ✅

- [x] `GET /` — Lista todas las deudas con saldo actual + detalles (2 queries)
- [x] `GET /{id}` — Detalle de deuda con todos sus movimientos (PaymentDetail)
- [x] `POST /` — Crear deuda (PersonName + monto inicial, auto-crea primer PaymentDetail tipo deuda)
- [x] `POST /{id}/detail` — Agregar movimiento: deuda/pago/interés_deuda/interés_pago
- [x] `PUT /{id}/detail/{detailId}` — Editar movimiento existente
- [x] `DELETE /{id}` — Eliminar deuda (cascada elimina sus detalles)
- [x] `DELETE /{id}/detail/{detailId}` — Eliminar un movimiento específico

---

## Salary — `api/salary`

- [ ] `GET /` — Obtener configuración actual
- [ ] `POST /` — Crear configuración
- [ ] `PUT /{id}` — Actualizar configuración

---

## Sellers — `api/seller`

- [ ] `GET /` — Lista todos los vendedores
- [ ] `POST /` — Crear vendedor
- [ ] `PUT /{id}` — Actualizar vendedor
- [ ] `DELETE /{id}` — Eliminar vendedor

---

## Dota Heroes — `api/dota/hero`

- [ ] `GET /` — Lista todos los héroes
- [ ] `POST /` — Crear héroe
- [ ] `PUT /{id}` — Actualizar héroe

---

## Dota Treasures — `api/dota/treasure`

- [ ] `GET /` — Lista todos los cofres
- [ ] `GET /{id}` — Detalle con media y caches
- [ ] `POST /` — Crear cofre
- [ ] `PUT /{id}` — Actualizar cofre
- [ ] `DELETE /{id}` — Eliminar cofre

---

## Dota Caches — `api/dota/cache`

- [ ] `GET /` — Lista todos los sets de cache
- [ ] `POST /` — Crear cache
- [ ] `PUT /{id}` — Actualizar cache (precio, cantidad, dueño)
- [ ] `DELETE /{id}` — Eliminar cache

---

## Steam Items — `api/steam/item`

- [ ] `GET /` — Lista items (filtrable por game/status)
- [ ] `POST /` — Crear item
- [ ] `PUT /{id}` — Actualizar item
- [ ] `PATCH /{id}/status` — Cambiar status
- [ ] `DELETE /{id}` — Eliminar item

---

## Steam Item Drops — `api/steam/drop`

- [ ] `GET /` — Lista drops semanales
- [ ] `POST /` — Registrar drop
- [ ] `PUT /{id}` — Actualizar drop
- [ ] `DELETE /{id}` — Eliminar drop

---

## Steam Item Purchases — `api/steam/purchase`

- [ ] `GET /` — Lista compras (filtrable por status)
- [ ] `POST /` — Registrar compra
- [ ] `PATCH /{id}/sell` — Marcar como vendido con precio de venta
- [ ] `DELETE /{id}` — Eliminar compra
