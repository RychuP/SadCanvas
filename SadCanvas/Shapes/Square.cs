namespace SadCanvas.Shapes;

/// <summary>
/// A primitive square <see cref="Shape"/>.
/// </summary>
public record Square : Rectangle
{
    /// <summary>
    /// Creates an instance of <see cref="Square"/> with the given parameters.
    /// </summary>
    /// <param name="origin">Origin point.</param>
    /// <param name="sideLength">Length of each side.</param>
    /// <param name="color">Color of the edges.</param>
    public Square(Point origin, int sideLength, MonoColor? color = null) : 
        base(origin, sideLength, sideLength, color)
    { }

    /// <summary>
    /// Generates a random <see cref="Square"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Square"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    /// <param name="mode">Mode for generating an instance.</param>
    /// <param name="color">Color of the square.</param>
    public Square(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength, 
        Mode mode = Mode.Random, MonoColor? color = null) 
        : base(area, minLineLength, maxLineLength, mode, color, true)
    {
        FillColor = Canvas.GetRandomColor();
    }
}