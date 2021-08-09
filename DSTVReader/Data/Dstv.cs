using DSTV.Contracts;

namespace DSTV.Data
{
    /// <summary>
    ///     The DSTV structure contents.
    /// </summary>
    public record DstvRecord : IDstv
    {
        /// <summary>
        ///     All the (basic) header information.
        /// </summary>
        public IDstvHeader? Header { get; set; }
    }
}