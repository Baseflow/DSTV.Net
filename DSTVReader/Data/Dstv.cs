using System.Collections.Generic;
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

    /// <summary>
    ///     All the (basic) header information.
    /// </summary>
    public IDstvHeader? Header { get; set; }
}