using DSTV.Contracts;
using DSTV.Net.Enums;

namespace DSTV.Net.Data;

public record DstvHeader : IDstvHeader
{
    public string? OrderIdentification { get; set; }
    public string? DrawingIdentification { get; set; }
    public string? PhaseIdentification { get; set; }
    public string? PieceIdentification { get; set; }
    public string? SteelQuality { get; set; }
    public int QuantityOfPieces { get; set; }
    public string? Profile { get; set; }
    public CodeProfile CodeProfile { get; set; }
    public double Length { get; set; }
    public double? SawLength { get; set; }
    public double ProfileHeight { get; set; }
    public double FlangeWidth { get; set; }
    public double FlangeThickness { get; set; }
    public double WebThickness { get; set; }
    public double Radius { get; set; }
    public double WeightByMeter { get; set; }
    public double PaintingSurfaceByMeter { get; set; }
    public double WebStartCut { get; set; }
    public double WebEndCut { get; set; }
    public double FlangeStartCut { get; set; }
    public double FlangeEndCut { get; set; }
    public string? Text1InfoOnPiece { get; set; }
    public string? Text2InfoOnPiece { get; set; }
    public string? Text3InfoOnPiece { get; set; }
    public string? Text4InfoOnPiece { get; set; }
}