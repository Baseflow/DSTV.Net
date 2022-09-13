using System;
using System.Text.RegularExpressions;
using DSTV.Contracts;

namespace DSTV.Implementations;

internal class FineSplitter : ISplitter
{
    internal static readonly ISplitter Instance = new Lazy<FineSplitter>(() => new FineSplitter()).Value;

    /// <summary>
    /// Splitter for full carefully splitting - saving all lexemes
    /// </summary>
    public string[] Split(string input)
    {
        var match = Regex.Match(input, "(?<=\\d)(?=[a-z])|(?<=[a-z])(?=\\d)|(?<=[\\d\\w.]) +");
        return match.Success ? input.Split(match.Value) : new []{input};
    }
}