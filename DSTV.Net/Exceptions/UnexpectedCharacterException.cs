using DSTV.Net.Implementations;
using System.Globalization;
using System.Text;

namespace DSTV.Net.Exceptions;

/// <summary>
///     Thrown when a character is expected, but another character is read.
/// </summary>
public class UnexpectedCharacterException : ParseException
{
#if NET8_0_OR_GREATER
    public UnexpectedCharacterException(ReaderContext context, char expected, char actual)
        : base(context,
            string.Format(CultureInfo.InvariantCulture, UnexpectedCharacterMessageFormat,
                context?.LineNumber, expected, actual))
    {
    }
#else
    public UnexpectedCharacterException(ReaderContext context, char expected, char actual)
        : base(context,
            string.Format(CultureInfo.InvariantCulture, Constants.UnexpectedCharacterExceptionMessage,
                context?.LineNumber, expected, actual))
    {
    }
#endif

    protected UnexpectedCharacterException(ReaderContext context) : base(context)
    {
    }

    protected UnexpectedCharacterException(ReaderContext context, string message) : base(context, message)
    {
    }

    protected UnexpectedCharacterException()
    {
    }

    protected UnexpectedCharacterException(string? message) : base(message)
    {
    }

    protected UnexpectedCharacterException(string? message, Exception? innerException) : base(message,
        innerException)
    {
    }
}