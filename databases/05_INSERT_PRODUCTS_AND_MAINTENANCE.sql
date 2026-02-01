-- =============================================
-- INSERT PRODUCTS Y PROPERTY ITEMS MAINTENANCE
-- =============================================

USE DB_BMIAMI;
GO

-- =============================================
-- PRODUCTOS DE LIMPIEZA
-- ProductType: 1 = Cleaning
-- PresentationUnit y PresentationSize: Cómo se compra
-- Unit y Size: Cómo se almacena en inventario (unidad base)
-- MeasurementUnit: 0=Unit, 1=Milliliter, 2=Liter, 3=FluidOunce, 4=Gallon
-- SKU: Continúa desde 0959
-- =============================================
INSERT INTO Products (Sku, Name, BrandId, ProductGroupId, ProductType, PresentationUnit, PresentationSize, Unit, Size, CreatedAt)
VALUES 
-- Papel Higiénico (3 marcas)    12,24,32  | gasta  12 
('0960', 'Papel Higiénico Member''s Mark', 1, NULL, 1, 0, 12, 0, 12, GETDATE()),
('0961', 'Papel Higiénico Charmin', 2, NULL, 1, 0, 24, 0, 24, GETDATE()),
('0962', 'Papel Higiénico Great Value', 3, NULL, 1, 0, 32, 0, 32, GETDATE()),
-- Toallín (3 marcas) 12 | gasta 5
('0963', 'Toallín Member''s Mark', 1, NULL, 1, 0, 12, 0, 12, GETDATE()),
('0964', 'Toallín Charmin', 2, NULL, 1, 0, 12, 0, 12, GETDATE()),
('0965', 'Toallín Great Value', 3, NULL, 1, 0, 12, 0, 12, GETDATE()),
-- Servilletas 500 | gasta 150
('0966', 'Servilletas Great Value', 3, NULL, 1, 0, 500, 0, 500, GETDATE()),
-- Jabón en Cápsulas (2 marcas) | gasta 20
('0967', 'Jabón en Cápsulas Member''s Mark', 1, NULL, 1, 0, 20, 0, 20, GETDATE()),
('0968', 'Jabón en Cápsulas Cascade', 4, NULL, 1, 0, 20, 0, 20, GETDATE()),
-- Enjuague - 128 oz = 3785 ml |  300 ml
('0969', 'Enjuague para Ropa', 10, NULL, 1, 3, 128, 1, 3785, GETDATE()),
-- Cloro Líquido (2 marcas) - 1 galón = 3785 ml | gasta  500 ml
('0970', 'Cloro Líquido Member''s Mark', 1, NULL, 1, 4, 1, 1, 3785, GETDATE()),
('0971', 'Cloro Líquido Clorox', 5, NULL, 1, 4, 1, 1, 3785, GETDATE()),
-- Windex - 23 oz = 680 ml  | gasta 100 ml
('0972', 'Windex Limpiador de Vidrios', 6, NULL, 1, 3, 23, 1, 680, GETDATE()),
-- Cloro Spray Azul - 32 oz = 946 ml |  gasta 150 ml
('0973', 'Cloro Spray Azul Clorox Clean Up', 5, NULL, 1, 3, 32, 1, 946, GETDATE()),
-- Cloro Spray Verde - 32 oz = 946 ml  | gasta 150ml
('0974', 'Cloro Spray Verde Clorox Clean Up', 5, NULL, 1, 3, 32, 1, 946, GETDATE()),
-- Fabuloso (2 presentaciones) | gasta 150 ml
('0975', 'Fabuloso Desinfectante 128oz', 7, NULL, 1, 3, 128, 1, 3785, GETDATE()),
('0976', 'Fabuloso Desinfectante 32oz', 7, NULL, 1, 3, 32, 1, 946, GETDATE()),
-- Cápsulas Dishwasher (2 marcas) | gasta 5
('0977', 'Cápsulas Dishwasher Member''s Mark', 1, NULL, 1, 0, 20, 0, 20, GETDATE()),
('0978', 'Cápsulas Dishwasher Cascade', 4, NULL, 1, 0, 80, 0, 80, GETDATE()),
-- Producto para Metales - 15 oz = 444 ml | gasta 1 oz
('0979', 'Sprayaway Limpiador de Metales', 10, NULL, 1, 3, 15, 1, 444, GETDATE()),
-- Desengrasante - 32 oz = 946 ml | gasta 6 oz
('0980', 'Easy Off Desengrasante', 10, NULL, 1, 3, 32, 1, 946, GETDATE()),
-- Desmanchador - 32 oz = 946 ml | gasta  1.5
('0981', 'Tide Desmanchador', 8, NULL, 1, 3, 32, 1, 946, GETDATE()),
-- Crema de Dientes | gasta 3 
('0982', 'Crema de Dientes', 10, NULL, 1, 0, 72, 0, 72, GETDATE()),
-- Body Wash - 32 oz = 946 ml | gasta 300 ml
('0983', 'Body Wash', 10, NULL, 1, 3, 32, 1, 946, GETDATE()),
-- Champú - 10 litros = 10000 ml | gasta 300 ml
('0984', 'Champú', 10, NULL, 1, 2, 10, 1, 10000, GETDATE()),
-- Enjuague para el Cabello - 10 litros = 10000 ml | gasta 300 ml
('0985', 'Enjuague para el Cabello', 10, NULL, 1, 2, 10, 1, 10000, GETDATE()),
-- Lysol Spray - 19 oz = 561 ml | gasta 8 oz
('0986', 'Lysol Desinfectante Spray', 9, NULL, 1, 3, 19, 1, 561, GETDATE()),
-- Lysol Limpiador - 32 oz = 946 ml | gasta 3 oz
('0987', 'Lysol Limpiador de Superficies', 9, NULL, 1, 3, 32, 1, 946, GETDATE()),
-- PAPEL ALUMINIO  - 12 U = 12 ml | gasta 3 U
('0988', 'Papel Aluminio', 9, NULL, 1, 0, 12, 0, 12, GETDATE()),
-- Guantes Desechables - 2 paquetes 300 unidades | gasta 4 unidades
('0989', 'Guantes Desechables', 10, NULL, 1, 0, 300, 0, 300, GETDATE()),
-- Esponjas - paquete 24 | gasta 1 unidad
('0990', 'Esponjas de Lavaplatos', 10, NULL, 1, 0, 24, 0, 24, GETDATE()),
-- Pilas para cámaras - paquete 18 | gasta 8 pares
('0991', 'Pilas para Cámaras y Controles', 10, NULL, 1, 0, 18, 0, 18, GETDATE()),
-- Jabón para Manos - 32 oz = 946 ml | gasta 300 ml
('0992', 'Jabón para Manos', 10, NULL, 1, 3, 32, 1, 946, GETDATE()),
-- Esponjas Mágicas - paquete 5 o 7 | gasta 5 o 7 unidades
('0993', 'Esponjas Mágicas', 10, NULL, 1, 0, 7, 0, 7, GETDATE()),
-- Bolsas de Basura 13 Galones - paquete 200 u 80 unidades | gasta 12 unidades
('0994', 'Bolsas de Basura Galones 200u', 10, NULL, 1, 0, 200, 0, 200, GETDATE()),
('0995', 'Bolsas de Basura Galones 80u', 10, NULL, 1, 0, 80, 0, 80, GETDATE()),
-- Bolsas de Basura 4-8 Galones - paquete 40 unidades | gasta 15 unidades
('0996', 'Bolsas de Basura 4-8 Galones', 10, NULL, 1, 0, 40, 0, 40, GETDATE()),
-- Jabón Lavaplatos Líquido - 32 oz = 946 ml | gasta 150 ml
('0997', 'Jabón Lavaplatos Líquido', 10, NULL, 1, 3, 32, 1, 946, GETDATE()),
-- Jabón Lavaplatos Eléctrico Pods - 60-80-100 unidades | gasta 6 unidades
('0998', 'Jabón Lavaplatos Eléctrico Pods Member''s Mark 80u', 1, NULL, 1, 0, 80, 0, 80, GETDATE()),
('0999', 'Jabón Lavaplatos Eléctrico Pods Cascade 60u', 4, NULL, 1, 0, 60, 0, 60, GETDATE()),
('1000', 'Jabón Lavaplatos Eléctrico Pods Cascade 100u', 4, NULL, 1, 0, 100, 0, 100, GETDATE()),
-- Bandejas de Parrilleras - paquete 2 o 18 unidades | gasta 1 unidad
('1001', 'Bandejas de Parrilleras', 10, NULL, 1, 0, 18, 0, 18, GETDATE()),
-- Air Wick - paquete 5 | gasta 1 unidad
('1002', 'Air Wick Enchufe', 10, NULL, 1, 0, 5, 0, 5, GETDATE()),
-- Febreeze - paquete 2 unidades 8 oz | gasta 4 oz (40c)
('1003', 'Febreeze Spray', 10, NULL, 1, 3, 8, 1, 260, GETDATE()),
-- Roll On para Pelusas - paquete 2 unidades | gasta 0.5 oz
('1004', 'Roll On para Pelusas', 10, NULL, 1, 0, 2, 0, 2, GETDATE());

