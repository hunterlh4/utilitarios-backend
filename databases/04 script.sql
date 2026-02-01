



DECLARE @SkuBase INT;

SELECT @SkuBase = ISNULL(MAX(CAST(Sku AS INT)), 0)
FROM Products;

WITH ProductosExcel AS (
    SELECT
        MIN(id) AS OrdenExcel,
        LTRIM(RTRIM(nombre)) AS nombre
    FROM excel_tmp
    WHERE nombre IS NOT NULL
    GROUP BY LTRIM(RTRIM(nombre))
),
ProductosNuevos AS (
    SELECT
        RIGHT('0000' + CAST(
            @SkuBase + ROW_NUMBER() OVER (ORDER BY OrdenExcel) AS VARCHAR(4)
        ), 4) AS Sku,
        nombre
    FROM ProductosExcel e
    WHERE NOT EXISTS (
        SELECT 1
        FROM Products p
        WHERE p.Name = e.nombre
    )
)
INSERT INTO Products
(
    Sku,
    Name,
    BrandId,
    ProductGroupId,
    PresentationUnit,
    PresentationSize,
    Unit,
    Size,
    ProductType,
    Description,
    CreatedAt
)
SELECT
    Sku,
    nombre,
    1,                       -- BrandId
    NULL,                    -- ProductGroupId
    0,                       -- PresentationUnit = Unit (por unidad)
    1,                       -- PresentationSize = 1 (1 unidad)
    0,                       -- Unit = Unit (unidad base)
    1,                       -- Size = 1 (1 unidad de presentación = 1 unidad base)
    0,                       -- ProductType = Property (inmuebles)
    NULL,                    -- Description
    GETDATE()                -- CreatedAt
FROM ProductosNuevos;


INSERT INTO Rooms (Name, PropertyId, CreatedAt)
SELECT
    ambiente,
    propiedadId,
    GETDATE()
FROM (
    SELECT
        id,
        propiedadId,
        LTRIM(RTRIM(ambiente)) AS ambiente,
        ROW_NUMBER() OVER (
            PARTITION BY propiedadId, LTRIM(RTRIM(ambiente))
            ORDER BY id
        ) AS rn
    FROM excel_tmp
    WHERE ambiente IS NOT NULL
) x
WHERE rn = 1
ORDER BY id;


SELECT 
    e.id,
    e.propiedadId,
    e.ambienteId AS ExcelAmbienteId,
    r.Id AS RoomId,
    e.ambiente AS ExcelAmbiente,
    r.Name AS RoomName,
    CASE 
        WHEN e.ambienteId = r.Id THEN 'OK'
        WHEN e.ambienteId IS NULL THEN 'Excel sin ID'
        WHEN r.Id IS NULL THEN 'Room no existe'
        ELSE 'IDs diferentes'
    END AS Estado
FROM excel_tmp e
LEFT JOIN Rooms r
    ON r.PropertyId = e.propiedadId
    AND LTRIM(RTRIM(r.Name)) = LTRIM(RTRIM(e.ambiente))
WHERE e.ambienteId != r.Id 
   OR e.ambienteId IS NULL 
   OR r.Id IS NULL
ORDER BY e.id;


WITH ComprasIniciales AS (
    SELECT
        e.storeId                AS StoreId,
        p.Id                     AS ProductId,
        SUM(e.cantidad)          AS Quantity,
        DENSE_RANK() OVER (ORDER BY e.storeId) AS StoreOrder
    FROM excel_tmp e
    JOIN Products p
        ON LTRIM(RTRIM(p.Name)) = LTRIM(RTRIM(e.nombre))
    GROUP BY
        e.storeId,
        p.Id
)
INSERT INTO Inventory
(
    ProductId,
    StoreId,
    Operation,
    TransactionType,
    UnitCount,
    Quantity,
    Price,
    PricePresentation,
    UnitBase,
    UnitPresentation,
    RelatedStoreId,
    RelatedInventoryId,
    Note,
    OrderNumber,
    CreatedAt
)
SELECT
    ProductId,
    StoreId,
    0 AS Operation,                             
    0 AS TransactionType,                       
    Quantity AS UnitCount,                       
    Quantity AS Quantity,                       
    0 AS Price,                               
    0 AS PricePresentation,                     
    0 AS UnitBase,                              
    0 AS UnitPresentation,                     
    NULL AS RelatedStoreId,
    NULL AS RelatedInventoryId,
    'Inventario inicial' AS Note,
    'MIG-' + RIGHT('000' + CAST(StoreOrder AS VARCHAR(3)), 3) AS OrderNumber,
    SYSDATETIMEOFFSET() AS CreatedAt
FROM ComprasIniciales
ORDER BY StoreId, ProductId;


WITH StockInicial AS (
    SELECT
        inv.StoreId,
        inv.ProductId,
        SUM(CASE WHEN inv.TransactionType = 0 THEN inv.Quantity ELSE -inv.Quantity END) AS Quantity
    FROM Inventory inv
    WHERE inv.Note LIKE 'Inventario inicial'
    GROUP BY
        inv.StoreId,
        inv.ProductId
)
INSERT INTO Stock
(
    StoreId,
    ProductId,
    Quantity,
    AssignedQuantity,
    AveragePrice,
    TotalValue,
    LastUpdated
)
SELECT
    StoreId,
    ProductId,
    Quantity,
    0 AS AssignedQuantity,      
    0 AS AveragePrice,
    0 AS TotalValue,
    SYSDATETIMEOFFSET() AS LastUpdated
FROM StockInicial;


INSERT INTO PropertyItems
(
    PropertyId,
    RoomId,
    ProductId,
    ProductGroupId,
    StoreId,
    Unit,
    Size,
    Quantity,
    isPriority,
    CreatedAt
)
SELECT
    e.propiedadId            AS PropertyId,
    e.ambienteId             AS RoomId,
    p.Id                     AS ProductId,
    NULL                     AS ProductGroupId,
    e.storeId                AS StoreId,
    0                        AS Unit,            
    e.cantidad               AS Size,             
    e.cantidad               AS Quantity,         
    0                        AS isPriority,
    GETDATE()                AS CreatedAt
FROM excel_tmp e
JOIN Products p
    ON LTRIM(RTRIM(p.Name)) = LTRIM(RTRIM(e.nombre))
WHERE e.ambienteId IS NOT NULL;


UPDATE s
SET s.AssignedQuantity = ISNULL(pi.TotalAssigned, 0)
FROM Stock s
LEFT JOIN (
    SELECT
        StoreId,
        ProductId,
        SUM(Quantity) AS TotalAssigned
    FROM PropertyItems
    GROUP BY StoreId, ProductId
) pi
    ON s.StoreId = pi.StoreId
    AND s.ProductId = pi.ProductId;

