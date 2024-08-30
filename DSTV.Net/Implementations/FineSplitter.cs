using DSTV.Net.Contracts;

namespace DSTV.Net.Implementations;

internal class FineSplitter : ISplitter
{
    internal static readonly ISplitter Instance = new Lazy<FineSplitter>(() => new FineSplitter()).Value;
    internal static readonly string[] Separator = {" "};

    /// <summary>
    ///     Splitter for full carefully splitting - saving all lexemes
    /// </summary>
    public string[] Split(string input) => input.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
}