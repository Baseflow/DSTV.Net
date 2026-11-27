using DSTV.Net.Contracts;
using DSTV.Net.Data;
using System.Globalization;
using System.Text;

namespace DSTV.Net.Comparers;

/// <summary>
/// Compares DSTV models without considering block order or contour traversal order.
/// </summary>
public sealed class DstvSemanticComparer : IEqualityComparer<IDstv>
{
    public static DstvSemanticComparer Instance { get; } = new();

    public bool Equals(IDstv? x, IDstv? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return string.Equals(HeaderSignature(x.Header), HeaderSignature(y.Header), StringComparison.Ordinal)
               && ElementSignatures(x).SequenceEqual(ElementSignatures(y));
    }

    public int GetHashCode(IDstv obj)
    {
#if NET
        ArgumentNullException.ThrowIfNull(obj);
#else
        if (obj is null) throw new ArgumentNullException(nameof(obj));
#endif
        var hash = new HashCode();
        hash.Add(HeaderSignature(obj.Header), StringComparer.Ordinal);
        foreach (var signature in ElementSignatures(obj))
            hash.Add(signature, StringComparer.Ordinal);
        return hash.ToHashCode();
    }

    private static string HeaderSignature(IDstvHeader? header)
    {
        if (header is null) return "<null>";

        return string.Join("|",
            Escape(header.OrderIdentification),
            Escape(header.DrawingIdentification),
            Escape(header.PhaseIdentification),
            Escape(header.PieceIdentification),
            Escape(header.SteelQuality),
            header.QuantityOfPieces.ToString(CultureInfo.InvariantCulture),
            Escape(header.Profile),
            header.CodeProfile,
            header.Length.ToString("R", CultureInfo.InvariantCulture),
            header.SawLength?.ToString("R", CultureInfo.InvariantCulture),
            header.ProfileHeight.ToString("R", CultureInfo.InvariantCulture),
            header.FlangeWidth.ToString("R", CultureInfo.InvariantCulture),
            header.FlangeThickness.ToString("R", CultureInfo.InvariantCulture),
            header.WebThickness.ToString("R", CultureInfo.InvariantCulture),
            header.Radius.ToString("R", CultureInfo.InvariantCulture),
            header.WeightByMeter.ToString("R", CultureInfo.InvariantCulture),
            header.PaintingSurfaceByMeter.ToString("R", CultureInfo.InvariantCulture),
            header.WebStartCut.ToString("R", CultureInfo.InvariantCulture),
            header.WebEndCut.ToString("R", CultureInfo.InvariantCulture),
            header.FlangeStartCut.ToString("R", CultureInfo.InvariantCulture),
            header.FlangeEndCut.ToString("R", CultureInfo.InvariantCulture),
            Escape(header.Text1InfoOnPiece),
            Escape(header.Text2InfoOnPiece),
            Escape(header.Text3InfoOnPiece),
            Escape(header.Text4InfoOnPiece));
    }

    private static string[] ElementSignatures(IDstv model) =>
        [.. model.Elements.Select(ElementSignature).Order(StringComparer.Ordinal)];

    private static string ElementSignature(DstvElement element) =>
        element switch
        {
            Contour contour => ContourSignature(contour),
            DstvSlot slot => $"slot|{SlotSignature(slot)}",
            DstvHole hole => $"hole|{HoleSignature(hole)}",
            DstvNumeration numeration => $"numeration|{Format(numeration.FlCode)}|"
                                          + $"{Format(numeration.XCoord)}|{Format(numeration.YCoord)}|"
                                          + $"{Format(numeration.Angle)}|{Format(numeration.LetterHeight)}|"
                                          + Escape(numeration.Text),
            DstvBend bend => $"bend|{Format(bend.OriginX)}|{Format(bend.OriginY)}|"
                             + $"{Format(bend.FinishX)}|{Format(bend.FinishY)}|"
                             + $"{Format(bend.BendingAngle)}|{Format(bend.BendingRadius)}",
            DstvCut cut => $"cut|{Format(cut.OriginX)}|{Format(cut.OriginY)}|{Format(cut.OriginZ)}|"
                           + $"{Format(cut.NormalX)}|{Format(cut.NormalY)}|{Format(cut.NormalZ)}",
            _ => throw new NotSupportedException($"Unsupported DSTV element type: {element.GetType().Name}"),
        };

    private static string SlotSignature(DstvSlot slot) =>
        $"{HoleSignature(slot)}|{Format(slot.SlotLength)}|{Format(slot.SlotWidth)}|"
        + Format(slot.SlotAngle);

    private static string HoleSignature(DstvHole hole) =>
        $"{Format(hole.FlCode)}|{Format(hole.XCoord)}|{Format(hole.YCoord)}|"
        + $"{Format(hole.Diameter)}|{Format(hole.Depth)}";

    private static string ContourSignature(Contour contour)
    {
        DstvContourPoint[] points = [.. contour.Points];
        if (points.Length == 0) return $"contour|{Format(contour.Type)}|open|";

        var closed = string.Equals(PointSignature(points[0]), PointSignature(points[^1]),
            StringComparison.Ordinal);
        if (closed)
        {
#if NETSTANDARD2_0
            var openPoints = new DstvContourPoint[points.Length - 1];
            Array.Copy(points, openPoints, openPoints.Length);
            points = openPoints;
#else
            points = points[..^1];
#endif
        }

        var forward = PointSignatures(points);
        var reverse = PointSignatures(points.Reverse());
        var normalized = closed
            ? MinimumRotation(forward, reverse)
            : string.Compare(
                string.Join(";", forward),
                string.Join(";", reverse),
                StringComparison.Ordinal) <= 0
                ? string.Join(";", forward)
                : string.Join(";", reverse);

        return $"contour|{Format(contour.Type)}|{(closed ? "closed" : "open")}|{normalized}";
    }

    private static string[] PointSignatures(IEnumerable<DstvContourPoint> points) =>
        [.. points.Select(PointSignature)];

    private static string PointSignature(DstvContourPoint point)
    {
        StringBuilder signature = new();
        signature
            .Append(Format(point.FlCode)).Append('|')
            .Append(Format(point.XCoord)).Append('|')
            .Append(Format(point.YCoord)).Append('|')
            .Append(point.IsNotch).Append('|')
            .Append(Format(point.Radius));

        if (point is DstvSkewedPoint skewed)
        {
            signature.Append('|').Append(Format(skewed.FirstAngle))
                .Append('|').Append(Format(skewed.FirstBlunting))
                .Append('|').Append(Format(skewed.SecondAngle))
                .Append('|').Append(Format(skewed.SecondBlunting));
        }

        return signature.ToString();
    }

    private static string MinimumRotation(params string[][] traversals)
    {
        var minimum = string.Empty;
        foreach (var traversal in traversals)
        {
            for (var offset = 0; offset < traversal.Length; offset++)
            {
                var candidate = string.Join(";",
                    Enumerable.Range(0, traversal.Length)
                        .Select(index => traversal[(index + offset) % traversal.Length]));
                if (minimum.Length == 0 ||
                    string.Compare(candidate, minimum, StringComparison.Ordinal) < 0)
                    minimum = candidate;
            }
        }

        return minimum;
    }

    private static string Format<T>(T value) where T : struct, IFormattable =>
        value.ToString(null, CultureInfo.InvariantCulture) ?? string.Empty;

    private static string Escape(string? value) =>
        value is null ? "<null>" : Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
}
