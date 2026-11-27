using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using DSTV.Net.Svg;

namespace DSTV.Net.Data;

public record DstvCut : DstvElement
{
    public double OriginX { get; }

    public double OriginY { get; }

    public double OriginZ { get; }

    public double NormalX { get; }

    public double NormalY { get; }

    public double NormalZ { get; }

    public DstvCut(double OriginX, double OriginY, double OriginZ, double NormalX, double NormalY, double NormalZ)
    {
        Type = ContourType.SC;
        this.OriginX = OriginX;
        this.OriginY = OriginY;
        this.OriginZ = OriginZ;
        this.NormalX = NormalX;
        this.NormalY = NormalY;
        this.NormalZ = NormalZ;
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

    internal override string ToSvg(FaceCode faceCode, DstvSvgExporter exporter)
    {
        if (faceCode != FaceCode.V) return string.Empty;

        const double indicatorLength = 20;
        var scale = Math.Max(Math.Abs(NormalX), Math.Abs(NormalY));
        if (scale == 0) return string.Empty;

        var endX = OriginX + NormalX / scale * indicatorLength;
        var endY = OriginY + NormalY / scale * indicatorLength;
        return $"<line class=\"{exporter.Classes(SvgCssClass.Cut, SvgCssClass.Outline)}\" x1=\"{OriginX:F}\" y1=\"{OriginY:F}\" x2=\"{endX:F}\" y2=\"{endY:F}\" data-normal-z=\"{NormalZ:F}\" />";
    }
}