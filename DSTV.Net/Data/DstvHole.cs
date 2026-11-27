using DSTV.Net.Exceptions;
using DSTV.Net.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using DSTV.Net.Svg;

namespace DSTV.Net.Data;

[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "This is a DTO")]
public partial record DstvHole : LocatedElement
{
    private const string NumberPattern = @"[+-]?(?:\d+(?:\.\d*)?|\.\d+)";

    [GeneratedRegex(
        @"^\s*(?:[glms])?\s*(?<diameter>" + NumberPattern +
        @")(?:\s+(?<depth>" + NumberPattern +
        @"))?(?:\s*l?\s*(?<slotLength>" + NumberPattern +
        @")\s+(?<slotWidth>" + NumberPattern +
        @")\s+(?<slotAngle>" + NumberPattern + @"))?\s*$",
        RegexOptions.IgnoreCase,
        1000)]
    private static partial Regex HolePattern();

    // 0 if through
    public double Depth { get; }

    /// <summary>
    /// Gets the diameter of the hole. Represents the size of the hole in the object, defined during construction.
    /// </summary>
    public double Diameter { get; }

    public DstvHole(FaceCode FlCode, double XCoord, double YCoord, double Diameter, double Depth)
        : base(FlCode, XCoord, YCoord)
    {
        Type = ContourType.BO;
        this.Diameter = Diameter;
        this.Depth = Depth;
    }


    public static DstvElement CreateHole(string holeNote)
    {
        var (faceCode, xCoord, yCoord, remaining) = ParseLocation(holeNote);
        var match = HolePattern().Match(remaining);
        if (!match.Success)
            throw new DstvParseException("Illegal data vector format (BO)");

        var diam = double.Parse(match.Groups["diameter"].Value, Constants.ParserCultureInfo);
        var depth = 0d;
        if (match.Groups["depth"].Success)
            depth = double.Parse(match.Groups["depth"].Value, Constants.ParserCultureInfo);

        if (match.Groups["slotLength"].Success)
        {
            var slotLen = double.Parse(match.Groups["slotLength"].Value, Constants.ParserCultureInfo);
            var slotWidth = double.Parse(match.Groups["slotWidth"].Value, Constants.ParserCultureInfo);
            var slotAng = double.Parse(match.Groups["slotAngle"].Value, Constants.ParserCultureInfo);
            return new DstvSlot(faceCode, xCoord, yCoord, diam, depth, slotLen, slotWidth, slotAng);
        }

        return new DstvHole(faceCode, xCoord, yCoord, diam, depth);
    }

    public override string ToString() =>
        $"DStVHole : faceCode={FlCode}, xCoord={XCoord}, yCoord={YCoord}, diam={Diameter}, depth={Depth}";

    internal override string ToSvg(FaceCode faceCode, DstvSvgExporter exporter) =>
        IsOnFace(faceCode)
            ? $"<circle class=\"{exporter.Classes(SvgCssClass.Hole, SvgCssClass.Outline)}\" cx=\"{XCoord:F}\" cy=\"{YCoord:F}\" r=\"{Diameter / 2:F}\" />"
            : string.Empty;
}