GO

-- =============================================
-- CREAR ROOM DE PRUEBA Y PROPERTY ITEMS MAINTENANCE
-- =============================================

-- Room de prueba para la propiedad 453595
INSERT INTO Rooms (Name, PropertyId, CreatedAt)
VALUES ('Room de Prueba', 453595, GETDATE());

DECLARE @RoomIdPrueba INT = SCOPE_IDENTITY();

-- =============================================
-- PROPERTY ITEMS MAINTENANCE
-- Consumo estimado por limpieza/estadía
-- PresentationUnit y PresentationSize: Lo que se consume (según archivo 05)
-- Unit y Size: Unidad mínima de almacenamiento (ml para líquidos, unidades para sólidos)
-- MeasurementUnit: 0=Unit, 1=Milliliter, 2=Liter, 3=FluidOunce, 4=Gallon
-- =============================================

-- Propiedad 453595 - Store 21 - Room de Prueba
-- Todos los items asignados al Room de Prueba

-- Papel Higiénico - Consumo: 12 rollos
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0960'), NULL, 0, 12, 0, 12, 1, GETDATE()),
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0961'), NULL, 0, 12, 0, 12, 1, GETDATE()),
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0962'), NULL, 0, 12, 0, 12, 1, GETDATE());

-- Toallín - Consumo: 5 rollos
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0963'), NULL, 0, 5, 0, 5, 1, GETDATE()),
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0964'), NULL, 0, 5, 0, 5, 1, GETDATE()),
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0965'), NULL, 0, 5, 0, 5, 1, GETDATE());

