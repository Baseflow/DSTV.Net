namespace DSTV.Data;

public record DstvSkewedPoint : DstvContourPoint
{
    private readonly double _ang1;

    private readonly double _ang2;

    private readonly double _blunting1;

    private readonly double _blunting2;

    public DstvSkewedPoint(string flCode, double xCoord, double yCoord, double radius, double ang1, double blunting1,
        double ang2, double blunting2) : base(flCode, xCoord, yCoord, radius)
    {
        _ang1 = ang1;
        _blunting1 = blunting1;
        _ang2 = ang2;
        _blunting2 = blunting2;
    }


    public override string ToString()
    {
        return
            $"DStVSkewedPoint{{ang1={_ang1}, blunting1={_blunting1}, ang2={_ang2}, blunting2={_blunting2}, radius={Radius}, flCode='{FlCode}', xCoord={XCoord}, yCoord={YCoord}}}";
    }
}