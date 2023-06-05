using DSTV.Net.Contracts;
using System.Globalization;

namespace DSTV.Net.Data;

/// <summary>
///     The DSTV structure contents.
/// </summary>
public record DstvRecord : IDstv
{
    /// <summary>
    ///     All the elements in the structure.
    /// </summary>
    public IEnumerable<DstvElement> Elements { get; init; } = new List<DstvElement>();

    public string ToSvg()
    {
        var tmpCulture = Thread.CurrentThread.CurrentCulture;
        try
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            return
                @$"<svg viewbox=""0 0 {Header?.Length} {Header?.ProfileHeight}"" width=""{Header?.Length}"" height=""{Header?.ProfileHeight}"" xmlns=""http://www.w3.org/2000/svg"">{string.Concat(Elements.OrderByDescending(d => d is Contour).Select(d => d.ToSvg()))}</svg>";
        }
        finally
        {
            Thread.CurrentThread.CurrentCulture = tmpCulture;
        }
    }

    /// <summary>
    ///     All the (basic) header information.
    /// </summary>
    public IDstvHeader? Header { get; init; }
}