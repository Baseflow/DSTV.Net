using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using DSTV.Contracts;

namespace DSTV.Data;

/// <summary>
///     The DSTV structure contents.
/// </summary>
public record DstvRecord : IDstv
{
    /// <summary>
    ///     All the elements in the structure.
    /// </summary>
    public IEnumerable<DstvElement> Elements { get; set; } = new List<DstvElement>();

    public string ToSvg()
    {
        var tmpCulture = Thread.CurrentThread.CurrentCulture;
        try
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            return @$"<svg width=""{Header?.Length}"" height=""{Header?.ProfileHeight}"" xmlns=""http://www.w3.org/2000/svg"">{string.Concat(Elements.OrderByDescending(d => d is Contour).Select(d => d.ToSvg()))}</svg>";
        }
        finally
        {
            Thread.CurrentThread.CurrentCulture = tmpCulture;
        }
    }

    /// <summary>
    ///     All the (basic) header information.
    /// </summary>
    public IDstvHeader? Header { get; set; }
}