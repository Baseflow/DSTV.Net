using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSTV;
using DSTV.Enums;
using DSTV.Exceptions;
using DSTV.Implementations;
using Xunit;

namespace DSTVReader.Tests
{
    public class Tests
    {
        private const string DataPath = "../../../";

        /// <summary>
        ///     Example of a valid DSTV file header.
        ///     Expect a valid parse result
        /// </summary>
        [Fact]
        public async Task Test_Body_NC4()
        {
            using var streamReader = new StreamReader($"{DataPath}/Data/RealWorld/4.NC");
            var dstvReader = new DstvReader();

            var dstv = await dstvReader.ParseAsync(streamReader);
            var elements = dstv.Elements;
            Assert.Equal(3, elements.Count());
        }

        /// <summary>
        ///     Example of a valid DSTV file header.
        ///     Expect a valid parse result
        /// </summary>
        [Fact]
        public async Task Test_P1()
        {
            using var streamReader = new StreamReader($"{DataPath}/Data/P1.nc");
            var dstvReader = new DstvReader();

            var header = (await dstvReader.ParseAsync(streamReader)).Header;

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
        ///     Example of a valid DSTV file header.
        ///     Expect a valid parse result
        /// </summary>
        [Fact]
        public async Task Test_PRST37_2()
        {
            using var streamReader = new StreamReader($"{DataPath}/Data/RST37-2.nc");
            var dstvReader = new DstvReader();

            var header = (await dstvReader.ParseAsync(streamReader)).Header;

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
        public async Task Test_FreeTextTooLargeException()
        {
            using var streamReader = new StreamReader($"{DataPath}/Data/E1.nc");
            var dstvReader = new DstvReader();

            var exception =
                await Assert.ThrowsAsync<FreeTextTooLargeException>(() => dstvReader.ParseAsync(streamReader));
            Assert.Equal(string.Format(Constants.FreeTextTooLargeExceptionMessage, 3), exception.Message);
        }

        /// <summary>
        ///     Example of an invalid CodeProfile field (not being recognized as a valid enumeration of type
        ///     <seealso cref="CodeProfile" />
        ///     Expect a <seealso cref="EnumParseException{TEnum}" />
        /// </summary>
        [Fact]
        public async Task Test_EnumParseException()
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
        public async Task Test_MissingStartOfFileException()
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
        public async Task Test_DoubleParseException()
        {
            using var streamReader = new StreamReader($"{DataPath}/Data/E4.nc");
            var dstvReader = new DstvReader();

            var exception = await Assert.ThrowsAsync<DoubleParseException>(() => dstvReader.ParseAsync(streamReader));
            Assert.Equal(string.Format(Constants.DoubleParseExceptionMessage, 11), exception.Message);
        }

        /// <summary>
        ///     Example of an invalid 2x (2 spaces) field in the DSTV file (at line 7)
        ///     Expect a <seealso cref="UnexpectedCharacterException" />
        /// </summary>
        [Fact]
        public async Task Test_UnexpectedCharacterException()
        {
            using var streamReader = new StreamReader($"{DataPath}/Data/E5.nc");
            var dstvReader = new DstvReader();

            var exception =
                await Assert.ThrowsAsync<UnexpectedCharacterException>(() => dstvReader.ParseAsync(streamReader));
            Assert.Equal(string.Format(Constants.UnexpectedCharacterExceptionMessage, 7, ' ', 'k'), exception.Message);
        }

        /// <summary>
        ///     Example of an invalid integer value in the quantity property of the DSTV file
        ///     Expect a <seealso cref="IntegerParseException" />
        /// </summary>
        [Fact]
        public async Task Test_IntegerParseException()
        {
            using var streamReader = new StreamReader($"{DataPath}/Data/E6.nc");
            var dstvReader = new DstvReader();

            var exception = await Assert.ThrowsAsync<IntegerParseException>(() => dstvReader.ParseAsync(streamReader));
            Assert.Equal(string.Format(Constants.IntegerParseExceptionMessage, 8), exception.Message);
        }

        /// <summary>
        ///     Example of an invalid tuple value in the quantity property of the DSTV file
        ///     Expect a <seealso cref="TupleParseException" />
        /// </summary>
        [Fact]
        public async Task Test_TupleParseException()
        {
            using var streamReader = new StreamReader($"{DataPath}/Data/E7.nc");
            var dstvReader = new DstvReader();

            var exception =
                await Assert.ThrowsAsync<TupleParseException<double>>(() => dstvReader.ParseAsync(streamReader));
            Assert.Equal(string.Format(Constants.TupleParseExceptionMessage, 11, "1 to 2", nameof(Double)),
                exception.Message);
        }

        /// <summary>
        ///     Example of an invalid end in the DSTV file
        ///     Expect a <seealso cref="UnexpectedEndException" />
        /// </summary>
        [Fact]
        public async Task Test_UnexpectedEndException()
        {
            using var streamReader = new StreamReader($"{DataPath}/Data/E8.nc");
            var dstvReader = new DstvReader();

            var exception = await Assert.ThrowsAsync<UnexpectedEndException>(() => dstvReader.ParseAsync(streamReader));
            Assert.Equal(string.Format(Constants.UnexpectedEndExceptionMessage), exception.Message);
        }
    }
}