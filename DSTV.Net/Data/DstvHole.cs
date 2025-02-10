using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using System.Diagnostics.CodeAnalysis;

namespace DSTV.Net.Data;

[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "This is a DTO")]
public record DstvHole : LocatedElement
{
    // 0 if through
    public double Depth { get; }

    /// <summary>
    /// Gets the diameter of the hole. Represents the size of the hole in the object, defined during construction.
    /// </summary>
    public double Diameter { get; }

    protected DstvHole(string flCode, double xCoord, double yCoord, double diam, double depth)
        : base(flCode, xCoord, yCoord)
    {
        Diameter = diam;
        Depth = depth;
    }


    public static DstvElement CreateHole(string holeNote)
    {
        var separated = GetDataVector(holeNote, FineSplitter.Instance);
        separated[0] = separated[0].Trim();
        separated = CorrectSplits(separated, true);
        if (!ValidateFlange(separated[0]))
            throw new DstvParseException("Illegal flange code signature in BO data line");

        if (separated.Length < 4) throw new DstvParseException("Illegal data vector format (BO): too short");

        var xCoord = double.Parse(separated[1], Constants.ParserCultureInfo);
        var yCoord = double.Parse(separated[2], Constants.ParserCultureInfo);
        var diam = double.Parse(separated[3], Constants.ParserCultureInfo);
        var depth = 0d;
        if (separated.Length > 4)
            // the depth can be optional
            depth = double.Parse(separated[4], Constants.ParserCultureInfo);

        if (separated.Length is 4 or 5) return new DstvHole(separated[0], xCoord, yCoord, diam, depth);

        if (separated.Length == 8)
        {
            var slotLen = double.Parse(separated[5], Constants.ParserCultureInfo);
            var slotWidth = double.Parse(separated[6], Constants.ParserCultureInfo);
            var slotAng = double.Parse(separated[7], Constants.ParserCultureInfo);
            return new DstvSlot(separated[0], xCoord, yCoord, diam, depth, slotLen, slotWidth, slotAng);
        }

        throw new DstvParseException("Illegal data vector format (BO): length not equals 5 or 8");
    }

    public override string ToString() =>
        $"DStVHole : flCode='{FlCode}', xCoord={XCoord}, yCoord={YCoord}, diam={Diameter}, depth={Depth}";

    public override string ToSvg() =>
        $"<circle cx=\"{XCoord:F}\" cy=\"{YCoord:F}\" r=\"{Diameter / 2:F}\" fill=\"white\" />";
}