-- Servilletas - Consumo: 150 unidades
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0966'), NULL, 0, 150, 0, 150, 1, GETDATE());

-- Jabón en Cápsulas - Consumo: 20 cápsulas
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0967'), NULL, 0, 20, 0, 20, 1, GETDATE()),
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0968'), NULL, 0, 20, 0, 20, 1, GETDATE());

-- Enjuague - Consumo: 300 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0969'), NULL, 1, 300, 1, 300, 1, GETDATE());

-- Cloro Líquido - Consumo: 500 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0970'), NULL, 1, 500, 1, 500, 1, GETDATE()),
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0971'), NULL, 1, 500, 1, 500, 1, GETDATE());

-- Windex - Consumo: 100 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0972'), NULL, 1, 100, 1, 100, 1, GETDATE());

-- Cloro Spray Azul - Consumo: 150 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0973'), NULL, 1, 150, 1, 150, 1, GETDATE());

-- Cloro Spray Verde - Consumo: 150 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0974'), NULL, 1, 150, 1, 150, 1, GETDATE());

-- Fabuloso - Consumo: 150 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0975'), NULL, 1, 150, 1, 150, 1, GETDATE()),
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0976'), NULL, 1, 150, 1, 150, 1, GETDATE());

-- Cápsulas Dishwasher - Consumo: 5 cápsulas
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0977'), NULL, 0, 5, 0, 5, 1, GETDATE()),
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0978'), NULL, 0, 5, 0, 5, 1, GETDATE());

