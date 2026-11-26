using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using DSTV.Net.Extensions;
using System.Globalization;
using DSTV.Net.Data;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace DSTV.Net.Test;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1515", Justification = "Test classes must be public for xUnit")]
public class Tests
{
    private const string DataPath = "../../../";

    /// <summary>
    ///     Example of a valid DSTV file header.
    ///     Expect a valid parse result
    /// </summary>
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
    public async Task TestP1Header()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/P1.nc");
        var dstvReader = new DstvReader();

        var parsed = await dstvReader.ParseAsync(streamReader);
        var header = parsed.Header;

        Assert.NotNull(header);

        // ! TODO:
        var result = header!;

        //2x, a Order identification
        Assert.Equal("PROJECT-1", result.OrderIdentification);
        //2x, a Drawing identification
        Assert.Equal("0", result.DrawingIdentification);
        //2x, a Phase identification
        Assert.Equal("1", result.PhaseIdentification);
        //2x, a Piece identification
        Assert.Equal("B_1", result.PieceIdentification);
        //2x, a Steel quality
        Assert.Equal("A992", result.SteelQuality);
        //2x, i Quantity of pieces
        Assert.Equal(1, result.QuantityOfPieces);
        //2x, a Profile
        Assert.Equal("W21X44", result.Profile);
        //2x, a Code Profile
        Assert.Equal(CodeProfile.I, result.CodeProfile);
        //2x, f [,f ] Length , Saw Length
        Assert.Equal(6236.88, result.Length);
        Assert.Null(result.SawLength);
        //2x, f Profile height
        Assert.Equal(525.78, result.ProfileHeight);
        //2x, f Flange width
        Assert.Equal(165.10, result.FlangeWidth);
        //2x, f Flange thickness
        Assert.Equal(11.43, result.FlangeThickness);
        //2x, f Web thickness
        Assert.Equal(8.89, result.WebThickness);
        //2x, f Radius
        Assert.Equal(17.15, result.Radius);
        //2x, f Weight by meter
        Assert.Equal(65.479, result.WeightByMeter);
        //2x, f Painting surface by meter < m2/m >
        Assert.Equal(0.000, result.PaintingSurfaceByMeter);
        //2x, f Web Start Cut
        Assert.Equal(0.000, result.WebStartCut);
        //2x, f Web End Cut
        Assert.Equal(15.000, result.WebEndCut);
        //2x, f Flange Start Cut
        Assert.Equal(0.000, result.FlangeStartCut);
        //2x, f Flange End Cut
        Assert.Equal(0.000, result.FlangeEndCut);
        //2x, a Text info on piece
        Assert.Equal(string.Empty, result.Text1InfoOnPiece);
        //2x, a Text info on piece
        Assert.Equal(string.Empty, result.Text2InfoOnPiece);
        //2x, a Text info on piece
        Assert.Equal(string.Empty, result.Text3InfoOnPiece);
        //2x, a Text info on piece
        Assert.Equal(string.Empty, result.Text4InfoOnPiece);
    }

	/// <summary>
    ///     Example of a valid DSTV file body.
    ///     Expect a valid parse result
    /// </summary>
    [Fact]
    public async Task TestP1Body()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/P1.nc");
        var dstvReader = new DstvReader();

        var parsed = await dstvReader.ParseAsync(streamReader);
        var header = parsed.Header;

        Assert.NotNull(parsed.Elements);
        Assert.Equal(10, parsed.Elements.Count());

        var elements = parsed.Elements.ToArray();
        Assert.True(elements[0] is DstvHole);
        Assert.True(elements[1] is DstvHole);
        Assert.True(elements[2] is DstvHole);
        Assert.True(elements[3] is Contour);
        Assert.True(elements[4] is Contour);
        Assert.True(elements[5] is Contour);
        Assert.True(elements[6] is Contour);
        Assert.True(elements[7] is Contour);
        Assert.True(elements[8] is DstvNumeration);
        Assert.True(elements[9] is DstvNumeration);
        Assert.All(elements.OfType<DstvHole>(), hole => Assert.Equal(ContourType.BO, hole.Type));
        Assert.All(elements.OfType<DstvNumeration>(), numeration =>
            Assert.Equal(ContourType.SI, numeration.Type));
        var numerations = elements.OfType<DstvNumeration>().ToArray();
        Assert.Equal(FaceCode.V, numerations[0].FlCode);
        Assert.Equal("B_1", numerations[0].Text);
        Assert.Equal(76, numerations[0].LetterHeight);
        Assert.Equal("pl", numerations[1].Text);
        Assert.Equal(FaceCode.V, numerations[1].FlCode);
        Assert.All(elements.OfType<Contour>(), contour =>
            Assert.Contains(contour.Type, new[] { ContourType.AK, ContourType.IK, ContourType.PU, ContourType.KO }));
    }

    /// <summary>
    ///     Example of a valid DSTV file header.
    ///     Expect a valid parse result
    /// </summary>
    [Fact]
    public async Task TestPRST372()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/RST37-2.nc");
        var dstvReader = new DstvReader();

        var header = (await dstvReader.ParseAsync(streamReader)).Header;

        Assert.NotNull(header);

        // ! TODO:
        var result = header!;

        //2x, a Order identification
        Assert.Equal("1", result.OrderIdentification);
        //2x, a Drawing identification
        Assert.Equal("1", result.DrawingIdentification);
        //2x, a Phase identification
        Assert.Equal("14", result.PhaseIdentification);
        //2x, a Piece identification
        Assert.Equal("14", result.PieceIdentification);
        //2x, a Steel quality
        Assert.Equal("RST37-2", result.SteelQuality);
        //2x, i Quantity of pieces
        Assert.Equal(2, result.QuantityOfPieces);
        //2x, a Profile
        Assert.Equal("ZS175*1.5", result.Profile);
        //2x, a Code Profile
        Assert.Equal(CodeProfile.SO, result.CodeProfile);
        //2x, f [,f ] Length , Saw Length
        Assert.Equal(1133.00, result.Length);
        Assert.Null(result.SawLength);
        //2x, f Profile height
        Assert.Equal(175.00, result.ProfileHeight);
        //2x, f Flange width
        Assert.Equal(81.00, result.FlangeWidth);
        //2x, f Flange thickness
        Assert.Equal(1.50, result.FlangeThickness);
        //2x, f Web thickness
        Assert.Equal(1.50, result.WebThickness);
        //2x, f Radius
        Assert.Equal(4.00, result.Radius);
        //2x, f Weight by meter
        Assert.Equal(4.416, result.WeightByMeter);
        //2x, f Painting surface by meter < m2/m >
        Assert.Equal(0.753, result.PaintingSurfaceByMeter);
        //2x, f Web Start Cut
        Assert.Equal(0.000, result.WebStartCut);
        //2x, f Web End Cut
        Assert.Equal(0.000, result.WebEndCut);
        //2x, f Flange Start Cut
        Assert.Equal(0.000, result.FlangeStartCut);
        //2x, f Flange End Cut
        Assert.Equal(0.000, result.FlangeEndCut);
        //2x, a Text info on piece
        Assert.Equal("Pfette", result.Text1InfoOnPiece);
        //2x, a Text info on piece
        Assert.Equal(string.Empty, result.Text2InfoOnPiece);
        //2x, a Text info on piece
        Assert.Equal(string.Empty, result.Text3InfoOnPiece);
        //2x, a Text info on piece
        Assert.Equal(string.Empty, result.Text4InfoOnPiece);
    }

    /// <summary>
    ///     Example of an invalid OrderIdentification field (being too large, over 80 characters)
    ///     Expect a <seealso cref="FreeTextTooLargeException" />
    /// </summary>
    [Fact]
    public async Task TestFreeTextTooLargeException()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/E1.nc");
        var dstvReader = new DstvReader();

        var exception =
            await Assert.ThrowsAsync<FreeTextTooLargeException>(() => dstvReader.ParseAsync(streamReader));

