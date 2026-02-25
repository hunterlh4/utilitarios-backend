# Ejemplos de Datos

Registros de referencia ordenados por secuencia de uso.  
Los IDs de `Media` y `Link` son continuos a través de todas las tablas (tabla única compartida).

---

## GirlGalery (Galería de chicas)
| id | name | createdAt |
|----|------|-----------|
| 1 | Yui Hatano | 2025-01-10 11:25:30 |
| 2 | Maria Ozawa | 2025-01-15 15:40:12 |
| 3 | Sasha Grey | 2025-01-20 09:55:45 |

---

## Media — GirlGalery (type=1, N imágenes)
| id | type | refId | url | thumbnail | deleteUrl | orderIndex | createdAt |
|----|------|-------|-----|-----------|-----------|------------|-----------|
| 1 | 1 | 1 | https://i.ibb.co/xxx/yui1.jpg | https://i.ibb.co/xxx/thumb.jpg | https://ibb.co/xxx/delete | 0 | 2025-01-10 11:26:15 |
| 2 | 1 | 1 | https://i.ibb.co/yyy/yui2.jpg | https://i.ibb.co/yyy/thumb.jpg | https://ibb.co/yyy/delete | 1 | 2025-01-10 11:27:42 |
| 3 | 1 | 2 | https://i.ibb.co/zzz/maria1.jpg | https://i.ibb.co/zzz/thumb.jpg | https://ibb.co/zzz/delete | 0 | 2025-01-15 15:41:20 |

**type=1 → MediaType.GirlGalery**  
Yui Hatano (id=1) tiene 2 imágenes · Maria Ozawa (id=2) tiene 1 imagen

---

## Link — GirlGalery (type=4)
| id | type | refId | name | url | orderIndex | createdAt |
|----|------|-------|------|-----|------------|-----------|
| 1 | 4 | 1 | NULL | https://www.instagram.com/yuihatano | 0 | 2025-01-10 11:30:00 |
| 2 | 4 | 1 | NULL | https://twitter.com/yuihatano | 1 | 2025-01-10 11:31:00 |
| 3 | 4 | 2 | NULL | https://www.instagram.com/mariaozawa | 0 | 2025-01-15 15:45:00 |

**type=4 → LinkType.GirlGalery**  
Yui Hatano (id=1) tiene 2 links (Instagram, Twitter)

---

## AnimeGalery (Galería de imágenes de anime)
| id | name | createdAt |
|----|------|-----------|
| 1 | Shigatsu | 2025-01-12 16:30:00 |
| 2 | Konosuba | 2025-01-15 13:45:18 |
| 3 | Steins;Gate | 2025-01-18 19:20:55 |

---

## Media — AnimeGalery (type=2, N imágenes)
| id | type | refId | url | thumbnail | deleteUrl | orderIndex | createdAt |
|----|------|-------|-----|-----------|-----------|------------|-----------|
| 4 | 2 | 1 | https://i.ibb.co/aaa/shigatsu1.jpg | https://i.ibb.co/aaa/thumb.jpg | https://ibb.co/aaa/delete | 0 | 2025-01-12 16:31:20 |
| 5 | 2 | 1 | https://i.ibb.co/bbb/shigatsu2.jpg | https://i.ibb.co/bbb/thumb.jpg | https://ibb.co/bbb/delete | 1 | 2025-01-12 16:32:10 |
| 6 | 2 | 2 | https://i.ibb.co/ccc/konosuba1.jpg | https://i.ibb.co/ccc/thumb.jpg | https://ibb.co/ccc/delete | 0 | 2025-01-15 13:46:05 |

**type=2 → MediaType.AnimeGalery**  
Shigatsu (id=1) tiene 2 imágenes · Konosuba (id=2) tiene 1 imagen

---

## Link — AnimeGalery (type=8)
| id | type | refId | name | url | orderIndex | createdAt |
|----|------|-------|------|-----|------------|--------|
| 4 | 8 | 1 | NULL | https://www.myanimelist.net/anime/11061/shigatsu | 0 | 2025-01-12 16:33:00 |
| 5 | 8 | 1 | NULL | https://www.instagram.com/shigatsu_official | 1 | 2025-01-12 16:34:00 |
| 6 | 8 | 2 | NULL | https://www.myanimelist.net/anime/30831/konosuba | 0 | 2025-01-15 13:47:00 |

**type=8 → LinkType.AnimeGalery**  
Shigatsu (id=1) tiene 2 links · Konosuba (id=2) tiene 1 link

---

## ActressJav (Actrices de JAV)
| id | name | createdAt |
|----|------|-----------|
| 1 | Yuria Yoshine | 2025-01-15 12:00:00 |
| 2 | Ai Sayama | 2025-01-16 14:30:25 |
| 3 | Saki Okuda | 2025-01-18 10:15:00 |

---

