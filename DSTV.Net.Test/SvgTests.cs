using Xunit;
using DSTV.Net.Implementations;
using Xunit.Abstractions;

namespace DSTV.Net.Test;

/// <summary>
///     this test class makes sure all the svg generation works as expected.
///     testing svg generation will also ensure all geometrical data is still parsed correctly.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1515", Justification = "Test classes must be public for xUnit")]
public class SvgTests
{
    [Theory]
    [InlineData("../../../Data/product2.NC1")]
    [InlineData("../../../Data/product3.NC1")]
    public async Task TestGenerateProduct2(string file) {
        using var streamReader = new StreamReader(file);
        var dstvReader = new DstvReader();
        var dstv = await dstvReader.ParseAsync(streamReader);
        var svg = dstv.ToSvg();
        var svgFile = Path.ChangeExtension(file, ".svg");
        Assert.Equal(await File.ReadAllTextAsync(svgFile), svg);
    }
}
