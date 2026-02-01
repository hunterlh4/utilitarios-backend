
IF NOT EXISTS (
    SELECT name 
    FROM sys.databases 
    WHERE name = 'DB_BMIAMI'
)
BEGIN
    CREATE DATABASE DB_BMIAMI;
END
GO

USE DB_BMIAMI;
GO

-- =============================================
-- ENUMS DOCUMENTATION
-- =============================================
-- MeasurementUnit (usado en Products y PropertyItems):
-- 0 = Unit (unidad, piezas, rollos)
-- 1 = Milliliter (ml)
-- 2 = Liter (L)
-- 3 = FluidOunce (oz líquidas)
-- 4 = Gallon (gal)
-- 5 = Gram (g)
-- 6 = Kilogram (kg)
-- 7 = Pound (lb)
-- 8 = Ounce (oz peso)
--
-- ProductType (usado en Products):
-- 0 = Furniture (Muebles e inmuebles)
-- 1 = Cleaning (Productos de limpieza)
-- 2 = Consumable (Productos consumibles)
--
-- UserType (usado en Users):
-- 0 = Admin
-- 1 = Gestión de Operaciones
-- 2 = Limpieza
-- 3 = Manager Mantenimiento y limpieza
-- =============================================

-- =============================================
-- INVENTORY SYSTEM DOCUMENTATION
-- =============================================
-- Stock.Quantity: Total físico en el almacén
-- Stock.AssignedQuantity: Cantidad asignada a PropertyItems
-- Stock.AvailableQuantity: Quantity - AssignedQuantity (calculado en aplicación)
--
-- PropertyItems.StoreId: Almacén del cual se asigna el producto
-- PropertyItems.Quantity: Cantidad en unidad base (ml, g, unidades)
--
-- Flujo:
-- 1. Purchase → Incrementa Stock.Quantity
-- 2. CreatePropertyItem → Incrementa Stock.AssignedQuantity
-- 3. UpdatePropertyItem → Ajusta Stock.AssignedQuantity
-- 4. DeletePropertyItem → Decrementa Stock.AssignedQuantity
-- 5. Consumption → Decrementa Stock.Quantity
-- =============================================

CREATE TABLE Users
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Username NVARCHAR(100) NOT NULL,
	PasswordHash NVARCHAR(200) NOT NULL,
	SuperUser BIT NOT NULL DEFAULT 0,
	UserType INT NOT NULL DEFAULT 0, -- 0=Admin, 1=Cliente, 2=Limpieza, 3=Inspector
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL,
	CONSTRAINT UC_Users_Username UNIQUE (Username)
)
GO

CREATE TABLE UserDetails
(
	UserId INT PRIMARY KEY,
	FirstName NVARCHAR(100) NOT NULL,
	LastName NVARCHAR(100) NOT NULL,
	Email NVARCHAR(200) NOT NULL,
	PhoneNumber NVARCHAR(50) NULL,
	CountryCode  NVARCHAR(10) null,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL,
	CONSTRAINT FK_UserDetails_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
)
GO

CREATE TABLE Roles
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(200) NOT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE Permissions
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(200) NOT NULL,
	Controller NVARCHAR(100) NOT NULL,
	ActionName NVARCHAR(100) NOT NULL,
	HttpMethod NVARCHAR(100) NOT NULL,
	ActionType NVARCHAR(50) NOT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE UserRoles
(
	UserId INT NOT NULL,
	RoleId INT NOT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId),
	CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id)
)
GO

CREATE TABLE RolePermissions
(
	RoleId INT NOT NULL,
	PermissionId INT NOT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	CONSTRAINT PK_RolePermissions PRIMARY KEY (RoleId, PermissionId),
	CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id),
    CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionId) REFERENCES Permissions(Id)
)
GO

