using System.Collections.Generic;
using DSTV.Net.Data;

namespace DSTV.Contracts;

/// <summary>
///     Interface describing the model of the parsed DSTV structure
/// </summary>
public interface IDstv
{
    /// <summary>
    ///     The header of the DSTV structure
    /// </summary>
    IDstvHeader? Header { get; }

    /// <summary>
    ///     The list of all the DSTV entries
    /// </summary>
    IEnumerable<DstvElement> Elements { get; }

    /// <summary>
    ///     Renders the DSTV structure to svg markup.
    /// </summary>
    /// <returns></returns>
    string ToSvg();
}