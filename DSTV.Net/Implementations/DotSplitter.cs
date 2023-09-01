using DSTV.Net.Contracts;

namespace DSTV.Net.Implementations;

internal class DotSplitter : ISplitter
{
    internal static Lazy<DotSplitter> Instance = new(() => new DotSplitter());

    public string[] Split(string input)
    {
        const int intDigits = 4;
        const int n = 3;

        var dotPoss = new int[n];
        for (var i = 0; i < n; i++)
        {
            var prevPos = 0;
            if (i > 0) prevPos = dotPoss[i - 1];
            dotPoss[i] = input.IndexOf(".", prevPos + 1, StringComparison.Ordinal);
        }

        var outArr = new string[n];
        for (var i = 0; i < n; i++) outArr[i] = input.Substring(dotPoss[i] - intDigits, dotPoss[i] + 2);

        return outArr;
    }
}