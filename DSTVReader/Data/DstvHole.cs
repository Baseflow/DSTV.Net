using DSTV.Exceptions;
using DSTV.Implementations;

namespace DSTV.Data;

public record DstvHole : LocatedElem
{
    //0 if through
    private readonly double _depth;
    private readonly double _diam;

    public DstvHole(string flCode, double xCoord, double yCoord, double diam, double depth) : base(flCode, xCoord, yCoord)
    {
        _diam = diam;
        _depth = depth;
    }


    public static DstvElement CreateHole(string holeNote)
    {
        var separated = GetDataVector(holeNote, FineSplitter.Instance);
        separated[0] = separated[0].Trim();
        separated = CorrectSplits(separated, true);
        if (!ValidateFlange(separated[0]))
            throw new DstvParseException("Illegal flange code signature in BO data line");

        if (separated.Length < 5) throw new DstvParseException("Illegal data vector format (BO): too short");

        var xCoord = double.Parse(separated[1], Constants.ParserCultureInfo);
        var yCoord = double.Parse(separated[2], Constants.ParserCultureInfo);
        var diam = double.Parse(separated[3], Constants.ParserCultureInfo);
        var depth = double.Parse(separated[4], Constants.ParserCultureInfo);
        if (separated.Length == 5) return new DstvHole(separated[0], xCoord, yCoord, diam, depth);

        if (separated.Length == 8)
        {
            var slotLen = double.Parse(separated[5], Constants.ParserCultureInfo);
            var slotWidth = double.Parse(separated[6], Constants.ParserCultureInfo);
            var slotAng = double.Parse(separated[7], Constants.ParserCultureInfo);
            return new DstvSlot(separated[0], xCoord, yCoord, diam, depth, slotLen, slotWidth, slotAng);
        }

        throw new DstvParseException("Illegal data vector format (BO): length not equals 5 or 8");
    }

    public override string ToString()
    {
        return $"DStVHole : flCode='{FlCode}', xCoord={XCoord}, yCoord={YCoord}, diam={_diam}, depth={_depth}";
    }
}