-- Producto para Metales - Consumo: 1 oz (30 ml)
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0979'), NULL, 3, 1, 1, 30, 1, GETDATE());

-- Desengrasante - Consumo: 6 oz (177 ml)
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0980'), NULL, 3, 6, 1, 177, 1, GETDATE());

-- Desmanchador Tide - Consumo: 1.5 oz (44 ml) - COMENTADO POR DECIMALES
-- INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
-- VALUES 
-- (453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0981'), NULL, 3, 1.5, 1, 44, 1, GETDATE());

-- Crema de Dientes - Consumo: 3 unidades
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0982'), NULL, 0, 3, 0, 3, 1, GETDATE());

-- Body Wash - Consumo: 300 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0983'), NULL, 1, 300, 1, 300, 1, GETDATE());

-- Champú - Consumo: 300 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0984'), NULL, 1, 300, 1, 300, 1, GETDATE());

-- Enjuague para el Cabello - Consumo: 300 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0985'), NULL, 1, 300, 1, 300, 1, GETDATE());

-- Lysol Spray - Consumo: 8 oz (236 ml)
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0986'), NULL, 3, 8, 1, 236, 1, GETDATE());

-- Lysol Limpiador - Consumo: 3 oz (89 ml)
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES 
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0987'), NULL, 3, 3, 1, 89, 1, GETDATE());

-- Papel Aluminio - Consumo: 3 metros
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0988'), NULL, 0, 3, 0, 3, 1, GETDATE());

-- Guantes Desechables - Consumo: 4 unidades
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0989'), NULL, 0, 4, 0, 4, 1, GETDATE());

-- Esponjas - Consumo: 1 unidad
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0990'), NULL, 0, 1, 0, 1, 1, GETDATE());

-- Pilas - Consumo: 8 pares
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0991'), NULL, 0, 8, 0, 8, 1, GETDATE());

-- Jabón para Manos - Consumo: 300 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0992'), NULL, 1, 300, 1, 300, 1, GETDATE());

-- Esponjas Mágicas - Consumo: 5 unidades
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0993'), NULL, 0, 5, 0, 5, 1, GETDATE());

-- Bolsas de Basura 13 Galones 200u - Consumo: 12 unidades
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0994'), NULL, 0, 12, 0, 12, 1, GETDATE());

-- Bolsas de Basura 13 Galones 80u - Consumo: 12 unidades
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0995'), NULL, 0, 12, 0, 12, 1, GETDATE());

-- Bolsas de Basura 4-8 Galones - Consumo: 15 unidades
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0996'), NULL, 0, 15, 0, 15, 1, GETDATE());

-- Jabón Lavaplatos Líquido - Consumo: 150 ml
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0997'), NULL, 1, 150, 1, 150, 1, GETDATE());

-- Jabón Lavaplatos Eléctrico Pods 80u - Consumo: 6 unidades
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0998'), NULL, 0, 6, 0, 6, 1, GETDATE());

-- Jabón Lavaplatos Eléctrico Pods 60u - Consumo: 6 unidades
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '0999'), NULL, 0, 6, 0, 6, 1, GETDATE());

-- Jabón Lavaplatos Eléctrico Pods 100u - Consumo: 6 unidades
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '1000'), NULL, 0, 6, 0, 6, 1, GETDATE());

-- Bandejas de Parrilleras - Consumo: 1 unidad
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '1001'), NULL, 0, 1, 0, 1, 1, GETDATE());

-- Air Wick - Consumo: 1 unidad
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '1002'), NULL, 0, 1, 0, 1, 1, GETDATE());

-- Febreeze - Consumo: 4 oz (118 ml)
INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
VALUES
(453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '1003'), NULL, 3, 4, 1, 118, 1, GETDATE());

-- Roll On para Pelusas - Consumo: 0.5 unidad - COMENTADO POR DECIMALES
-- INSERT INTO PropertyItemsMaintenance (PropertyId, RoomId, StoreId, ProductId, ProductGroupId, PresentationUnit, PresentationSize, Unit, Size, IsVariable, CreatedAt)
-- VALUES
-- (453595, @RoomIdPrueba, 21, (SELECT Id FROM Products WHERE Sku = '1004'), NULL, 0, 0.5, 0, 0.5, 1, GETDATE());