## Media — ActressJav (type=4, 1 imagen)
| id | type | refId | url | thumbnail | deleteUrl | orderIndex | createdAt |
|----|------|-------|-----|-----------|-----------|------------|-----------|
| 7 | 4 | 1 | https://i.ibb.co/abc/yuria.jpg | https://i.ibb.co/abc/thumb.jpg | https://ibb.co/abc/delete | 0 | 2025-01-15 12:01:00 |
| 8 | 4 | 2 | https://i.ibb.co/def/ai.jpg | https://i.ibb.co/def/thumb.jpg | https://ibb.co/def/delete | 0 | 2025-01-16 14:31:00 |

**type=4 → MediaType.ActressJav**  
1 imagen por actriz. `orderIndex=0` siempre.

---

## Link — ActressJav (type=5)
| id | type | refId | name | url | orderIndex | createdAt |
|----|------|-------|------|-----|------------|-----------|
| 9 | 5 | 1 | NULL | https://missav123.com/dm121/en/actresses/Yuria%20Yoshine | 0 | 2025-01-15 12:05:00 |
| 10 | 5 | 1 | NULL | https://www.instagram.com/yuriayoshine | 1 | 2025-01-15 12:06:00 |
| 11 | 5 | 2 | NULL | https://missav123.com/dm121/en/actresses/Ai%20Sayama | 0 | 2025-01-16 14:35:00 |

**type=5 → LinkType.ActressJav**  
Yuria Yoshine (id=1) tiene 2 links (missav, Instagram)

---

## Jav (Videos JAV)
| id | code | image | status | createdAt |
|----|------|-------|--------|-----------|
| 1 | NIMA-055 | https://fourhoi.com/nima-055-uncensored-leak/cover-n.jpg | 1 | 2025-01-15 18:25:40 |
| 2 | SSIS-123 | https://example.com/ssis123.jpg | 2 | 2025-01-16 21:10:15 |
| 3 | PRED-456 | https://example.com/pred456.jpg | 1 | 2025-01-18 11:00:00 |

**status:** 1=Proximamente · 2=Completado (ContentStatus)

---

## JavActress (Relación N:N Jav ↔ ActressJav)
| javId | actressId |
|-------|----------|
| 1 | 1 |
| 2 | 2 |
| 3 | 1 |
| 3 | 3 |

NIMA-055 (id=1) → Yuria Yoshine · SSIS-123 (id=2) → Ai Sayama  
PRED-456 (id=3) → Yuria Yoshine + Saki Okuda (2 actrices)

---

## Link — Jav/Streaming (type=2)
| id | type | refId | name | url | orderIndex | createdAt |
|----|------|-------|------|-----|------------|-----------|
| 12 | 2 | 1 | NULL | https://missav123.com/en/nima-055-uncensored-leak | 0 | 2025-01-15 18:30:12 |
| 13 | 2 | 1 | NULL | https://es.eporner.com/video-KxEjAYhz2dx | 1 | 2025-01-15 18:31:45 |
| 14 | 2 | 1 | NULL | https://es.xsz-av.com/video/202931 | 2 | 2025-01-15 18:32:20 |
| 15 | 2 | 2 | NULL | https://missav123.com/en/ssis-123 | 0 | 2025-01-16 21:15:00 |

**type=2 → LinkType.Jav**  
NIMA-055 (id=1) tiene 3 links de streaming

---

## Link — HelperJav (type=3, refId=NULL)
| id | type | refId | name | url | orderIndex | createdAt |
|----|------|-------|------|-----|------------|--------|
| 16 | 3 | NULL | JAVLibrary | https://www.javlibrary.com | NULL | 2025-01-05 10:00:00 |
| 17 | 3 | NULL | JAVDatabase | https://www.javdatabase.com | NULL | 2025-01-05 10:05:30 |
| 18 | 3 | NULL | buscador | https://www5.javmost.com/search/advance | NULL | 2025-01-05 10:10:15 |

**type=3 → LinkType.HelperJav** · refId=NULL porque no pertenecen a ninguna entidad específica

---

## Tag — ActressJav (type=1)
| id | name | type |
|----|------|------|
| 1 | Creampie | 1 |
| 2 | Big Tits | 1 |
| 3 | Uncensored | 1 |

**type=1 → TagType.ActressJav** · UNIQUE por (Name, Type)

---

## TagRelation — ActressJav (type=1)
| tagId | refId | type |
|-------|-------|------|
| 1 | 1 | 1 |
| 2 | 1 | 1 |
| 3 | 1 | 1 |
| 1 | 2 | 1 |

Yuria Yoshine (actriz id=1) tiene 3 tags: Creampie, Big Tits, Uncensored  
Ai Sayama (actriz id=2) tiene 1 tag: Creampie

---

## ActressAdult (Actrices porno)
| id | name | createdAt |
|----|------|-----------|
| 1 | Mia Khalifa | 2025-02-01 10:00:00 |
| 2 | Lana Rhoades | 2025-02-02 11:30:00 |
| 3 | Riley Reid | 2025-02-03 09:00:00 |

---

