using System.Diagnostics.CodeAnalysis;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;

namespace DSTV.Net.Data;

[SuppressMessage("Designer", "CA1051:Do not declare visible instance fields", Justification = "This is a DTO")]
public record DstvContourPoint : LocatedElem
{
    protected readonly double _radius;

    public DstvContourPoint(string flCode, double xCoord, double yCoord, double radius) :
        base(flCode, xCoord, yCoord) => _radius = radius;

    public double Radius => _radius;

    public static DstvContourPoint CreatePoint(string dstvElement)
    {
        var separated = GetDataVector(dstvElement, FineSplitter.Instance);
        // temporary flange-code in case of missing a signature in line
        var flCode = "x";
        separated[0] = separated[0].Trim();
        if (ValidateFlange(separated[0])) flCode = separated[0];
        separated = CorrectSplits(separated);
        
        var xCoord = double.Parse(separated[0], Constants.ParserCultureInfo);
        var yCoord = double.Parse(separated[1], Constants.ParserCultureInfo);
        var rad = double.Parse(separated[2], Constants.ParserCultureInfo);
        if (separated.Length <= 4) return new DstvContourPoint(flCode, xCoord, yCoord, rad);

        double ang1;
        double blunting1;
        if (separated.Length == 6)
        {
            ang1 = double.Parse(separated[4], Constants.ParserCultureInfo);
            blunting1 = double.Parse(separated[5], Constants.ParserCultureInfo);
            if (ang1 == 0 && blunting1 == 0) return new DstvContourPoint(flCode, xCoord, yCoord, rad);

            return new DstvSkewedPoint(flCode, xCoord, yCoord, rad, ang1, blunting1, 0, 0);
        }

        if (separated.Length == 7)
        {
            ang1 = double.Parse(separated[4], Constants.ParserCultureInfo);
            var ang2 = double.Parse(separated[6], Constants.ParserCultureInfo);
            blunting1 = double.Parse(separated[5], Constants.ParserCultureInfo);
            if (ang1 == 0 && blunting1 == 0) return new DstvContourPoint(flCode, xCoord, yCoord, rad);

            return new DstvSkewedPoint(flCode, xCoord, yCoord, rad, ang1, blunting1, ang2, 0);
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
                return new DstvContourPoint(flCode, xCoord, yCoord, rad);
            }

            return new DstvSkewedPoint(flCode, xCoord, yCoord, rad, ang1, blunting1, ang2, blunting2);
        }

        throw new DstvParseException("Illegal data vector format (AK/IK)");
    }

    public override string ToString() =>
        $"DStVContourPoint : radius={_radius}, flCode=\'{FlCode}\', xCoord={XCoord}, yCoord={YCoord}";
}