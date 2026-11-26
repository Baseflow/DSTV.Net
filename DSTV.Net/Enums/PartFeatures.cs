namespace DSTV.Net.Enums;

/// <summary>
/// Describes fabrication features present in a parsed DSTV part.
/// </summary>
[Flags]
public enum PartFeatures
{
    None = 0,
    RegularBoreholes = 1 << 0,
    SlottedBoreholes = 1 << 1,
    PowderMarkings = 1 << 2,
    PunchMarkings = 1 << 3,
    Numerations = 1 << 4,
    Bends = 1 << 5,
    SawCuts = 1 << 6,
    SkewedSawCuts = 1 << 7,
    RegularCuts = 1 << 8,
    SkewedRegularCuts = 1 << 9,
    InternalContours = 1 << 10,
    CurvedRegularCuts = 1 << 11,
}
