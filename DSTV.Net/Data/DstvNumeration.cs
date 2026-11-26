using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;

namespace DSTV.Net.Data;

public record DstvNumeration : LocatedElement
{
    public double Angle { get; }

    public double LetterHeight { get; }

    public string Text { get; }

    private DstvNumeration(FaceCode flCode, double xCoord, double yCoord, double angle, double letterHeight,
        string text) :
        base(flCode, xCoord, yCoord)
    {
        Type = ContourType.SI;
        Angle = angle;
        LetterHeight = letterHeight;
        Text = text;
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
}