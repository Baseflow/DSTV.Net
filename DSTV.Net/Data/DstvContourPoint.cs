using DSTV.Net.Exceptions;
using DSTV.Net.Enums;
using DSTV.Net.Implementations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace DSTV.Net.Data;

[SuppressMessage("Designer", "CA1051:Do not declare visible instance fields", Justification = "This is a DTO")]
[JsonPolymorphic]
[JsonDerivedType(typeof(DstvSkewedPoint), "skewed")]
public record DstvContourPoint(FaceCode FlCode, double XCoord, double YCoord, bool IsNotch, double Radius)
{
    public FaceCode FlCode { get; set; } = FlCode;

    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "Parsing the supported contour layouts is clearer in one method")]
    public static DstvContourPoint CreatePoint(string dstvElement)
    {
        var separated = DstvElement.GetDataVector(dstvElement, FineSplitter.Instance);
        var flCode = FaceCode.None;
        separated[0] = separated[0].Trim();
        var yCoordIndex = 1;
        if (DstvElement.TryParseFaceCode(separated[0], out var parsedFlCode))
        {
            flCode = parsedFlCode;
            ++yCoordIndex;
        }

        var isNotchPoint = IsNotchPoint(separated[yCoordIndex]);
        separated = DstvElement.CorrectSplits(separated);
        
        var xCoord = double.Parse(separated[0], Constants.ParserCultureInfo);
        var yCoord = double.Parse(separated[1], Constants.ParserCultureInfo);
        var rad = double.Parse(separated[2], Constants.ParserCultureInfo);
        if (separated.Length <= 3)
            return new DstvContourPoint(flCode, xCoord, yCoord, isNotchPoint, rad);

        double ang1;
        double blunting1;
        if (separated.Length == 5)
        {
            ang1 = double.Parse(separated[3], Constants.ParserCultureInfo);
            blunting1 = double.Parse(separated[4], Constants.ParserCultureInfo);
            if (ang1 == 0 && blunting1 == 0)
                return new DstvContourPoint(flCode, xCoord, yCoord, isNotchPoint, rad);

            return new DstvSkewedPoint(flCode, xCoord, yCoord, isNotchPoint, rad, ang1, blunting1, 0, 0);
        }

        if (separated.Length == 7)
        {
            ang1 = double.Parse(separated[3], Constants.ParserCultureInfo);
            blunting1 = double.Parse(separated[4], Constants.ParserCultureInfo);
            var ang2 = double.Parse(separated[5], Constants.ParserCultureInfo);
            var blunting2 = double.Parse(separated[6], Constants.ParserCultureInfo);
            if (ang1 == 0
                && blunting1 == 0
                && ang2 == 0
                && blunting2 == 0)
            {
                return new DstvContourPoint(flCode, xCoord, yCoord, isNotchPoint, rad);
            }

            return new DstvSkewedPoint(flCode, xCoord, yCoord, isNotchPoint, rad, ang1, blunting1, ang2,
                blunting2);
        }

        throw new DstvParseException("Illegal data vector format (AK/IK)");
    }

    private static bool IsNotchPoint(string yCoordValue)
    {
        var numberRegexString = "[.\\d-]+";
        var regexString = $"{numberRegexString}[wt]";
        return Regex.IsMatch(yCoordValue, regexString, RegexOptions.None, TimeSpan.FromSeconds(1));
    }

    public override string ToString() =>
        $"DStVContourPoint : radius={Radius}, faceCode={FlCode}, xCoord={XCoord}, yCoord={YCoord}";
}