# Data Reference - Utilitarios

Este documento describe los valores de datos (enums, status, tipos) para cada entidad, ordenado por secuencia de uso desde GirlGalery.

---

## 🖼️ GALERÍA DE CHICAS (GirlGalery)

### Media.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `GirlGalery` | N imágenes por galería |

### Link.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `4` | `GirlGalery` | Links asociados a una galería de chicas |

---

## 🎌 GALERÍA ANIME (AnimeGalery)

### Media.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `2` | `AnimeGalery` | N imágenes por galería de anime |

---

## 🎭 ACTRIZ JAV (ActressJav)

### Media.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `4` | `ActressJav` | 1 imagen por actriz JAV |

### Link.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `5` | `ActressJav` | Links de perfil de actriz JAV |

---

## 🎞️ JAV

### Jav.Status → ContentStatus
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Proximamente` | Pendiente / por ver |
| `2` | `Completado` | Visto / completado |

### Link.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `2` | `Jav` | Links de streaming para un JAV |

### Tag.Type / TagRelation.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Jav` | Tags asociados a un JAV |

---

## 🔞 ACTRIZ ADULT (ActressAdult)

### Media.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `5` | `ActressAdult` | 1 imagen por actriz adulto |

### Link.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `7` | `ActressAdult` | Links de perfil de actriz adulto |

---

## 🎬 VIDEO ADULT (VideoAdult)

### VideoAdult.Source → VideoAdultSource
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Pornhub` | Video de Pornhub |
| `2` | `Xvideos` | Video de Xvideos |

### VideoAdult.Status → ContentStatus
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Proximamente` | Pendiente |
| `2` | `Completado` | Visto |

---

## 🗂️ PROYECTO (Proyect)

### Media.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `3` | `Project` | N imágenes por proyecto |

### Link.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Project` | URL extra / link de proyecto |

### Tag.Type / TagRelation.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `2` | `Project` | Tags asociados a un proyecto |

---

## 📝 POST (Post / PostContent)

### Post.Category → PostCategory
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Frontend` | Frontend |
| `2` | `Backend` | Backend |
| `3` | `Mobile` | Mobile |
| `4` | `Diseno` | Diseño |
| `5` | `BaseDatos` | Base de Datos |
| `6` | `Utilidades` | Utilidades |
| `7` | `ORM` | ORM |
| `8` | `Fullstack` | Fullstack |

### PostContent.Type → PostContentType
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Titulo` | Bloque título |
| `2` | `Parrafo` | Bloque párrafo |
| `3` | `Codigo` | Bloque código (con Language) |
| `4` | `Imagen` | Bloque imagen (con Url / Alt) |
| `5` | `Lista` | Bloque lista (con PostContentItem) |

### Link.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `6` | `Post` | Links de referencia en un post |

### Tag.Type / TagRelation.Type
| Valor | Enum | Descripción |
|-------|------|-------------|
| `3` | `Post` | Tags asociados a un post |

---

## 📺 YOUTUBE (YouTube)

### YouTube.Category → YouTubeCategory
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Anime` | Video de anime |
| `2` | `Serie` | Video de serie |
| `3` | `Pelicula` | Video de película |
| `4` | `Shorts` | Short de YouTube |

---

## 🎥 ANIME / HENTAI / SERIES

### Status → ContentStatus
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Proximamente` | Pendiente / por ver |
| `2` | `Completado` | Visto / completado |

### Genre (catálogo fijo, Id 1–99)
> Usado por `HentaiGenre` para relacionar géneros con hentais.

---

## 🎮 DOTA 2

### DotaTreasure.Type → TreasureType
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `TreasureI` | Treasure I del año |
| `2` | `TreasureII` | Treasure II del año |
| `null` | — | Sin número |

### DotaMedia.Type → DotaMediaType
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `DotaTreasure` | Foto de un cofre (DotaTreasure) |
| `2` | `DotaCache` | Foto de un set de cache (DotaCache) |

---

## 🛒 STEAM

### SteamItem.Game → GameType
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Dota2` | Item de Dota 2 |
| `2` | `CS2` | Item de CS2 |

### SteamItem.Status → SteamItemStatus
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Historial` | Item ya adquirido / historial |
| `2` | `PorComprar` | Item en lista de deseos |

### SteamItemPurchase.Status
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Comprado` | Comprado, aún no vendido |
| `2` | `Vendido` | Vendido |

---

## 👤 CUENTA (Account)

### Account.Type → AccountType
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Email` | Correo electrónico |
| `2` | `Steam` | Cuenta Steam |
| `3` | `Facebook` | Cuenta Facebook |
| `4` | `Instagram` | Cuenta Instagram |
| `5` | `Game` | Cuenta de juego |
| `6` | `Other` | Otro tipo |

### AccountProperty.Device → AccountPropertyKey
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `HasDota2` | Tiene Dota 2 instalado |
| `2` | `HasCS2` | Tiene CS2 instalado |
| `3` | `HasSteamMobile` | Tiene Steam Mobile |
| `4` | `VacBanned` | Tiene VAC Ban |

---

## 💰 PAGOS / PERSONA (Payment / Person)

### Payment.Type → PaymentType
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Deuda` | Deuda nueva |
| `2` | `Pago` | Pago realizado |
| `3` | `InteresDeuda` | Interés sobre deuda |
| `4` | `InteresPago` | Interés sobre pago |

---

## ✅ LISTA DE TAREAS (TaskList / Task)

### TaskList.Status → TaskListStatus
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `EnProceso` | Lista activa / en progreso |
| `2` | `Completado` | Lista completada |

---

## 📅 EVENTO (Event)

### Event.Type → EventType
| Valor | Enum | Descripción |
|-------|------|-------------|
| `1` | `Festivo` | Día festivo / feriado |
| `2` | `Personal` | Evento personal |

---

## 🔗 TRANSVERSALES

### Media.Type → MediaType (tabla `Media`)
| Valor | Enum | Entidad | Imágenes |
|-------|------|---------|---------|
| `1` | `GirlGalery` | GirlGalery | N imágenes |
| `2` | `AnimeGalery` | AnimeGalery | N imágenes |
| `3` | `Project` | Proyect | N imágenes |
| `4` | `ActressJav` | ActressJav | 1 imagen |
| `5` | `ActressAdult` | ActressAdult | 1 imagen |

### Link.Type → LinkType (tabla `Link`)
| Valor | Enum | Entidad | Descripción |
|-------|------|---------|-------------|
| `1` | `Project` | Proyect | URL extra del proyecto |
| `2` | `Jav` | Jav | Links de streaming |
| `3` | `Helper` | — | Links helper (RefId = NULL) |
| `4` | `GirlGalery` | GirlGalery | Links de galería |
| `5` | `ActressJav` | ActressJav | Links de actriz JAV |
| `6` | `Post` | Post | Links de referencia |
| `7` | `ActressAdult` | ActressAdult | Links de actriz adulto |

### Tag.Type → TagType (tabla `Tag` / `TagRelation`)
| Valor | Enum | Entidad |
|-------|------|---------|
| `1` | `Jav` | Jav |
| `2` | `Project` | Proyect |
| `3` | `Post` | Post |
| `4` | `Other` | Otros |
