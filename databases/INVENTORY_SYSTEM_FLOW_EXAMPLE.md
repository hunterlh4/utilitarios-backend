# Sistema de Inventario - Flujo Completo con Ejemplos

Este documento explica el flujo completo del sistema de inventario con ejemplos reales de registros.

## 📋 Estructura de Tablas

### 1. Products (Productos)
Almacena la información de los productos, tanto de limpieza como inmuebles.

### 2. Inventory (Kardex)
Registra todos los movimientos de entrada/salida de productos.

### 3. Stock
Mantiene el inventario actual por almacén y producto.

### 4. PropertyItems
Define qué productos están asignados a cada propiedad (inmuebles).

### 5. PropertyItemsMaintenance
Define qué productos de limpieza se usan en cada propiedad/habitación.

### 6. Maintenance
Registra las tareas de mantenimiento/limpieza.

### 7. MaintenanceItems
Detalla los productos usados en cada mantenimiento.

---

## 🎯 Ejemplo Completo: Flujo de Inventario

### PASO 1: Registrar Productos

#### Producto 1: Inmueble (Toallas)
```sql
-- Products
| Id | Sku      | Name           | ProductType | PresentationUnit | PresentationSize | Unit | Size | ProductGroupId |
|----|----------|----------------|-------------|------------------|------------------|------|------|----------------|
| 1  | TOW-001  | Toalla Blanca  | Property    | Unit             | 1                | Unit | 1    | 10             |
```

**Explicación:**
- **ProductType**: `Property` (Inmueble)
- **PresentationUnit**: `Unit` (Unidad)
- **PresentationSize**: 1 (se compra de 1 en 1)
- **Unit**: `Unit` (unidad base también es unidad)
- **Size**: 1 (1 unidad de presentación = 1 unidad base)

#### Producto 2: Limpieza (Shampoo)
```sql
-- Products
| Id | Sku      | Name              | ProductType | PresentationUnit | PresentationSize | Unit       | Size    | ProductGroupId |
|----|----------|-------------------|-------------|------------------|------------------|------------|---------|----------------|
| 2  | SHP-001  | Shampoo Dove 16oz | Cleaning    | FluidOunce       | 16               | Milliliter | 473.176 | 5              |
```

**Explicación:**
- **ProductType**: `Cleaning` (Limpieza)
- **PresentationUnit**: `FluidOunce` (Onzas fluidas - cómo se presenta)
- **PresentationSize**: 16 (botella de 16 onzas)
- **Unit**: `Milliliter` (unidad base para almacenamiento)
- **Size**: 473.176 (16 oz = 473.176 ml)

---

### PASO 2: Comprar Productos (Purchase)

#### Compra 1: 5 Toallas a $15.00 c/u
**Request:**
```json
{
  "storeDestinationId": 1,
  "orderNumber": "PO-2024-001",
  "items": [
    {
      "productId": 1,
      "quantity": 5,
      "price": 15.00
    }
  ]
}
```

**Registros creados:**

```sql
-- Inventory (Kardex)
| Id | ProductId | StoreId | Operation | TransactionType | UnitCount | Quantity | Price    | PricePresentation | UnitBase | UnitPresentation | Note                                    |
|----|-----------|---------|-----------|-----------------|-----------|----------|----------|-------------------|----------|------------------|-----------------------------------------|
| 1  | 1         | 1       | Purchase  | In              | 5         | 5        | 15.00    | 15.00             | Unit     | Unit             | Purchase - Order: PO-2024-001 - 5 units @ $15.00 each (Total: $75.00) |
```

**Explicación:**
- **UnitCount**: 5 (5 toallas)
- **Quantity**: 5 (5 unidades base)
- **Price**: 15.00 (precio por unidad base = precio por toalla)
- **PricePresentation**: 15.00 (precio por toalla)

```sql
-- Stock
| Id | StoreId | ProductId | Quantity | AveragePrice | TotalValue | AssignedQuantity |
|----|---------|-----------|----------|--------------|------------|------------------|
| 1  | 1       | 1         | 5        | 15.00        | 75.00      | 0                |
```

