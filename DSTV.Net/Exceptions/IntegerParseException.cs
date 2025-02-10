using DSTV.Net.Implementations;
using System.Globalization;
using System.Text;

namespace DSTV.Net.Exceptions;

public class IntegerParseException : ParseException
{
#if NET8_0_OR_GREATER
    public IntegerParseException(ReaderContext context) :
        base(context,
            string.Format(CultureInfo.InvariantCulture, IntegerParseExceptionMessageFormat, context?.LineNumber))
    {
    }
#else
    public IntegerParseException(ReaderContext context) : base(context,
        string.Format(CultureInfo.InvariantCulture, Constants.IntegerParseExceptionMessage, context?.LineNumber))
    {
    }
#endif

    protected IntegerParseException(ReaderContext context, string message) : base(context, message)
    {
    }

    protected IntegerParseException()
    {
    }

    protected IntegerParseException(string? message) : base(message)
    {
    }

    protected IntegerParseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}