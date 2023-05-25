using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;

namespace DSTV.Net.Data;

public record DstvCut : DstvElement
{
    private readonly double _normVecX;
    private readonly double _normVecY;
    private readonly double _normVecZ;
    private readonly double _spPointX;
    private readonly double _spPointY;
    private readonly double _spPointZ;

    public DstvCut(double spPointX, double spPointY, double spPointZ, double normVecX, double normVecY, double normVecZ)
    {
        _spPointX = spPointX;
        _spPointY = spPointY;
        _spPointZ = spPointZ;
        _normVecX = normVecX;
        _normVecY = normVecY;
        _normVecZ = normVecZ;
    }

    public static DstvCut CreateCut(string dataLine)
    {
        var separated = GetDataVector(dataLine, FineSplitter.Instance);

        separated = CorrectSplits(separated);

        if (separated.Length < 6) throw new DstvParseException("Illegal data vector format (SC): too short");

        var ptX = double.Parse(separated[0], Constants.ParserCultureInfo);
        var ptY = double.Parse(separated[1], Constants.ParserCultureInfo);
        var ptZ = double.Parse(separated[2], Constants.ParserCultureInfo);
        var vecX = double.Parse(separated[3], Constants.ParserCultureInfo);
        var vecY = double.Parse(separated[4], Constants.ParserCultureInfo);
        var vecZ = double.Parse(separated[5], Constants.ParserCultureInfo);
        return new DstvCut(ptX, ptY, ptZ, vecX, vecY, vecZ);
    }

    public override string ToString() =>
        $"DstvCut: spPointX={_spPointX}, spPointY={_spPointY}, spPointZ={_spPointZ}, normVecX={_normVecX}, normVecY={_normVecY}, normVecZ={_normVecZ}";
}