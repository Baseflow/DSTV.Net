using System.Threading.Tasks;
using DSTV.Exceptions;

namespace DSTV.Implementations
{
    internal sealed class BodyReader
    {
        /// <summary>
        ///     Currently the body is not read.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static async Task ParseAsync(ReaderContext context)
        {
            // encounter the "EN"-mark.
            var line = string.Empty;
            while (line != null && line.Trim() != Constants.EndOfFile)
                line = await context.Source.ReadLineAsync().ConfigureAwait(false);

            if (line?.Trim() != Constants.EndOfFile) throw new UnexpectedEndException(context);
        }
    }
}