## Media — ActressAdult (type=5, 1 imagen)
| id | type | refId | url | thumbnail | deleteUrl | orderIndex | createdAt |
|----|------|-------|-----|-----------|-----------|------------|-----------|
| 19 | 5 | 1 | https://i.ibb.co/ghi/mia.jpg | https://i.ibb.co/ghi/thumb.jpg | https://ibb.co/ghi/delete | 0 | 2025-02-01 10:01:00 |
| 20 | 5 | 2 | https://i.ibb.co/jkl/lana.jpg | https://i.ibb.co/jkl/thumb.jpg | https://ibb.co/jkl/delete | 0 | 2025-02-02 11:31:00 |

**type=5 → MediaType.ActressAdult**  
1 imagen por actriz. `orderIndex=0` siempre.

---

## Link — ActressAdult (type=7)
| id | type | refId | name | url | orderIndex | createdAt |
|----|------|-------|------|-----|------------|-----------|
| 21 | 7 | 1 | NULL | https://pornhub.com/pornstar/mia-khalifa | 0 | 2025-02-01 10:05:00 |
| 22 | 7 | 1 | NULL | https://www.instagram.com/miakhalifa | 1 | 2025-02-01 10:06:00 |
| 23 | 7 | 2 | NULL | https://pornhub.com/pornstar/lana-rhoades | 0 | 2025-02-02 11:35:00 |

**type=7 → LinkType.ActressAdult**

---

## VideoAdult (Videos porno)
| id | source | externalId | videoUrl | title | thumbnailUrl | status | createdAt |
|----|--------|------------|----------|-------|--------------|--------|-----------|
| 1 | pornhub | ph5e4dbd7c8a7a2 | https://www.pornhub.com/view_video.php?viewkey=ph5e4dbd7c8a7a2 | Mia Khalifa Best | https://ei.phncdn.com/videos/thumb.jpg | 2 | 2025-02-01 12:00:00 |
| 2 | xvideos | 12345678 | https://www.xvideos.com/video12345678 | Lana Rhoades Scene | https://cdn.xvideos.com/thumb.jpg | 1 | 2025-02-02 14:00:00 |

**source:** pornhub / xvideos (VideoAdultSource)  
**status:** 1=Proximamente · 2=Completado (ContentStatus)

---

## ActressVideo (Relación N:N ActressAdult ↔ VideoAdult)
| actressAdultId | videoAdultId |
|----------------|--------------|
| 1 | 1 |
| 2 | 2 |
| 3 | 1 |

Video id=1 tiene 2 actrices (Mia Khalifa + Riley Reid)  
Video id=2 tiene 1 actriz (Lana Rhoades)

---

## Tag — ActressAdult (type=5)
| id | name | type |
|----|------|------|
| 9 | Latina | 5 |
| 10 | MILF | 5 |
| 11 | Blowjob | 5 |

**type=5 → TagType.ActressAdult** · tags propios de la actriz porno (no del video)

---

## TagRelation — ActressAdult (type=5)
| tagId | refId | type |
|-------|-------|------|
| 9 | 1 | 5 |
| 10 | 1 | 5 |
| 11 | 2 | 5 |

Mia Khalifa (actriz id=1) tiene 2 tags: Latina, MILF  
Lana Rhoades (actriz id=2) tiene 1 tag: Blowjob

---

## Anime (Lista de anime)
| id | apiId | title | image | episodes | status | createdAt |
|----|-------|-------|-------|----------|--------|----------|
| 1 | 11061 | Shigatsu wa Kimi no Uso | https://cdn.myanimelist.net/images/anime/3/67177.jpg | 22 | 2 | 2025-01-15 14:30:25 |
| 2 | 9253 | Steins;Gate | https://cdn.myanimelist.net/images/anime/5/73199.jpg | 24 | 1 | 2025-01-20 09:15:42 |

**status:** 1=Proximamente · 2=Completado (ContentStatus) · **apiId:** mal_id de MyAnimeList API

---

## Hentai (Lista de hentai)
| id | apiId | title | image | episodes | status | createdAt |
|----|-------|-------|-------|----------|--------|----------|
| 1 | 33137 | Overflow | https://cdn.myanimelist.net/images/anime/overflow.jpg | 8 | 2 | 2025-01-10 20:45:10 |
| 2 | 10415 | Kanojo x Kanojo x Kanojo | https://cdn.myanimelist.net/images/anime/kxkxk.jpg | 3 | 1 | 2025-01-18 16:22:33 |

**status:** 1=Proximamente · 2=Completado (ContentStatus)

---

## Tag — Hentai (type=6)
| id | name | type |
|----|------|------|
| 12 | Romance | 6 |
| 13 | Comedy | 6 |
| 14 | Netorare | 6 |

**type=6 → TagType.Hentai** · reemplaza las tablas Genre + HentaiGenre (UNIQUE por Name+Type)

---

## TagRelation — Hentai (type=6)
| tagId | refId | type |
|-------|-------|------|
| 12 | 1 | 6 |
| 13 | 1 | 6 |
| 12 | 2 | 6 |

Overflow (id=1) → Romance, Comedy  
Kanojo x Kanojo (id=2) → Romance

---

