using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;

namespace DSTV.Net.Data;

public record DstvBend : DstvElement
{
    private readonly double _bendingAngle;

    private readonly double _bendingRadius;

    private readonly double _finishX;

    private readonly double _finishY;
    private readonly double _originX;

    private readonly double _originY;

    public DstvBend(double originX, double originY, double finishX, double finishY, double bendingAngle,
        double bendingRadius)
    {
        _originX = originX;
        _originY = originY;
        _finishX = finishX;
        _finishY = finishY;
        _bendingAngle = bendingAngle;
        _bendingRadius = bendingRadius;
    }

    public static DstvBend CreateBend(string bendDataLine)
    {
        var separated = GetDataVector(bendDataLine, FineSplitter.Instance);
        separated = CorrectSplits(separated);
        if (separated.Length < 6) throw new DstvParseException("Illegal data vector format (KA): too short");

        var orX = double.Parse(separated[0], Constants.ParserCultureInfo);
        var orY = double.Parse(separated[1], Constants.ParserCultureInfo);
        var finX = double.Parse(separated[2], Constants.ParserCultureInfo);
        var finY = double.Parse(separated[3], Constants.ParserCultureInfo);
        var ang = double.Parse(separated[4], Constants.ParserCultureInfo);
        var rad = double.Parse(separated[5], Constants.ParserCultureInfo);
        return new DstvBend(orX, orY, finX, finY, ang, rad);
    }


    public override string ToString() =>
        $"DstvBend{{originX={_originX}, originY={_originY}, finishX={_finishX} , finishY={_finishY}, bendingAngle={_bendingAngle}, bendingRadius={_bendingRadius}'}}";
}