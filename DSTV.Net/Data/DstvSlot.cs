namespace DSTV.Net.Data;

public record DstvSlot : DstvHole
{
    public double SlotAngle { get; }

    //optional, for slots only
    public double SlotLength { get; }

    public double SlotWidth { get; }

    public DstvSlot(
        string flCode, 
        double xCoord, 
        double yCoord, 
        double diam, 
        double depth, 
        double slotLen,
        double slotWidth, 
        double slotAng) 
        : base(flCode, xCoord, yCoord, diam, depth)
    {
        SlotLength = slotLen;
        SlotWidth = slotWidth;
        SlotAngle = slotAng;
    }

    public override string ToString() => $"{base.ToString()}, SlotLength : {SlotLength}, SlothWidth : {SlotWidth}, SlotAngle : {SlotAngle}";

    public override string ToSvg() => $"<rect x=\"{XCoord}\" y=\"{YCoord}\" width=\"{SlotLength + Diameter}\" height=\"{Diameter}\" fill=\"white\" rx=\"{Diameter / 2}\" />";
}