using DSTV.Net.Enums;

namespace DSTV.Net.Data;

public record DstvSkewedPoint(
    FaceCode FlCode,
    double XCoord, 
    double YCoord,
    bool IsNotch,
    double Radius,
    double FirstAngle, 
    double FirstBlunting,
    double SecondAngle, 
    double SecondBlunting) 
    : DstvContourPoint(FlCode, XCoord, YCoord, IsNotch, Radius)
{
    public override string ToString() =>
        $"DStVSkewedPoint{{ang1={FirstAngle}, blunting1={FirstBlunting}, ang2={SecondAngle}, blunting2={SecondBlunting}, radius={Radius}, faceCode={FlCode}, xCoord={XCoord}, yCoord={YCoord}}}";
}