#### Compra 2: 10 Botellas de Shampoo a $8.50 c/u
**Request:**
```json
{
  "storeDestinationId": 1,
  "orderNumber": "PO-2024-002",
  "items": [
    {
      "productId": 2,
      "quantity": 10,
      "price": 8.50
    }
  ]
}
```

**Registros creados:**

```sql
-- Inventory (Kardex)
| Id | ProductId | StoreId | Operation | TransactionType | UnitCount | Quantity  | Price       | PricePresentation | UnitBase   | UnitPresentation | Note                                     |
|----|-----------|---------|-----------|-----------------|-----------|-----------|-------------|-------------------|------------|------------------|------------------------------------------|
| 2  | 2         | 1       | Purchase  | In              | 10        | 4731.76   | 0.000018    | 8.50              | Milliliter | FluidOunce       | Purchase - Order: PO-2024-002 - 10 units @ $8.50 each (Total: $85.00) |
```

**Explicación:**
- **UnitCount**: 10 (10 botellas)
- **Quantity**: 4731.76 (10 × 473.176 ml)
- **Price**: 0.000018 (85.00 / 4731.76 = precio por ml)
- **PricePresentation**: 8.50 (precio por botella)

```sql
-- Stock
| Id | StoreId | ProductId | Quantity | AveragePrice | TotalValue | AssignedQuantity |
|----|---------|-----------|----------|--------------|------------|------------------|
| 2  | 1       | 2         | 4731.76  | 0.000018     | 85.00      | 0                |
```

---

### PASO 2B: Compras Adicionales (Promedio Ponderado)

#### Compra 3: 8 Toallas a $12.00 c/u (precio más bajo)
**Request:**
```json
{
  "storeDestinationId": 1,
  "orderNumber": "PO-2024-003",
  "items": [
    {
      "productId": 1,
      "quantity": 8,
      "price": 12.00
    }
  ]
}
```

**Registros creados:**

```sql
-- Inventory (Kardex)
| Id | ProductId | StoreId | Operation | TransactionType | UnitCount | Quantity | Price    | PricePresentation | UnitBase | UnitPresentation | Note                                    |
|----|-----------|---------|-----------|-----------------|-----------|----------|----------|-------------------|----------|------------------|-----------------------------------------|
| 3  | 1         | 1       | Purchase  | In              | 8         | 8        | 12.00    | 12.00             | Unit     | Unit             | Purchase - Order: PO-2024-003 - 8 units @ $12.00 each (Total: $96.00) |
```

**Cálculo del Promedio Ponderado:**
- Stock anterior: 5 unidades @ $15.00 = $75.00
- Nueva compra: 8 unidades @ $12.00 = $96.00
- Total: 13 unidades por $171.00
- **Nuevo precio promedio**: $171.00 / 13 = $13.15 por unidad

```sql
-- Stock (actualizado con promedio ponderado)
| Id | StoreId | ProductId | Quantity | AveragePrice | TotalValue | AssignedQuantity |
|----|---------|-----------|----------|--------------|------------|------------------|
| 1  | 1       | 1         | 13       | 13.15        | 171.00     | 0                |
```

#### Compra 4: 15 Botellas de Shampoo a $7.80 c/u (precio más bajo)
**Request:**
```json
{
  "storeDestinationId": 1,
  "orderNumber": "PO-2024-004",
  "items": [
    {
      "productId": 2,
      "quantity": 15,
      "price": 7.80
    }
  ]
}
```

**Registros creados:**

```sql
-- Inventory (Kardex)
| Id | ProductId | StoreId | Operation | TransactionType | UnitCount | Quantity  | Price       | PricePresentation | UnitBase   | UnitPresentation | Note                                     |
|----|-----------|---------|-----------|-----------------|-----------|-----------|-------------|-------------------|------------|------------------|------------------------------------------|
| 4  | 2         | 1       | Purchase  | In              | 15        | 7097.64   | 0.000016    | 7.80              | Milliliter | FluidOunce       | Purchase - Order: PO-2024-004 - 15 units @ $7.80 each (Total: $117.00) |
```

