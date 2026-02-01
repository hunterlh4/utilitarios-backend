# Guía de Registro de Productos

Esta guía explica cómo registrar correctamente productos en el sistema según su tipo y unidad de medida.

## 📋 Tipos de Productos

### ProductType (Enum)
- **0 = Property/Furniture** (Inmuebles/Muebles): Toallas, sábanas, almohadas, muebles
- **1 = Cleaning** (Limpieza): Productos de limpieza y consumibles
- **2 = Consumable** (Consumibles): Productos que se consumen

## 📏 Unidades de Medida (MeasurementUnit Enum)

```
0 = Unit (unidad, piezas, rollos)
1 = Milliliter (ml)
2 = Liter (L)
3 = FluidOunce (oz líquidas)
4 = Gallon (gal)
5 = Gram (g)
6 = Kilogram (kg)
7 = Pound (lb)
8 = Ounce (oz peso)
```

## 🎯 Estructura de un Producto

Cada producto tiene dos conjuntos de unidades:

### 1. Unidad de Presentación (Cómo se compra)
- **PresentationUnit**: Unidad en la que se presenta el producto
- **PresentationSize**: Tamaño de la presentación

### 2. Unidad Base (Cómo se almacena en inventario)
- **Unit**: Unidad mínima de almacenamiento
- **Size**: Cantidad en unidad base que equivale a una unidad de presentación

---

## 📦 CASO 1: Productos Inmuebles (Property)

### Características:
- Se registran siempre en **unidades** (1:1)
- No hay conversión entre presentación y base
- Ejemplos: Toallas, sábanas, almohadas, muebles

### Ejemplo 1: Toallas
```sql
INSERT INTO Products (
    Sku, Name, BrandId, ProductType, 
    PresentationUnit, PresentationSize, 
    Unit, Size, 
    CreatedAt
)
VALUES (
    'TOW-001',           -- SKU
    'Toalla Blanca',     -- Nombre
    1,                   -- BrandId
    0,                   -- ProductType: 0 = Property
    0,                   -- PresentationUnit: 0 = Unit
    1,                   -- PresentationSize: 1 unidad
    0,                   -- Unit: 0 = Unit
    1,                   -- Size: 1 (1 presentación = 1 unidad base)
    GETDATE()
);
```

**Explicación:**
- Se compra: 1 toalla (PresentationUnit=Unit, PresentationSize=1)
- Se almacena: 1 toalla (Unit=Unit, Size=1)
- **Stock**: Si compras 10 toallas → Stock = 10 unidades

### Ejemplo 2: Sábanas
```sql
INSERT INTO Products (
    Sku, Name, BrandId, ProductType, 
    PresentationUnit, PresentationSize, 
    Unit, Size, 
    CreatedAt
)
VALUES (
    'SHT-001',           -- SKU
    'Sábana Queen',      -- Nombre
    1,                   -- BrandId
    0,                   -- ProductType: 0 = Property
    0,                   -- PresentationUnit: 0 = Unit
    1,                   -- PresentationSize: 1 unidad
    0,                   -- Unit: 0 = Unit
    1,                   -- Size: 1
    GETDATE()
);
```

---

## 🧴 CASO 2: Productos Líquidos (Cleaning)

### Características:
- Se compran en **onzas, litros o galones**
- Se almacenan en **mililitros (ml)** para precisión
- Hay conversión entre presentación y base

### Conversiones Comunes:
- 1 oz (FluidOunce) = 29.5735 ml
- 1 galón = 3785 ml
- 1 litro = 1000 ml

### Ejemplo 1: Shampoo 16oz
```sql
INSERT INTO Products (
    Sku, Name, BrandId, ProductType, 
    PresentationUnit, PresentationSize, 
    Unit, Size, 
    CreatedAt
)
VALUES (
    'SHP-001',                -- SKU
    'Shampoo Dove 16oz',      -- Nombre
    2,                        -- BrandId
    1,                        -- ProductType: 1 = Cleaning
    3,                        -- PresentationUnit: 3 = FluidOunce
    16,                       -- PresentationSize: 16 oz
    1,                        -- Unit: 1 = Milliliter
    473.176,                  -- Size: 16 oz = 473.176 ml
    GETDATE()
);
```

**Explicación:**
- Se compra: Botella de 16 oz (PresentationUnit=FluidOunce, PresentationSize=16)
- Se almacena: 473.176 ml (Unit=Milliliter, Size=473.176)
- **Cálculo**: 16 oz × 29.5735 ml/oz = 473.176 ml
- **Stock**: Si compras 10 botellas → Stock = 4731.76 ml = 10 botellas

