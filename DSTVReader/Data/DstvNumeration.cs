using DSTV.Exceptions;
using DSTV.Implementations;

namespace DSTV.Data;

public record DstvNumeration : LocatedElem
{
    private readonly double _angle;
    private readonly double _letterHeight;
    private readonly string _text;

    private DstvNumeration(string flCode, double xCoord, double yCoord, double angle, double letterHeight, string text) :
        base(flCode, xCoord, yCoord)
    {
        _angle = angle;
        _letterHeight = letterHeight;
        _text = text;
    }


    public static DstvNumeration CreateNumeration(string dstvLine)
    {
        var separated = GetDataVector(dstvLine, FineSplitter.Instance);
        // temporary flange-code in case of missing a signature in line
        var flCode = "x";
        if (ValidateFlange(separated[1]))
        {
            flCode = separated[1];
        }

        separated = CorrectSplits(separated, skipLast:true);

        if (separated.Length < 5)
        {
            throw new DstvParseException("Illegal data-vector length (SI) - too short");
        }

        var xCoord = double.Parse(separated[0], Constants.ParserCultureInfo);
        var yCoord = double.Parse(separated[1], Constants.ParserCultureInfo);
        var angle = double.Parse(separated[2], Constants.ParserCultureInfo);
        var lh = double.Parse(separated[3], Constants.ParserCultureInfo);
        return new DstvNumeration(flCode, xCoord, yCoord, angle, lh, separated[4]);
    }

    public override string ToString()
    {
        return $"Numeration: FlCode={FlCode}, XCoord={XCoord}, YCoord={YCoord}, Angle={_angle}, LetterHeight {_letterHeight}, Text : {_text}";
    }
}