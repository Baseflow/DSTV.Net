using System.Globalization;
using DSTV.Net.Implementations;

namespace DSTV.Net.Exceptions;

/// <summary>
///     Thrown when a character is expected, but another character is read.
/// </summary>
public class UnexpectedCharacterException : ParseException
{
    public UnexpectedCharacterException(ReaderContext context, char expected, char actual)
        : base(context,
            string.Format(CultureInfo.InvariantCulture, Constants.UnexpectedCharacterExceptionMessage,
                context?.LineNumber, expected, actual))
    {
    }

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