-- =============================================
-- INVENTORY - COMPRAS INICIALES
-- Operation: 0=Purchase
-- TransactionType: 0=In
-- =============================================

-- Papel Higiénico
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0960'), 21, 0, 0, 10, 120, 0.50, 6.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET()),
((SELECT Id FROM Products WHERE Sku = '0961'), 21, 0, 0, 5, 120, 0.60, 14.40, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET()),
((SELECT Id FROM Products WHERE Sku = '0962'), 21, 0, 0, 5, 160, 0.45, 14.40, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());

-- Toallín
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0963'), 21, 0, 0, 10, 120, 0.40, 4.80, 0, 0, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '0964'), 21, 0, 0, 5, 60, 0.50, 6.00, 0, 0, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '0965'), 21, 0, 0, 5, 60, 0.35, 4.20, 0, 0, 'Compra inicial', GETDATE());

-- Servilletas
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0966'), 21, 0, 0, 5, 2500, 0.004, 2.00, 0, 0, 'Compra inicial', GETDATE());

-- Jabón en Cápsulas
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0967'), 21, 0, 0, 10, 200, 0.30, 6.00, 0, 0, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '0968'), 21, 0, 0, 10, 200, 0.35, 7.00, 0, 0, 'Compra inicial', GETDATE());

-- Enjuague
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0969'), 21, 0, 0, 5, 18925, 0.0026, 10.00, 1, 3, 'Compra inicial', GETDATE());

-- Cloro Líquido
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0970'), 21, 0, 0, 10, 37850, 0.0013, 5.00, 1, 4, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '0971'), 21, 0, 0, 10, 37850, 0.0015, 5.50, 1, 4, 'Compra inicial', GETDATE());

-- Windex
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0972'), 21, 0, 0, 10, 6800, 0.0044, 3.00, 1, 3, 'Compra inicial', GETDATE());

-- Cloro Spray
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0973'), 21, 0, 0, 10, 9460, 0.0042, 4.00, 1, 3, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '0974'), 21, 0, 0, 10, 9460, 0.0042, 4.00, 1, 3, 'Compra inicial', GETDATE());

-- Fabuloso
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0975'), 21, 0, 0, 5, 18925, 0.0026, 10.00, 1, 3, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '0976'), 21, 0, 0, 10, 9460, 0.0032, 3.00, 1, 3, 'Compra inicial', GETDATE());

-- Cápsulas Dishwasher
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0977'), 21, 0, 0, 10, 200, 0.30, 6.00, 0, 0, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '0978'), 21, 0, 0, 5, 400, 0.25, 20.00, 0, 0, 'Compra inicial', GETDATE());

-- Producto para Metales
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0979'), 21, 0, 0, 10, 4440, 0.0068, 3.00, 1, 3, 'Compra inicial', GETDATE());

-- Desengrasante
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0980'), 21, 0, 0, 10, 9460, 0.0053, 5.00, 1, 3, 'Compra inicial', GETDATE());

-- Desmanchador
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0981'), 21, 0, 0, 10, 9460, 0.0042, 4.00, 1, 3, 'Compra inicial', GETDATE());

-- Crema de Dientes
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0982'), 21, 0, 0, 5, 360, 0.05, 18.00, 0, 0, 'Compra inicial', GETDATE());

-- Body Wash
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0983'), 21, 0, 0, 10, 9460, 0.0032, 3.00, 1, 3, 'Compra inicial', GETDATE());

-- Champú
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0984'), 21, 0, 0, 3, 30000, 0.0020, 60.00, 1, 2, 'Compra inicial', GETDATE());

-- Enjuague para el Cabello
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0985'), 21, 0, 0, 3, 30000, 0.0020, 60.00, 1, 2, 'Compra inicial', GETDATE());

-- Lysol
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0986'), 21, 0, 0, 10, 5610, 0.0071, 4.00, 1, 3, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '0987'), 21, 0, 0, 10, 9460, 0.0042, 4.00, 1, 3, 'Compra inicial', GETDATE());

