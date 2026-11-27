using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using System.Net;
using System.Text.RegularExpressions;
using DSTV.Net.Svg;

namespace DSTV.Net.Data;

public partial record DstvNumeration : LocatedElement
{
    private const string NumberPattern = @"[+-]?(?:\d+(?:\.\d*)?|\.\d+)";

#if NET7_0_OR_GREATER
    [GeneratedRegex(
        @"^\s*(?<angle>" + NumberPattern + @")\s+(?<letterHeight>" + NumberPattern +
        @")(?:\s*[rz])?\s*(?<text>.+?)\s*$",
        RegexOptions.IgnoreCase,
        1000)]
    private static partial Regex NumerationPattern();
#else
    private static readonly Regex NumerationRegex = new(
        @"^\s*(?<angle>" + NumberPattern + @")\s+(?<letterHeight>" + NumberPattern +
        @")(?:\s*[rz])?\s*(?<text>.+?)\s*$",
        RegexOptions.IgnoreCase,
        TimeSpan.FromSeconds(1));

    private static Regex NumerationPattern() => NumerationRegex;
#endif

    public double Angle { get; }

    public double LetterHeight { get; }

    public string Text { get; }

    public DstvNumeration(FaceCode FlCode, double XCoord, double YCoord, double Angle, double LetterHeight,
        string Text) :
        base(FlCode, XCoord, YCoord)
    {
        Type = ContourType.SI;
        this.Angle = Angle;
        this.LetterHeight = LetterHeight;
        this.Text = Text;
    }


    public static DstvNumeration CreateNumeration(string dstvLine)
    {
        var (flCode, xCoord, yCoord, remaining) = ParseLocation(dstvLine);
        var match = NumerationPattern().Match(remaining);
        if (!match.Success)
            throw new DstvParseException("Illegal numeric data vector format (SI)");

        var angle = double.Parse(match.Groups["angle"].Value, Constants.ParserCultureInfo);
        var letterHeight = double.Parse(match.Groups["letterHeight"].Value, Constants.ParserCultureInfo);
        var text = match.Groups["text"].Value.Trim();

        return new DstvNumeration(flCode, xCoord, yCoord, angle, letterHeight, text);
    }

    public override string ToString() =>
        $"Numeration: FlCode={FlCode}, XCoord={XCoord}, YCoord={YCoord}, Angle={Angle}, LetterHeight {LetterHeight}, Text : {Text}";

    internal override string ToSvg(FaceCode faceCode, DstvSvgExporter exporter) =>
        IsOnFace(faceCode)
            ? $"<text class=\"{exporter.Classes(SvgCssClass.Text)}\" x=\"{XCoord:F}\" y=\"{YCoord:F}\" font-size=\"{LetterHeight:F}\" transform=\"rotate({-Angle:F} {XCoord:F} {YCoord:F})\">{WebUtility.HtmlEncode(Text)}</text>"
            : string.Empty;
}