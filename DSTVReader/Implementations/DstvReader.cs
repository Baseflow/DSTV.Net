using System;
using System.IO;
using System.Threading.Tasks;
using DSTV.Contracts;
using DSTV.Data;
using DSTV.Exceptions;

namespace DSTV.Implementations
{
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
            var result = new DstvRecord();
            HeaderReader headerReader = new();
            BodyReader bodyReader = new();

            result.Header = await headerReader.ParseAsync(context).ConfigureAwait(false);

            // Body reading is not yet implemented.
            await BodyReader.ParseAsync(context).ConfigureAwait(false);

            return result;
        }
    }
}