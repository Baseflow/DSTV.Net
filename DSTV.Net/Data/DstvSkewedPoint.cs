namespace DSTV.Net.Data;

public record DstvSkewedPoint(
    string FlCode, 
    double XCoord, 
    double YCoord, 
    double Radius, 
    double FirstAngle, 
    double FirstBlunting,
    double SecondAngle, 
    double SecondBlunting) 
    : DstvContourPoint(FlCode, XCoord, YCoord, Radius)
{
    public override string ToString() =>
        $"DStVSkewedPoint{{ang1={FirstAngle}, blunting1={FirstBlunting}, ang2={SecondAngle}, blunting2={SecondBlunting}, radius={Radius}, flCode='{FlCode}', xCoord={XCoord}, yCoord={YCoord}}}";
}