#if NET8_0_OR_GREATER
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, ParseException.FreeTextTooLargeMessageFormat, 3), exception.Message);
#else
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, Constants.FreeTextTooLargeExceptionMessage, 3), exception.Message);
#endif
    }

    /// <summary>
    ///     Example of an invalid CodeProfile field (not being recognized as a valid enumeration of type
    ///     <seealso cref="CodeProfile" />
    ///     Expect a <seealso cref="EnumParseException{TEnum}" />
    /// </summary>
    [Fact]
    public async Task TestEnumParseException()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/E2.nc");
        var dstvReader = new DstvReader();

        await Assert.ThrowsAsync<EnumParseException<CodeProfile>>(() => dstvReader.ParseAsync(streamReader));
    }

    /// <summary>
    ///     Example of an invalid Start of file (ST header at first row)
    ///     Expect a <seealso cref="MissingStartOfFileException" />
    /// </summary>
    [Fact]
    public async Task TestMissingStartOfFileException()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/E3.nc");
        var dstvReader = new DstvReader();

        await Assert.ThrowsAsync<MissingStartOfFileException>(() => dstvReader.ParseAsync(streamReader));
    }

    /// <summary>
    ///     Example of an invalid double value in the Length property of the DSTV file
    ///     Expect a <seealso cref="DoubleParseException" />
    /// </summary>
    [Fact]
    public async Task TestDoubleParseException()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/E4.nc");
        var dstvReader = new DstvReader();

        var exception = await Assert.ThrowsAsync<DoubleParseException>(() => dstvReader.ParseAsync(streamReader));
