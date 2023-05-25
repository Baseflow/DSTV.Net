using System;
using DSTV.Net.Implementations;

namespace DSTV.Net.Exceptions;

/// <summary>
///     The end of the file is missing.
///     We except a 'EN' at the end of the DSTV file
/// </summary>
public class UnexpectedEndException : ParseException
{
    public UnexpectedEndException(ReaderContext context) : base(context, Constants.UnexpectedEndExceptionMessage)
    {
    }

    protected UnexpectedEndException(ReaderContext context, string message) : base(context, message)
    {
    }

    protected UnexpectedEndException()
    {
    }

    protected UnexpectedEndException(string? message) : base(message)
    {
    }

    protected UnexpectedEndException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}