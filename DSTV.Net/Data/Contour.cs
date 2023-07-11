using System.Diagnostics.CodeAnalysis;
using System.Text;
using DSTV.Net.Enums;
using DSTV.Net.Exceptions;

namespace DSTV.Net.Data;

[SuppressMessage("Design", "CA1303:Do not pass literals as localized parameters", Justification = "Will fix later")]
public record Contour : DstvElement
{
    private readonly List<DstvContourPoint> _pointList;

    // ReSharper disable once NotAccessedField.Local
    private readonly ContourType _type;

    private Contour(List<DstvContourPoint> pointList, ContourType type)
    {
        _pointList = pointList;
        _type = type;
    }

    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "This is a record")]
    public static IEnumerable<Contour> CreateSeveralContours(List<DstvContourPoint> pointList, ContourType type)
    {
        if (pointList is null) throw new ArgumentNullException(nameof(pointList));

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
                    Console.WriteLine(
                        "Warning: first point of contour haven\'t flange mark, mark will be taken from previous contour in section");

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
        var isFirst = true;
        var previous = new DstvContourPoint("x", 0, 0, 0);
        foreach (var point in _pointList)
        {
            if (isFirst)
            {
                sb.Append('M').Append(point.XCoord).Append(',').Append(point.YCoord);
            }
            else
            {
                sb.Append(' ');
                var radius = previous.Radius;
                if (radius > 0)
                {
                    if (previous.YCoord > point.YCoord && point.XCoord > previous.XCoord) // left-top corner
                        sb.Append('Q').Append(previous.XCoord).Append(',').Append(point.YCoord)
                            .Append(',').Append(point.XCoord).Append(',').Append(point.YCoord);
                    else if (previous.YCoord < point.YCoord && point.XCoord > previous.XCoord) // top-right corner
                        sb.Append('Q').Append(point.XCoord).Append(',').Append(previous.YCoord)
                            .Append(',').Append(point.XCoord).Append(',').Append(point.YCoord);
                    else if (previous.YCoord < point.YCoord && point.XCoord < previous.XCoord) // right-bottom corner
                        sb.Append('Q').Append(previous.XCoord).Append(',').Append(point.YCoord)
                            .Append(',').Append(point.XCoord).Append(',').Append(point.YCoord);
                    else if (previous.YCoord > point.YCoord && point.XCoord < previous.XCoord) // bottom-left corner
                        sb.Append('Q').Append(point.XCoord).Append(',').Append(previous.YCoord)
                            .Append(',').Append(point.XCoord).Append(',').Append(point.YCoord);
                }
                else
                {
                    sb.Append('L').Append(point.XCoord).Append(',').Append(point.YCoord);
                }

                if (previous is DstvSkewedPoint screwingPoint)
                {
                    sbLine.Append("<line x1=\"").Append(screwingPoint.XCoord).Append("\" y1=\"")
                        .Append(screwingPoint.YCoord).Append("\" x2=\"").Append(point.XCoord).Append("\" y2=\"")
                        .Append(point.YCoord).Append("\" stroke=\"red\" stroke-width=\"4\" />");
                }
            }

            isFirst = false;
            previous = point;
        }

        var points = sb.ToString();
        //var points = string.Join(" ", _pointList.Select(d => _pointList.IndexOf(d) == 0 ? "M" : d is DstvContourPoint cp && cp.Radius >0 ? "Q" : "L").Zip(_pointList, (a, b) => $"{a}{b.XCoord},{b.YCoord}"));
        return $"<path d=\"{points}\" fill=\"gray\" stroke=\"black\" stroke-width=\"0.5\" />{sbLine}";
    }
}