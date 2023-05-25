using DSTV.Net.Implementations;

namespace DSTV.Net.Exceptions;

/// <summary>
///     The start of the file is missing.
///     We except a 'ST' at the beginning of the DSTV file
/// </summary>
public class MissingStartOfFileException : ParseException
{
    public MissingStartOfFileException(ReaderContext context) : base(context)
    {
    }

    protected MissingStartOfFileException(ReaderContext context, string message) : base(context, message)
    {
    }

    protected MissingStartOfFileException()
    {
    }

    protected MissingStartOfFileException(string? message) : base(message)
    {
    }

    protected MissingStartOfFileException(string? message, Exception? innerException) : base(message,
        innerException)
    {
    }
}