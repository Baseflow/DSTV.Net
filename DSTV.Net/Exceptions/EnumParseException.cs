using DSTV.Net.Implementations;

namespace DSTV.Net.Exceptions;

public class EnumParseException<TEnum> : ParseException
    where TEnum : struct, Enum
{
    public EnumParseException(ReaderContext context) : base(context)
    {
    }

    protected EnumParseException(ReaderContext context, string message) : base(context, message)
    {
    }

    protected EnumParseException()
    {
    }

    protected EnumParseException(string? message) : base(message)
    {
    }

    protected EnumParseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}