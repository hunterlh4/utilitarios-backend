-- Anime table (lista de anime)
CREATE TABLE Anime (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ApiId NVARCHAR(500) NOT NULL UNIQUE, -- mal_id de MyAnimeList API
    Title NVARCHAR(500) NOT NULL,
    Image NVARCHAR(1000) NOT NULL,
    Episodes INT NOT NULL,
    Status INT NOT NULL, -- 1: upcoming, 2: completed
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Hentai table (lista de hentai)
CREATE TABLE Hentai (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ApiId NVARCHAR(500) NOT NULL UNIQUE, -- mal_id de MyAnimeList API
    Title NVARCHAR(500) NOT NULL,
    Image NVARCHAR(1000) NOT NULL,
    Episodes INT NOT NULL,
    Status INT NOT NULL, -- 1: upcoming, 2: completed
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Series table (lista de series/películas)
CREATE TABLE Series (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ImdbId NVARCHAR(20) NOT NULL,
    Title NVARCHAR(500) NOT NULL,
    Image NVARCHAR(1000) NOT NULL,
    Year INT,
    Rating DECIMAL(3,1),
    Type NVARCHAR(50),
    Status INT NOT NULL, -- 1: upcoming, 2: completed
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Girl table (galería de chicas)
CREATE TABLE GirlGalery (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Image NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- AnimeGaleria table (galería de imágenes de anime)
CREATE TABLE AnimeGalery (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL, -- Shigatsu, Konosuba, Steins;Gate, etc.
    Image NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- ActressAdult table (actrices porno)
CREATE TABLE ActressAdult (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Image NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- VideoAdult table (videos porno)
CREATE TABLE VideoAdult (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Source VARCHAR(20) NOT NULL,        -- pornhub, xvideos
    ExternalId VARCHAR(100) NOT NULL,   -- viewkey / video id
    VideoUrl NVARCHAR(500) NOT NULL,
    Title NVARCHAR(255),
    ThumbnailUrl NVARCHAR(1000),
    EmbedHtml NVARCHAR(MAX),
    Status INT NOT NULL DEFAULT 1, -- 1: upcoming, 2: completed
    CreatedAt DATETIME DEFAULT GETDATE(),
    UNIQUE (Source, ExternalId)
);

-- ActressVideo table (relación N:N entre actrices y videos)
CREATE TABLE ActressVideo (
    ActressAdultId INT NOT NULL,
    VideoAdultId INT NOT NULL,
    PRIMARY KEY (ActressAdultId, VideoAdultId)
);

-- ActressJav table (actrices de JAV)
CREATE TABLE ActressJav (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Image NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Jav table (videos JAV)
CREATE TABLE Jav (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Code NVARCHAR(50) NOT NULL UNIQUE, -- NIMA-055
    Image NVARCHAR(1000) NULL,
    Status INT NOT NULL, -- 1: upcoming, 2: completed, 3: pending
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- JavActress table (relación N:N entre JAVs y actrices)
CREATE TABLE JavActress (
    JavId INT NOT NULL,
    ActressId INT NOT NULL,
    PRIMARY KEY (JavId, ActressId)
);



-- Media table (Imagenes subida) 1,2,3 N imagenes, 4 actres 1 imagen, 5 1 imagen
CREATE TABLE Media (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Type INT NOT NULL, -- 1: GirlGalery N, 2: AnimeGalery N, 3: Project N, 4: ActressJav, 5: ActressAdult
    RefId INT NOT NULL,
    Url NVARCHAR(1000) NOT NULL,
    Thumbnail NVARCHAR(1000),
    DeleteUrl NVARCHAR(1000),
    OrderIndex INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Link table (enlaces genéricos para Project, Jav, GirlGalery, Actress, Post, helpers)
CREATE TABLE Link (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Type INT NOT NULL, -- 1: Project (url_extra), 2: Jav (streaming), 3: HelperJav, 4: GirlGalery, 5: ActressJav, 6: Post, 7: ActressAdult, 8: AnimeGalery
    RefId INT, -- ID de la entidad (NULL para helpers)
    Name NVARCHAR(200), -- Nombre del helper o link
    Url NVARCHAR(1000) NOT NULL,
    OrderIndex INT,
    CreatedAt DATETIME DEFAULT GETDATE()
);


-- Projecto table (galería de projectos)
CREATE TABLE Project (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(500) NOT NULL,
    Description NVARCHAR(MAX),
    Url NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE()
);


-- YouTube table (videos de YouTube con metadata oEmbed)
CREATE TABLE YouTube (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Url NVARCHAR(500) NOT NULL,
    Title NVARCHAR(500) NOT NULL,
    AuthorName NVARCHAR(200),
    AuthorUrl NVARCHAR(500),
    Type NVARCHAR(50), -- "video"
    Height INT,
    Width INT,
    Version NVARCHAR(10), -- "1.0"
    ProviderName NVARCHAR(100), -- "YouTube"
    ProviderUrl NVARCHAR(500), -- "https://www.youtube.com/"
    ThumbnailHeight INT,
    ThumbnailWidth INT,
    ThumbnailUrl NVARCHAR(1000),
    Html NVARCHAR(MAX), -- iframe embed code
    Category INT NOT NULL, -- 1: anime, 2: serie, 3: pelicula, 4: shorts
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Tag table (tags genéricos reutilizables para Jav, Project, Post, VideoAdult, Hentai)
CREATE TABLE Tag (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Type INT NOT NULL, -- 1: ActressJav, 2: Project, 3: Post, 4: Other, 5: ActressAdult, 6: Hentai, 7: Jav, 8: VideoAdult
    UNIQUE (Name, Type) -- Mismo nombre puede existir en diferentes tipos
);

-- TagRelation table (relación genérica entre tags y entidades)
CREATE TABLE TagRelation (
    TagId INT NOT NULL,
    RefId INT NOT NULL, -- ID de la entidad (ActressJav, Project, Post, ActressAdult, Hentai, Jav, VideoAdult)
    Type INT NOT NULL, -- 1: ActressJav, 2: Project, 3: Post, 4: Other, 5: ActressAdult, 6: Hentai, 7: Jav, 8: VideoAdult
    PRIMARY KEY (TagId, RefId, Type)
);

-- Seller table (vendedores)
CREATE TABLE Seller (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200),
    Whatsapp NVARCHAR(20), -- +1234567890
    Products NVARCHAR(MAX), -- Lista de productos que vende
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null
);

-- DotaHero table (héroes de Dota 2)
CREATE TABLE DotaHero (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Image NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null,
);

-- DotaTreasure table (cofres de Dota 2)
CREATE TABLE DotaTreasure (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Image NVARCHAR(1000) NOT NULL,
    ImagePresentation NVARCHAR(1000),
    Year INT NOT NULL,
    Type INT, -- 1: Treasure I, 2: Treasure II, NULL: sin número
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null,
);

-- DotaCache table (sets de cache)
CREATE TABLE DotaCache (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TreasureId INT NOT NULL, -- ID del cofre
    HeroId INT NOT NULL, -- ID del héroe
    Name NVARCHAR(200) NOT NULL, -- Nombre del set
    Photo NVARCHAR(1000) NOT NULL, -- Foto principal
    Price DECIMAL(10,2),
    Quantity INT,
    Total DECIMAL(10,2),
    Owner NVARCHAR(200),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null,

);

-- DotaMedia table (fotos para cofres y cache)
CREATE TABLE DotaMedia (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Type INT NOT NULL, -- 1: DotaTreasure, 2: DotaCache
    RefId INT NOT NULL, -- ID del cofre o cache
    Url NVARCHAR(1000) NOT NULL,
    OrderIndex INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- SteamItem table (catálogo de items de Steam)
CREATE TABLE SteamItem (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ExternalId NVARCHAR(200), -- ID del item en Steam (market hash name o classid)
    Name NVARCHAR(300) NOT NULL,
    Image NVARCHAR(1000) NOT NULL,
    Price decimal(10,2), -- precio en soles "S/. 2.50"
    Game INT NOT NULL, -- 1: dota2, 2: cs2
    MarketUrl NVARCHAR(1000) NOT NULL,
    Status INT NOT NULL, -- 1: historial, 2: por_comprar
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null
);

-- SteamItemDrop table (drops semanales - items que tengo)
CREATE TABLE SteamItemDrop (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SteamItemId INT NOT NULL, -- ID del SteamItem
    Quantity INT NOT NULL,
    Price DECIMAL(10,2), -- Precio unitario
    SalePrice DECIMAL(10,2), -- Precio de venta
    Total DECIMAL(10,2), -- quantity * salePrice
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null,
);

-- SteamItemPurchase table (compras de items de Steam)
CREATE TABLE SteamItemPurchase (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SteamItemId INT NOT NULL, -- ID del SteamItem
    PurchasePrice DECIMAL(10,2) NOT NULL, -- Precio de compra
    SalePrice DECIMAL(10,2) DEFAULT 0, -- Precio de venta (0 hasta que se venda)
    Profit DECIMAL(10,2), -- Ganancia (salePrice - purchasePrice)
    Status INT NOT NULL, -- 1: comprado, 2: vendido
    PurchaseDate DATETIME NOT NULL, -- Fecha de compra
    SaleDate DATETIME, -- Fecha de venta (opcional)
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null
);

-- AccountEmail table
CREATE TABLE AccountEmail (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Provider NVARCHAR(50) NOT NULL,        -- gmail, outlook, otro
    Email NVARCHAR(200) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(20),
    RecoveryEmailId INT,                   -- FK a otra AccountEmail (correo de recuperación)
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null

);

-- AccountSteam table
CREATE TABLE AccountSteam (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmailId INT,                           -- FK a AccountEmail (opcional)
    Username NVARCHAR(200) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(20),
    ProfileUrl NVARCHAR(1000),
    ImageUrl NVARCHAR(1000),
    HasDota2 BIT NOT NULL DEFAULT 0,
    HasCS2 BIT NOT NULL DEFAULT 0,
    IsUnlimited BIT NOT NULL DEFAULT 0,
    IsVacBanned BIT NOT NULL DEFAULT 0,
    HasSteamMobile BIT NOT NULL DEFAULT 0,
    LastPurchaseDate DATETIME, -- Fecha del último movimiento (compra o venta)
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null
);

-- AccountGitHub table
CREATE TABLE AccountGitHub (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmailId INT,                           -- FK a AccountEmail (opcional)
    Username NVARCHAR(200) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    ProfileUrl NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null
);

-- AccountGeneral table (Facebook, Instagram, Rakion, LOL, etc.)
CREATE TABLE AccountGeneral (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Platform INT NOT NULL,                 -- enum: 1=Facebook, 2=Instagram, 3=Rakion, 4=LOL, 5=Other
    Username NVARCHAR(200) NOT NULL,
    Password NVARCHAR(200) NOT NULL,
    EmailId INT,                           -- FK a AccountEmail (opcional)
    ProfileUrl NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null
);

-- AccountKiro table (una sola cuenta, referencia a Email o GitHub)
CREATE TABLE AccountKiro (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    LinkedType INT NOT NULL,               -- 1: Email, 2: GitHub
    RefId INT NOT NULL,                    -- Id de AccountEmail o AccountGitHub
    IsNew BIT NOT NULL DEFAULT 1,
    LastUsed DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null
);

-- Payment table (deudas/pagos por persona)
CREATE TABLE Payment (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PersonName NVARCHAR(200) NOT NULL,   -- Nombre de la persona (string directo)
    Amount DECIMAL(10,2) NOT NULL,       -- Monto inicial de la deuda
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null

);

-- PaymentDetail table (movimientos de una deuda: abonos, intereses, pagos)
CREATE TABLE PaymentDetail (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PaymentId INT NOT NULL,              -- FK a Payment
    Type INT NOT NULL,                  -- 1: deuda (+), 2: pago (-), 3: interés_deuda (+), 4: interés_pago (-)
    Amount DECIMAL(10,2) NOT NULL,      -- Monto del movimiento (siempre positivo, el tipo define signo)
    Date DATETIME NOT NULL,             -- Fecha del movimiento (sin hora)
    Description NVARCHAR(500),          -- Detalle opcional del movimiento
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null,
);

-- Salary table (configuración de sueldo)
CREATE TABLE Salary (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CurrentMoney DECIMAL(10,2), -- Dinero actual
    GrossSalary DECIMAL(10,2) NOT NULL, -- Sueldo bruto mensual
    AfpDiscount DECIMAL(10,2) NOT NULL, -- Descuento AFP/ONP
    FirstFortnightNet DECIMAL(10,2) NOT NULL, -- Primera quincena neta
    SecondFortnightNet DECIMAL(10,2) NOT NULL, -- Segunda quincena neta
    Cts DECIMAL(10,2), -- CTS semestral
    Bonus DECIMAL(10,2), -- Gratificación semestral
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null

);

-- Post table (publicaciones/manuales)
CREATE TABLE Post (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(500) NOT NULL,
    Description NVARCHAR(MAX),
    Category INT NOT NULL, -- 1: Frontend, 2: Backend, 3: Mobile, 4: Diseño, 5: Base de Datos, 6: Utilidades, 7: ORM, 8: Fullstack
    Subcategory NVARCHAR(100),
    Slug NVARCHAR(200) NOT NULL UNIQUE,
    Date DATE NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null 
);

-- PostContent table (bloques de contenido)
CREATE TABLE PostContent (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PostId INT NOT NULL,
    Type INT NOT NULL, -- 1: titulo, 2: parrafo, 3: codigo, 4: imagen, 5: lista
    Text NVARCHAR(MAX),
    Language NVARCHAR(50), -- Para código
    Url NVARCHAR(1000), -- Para imagen
    Alt NVARCHAR(500), -- Para imagen
    OrderIndex INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME null

);

-- PostContentItem table (items de lista)
CREATE TABLE PostContentItem (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PostContentId INT NOT NULL,
    Text NVARCHAR(MAX) NOT NULL,
    OrderIndex INT NOT NULL
);  

-- Task table (listas de tareas)
CREATE TABLE Task (
    Id NVARCHAR(50) PRIMARY KEY, -- timestamp como string
    Title NVARCHAR(500) NOT NULL,
    Status INT NOT NULL, -- 1: in progress, 2: completed
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME null
);

-- TaskDetail table (tareas individuales de una lista)
CREATE TABLE TaskDetail (
    Id NVARCHAR(50) PRIMARY KEY,
    TaskId NVARCHAR(50) NOT NULL, -- FK a Task
    Title NVARCHAR(500) NOT NULL,
    Status INT NOT NULL DEFAULT 1, -- 1: pending, 2: complete
    Date DATETIME, -- deadline or completion date
);

-- Event table (eventos de calendario)
CREATE TABLE Event (
    Id NVARCHAR(50) PRIMARY KEY,
    Title NVARCHAR(500) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Type INT NOT NULL, -- 1: festivo, 2: personal
    AllDay BIT NOT NULL DEFAULT 1, -- 0 = false, 1 = true
    Color NVARCHAR(20), -- #ef4444
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Comic (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NULL, -- Shigatsu, Konosuba, Steins;Gate, etc.
    Image NVARCHAR(1000) null,
    Url NVARCHAR(1000) null, 
    Category NVARCHAR(100) null,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Indexes para mejorar rendimiento de consultas

-- Filtrar por status (proximamente/completado)
CREATE INDEX IX_Anime_Status ON Anime(Status);
CREATE INDEX IX_Hentai_Status ON Hentai(Status);
CREATE INDEX IX_Series_Status ON Series(Status);
CREATE INDEX IX_Jav_Status ON Jav(Status);

-- Buscar por IMDB ID
CREATE INDEX IX_Series_ImdbId ON Series(ImdbId);

-- Buscar por código JAV
CREATE INDEX IX_Jav_Code ON Jav(Code);

-- Buscar actrices de un JAV y JAVs de una actriz
CREATE INDEX IX_JavActress_JavId ON JavActress(JavId);
CREATE INDEX IX_JavActress_ActressId ON JavActress(ActressId);

-- Obtener videos de una actriz
CREATE INDEX IX_ActressVideo_ActressAdultId ON ActressVideo(ActressAdultId);
CREATE INDEX IX_ActressVideo_VideoAdultId ON ActressVideo(VideoAdultId);

-- Obtener media de una entidad
CREATE INDEX IX_Media_Type_RefId ON Media(Type, RefId);

-- Obtener links de una entidad
CREATE INDEX IX_Link_Type_RefId ON Link(Type, RefId);

-- Obtener tags de una entidad
CREATE INDEX IX_TagRelation_Type_RefId ON TagRelation(Type, RefId);

-- Buscar tags por tipo
CREATE INDEX IX_Tag_Type ON Tag(Type);

-- Filtrar YouTube por categoría
CREATE INDEX IX_YouTube_Category ON YouTube(Category);

-- Ordenar por fecha de creación
CREATE INDEX IX_Anime_CreatedAt ON Anime(CreatedAt DESC);
CREATE INDEX IX_Hentai_CreatedAt ON Hentai(CreatedAt DESC);
CREATE INDEX IX_Series_CreatedAt ON Series(CreatedAt DESC);
CREATE INDEX IX_Jav_CreatedAt ON Jav(CreatedAt DESC);
CREATE INDEX IX_YouTube_CreatedAt ON YouTube(CreatedAt DESC);

-- Índices para Account
CREATE INDEX IX_AccountSteam_EmailId ON AccountSteam(EmailId);
CREATE INDEX IX_AccountGitHub_EmailId ON AccountGitHub(EmailId);
CREATE INDEX IX_AccountGeneral_Platform ON AccountGeneral(Platform);
CREATE INDEX IX_AccountGeneral_EmailId ON AccountGeneral(EmailId);
CREATE INDEX IX_AccountKiro_LinkedType ON AccountKiro(LinkedType);

-- Índices para Payment y PaymentDetail
CREATE INDEX IX_Payment_PersonName ON Payment(PersonName);
CREATE INDEX IX_Payment_CreatedAt ON Payment(CreatedAt DESC);
CREATE INDEX IX_PaymentDetail_PaymentId ON PaymentDetail(PaymentId);
CREATE INDEX IX_PaymentDetail_Date ON PaymentDetail(Date DESC);
CREATE INDEX IX_PaymentDetail_Type ON PaymentDetail(Type);

-- Índices para Post
CREATE INDEX IX_Post_Category ON Post(Category);
CREATE INDEX IX_Post_Slug ON Post(Slug);
CREATE INDEX IX_Post_Date ON Post(Date DESC);
CREATE INDEX IX_PostContent_PostId ON PostContent(PostId);
CREATE INDEX IX_PostContentItem_PostContentId ON PostContentItem(PostContentId);

-- Índices para Task y TaskDetail
CREATE INDEX IX_Task_Status ON Task(Status);
CREATE INDEX IX_Task_CreatedAt ON Task(CreatedAt DESC);
CREATE INDEX IX_TaskDetail_TaskId ON TaskDetail(TaskId);
CREATE INDEX IX_TaskDetail_Status ON TaskDetail(Status);

-- Índices para Event
CREATE INDEX IX_Event_Type ON Event(Type);
CREATE INDEX IX_Event_StartDate ON Event(StartDate);
CREATE INDEX IX_Event_EndDate ON Event(EndDate);
