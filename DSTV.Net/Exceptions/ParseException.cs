using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using System.Globalization;
using System.Text;

namespace DSTV.Net.Exceptions;

/// <summary>
///     Exception that is being thrown during DSTV parsing
/// </summary>
public abstract class ParseException : Exception
{
    private readonly ReaderContext? _context;

#if NET8_0_OR_GREATER
    internal static readonly CompositeFormat FreeTextTooLargeMessageFormat = CompositeFormat.Parse(Constants.FreeTextTooLargeExceptionMessage);
    internal static readonly CompositeFormat DoubleParseExceptionMessageFormat = CompositeFormat.Parse(Constants.DoubleParseExceptionMessage);
    internal static readonly CompositeFormat IntegerParseExceptionMessageFormat = CompositeFormat.Parse(Constants.IntegerParseExceptionMessage);
    internal static readonly CompositeFormat TupleParseExceptionMessage = CompositeFormat.Parse(Constants.TupleParseExceptionMessage);
    internal static readonly CompositeFormat UnexpectedCharacterMessageFormat = CompositeFormat.Parse(Constants.UnexpectedCharacterExceptionMessage);
#endif

    protected ParseException(ReaderContext context) => _context = context;

    protected ParseException(ReaderContext context, string message) : base(message) => _context = context;

    protected ParseException()
    {
    }

    protected ParseException(string? message) : base(message)
    {
    }

    protected ParseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    ///     Exposes the line number on which the error occured, or null if not present.
    /// </summary>
    public int? LineNumber => _context?.LineNumber;
}