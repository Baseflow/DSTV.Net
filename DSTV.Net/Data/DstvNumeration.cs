using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using System.Text.Json.Serialization;
using System.Net;
using DSTV.Net.Svg;

namespace DSTV.Net.Data;

public record DstvNumeration : LocatedElement
{
    public double Angle { get; }

    public double LetterHeight { get; }

    public string Text { get; }

    [JsonConstructor]
    private DstvNumeration(FaceCode FlCode, double XCoord, double YCoord, double Angle, double LetterHeight,
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
        var separated = GetDataVector(dstvLine, FineSplitter.Instance);
        if (separated.Length < 6)
            throw new DstvParseException("Illegal data-vector length (SI) - too short");

        if (!TryParseFaceCode(separated[0], out var flCode))
            throw new DstvParseException("Illegal face code signature in SI data line");

        var coordinates = CorrectSplits(separated[1..5]);
        if (coordinates.Length != 4)
            throw new DstvParseException("Illegal numeric data vector format (SI)");

        var xCoord = double.Parse(coordinates[0], Constants.ParserCultureInfo);
        var yCoord = double.Parse(coordinates[1], Constants.ParserCultureInfo);
        var angle = double.Parse(coordinates[2], Constants.ParserCultureInfo);
        var letterHeight = double.Parse(coordinates[3], Constants.ParserCultureInfo);
        var text = string.Join(" ", separated[5..]);

        return new DstvNumeration(flCode, xCoord, yCoord, angle, letterHeight, text);
    }

    public override string ToString() =>
        $"Numeration: FlCode={FlCode}, XCoord={XCoord}, YCoord={YCoord}, Angle={Angle}, LetterHeight {LetterHeight}, Text : {Text}";

    internal override string ToSvg(FaceCode faceCode, DstvSvgExporter exporter) =>
        IsOnFace(faceCode)
            ? $"<text class=\"{exporter.Classes(SvgCssClass.Text)}\" x=\"{XCoord:F}\" y=\"{YCoord:F}\" font-size=\"{LetterHeight:F}\" transform=\"rotate({-Angle:F} {XCoord:F} {YCoord:F})\">{WebUtility.HtmlEncode(Text)}</text>"
            : string.Empty;
}