-- Papel Aluminio
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0988'), 21, 0, 0, 10, 120, 0.25, 3.00, 0, 0, 'Compra inicial', GETDATE());

-- Guantes
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0989'), 21, 0, 0, 5, 1500, 0.01, 15.00, 0, 0, 'Compra inicial', GETDATE());

-- Esponjas
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0990'), 21, 0, 0, 10, 240, 0.025, 6.00, 0, 0, 'Compra inicial', GETDATE());

-- Pilas
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0991'), 21, 0, 0, 5, 90, 0.22, 20.00, 0, 0, 'Compra inicial', GETDATE());

-- Jabón para Manos
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0992'), 21, 0, 0, 10, 9460, 0.0032, 3.00, 1, 3, 'Compra inicial', GETDATE());

-- Esponjas Mágicas
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0993'), 21, 0, 0, 10, 70, 0.14, 10.00, 0, 0, 'Compra inicial', GETDATE());

-- Bolsas de Basura 13 Galones
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0994'), 21, 0, 0, 5, 1000, 0.015, 15.00, 0, 0, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '0995'), 21, 0, 0, 5, 400, 0.0188, 7.50, 0, 0, 'Compra inicial', GETDATE());

-- Bolsas de Basura 4-8 Galones
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0996'), 21, 0, 0, 10, 400, 0.0125, 5.00, 0, 0, 'Compra inicial', GETDATE());

-- Jabón Lavaplatos Líquido
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0997'), 21, 0, 0, 10, 9460, 0.0032, 3.00, 1, 3, 'Compra inicial', GETDATE());

-- Jabón Lavaplatos Pods
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '0998'), 21, 0, 0, 5, 400, 0.0375, 15.00, 0, 0, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '0999'), 21, 0, 0, 5, 300, 0.0333, 10.00, 0, 0, 'Compra inicial', GETDATE()),
((SELECT Id FROM Products WHERE Sku = '1000'), 21, 0, 0, 5, 500, 0.04, 20.00, 0, 0, 'Compra inicial', GETDATE());

-- Bandejas
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '1001'), 21, 0, 0, 10, 180, 0.0556, 10.00, 0, 0, 'Compra inicial', GETDATE());

-- Air Wick
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '1002'), 21, 0, 0, 10, 50, 0.20, 10.00, 0, 0, 'Compra inicial', GETDATE());

-- Febreeze
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '1003'), 21, 0, 0, 10, 2360, 0.0085, 2.00, 1, 3, 'Compra inicial', GETDATE());

-- Roll On
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES
((SELECT Id FROM Products WHERE Sku = '1004'), 21, 0, 0, 10, 20, 0.25, 5.00, 0, 0, 'Compra inicial', GETDATE());

GO

-- =============================================
-- STOCK - ESTADO INICIAL
-- Quantity: Total en almacén
-- AssignedQuantity: 0 (aún no asignado)
-- AveragePrice: Precio promedio por unidad base
-- TotalValue: Quantity * AveragePrice
-- =============================================

-- Papel Higiénico
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0960'), 120, 0, 0.50, 60.00, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0961'), 120, 0, 0.60, 72.00, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0962'), 160, 0, 0.45, 72.00, SYSDATETIMEOFFSET());

-- Toallín
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0963'), 120, 0, 0.40, 48.00, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0964'), 60, 0, 0.50, 30.00, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0965'), 60, 0, 0.35, 21.00, SYSDATETIMEOFFSET());

-- Servilletas
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0966'), 2500, 0, 0.004, 10.00, SYSDATETIMEOFFSET());

-- Jabón en Cápsulas
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0967'), 200, 0, 0.30, 60.00, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0968'), 200, 0, 0.35, 70.00, SYSDATETIMEOFFSET());

-- Enjuague
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0969'), 18925, 0, 0.0026, 49.21, SYSDATETIMEOFFSET());

-- Cloro Líquido
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0970'), 37850, 0, 0.0013, 49.21, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0971'), 37850, 0, 0.0015, 56.78, SYSDATETIMEOFFSET());

-- Windex
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0972'), 6800, 0, 0.0044, 29.92, SYSDATETIMEOFFSET());

