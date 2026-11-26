using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using System.Globalization;

namespace DSTV.Net.Extensions;

/// <summary>
///     Extensions for <seealso cref="TextReader" /> that helps us in parsing the DSTV files
/// </summary>
internal static class TextReaderExtensions
{
    /// <summary>
    ///     Parses the freeText inside a DSTV header field.
    /// </summary>
    /// <param name="reader">An instance of a <see cref="TextReader" /> containing the DSTV source data</param>
    /// <param name="context">The active reader context</param>
    /// <param name="cancellationToken">A token that can cancel reading</param>
    /// <returns>The string instance of the read freeText or an exception if there is too much characters read</returns>
    /// <exception cref="FreeTextTooLargeException"></exception>
    internal static async Task<string> ParseFreeText(this TextReader reader, ReaderContext context,
        CancellationToken cancellationToken)
    {
        var result = (await reader.ReadLineWithCancellationAsync(cancellationToken).ConfigureAwait(false))?.Trim();

        // Only allow 80 characters or less (according to specification).

        if (result?.Length > Constants.FreeTextMaximumSize) throw new FreeTextTooLargeException(context);

        context.IncrementLineNumber();

        return result ?? string.Empty;
    }

    /// <summary>
    ///     Parses an optional freeText header field without consuming a following body section marker.
    /// </summary>
    internal static async Task<string> ParseOptionalFreeText(this TextReader reader, ReaderContext context,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (context.HasBufferedLine) return string.Empty;

        var line = await reader.ReadLineWithCancellationAsync(cancellationToken).ConfigureAwait(false);
        if (line is not null && BodyReader.IsCodeLine(line))
        {
            context.BufferLine(line);
            return string.Empty;
        }

        var result = line?.Trim();
        if (result?.Length > Constants.FreeTextMaximumSize) throw new FreeTextTooLargeException(context);

        context.IncrementLineNumber();
        return result ?? string.Empty;
    }

    /// <summary>
    ///     Parses a <see cref="double" />.
    /// </summary>
    /// <param name="reader">An instance of a <see cref="TextReader" /> containing the DSTV source data</param>
    /// <param name="context">The active reader context</param>
    /// <param name="cancellationToken">A token that can cancel reading</param>
    /// <returns>The double read or an exception</returns>
    /// <exception cref="DoubleParseException"></exception>
    internal static async Task<double> ParseDouble(this TextReader reader, ReaderContext context,
        CancellationToken cancellationToken)
    {
        var text = await reader.ReadLineWithCancellationAsync(cancellationToken).ConfigureAwait(false);
        if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
            throw new DoubleParseException(context);

        context.IncrementLineNumber();

        return value;
    }

    /// <summary>
    ///     Parses a tuple of doubles where the second item in the tuple is optional
    /// </summary>
    /// <param name="reader">An instance of a <see cref="TextReader" /> containing the DSTV source data</param>
    /// <param name="context">The active reader context</param>
    /// <param name="cancellationToken">A token that can cancel reading</param>
    /// <returns></returns>
    /// <exception cref="TupleParseException{TDouble}"></exception>
    /// <exception cref="DoubleParseException"></exception>
    internal static async Task<Tuple<double, double?>> ParseTupleDouble(this TextReader reader,
        ReaderContext context, CancellationToken cancellationToken)
    {
        var text = await reader.ReadLineWithCancellationAsync(cancellationToken).ConfigureAwait(false);
        var split = text?.Split(',');

        if (split == null || split.Length < 1 || split.Length > 2)
            throw new TupleParseException<double>(context, "1 to 2");

        if (!double.TryParse(split[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var value1))
            throw new DoubleParseException(context);

        context.IncrementLineNumber();

        if (split.Length == 2 &&
            double.TryParse(split[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var value2))
            return new Tuple<double, double?>(value1, value2);

        return new Tuple<double, double?>(value1, null);
    }

    /// <summary>
    ///     Parses a <see cref="int" />.
    /// </summary>
    /// <param name="reader">An instance of a <see cref="TextReader" /> containing the DSTV source data</param>
    /// <param name="context">The active reader context</param>
    /// <param name="cancellationToken">A token that can cancel reading</param>
    /// <returns>The integer read or an exception</returns>
    /// <exception cref="IntegerParseException"></exception>
    internal static async Task<int> ParseInteger(this TextReader reader, ReaderContext context,
        CancellationToken cancellationToken)
    {
        var text = await reader.ReadLineWithCancellationAsync(cancellationToken).ConfigureAwait(false);
        if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)) throw new IntegerParseException(context);

        context.IncrementLineNumber();

        return value;
    }

    /// <summary>
    ///     Parses an enum of the given generic type.
    /// </summary>
    /// <typeparam name="TEnum">The type of the Enum which needs to be parsed</typeparam>
    /// <param name="reader">An instance of a <see cref="TextReader" /> containing the DSTV source data</param>
    /// <param name="context">The active reader context</param>
    /// <param name="cancellationToken">A token that can cancel reading</param>
    /// <returns>The integer read or an exception</returns>
    /// <exception cref="EnumParseException{TEnum}"></exception>
    internal static async Task<TEnum> ParseEnum<TEnum>(this TextReader reader, ReaderContext context,
        CancellationToken cancellationToken)
        where TEnum : struct, Enum
    {
        var text = await reader.ReadLineWithCancellationAsync(cancellationToken).ConfigureAwait(false);
        if (!Enum.TryParse(text, out TEnum value)) throw new EnumParseException<TEnum>(context);

        context.IncrementLineNumber();

        return value;
    }

    internal static async Task<string?> ReadLineWithCancellationAsync(this TextReader reader,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
#if NET8_0_OR_GREATER
        return await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
#else
        return await reader.ReadLineAsync().ConfigureAwait(false);
#endif
    }

    internal static async Task<int> ReadWithCancellationAsync(this TextReader reader, char[] buffer, int index,
        int count, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
#if NET8_0_OR_GREATER
        return await reader.ReadAsync(buffer.AsMemory(index, count), cancellationToken).ConfigureAwait(false);
#else
        return await reader.ReadAsync(buffer, index, count).ConfigureAwait(false);
#endif
    }
}
