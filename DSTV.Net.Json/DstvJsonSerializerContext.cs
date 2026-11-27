using System.Text.Json.Serialization;
using DSTV.Net.Data;

namespace DSTV.Net.Json;

/// <summary>
/// Source-generated JSON type metadata for DSTV models.
/// </summary>
[JsonSourceGenerationOptions(UseStringEnumConverter = true)]
[JsonSerializable(typeof(DstvRecord))]
[JsonSerializable(typeof(DstvHeader))]
[JsonSerializable(typeof(DstvElement))]
[JsonSerializable(typeof(Contour))]
[JsonSerializable(typeof(DstvHole))]
[JsonSerializable(typeof(DstvSlot))]
[JsonSerializable(typeof(DstvNumeration))]
[JsonSerializable(typeof(DstvBend))]
[JsonSerializable(typeof(DstvCut))]
[JsonSerializable(typeof(DstvContourPoint))]
[JsonSerializable(typeof(DstvSkewedPoint))]
public partial class DstvJsonSerializerContext : JsonSerializerContext;
