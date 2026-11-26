using DSTV.Net.Data;
using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace DSTV.Net.Implementations;

internal static class BodyReader
{
    /// <summary>
    ///     Skips the item in <paramref name="items"/> where the item is equal to <paramref name="contourType"/> and executes the <paramref name="action"/> on the remaining items.
    /// </summary>
    /// <param name="items">An enumerable of strings that will be iterated.</param>
    /// <param name="contourType">The type of contour to handle with.</param>
    /// <param name="action">The action to call for each non-skipped item.</param>
    /// <param name="logger">The logger for recoverable parse failures.</param>
    /// <param name="cancellationToken">A token that can cancel processing.</param>
    private static void SkipSelfAndExecute(this IEnumerable<string> items, ContourType contourType,
        Action<string> action, ILogger logger, CancellationToken cancellationToken)
    {
        foreach (var item in items)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                if (item.Equals(contourType.ToString(), StringComparison.Ordinal)) continue;
                action(item);
            }
            catch (DstvParseException dStVParseException)
            {
                DstvLog.LogParseElementFailed(logger, item, dStVParseException);
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "Should be cleaned up later")]
    public static async Task<IEnumerable<DstvElement>> GetElementsAsync(ReaderContext readerContext,
        ILogger logger, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        List<DstvElement> outputList = [];
        var elemMap = await GetElementMapAsync(readerContext, logger, cancellationToken).ConfigureAwait(false);
        // holes & slots
        (elemMap.GetValueOrDefault(ContourType.BO) ?? [])
            .SelectMany(holeList => holeList)
            .SkipSelfAndExecute(ContourType.BO, holeNote => outputList.Add(DstvHole.CreateHole(holeNote)),
                logger, cancellationToken);

        // outer contours
        var outerBorders = elemMap.GetValueOrDefault(ContourType.AK);
        AddContoursByType(outputList, outerBorders, ContourType.AK, logger, cancellationToken);
        // inner contours
        var innerBorders = elemMap.GetValueOrDefault(ContourType.IK);
        AddContoursByType(outputList, innerBorders, ContourType.IK, logger, cancellationToken);
        // powder contours
        var powderPointNotes = elemMap.GetValueOrDefault(ContourType.PU);
        AddContoursByType(outputList, powderPointNotes, ContourType.PU, logger, cancellationToken);
        // punch contours
        var punchPointNotes = elemMap.GetValueOrDefault(ContourType.KO);
        AddContoursByType(outputList, punchPointNotes, ContourType.KO, logger, cancellationToken);
        // numeration
        (elemMap.GetValueOrDefault(ContourType.SI) ?? [])
            .SelectMany(numNote => numNote)
            .SkipSelfAndExecute(ContourType.SI, numNote => outputList.Add(DstvNumeration.CreateNumeration(numNote)),
                logger, cancellationToken);
        // bends
        (elemMap.GetValueOrDefault(ContourType.KA) ?? [])
            .SelectMany(bendNote => bendNote)
            .SkipSelfAndExecute(ContourType.KA, bendNote => outputList.Add(DstvBend.CreateBend(bendNote)),
                logger, cancellationToken);
        // cuts
        (elemMap.GetValueOrDefault(ContourType.SC) ?? [])
            .SelectMany(cutNote => cutNote)
            .SkipSelfAndExecute(ContourType.SC, cutNote => outputList.Add(DstvCut.CreateCut(cutNote)),
                logger, cancellationToken);
       
        return outputList;
    }

    private static void AddContoursByType(List<DstvElement> outElemList, List<List<string>>? notesBlockList,
        ContourType type, ILogger logger, CancellationToken cancellationToken)
    {
        if (notesBlockList is null) return;

        foreach (var noteList in notesBlockList)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<DstvContourPoint> localList = [];
            foreach (var pointNote in noteList)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    if (pointNote.Equals(type.ToString(), StringComparison.Ordinal)) continue;
                    localList.Add(DstvContourPoint.CreatePoint(pointNote));
                }
                catch (DstvParseException dStVParseException)
                {
                    DstvLog.LogParseElementFailed(logger, pointNote, dStVParseException);
                }
            }

            try
            {
                outElemList.AddRange(Contour.CreateSeveralContours(localList, type, logger));
            }
            catch (DstvParseException dStVParseException)
            {
                DstvLog.LogCreateElementFailed(logger, noteList, dStVParseException);
            }
        }
    }

    private static async Task<Dictionary<ContourType, List<List<string>>?>> GetElementMapAsync(
        ReaderContext context, ILogger logger, CancellationToken cancellationToken)
    {
        Dictionary<ContourType, List<List<string>>?> elemMap = [];
        var curKey = ContourType.None;
        while (true)
        {
            var line = await context.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            // if has END-mark.
            //  Maybe to be refactored for multi-peace file processing
            if (line?.Equals(Constants.EndOfFile, StringComparison.Ordinal) != false ||
                line.Equals(Constants.Indicator, StringComparison.Ordinal)) break;

            // if has quote-mark
            if (Regex.IsMatch(line, "^\\*\\*.*", RegexOptions.None, TimeSpan.FromSeconds(1))) continue;

            if (IsCodeLine(line))
            {
#if NET
                curKey = (ContourType)Enum.Parse<ContourType>(line);
#else
                curKey = (ContourType)Enum.Parse(typeof(ContourType), line);
#endif
                if (!CheckIfMark(line)) DstvLog.LogUnknownBlockCode(logger, line);

                if (!elemMap.ContainsKey(curKey)) elemMap.Add(curKey, []);

                elemMap.GetValueOrDefault(curKey)?.Add([]);
            }

            if (curKey is ContourType.None) continue;

            var outerList = elemMap.GetValueOrDefault(curKey);
            outerList?.Last().Add(line);
        }

        return elemMap;
    }

    private static bool CheckIfMark(string str) =>
        // there are some more patterns in DStV, but they are too seldom and/or is not necessary yet
        Regex.IsMatch(str,
            "^BO$|^SI$|^AK$|^IK$|^PU$|^KO$|^SC$|^UE$|^KA$|^EN$|^ST$|^E[0-9]$|^B[0-9]$|^S[0-9]$|^A[0-9]$|^I[0-9]$|^P[0-9]$|^K[0-9]$", RegexOptions.None, TimeSpan.FromSeconds(1));

    internal static bool IsCodeLine(string? str) => str is not null
        && Regex.IsMatch(str, "^[A-Z0-9]{2}$", RegexOptions.None, TimeSpan.FromSeconds(1));

    public static string[] RemoveVoids(IEnumerable<string> toBeRefined)
    {
        return [.. toBeRefined.Where(static item => !string.IsNullOrEmpty(item))];
    }
}
