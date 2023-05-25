using System.Globalization;

namespace DSTV.Net;

/// <summary>
///     Constants class used for holding constant strings.
/// </summary>
internal static class Constants
{
    internal const string StartOfFile = "ST";

    internal const string EndOfFile = "EN";

    internal const string Indicator = "IN";

    internal const int FreeTextMaximumSize = 80;

    internal const string DoubleParseExceptionMessage = "Could not parse double value on lineNumber {0}";

    internal const string IntegerParseExceptionMessage = "Could not parse integer value on lineNumber {0}";

    internal const string TupleParseExceptionMessage = "Expected {1} {2} values on lineNumber {0}";

    internal const string UnexpectedCharacterExceptionMessage =
        "Expected character '{1}' at lineNumber {0}, but retrieved character '{2}' instead.";

    internal const string UnexpectedEndExceptionMessage =
        "Unexpected end encountered. The file should end with 'EN'";

    internal const string FreeTextTooLargeExceptionMessage =
        "The parsed FreeText is too large, only a maximum of 80 characters is allowed on lineNumber {0}";

    internal static CultureInfo ParserCultureInfo = CultureInfo.InvariantCulture;
}