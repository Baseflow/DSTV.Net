using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using DSTV.Net.Data;

namespace DSTV.Net.Json;

/// <summary>
/// Serializes DSTV models using source-generated type metadata.
/// </summary>
public static class DstvJson
{
    /// <summary>
    /// Creates options configured with DSTV polymorphism metadata.
    /// </summary>
    public static JsonSerializerOptions CreateSerializerOptions(JsonSerializerOptions? options = null)
    {
        var result = options is null ? new JsonSerializerOptions() : new JsonSerializerOptions(options);
        result.TypeInfoResolver = JsonTypeInfoResolver.WithAddedModifier(
            DstvJsonSerializerContext.Default,
            ConfigurePolymorphism);
        return result;
    }

    /// <summary>
    /// Serializes a DSTV record.
    /// </summary>
    public static string Serialize(DstvRecord record, JsonSerializerOptions? options = null)
    {
#if NET
        ArgumentNullException.ThrowIfNull(record);
#else
        if (record is null) throw new ArgumentNullException(nameof(record));
#endif
        return JsonSerializer.Serialize(record, CreateSerializerOptions(options));
    }

    /// <summary>
    /// Deserializes a DSTV record.
    /// </summary>
    public static DstvRecord? Deserialize(string json, JsonSerializerOptions? options = null)
    {
#if NET
        ArgumentNullException.ThrowIfNull(json);
#else
        if (json is null) throw new ArgumentNullException(nameof(json));
#endif
        return JsonSerializer.Deserialize<DstvRecord>(json, CreateSerializerOptions(options));
    }

    private static void ConfigurePolymorphism(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Type == typeof(DstvElement))
        {
            typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(Contour), "contour"),
                    new JsonDerivedType(typeof(DstvHole), "hole"),
                    new JsonDerivedType(typeof(DstvSlot), "slot"),
                    new JsonDerivedType(typeof(DstvNumeration), "numeration"),
                    new JsonDerivedType(typeof(DstvBend), "bend"),
                    new JsonDerivedType(typeof(DstvCut), "cut"),
                },
            };
        }

        if (typeInfo.Type == typeof(DstvContourPoint))
            typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                DerivedTypes = { new JsonDerivedType(typeof(DstvSkewedPoint), "skewed") },
            };
    }
}
