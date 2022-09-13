using System;
using System.Globalization;
using DSTV.Implementations;

namespace DSTV.Exceptions;

public class TupleParseException<TType> : ParseException
{
    public TupleParseException(ReaderContext context, string tuplesExpected) : base(context,
        string.Format(CultureInfo.InvariantCulture, Constants.TupleParseExceptionMessage, context?.LineNumber,
            tuplesExpected, typeof(TType).Name))
    {
    }

    protected TupleParseException(ReaderContext context) : base(context)
    {
    }

    protected TupleParseException()
    {
    }

    protected TupleParseException(string? message) : base(message)
    {
    }

    protected TupleParseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}