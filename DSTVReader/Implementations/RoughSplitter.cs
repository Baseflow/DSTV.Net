using System;
using System.Text.RegularExpressions;
using DSTV.Contracts;

namespace DSTV.Implementations;

internal class RoughSplitter : ISplitter
{
    internal static Lazy<RoughSplitter> Instance = new(() => new RoughSplitter());

    /// <summary>
    ///     Splitter for rough splitting - delete letter-sequence lexemes between two digits (without additional spaces)
    /// </summary>
    public string[] Split(string input)
    {
        var match = Regex.Match(input, "(?<!\\s|\\D)[a-z]+(?!\\s+|\\D)|\\s+");
        return match.Success ? match.Value.Split(match.Value) : new []{input};
    }
}