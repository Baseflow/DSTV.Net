using DSTV.Net.Contracts;

namespace DSTV.Net.Implementations;

internal class PositionNumericSplitter : ISplitter
{
    internal static readonly ISplitter Instance =
        new Lazy<PositionNumericSplitter>(() => new PositionNumericSplitter()).Value;

    public string[] Split(string input)
    {
        return new[]
        {
            input[..2],
            input[2..3],
            input[3..15],
            input[15..25],
            input[25..31],
            input[31..35],
            input[35..36],
            input[36..]
        };
    }
}