**Cálculo del Promedio Ponderado:**
- Stock anterior: 4731.76 ml @ $0.000018 por ml = $85.00
- Nueva compra: 7097.64 ml @ $0.000016 por ml = $117.00
- Total: 11829.40 ml por $202.00
- **Nuevo precio promedio**: $202.00 / 11829.40 ml = $0.000017 por ml

```sql
-- Stock (actualizado con promedio ponderado)
| Id | StoreId | ProductId | Quantity  | AveragePrice | TotalValue | AssignedQuantity |
|----|---------|-----------|-----------|--------------|------------|------------------|
| 2  | 1       | 2         | 11829.40  | 0.000017     | 202.00     | 0                |
```

#### Compra 5: 3 Toallas a $18.00 c/u (precio más alto)
**Request:**
```json
{
  "storeDestinationId": 1,
  "orderNumber": "PO-2024-005",
  "items": [
    {
      "productId": 1,
      "quantity": 3,
      "price": 18.00
    }
  ]
}
```

**Registros creados:**

```sql
-- Inventory (Kardex)
| Id | ProductId | StoreId | Operation | TransactionType | UnitCount | Quantity | Price    | PricePresentation | UnitBase | UnitPresentation | Note                                    |
|----|-----------|---------|-----------|-----------------|-----------|----------|----------|-------------------|----------|------------------|-----------------------------------------|
| 5  | 1         | 1       | Purchase  | In              | 3         | 3        | 18.00    | 18.00             | Unit     | Unit             | Purchase - Order: PO-2024-005 - 3 units @ $18.00 each (Total: $54.00) |
```

**Cálculo del Promedio Ponderado:**
- Stock anterior: 13 unidades @ $13.15 = $171.00
- Nueva compra: 3 unidades @ $18.00 = $54.00
- Total: 16 unidades por $225.00
- **Nuevo precio promedio**: $225.00 / 16 = $14.06 por unidad

```sql
-- Stock (actualizado con promedio ponderado)
| Id | StoreId | ProductId | Quantity | AveragePrice | TotalValue | AssignedQuantity |
|----|---------|-----------|----------|--------------|------------|------------------|
| 1  | 1       | 1         | 16       | 14.06        | 225.00     | 0                |
```

#### Compra 6: 20 Botellas de Shampoo a $9.20 c/u (precio medio)
**Request:**
```json
{
  "storeDestinationId": 1,
  "orderNumber": "PO-2024-006",
  "items": [
    {
      "productId": 2,
      "quantity": 20,
      "price": 9.20
    }
  ]
}
```

**Registros creados:**

```sql
-- Inventory (Kardex)
| Id | ProductId | StoreId | Operation | TransactionType | UnitCount | Quantity  | Price       | PricePresentation | UnitBase   | UnitPresentation | Note                                     |
|----|-----------|---------|-----------|-----------------|-----------|-----------|-------------|-------------------|------------|------------------|------------------------------------------|
| 6  | 2         | 1       | Purchase  | In              | 20        | 9463.52   | 0.000019    | 9.20              | Milliliter | FluidOunce       | Purchase - Order: PO-2024-006 - 20 units @ $9.20 each (Total: $184.00) |
```

**Cálculo del Promedio Ponderado:**
- Stock anterior: 11829.40 ml @ $0.000017 por ml = $202.00
- Nueva compra: 9463.52 ml @ $0.000019 por ml = $184.00
- Total: 21292.92 ml por $386.00
- **Nuevo precio promedio**: $386.00 / 21292.92 ml = $0.000018 por ml

```sql
-- Stock (actualizado con promedio ponderado)
| Id | StoreId | ProductId | Quantity  | AveragePrice | TotalValue | AssignedQuantity |
|----|---------|-----------|-----------|--------------|------------|------------------|
| 2  | 1       | 2         | 21292.92  | 0.000018     | 386.00     | 0                |
```

---

### 📊 Resumen de Compras