CREATE TABLE Brands
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(200) NOT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(200) NOT NULL,
	ParentId INT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE Products
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Sku NVARCHAR(50) NOT NULL,
	Name NVARCHAR(200) NOT NULL,
	BrandId INT  NULL,
	GroupStatus int NOT NULL DEFAULT 0; -- 0=Unassigned, 1=Parent, 2=Child
	ProductGroupId INT NULL, -- Grupo de productos (para variantes del mismo producto)
	-- Unidad de Presentación (cómo se compra)
	PresentationUnit INT NULL, -- Enum MeasurementUnit (0=Unit, 1=Milliliter, 2=Liter, 3=FluidOunce, 4=Gallon, 5=Gram, 6=Kilogram, 7=Pound, 8=Ounce)
	PresentationSize DECIMAL(18,3) NULL, -- Tamaño de presentación (ej: 32 para 32oz)
	-- Unidad Base (cómo se registra en inventario)
	Unit INT NULL, -- Enum MeasurementUnit
	Size DECIMAL(18,3) NULL, -- Tamaño en unidad base (ej: 946 para 32oz = 946ml)
	ProductType INT NOT NULL DEFAULT 0, -- 0=Mueble, 1=Limpieza, 2=Consumible
	MinimumStock DECIMAL(18,3) default 1 null,
	Description NVARCHAR(500) NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL,
	CONSTRAINT UC_Products_Sku UNIQUE (Sku)
)
GO

CREATE TABLE ProductCategories
(
	ProductId INT NOT NULL,
	CategoryId INT NOT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL
)
GO

CREATE TABLE ProductGroups
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(200) NOT NULL,
	Description NVARCHAR(500) NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE PropertyTypes
(
	Id INT PRIMARY KEY,
	Name NVARCHAR(200) NOT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE Properties
(
	Id INT PRIMARY KEY,
	TypeId INT NOT NULL,
	Name NVARCHAR(200) NOT NULL,
	Country NVARCHAR(100) NULL,
	City NVARCHAR(100) NULL,
	Street NVARCHAR(200) NULL,
	PersonCapacity INT NULL,
	BedroomsNumber INT NULL,
	BedsNumber INT NULL,
	BathroomsNumber INT NULL,
	GuestBathroomsNumber INT NULL,
	AverageReviewRating DECIMAL(5,2) NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE PropertyImages
(
	Id INT PRIMARY KEY,
	PropertyId INT NOT NULL,
	ItemCaption NVARCHAR(MAX) NULL,
	ItemUrl NVARCHAR(500) NULL,
	SortOrder INT NOT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE Guests
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	FirstName NVARCHAR(100) NOT NULL,
	LastName NVARCHAR(100) NOT NULL,
	Email NVARCHAR(200) NOT NULL,
	PhoneNumber NVARCHAR(50) NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE Inspections
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	CheckType INT NOT NULL, -- 0=checklis (property items + ispriority) 1= inspeccion =todos los propertyitems
	PropertyId INT NOT NULL,
	GuestId INT NULL,
	CheckDate DATETIMEOFFSET NOT NULL,
	CurrentState INT NOT NULL, -- 0=Pendiente, 1=Activo, 2=Completado, 3=Cancelado
	CreatedBy INT NOT NULL,
	UpdatedBy INT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE InspectionRooms
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	InspectionId INT NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	CreatedBy INT NOT NULL,
	UpdatedBy INT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

-- Índices para Inspections
CREATE INDEX IX_Inspections_PropertyId ON Inspections(PropertyId);
CREATE INDEX IX_Inspections_CheckType ON Inspections(CheckType);
CREATE INDEX IX_Inspections_CurrentState ON Inspections(CurrentState);
GO

CREATE TABLE InspectionItems
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	InspectionId INT NOT NULL,
	RoomId INT NOT NULL, -- Referencia a Rooms (ambiente de la propiedad)
	ItemId INT NOT NULL, -- Referencia a Products
	Sku NVARCHAR(50) NOT NULL,
	Name NVARCHAR(200) NULL, -- Nombre histórico del producto en el momento de la inspección
	ProductGroupId INT NULL, -- Grupo de productos (para variantes del mismo producto)
	-- Unidad de Presentación (cómo se compra) - Snapshot histórico
	PresentationUnit INT NULL, -- Enum MeasurementUnit
	PresentationSize DECIMAL(18,3) NULL, -- Tamaño de presentación (ej: 32 para 32oz)
	-- Unidad Base (cómo se registra en inventario) - Snapshot histórico
	Unit INT NULL, -- Enum MeasurementUnit
	IsExists BIT NOT NULL DEFAULT 0,
	Size DECIMAL(18,3) NULL, -- Tamaño en unidad base (ej: 946 para 32oz = 946ml)
	ProductType INT NOT NULL DEFAULT 0, -- 0=Mueble, 1=Limpieza, 2=Consumible
	Condition INT NOT NULL DEFAULT 0, -- 0=Pending, 1=New, 2=Good, 3=Fair, 4=Poor
	Comments NVARCHAR(MAX) NULL,
	CreatedBy INT NOT NULL,
	UpdatedBy INT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL,
)
GO

-- Índices para InspectionItems
CREATE INDEX IX_InspectionItems_InspectionId ON InspectionItems(InspectionId);
CREATE INDEX IX_InspectionItems_RoomId ON InspectionItems(RoomId);
CREATE INDEX IX_InspectionItems_Condition ON InspectionItems(Condition);
GO

CREATE TABLE ItemAttachments
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	ItemId INT NOT NULL, 
	FilePath NVARCHAR(500) NOT NULL,
	CreatedBy INT NOT NULL,
	UpdatedBy INT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO

CREATE TABLE Incidents
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	InspectionId INT NULL, --inspeccion
	PropertyId INT  NULL, --casa
	RoomId INT  NULL, --ambiente
	ItemId INT  NULL, -- item
	CleaningUserId INT NULL, --limpieza asignado
	IssueType INT  NULL, -- 0=Issue, 1=Incident
	IssueDate DATETIMEOFFSET  NULL, 
	Comments NVARCHAR(MAX) NULL,
	Note NVARCHAR(MAX) NULL,
	CurrentState INT NOT NULL, -- 0=Pendiente, 1=EnProceso, 2=Completado, 3=Cancelado
	CreatedBy INT NOT NULL,
	UpdatedBy INT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL
)
GO



-- Tabla de Stores (Almacenes/Tiendas)
CREATE TABLE Stores (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    PropertyId INT NULL, -- NULL para almacén central
    CreatedAt DATETIMEOFFSET NOT NULL,
    UpdatedAt DATETIMEOFFSET NULL
)
GO

-- Tabla de Rooms (Ambientes de cada propiedad)
CREATE TABLE Rooms (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    PropertyId INT NOT NULL,
    CreatedAt DATETIMEOFFSET NOT NULL,
    UpdatedAt DATETIMEOFFSET NULL
)
GO

-- Tabla de PropertyItems (Lo que necesita cada propiedad en cada ambiente)
CREATE TABLE PropertyItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PropertyId INT NOT NULL,
    RoomId INT NOT NULL,
    ProductId INT NULL, -- Producto específico
    ProductGroupId INT NULL, -- O grupo de productos
    StoreId INT NOT NULL, -- Almacén del cual se asigna el producto
    Unit INT NULL, -- Enum MeasurementUnit - Unidad del consumo por evento
    Size DECIMAL(18,3) NULL, -- Tamaño del consumo (ej: 10 para 10oz, 2 para 2 unidades)
    Quantity DECIMAL(18,3) NOT NULL, -- Cantidad total en unidad base
	isPriority BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIMEOFFSET NOT NULL,
    UpdatedAt DATETIMEOFFSET NULL
)
GO

