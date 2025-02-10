using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using DSTV.Net.Implementations;
using System.Globalization;
using DSTV.Net.Data;
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
    public async Task TestP1Header()
    {
        using var streamReader = new StreamReader($"{DataPath}/Data/P1.nc");
        var dstvReader = new DstvReader();

        var parsed = await dstvReader.ParseAsync(streamReader).ConfigureAwait(false);
        var header = parsed.Header;

        Assert.NotNull(header);

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

        var parsed = await dstvReader.ParseAsync(streamReader).ConfigureAwait(false);
        var header = parsed.Header;

        Assert.NotNull(parsed.Elements);
        Assert.Equal(9, parsed.Elements.Count());

        var elements = parsed.Elements.ToArray();
        Assert.True(elements[0] is DstvHole);
        Assert.True(elements[1] is DstvHole);
        Assert.True(elements[2] is DstvHole);
        Assert.True(elements[3] is Contour);
        Assert.True(elements[4] is Contour);
        Assert.True(elements[5] is Contour);
        Assert.True(elements[6] is Contour);
        Assert.True(elements[7] is DstvNumeration);
        Assert.True(elements[8] is DstvNumeration);
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

        var header = (await dstvReader.ParseAsync(streamReader).ConfigureAwait(false)).Header;

        Assert.NotNull(header);

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
            await Assert.ThrowsAsync<FreeTextTooLargeException>(() => dstvReader.ParseAsync(streamReader)).ConfigureAwait(false);

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

        await Assert.ThrowsAsync<EnumParseException<CodeProfile>>(() => dstvReader.ParseAsync(streamReader)).ConfigureAwait(false);
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

        await Assert.ThrowsAsync<MissingStartOfFileException>(() => dstvReader.ParseAsync(streamReader)).ConfigureAwait(false);
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

        var exception = await Assert.ThrowsAsync<DoubleParseException>(() => dstvReader.ParseAsync(streamReader)).ConfigureAwait(false);
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
            await Assert.ThrowsAsync<UnexpectedCharacterException>(() => dstvReader.ParseAsync(streamReader)).ConfigureAwait(false);
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

        var exception = await Assert.ThrowsAsync<IntegerParseException>(() => dstvReader.ParseAsync(streamReader)).ConfigureAwait(false);
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
            await Assert.ThrowsAsync<TupleParseException<double>>(() => dstvReader.ParseAsync(streamReader)).ConfigureAwait(false);
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

        var result = await dstvReader.ParseAsync(streamReader).ConfigureAwait(false);
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