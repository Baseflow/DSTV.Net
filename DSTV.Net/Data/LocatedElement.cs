using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace DSTV.Net.Data;

[SuppressMessage("Designer", "CA1051:Do not declare visible instance fields", Justification = "This is a DTO")]
public abstract partial record LocatedElement(FaceCode FlCode, double XCoord, double YCoord) : DstvElement
{
    private const string NumberPattern = @"[+-]?(?:\d+(?:\.\d*)?|\.\d+)";

#if NET7_0_OR_GREATER
    [GeneratedRegex(
        @"^\s*(?<face>[vouh])\s*(?<x>" + NumberPattern + @")\s*(?:[osu])?\s*(?<y>" +
        NumberPattern + @")(?<remaining>.*)$",
        RegexOptions.IgnoreCase,
        1000)]
    private static partial Regex LocationPattern();
#else
    private static readonly Regex LocationRegex = new(
        @"^\s*(?<face>[vouh])\s*(?<x>" + NumberPattern + @")\s*(?:[osu])?\s*(?<y>" +
        NumberPattern + @")(?<remaining>.*)$",
        RegexOptions.IgnoreCase,
        TimeSpan.FromSeconds(1));

    private static Regex LocationPattern() => LocationRegex;
#endif

    public FaceCode FlCode { get; set; } = FlCode;

    protected bool IsOnFace(FaceCode faceCode) => FlCode == faceCode;

    internal static (FaceCode FaceCode, double XCoord, double YCoord, string Remaining) ParseLocation(
        string dstvLine)
    {
#if NET
        ArgumentNullException.ThrowIfNull(dstvLine);
#else
        if (dstvLine is null) throw new ArgumentNullException(nameof(dstvLine));
#endif
        var match = LocationPattern().Match(dstvLine);
        if (!match.Success || !TryParseFaceCode(match.Groups["face"].Value, out var faceCode))
            throw new DstvParseException("Illegal location format in data line");

        var xCoord = double.Parse(match.Groups["x"].Value, Constants.ParserCultureInfo);
        var yCoord = double.Parse(match.Groups["y"].Value, Constants.ParserCultureInfo);
        return (faceCode, xCoord, yCoord, match.Groups["remaining"].Value);
    }

    public virtual bool Equals(LocatedElement? other)
    {
        if (other is null) return false;

        if (Math.Abs(YCoord - other.XCoord) <= 0) return false;

        if (Math.Abs(YCoord - other.YCoord) <= 0) return false;

        return FlCode == other.FlCode;
    }

    public override int GetHashCode() => HashCode.Combine(FlCode, XCoord, YCoord);
}