#### Toallas (Producto 1)
```sql
| Compra | Cantidad | Precio Unit | Total    | Stock Acum | Precio Prom |
|--------|----------|-------------|----------|------------|-------------|
| 1      | 5        | $15.00      | $75.00   | 5          | $15.00      |
| 3      | 8        | $12.00      | $96.00   | 13         | $13.15      |
| 5      | 3        | $18.00      | $54.00   | 16         | $14.06      |
```

**Total invertido**: $225.00 en 16 toallas
**Precio promedio final**: $14.06 por toalla

#### Shampoo (Producto 2)
```sql
| Compra | Cantidad | Precio Unit | Total    | Stock Acum (ml) | Precio Prom (ml) |
|--------|----------|-------------|----------|-----------------|------------------|
| 2      | 10       | $8.50       | $85.00   | 4731.76         | $0.00001796      |
| 4      | 15       | $7.80       | $117.00  | 11829.40        | $0.00001707      |
| 6      | 20       | $9.20       | $184.00  | 21292.92        | $0.00001813      |
```

**Total invertido**: $386.00 en 45 botellas (21292.92 ml)
**Precio promedio final**: $0.000018 por ml ($8.58 por botella de 16oz)

---

### PASO 3: Asignar a Propiedad (PropertyItems - Inmuebles)

```sql
-- PropertyItems
| Id | PropertyId | RoomId | ProductId | Quantity | CreatedAt           |
|----|------------|--------|-----------|----------|---------------------|
| 1  | 1          | 101    | 1         | 2        | 2024-01-20 10:00:00 |
```

**Explicación:**
- Habitación 101 tiene asignadas 2 toallas

---

### PASO 4: Configurar Productos de Limpieza (PropertyItemsMaintenance)

**Request:**
```json
{
  "propertyId": 1,
  "roomId": 101,
  "storeId": 1,
  "productId": 2,
  "size": 3,
  "isVariable": false
}
```

**Registro creado:**

```sql
-- PropertyItemsMaintenance
| Id | PropertyId | RoomId | ProductId | StoreId | PresentationUnit | PresentationSize | Unit       | Size    | IsVariable | CreatedAt           |
|----|------------|--------|-----------|---------|------------------|------------------|------------|---------|------------|---------------------|
| 1  | 1          | 101    | 2         | 1       | FluidOunce       | 3                | Milliliter | 88.72   | 0          | 2024-01-20 10:30:00 |
```

**Explicación:**
- **PresentationSize**: 3 (3 onzas por limpieza)
- **Size**: 88.72 (3 oz = 88.72 ml en unidad base)
- **IsVariable**: false (cantidad fija)

```sql
-- Stock (actualizado con AssignedQuantity)
| Id | StoreId | ProductId | Quantity | AveragePrice | TotalValue | AssignedQuantity | AvailableQuantity |
|----|---------|-----------|----------|--------------|------------|------------------|-------------------|
| 2  | 1       | 2         | 4731.76  | 0.000018     | 85.00      | 88.72            | 4643.04           |
```

**Explicación:**
- **Quantity**: 4731.76 ml (stock físico total)
- **AssignedQuantity**: 88.72 ml (asignado a PropertyItemsMaintenance)
- **AvailableQuantity**: 4643.04 ml (disponible para nuevas asignaciones)

**IMPORTANTE**: Esta asignación **NO crea movimiento en Inventory (Kardex)**. Solo actualiza el campo `AssignedQuantity` en Stock.

**AvailableQuantity** = Quantity - AssignedQuantity = 4731.76 - 88.72 = 4643.04 ml

---

### PASO 5: Crear Mantenimiento

**Request:**
```json
{
  "propertyId": 1,
  "roomId": 101,
  "assignedUserId": 5,
  "maintenanceType": 0,
  "notes": "Limpieza diaria"
}
```

**Registros creados:**

```sql
-- Maintenance
| Id | PropertyId | PropertyName    | RoomId | RoomName      | UserCleaningId | MaintenanceType | Status | CreatedAt           |
|----|------------|-----------------|--------|---------------|----------------|-----------------|--------|---------------------|
| 1  | 1          | Hotel Paradise  | 101    | Habitación 101| 5              | Cleaning        | Active | 2024-01-20 14:00:00 |
```

