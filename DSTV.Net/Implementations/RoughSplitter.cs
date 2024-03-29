using DSTV.Net.Contracts;
using System.Text.RegularExpressions;

namespace DSTV.Net.Implementations;

internal class RoughSplitter : ISplitter
{
    internal static Lazy<RoughSplitter> Instance = new(() => new RoughSplitter());

    /// <summary>
    ///     Splitter for rough splitting - delete letter-sequence lexemes between two digits (without additional spaces)
    /// </summary>
    public string[] Split(string input)
    {
        var match = Regex.Match(input, @"(?<!\s|\D)[a-z]+(?!\s+|\D)|\s+", RegexOptions.None, TimeSpan.FromSeconds(1));
        return match.Success ? match.Value.Split(new [] { match.Value }, StringSplitOptions.None) : new[] { input };
    }
}