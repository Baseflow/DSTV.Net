using DSTV.Net.Contracts;
using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using System.Text.RegularExpressions;

namespace DSTV.Net.Data;

public record DstvElement
{
    /// <summary>
    /// Gets the DSTV section type that contains the element.
    /// </summary>
    public ContourType Type { get; protected init; }

    internal static string[] GetDataVector(string dstvElementLine, ISplitter splitter)
    {
#if NET
        ArgumentNullException.ThrowIfNull(splitter);
#else
        if (splitter is null) throw new ArgumentNullException(nameof(splitter));
#endif
        if (Regex.IsMatch(dstvElementLine, "^\\*\\*.*", RegexOptions.None, TimeSpan.FromSeconds(1)))
            throw new DstvParseException("Attempt to get data from quote-line detected");

        if (!Regex.IsMatch(dstvElementLine, "^\\s+.*", RegexOptions.None, TimeSpan.FromSeconds(1)))
            throw new DstvParseException("Illegal start sequence in data line (must starts with \\\"  \\\")");

        // DStVSign = DStVSign.trim();
        return splitter.Split(dstvElementLine);
    }

    internal static bool TryParseFaceCode(string value, out FaceCode faceCode)
    {
#if NET
        ArgumentNullException.ThrowIfNull(value);
#else
        if (value is null) throw new ArgumentNullException(nameof(value));
#endif
        faceCode = FaceCode.None;
        if (value.Length != 1) return false;

        faceCode = char.ToUpperInvariant(value[0]) switch
        {
            'V' => FaceCode.V,
            'O' => FaceCode.O,
            'U' => FaceCode.U,
            'H' => FaceCode.H,
            _ => FaceCode.None,
        };

        return faceCode != FaceCode.None;
    }

    internal static string[] CorrectSplits(string[] separated, bool skipFirst = false, bool skipLast = false)
    {
#if NET
        ArgumentNullException.ThrowIfNull(separated);
#else
        if (separated is null) throw new ArgumentNullException(nameof(separated));
#endif
        for (var i = skipFirst ? 1 : 0; i < separated.Length - (skipLast ? 1 : 0); i++)
        {
            foreach (Match match in Regex.Matches(separated[i], "([^.\\d-]+)", RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(1)))
                separated[i] = separated[i].Replace(match.Value, string.Empty, StringComparison.Ordinal);
        }

        return BodyReader.RemoveVoids(separated);
    }

    public virtual string ToSvg() => string.Empty;
}