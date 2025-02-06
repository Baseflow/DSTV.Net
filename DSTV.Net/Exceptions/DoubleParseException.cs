using DSTV.Net.Implementations;
using System.Globalization;
using System.Text;

namespace DSTV.Net.Exceptions;

public class DoubleParseException : ParseException
{
#if NET8_0_OR_GREATER
    public DoubleParseException(ReaderContext context)
        : base(context, string.Format(CultureInfo.InvariantCulture, DoubleParseExceptionMessageFormat, context?.LineNumber))
    {
    }
#else
    public DoubleParseException(ReaderContext context) : base(context,
            string.Format(CultureInfo.InvariantCulture, Constants.DoubleParseExceptionMessage, context?.LineNumber))
    {
    }
#endif


    protected DoubleParseException(ReaderContext context, string message) : base(context, message)
    {
    }

    protected DoubleParseException()
    {
    }

    protected DoubleParseException(string? message) : base(message)
    {
    }

    protected DoubleParseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}