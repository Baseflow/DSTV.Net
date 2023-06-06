using DSTV.Net.Data;
using DSTV.Net.Enums;
using DSTV.Net.Exceptions;
using DSTV.Net.Extensions;

namespace DSTV.Net.Implementations;

internal sealed class HeaderReader
{
    private const char Space = ' ';
    private const char Comment = '*';

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0051:Method is too long", Justification = "<Pending>")]
    internal static async Task<DstvHeader> ParseAsync(ReaderContext context)
    {
        var reader = context.Source;
        var result = new DstvHeader();

        //2x, a Order identification
        await Parse2XAsync(context).ConfigureAwait(false);
        result.OrderIdentification = await reader.ParseFreeText(context).ConfigureAwait(false);
        //2x, a Drawing identification
        await Parse2XAsync(context).ConfigureAwait(false);
        result.DrawingIdentification = await reader.ParseFreeText(context).ConfigureAwait(false);
        //2x, a Phase identification
        await Parse2XAsync(context).ConfigureAwait(false);
        result.PhaseIdentification = await reader.ParseFreeText(context).ConfigureAwait(false);
        //2x, a Piece identification
        await Parse2XAsync(context).ConfigureAwait(false);
        result.PieceIdentification = await reader.ParseFreeText(context).ConfigureAwait(false);
        //2x, a Steel quality
        await Parse2XAsync(context).ConfigureAwait(false);
        result.SteelQuality = await reader.ParseFreeText(context).ConfigureAwait(false);
        //2x, i Quantity of pieces
        await Parse2XAsync(context).ConfigureAwait(false);
        result.QuantityOfPieces = await reader.ParseInteger(context).ConfigureAwait(false);
        //2x, a Profile
        await Parse2XAsync(context).ConfigureAwait(false);
        result.Profile = await reader.ParseFreeText(context).ConfigureAwait(false);
        //2x, a Code Profile
        await Parse2XAsync(context).ConfigureAwait(false);
        result.CodeProfile = await reader.ParseEnum<CodeProfile>(context).ConfigureAwait(false);
        //2x, f [,f ] Length , Saw Length
        await Parse2XAsync(context).ConfigureAwait(false);
        var lengths = await reader.ParseTupleDouble(context).ConfigureAwait(false);
        result.Length = lengths.Item1;
        result.SawLength = lengths.Item2;
        //2x, f Profile height
        await Parse2XAsync(context).ConfigureAwait(false);
        result.ProfileHeight = await reader.ParseDouble(context).ConfigureAwait(false);
        //2x, f Flange width
        await Parse2XAsync(context).ConfigureAwait(false);
        result.FlangeWidth = await reader.ParseDouble(context).ConfigureAwait(false);
        //2x, f Flange thickness
        await Parse2XAsync(context).ConfigureAwait(false);
        result.FlangeThickness = await reader.ParseDouble(context).ConfigureAwait(false);
        //2x, f Web thickness
        await Parse2XAsync(context).ConfigureAwait(false);
        result.WebThickness = await reader.ParseDouble(context).ConfigureAwait(false);
        //2x, f Radius
        await Parse2XAsync(context).ConfigureAwait(false);
        result.Radius = await reader.ParseDouble(context).ConfigureAwait(false);
        //2x, f Weight by meter
        await Parse2XAsync(context).ConfigureAwait(false);
        result.WeightByMeter = await reader.ParseDouble(context).ConfigureAwait(false);
        //2x, f Painting surface by meter < m2/m >
        await Parse2XAsync(context).ConfigureAwait(false);
        result.PaintingSurfaceByMeter = await reader.ParseDouble(context).ConfigureAwait(false);
        //2x, f Web Start Cut
        await Parse2XAsync(context).ConfigureAwait(false);
        result.WebStartCut = await reader.ParseDouble(context).ConfigureAwait(false);
        //2x, f Web End Cut
        await Parse2XAsync(context).ConfigureAwait(false);
        result.WebEndCut = await reader.ParseDouble(context).ConfigureAwait(false);
        //2x, f Flange Start Cut
        await Parse2XAsync(context).ConfigureAwait(false);
        result.FlangeStartCut = await reader.ParseDouble(context).ConfigureAwait(false);
        //2x, f Flange End Cut
        await Parse2XAsync(context).ConfigureAwait(false);
        result.FlangeEndCut = await reader.ParseDouble(context).ConfigureAwait(false);
        // //2x, a Text info on piece
        result.Text1InfoOnPiece = await reader.ParseFreeText(context).ConfigureAwait(false);
        // //2x, a Text info on piece
        result.Text2InfoOnPiece = await reader.ParseFreeText(context).ConfigureAwait(false);
        //2x, a Text info on piece
        result.Text3InfoOnPiece = await reader.ParseFreeText(context).ConfigureAwait(false);
        //2x, a Text info on piece
        result.Text4InfoOnPiece = await reader.ParseFreeText(context).ConfigureAwait(false);

        return result;
    }

    /// <summary>
    ///     Reads two spaces. Throws an error if other things are read.
    /// </summary>
    /// <param name="context">The context, containing the source TextReader and lineNumber information</param>
    /// <exception cref="UnexpectedEndException">Is thrown whenever the end of the data is unexpectedly encountered</exception>
    /// <exception cref="UnexpectedCharacterException">Is throw whenever an unexpected character is encountered</exception>
    private static async Task Parse2XAsync(ReaderContext context)
    {
        var buffer = new char[2];

        var cnt = await context.Source.ReadAsync(buffer, 0, 2).ConfigureAwait(false);

        // Test whether there were actually two characters read.
        if (cnt != 2) throw new UnexpectedEndException(context);

        // If this is a comment, ignore the comment
        if (buffer[0] == Comment && buffer[1] == Comment)
        {
            await context.Source.ReadLineAsync().ConfigureAwait(false);
            context.IncrementLineNumber();
            await Parse2XAsync(context).ConfigureAwait(false);
            return;
        }

        if (buffer[0] != Space) throw new UnexpectedCharacterException(context, Space, buffer[0]);

        if (buffer[1] != Space) throw new UnexpectedCharacterException(context, Space, buffer[1]);
    }
}