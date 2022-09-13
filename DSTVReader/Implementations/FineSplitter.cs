using System;
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
        return input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    }
}