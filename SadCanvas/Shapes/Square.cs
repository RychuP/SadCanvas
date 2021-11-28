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
    public Point Position { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Square"/> with the given parameters.
    /// </summary>
    /// <param name="position">Position of the <see cref="Square"/>.</param>
    /// <param name="sideLength">Length of the side of the <see cref="Square"/>.</param>
    public Square(Point position, int sideLength) : base(new Point[4])
    {
        if (sideLength <= 0) throw new ArgumentException("Side length cannot be 0 or negative.");

        Position = position;
        SideLength = sideLength;
        Vertices[0] = position;
        Vertices[1] = position + (sideLength, 0);
        Vertices[2] = position + (sideLength, sideLength);
        Vertices[3] = position + (0, sideLength);
    }

    /// <summary>
    /// Creates an instance of <see cref="Square"/> with the given parameters.
    /// </summary>
    /// <param name="position">Position of the <see cref="Square"/>.</param>
    /// <param name="sideLength">Length of the side of the <see cref="Square"/>.</param>
    /// <param name="lineColor"><see cref="MonoColor"/> of the outline of the <see cref="Square"/>.</param>
    public Square(Point position, int sideLength, MonoColor lineColor) : this(position, sideLength)
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
            var pos = Canvas.GetRandomPosition(area);
            var maxWidth = area.Width - pos.X;
            var maxHeight = area.Height - pos.Y;
            int maxSideLength = Math.Min(maxWidth, maxHeight);
            if (maxSideLength >= minLineLength)
            {
                int sideLength = Canvas.GetRandomInt(minLineLength, maxSideLength);
                if (sideLength <= maxLineLength)
                    return new Square(pos, sideLength, Canvas.GetRandomColor());
            }
        }
    }
}