using DSTV.Net.Extensions;

namespace DSTV.Net.Implementations;

/// <summary>
///     The context of the reader, used for retrieving the <see cref="LineNumber" />
/// </summary>
public sealed class ReaderContext
{
    /// <summary>
    ///     The LineNumber the parser is currently reading
    /// </summary>
    private int _lineNumber;
    private string? _bufferedLine;

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

    internal bool HasBufferedLine => _bufferedLine is not null;

    internal void BufferLine(string line) => _bufferedLine = line;

    internal async Task<string?> ReadLineAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (_bufferedLine is null)
            return await Source.ReadLineWithCancellationAsync(cancellationToken).ConfigureAwait(false);

        var line = _bufferedLine;
        _bufferedLine = null;
        return line;
    }

    /// <summary>
    ///     Increments the <see cref="LineNumber" />
    /// </summary>
    internal void IncrementLineNumber()
    {
        Interlocked.Increment(ref _lineNumber);
    }
}