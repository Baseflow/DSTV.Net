using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace DSTV.Net.Data;

[SuppressMessage("Designer", "CA1051:Do not declare visible instance fields", Justification = "This is a DTO")]
public record DstvContourPoint(string FlCode, double XCoord, double YCoord, bool IsNotch, double Radius) : LocatedElement(FlCode, XCoord, YCoord)
{
    public static DstvContourPoint CreatePoint(string dstvElement)
    {
        var separated = GetDataVector(dstvElement, FineSplitter.Instance);
        // temporary flange-code in case of missing a signature in line
        var flCode = "x";
        separated[0] = separated[0].Trim();
        var yCoordIndex = 1;
        if (ValidateFlange(separated[0]))
        {
            flCode = separated[0];
            ++yCoordIndex;
        }

        var isNotchPoint = IsNotchPoint(separated[yCoordIndex]);
        separated = CorrectSplits(separated);
        
        var xCoord = double.Parse(separated[0], Constants.ParserCultureInfo);
        var yCoord = double.Parse(separated[1], Constants.ParserCultureInfo);
        var rad = double.Parse(separated[2], Constants.ParserCultureInfo);
        if (separated.Length <= 4) return new DstvContourPoint(flCode, xCoord, yCoord, isNotchPoint, rad);

        double ang1;
        double blunting1;
        if (separated.Length == 5)
        {
            ang1 = double.Parse(separated[3], Constants.ParserCultureInfo);
            blunting1 = double.Parse(separated[4], Constants.ParserCultureInfo);
            if (ang1 == 0 && blunting1 == 0) return new DstvContourPoint(flCode, xCoord, yCoord, isNotchPoint, rad);

            return new DstvSkewedPoint(flCode, xCoord, yCoord, isNotchPoint, rad, ang1, blunting1, 0, 0);
        }

        if (separated.Length == 7)
        {
            ang1 = double.Parse(separated[4], Constants.ParserCultureInfo);
            var ang2 = double.Parse(separated[6], Constants.ParserCultureInfo);
            blunting1 = double.Parse(separated[5], Constants.ParserCultureInfo);
            if (ang1 == 0 && blunting1 == 0) return new DstvContourPoint(flCode, xCoord, yCoord, isNotchPoint, rad);

            return new DstvSkewedPoint(flCode, xCoord, yCoord, isNotchPoint, rad, ang1, blunting1, ang2, 0);
        }

        if (separated.Length == 8)
        {
            ang1 = double.Parse(separated[4], Constants.ParserCultureInfo);
            blunting1 = double.Parse(separated[5], Constants.ParserCultureInfo);
            var ang2 = double.Parse(separated[6], Constants.ParserCultureInfo);
            var blunting2 = double.Parse(separated[7], Constants.ParserCultureInfo);
            if (ang1 == 0
                && blunting1 == 0
                && ang2 == 0
                && blunting2 == 0)
            {
                return new DstvContourPoint(flCode, xCoord, yCoord, isNotchPoint, rad);
            }

            return new DstvSkewedPoint(flCode, xCoord, yCoord, isNotchPoint, rad, ang1, blunting1, ang2, blunting2);
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
        $"DStVContourPoint : radius={Radius}, flCode=\'{FlCode}\', xCoord={XCoord}, yCoord={YCoord}";
}