### Ejemplo 2: Cloro 1 Galón
```sql
INSERT INTO Products (
    Sku, Name, BrandId, ProductType, 
    PresentationUnit, PresentationSize, 
    Unit, Size, 
    CreatedAt
)
VALUES (
    'CLR-001',                -- SKU
    'Cloro Líquido 1gal',     -- Nombre
    5,                        -- BrandId
    1,                        -- ProductType: 1 = Cleaning
    4,                        -- PresentationUnit: 4 = Gallon
    1,                        -- PresentationSize: 1 galón
    1,                        -- Unit: 1 = Milliliter
    3785,                     -- Size: 1 galón = 3785 ml
    GETDATE()
);
```

**Explicación:**
- Se compra: Galón de 1 galón (PresentationUnit=Gallon, PresentationSize=1)
- Se almacena: 3785 ml (Unit=Milliliter, Size=3785)
- **Stock**: Si compras 5 galones → Stock = 18925 ml = 5 galones

### Ejemplo 3: Champú 10 Litros
```sql
INSERT INTO Products (
    Sku, Name, BrandId, ProductType, 
    PresentationUnit, PresentationSize, 
    Unit, Size, 
    CreatedAt
)
VALUES (
    'SHP-002',                -- SKU
    'Champú 10L',             -- Nombre
    10,                       -- BrandId
    1,                        -- ProductType: 1 = Cleaning
    2,                        -- PresentationUnit: 2 = Liter
    10,                       -- PresentationSize: 10 litros
    1,                        -- Unit: 1 = Milliliter
    10000,                    -- Size: 10 L = 10000 ml
    GETDATE()
);
```

**Explicación:**
- Se compra: Bidón de 10 litros (PresentationUnit=Liter, PresentationSize=10)
- Se almacena: 10000 ml (Unit=Milliliter, Size=10000)
- **Stock**: Si compras 3 bidones → Stock = 30000 ml = 3 bidones

---

## 📦 CASO 3: Productos por Unidades (Cleaning - Bolsas, Rollos, etc.)

### Características:
- Se compran en **paquetes** con múltiples unidades
- Se almacenan en **unidades individuales**
- Hay conversión entre presentación y base

### Ejemplo 1: Papel Higiénico (Paquete de 12 rollos)
```sql
INSERT INTO Products (
    Sku, Name, BrandId, ProductType, 
    PresentationUnit, PresentationSize, 
    Unit, Size, 
    CreatedAt
)
VALUES (
    'TP-001',                      -- SKU
    'Papel Higiénico 12 rollos',  -- Nombre
    2,                             -- BrandId
    1,                             -- ProductType: 1 = Cleaning
    0,                             -- PresentationUnit: 0 = Unit
    12,                            -- PresentationSize: 12 rollos
    0,                             -- Unit: 0 = Unit
    12,                            -- Size: 12 (1 paquete = 12 rollos)
    GETDATE()
);
```

**Explicación:**
- Se compra: Paquete de 12 rollos (PresentationUnit=Unit, PresentationSize=12)
- Se almacena: 12 rollos individuales (Unit=Unit, Size=12)
- **Stock**: Si compras 5 paquetes → Stock = 60 rollos
- **Consumo**: Si gastas 12 rollos → Stock = 48 rollos

### Ejemplo 2: Servilletas (Bolsa de 500 unidades)
```sql
INSERT INTO Products (
    Sku, Name, BrandId, ProductType, 
    PresentationUnit, PresentationSize, 
    Unit, Size, 
    CreatedAt
)
VALUES (
    'NAP-001',                     -- SKU
    'Servilletas 500u',            -- Nombre
    3,                             -- BrandId
    1,                             -- ProductType: 1 = Cleaning
    0,                             -- PresentationUnit: 0 = Unit
    500,                           -- PresentationSize: 500 servilletas
    0,                             -- Unit: 0 = Unit
    500,                           -- Size: 500 (1 bolsa = 500 servilletas)
    GETDATE()
);
```

**Explicación:**
- Se compra: Bolsa de 500 servilletas (PresentationUnit=Unit, PresentationSize=500)
- Se almacena: 500 servilletas individuales (Unit=Unit, Size=500)
- **Stock**: Si compras 5 bolsas → Stock = 2500 servilletas
- **Consumo**: Si gastas 150 servilletas → Stock = 2350 servilletas

