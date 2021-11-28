namespace SadCanvas.Shapes;

/// <summary>
/// A primitive square that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Square : Polygon
{
    /// <summary>
    /// Length of the side of the <see cref="Square"/>.
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
    /// <param name="sideLength">Length of the side of the <see cref="Square"/>.</param>
    public Square(Point origin, int sideLength) : base(new Point[4])
    {
        if (sideLength <= 0) throw new ArgumentException("Side length cannot be 0 or negative.");

        Origin = origin;
        SideLength = sideLength;
        Vertices[0] = origin;
        Vertices[1] = origin + (sideLength, 0);
        Vertices[2] = origin + (sideLength, sideLength);
        Vertices[3] = origin + (0, sideLength);
    }

    /// <summary>
    /// Creates an instance of <see cref="Square"/> with the given parameters.
    /// </summary>
    /// <param name="origin">Origin point.</param>
    /// <param name="sideLength">Length of the side of the <see cref="Square"/>.</param>
    /// <param name="lineColor"><see cref="MonoColor"/> of the outline of the <see cref="Square"/>.</param>
    public Square(Point origin, int sideLength, MonoColor lineColor) : this(origin, sideLength)
    {
        LineColor = lineColor;
    }

    /// <summary>
    /// Generates a random <see cref="Square"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Square"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public static Square GetRandomSquare(Canvas canvas, int minLineLength, int maxLineLength) =>
        GetRandomSquare(canvas.Area, minLineLength, maxLineLength);

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
            var origin = Canvas.GetRandomPosition(area);
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
}