```sql
-- MaintenanceItems (copiado desde PropertyItemsMaintenance)
| Id | MaintenanceId | RoomId | ProductId | Sku     | Name              | PresentationUnit | PresentationSize | Unit       | Size  | IsVariable | QuantityUsed | CreatedAt           |
|----|---------------|--------|-----------|---------|-------------------|------------------|------------------|------------|-------|------------|--------------|---------------------|
| 1  | 1             | 101    | 2         | SHP-001 | Shampoo Dove 16oz | FluidOunce       | 3                | Milliliter | 88.72 | 0          | 0            | 2024-01-20 14:00:00 |
```

**Explicación:**
- Se copia la configuración de `PropertyItemsMaintenance`
- **QuantityUsed**: 0 (aún no se ha completado el mantenimiento)

---

### PASO 6: Completar Mantenimiento (Consumo)

**Request:**
```json
{
  "maintenanceId": 1,
  "items": [
    {
      "maintenanceItemId": 1,
      "presentationSize": 2
    }
  ]
}
```

**Explicación del cálculo:**
- Usuario reporta: 2 onzas usadas
- Cálculo: (2 / 3) × 88.72 = 59.147 ml

**Registros actualizados/creados:**

```sql
-- MaintenanceItems (actualizado)
| Id | MaintenanceId | RoomId | ProductId | PresentationUnit | PresentationSize | Unit       | Size  | QuantityUsed | UpdatedAt           |
|----|---------------|--------|-----------|------------------|------------------|------------|-------|--------------|---------------------|
| 1  | 1             | 101    | 2         | FluidOunce       | 2                | Milliliter | 88.72 | 59.147       | 2024-01-20 16:00:00 |
```

```sql
-- Inventory (Kardex) - Nuevo movimiento de consumo
| Id | ProductId | StoreId | Operation   | TransactionType | UnitCount | Quantity | Price | PricePresentation | UnitBase   | UnitPresentation | Note                                              |
|----|-----------|---------|-------------|-----------------|-----------|----------|-------|-------------------|------------|------------------|---------------------------------------------------|
| 3  | 2         | 1       | Consumption | Out             | 0         | 59.147   | NULL  | NULL              | Milliliter | FluidOunce       | Maintenance Consumption - Maintenance ID: 1 - 2 FluidOunce consumed |
```

**Explicación:**
- **UnitCount**: 0 (no se consume una botella completa de 16oz, solo 2oz)
- **Quantity**: 59.147 (consumo en ml - unidad base)
- **Price**: NULL (no tiene precio el consumo)
- **Nota**: Si se consumieran 16oz o más, UnitCount sería 1 o más (unidades físicas completas)

```sql
-- Stock (actualizado - stock reducido)
| Id | StoreId | ProductId | Quantity | AveragePrice | TotalValue | AssignedQuantity |
|----|---------|-----------|----------|--------------|------------|------------------|
| 2  | 1       | 2         | 4672.613 | 0.000018     | 83.94      | 88.72            |
```

**Explicación:**
- **Quantity anterior**: 4731.76 ml
- **Consumo**: 59.147 ml
- **Quantity nueva**: 4731.76 - 59.147 = 4672.613 ml
- **TotalValue**: 4672.613 × 0.000018 = 83.94

```sql
-- Maintenance (actualizado)
| Id | PropertyId | Status    | UpdatedAt           |
|----|------------|-----------|---------------------|
| 1  | 1          | Completed | 2024-01-20 16:00:00 |
```

---

## 📊 Vista del Kardex (GetKardex)

### Kardex del Producto 1 (Toallas)

```sql
| Fecha               | Operación   | Tipo    | Unidades Físicas | Cantidad | Precio Unit | Total    | Saldo | Saldo Físico |
|---------------------|-------------|---------|------------------|----------|-------------|----------|-------|--------------|
| 2024-01-20 09:00:00 | Compra      | Entrada | 5                | 5        | $15.00      | $75.00   | 5     | 5            |
| 2024-01-20 10:00:00 | Compra      | Entrada | 8                | 8        | $12.00      | $96.00   | 13    | 13           |
| 2024-01-20 11:00:00 | Compra      | Entrada | 3                | 3        | $18.00      | $54.00   | 16    | 16           |
```

