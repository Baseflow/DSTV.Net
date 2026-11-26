using DSTV.Net.Contracts;
using DSTV.Net.Data;
using DSTV.Net.Enums;
using System.Globalization;
using System.Text;

namespace DSTV.Net.Svg;

/// <summary>
/// Renders parsed DSTV models as SVG while allowing callers to replace the generated CSS class names.
/// </summary>
public sealed class DstvSvgExporter
{
    private readonly Dictionary<SvgCssClass, SvgCssClassOptions> _classNames = new()
    {
        [SvgCssClass.Outline] = new("dstv-outline", "fill:none;stroke:#000;stroke-width:.5"),
        [SvgCssClass.Contour] = new("dstv-contour", "fill:none;stroke:#000;stroke-width:.5"),
        [SvgCssClass.OuterContour] = new("dstv-outer-contour", "fill:none;stroke:#000;stroke-width:.5"),
        [SvgCssClass.InnerContour] = new("dstv-inner-contour", "fill:none;stroke:#000;stroke-width:.5"),
        [SvgCssClass.Hole] = new("dstv-hole", string.Empty),
        [SvgCssClass.Slot] = new("dstv-slot", string.Empty),
        [SvgCssClass.Bend] = new("dstv-bend", string.Empty),
        [SvgCssClass.Cut] = new("dstv-cut", string.Empty),
        [SvgCssClass.Marking] = new("dstv-marking", "fill:none;stroke:#000;stroke-width:.5"),
        [SvgCssClass.Powder] = new("dstv-powder", "fill:none;stroke:#000;stroke-width:.5;stroke-dasharray:4 2"),
        [SvgCssClass.Punch] = new("dstv-punch", "fill:none;stroke:#000;stroke-width:.5;stroke-dasharray:1 2"),
        [SvgCssClass.Bevel] = new("dstv-bevel", "stroke-width:1"),
        [SvgCssClass.Text] = new("dstv-text", "fill:#000;stroke:none"),
    };

    /// <summary>Gets the options for a semantic SVG feature.</summary>
    public SvgCssClassOptions this[SvgCssClass cssClass] => _classNames[cssClass];

    /// <summary>Exports one SVG element without a document doctype.</summary>
    public string Export(IDstv model, FaceCode faceCode)
    {
#if NET
        ArgumentNullException.ThrowIfNull(model);
#else
        if (model is null) throw new ArgumentNullException(nameof(model));
#endif
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var width = model.Header?.Length ?? 0;
            var height = faceCode is FaceCode.O or FaceCode.U
                ? model.Header?.FlangeWidth ?? 0
                : model.Header?.ProfileHeight ?? 0;
            var body = string.Concat(model.Elements
                .OrderByDescending(element => element is Contour)
                .Select(element => element.ToSvg(faceCode, this)));

            return $"<svg viewBox=\"0 0 {width} {height}\" width=\"{width}\" height=\"{height}\" xmlns=\"http://www.w3.org/2000/svg\">{Styles()}{body}</svg>";
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    /// <summary>Exports an SVG document including the SVG 1.1 doctype.</summary>
    public string ExportDocument(IDstv model, FaceCode faceCode) =>
        $"<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">{Environment.NewLine}{Export(model, faceCode)}";

    internal string Classes(params ReadOnlySpan<SvgCssClass> cssClasses)
    {
        var result = new StringBuilder();
        foreach (var cssClass in cssClasses)
        {
            if (result.Length != 0) result.Append(' ');
            result.Append(this[cssClass].ClassName);
        }

        return result.ToString();
    }

    private string Styles() =>
        $"<style>{string.Concat(_classNames.Values.Where(static options => !string.IsNullOrWhiteSpace(options.Style)).Select(static options => $".{options.ClassName}{{{options.Style}}}"))}</style>";
}