### Ejemplo 3: Bolsas de Basura (Paquete de 200 unidades)
```sql
INSERT INTO Products (
    Sku, Name, BrandId, ProductType, 
    PresentationUnit, PresentationSize, 
    Unit, Size, 
    CreatedAt
)
VALUES (
    'BAG-001',                     -- SKU
    'Bolsas Basura 13gal 200u',   -- Nombre
    10,                            -- BrandId
    1,                             -- ProductType: 1 = Cleaning
    0,                             -- PresentationUnit: 0 = Unit
    200,                           -- PresentationSize: 200 bolsas
    0,                             -- Unit: 0 = Unit
    200,                           -- Size: 200 (1 paquete = 200 bolsas)
    GETDATE()
);
```

**Explicación:**
- Se compra: Paquete de 200 bolsas (PresentationUnit=Unit, PresentationSize=200)
- Se almacena: 200 bolsas individuales (Unit=Unit, Size=200)
- **Stock**: Si compras 5 paquetes → Stock = 1000 bolsas
- **Consumo**: Si gastas 12 bolsas → Stock = 988 bolsas

### Ejemplo 4: Guantes Desechables (Caja de 300 unidades)
```sql
INSERT INTO Products (
    Sku, Name, BrandId, ProductType, 
    PresentationUnit, PresentationSize, 
    Unit, Size, 
    CreatedAt
)
VALUES (
    'GLV-001',                     -- SKU
    'Guantes Desechables 300u',   -- Nombre
    10,                            -- BrandId
    1,                             -- ProductType: 1 = Cleaning
    0,                             -- PresentationUnit: 0 = Unit
    300,                           -- PresentationSize: 300 guantes
    0,                             -- Unit: 0 = Unit
    300,                           -- Size: 300 (1 caja = 300 guantes)
    GETDATE()
);
```

**Explicación:**
- Se compra: Caja de 300 guantes (PresentationUnit=Unit, PresentationSize=300)
- Se almacena: 300 guantes individuales (Unit=Unit, Size=300)
- **Stock**: Si compras 5 cajas → Stock = 1500 guantes
- **Consumo**: Si gastas 4 guantes → Stock = 1496 guantes

---

## 📊 Resumen de Cálculos de Stock

### Para Productos Inmuebles (1:1):
```
Stock en Reporte = Suma de Entradas - Suma de Salidas
Ejemplo: 10 toallas compradas - 2 toallas consumidas = 8 toallas
```

### Para Productos Líquidos (con conversión):
```
Stock en Base (ml) = Suma de Entradas (ml) - Suma de Salidas (ml)
Stock en Presentación = Stock en Base / Size

Ejemplo Shampoo 16oz (Size=473.176 ml):
- Compras: 10 botellas = 4731.76 ml
- Consumo: 300 ml
- Stock Base: 4731.76 - 300 = 4431.76 ml
- Stock Presentación: 4431.76 / 473.176 = 9.37 botellas
```

### Para Productos por Unidades (con conversión):
```
Stock en Base (unidades) = Suma de Entradas (unidades) - Suma de Salidas (unidades)
Stock en Presentación = Stock en Base / Size

Ejemplo Servilletas (Size=500):
- Compras: 5 bolsas = 2500 servilletas
- Consumo: 150 servilletas
- Stock Base: 2500 - 150 = 2350 servilletas
- Stock Presentación: 2350 / 500 = 4.7 bolsas
```

---

## 🔄 Flujo de Compra e Inventario

### Campos en Inventory:
- **UnitCount**: Unidades físicas que compras (paquetes, botellas, cajas)
- **Quantity**: Cantidad total en unidad base (ml para líquidos, unidades individuales para sólidos)
- **Price**: Precio por unidad base (por ml, por unidad individual)
- **PricePresentation**: Precio por unidad física (por botella, por paquete)
- **UnitBase**: Debe coincidir con `Product.Unit`
- **UnitPresentation**: Debe coincidir con `Product.PresentationUnit`

### 1. Compra de Producto Líquido (Shampoo 16oz)
```sql
-- Comprar 10 botellas a $8.50 c/u
INSERT INTO Inventory (
    ProductId, StoreId, Operation, TransactionType,
    UnitCount, Quantity, Price, PricePresentation,
    UnitBase, UnitPresentation, Note, CreatedAt
)
VALUES (
    1,                  -- ProductId (Shampoo)
    1,                  -- StoreId
    0,                  -- Operation: 0 = Purchase
    0,                  -- TransactionType: 0 = In
    10,                 -- UnitCount: 10 botellas (unidades físicas)
    4731.76,            -- Quantity: 10 × 473.176 ml (unidad base)
    0.000018,           -- Price: $8.50 / 473.176 ml = $0.000018 por ml
    8.50,               -- PricePresentation: $8.50 por botella
    1,                  -- UnitBase: 1 = Milliliter (coincide con Product.Unit)
    3,                  -- UnitPresentation: 3 = FluidOunce (coincide con Product.PresentationUnit)
    'Compra inicial',   -- Note
    GETDATE()
);
```

