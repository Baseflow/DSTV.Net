using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DSTV.Net.Data;

[SuppressMessage("Design", "CA1303:Do not pass literals as localized parameters", Justification = "Will fix later")]
public record Contour : DstvElement
{
    /// <summary>
    /// Gets the list of contour points that define the shape of the contour.
    /// </summary>
    public IReadOnlyList<DstvContourPoint> PointList => _pointList;

    private readonly List<DstvContourPoint> _pointList;

    /// <summary>
    /// Gets the <see cref="ContourType"/> of the contour.
    /// </summary>
    public ContourType Type { get; }

    public IEnumerable<DstvContourPoint> Points => _pointList.AsEnumerable();

    private Contour(List<DstvContourPoint> pointList, ContourType type)
    {
        _pointList = pointList;
        Type = type;
    }

    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "This is a record")]
    [SuppressMessage("Design", "MA0016:Prefer returning collection abstraction instead of implementation", Justification = "This is a record")]
    public static IEnumerable<Contour> CreateSeveralContours(List<DstvContourPoint> pointList, ContourType type)
    {
#if NET
        ArgumentNullException.ThrowIfNull(pointList, nameof(pointList));
#else
        if (pointList is null) throw new ArgumentNullException(nameof(pointList));
#endif

        List<Contour> outList = new();
        if (type is ContourType.AK or ContourType.IK)
        {
            outList.Add(new Contour(pointList, type));
            return outList;
        }

        var first = pointList[0];
        var firstIndex = 0;
        for (var i = 1; i < pointList.Count; i++)
        {
            if (pointList[i].FlCode.Equals("x", StringComparison.OrdinalIgnoreCase))
            {
                if (i == 0)
                    throw new DstvParseException("First point of AK/IK block haven\'t flange mark, processing aborted");

                if (i == firstIndex)
                {
                    Console.WriteLine(
                        "Warning: first point of contour haven\'t flange mark, mark will be taken from previous contour in section");
                }

                pointList[i].FlCode = pointList[i - 1].FlCode;
            }

            if (pointList[i].Equals(first))
            {
                var lastIndex = i;
                outList.Add(new Contour(pointList.GetRange(firstIndex, lastIndex + 1 - firstIndex), type));
                firstIndex = ++i;
                // if pointer out of list bound:
                if (firstIndex == pointList.Count) break;

                first = pointList[firstIndex];
            }

            if (i == pointList.Count - 1)
                Console.WriteLine("Warning: there are non-closed part of points in end of contour section");
        }

        return outList;
    }

    public override string ToSvg()
    {
        var sb = new StringBuilder();
        var sbLine = new StringBuilder();
        var previous = new DstvContourPoint("x", 0, 0, false, 0);
        foreach (var point in _pointList)
        {
            if (_pointList.IndexOf(point) == 0)
            {
                sb.Append("M ").Append(point.XCoord).Append(' ').Append(point.YCoord);
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
                    .Append(point.YCoord).Append("\" stroke=\"red\" stroke-width=\"4\" />");
            }
            
            previous = point;
        }

        var points = sb.ToString();
        var color = Type == ContourType.AK ? "grey" : "white";
        return $"<path d=\"{points}\" fill=\"{color}\" stroke=\"black\" stroke-width=\"0.5\" />{sbLine}";
    }
}