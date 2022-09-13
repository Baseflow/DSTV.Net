using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DSTV.Enums;
using DSTV.Exceptions;

namespace DSTV.Data;

[SuppressMessage("Design", "CA1303:Do not pass literals as localized parameters", Justification = "Will fix later")]
public record Contour : DstvElement {
    private readonly List<DstvContourPoint> _pointList;
    private readonly ContourType _type;
    
    private Contour(List<DstvContourPoint> pointList, ContourType type)
    {
        _pointList = pointList;
        _type = type;
    }

    [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "This is a record")]
    public static IEnumerable<Contour> CreateSeveralContours(List<DstvContourPoint> pointList, ContourType type)
    {
        if (pointList is null)
        {
            throw new ArgumentNullException(nameof(pointList));
        }

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
                {
                    throw new DstvParseException("First point of AK/IK block haven\'t flange mark, processing aborted");
                }

                if (i == firstIndex)
                {
                    Console.WriteLine("Warning: first point of contour haven\'t flange mark, mark will be taken from previous contour in section");
                }

                pointList[i].FlCode = pointList[i - 1].FlCode;
            }

            if (pointList[i].Equals(first))
            {
                var lastIndex = i;
                outList.Add(new Contour(pointList.GetRange(firstIndex, lastIndex + 1 - firstIndex), type));
                firstIndex = ++i;
                // if pointer out of list bound:
                if (firstIndex == pointList.Count)
                {
                    break;
                }

                first = pointList[firstIndex];
            }

            if (i == pointList.Count - 1)
            {
                Console.WriteLine("Warning: there are non-closed part of points in end of contour section");
            }
        }

        return outList;
    }

    public override string ToSvg()
    {
        var points = string.Join(" ", _pointList.Select(d => _pointList.IndexOf(d) == 0 ? "M" : "L").Zip(_pointList, (a, b) => $"{a}{b.XCoord},{b.YCoord}"));
        return $"<path d=\"{points}\" fill=\"gray\" stroke=\"black\" stroke-width=\"0.5\" />";
    }
}