**Explicación:**
- **UnitCount = 10**: Compras 10 botellas físicas
- **Quantity = 4731.76**: Total en ml (10 botellas × 473.176 ml/botella)
- **Price = 0.000018**: Precio por ml ($85.00 total / 4731.76 ml)
- **PricePresentation = 8.50**: Precio por botella física

### 2. Compra de Producto por Unidades (Servilletas 500u)
```sql
-- Comprar 5 bolsas a $2.00 c/u
INSERT INTO Inventory (
    ProductId, StoreId, Operation, TransactionType,
    UnitCount, Quantity, Price, PricePresentation,
    UnitBase, UnitPresentation, Note, CreatedAt
)
VALUES (
    2,                  -- ProductId (Servilletas)
    1,                  -- StoreId
    0,                  -- Operation: 0 = Purchase
    0,                  -- TransactionType: 0 = In
    5,                  -- UnitCount: 5 bolsas (unidades físicas)
    2500,               -- Quantity: 5 × 500 servilletas (unidad base)
    0.004,              -- Price: $2.00 / 500 = $0.004 por servilleta
    2.00,               -- PricePresentation: $2.00 por bolsa
    0,                  -- UnitBase: 0 = Unit (coincide con Product.Unit)
    0,                  -- UnitPresentation: 0 = Unit (coincide con Product.PresentationUnit)
    'Compra inicial',   -- Note
    GETDATE()
);
```

**Explicación:**
- **UnitCount = 5**: Compras 5 bolsas físicas
- **Quantity = 2500**: Total en servilletas individuales (5 bolsas × 500 servilletas/bolsa)
- **Price = 0.004**: Precio por servilleta individual ($10.00 total / 2500 servilletas)
- **PricePresentation = 2.00**: Precio por bolsa física

### 3. Compra de Producto Inmueble (Toallas)
```sql
-- Comprar 10 toallas a $15.00 c/u
INSERT INTO Inventory (
    ProductId, StoreId, Operation, TransactionType,
    UnitCount, Quantity, Price, PricePresentation,
    UnitBase, UnitPresentation, Note, CreatedAt
)
VALUES (
    3,                  -- ProductId (Toallas)
    1,                  -- StoreId
    0,                  -- Operation: 0 = Purchase
    0,                  -- TransactionType: 0 = In
    10,                 -- UnitCount: 10 toallas (unidades físicas)
    10,                 -- Quantity: 10 toallas (unidad base = unidad física)
    15.00,              -- Price: $15.00 por toalla
    15.00,              -- PricePresentation: $15.00 por toalla
    0,                  -- UnitBase: 0 = Unit (coincide con Product.Unit)
    0,                  -- UnitPresentation: 0 = Unit (coincide con Product.PresentationUnit)
    'Compra inicial',   -- Note
    GETDATE()
);
```

**Explicación:**
- **UnitCount = 10**: Compras 10 toallas físicas
- **Quantity = 10**: Total en toallas (1:1, no hay conversión)
- **Price = 15.00**: Precio por toalla
- **PricePresentation = 15.00**: Precio por toalla (igual porque no hay conversión)

### 4. Consumo en Mantenimiento (Shampoo)
```sql
-- Consumir 300 ml (0.63 botellas)
INSERT INTO Inventory (
    ProductId, StoreId, Operation, TransactionType,
    UnitCount, Quantity, Price, PricePresentation,
    UnitBase, UnitPresentation, Note, CreatedAt
)
VALUES (
    1,                          -- ProductId (Shampoo)
    1,                          -- StoreId
    3,                          -- Operation: 3 = Consumption
    1,                          -- TransactionType: 1 = Out
    0,                          -- UnitCount: 0 (no se consume botella completa)
    300,                        -- Quantity: 300 ml consumidos
    NULL,                       -- Price: NULL (no tiene precio el consumo)
    NULL,                       -- PricePresentation: NULL
    1,                          -- UnitBase: 1 = Milliliter
    3,                          -- UnitPresentation: 3 = FluidOunce
    'Mantenimiento ID: 1',      -- Note
    GETDATE()
);
```

**Explicación:**
- **UnitCount = 0**: No se consume una botella completa (solo 300ml de 473.176ml)
- **Quantity = 300**: Cantidad consumida en ml
- Si se consumiera una botella completa (473.176 ml), entonces **UnitCount = 1**

