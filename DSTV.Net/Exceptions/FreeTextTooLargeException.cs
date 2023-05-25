using System;
using System.Globalization;
using DSTV.Net.Implementations;

namespace DSTV.Net.Exceptions;

public class FreeTextTooLargeException : ParseException
{
    public FreeTextTooLargeException(ReaderContext context) :
        base(context,
            string.Format(CultureInfo.InvariantCulture, Constants.FreeTextTooLargeExceptionMessage,
                context?.LineNumber))
    {
    }

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