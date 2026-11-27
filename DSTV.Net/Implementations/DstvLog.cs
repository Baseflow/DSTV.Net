using Microsoft.Extensions.Logging;

namespace DSTV.Net.Implementations;

internal static partial class DstvLog
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Warning,
        Message = "Failed to parse DSTV element data: {DataLine}")]
    internal static partial void LogParseElementFailed(ILogger logger, string dataLine, Exception exception);

    internal static void LogCreateElementFailed(ILogger logger, IEnumerable<string> dataLines,
        Exception exception) =>
        LogCreateContourFailed(logger, string.Join(Environment.NewLine, dataLines), exception);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "Failed to create DSTV contour: {DataLines}")]
    private static partial void LogCreateContourFailed(ILogger logger, string dataLines, Exception exception);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Warning,
        Message = "Unregistered DSTV block code detected: {BlockCode}")]
    internal static partial void LogUnknownBlockCode(ILogger logger, string blockCode);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Warning,
        Message = "First point of contour is missing a face code; using the previous contour point's face code")]
    internal static partial void LogMissingFaceCode(ILogger logger);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Warning,
        Message = "Contour section ends with an unclosed sequence of points")]
    internal static partial void LogUnclosedContour(ILogger logger);
}