#if NET8_0_OR_GREATER
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, ParseException.DoubleParseExceptionMessageFormat, 11), exception.Message);
#else
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, Constants.DoubleParseExceptionMessage, 11), exception.Message);
#endif
    }

    /// <summary>
    ///     Example of an invalid 2x (2 spaces) field in the DSTV file (at line 7)
    ///     Expect a <seealso cref="UnexpectedCharacterException" />
    /// </summary>
    [Fact]
    public async Task TestUnexpectedCharacterException()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/E5.nc");
        var dstvReader = new DstvReader();

        var exception =
            await Assert.ThrowsAsync<UnexpectedCharacterException>(() => dstvReader.ParseAsync(streamReader));
#if NET8_0_OR_GREATER
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, ParseException.UnexpectedCharacterMessageFormat, 7, ' ', 'k'), exception.Message);
#else
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, Constants.UnexpectedCharacterExceptionMessage, 7, ' ', 'k'), exception.Message);
#endif
    }

    /// <summary>
    ///     Example of an invalid integer value in the quantity property of the DSTV file
    ///     Expect a <seealso cref="IntegerParseException" />
    /// </summary>
    [Fact]
    public async Task TestIntegerParseException()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/E6.nc");
        var dstvReader = new DstvReader();

        var exception = await Assert.ThrowsAsync<IntegerParseException>(() => dstvReader.ParseAsync(streamReader));
#if NET8_0_OR_GREATER
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, ParseException.IntegerParseExceptionMessageFormat, 8), exception.Message);
#else
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, Constants.IntegerParseExceptionMessage, 8), exception.Message);
#endif
    }

    /// <summary>
    ///     Example of an invalid tuple value in the quantity property of the DSTV file
    ///     Expect a <seealso cref="TupleParseException{TType}" />
    /// </summary>
    [Fact]
    public async Task TestTupleParseException()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/E7.nc");
        var dstvReader = new DstvReader();

        var exception =
            await Assert.ThrowsAsync<TupleParseException<double>>(() => dstvReader.ParseAsync(streamReader));
#if NET8_0_OR_GREATER
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, ParseException.TupleParseExceptionMessage, 11, "1 to 2", nameof(Double)),
            exception.Message);
#else
        Assert.Equal(string.Format(CultureInfo.InvariantCulture, Constants.TupleParseExceptionMessage, 11, "1 to 2", nameof(Double)),
            exception.Message);
