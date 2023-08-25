using DSTV.Net.Data;
using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using System.Text.RegularExpressions;

namespace DSTV.Net.Implementations;

internal static class BodyReader
{
    /// <summary>
    ///     Skips the item in <param name="items"></param> where the item is equal to <param name="contourType"></param> and executes the <param name="action"></param> on the remaining items.
    /// </summary>
    /// <param name="items">An enumerable of strings that will be iterated.</param>
    /// <param name="contourType">The type of contour to handle with.</param>
    /// <param name="action">The action to call for each non-skipped item.</param>
    private static void SkipSelfAndExecute(this IEnumerable<string> items, ContourType contourType, Action<string> action)
    {
        foreach (var item in items)
        {
            try
            {
                if (item.Equals(contourType.ToString(), StringComparison.Ordinal)) continue;
                action(item);
            }
            catch (DstvParseException dStVParseException)
            {
                Console.WriteLine(dStVParseException);
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "Should be cleaned up later")]
    public static async Task<IEnumerable<DstvElement>> GetElementsAsync(ReaderContext readerContext)
    {
        var outputList = new List<DstvElement>();
        var elemMap = await GetElementMapAsync(readerContext).ConfigureAwait(false);
        // holes & slots
        (elemMap.GetValueOrDefault(ContourType.BO) ?? new List<List<string>>())
            .SelectMany(holeList => holeList)
            .SkipSelfAndExecute(ContourType.BO, (holeNote) => outputList.Add(DstvHole.CreateHole(holeNote)));

        // outer contours
        var outerBorders = elemMap.GetValueOrDefault(ContourType.AK);
        AddContoursByType(outputList, outerBorders, ContourType.AK);
        // inner contours
        var innerBorders = elemMap.GetValueOrDefault(ContourType.IK);
        AddContoursByType(outputList, innerBorders, ContourType.IK);
        // powder contours
        var powderPointNotes = elemMap.GetValueOrDefault(ContourType.PU);
        AddContoursByType(outputList, powderPointNotes, ContourType.PU);
        // punch contours
        var punchPointNotes = elemMap.GetValueOrDefault(ContourType.KO);
        AddContoursByType(outputList, punchPointNotes, ContourType.KO);
        // numeration
        (elemMap.GetValueOrDefault(ContourType.SI)?? new List<List<string>>())
            .SelectMany(numNote => numNote)
            .SkipSelfAndExecute(ContourType.SI, (numNote) => outputList.Add(DstvNumeration.CreateNumeration(numNote)));
        // bends
        (elemMap.GetValueOrDefault(ContourType.KA) ?? new List<List<string>>())
            .SelectMany(bendNote => bendNote)
            .SkipSelfAndExecute(ContourType.KA, (bendNote) => outputList.Add(DstvBend.CreateBend(bendNote)));
        // cuts
        (elemMap.GetValueOrDefault(ContourType.SC) ?? new List<List<string>>())
            .SelectMany(cutNote => cutNote)
            .SkipSelfAndExecute(ContourType.SC, (cutNote) => outputList.Add(DstvCut.CreateCut(cutNote)));
       
        return outputList;
    }

    private static void AddContoursByType(List<DstvElement> outElemList, List<List<string>>? notesBlockList,
        ContourType type)
    {
        if (notesBlockList is null) return;

        foreach (var noteList in notesBlockList)
        {
            List<DstvContourPoint> localList = new();
            foreach (var pointNote in noteList)
            {
                try
                {
                    if (pointNote.Equals(type.ToString(), StringComparison.Ordinal)) continue;
                    localList.Add(DstvContourPoint.CreatePoint(pointNote));
                }
                catch (DstvParseException dStVParseException)
                {
                    Console.WriteLine(dStVParseException);
                }
            }

            try
            {
                outElemList.AddRange(Contour.CreateSeveralContours(localList, type));
            }
            catch (DstvParseException dStVParseException)
            {
                Console.WriteLine(dStVParseException);
            }
        }
    }

    private static async Task<Dictionary<ContourType, List<List<string>>?>> GetElementMapAsync(ReaderContext context)
    {
        var reader = context.Source;

        Dictionary<ContourType, List<List<string>>?> elemMap = new();
        var curKey = ContourType.None;
        while (true)
        {
            var line = await reader.ReadLineAsync().ConfigureAwait(false);
            // if has END-mark.
            //  Maybe to be refactored for multi-peace file processing
            if (line?.Equals(Constants.EndOfFile, StringComparison.Ordinal) != false ||
                line.Equals(Constants.Indicator, StringComparison.Ordinal)) break;

            // if has quote-mark
            if (Regex.IsMatch(line, "^\\*\\*.*", RegexOptions.None, TimeSpan.FromSeconds(1))) continue;

            if (CheckCodeLine(line))
            {
                curKey = Enum.Parse<ContourType>(line);
                if (!CheckIfMark(line)) Console.WriteLine(line + "Warning: unregistered DStV code-line detected: ");

                if (!elemMap.ContainsKey(curKey)) elemMap.Add(curKey, new List<List<string>>());

                elemMap.GetValueOrDefault(curKey)?.Add(new List<string>());
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

    private static bool CheckCodeLine(string str) => Regex.IsMatch(str, "^[A-Z0-9]{2}$", RegexOptions.None, TimeSpan.FromSeconds(1));

    public static string[] RemoveVoids(IEnumerable<string> toBeRefined)
    {
        var tempList = new List<string>(toBeRefined);
        tempList.RemoveAll(string.IsNullOrEmpty);
        return tempList.ToArray();
    }
}