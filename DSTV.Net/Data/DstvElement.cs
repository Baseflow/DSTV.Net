using DSTV.Net.Contracts;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using System.Text.RegularExpressions;

namespace DSTV.Net.Data;

public record DstvElement
{
    protected static string[] GetDataVector(string dstvElementLine, ISplitter splitter)
    {
        if (splitter is null) throw new ArgumentNullException(nameof(splitter));
        if (Regex.IsMatch(dstvElementLine, "^\\*\\*.*", RegexOptions.None, TimeSpan.FromSeconds(1)))
            throw new DstvParseException("Attempt to get data from quote-line detected");

        if (!Regex.IsMatch(dstvElementLine, "^\\s+.*", RegexOptions.None, TimeSpan.FromSeconds(1)))
            throw new DstvParseException("Illegal start sequence in data line (must starts with \\\"  \\\")");

        // DStVSign = DStVSign.trim();
        return splitter.Split(dstvElementLine);
    }

    protected static bool ValidateFlange(string flDependMark) => Regex.IsMatch(flDependMark, "[ovuh]", RegexOptions.None, TimeSpan.FromSeconds(1));

    protected static string[] CorrectSplits(string[] separated, bool skipFirst = false, bool skipLast = false)
    {
        if (separated is null) throw new ArgumentNullException(nameof(separated));
        for (var i = skipFirst ? 1 : 0; i < separated.Length - (skipLast ? 1 : 0); i++)
        {
            foreach (var match in Regex.Matches(separated[i], "([^.\\d-]+)", RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(1)).AsEnumerable())
                separated[i] = separated[i].Replace(match.Value, string.Empty, StringComparison.Ordinal);
        }

        return BodyReader.RemoveVoids(separated);
    }

    public virtual string ToSvg() => string.Empty;
}