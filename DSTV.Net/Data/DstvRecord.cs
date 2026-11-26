using DSTV.Net.Contracts;
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

    /// <summary>
    ///     All the (basic) header information.
    /// </summary>
    public DstvHeader? Header { get; init; }

    IDstvHeader? IDstv.Header => Header;
}