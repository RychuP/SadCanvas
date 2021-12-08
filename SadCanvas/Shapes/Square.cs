namespace SadCanvas.Shapes;

/// <summary>
/// A primitive square <see cref="Shape"/>.
/// </summary>
public class Square : Rectangle
{
    /// <summary>
    /// Creates an instance of <see cref="Square"/> with the given parameters.
    /// </summary>
    /// <param name="origin">Origin point.</param>
    /// <param name="sideLength">Length of each side.</param>
    /// <param name="color">Color of the edges.</param>
    /// <param name="fillColor">Color of the interior area.</param>
    public Square(Point origin, int sideLength, MonoColor? color = null, MonoColor? fillColor = null) : 
        base(origin, sideLength, sideLength, color, fillColor)
    { }

    /// <summary>
    /// Creates an instance of <see cref="Square"/> with the given parameters.
    /// </summary>
    /// <param name="origin">Origin point.</param>
    /// <param name="sideLength">Length of each side.</param>
    /// <param name="randomColors">Colors will be random or default.</param>
    public Square(Point origin, int sideLength, bool randomColors = false) :
        base(origin, sideLength, sideLength, randomColors)
    { }

    /// <inheritdoc/>
    public override Square Clone(Transform? transform = null)
    {
        var square = new Square(Origin, Width, Color, FillColor);
        if (transform is Transform t)
            square.Apply(t);
        return square;
    }

    /// <summary>
    /// Generates a random <see cref="Square"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Square"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    /// <param name="mode">Mode for generating the square.</param>
    /// <remarks>Colors are random by default.</remarks>
    public Square(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength, Mode mode = Mode.Random)
        : base(area, minLineLength, maxLineLength, mode, true)
    { }
}