## Series (Lista de series/películas)
| id | imdbId | title | image | year | rating | type | status | createdAt |
|----|--------|-------|-------|------|--------|------|--------|-----------|
| 1 | tt0124298 | Blast from the Past | https://m.media-amazon.com/images/M/MV5BODQ0ZmNk... | 1999 | 6.7 | movie | 1 | 2025-01-12 18:30:15 |
| 2 | tt0944947 | Game of Thrones | https://m.media-amazon.com/images/M/MV5BYTRiNDQ... | 2011 | 9.2 | series | 2 | 2025-01-05 22:10:48 |

**status:** 1=Proximamente · 2=Completado (ContentStatus)

---

## Proyect (Galería de proyectos)
| id | name | description | url | createdAt |
|----|------|-------------|-----|-----------|
| 1 | Sistema de Ventas | user: admin@gmail.com\npassword: 123\n\nNET 9 \| ANGULAR 18 \| SQL | https://ventas.perfisoft.com | 2025-01-08 14:20:35 |
| 2 | Sistema de Reservas Hotel | PHP \| MYSQL\n\nuser: admin@gmail.com\npassword: 123456 | https://reserva.lltechnologyservicepr.com | 2025-01-10 10:15:22 |

---

## Media — Project (type=3, N imágenes)
| id | type | refId | url | thumbnail | deleteUrl | orderIndex | createdAt |
|----|------|-------|-----|-----------|-----------|------------|-----------|
| 24 | 3 | 1 | https://scontent.ftcq1-1.fna.fbcdn.net/v/... | NULL | NULL | 0 | 2025-01-08 14:22:10 |
| 25 | 3 | 1 | https://scontent.ftcq1-1.fna.fbcdn.net/v/... | NULL | NULL | 1 | 2025-01-08 14:22:45 |
| 26 | 3 | 2 | https://i.ibb.co/ddd/hotel1.jpg | https://i.ibb.co/ddd/thumb.jpg | https://ibb.co/ddd/delete | 0 | 2025-01-10 10:16:30 |

**type=3 → MediaType.Project**

---

## Link — Project (type=1, url_extra)
| id | type | refId | name | url | orderIndex | createdAt |
|----|------|-------|------|-----|------------|-----------|
| 27 | 1 | 1 | NULL | https://www.facebook.com/61555695754588/videos/... | 0 | 2025-01-08 14:25:40 |
| 28 | 1 | 2 | NULL | https://www.youtube.com/watch?v=demo123 | 0 | 2025-01-10 10:18:15 |

**type=1 → LinkType.Project**

---

## YouTube (Videos de YouTube)
| id | url | title | authorName | thumbnailUrl | category | createdAt |
|----|-----|-------|------------|--------------|----------|-----------|
| 1 | https://www.youtube.com/watch?v=ekr2nIex040 | ROSÉ & Bruno Mars - APT. | ROSÉ | https://i.ytimg.com/vi/ekr2nIex040/hqdefault.jpg | 1 | 2024-11-22 15:30:00 |
| 2 | https://www.youtube.com/watch?v=dQw4w9WgXcQ | Never Gonna Give You Up | Rick Astley | https://i.ytimg.com/vi/dQw4w9WgXcQ/hqdefault.jpg | 4 | 2024-12-01 20:45:30 |

**category:** 1=Anime · 2=Serie · 3=Pelicula · 4=Shorts (YouTubeCategory)

---

## Tag (Tags genéricos)
| id | name | type |
|----|------|------|
| 1 | Creampie | 1 |
| 2 | Big Tits | 1 |
| 3 | React | 2 |
| 4 | Angular | 2 |
| 5 | SQL Server | 2 |
| 6 | ui | 3 |
| 7 | diseño | 3 |
| 8 | hooks | 3 |
| 9 | Latina | 5 |
| 10 | MILF | 5 |
| 11 | Blowjob | 5 |
| 12 | Romance | 6 |
| 13 | Comedy | 6 |
| 14 | Netorare | 6 |

**type:** 1=ActressJav · 2=Project · 3=Post · 4=Other · 5=ActressAdult · 6=Hentai (TagType)  
Mismo nombre puede existir en distintos tipos (UNIQUE por Name+Type)

---

## TagRelation (Relación Tag-Entidad)
| tagId | refId | type |
|-------|-------|------|
| 1 | 1 | 1 |
| 2 | 1 | 1 |
| 3 | 1 | 2 |
| 4 | 1 | 2 |
| 5 | 1 | 2 |
| 6 | 1 | 3 |
| 7 | 1 | 3 |
| 8 | 2 | 3 |
| 9 | 1 | 5 |
| 10 | 1 | 5 |
| 11 | 2 | 5 |
| 12 | 1 | 6 |
| 13 | 1 | 6 |
| 12 | 2 | 6 |

ActressJav id=1 → Creampie, Big Tits  
Project id=1 → React, Angular, SQL Server  
Post id=1 → ui, diseño · Post id=2 → hooks  
ActressAdult id=1 → Latina, MILF · ActressAdult id=2 → Blowjob  
Hentai id=1 → Romance, Comedy · Hentai id=2 → Romance


