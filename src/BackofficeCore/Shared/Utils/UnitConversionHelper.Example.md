# UnitConversionHelper - Ejemplos de Uso

## Helper para Crear (Guardar)

Convierte de unidades grandes a unidades base de almacenamiento:

```csharp
// Ejemplo 1: 5 Litros → 5000 Mililitros
var quantity = 5m;
var unit = MeasurementUnit.Liter;
var storageQuantity = UnitConversionHelper.ConvertToStorageUnit(quantity, unit);
// storageQuantity = 5000

// Ejemplo 2: 10 Onzas (peso) → 283.495 Gramos
var quantity = 10m;
var unit = MeasurementUnit.Ounce;
var storageQuantity = UnitConversionHelper.ConvertToStorageUnit(quantity, unit);
// storageQuantity = 283.495

// Ejemplo 3: 2 Galones → 7570.82 Mililitros
var quantity = 2m;
var unit = MeasurementUnit.Gallon;
var storageQuantity = UnitConversionHelper.ConvertToStorageUnit(quantity, unit);
// storageQuantity = 7570.82
```

## Helper para Presentar (Mostrar)

Convierte de unidades base de almacenamiento a unidades de presentación:

```csharp
// Ejemplo 1: 10000 Mililitros → 10 Litros
var storageValue = 10000m;
var targetUnit = MeasurementUnit.Liter;
var displayValue = UnitConversionHelper.ConvertFromStorageUnit(storageValue, targetUnit);
// displayValue = 10

// Ejemplo 2: 500 Gramos → 17.637 Onzas
var storageValue = 500m;
var targetUnit = MeasurementUnit.Ounce;
var displayValue = UnitConversionHelper.ConvertFromStorageUnit(storageValue, targetUnit);
// displayValue = 17.637

// Ejemplo 3: 3785.41 Mililitros → 1 Galón
var storageValue = 3785.41m;
var targetUnit = MeasurementUnit.Gallon;
var displayValue = UnitConversionHelper.ConvertFromStorageUnit(storageValue, targetUnit);
// displayValue = 1
```

## Uso en CreatePropertyItemCommand

El comando automáticamente convierte las unidades al guardar:

```csharp
// Request del cliente
{
    "propertyId": 1,
    "roomId": 5,
    "productId": 10,
    "unit": 2,  // Liter
    "quantity": 5
}

// Se guarda en la base de datos:
// unit = 1 (Milliliter)
// quantity = 5000
```

## Uso en PropertyItemDto

Para mostrar la cantidad en una unidad específica:

```csharp
var item = await GetPropertyItemById(123);
// item.Quantity = 5000 (almacenado en mililitros)
// item.Unit = Milliliter

// Convertir a litros para mostrar
var displayInLiters = item.GetDisplayQuantity(MeasurementUnit.Liter);
// displayInLiters = 5

// Convertir a galones para mostrar
var displayInGallons = item.GetDisplayQuantity(MeasurementUnit.Gallon);
// displayInGallons = 1.32
```

## Conversiones Soportadas

### Volumen (se almacena en Mililitros)
- Liter → Milliliter (×1000)
- FluidOunce → Milliliter (×29.5735)
- Gallon → Milliliter (×3785.41)

### Peso (se almacena en Gramos)
- Kilogram → Gram (×1000)
- Pound → Gram (×453.592)
- Ounce → Gram (×28.3495)

### Unidades sin conversión
- Unit (piezas)
- Milliliter (ya es unidad base)
- Gram (ya es unidad base)