-- Índices para PropertyItems
CREATE INDEX IX_PropertyItems_StoreId ON PropertyItems(StoreId);
CREATE INDEX IX_PropertyItems_ProductId_StoreId ON PropertyItems(ProductId, StoreId);
GO

CREATE TABLE Inventory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    StoreId INT NOT NULL,
    Operation INT NOT NULL, -- 0=Purchase, 1=Sale, 2=Transfer, 3=Consumption, 4=Adjustment, 5=Return
    TransactionType INT NOT NULL, -- 0=In, 1=Out
    UnitCount INT NULL, -- Cantidad exacta de unidades (ej: 10 botellas)
    Quantity DECIMAL(18,3) NOT NULL, -- Cantidad en unidad base mínima (ml, g, etc.)
    Price DECIMAL(18,6) NULL, -- Precio por unidad base (por ml, por g, etc.)
    PricePresentation DECIMAL(18,6) NULL, -- Precio por unidad que manda el cliente (ej: $10 por botella)
    UnitBase INT NULL, -- Enum MeasurementUnit - Unidad base en la que se guarda (0=Unit, 1=Milliliter, etc.)
    UnitPresentation INT NULL, -- Enum MeasurementUnit - Unidad de presentación (3=FluidOunce para 128oz, etc.)
    RelatedStoreId INT NULL, -- Para transferencias
    RelatedInventoryId INT NULL, -- Para transferencias
    Note NVARCHAR(255),
    OrderNumber NVARCHAR(50) NULL,
    CreatedAt DATETIMEOFFSET NOT NULL,
    UpdatedAt DATETIMEOFFSET NULL
)
GO