-- Cloro Spray
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0973'), 9460, 0, 0.0042, 39.73, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0974'), 9460, 0, 0.0042, 39.73, SYSDATETIMEOFFSET());

-- Fabuloso
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0975'), 18925, 0, 0.0026, 49.21, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0976'), 9460, 0, 0.0032, 30.27, SYSDATETIMEOFFSET());

-- Cápsulas Dishwasher
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0977'), 200, 0, 0.30, 60.00, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0978'), 400, 0, 0.25, 100.00, SYSDATETIMEOFFSET());

-- Producto para Metales
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0979'), 4440, 0, 0.0068, 30.19, SYSDATETIMEOFFSET());

-- Desengrasante
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0980'), 9460, 0, 0.0053, 50.14, SYSDATETIMEOFFSET());

-- Desmanchador
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0981'), 9460, 0, 0.0042, 39.73, SYSDATETIMEOFFSET());

-- Crema de Dientes
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0982'), 360, 0, 0.05, 18.00, SYSDATETIMEOFFSET());

-- Body Wash
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0983'), 9460, 0, 0.0032, 30.27, SYSDATETIMEOFFSET());

-- Champú
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0984'), 30000, 0, 0.0020, 60.00, SYSDATETIMEOFFSET());

-- Enjuague para el Cabello
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0985'), 30000, 0, 0.0020, 60.00, SYSDATETIMEOFFSET());

-- Lysol
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0986'), 5610, 0, 0.0071, 39.83, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0987'), 9460, 0, 0.0042, 39.73, SYSDATETIMEOFFSET());

-- Papel Aluminio
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0988'), 120, 0, 0.25, 30.00, SYSDATETIMEOFFSET());

-- Guantes
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0989'), 1500, 0, 0.01, 15.00, SYSDATETIMEOFFSET());

-- Esponjas
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0990'), 240, 0, 0.025, 6.00, SYSDATETIMEOFFSET());

-- Pilas
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0991'), 90, 0, 0.22, 19.80, SYSDATETIMEOFFSET());

-- Jabón para Manos
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0992'), 9460, 0, 0.0032, 30.27, SYSDATETIMEOFFSET());

-- Esponjas Mágicas
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0993'), 70, 0, 0.14, 9.80, SYSDATETIMEOFFSET());

-- Bolsas de Basura 13 Galones
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0994'), 1000, 0, 0.015, 15.00, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0995'), 400, 0, 0.0188, 7.52, SYSDATETIMEOFFSET());

-- Bolsas de Basura 4-8 Galones
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0996'), 400, 0, 0.0125, 5.00, SYSDATETIMEOFFSET());

-- Jabón Lavaplatos Líquido
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0997'), 9460, 0, 0.0032, 30.27, SYSDATETIMEOFFSET());

-- Jabón Lavaplatos Pods
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '0998'), 400, 0, 0.0375, 15.00, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '0999'), 300, 0, 0.0333, 9.99, SYSDATETIMEOFFSET()),
(21, (SELECT Id FROM Products WHERE Sku = '1000'), 500, 0, 0.04, 20.00, SYSDATETIMEOFFSET());

-- Bandejas
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '1001'), 180, 0, 0.0556, 10.01, SYSDATETIMEOFFSET());

-- Air Wick
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '1002'), 50, 0, 0.20, 10.00, SYSDATETIMEOFFSET());

-- Febreeze
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '1003'), 2360, 0, 0.0085, 20.06, SYSDATETIMEOFFSET());

-- Roll On
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES
(21, (SELECT Id FROM Products WHERE Sku = '1004'), 20, 0, 0.25, 5.00, SYSDATETIMEOFFSET());

GO

PRINT 'Productos, PropertyItemsMaintenance, Inventory y Stock insertados correctamente';

-- =============================================
-- INVENTORY Y STOCK PARA PRODUCTOS DE MUEBLES (SKU 0001-0020)
-- Store 21 - Compra inicial
-- =============================================

