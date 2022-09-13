using System.Collections.Generic;
using System.Linq;
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
        return $"<svg width={Header?.Length} height={Header?.ProfileHeight}>{string.Concat(Elements.OrderByDescending(d => d is Contour).Select(d => d.ToSvg()))}</svg>";
    }

    /// <summary>
    ///     All the (basic) header information.
    /// </summary>
    public IDstvHeader? Header { get; set; }
}