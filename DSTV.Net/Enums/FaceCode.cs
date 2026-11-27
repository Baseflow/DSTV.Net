namespace DSTV.Net.Enums;

/// <summary>
/// Identifies the profile face on which an element is located.
/// </summary>
public enum FaceCode
{
    /// <summary>The source line does not specify a face.</summary>
    None,

    /// <summary>The web/front face (v).</summary>
    V,

    /// <summary>The o flange face.</summary>
    O,

    /// <summary>The u flange face.</summary>
    U,

    /// <summary>The rear/second web face (h).</summary>
    H,
}
