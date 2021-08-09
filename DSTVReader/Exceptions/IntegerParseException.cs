using System;
using System.Globalization;
using DSTV.Implementations;

namespace DSTV.Exceptions
{
    public class IntegerParseException : ParseException
    {
        public IntegerParseException(ReaderContext context) : base(context,
            string.Format(CultureInfo.InvariantCulture, Constants.IntegerParseExceptionMessage, context?.LineNumber))
        {
        }

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
}