**Explicación:**
- **Unidades Físicas**: Número exacto de unidades completas en el movimiento (5 toallas, 8 toallas, 3 toallas)
- **Cantidad**: Igual que unidades físicas para productos por unidad
- **Saldo**: Saldo acumulado en unidades
- **Saldo Físico**: Unidades físicas completas que quedan (igual que Saldo para productos por unidad)

**Stock actual**: 16 toallas
**Valor total**: $225.00
**Precio promedio**: $14.06 por toalla

### Kardex del Producto 2 (Shampoo)

```sql
| Fecha               | Operación   | Tipo    | Unidades Físicas | Cantidad (oz) | Precio Unit | Total    | Saldo (oz) | Saldo Físico |
|---------------------|-------------|---------|------------------|---------------|-------------|----------|------------|--------------|
| 2024-01-20 09:30:00 | Compra      | Entrada | 10               | 160.00        | $8.50       | $85.00   | 160.00     | 10           |
| 2024-01-20 10:30:00 | Compra      | Entrada | 15               | 240.00        | $7.80       | $117.00  | 400.00     | 25           |
| 2024-01-20 11:30:00 | Compra      | Entrada | 20               | 320.00        | $9.20       | $184.00  | 720.00     | 45           |
| 2024-01-20 16:00:00 | Consumo     | Salida  | 0                | 2.00          | -           | -        | 718.00     | 44           |
```

**Explicación:**
- **Unidades Físicas**: 
  - **Compras**: Número de botellas completas (10, 15, 20)
  - **Consumos**: 0 hasta completar 16 oz (1 botella completa)
- **Cantidad (oz)**: Onzas totales para visualización
- **Saldo (oz)**: Saldo acumulado en onzas
- **Saldo Físico**: Botellas completas que quedan
  - Después de comprar 45 botellas (720 oz)
  - Consumir 2 oz deja 718 oz = 44.875 botellas → **44 botellas completas**

**Cálculo de saldo físico:**
```
Saldo en oz: 718 oz
Tamaño de botella: 16 oz
Saldo físico: floor(718 / 16) = 44 botellas completas
Fracción restante: 718 - (44 × 16) = 14 oz sueltas
```

**Ejemplo cuando se consuma 1 botella completa:**
```sql
| Fecha               | Operación   | Tipo    | Unidades Físicas | Cantidad (oz) | Precio Unit | Total    | Saldo (oz) | Saldo Físico |
|---------------------|-------------|---------|------------------|---------------|-------------|----------|------------|--------------|
| 2024-01-20 17:00:00 | Consumo     | Salida  | 1                | 16.00         | -           | -        | 702.00     | 43           |
```

**Cálculo:**
- **Consumo**: 16 oz = 1 botella completa
- **Saldo**: 718 - 16 = 702 oz
- **Saldo físico**: floor(702 / 16) = 43 botellas completas

**Stock actual**: 21233.773 ml (718 oz) = **44 botellas completas + 14 oz**
**Valor total**: $384.93
**Precio promedio**: $0.000018 por ml ($8.58 por botella de 16oz)

---

## 🔑 Puntos Clave

### 1. Productos Inmuebles (Property)
- Se manejan en **unidades** (1:1)
- Ejemplo: 5 toallas = 5 unidades base

### 2. Productos de Limpieza (Cleaning)
- Se compran en **unidades de presentación** (botellas, cajas, etc.)
- Se almacenan en **unidad base** (ml, g, etc.)
- Se consumen en **unidades de presentación** (onzas, ml, etc.)

### 3. Conversiones
- **Compra**: 10 botellas × 473.176 ml = 4731.76 ml
- **Consumo**: 2 oz × 29.5735 ml/oz = 59.147 ml
- **Kardex**: Muestra en unidades comprensibles (oz, botellas, etc.)

