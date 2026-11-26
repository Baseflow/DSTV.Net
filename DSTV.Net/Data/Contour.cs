using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;
using DSTV.Net.Svg;

namespace DSTV.Net.Data;

[SuppressMessage("Design", "CA1303:Do not pass literals as localized parameters", Justification = "Will fix later")]
public record Contour : DstvElement
{
    private readonly List<DstvContourPoint> _pointList;

    public IEnumerable<DstvContourPoint> Points => _pointList;

    [JsonConstructor]
    private Contour(IEnumerable<DstvContourPoint> points, ContourType type)
    {
        _pointList = [.. points];
        Type = type;
    }

    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "This is a record")]
    [SuppressMessage("Design", "MA0016:Prefer returning collection abstraction instead of implementation", Justification = "This is a record")]
    public static IEnumerable<Contour> CreateSeveralContours(List<DstvContourPoint> pointList, ContourType type)
        => CreateSeveralContours(pointList, type, NullLogger.Instance);

    internal static IEnumerable<Contour> CreateSeveralContours(List<DstvContourPoint> pointList,
        ContourType type, ILogger logger)
    {
#if NET
        ArgumentNullException.ThrowIfNull(pointList);
#else
        if (pointList is null) throw new ArgumentNullException(nameof(pointList));
#endif

        NormalizePointFaces(pointList);

        List<Contour> outList = [];
        if (type is ContourType.AK or ContourType.IK or ContourType.PU or ContourType.KO)
        {
            outList.Add(new Contour(pointList, type));
            return outList;
        }

        var first = pointList[0];
        var firstIndex = 0;
        for (var i = 1; i < pointList.Count; i++)
        {
            if (IsLegacyContourBoundary(pointList[i], first))
            {
                var lastIndex = i;
                outList.Add(new Contour(pointList.GetRange(firstIndex, lastIndex + 1 - firstIndex), type));
                firstIndex = ++i;
                // if pointer out of list bound:
                if (firstIndex == pointList.Count) break;

                first = pointList[firstIndex];
            }

            if (i == pointList.Count - 1)
                DstvLog.LogUnclosedContour(logger);
        }

        return outList;
    }

    private static void NormalizePointFaces(List<DstvContourPoint> points)
    {
        if (points.Count == 0) return;
        if (points[0].FlCode == FaceCode.None)
            throw new DstvParseException("First point of contour block has no face mark, processing aborted");

        var faceCode = points[0].FlCode;
        for (var index = 1; index < points.Count; index++)
        {
            if (points[index].FlCode == FaceCode.None)
                points[index].FlCode = faceCode;
            else
                faceCode = points[index].FlCode;
        }
    }

    private static bool IsLegacyContourBoundary(DstvContourPoint point, DstvContourPoint first)
    {
        if (Math.Abs(point.YCoord - first.XCoord) <= 0) return false;
        if (Math.Abs(point.YCoord - first.YCoord) <= 0) return false;

        return point.FlCode == first.FlCode;
    }

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "SVG contour traversal keeps face and radius state together")]
    internal override string ToSvg(FaceCode faceCode, DstvSvgExporter exporter)
    {
        var sb = new StringBuilder();
        var sbLine = new StringBuilder();
        var previous = new DstvContourPoint(FaceCode.None, 0, 0, IsNotch: false, 0);
        var activeFace = FaceCode.None;
        var inPath = false;
        foreach (var point in _pointList)
        {
            if (point.FlCode != FaceCode.None) activeFace = point.FlCode;
            if (activeFace != faceCode)
            {
                inPath = false;
                previous = point;
                continue;
            }

            if (!inPath)
            {
                sb.Append("M ").Append(point.XCoord).Append(' ').Append(point.YCoord);
                inPath = true;
                previous = point;
                continue;
            }

            sb.Append(' ');
            var radius = previous.Radius;
            switch (radius)
            {
                // top-right corner Quadratic Bézier curve
                case > 0 when previous.YCoord < point.YCoord && point.XCoord > previous.XCoord:
                    sb.Append("Q ").Append(point.XCoord).Append(' ').Append(previous.YCoord)
                        .Append(' ').Append(point.XCoord).Append(' ').Append(point.YCoord);
                    break;
                // right-bottom corner Quadratic Bézier curve
                case > 0 when previous.YCoord < point.YCoord && point.XCoord < previous.XCoord:
                    sb.Append("Q ").Append(previous.XCoord).Append(' ').Append(point.YCoord)
                        .Append(' ').Append(point.XCoord).Append(' ').Append(point.YCoord);
                    break;
                // bottom-left corner Quadratic Bézier curve
                case > 0 when previous.YCoord > point.YCoord && point.XCoord < previous.XCoord:
                    sb.Append("Q ").Append(point.XCoord).Append(' ').Append(previous.YCoord)
                        .Append(' ').Append(point.XCoord).Append(' ').Append(point.YCoord);
                    break;
                // in all other cases we use arc if the radius is not 0
                case > 0:
                case < 0:
                    sb.Append("A ").Append(-radius).Append(' ').Append(-radius).Append(" 0 0 0 ").Append(point.XCoord)
                        .Append(' ').Append(point.YCoord);
                    break;
                // straight Line
                default:
                    sb.Append('L').Append(point.XCoord).Append(',').Append(point.YCoord);
                    break;
            }

            // if previous point is screwing point, we draw a line from it to current point to show a bezel line
            if (previous is DstvSkewedPoint screwingPoint)
            {
                sbLine.Append("<line x1=\"").Append(screwingPoint.XCoord).Append("\" y1=\"")
                    .Append(screwingPoint.YCoord).Append("\" x2=\"").Append(point.XCoord).Append("\" y2=\"")
                    .Append(point.YCoord).Append("\" class=\"")
                    .Append(exporter.Classes(SvgCssClass.Bevel, SvgCssClass.Outline)).Append("\" />");
            }

            previous = point;
        }

        var points = sb.ToString();
        if (points.Length == 0) return string.Empty;

        var contourClass = Type switch
        {
            ContourType.PU => SvgCssClass.Powder,
            ContourType.KO => SvgCssClass.Punch,
            ContourType.IK => SvgCssClass.InnerContour,
            _ => SvgCssClass.OuterContour,
        };
        var cssClass = Type is ContourType.PU or ContourType.KO
            ? exporter.Classes(SvgCssClass.Contour, SvgCssClass.Marking, contourClass)
            : exporter.Classes(SvgCssClass.Contour, contourClass);
        return $"<path class=\"{cssClass}\" d=\"{points}\" />{sbLine}";
    }
}