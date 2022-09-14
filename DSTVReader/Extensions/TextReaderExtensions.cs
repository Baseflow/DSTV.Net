using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using DSTV.Exceptions;
using DSTV.Implementations;

namespace DSTV.Extensions;

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
    /// <returns>The string instance of the read freeText or an exception if there is too much characters read</returns>
    /// <exception cref="FreeTextTooLargeException"></exception>
    internal static async Task<string> ParseFreeText(this TextReader reader, ReaderContext context)
    {
        var result = (await reader.ReadLineAsync().ConfigureAwait(false))?.Trim();

        // Only allow 80 characters or less (according to specification).

        if (result?.Length > Constants.FreeTextMaximumSize) throw new FreeTextTooLargeException(context);

        context.IncrementLineNumber();

        return result ?? string.Empty;
    }

    /// <summary>
    ///     Parses a <see cref="double" />.
    /// </summary>
    /// <param name="reader">An instance of a <see cref="TextReader" /> containing the DSTV source data</param>
    /// <param name="context">The active reader context</param>
    /// <returns>The double read or an exception</returns>
    /// <exception cref="DoubleParseException"></exception>
    internal static async Task<double> ParseDouble(this TextReader reader, ReaderContext context)
    {
        var text = await reader.ReadLineAsync().ConfigureAwait(false);
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
    /// <returns></returns>
    /// <exception cref="TupleParseException{TDouble}"></exception>
    /// <exception cref="DoubleParseException"></exception>
    internal static async Task<Tuple<double, double?>> ParseTupleDouble(this TextReader reader,
        ReaderContext context)
    {
        var text = await reader.ReadLineAsync().ConfigureAwait(false);
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
    /// <returns>The integer read or an exception</returns>
    /// <exception cref="IntegerParseException"></exception>
    internal static async Task<int> ParseInteger(this TextReader reader, ReaderContext context)
    {
        var text = await reader.ReadLineAsync().ConfigureAwait(false);
        if (!int.TryParse(text, out var value)) throw new IntegerParseException(context);

        context.IncrementLineNumber();

        return value;
    }

    /// <summary>
    ///     Parses an enum of type <see cref="TEnum" />
    /// </summary>
    /// <param name="reader">An instance of a <see cref="TextReader" /> containing the DSTV source data</param>
    /// <param name="context">The active reader context</param>
    /// <returns>The integer read or an exception</returns>
    /// <exception cref="EnumParseException{TEnum}"></exception>
    internal static async Task<TEnum> ParseEnum<TEnum>(this TextReader reader, ReaderContext context)
        where TEnum : struct, Enum
    {
        var text = await reader.ReadLineAsync().ConfigureAwait(false);
        if (!Enum.TryParse(text, out TEnum value)) throw new EnumParseException<TEnum>(context);

        context.IncrementLineNumber();

        return value;
    }
}