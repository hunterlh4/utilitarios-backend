using BackofficeCore.Domain.Enums;

namespace BackofficeCore.Shared.Utils;

/// <summary>
/// Helper para convertir unidades de medida entre formatos de almacenamiento y presentación
/// </summary>
public static class UnitConversionHelper
{
    /// <summary>
    /// Convierte una cantidad de una unidad grande a su unidad base de almacenamiento (pequeña)
    /// Ejemplo: 5 Litros → 5000 Mililitros, 10 Onzas líquidas → 295.735 Mililitros
    /// </summary>
    /// <param name="value">Valor a convertir</param>
    /// <param name="unit">Unidad de medida</param>
    /// <returns>Valor convertido a la unidad base de almacenamiento</returns>
    public static decimal ConvertToStorageUnit(decimal value, MeasurementUnit unit)
    {
        return unit switch
        {
            // Volumen líquido - almacenar en mililitros
            MeasurementUnit.Liter => value * 1000m,
            MeasurementUnit.FluidOunce => value * 29.5735m,  // Onzas líquidas (fl oz)
            MeasurementUnit.Gallon => value * 3785.41m,
            
            // Peso - almacenar en gramos
            MeasurementUnit.Kilogram => value * 1000m,
            MeasurementUnit.Pound => value * 453.592m,
            MeasurementUnit.Ounce => value * 28.3495m,  // Onzas de peso (oz)
            
            // Unidades base - no requieren conversión
            MeasurementUnit.Milliliter => value,
            MeasurementUnit.Gram => value,
            MeasurementUnit.Unit => value,
            
            _ => value
        };
    }

    /// <summary>
    /// Convierte una cantidad de la unidad base de almacenamiento a la unidad de presentación deseada
    /// Ejemplo: 5000 Mililitros → 5 Litros, 295.735 Mililitros → 10 Onzas líquidas
    /// </summary>
    /// <param name="storageValue">Valor almacenado en unidad base</param>
    /// <param name="targetUnit">Unidad de medida objetivo para presentación</param>
    /// <returns>Valor convertido a la unidad de presentación</returns>
    public static decimal ConvertFromStorageUnit(decimal storageValue, MeasurementUnit targetUnit)
    {
        return targetUnit switch
        {
            // Volumen líquido - desde mililitros
            MeasurementUnit.Liter => storageValue / 1000m,
            MeasurementUnit.FluidOunce => storageValue / 29.5735m,  // Onzas líquidas (fl oz)
            MeasurementUnit.Gallon => storageValue / 3785.41m,
            
            // Peso - desde gramos
            MeasurementUnit.Kilogram => storageValue / 1000m,
            MeasurementUnit.Pound => storageValue / 453.592m,
            MeasurementUnit.Ounce => storageValue / 28.3495m,  // Onzas de peso (oz)
            
            // Unidades base - no requieren conversión
            MeasurementUnit.Milliliter => storageValue,
            MeasurementUnit.Gram => storageValue,
            MeasurementUnit.Unit => storageValue,
            
            _ => storageValue
        };
    }

    /// <summary>
    /// Obtiene la unidad base de almacenamiento para una unidad dada
    /// FluidOunce (líquido) → Milliliter, Ounce (peso) → Gram
    /// </summary>
    /// <param name="unit">Unidad de medida</param>
    /// <returns>Unidad base correspondiente</returns>
    public static MeasurementUnit GetStorageUnit(MeasurementUnit unit)
    {
        return unit switch
        {
            // Volumen líquido → Mililitros
            MeasurementUnit.Liter or MeasurementUnit.FluidOunce or MeasurementUnit.Gallon => MeasurementUnit.Milliliter,
            
            // Peso → Gramos
            MeasurementUnit.Kilogram or MeasurementUnit.Pound or MeasurementUnit.Ounce => MeasurementUnit.Gram,
            
            _ => unit
        };
    }

    /// <summary>
    /// Obtiene el tipo de medida (Volumen, Peso, Unidad)
    /// </summary>
    public static MeasurementType GetMeasurementType(MeasurementUnit unit)
    {
        return unit switch
        {
            MeasurementUnit.Milliliter or MeasurementUnit.Liter or MeasurementUnit.FluidOunce or MeasurementUnit.Gallon => MeasurementType.Volume,
            MeasurementUnit.Gram or MeasurementUnit.Kilogram or MeasurementUnit.Pound or MeasurementUnit.Ounce => MeasurementType.Weight,
            MeasurementUnit.Unit => MeasurementType.Unit,
            _ => MeasurementType.Unknown
        };
    }

    /// <summary>
    /// Valida si dos unidades son compatibles (mismo tipo de medida)
    /// </summary>
    public static bool AreUnitsCompatible(MeasurementUnit unit1, MeasurementUnit unit2)
    {
        var type1 = GetMeasurementType(unit1);
        var type2 = GetMeasurementType(unit2);
        
        return type1 == type2 && type1 != MeasurementType.Unknown;
    }
}

/// <summary>
/// Tipo de medida
/// </summary>
public enum MeasurementType
{
    Unknown = 0,
    Volume = 1,   // Líquidos
    Weight = 2,   // Peso
    Unit = 3      // Unidades (piezas)
}
