using Xunit;
using DSTV.Net.Implementations;
using DSTV.Net.Enums;
using DSTV.Net.Svg;
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
        var exporter = new DstvSvgExporter();
        var svg = exporter.Export(dstv, FaceCode.V);

        Assert.StartsWith("<svg ", svg, StringComparison.Ordinal);
        Assert.Contains("class=\"dstv-contour dstv-outer-contour\"", svg, StringComparison.Ordinal);
        Assert.DoesNotContain("fill=\"white\"", svg, StringComparison.Ordinal);
        Assert.Contains("<style>", svg, StringComparison.Ordinal);
    }

    [Fact]
    public async Task TestSvgUsesRequestedFace()
    {
        using var streamReader = new StreamReader("../../../Data/blocks.nc");
        var model = await new DstvReader().ParseAsync(streamReader);

        var svg = new DstvSvgExporter().Export(model, FaceCode.O);

        Assert.DoesNotContain("class=\"dstv-contour", svg, StringComparison.Ordinal);
        Assert.DoesNotContain("class=\"dstv-hole", svg, StringComparison.Ordinal);
        Assert.DoesNotContain("class=\"dstv-text", svg, StringComparison.Ordinal);
    }

    [Fact]
    public async Task TestSvgIncludesEverySupportedBlock()
    {
        using var streamReader = new StreamReader("../../../Data/blocks.nc");
        var model = await new DstvReader().ParseAsync(streamReader);

        var svg = new DstvSvgExporter().Export(model, FaceCode.V);

        Assert.Contains("dstv-outer-contour", svg, StringComparison.Ordinal);
        Assert.Contains("dstv-inner-contour", svg, StringComparison.Ordinal);
        Assert.Contains("dstv-powder", svg, StringComparison.Ordinal);
        Assert.Contains("dstv-punch", svg, StringComparison.Ordinal);
        Assert.Contains("dstv-text", svg, StringComparison.Ordinal);
        Assert.Contains("dstv-hole", svg, StringComparison.Ordinal);
        Assert.Contains("dstv-slot", svg, StringComparison.Ordinal);
        Assert.Contains("dstv-cut", svg, StringComparison.Ordinal);
        Assert.Contains("dstv-bend", svg, StringComparison.Ordinal);
        Assert.Contains("BLOCKS-1 sample text", svg, StringComparison.Ordinal);
    }

    [Fact]
    public async Task TestSvgExporterSupportsClassOverridesAndDocumentDoctype()
    {
        using var streamReader = new StreamReader("../../../Data/blocks.nc");
        var model = await new DstvReader().ParseAsync(streamReader);
        var exporter = new DstvSvgExporter();
        exporter[SvgCssClass.Contour].ClassName = "project-contour";
        exporter[SvgCssClass.Contour].Style = "fill:#fafafa;stroke:navy";
        exporter[SvgCssClass.Text].ClassName = "project-text";

        var svg = exporter.ExportDocument(model, FaceCode.V);

        Assert.StartsWith("<!DOCTYPE svg ", svg, StringComparison.Ordinal);
        Assert.Contains("class=\"project-contour dstv-outer-contour\"", svg, StringComparison.Ordinal);
        Assert.Contains("class=\"project-text\"", svg, StringComparison.Ordinal);
        Assert.Contains(".project-contour{fill:#fafafa;stroke:navy}", svg, StringComparison.Ordinal);
        Assert.Contains(".project-text{fill:#000;stroke:none}", svg, StringComparison.Ordinal);
        Assert.Contains(".dstv-outer-contour{fill:none;stroke:#000;stroke-width:.5}", svg, StringComparison.Ordinal);
        Assert.Contains(".dstv-inner-contour{fill:none;stroke:#000;stroke-width:.5}", svg, StringComparison.Ordinal);
        Assert.DoesNotContain(".dstv-hole{", svg, StringComparison.Ordinal);
        Assert.Contains("class=\"dstv-hole dstv-outline\"", svg, StringComparison.Ordinal);
    }

    [Fact]
    public void TestSvgExporterRejectsInvalidClassNames()
    {
        var exporter = new DstvSvgExporter();

        Assert.Throws<ArgumentException>(() => exporter[SvgCssClass.Hole].ClassName = "hole name");
        Assert.Throws<ArgumentException>(() => exporter[SvgCssClass.Hole].ClassName = "1hole");
    }
}
