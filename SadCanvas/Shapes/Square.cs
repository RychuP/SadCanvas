namespace SadCanvas.Shapes;

/// <summary>
/// A primitive square that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Square : Polygon
{
    /// <summary>
    /// Length of each side.
    /// </summary>
    public int SideLength { get; init; }

    /// <summary>
    /// Start position (top left) to calculate the rest of vertices from.
    /// </summary>
    public Point Origin { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Square"/> with the given parameters.
    /// </summary>
    /// <param name="origin">Origin point.</param>
    /// <param name="sideLength">Length of each side.</param>
    public Square(Point origin, int sideLength) :
        this(origin, sideLength, DefaultColor)
    { }

    /// <summary>
    /// Creates an instance of <see cref="Square"/> with the given parameters.
    /// </summary>
    /// <param name="origin">Origin point.</param>
    /// <param name="sideLength">Length of each side.</param>
    /// <param name="color">Color of the edges.</param>
    public Square(Point origin, int sideLength, MonoColor color) : 
        base(GetVertices(origin, sideLength), color)
    {
        (Origin, SideLength) = (Origin, SideLength);
    }

    /// <summary>
    /// Generates a random <see cref="Square"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Square"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public static Square GetRandomSquare(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength) 
    {
        if (minLineLength <= 0 || maxLineLength <= 0) throw new ArgumentException("Square constraints cannot be 0 or negative.");
        if (maxLineLength < minLineLength) throw new ArgumentException("Max length cannot be smaller than min length.");
        if (area.Width < minLineLength || area.Height < minLineLength) throw new ArgumentException("Area width cannot be smaller than min length.");

        while (true)
        {
            var origin = area.GetRandomPosition();
            var maxWidth = area.Width - origin.X;
            var maxHeight = area.Height - origin.Y;
            int maxSideLength = Math.Min(maxWidth, maxHeight);
            if (maxSideLength >= minLineLength)
            {
                int sideLength = Canvas.GetRandomInt(minLineLength, maxSideLength);
                if (sideLength <= maxLineLength)
                    return new Square(origin, sideLength, Canvas.GetRandomColor());
            }
        }
    }

    static Point[] GetVertices(Point origin, int sideLength)
    {
        if (sideLength <= 0) throw new ArgumentException("Side length cannot be 0 or negative.");

        return new Point[]
        {
            origin,
            origin + (sideLength, 0),
            origin + (sideLength, sideLength),
            origin + (0, sideLength)
        };
    }
}