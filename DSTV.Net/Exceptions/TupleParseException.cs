using DSTV.Net.Implementations;
using System.Globalization;
using System.Text;

namespace DSTV.Net.Exceptions;

public class TupleParseException<TType> : ParseException
{

#if NET8_0_OR_GREATER

    public TupleParseException(ReaderContext context, string tuplesExpected) : base(context,
        string.Format(CultureInfo.InvariantCulture, TupleParseExceptionMessage, context?.LineNumber,
            tuplesExpected, typeof(TType).Name))
    {
    }
#else
    public TupleParseException(ReaderContext context, string tuplesExpected) : base(context,
        string.Format(CultureInfo.InvariantCulture, Constants.TupleParseExceptionMessage, context?.LineNumber,
            tuplesExpected, typeof(TType).Name))
    {
    }
#endif

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