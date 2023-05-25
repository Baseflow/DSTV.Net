namespace DSTV.Net.Data;

public record DstvSlot : DstvHole
{
    private readonly double _slotAng;

    //optional, for slots only
    private readonly double _slotLen;
    private readonly double _slotWidth;

    public DstvSlot(string flCode, double xCoord, double yCoord, double diam, double depth, double slotLen,
        double slotWidth, double slotAng) : base(flCode, xCoord, yCoord, diam, depth)
    {
        _slotLen = slotLen;
        _slotWidth = slotWidth;
        _slotAng = slotAng;
    }

    public override string ToString() =>
        $"{base.ToString()}, SlotLength : {_slotLen}, SlothWidth : {_slotWidth}, SlotAngle : {_slotAng}";

    public override string ToSvg() => $"<circle cx=\"{XCoord}\" cy=\"{YCoord}\" r=\"{Diam / 2}\" fill=\"white\" />";
}