#endif
    }

    /// <summary>
    ///     Example with notch points
    ///     Expect valid IsNotch flag for points
    /// </summary>
    [Fact]
    public async Task TestIsNotchPoint()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/notch.nc1");
        var dstvReader = new DstvReader();

        var result = await dstvReader.ParseAsync(streamReader);
        var dstvElement = result.Elements.First();

        Assert.IsType<Contour>(dstvElement);
        var contour = (Contour)dstvElement;

        Assert.Equal(ContourType.AK, contour.Type);
        var actualPoints = contour.Points.ToArray();

        Assert.Equal(6, actualPoints.Length);
        // First notch "w" point with flange code
        Assert.True(actualPoints[1].IsNotch);
        // First not notch point with flange code
        Assert.False(actualPoints[2].IsNotch);
        // Second notch "t" point without flange code
        Assert.True(actualPoints[3].IsNotch);
        // Second not notch point without flange code
        Assert.False(actualPoints[4].IsNotch);
    }

    [Fact]
    public void TestContourPointPreservesWeldPreparation()
    {
        var point = DstvContourPoint.CreatePoint(
            "  o      15.15s      0.00       0.00       7.24       0.00       0.00       0.00");
        var skewedPoint = Assert.IsType<DstvSkewedPoint>(DstvContourPoint.CreatePoint(
            "  o      48.95      76.95       0.00      10.26     -38.00       0.00       0.00"));

        var firstPoint = Assert.IsType<DstvSkewedPoint>(point);
        Assert.Equal(7.24, firstPoint.FirstAngle);
        Assert.Equal(0, firstPoint.FirstBlunting);
        Assert.Equal(10.26, skewedPoint.FirstAngle);
        Assert.Equal(-38.00, skewedPoint.FirstBlunting);
    }

    [Theory]
    [InlineData("v", FaceCode.V)]
    [InlineData("o", FaceCode.O)]
    [InlineData("u", FaceCode.U)]
    [InlineData("h", FaceCode.H)]
    public void TestFaceCodesAreParsed(string sourceCode, FaceCode expected)
    {
        var point = DstvContourPoint.CreatePoint($"  {sourceCode} 10.00 20.00 0.00");

        Assert.Equal(expected, point.FlCode);
    }

    [Fact]
    public async Task TestMissingOptionalHeaderTextPreservesFirstVContour()
    {
        const string data = """
                            ST
                            ** profile
                              order
                              drawing
                              phase
                              piece
                              steel
                              1
                              profile
                              I
                                 370.57
                                 200.00
                                 200.00
                                  15.00
                                   9.00
                                  18.00
                                 61.300
                                  1.150
                                  0.000
                                  0.000
                                  0.000
                                  0.000



                            AK
                              v      34.70s      0.00       0.00       0.00       0.00       0.00       0.00
                                    370.57       0.00       0.00       0.00       0.00       0.00       0.00
                            AK
                              o      15.15s      0.00       0.00       0.00       0.00       0.00       0.00
                                    370.57       0.00       0.00       0.00       0.00       0.00       0.00
                            EN
                            """;
        var dstvReader = new DstvReader();

        var result = await dstvReader.ParseAsync(data);
        var contours = result.Elements.OfType<Contour>().ToArray();

        Assert.Equal(2, contours.Length);
        Assert.Equal(FaceCode.V, contours[0].Points.First().FlCode);
        Assert.Equal(34.70, contours[0].Points.First().XCoord);
        Assert.Equal(FaceCode.O, contours[1].Points.First().FlCode);
    }

    [Fact]
    public async Task TestCancellationTokenReachesTextReader()
    {
        using var cancellationSource = new CancellationTokenSource();
        using var streamReader = new CancelOnReadTextReader("ST", cancellationSource);
        var dstvReader = new DstvReader();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => dstvReader.ParseAsync(streamReader, cancellationSource.Token));

        Assert.True(streamReader.ReceivedCancellationToken);
    }

    [Fact]
    public async Task TestParserReportsWarningsThroughInjectedLogger()
    {
        var data = await File.ReadAllTextAsync($"{DataPath}/Data/P1.nc");
        data = data.Replace(
            "  o      674.41s    38.10      20.64     0.00",
            "  o      674.41s",
            StringComparison.Ordinal);
        var logger = new CollectingLogger<DstvReader>();
        var dstvReader = new DstvReader(logger);

        await dstvReader.ParseAsync(data);

        var warning = Assert.Single(logger.Entries,
            entry => entry.Level == LogLevel.Warning && entry.EventId.Id == 1);
        Assert.Equal(1, warning.EventId.Id);
        Assert.Contains("674.41s", warning.Message, StringComparison.Ordinal);
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long",
        Justification = "Feature detection is verified against one representative mixed model")]
    public void TestDstvFeatureDetection()
    {
        var regularHole = DstvHole.CreateHole("  v 10.00 20.00 5.00 0.00");
        var slottedHole = DstvHole.CreateHole("  v 10.00 20.00 5.00 0.00 10.00 2.00 45.00");
        var straightCut = DstvCut.CreateCut("  0.00 0.00 0.00 1.00 0.00 0.00");
        var skewedCut = DstvCut.CreateCut("  0.00 0.00 0.00 1.00 1.00 0.00");
        var bevel = new DstvSkewedPoint(FaceCode.V, 0, 0, false, 0, 10, 4, 0, 0);
        var curved = new DstvContourPoint(FaceCode.V, 0, 0, false, 10);
        var externalContour = Contour.CreateSeveralContours([bevel, curved], ContourType.AK).Single();
        var internalContour = Contour.CreateSeveralContours(
            [new DstvContourPoint(FaceCode.V, 0, 0, false, 0)],
            ContourType.IK).Single();
        var powderMarking = Contour.CreateSeveralContours(
            [
                new DstvContourPoint(FaceCode.V, 0, 0, false, 0),
                new DstvContourPoint(FaceCode.V, 100, 20, false, 0),
            ],
            ContourType.PU).Single();
        var punchMarking = Contour.CreateSeveralContours(
            [
                new DstvContourPoint(FaceCode.V, 0, 0, false, 0),
                new DstvContourPoint(FaceCode.V, 100, 20, false, 0),
            ],
            ContourType.KO).Single();
        var numeration = DstvNumeration.CreateNumeration("  v 10.00 20.00 0.00 5.00 TEST");
        var bend = DstvBend.CreateBend("  0.00 0.00 100.00 0.00 90.00 5.00");
        var model = new DstvRecord
        {
            Elements =
            [
                regularHole, slottedHole, powderMarking, punchMarking, numeration, bend, straightCut,
                skewedCut, externalContour, internalContour,
            ],
        };

        Assert.True(model.HasRegularBoreholes());
        Assert.True(model.HasSlottedBoreholes());
        Assert.True(model.HasPowderMarkings());
        Assert.True(model.HasPunchMarkings());
        Assert.True(model.HasNumerations());
        Assert.True(model.HasBends());
        Assert.True(model.HasSawCuts());
        Assert.True(model.HasSkewedSawCuts());
        Assert.True(model.HasRegularCuts());
        Assert.True(model.HasSkewedRegularCuts());
        Assert.True(model.HasInternalContours());
        Assert.True(model.HasCurvedRegularCuts());
        Assert.Equal(
            PartFeatures.RegularBoreholes
            | PartFeatures.SlottedBoreholes
            | PartFeatures.PowderMarkings
            | PartFeatures.PunchMarkings
            | PartFeatures.Numerations
            | PartFeatures.Bends
            | PartFeatures.SawCuts
            | PartFeatures.SkewedSawCuts
            | PartFeatures.RegularCuts
            | PartFeatures.SkewedRegularCuts
            | PartFeatures.InternalContours
            | PartFeatures.CurvedRegularCuts,
            model.GetPartFeatures());

        var emptyModel = new DstvRecord();
        Assert.False(emptyModel.HasRegularBoreholes());
        Assert.False(emptyModel.HasSlottedBoreholes());
        Assert.False(emptyModel.HasPowderMarkings());
        Assert.False(emptyModel.HasPunchMarkings());
        Assert.False(emptyModel.HasNumerations());
        Assert.False(emptyModel.HasBends());
        Assert.False(emptyModel.HasSawCuts());
        Assert.False(emptyModel.HasSkewedSawCuts());
        Assert.False(emptyModel.HasRegularCuts());
        Assert.False(emptyModel.HasSkewedRegularCuts());
        Assert.False(emptyModel.HasInternalContours());
        Assert.False(emptyModel.HasCurvedRegularCuts());
        Assert.Equal(PartFeatures.None, emptyModel.GetPartFeatures());

        var slotOnlyModel = new DstvRecord { Elements = [slottedHole] };
        Assert.False(slotOnlyModel.HasRegularBoreholes());
        Assert.True(slotOnlyModel.HasSlottedBoreholes());

        var contourOnlyModel = new DstvRecord
        {
            Elements =
            [
                Contour.CreateSeveralContours(
                    [
                        new DstvContourPoint(FaceCode.V, 0, 0, false, 0),
                        new DstvContourPoint(FaceCode.None, 100, 0, false, 0),
                    ],
                    ContourType.AK).Single(),
            ],
        };
        Assert.False(contourOnlyModel.HasSawCuts());
        Assert.True(contourOnlyModel.HasRegularCuts());
    }

    [Fact]
    public async Task TestCombinedBlockKindsFromPublicFormatExamples()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/blocks.nc");
        var dstvReader = new DstvReader();

        var model = await dstvReader.ParseAsync(streamReader);
        var contours = model.Elements.OfType<Contour>().ToArray();

        Assert.Equal(2, model.Elements.OfType<DstvHole>().Count());
        Assert.Single(model.Elements.OfType<DstvHole>(), hole => hole is not DstvSlot);
        Assert.Single(model.Elements.OfType<DstvSlot>());
        Assert.Contains(contours, contour => contour.Type == ContourType.AK);
        Assert.Contains(contours, contour => contour.Type == ContourType.IK);
        Assert.Contains(contours, contour => contour.Type == ContourType.PU);
        Assert.Contains(contours, contour => contour.Type == ContourType.KO);
        Assert.Equal("BLOCKS-1 sample text", Assert.Single(model.Elements.OfType<DstvNumeration>()).Text);
        Assert.Equal(2, model.Elements.OfType<DstvCut>().Count());
        Assert.Equal(2, model.Elements.OfType<DstvBend>().Count());

        Assert.True(model.HasRegularBoreholes());
        Assert.True(model.HasSlottedBoreholes());
        Assert.True(model.HasPowderMarkings());
        Assert.True(model.HasPunchMarkings());
        Assert.True(model.HasNumerations());
        Assert.True(model.HasBends());
        Assert.True(model.HasSawCuts());
        Assert.True(model.HasSkewedSawCuts());
        Assert.True(model.HasRegularCuts());
        Assert.True(model.HasSkewedRegularCuts());
        Assert.True(model.HasCurvedRegularCuts());
        Assert.True(model.HasInternalContours());
    }

    [Fact]
    public async Task TestJsonRoundTripPreservesPartAndFeatures()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/blocks.nc");
        var model = await new DstvReader().ParseAsync(streamReader);
        var expectedFeatures = model.GetPartFeatures();
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() },
        };

        var json = JsonSerializer.Serialize<DstvRecord>((DstvRecord)model, options);
        var deserialized = JsonSerializer.Deserialize<DstvRecord>(json, options);

        Assert.NotNull(deserialized);
        Assert.Equal(expectedFeatures, deserialized.GetPartFeatures());
        Assert.Equal(model.Header?.PieceIdentification, deserialized.Header?.PieceIdentification);
        Assert.Equal(model.Elements.Count(), deserialized.Elements.Count());
        Assert.Equal(model.ToSvg(), deserialized.ToSvg());
    }

    private sealed class CancelOnReadTextReader(string content, CancellationTokenSource cancellationSource)
        : TextReader
    {
        private readonly StringReader _reader = new(content);

        public bool ReceivedCancellationToken { get; private set; }

        public override async ValueTask<string?> ReadLineAsync(CancellationToken cancellationToken)
        {
            ReceivedCancellationToken = cancellationToken.CanBeCanceled
                                       && cancellationToken == cancellationSource.Token;
            await cancellationSource.CancelAsync().ConfigureAwait(false);
            return await _reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _reader.Dispose();
            base.Dispose(disposing);
        }
    }

    private sealed class CollectingLogger<T> : ILogger<T>
    {
        public List<(LogLevel Level, EventId EventId, string Message)> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Entries.Add((logLevel, eventId, formatter(state, exception)));
        }
    }

    // /// <summary>
    // ///     Example of an invalid end in the DSTV file
    // ///     Expect a <seealso cref="UnexpectedEndException" />
    // /// </summary>
    // [Fact]
    // public async Task Test_UnexpectedEndException()
    // {
    //     using var streamReader = new StreamReader($"{DataPath}/Data/E8.nc");
    //     var dstvReader = new DstvReader();
    //
    //     var exception = await Assert.ThrowsAsync<UnexpectedEndException>(() => dstvReader.ParseAsync(streamReader));
    //     Assert.Equal(string.Format(Constants.UnexpectedEndExceptionMessage), exception.Message);
    // }
}