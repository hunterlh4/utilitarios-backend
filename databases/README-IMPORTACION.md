# Guia de importacion completa (orden recomendado)

Este documento explica como restaurar datos cuando exportaste varios modulos del projecto.

## 1) Punto clave: "todo el projecto" no se restaura 100% por Excel

Hoy solo tienen export/import por endpoint estos modulos:

- Tags
- Anime
- Hentai
- Series
- YouTube
- Galeria Anime
- Galeria Girl
- ActressJav
- ActressAdult
- Steam Items
- Steam Drops
- Steam Purchases

Tablas sin import/export Excel dedicado (segun controladores actuales):

- Project
- DotaHero, DotaTreasure, DotaCache, DotaMedia
- AccountEmail, AccountSteam, AccountGitHub
- Otras tablas fuera de los endpoints anteriores

Importa en este orden para evitar referencias rotas:

1. Tags
2. Anime
3. Hentai
4. Series
5. YouTube
6. Galeria Anime
7. Galeria Girl
8. ActressJav
9. ActressAdult
10. Steam Items
11. Steam Drops
12. Steam Purchases

## 4) Por que este orden

- Tags primero:
  Los importadores de Hentai, ActressJav y ActressAdult usan TagIds. Si no hay tags cargados, se perderan relaciones de tags.

- ActressJav y ActressAdult despues de Tags:
  Sus excels ya incluyen datos relacionados (actrices, videos/javs, links y relaciones), pero dependen de tags existentes para respetar TagIds.

- Steam Items antes de Drops/Purchases:
  Drops y Purchases dependen de SteamItemId.