### 5. Consumo en Mantenimiento (Servilletas)
```sql
-- Consumir 150 servilletas
INSERT INTO Inventory (
    ProductId, StoreId, Operation, TransactionType,
    UnitCount, Quantity, Price, PricePresentation,
    UnitBase, UnitPresentation, Note, CreatedAt
)
VALUES (
    2,                          -- ProductId (Servilletas)
    1,                          -- StoreId
    3,                          -- Operation: 3 = Consumption
    1,                          -- TransactionType: 1 = Out
    0,                          -- UnitCount: 0 (no se consume bolsa completa)
    150,                        -- Quantity: 150 servilletas consumidas
    NULL,                       -- Price: NULL (no tiene precio el consumo)
    NULL,                       -- PricePresentation: NULL
    0,                          -- UnitBase: 0 = Unit
    0,                          -- UnitPresentation: 0 = Unit
    'Mantenimiento ID: 1',      -- Note
    GETDATE()
);
```

**Explicación:**
- **UnitCount = 0**: No se consume una bolsa completa (solo 150 de 500 servilletas)
- **Quantity = 150**: Cantidad consumida en servilletas individuales
- Si se consumiera una bolsa completa (500 servilletas), entonces **UnitCount = 1**

---

## ✅ Reglas Importantes

1. **Productos Inmuebles**: Siempre usar `Unit` (0) para ambas unidades, Size = 1
2. **Productos Líquidos**: Siempre almacenar en `Milliliter` (1) para precisión
3. **Productos por Unidades**: Size = cantidad de unidades en el paquete
4. **Conversiones**: Calcular correctamente el Size según la presentación
5. **Stock**: El sistema calcula automáticamente el stock en unidades de presentación
6. **⚠️ IMPORTANTE - Inventory**: Los campos `UnitBase` y `UnitPresentation` en Inventory **DEBEN** coincidir con `Unit` y `PresentationUnit` del producto

### Ejemplo de Coherencia:

**Producto: Servilletas 500u**
```sql
-- Products
PresentationUnit = 0 (Unit)
PresentationSize = 500
Unit = 0 (Unit)
Size = 500

-- Inventory (al comprar)
UnitBase = 0 (Unit)          -- ✅ Coincide con Product.Unit
UnitPresentation = 0 (Unit)  -- ✅ Coincide con Product.PresentationUnit
```

**Producto: Shampoo 16oz**
```sql
-- Products
PresentationUnit = 3 (FluidOunce)
PresentationSize = 16
Unit = 1 (Milliliter)
Size = 473.176

-- Inventory (al comprar)
UnitBase = 1 (Milliliter)    -- ✅ Coincide con Product.Unit
UnitPresentation = 3 (FluidOunce)  -- ✅ Coincide con Product.PresentationUnit
```

---

## 📝 Tabla de Conversiones Rápidas

| Presentación | Unidad Base | Conversión |
|--------------|-------------|------------|
| 1 oz (FluidOunce) | Milliliter | 29.5735 ml |
| 1 galón | Milliliter | 3785 ml |
| 1 litro | Milliliter | 1000 ml |
| 1 paquete 12u | Unit | 12 unidades |
| 1 bolsa 500u | Unit | 500 unidades |
| 1 caja 300u | Unit | 300 unidades |

---

## 🎯 Ejemplos Completos por Categoría

### Líquidos:
- Shampoo 16oz → PresentationUnit=3 (FluidOunce), PresentationSize=16, Unit=1 (Milliliter), Size=473.176
- Cloro 1gal → PresentationUnit=4 (Gallon), PresentationSize=1, Unit=1 (Milliliter), Size=3785
- Champú 10L → PresentationUnit=2 (Liter), PresentationSize=10, Unit=1 (Milliliter), Size=10000

### Por Unidades:
- Papel Higiénico 12 rollos → PresentationUnit=0 (Unit), PresentationSize=12, Unit=0 (Unit), Size=12
- Servilletas 500u → PresentationUnit=0 (Unit), PresentationSize=500, Unit=0 (Unit), Size=500
- Bolsas Basura 200u → PresentationUnit=0 (Unit), PresentationSize=200, Unit=0 (Unit), Size=200

### Inmuebles:
- Toallas → PresentationUnit=0 (Unit), PresentationSize=1, Unit=0 (Unit), Size=1
- Sábanas → PresentationUnit=0 (Unit), PresentationSize=1, Unit=0 (Unit), Size=1