### 4. Precios
- **Price**: Precio por unidad base (valor decimal directo)
- **PricePresentation**: Precio por unidad de presentación (valor real)

### 5. Stock
- **Quantity**: Stock físico en unidad base
- **AssignedQuantity**: Cantidad asignada a propiedades
- **AvailableQuantity**: Quantity - AssignedQuantity

### 6. IsVariable
- **false**: Cantidad fija (siempre se usa la misma cantidad)
- **true**: Cantidad variable (puede cambiar en cada mantenimiento)

### 7. Diferencia entre Asignación y Consumo

#### Asignación (PropertyItemsMaintenance)
```
✅ Actualiza: Stock.AssignedQuantity
❌ NO crea: Movimiento en Inventory (Kardex)
❌ NO reduce: Stock.Quantity
```

**Ejemplo:**
- Asignas 3 oz (88.72 ml) a Habitación 101
- `AssignedQuantity` aumenta de 0 a 88.72 ml
- `Quantity` sigue siendo 4731.76 ml
- **NO aparece en Kardex**

#### Consumo (CompleteMaintenanceCommand)
```
✅ Crea: Movimiento en Inventory (Kardex)
✅ Reduce: Stock.Quantity
✅ Aparece: En el Kardex como "Consumo"
```

**Ejemplo:**
- Consumes 2 oz (59.147 ml) en mantenimiento
- `Quantity` reduce de 4731.76 a 4672.613 ml
- **SÍ aparece en Kardex** como movimiento de salida

#### Flujo Completo
```
1. Compra → Quantity aumenta, aparece en Kardex
2. Asignación → AssignedQuantity aumenta, NO aparece en Kardex
3. Consumo → Quantity reduce, aparece en Kardex
```

---

## 📈 Resumen del Flujo

1. **Registrar productos** → Define unidades de presentación y base
2. **Comprar** → Crea entrada en Inventory, actualiza Stock
3. **Asignar a propiedad** → PropertyItems (inmuebles) o PropertyItemsMaintenance (limpieza)
4. **Crear mantenimiento** → Copia configuración a MaintenanceItems
5. **Completar mantenimiento** → Actualiza MaintenanceItems, crea salida en Inventory, reduce Stock
6. **Consultar Kardex** → Muestra movimientos convertidos a unidades comprensibles


---

## 🔄 Consumo Flexible: Reportar en Diferentes Unidades

El sistema permite reportar consumo en la unidad que sea más conveniente, y automáticamente lo convierte a la unidad base.

### Ejemplo 1: Producto configurado en oz, consumo reportado en ml

**Producto: Shampoo 16oz**
```sql
-- Products
PresentationUnit = FluidOunce (3)
PresentationSize = 16
Unit = Milliliter (1)
Size = 473.176

-- PropertyItemsMaintenance (configuración)
PresentationUnit = FluidOunce (3)
PresentationSize = 3 (se espera consumir 3 oz)
Unit = Milliliter (1)
Size = 88.72 (3 oz = 88.72 ml)
```

**Escenario A: Usuario reporta en oz (como está configurado)**
```json
{
  "maintenanceItemId": 1,
  "presentationSize": 2
}
```
- Reporta: 2 oz
- Cálculo: (2 / 3) × 88.72 = 59.147 ml
- **Quantity en Inventory**: 59.147 ml
- **UnitCount**: 0 (no se completa una botella de 16oz)

**Escenario B: Usuario reporta en ml (diferente a la configuración)**
```json
{
  "maintenanceItemId": 1,
  "presentationSize": 100
}
```
- Reporta: 100 ml (aunque la configuración dice 3 oz)
- Cálculo: (100 / 88.72) × 88.72 = 100 ml
- **Quantity en Inventory**: 100 ml
- **UnitCount**: 0 (no se completa una botella de 16oz)
- **Nota**: El sistema acepta el valor reportado y lo convierte proporcionalmente

### Ejemplo 2: Servilletas - Consumo parcial

