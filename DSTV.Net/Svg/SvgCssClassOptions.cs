namespace DSTV.Net.Svg;

/// <summary>
/// Defines the class name and raw CSS declarations used for one SVG feature.
/// </summary>
public sealed class SvgCssClassOptions
{
    private string _className;

    internal SvgCssClassOptions(string className, string style)
    {
        _className = className;
        Style = style;
    }

    public string ClassName
    {
        get => _className;
        set
        {
            if (string.IsNullOrWhiteSpace(value)
                || !char.IsLetter(value[0]) && value[0] != '_'
                || value.Any(character =>
                    !char.IsLetterOrDigit(character) && character is not '-' and not '_'))
            {
                throw new ArgumentException(
                    "A CSS class name must start with a letter or underscore and contain only letters, digits, hyphens, or underscores.",
                    nameof(value));
            }

            _className = value;
        }
    }

    /// <summary>
    /// Gets or sets raw CSS declarations. Empty text omits this class from the style element.
    /// </summary>
    public string Style { get; set; }
}
