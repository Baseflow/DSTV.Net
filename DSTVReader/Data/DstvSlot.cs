namespace DSTV.Data;

public record DstvSlot : DstvHole
{
    //optional, for slots only
    private readonly double _slotLen;
    private readonly double _slotWidth;
    private readonly double _slotAng;

    public DstvSlot(string flCode, double xCoord, double yCoord, double diam, double depth, double slotLen, double slotWidth, double slotAng) : base(flCode, xCoord, yCoord, diam, depth)
    {
        _slotLen = slotLen;
        _slotWidth = slotWidth;
        _slotAng = slotAng;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, SlotLength : {_slotLen}, SlothWidth : {_slotWidth}, SlotAngle : {_slotAng}";
    }
}