-- Insertar compras en Inventory para productos 0001-0020
DECLARE @StoreId INT = 21;
DECLARE @ProductId INT;
DECLARE @Quantity DECIMAL(18,3) = 10; -- 10 unidades de cada producto
DECLARE @Price DECIMAL(18,6) = 50.00; -- $50 por unidad
DECLARE @TotalValue DECIMAL(18,2);


-- ESCRITORIO CON SILLA (0080)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0080');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 150.00, 150.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 150.00, 1500.00, SYSDATETIMEOFFSET());

-- ADORNOS DE BOLA DE CENTRO (0081)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0081');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 15.00, 15.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 15.00, 150.00, SYSDATETIMEOFFSET());

-- REPISAS PEQUEÑAS (0082)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0082');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 25.00, 25.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 25.00, 250.00, SYSDATETIMEOFFSET());

-- CUADROS MEDIANOS (0083)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0083');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 30.00, 30.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 30.00, 300.00, SYSDATETIMEOFFSET());

-- CUADROS PEQUEÑOS (0084)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0084');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 20.00, 20.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 20.00, 200.00, SYSDATETIMEOFFSET());

-- SOFA CAMA EN L (0085)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0085');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 500.00, 500.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 500.00, 5000.00, SYSDATETIMEOFFSET());

-- ALFOMBRA MEDIANA (0086)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0086');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 40.00, 40.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 40.00, 400.00, SYSDATETIMEOFFSET());

-- COJINES MEDIANOS (0087)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0087');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 12.00, 12.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 12.00, 120.00, SYSDATETIMEOFFSET());

-- COBIJA (0088)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0088');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 25.00, 25.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 25.00, 250.00, SYSDATETIMEOFFSET());

-- COLCHA (0089)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0089');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 35.00, 35.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 35.00, 350.00, SYSDATETIMEOFFSET());

-- ALMOHADAS (0090)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0090');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 15.00, 15.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 15.00, 150.00, SYSDATETIMEOFFSET());

-- TOALLAS (0091)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0091');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 10.00, 10.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 10.00, 100.00, SYSDATETIMEOFFSET());

-- SABANA LISA (0092)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0092');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 18.00, 18.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 18.00, 180.00, SYSDATETIMEOFFSET());

-- ALFOMBRAS DE RECIBIDOR (0093)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0093');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 30.00, 30.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 30.00, 300.00, SYSDATETIMEOFFSET());

-- MESA DE FUTBOL (0094)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0094');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 200.00, 200.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 200.00, 2000.00, SYSDATETIMEOFFSET());

-- MESA DE JUEGO ELECTRONICO (0095)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0095');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 250.00, 250.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 250.00, 2500.00, SYSDATETIMEOFFSET());

-- CORTINEROS INDIVIDUALES (0096)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0096');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 20.00, 20.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 20.00, 200.00, SYSDATETIMEOFFSET());

-- JUEGOS DE CORTINAS VERDE DE GAMUZA (0097)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0097');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 45.00, 45.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 45.00, 450.00, SYSDATETIMEOFFSET());

-- JUEGOS DE CORTINAS BLANCA EN TUL (0098)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0098');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 40.00, 40.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 40.00, 400.00, SYSDATETIMEOFFSET());

-- COOLER MARCA COLLEMAN (0099)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0099');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 60.00, 60.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 60.00, 600.00, SYSDATETIMEOFFSET());

-- CESTA DE MIMBRE PARA LA PISCINA (0100)
SET @ProductId = (SELECT Id FROM Products WHERE Sku = '0100');
INSERT INTO Inventory (ProductId, StoreId, Operation, TransactionType, UnitCount, Quantity, Price, PricePresentation, UnitBase, UnitPresentation, Note, CreatedAt)
VALUES (@ProductId, @StoreId, 0, 0, 10, 10, 35.00, 35.00, 0, 0, 'Compra inicial', SYSDATETIMEOFFSET());
INSERT INTO Stock (StoreId, ProductId, Quantity, AssignedQuantity, AveragePrice, TotalValue, LastUpdated)
VALUES (@StoreId, @ProductId, 10, 0, 35.00, 350.00, SYSDATETIMEOFFSET());

GO

PRINT 'Inventory y Stock para productos 0080-0100 insertados correctamente en Store 21';
