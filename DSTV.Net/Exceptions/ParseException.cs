using System;
using DSTV.Net.Implementations;

namespace DSTV.Net.Exceptions;

/// <summary>
///     Exception that is being thrown during DSTV parsing
/// </summary>
public abstract class ParseException : Exception
{
    private readonly ReaderContext? _context;

    protected ParseException(ReaderContext context) => _context = context;

    protected ParseException(ReaderContext context, string message) : base(message) => _context = context;

    protected ParseException()
    {
    }

    protected ParseException(string? message) : base(message)
    {
    }

    protected ParseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    ///     Exposes the line number on which the error occured, or null if not present.
    /// </summary>
    public int? LineNumber => _context?.LineNumber;
}