---

## Seller (Vendedores)
| id | name | whatsapp | products | createdAt |
|----|------|----------|----------|-----------|
| 1 | Juan Pérez | +50912345678 | Laptops, Teclados, Mouse, Monitores, Cables HDMI, Webcams | 2025-01-10 10:00:00 |
| 2 | María García | +50987654321 | Celulares, Fundas, Protectores de pantalla, Audífonos, Cargadores | 2025-01-12 14:30:00 |
| 3 | Carlos López | +50911223344 | Componentes PC, RAM, SSD, Tarjetas gráficas, Fuentes de poder | 2025-01-15 09:15:00 |

**Ejemplo:** Juan Pérez vende laptops, teclados, mouse, etc. Su WhatsApp es +50912345678


---

## DotaHero (Héroes de Dota 2)
| id | name | image | createdAt |
|----|------|-------|-----------|
| 1 | Phantom Assassin | https://cdn.cloudflare.steamstatic.com/apps/dota2/images/heroes/phantom_assassin_full.png | 2025-01-10 10:00:00 |
| 2 | Invoker | https://cdn.cloudflare.steamstatic.com/apps/dota2/images/heroes/invoker_full.png | 2025-01-10 10:01:00 |
| 3 | Pudge | https://cdn.cloudflare.steamstatic.com/apps/dota2/images/heroes/pudge_full.png | 2025-01-10 10:02:00 |

---

## DotaTreasure (Cofres de Dota 2)
| id | name | image | imagePresentation | year | type | createdAt |
|----|------|-------|-------------------|------|------|-----------|
| 1 | Collector's Cache 2024 | https://example.com/cache2024.jpg | https://example.com/cache2024_pres.jpg | 2024 | NULL | 2025-01-10 11:00:00 |
| 2 | Collector's Cache 2024 I | https://example.com/cache2024_1.jpg | https://example.com/cache2024_1_pres.jpg | 2024 | 1 | 2025-01-10 11:05:00 |
| 3 | Collector's Cache 2024 II | https://example.com/cache2024_2.jpg | https://example.com/cache2024_2_pres.jpg | 2024 | 2 | 2025-01-10 11:10:00 |

**Type:** 1=Treasure I, 2=Treasure II, NULL=sin número

---

## DotaCache (Sets de cache)
| id | treasureId | heroId | name | photo | price | quantity | total | owner | createdAt |
|----|------------|--------|------|-------|-------|----------|-------|-------|-----------|
| 1 | 1 | 1 | Crimson Witness | https://example.com/set1.jpg | 15.50 | 2 | 31.00 | Juan | 2025-01-10 12:00:00 |
| 2 | 1 | 2 | Dark Artistry | https://example.com/set2.jpg | 25.00 | 1 | 25.00 | Maria | 2025-01-10 12:05:00 |
| 3 | 2 | 3 | Feast of Abscession | https://example.com/set3.jpg | 10.00 | 3 | 30.00 | Carlos | 2025-01-10 12:10:00 |

---

## DotaMedia (Fotos adicionales para cofres y cache)
| id | type | refId | url | orderIndex | createdAt |
|----|------|-------|-----|------------|-----------|
| 1 | 1 | 1 | https://example.com/treasure1_extra1.jpg | 0 | 2025-01-10 11:01:00 |
| 2 | 1 | 1 | https://example.com/treasure1_extra2.jpg | 1 | 2025-01-10 11:02:00 |
| 3 | 2 | 1 | https://example.com/set1_pub1.jpg | 0 | 2025-01-10 12:01:00 |
| 4 | 2 | 1 | https://example.com/set1_pub2.jpg | 1 | 2025-01-10 12:02:00 |
| 5 | 2 | 2 | https://example.com/set2_pub1.jpg | 0 | 2025-01-10 12:06:00 |

**Type:** 1=DotaTreasure, 2=DotaCache  
**Ejemplo:** 
- Cofre id=1 tiene 2 fotos adicionales en DotaMedia
- Cache id=1 (Crimson Witness) tiene 2 fotos publicadas en DotaMedia
- Cache id=2 (Dark Artistry) tiene 1 foto publicada en DotaMedia


---

## SteamItem (Catálogo de items de Steam - del buscador)
| id | name | image | price | game | marketUrl | status | createdAt |
|----|------|-------|-------|------|-----------|--------|-----------|
| 1 | Snakebite Case | https://community.cloudflare.steamstatic.com/economy/image/... | $0.64 | 2 | https://steamcommunity.com/market/listings/730/Snakebite%20Case | 1 | 2025-01-10 14:00:00 |
| 2 | Gallery Case | https://community.cloudflare.steamstatic.com/economy/image/... | $1.40 | 2 | https://steamcommunity.com/market/listings/730/Gallery%20Case | 2 | 2025-01-10 14:05:00 |
| 3 | Caja Croma 2 | https://community.fastly.steamstatic.com/economy/image/... | $17.40 | 2 | https://steamcommunity.com/market/listings/730/Chroma%202%20Case | 1 | 2025-01-10 14:10:00 |

