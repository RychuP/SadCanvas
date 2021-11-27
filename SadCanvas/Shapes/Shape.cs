namespace SadCanvas.Shapes;

/// <summary>
/// A primitive shape that can be drawn on <see cref="Canvas"/>.
/// </summary>
public abstract record Shape()
{
    /// <summary>
    /// Minimum line/radius/side length for generating a random instance of <see cref="Shape"/>.
    /// </summary>
    protected const int MinLength = 20;

    /// <summary>
    /// Maximum line/radius/side length for generating a random instance of <see cref="Shape"/>.
    /// </summary>
    protected const int MaxLength = 200;

    /// <summary>
    /// A primitive shape with the given line color that can be drawn on <see cref="Canvas"/>.
    /// </summary>
    /// <param name="lineColor"></param>
    public Shape(MonoColor lineColor) : this()
    {
        LineColor = lineColor;
    }

    /// <summary>
    /// A primitive shape of given colors that can be drawn on <see cref="Canvas"/>.
    /// </summary>
    /// <param name="lineColor"></param>
    /// <param name="fillColor"></param>
    public Shape(MonoColor lineColor, MonoColor fillColor) : this(lineColor)
    {
        FillColor = fillColor;
    }

    /// <summary>
    /// <see cref="MonoColor"/> used in drawing the outline of this <see cref="Shape"/>.
    /// </summary>
    public MonoColor LineColor { get; init; } = MonoColor.White;

    /// <summary>
    /// <see cref="MonoColor"/> used to fill the area of this <see cref="Shape"/>.
    /// </summary>
    public MonoColor FillColor { get; init; } = MonoColor.White;
}