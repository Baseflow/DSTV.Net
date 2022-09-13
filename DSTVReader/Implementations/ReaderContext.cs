using System.IO;
using System.Threading;

namespace DSTV.Implementations;

/// <summary>
///     The context of the reader, used for retrieving the <see cref="LineNumber" />
/// </summary>
public sealed class ReaderContext
{
    /// <summary>
    ///     The LineNumber the parser is currently reading
    /// </summary>
    private int _lineNumber;

    internal ReaderContext(TextReader source)
    {
        Source = source;
        _lineNumber = 1;
    }

    /// <summary>
    ///     The LineNumber the parser is currently reading
    /// </summary>
    public int LineNumber => _lineNumber;

    /// <summary>
    ///     The underlying TextReader for reading the data structure
    /// </summary>
    internal TextReader Source { get; init; }

    /// <summary>
    ///     Increments the <see cref="LineNumber" />
    /// </summary>
    internal void IncrementLineNumber()
    {
        Interlocked.Increment(ref _lineNumber);
    }
}