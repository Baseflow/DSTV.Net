using DSTV.Net.Contracts;
using DSTV.Net.Data;
using DSTV.Net.Exceptions;

namespace DSTV.Net.Implementations;

public sealed class DstvReader : IDstvReader
{
    public Task<IDstv> ParseAsync(string dstvData) => ParseAsync(dstvData, CancellationToken.None);

    public async Task<IDstv> ParseAsync(string dstvData, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var stringReader = new StringReader(dstvData);

        return await ParseAsync(stringReader, cancellationToken).ConfigureAwait(false);
    }

    public Task<IDstv> ParseAsync(TextReader reader) => ParseAsync(reader, CancellationToken.None);

    public async Task<IDstv> ParseAsync(TextReader reader, CancellationToken cancellationToken)
    {
#if NET
        ArgumentNullException.ThrowIfNull(reader);
#else
        if (reader == null) throw new ArgumentNullException(nameof(reader));
#endif

        var context = new ReaderContext(reader);

        // First check if we encounter the right start of the file
        var startOfFile = await context.ReadLineAsync(cancellationToken).ConfigureAwait(false);
        if (!string.Equals(startOfFile, Constants.StartOfFile, StringComparison.Ordinal))
            throw new MissingStartOfFileException(context);
        context.IncrementLineNumber();

        // Then read the header information
        return new DstvRecord
        {
            Header = await HeaderReader.ParseAsync(context, cancellationToken).ConfigureAwait(false),
            Elements = await BodyReader.GetElementsAsync(context, cancellationToken).ConfigureAwait(false),
        };
    }
}