**Producto: Servilletas 500u**
```sql
-- Products
PresentationUnit = Unit (0)
PresentationSize = 500
Unit = Unit (0)
Size = 500

-- PropertyItemsMaintenance (configuración)
PresentationUnit = Unit (0)
PresentationSize = 150 (se espera consumir 150 servilletas)
Unit = Unit (0)
Size = 150
```

**Escenario A: Consumo menor a una bolsa**
```json
{
  "maintenanceItemId": 2,
  "presentationSize": 150
}
```
- Reporta: 150 servilletas
- Cálculo: (150 / 150) × 150 = 150 servilletas
- **Quantity en Inventory**: 150 servilletas
- **UnitCount**: 0 (no se completa una bolsa de 500)
- **Stock**: Si había 2500 servilletas → queda 2350 servilletas

**Escenario B: Consumo de una bolsa completa**
```json
{
  "maintenanceItemId": 2,
  "presentationSize": 500
}
```
- Reporta: 500 servilletas
- Cálculo: (500 / 150) × 150 = 500 servilletas
- **Quantity en Inventory**: 500 servilletas
- **UnitCount**: 1 (se consume 1 bolsa completa de 500)
- **Stock**: Si había 2500 servilletas → queda 2000 servilletas

**Escenario C: Consumo de más de una bolsa**
```json
{
  "maintenanceItemId": 2,
  "presentationSize": 1200
}
```
- Reporta: 1200 servilletas
- Cálculo: (1200 / 150) × 150 = 1200 servilletas
- **Quantity en Inventory**: 1200 servilletas
- **UnitCount**: 2 (se consumen 2 bolsas completas de 500, floor(1200/500) = 2)
- **Stock**: Si había 2500 servilletas → queda 1300 servilletas

---

## ⚠️ Puntos Importantes sobre Consumo

### 1. Unidad Base es la Verdad
- Todo se almacena y calcula en **unidad base** (ml, g, unidades)
- El consumo siempre se resta en **unidad base**
- Ejemplo: Stock en ml, consumo en ml → resta directa

### 2. Conversión Automática
- Puedes reportar en cualquier unidad (oz, ml, unidades)
- El sistema convierte proporcionalmente a la unidad base
- Ejemplo: Reportas 2 oz → sistema calcula 59.147 ml

### 3. UnitCount en Consumos
- **UnitCount = 0**: No se consume una unidad física completa
- **UnitCount ≥ 1**: Se consumen unidades físicas completas
- Cálculo: `floor(cantidadConsumida / tamañoPresentación)`
- Ejemplos:
  - 150 servilletas de bolsa 500u → UnitCount = 0
  - 500 servilletas de bolsa 500u → UnitCount = 1
  - 1200 servilletas de bolsa 500u → UnitCount = 2
  - 2 oz de botella 16oz → UnitCount = 0
  - 16 oz de botella 16oz → UnitCount = 1

### 4. Flexibilidad en Reportes
- **Configuración**: PropertyItemsMaintenance dice "3 oz esperadas"
- **Reporte real**: Puedes reportar 2 oz, 100 ml, o cualquier cantidad
- **Sistema**: Acepta y convierte proporcionalmente
- **Resultado**: Se registra el consumo real en unidad base

### 5. Ejemplo Completo: 100ml de producto configurado en 2oz

**Producto: Limpiador 32oz (946 ml)**
```sql
-- Products
PresentationSize = 32 (oz)
Size = 946 (ml)

-- PropertyItemsMaintenance
PresentationSize = 2 (oz esperadas)
Size = 59.147 (ml equivalentes a 2 oz)
```

**Usuario reporta: 100 ml**
```json
{
  "maintenanceItemId": 3,
  "presentationSize": 100
}
```

**Cálculo:**
- Reporta: 100 ml
- Proporción: (100 / 59.147) × 59.147 = 100 ml
- **Quantity en Inventory**: 100 ml (se resta directamente del stock)
- **UnitCount**: 0 (no se completa una botella de 32oz = 946ml)
- **Stock**: Si había 5000 ml → queda 4900 ml

**Conclusión**: El sistema es flexible y acepta el consumo en cualquier unidad, siempre lo convierte a unidad base para restar del stock.