-- Índices para optimizar consultas
CREATE INDEX IX_Inventory_ProductId ON Inventory(ProductId);
CREATE INDEX IX_Inventory_StoreId ON Inventory(StoreId);
CREATE INDEX IX_Inventory_Operation ON Inventory(Operation);
CREATE INDEX IX_Inventory_TransactionType ON Inventory(TransactionType);
CREATE INDEX IX_Inventory_OrderNumber ON Inventory(OrderNumber) WHERE OrderNumber IS NOT NULL;
GO

-- Tabla de Stock Actual (Existencias por Store/Producto)
CREATE TABLE Stock (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StoreId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity DECIMAL(18,3) NOT NULL DEFAULT 0, -- Total en almacén
    AssignedQuantity DECIMAL(18,6) NOT NULL DEFAULT 0, -- Asignado a PropertyItems
    -- AvailableQuantity = Quantity - AssignedQuantity (calculado en aplicación)
    AveragePrice DECIMAL(18,6) NOT NULL DEFAULT 0, -- Precio promedio por unidad base (6 decimales para precisión)
    TotalValue DECIMAL(18,2) NOT NULL DEFAULT 0, -- Valor total del stock
    LastUpdated DATETIMEOFFSET NOT NULL,
    CONSTRAINT UK_Stock_StoreProduct UNIQUE (StoreId, ProductId)
)
GO

-- Índices para Stock
CREATE INDEX IX_Stock_AssignedQuantity ON Stock(AssignedQuantity) WHERE AssignedQuantity > 0;
GO




-- lista de mis incidencias 
-- 

-- =============================================
-- MAINTENANCE/CLEANING SYSTEM
-- =============================================

-- Tabla de PropertyItemsMaintenance (Productos de limpieza necesarios por ambiente)
-- Similar a PropertyItems pero específicamente para productos de limpieza
CREATE TABLE PropertyItemsMaintenance (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PropertyId INT NOT NULL,
    RoomId INT NULL, -- NULL = aplica a toda la propiedad, o específico por ambiente
    StoreId INT NOT NULL, -- Almacén del cual se obtiene el producto
    ProductId INT NULL, -- Producto específico de limpieza
    ProductGroupId INT NULL, -- O grupo de productos de limpieza
    PresentationUnit INT NULL, -- Enum MeasurementUnit
    PresentationSize DECIMAL(18,3) NULL, -- Tamaño de presentación (ej: 1 para 1kg)
    Unit INT NULL, -- Enum MeasurementUnit - Unidad base
    Size DECIMAL(18,3) NULL, -- Cantidad en unidad base
    IsVariable BIT NOT NULL DEFAULT 0, -- Indica si los valores son fijos o pueden cambiar
    CreatedAt DATETIMEOFFSET NOT NULL,
    UpdatedAt DATETIMEOFFSET NULL
)
GO

-- Índices para PropertyItemsMaintenance
CREATE INDEX IX_PropertyItemsMaintenance_PropertyId ON PropertyItemsMaintenance(PropertyId);
CREATE INDEX IX_PropertyItemsMaintenance_RoomId ON PropertyItemsMaintenance(RoomId);
CREATE INDEX IX_PropertyItemsMaintenance_ProductId ON PropertyItemsMaintenance(ProductId);
GO

-- Tabla de Maintenance (Limpieza/Mantenimiento)
CREATE TABLE Maintenance (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PropertyId INT NOT NULL, -- Propiedad a limpiar
    PropertyName NVARCHAR(200) NULL, -- Snapshot del nombre de la propiedad
    RoomId INT NULL, -- NULL = toda la propiedad, o ambiente específico
    RoomName NVARCHAR(200) NULL, -- Snapshot del nombre del ambiente
    UserCleaningId INT NOT NULL, -- Usuario asignado para limpiar
    MaintenanceType INT NOT NULL DEFAULT 0, -- 0=Limpieza, 1=Mantenimiento, 2=DeepCleaning
    Status INT NOT NULL DEFAULT 0, -- 0=Active, 1=Completed, 2=Cancelled
    Notes NVARCHAR(MAX) NULL,
    CreatedBy INT NOT NULL,
    UpdatedBy INT NULL,
    CreatedAt DATETIMEOFFSET NOT NULL,
    UpdatedAt DATETIMEOFFSET NULL
)
GO

