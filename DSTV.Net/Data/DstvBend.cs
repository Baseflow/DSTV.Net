using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using DSTV.Net.Svg;

namespace DSTV.Net.Data;

public record DstvBend : DstvElement
{
    public double OriginX { get; }

    public double OriginY { get; }

    public double FinishX { get; }

    public double FinishY { get; }

    public double BendingAngle { get; }

    public double BendingRadius { get; }

    public DstvBend(double OriginX, double OriginY, double FinishX, double FinishY, double BendingAngle,
        double BendingRadius)
    {
        Type = ContourType.KA;
        this.OriginX = OriginX;
        this.OriginY = OriginY;
        this.FinishX = FinishX;
        this.FinishY = FinishY;
        this.BendingAngle = BendingAngle;
        this.BendingRadius = BendingRadius;
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
        $"DstvBend{{originX={OriginX}, originY={OriginY}, finishX={FinishX} , finishY={FinishY}, bendingAngle={BendingAngle}, bendingRadius={BendingRadius}'}}";

    internal override string ToSvg(FaceCode faceCode, DstvSvgExporter exporter) =>
        faceCode == FaceCode.V
            ? $"<line class=\"{exporter.Classes(SvgCssClass.Bend, SvgCssClass.Outline)}\" x1=\"{OriginX:F}\" y1=\"{OriginY:F}\" x2=\"{FinishX:F}\" y2=\"{FinishY:F}\" data-angle=\"{BendingAngle:F}\" data-radius=\"{BendingRadius:F}\" />"
            : string.Empty;
}