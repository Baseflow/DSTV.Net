using System;
using System.Text.RegularExpressions;
using DSTV.Contracts;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;

namespace DSTV.Net.Data;

public record DstvElement
{
    protected static string[] GetDataVector(string dstvElementLine, ISplitter splitter)
    {
        if (splitter is null) throw new ArgumentNullException(nameof(splitter));
        if (Regex.IsMatch(dstvElementLine, "^\\*\\*.*"))
            throw new DstvParseException("Attempt to get data from quote-line detected");

        if (!Regex.IsMatch(dstvElementLine, "^\\s+.*"))
            throw new DstvParseException("Illegal start sequence in data line (must starts with \\\"  \\\")");

        // DStVSign = DStVSign.trim();
        return splitter.Split(dstvElementLine);
    }

    protected static bool ValidateFlange(string flDependMark) => Regex.IsMatch(flDependMark, "[ovuh]");

    protected static string[] CorrectSplits(string[] separated, bool skipFirst = false, bool skipLast = false)
    {
        if (separated is null) throw new ArgumentNullException(nameof(separated));
        for (var i = skipFirst ? 1 : 0; i < separated.Length - (skipLast ? 1 : 0); i++)
        {
            var matches = Regex.Matches(separated[i], "([^.\\d-]+)");
            foreach (Match match in matches)
                separated[i] = separated[i].Replace(match.Value, string.Empty, StringComparison.Ordinal);
        }

        return BodyReader.RemoveVoids(separated);
    }

    public virtual string ToSvg() => string.Empty;
}