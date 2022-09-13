using System;
using System.Diagnostics.CodeAnalysis;

namespace DSTV.Data;

[SuppressMessage("Designer", "CA1051:Do not declare visible instance fields", Justification = "This is a DTO")]
public abstract record LocatedElem : DstvElement {
    public string FlCode { get; set; }

    protected readonly double XCoord;

    protected readonly double YCoord;

    protected LocatedElem(string flCode, double xCoord, double yCoord) {
        FlCode = flCode;
        XCoord = xCoord;
        YCoord = yCoord;
    }
    
    public virtual bool Equals(LocatedElem? other) {
        if (other is null) {
            return false;
        }

        if (Math.Abs(YCoord - other.XCoord) <= 0) {
            return false;
        }
        
        if (Math.Abs(YCoord - other.YCoord) <= 0) {
            return false;
        }
        
        return FlCode.Equals(other.FlCode, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FlCode, XCoord, YCoord);
    }
}