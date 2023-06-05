using DSTV.Net.Implementations;

namespace DSTV.Net.Exceptions;

public class DstvParseException : ParseException
{
    public DstvParseException(ReaderContext context) : base(context)
    {
    }

    public DstvParseException(ReaderContext context, string message) : base(context, message)
    {
    }

    public DstvParseException()
    {
    }

    public DstvParseException(string? message) : base(message)
    {
    }

    public DstvParseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}