**Game:** 1=dota2, 2=cs2  
**Status:** 1=historial (ya buscado), 2=por_comprar

---

## SteamItemDrop (Drops semanales - items que tengo)
| id | steamItemId | quantity | price | salePrice | total | createdAt |
|----|-------------|----------|-------|-----------|-------|-----------|
| 1 | 3 | 2 | 17.40 | 15.13 | 30.26 | 2025-01-10 15:00:00 |
| 2 | 1 | 5 | 0.64 | 0.55 | 2.75 | 2025-01-10 15:05:00 |
| 3 | 2 | 3 | 1.40 | 1.20 | 3.60 | 2025-01-10 15:10:00 |

**Ejemplo:** 
- Drop id=1 usa SteamItem id=3 (Caja Croma 2)
- Tengo 2 unidades, precio $17.40 c/u, venta $15.13 c/u
- Total: 2 × $15.13 = $30.26
- Para obtener nombre, image, url: JOIN con SteamItem

---

## SteamItemPurchase (Compras de items de Steam)
| id | steamItemId | purchasePrice | salePrice | profit | status | purchaseDate | saleDate | createdAt |
|----|-------------|---------------|-----------|--------|--------|--------------|----------|-----------|
| 1 | 4 | 72.82 | 0.00 | -72.82 | 1 | 2025-11-30 10:00:00 | NULL | 2025-11-30 10:00:00 |
| 2 | 5 | 50.00 | 65.00 | 15.00 | 2 | 2025-12-01 14:00:00 | 2025-12-15 16:30:00 | 2025-12-01 14:00:00 |
| 3 | 6 | 30.00 | 0.00 | -30.00 | 1 | 2025-12-10 09:00:00 | NULL | 2025-12-10 09:00:00 |

**Status:** 1=comprado, 2=vendido  
**Ejemplo:** 
- Compra id=1: Feast of Abscession comprado a $72.82, aún no vendido (ganancia -$72.82)
- Compra id=2: Item comprado a $50.00, vendido a $65.00 (ganancia +$15.00)
- Para obtener nombre, foto, link: JOIN con SteamItem


---

## Account (Cuentas genéricas - gestor de contraseñas)
| id | type | name | username | password | profileUrl | phoneNumber | recoveryEmail | lastConnection | createdAt |
|----|------|------|----------|----------|------------|-------------|---------------|----------------|-----------|
| 1 | 1 | Principal | juan@gmail.com | gmailpass123 | NULL | +50912345678 | juan.recovery@gmail.com | NULL | 2025-01-10 10:00:00 |
| 2 | 2 | Main | juangamer | steampass456 | https://steamcommunity.com/id/juangamer | +50912345678 | NULL | 2025-01-15 10:00:00 | 2025-01-10 10:30:00 |
| 3 | 3 | Personal | juan@gmail.com | fbpass789 | https://facebook.com/juanprofile | NULL | NULL | 2025-01-14 18:30:00 | 2025-01-10 11:00:00 |
| 4 | 4 | Main | @juanig | igpass111 | https://instagram.com/juanig | NULL | NULL | 2025-01-16 09:15:00 | 2025-01-10 11:30:00 |
| 5 | 1 | Secundario | juan2@gmail.com | gmail2pass | NULL | +50987654321 | juan@gmail.com | NULL | 2025-01-11 09:00:00 |
| 6 | 2 | Segunda | juanalt | steamalt222 | https://steamcommunity.com/id/juanalt | NULL | NULL | 2025-01-20 14:30:00 | 2025-01-11 09:30:00 |
| 7 | 5 | Main | JuanLOL | lolpass333 | https://lol.com/profile/juanlol | NULL | NULL | 2025-01-18 20:00:00 | 2025-01-11 10:00:00 |

**Type:** 1=Email, 2=Steam, 3=Facebook, 4=Instagram, 5=Game, 6=Other  
**Name:** Main, Segunda, Tercera, Principal, Secundario, etc.  
**profileUrl:** URL del perfil (Steam, Facebook, Instagram, LOL, etc.)  
**lastConnection:** Para cualquier tipo de cuenta, se actualiza al conectarse

---

## AccountRelation (Relaciones entre cuentas)
| id | parentAccountId | childAccountId | createdAt |
|----|-----------------|----------------|-----------|
| 1 | 1 | 2 | 2025-01-10 10:31:00 |
| 2 | 1 | 3 | 2025-01-10 11:01:00 |
| 3 | 1 | 4 | 2025-01-10 11:31:00 |
| 4 | 5 | 6 | 2025-01-11 09:31:00 |
| 5 | 1 | 7 | 2025-01-11 10:01:00 |

**Ejemplo:** 
- Email Principal (id=1) es padre de: Steam Main, Facebook, Instagram, LOL
- Email Secundario (id=5) es padre de: Steam Segunda

---

