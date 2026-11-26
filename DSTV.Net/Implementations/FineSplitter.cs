using DSTV.Net.Contracts;

namespace DSTV.Net.Implementations;

internal class FineSplitter : ISplitter
{
    internal static readonly ISplitter Instance = new Lazy<FineSplitter>(static () => new()).Value;

    /// <summary>
    ///     Splitter for full carefully splitting - saving all lexemes
    /// </summary>
    public string[] Split(string input) => input.Split(SplitSeparators, StringSplitOptions.RemoveEmptyEntries);

    private static readonly char[] SplitSeparators = [' '];

}