using DSTV.Net.Enums;

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

    public override string ToSvg() => $"<rect x=\"{XCoord}\" y=\"{YCoord}\" width=\"{SlotLength + Diameter}\" height=\"{Diameter}\" fill=\"white\" rx=\"{Diameter / 2}\" />";
}