using System.ComponentModel;

namespace DSTV.Enums;

public enum CodeProfile
{
    [Description("Profile I")]
    I,

    [Description("Profile L")]
    L,

    [Description("Profile U")]
    U,

    [Description("Sheets, Plate, teared sheets, etc.")]
    B,

    [Description("Round")]
    RU,

    [Description("Rounded Tube")]
    RO,

    [Description("Rectangular Tube")]
    M,

    [Description("Profile C")]
    C,

    [Description("Profile T")]
    T,

    [Description("Special Profile")]
    SO
}