## AccountProperty (Propiedades booleanas — solo Steam)
| id | accountId | device | value | createdAt |
|----|-----------|--------|-------|-----------|
| 1 | 2 | 1 | 1 | 2025-01-10 10:33:00 |
| 2 | 2 | 2 | 1 | 2025-01-10 10:34:00 |
| 3 | 2 | 3 | 1 | 2025-01-10 10:35:00 |
| 4 | 2 | 4 | 0 | 2025-01-10 10:36:00 |
| 5 | 6 | 1 | 1 | 2025-01-11 09:33:00 |
| 6 | 6 | 2 | 0 | 2025-01-11 09:34:00 |
| 7 | 6 | 4 | 0 | 2025-01-11 09:35:00 |

**device:** 1=HasDota2 · 2=HasCS2 · 3=HasSteamMobile · 4=VacBanned (AccountPropertyKey)  
**value (BIT):** 0=false · 1=true

---

### Árbol de cuentas

```
Account (Email Principal — juan@gmail.com)
├── Account (Steam Main - juangamer)
│   │   profileUrl: https://steamcommunity.com/id/juangamer
│   │   lastConnection: 2025-01-15 10:00:00
│   ├── AccountProperty device=1 HasDota2: 1
│   ├── AccountProperty device=2 HasCS2: 1
│   ├── AccountProperty device=3 HasSteamMobile: 1
│   └── AccountProperty device=4 VacBanned: 0
├── Account (Facebook Personal)
├── Account (Instagram Main — @juanig)
└── Account (LOL Main — JuanLOL)

Account (Email Secundario — juan2@gmail.com)
└── Account (Steam Segunda — juanalt)
    ├── AccountProperty device=1 HasDota2: 1
    ├── AccountProperty device=2 HasCS2: 0
    └── AccountProperty device=4 VacBanned: 0
```

---

## Person (Personas para control de dinero)
| id | name | createdAt |
|----|------|-----------|
| 1 | Rosa | 2025-01-10 10:00:00 |
| 2 | Juan | 2025-01-12 14:00:00 |
| 3 | María | 2025-01-15 09:00:00 |

---

## Payment (Pagos / Deudas)
| id | personId | type | amount | description | date | createdAt |
|----|----------|------|--------|-------------|------|-----------|
| 1 | 1 | 1 | 1170.00 | total | 2025-11-23 | 2025-11-23 10:00:00 |
| 2 | 1 | 2 | 570.00 | | 2025-11-23 | 2025-11-23 15:00:00 |
| 3 | 1 | 2 | 50.00 | | 2025-12-19 | 2025-12-19 12:00:00 |
| 4 | 1 | 2 | 50.00 | | 2026-01-17 | 2026-01-17 14:00:00 |
| 5 | 2 | 1 | 500.00 | préstamo | 2025-12-01 | 2025-12-01 10:00:00 |
| 6 | 2 | 3 | 25.00 | interés 5% | 2025-12-15 | 2025-12-15 10:00:00 |

**type:** 1=Deuda · 2=Pago · 3=InteresDeuda · 4=InteresPago (PaymentType)  
Rosa (id=1): deuda 1170, pagado 670 → debe 500  
Juan (id=2): deuda 500 + interés 25 → debe 525

---

## Salary (Configuración de sueldo)
| id | currentMoney | grossSalary | afpDiscount | firstFortnightNet | secondFortnightNet | cts | bonus | createdAt |
|----|--------------|-------------|-------------|-------------------|--------------------|----|-------|-----------|
| 1 | 5000.00 | 3000.00 | 390.00 | 1200.00 | 1410.00 | 1500.00 | 1500.00 | 2025-01-01 10:00:00 |

**Registras una sola vez y calculas:**

**Cálculos automáticos:**
```sql
-- Sueldo mensual neto
SELECT (firstFortnightNet + secondFortnightNet) as sueldoMensual FROM Salary

-- Cuánto ganaré en X meses (sin CTS ni bonus)
SELECT (firstFortnightNet + secondFortnightNet) * 3 as ganancia3Meses FROM Salary
SELECT (firstFortnightNet + secondFortnightNet) * 6 as ganancia6Meses FROM Salary
SELECT (firstFortnightNet + secondFortnightNet) * 12 as ganancia12Meses FROM Salary

-- Cuánto ganaré en 1 año (con CTS y bonus)
SELECT 
    (firstFortnightNet + secondFortnightNet) * 12 as sueldoAnual,
    cts * 2 as ctsAnual,
    bonus * 2 as bonusAnual,
    ((firstFortnightNet + secondFortnightNet) * 12) + (cts * 2) + (bonus * 2) as totalAnual
FROM Salary

-- Cuánto ganaré en 16 meses (con CTS y bonus proporcional)
SELECT 
    (firstFortnightNet + secondFortnightNet) * 16 as sueldo16Meses,
    cts * FLOOR(16/6.0) as ctsProporcional,  -- CTS cada 6 meses
    bonus * FLOOR(16/6.0) as bonusProporcional,  -- Bonus cada 6 meses
    ((firstFortnightNet + secondFortnightNet) * 16) + 
    (cts * FLOOR(16/6.0)) + 
    (bonus * FLOOR(16/6.0)) as total16Meses
FROM Salary
```

