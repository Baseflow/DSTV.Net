using DSTV.Net.Contracts;
using DSTV.Net.Data;
using DSTV.Net.Exceptions;

namespace DSTV.Net.Implementations;

public sealed class DstvReader : IDstvReader
{
    public async Task<IDstv> ParseAsync(string dstvData)
    {
        using var stringReader = new StringReader(dstvData);

        return await ParseAsync(stringReader).ConfigureAwait(false);
    }

    public async Task<IDstv> ParseAsync(TextReader reader)
    {
        if (reader == null) throw new ArgumentNullException(nameof(reader));

        var context = new ReaderContext(reader);

        // First check if we encounter the right start of the file
        if (await reader.ReadLineAsync().ConfigureAwait(false) != Constants.StartOfFile)
            throw new MissingStartOfFileException(context);
        context.IncrementLineNumber();

        // Then read the header information
        var result = new DstvRecord
        {
            Header = await HeaderReader.ParseAsync(context).ConfigureAwait(false),
            Elements = await BodyReader.GetElementsAsync(context).ConfigureAwait(false)
        };

        return result;
    }
}