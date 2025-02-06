using DSTV.Net.Implementations;
using System.Globalization;
using System.Text;

namespace DSTV.Net.Exceptions;

public class FreeTextTooLargeException : ParseException
{
#if NET8_0_OR_GREATER
    public FreeTextTooLargeException(ReaderContext context) :
        base(context,
            string.Format(CultureInfo.InvariantCulture, FreeTextTooLargeMessageFormat, context?.LineNumber))
    {
    }
#else
    public FreeTextTooLargeException(ReaderContext context) :
        base(context,
            string.Format(CultureInfo.InvariantCulture, Constants.FreeTextTooLargeExceptionMessage,
                context?.LineNumber))
    {
    }
#endif

    protected FreeTextTooLargeException(ReaderContext context, string message) : base(context, message)
    {
    }

    protected FreeTextTooLargeException()
    {
    }

    protected FreeTextTooLargeException(string? message) : base(message)
    {
    }

    protected FreeTextTooLargeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}