**Ejemplo con tus datos:**
- Sueldo mensual: 1200 + 1410 = **2,610**
- 3 meses: 2,610 × 3 = **7,830**
- 6 meses: 2,610 × 6 = **15,660** + CTS (1,500) + Bonus (1,500) = **18,660**
- 12 meses: 2,610 × 12 = **31,320** + CTS×2 (3,000) + Bonus×2 (3,000) = **37,320**
- 16 meses: 2,610 × 16 = **41,760** + CTS×2 (3,000) + Bonus×2 (3,000) = **47,760**

---

## Post (Publicaciones/Manuales)
| id | title | description | category | subcategory | slug | date | createdAt |
|----|-------|-------------|----------|-------------|------|------|-----------|
| 1 | Refactoring UI | Manual de diseño UI | 4 | UIUX | refactoring-ui | 2025-11-21 | 2025-11-21 10:00:00 |
| 2 | React Hooks Guide | Guía completa de hooks | 1 | React | react-hooks-guide | 2025-12-01 | 2025-12-01 14:00:00 |

**Category:** 1=Frontend, 2=Backend, 3=Mobile, 4=Diseño, 5=Base de Datos, 6=Utilidades, 7=ORM, 8=Fullstack

**Tags:** Se usan Tag (type=3) y TagRelation (type=3)

---

## PostContent (Bloques de contenido)
| id | postId | type | text | language | url | alt | orderIndex | createdAt |
|----|--------|------|------|----------|-----|-----|------------|-----------|
| 1 | 1 | 4 | NULL | NULL | https://example.com/ui-image.jpg | UI Design | 0 | 2025-11-21 10:05:00 |
| 2 | 1 | 1 | Introducción al diseño UI | NULL | NULL | NULL | 1 | 2025-11-21 10:06:00 |
| 3 | 1 | 2 | El diseño UI es fundamental... | NULL | NULL | NULL | 2 | 2025-11-21 10:07:00 |
| 4 | 1 | 3 | const theme = { colors: {...} } | javascript | NULL | NULL | 3 | 2025-11-21 10:08:00 |
| 5 | 1 | 5 | NULL | NULL | NULL | NULL | 4 | 2025-11-21 10:09:00 |

**Type:** 1=titulo, 2=parrafo, 3=codigo, 4=imagen, 5=lista

---

## PostContentItem (Items de lista)
| id | postContentId | text | orderIndex |
|----|---------------|------|------------|
| 1 | 5 | Usar colores consistentes | 0 |
| 2 | 5 | Mantener espaciado uniforme | 1 |
| 3 | 5 | Jerarquía visual clara | 2 |

**Ejemplo:** PostContent id=5 (tipo lista) tiene 3 items

---

## Link — Post (type=6)
| id | type | refId | name | url | orderIndex | createdAt |
|----|------|-------|------|-----|------------|--------|
| 29 | 6 | 1 | PDF | https://online.fliphtml5.com/uejlb/wnsd/#p=1 | 0 | 2025-11-21 10:10:00 |
| 30 | 6 | 1 | github | https://github.com/erikuus/good-ui/blob/main/SUMMARY.md | 1 | 2025-11-21 10:11:00 |

**type=6 → LinkType.Post**

---

### Estructura de un Post completo

```
Post (Refactoring UI — id=1)
├── TagRelation → Tag "ui" (type=3)
├── TagRelation → Tag "diseño" (type=3)
├── PostContent (type=4 imagen: ui-image.jpg)
├── PostContent (type=1 titulo: "Introducción al diseño UI")
├── PostContent (type=2 parrafo: "El diseño UI es fundamental...")
├── PostContent (type=3 codigo: "const theme = {...}" - language=javascript)
├── PostContent (type=5 lista)
│   ├── PostContentItem ("Usar colores consistentes")
│   ├── PostContentItem ("Mantener espaciado uniforme")
│   └── PostContentItem ("Jerarquía visual clara")
├── Link (type=6 PDF)
└── Link (type=6 github)
```

**Consultas útiles:**
```sql
-- Obtener publicación completa
SELECT * FROM Post WHERE slug = 'refactoring-ui'

-- Obtener tags de una publicación
SELECT t.name FROM Tag t
JOIN TagRelation tr ON t.id = tr.tagId
WHERE tr.refId = 1 AND tr.type = '3'

-- Obtener contenido ordenado
SELECT * FROM PostContent WHERE postId = 1 ORDER BY orderIndex

-- Obtener items de una lista
SELECT text FROM PostContentItem WHERE postContentId = 5 ORDER BY orderIndex

-- Obtener enlaces
SELECT name, url FROM Link WHERE refId = 1 AND type = '6' ORDER BY orderIndex

-- Buscar por categoría
SELECT * FROM Post WHERE category = '4' ORDER BY date DESC

-- Buscar por tag
SELECT DISTINCT p.* FROM Post p
JOIN TagRelation tr ON p.id = tr.refId AND tr.type = '3'
JOIN Tag t ON tr.tagId = t.id
WHERE t.name = 'ui' AND t.type = '3'
```
