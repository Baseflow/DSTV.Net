using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;

namespace DSTV.Net.Data;

public record DstvCut : DstvElement
{
    public double OriginX { get; }

    public double OriginY { get; }

    public double OriginZ { get; }

    public double NormalX { get; }

    public double NormalY { get; }

    public double NormalZ { get; }

    private DstvCut(double spPointX, double spPointY, double spPointZ, double normVecX, double normVecY, double normVecZ)
    {
        Type = ContourType.SC;
        OriginX = spPointX;
        OriginY = spPointY;
        OriginZ = spPointZ;
        NormalX = normVecX;
        NormalY = normVecY;
        NormalZ = normVecZ;
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
        $"DstvCut: spPointX={OriginX}, spPointY={OriginY}, spPointZ={OriginZ}, normVecX={NormalX}, normVecY={NormalY}, normVecZ={NormalZ}";
}