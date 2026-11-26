using DSTV.Net.Enums;
using DSTV.Net.Svg;

namespace DSTV.Net.Data;

public record DstvSlot : DstvHole
{
    public double SlotAngle { get; }

    //optional, for slots only
    public double SlotLength { get; }

    public double SlotWidth { get; }

    public DstvSlot(
        FaceCode FlCode,
        double XCoord,
        double YCoord,
        double Diameter,
        double Depth,
        double SlotLength,
        double SlotWidth,
        double SlotAngle)
        : base(FlCode, XCoord, YCoord, Diameter, Depth)
    {
        this.SlotLength = SlotLength;
        this.SlotWidth = SlotWidth;
        this.SlotAngle = SlotAngle;
    }

    public override string ToString() => $"{base.ToString()}, SlotLength : {SlotLength}, SlothWidth : {SlotWidth}, SlotAngle : {SlotAngle}";

    internal override string ToSvg(FaceCode faceCode, DstvSvgExporter exporter) =>
        IsOnFace(faceCode)
            ? $"<rect class=\"{exporter.Classes(SvgCssClass.Slot, SvgCssClass.Outline)}\" x=\"{XCoord - (SlotLength + Diameter) / 2:F}\" y=\"{YCoord - Diameter / 2:F}\" width=\"{SlotLength + Diameter:F}\" height=\"{Diameter:F}\" rx=\"{Diameter / 2:F}\" transform=\"rotate({SlotAngle:F} {XCoord:F} {YCoord:F})\" />"
            : string.Empty;
}