-- Índices para Maintenance
CREATE INDEX IX_Maintenance_PropertyId ON Maintenance(PropertyId);
CREATE INDEX IX_Maintenance_RoomId ON Maintenance(RoomId);
CREATE INDEX IX_Maintenance_UserCleaningId ON Maintenance(UserCleaningId);
CREATE INDEX IX_Maintenance_Status ON Maintenance(Status);
CREATE INDEX IX_Maintenance_MaintenanceType ON Maintenance(MaintenanceType);
GO

-- Tabla de MaintenanceItems (Detalle de productos usados en cada mantenimiento)
-- Similar a InspectionItems pero para registrar consumo de productos de limpieza
CREATE TABLE MaintenanceItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaintenanceId INT NOT NULL, -- Referencia al mantenimiento
    RoomId INT NULL, -- Ambiente donde se usó (puede ser NULL si aplica a toda la propiedad)
    ProductId INT NOT NULL, -- Referencia a Products
    Sku NVARCHAR(50) NOT NULL,
    Name NVARCHAR(200) NULL, -- Nombre histórico del producto en el momento del mantenimiento
    ProductGroupId INT NULL, -- Grupo de productos
    -- Unidad de Presentación (cómo se compra) - Snapshot histórico
    PresentationUnit INT NULL, -- Enum MeasurementUnit
    PresentationSize DECIMAL(18,3) NULL, -- Tamaño de presentación (ej: 1 para 1kg)
    -- Unidad Base (cómo se registra en inventario) - Snapshot histórico
    Unit INT NULL, -- Enum MeasurementUnit
    Size DECIMAL(18,3) NULL, -- Tamaño en unidad base (ej: 1000 para 1kg = 1000g)
    IsVariable BIT NOT NULL DEFAULT 0, -- Indica si los valores son fijos o pueden cambiar
    IsConsumed BIT NOT NULL DEFAULT 0, -- Indica si el producto fue consumido (0=No, 1=Sí)
    ProductType INT NOT NULL DEFAULT 1, -- 0=Mueble, 1=Limpieza, 2=Consumible
    Comments NVARCHAR(MAX) NULL,
    CreatedBy INT NOT NULL,
    UpdatedBy INT NULL,
    CreatedAt DATETIMEOFFSET NOT NULL,
    UpdatedAt DATETIMEOFFSET NULL
)
GO

-- Índices para MaintenanceItems
CREATE INDEX IX_MaintenanceItems_MaintenanceId ON MaintenanceItems(MaintenanceId);
CREATE INDEX IX_MaintenanceItems_RoomId ON MaintenanceItems(RoomId);
CREATE INDEX IX_MaintenanceItems_ProductId ON MaintenanceItems(ProductId);
CREATE INDEX IX_MaintenanceItems_ProductType ON MaintenanceItems(ProductType);
GO


CREATE TABLE excel_tmp (
    id INT PRIMARY KEY IDENTITY,
    propiedadId INT NOT NULL,
    ambienteId INT NULL,
    ambiente NVARCHAR(100) NOT NULL,
    productoId INT NULL,
    nombre NVARCHAR(255) NOT NULL,
    cantidad INT NOT NULL,
    storeId INT NOT NULL
);
GO


-- =============================================
-- INCIDENT ATTACHMENTS
-- =============================================
-- ItemType:
-- 1 = Inmueble (Furniture/Property items)
-- 2 = No Inmueble (Walls, ceiling, floors, etc.)
-- =============================================

CREATE TABLE IncidentAttachments
(
	Id INT PRIMARY KEY IDENTITY(1,1),
	ItemType INT NOT NULL, -- 1=Inmueble, 2=No Inmueble
	ItemId INT NOT NULL, -- IncidentId
	FilePath NVARCHAR(500) NOT NULL,
	CreatedBy INT NOT NULL,
	UpdatedBy INT NULL,
	CreatedAt DATETIMEOFFSET NOT NULL,
	UpdatedAt DATETIMEOFFSET NULL,
)
GO

 CREATE TABLE ProductGroupRelations
    (
        ParentId INT NOT NULL,
        ChildId INT NOT NULL,
        CONSTRAINT PK_ProductGroupRelations PRIMARY KEY (ParentId, ChildId)
    );