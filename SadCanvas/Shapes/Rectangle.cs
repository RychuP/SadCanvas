namespace SadCanvas.Shapes;

/// <summary>
/// A primitive rectangle.
/// </summary>
public record Rectangle : Polygon
{
    /// <summary>
    /// Width of the <see cref="Rectangle"/>.
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// Height of the <see cref="Rectangle"/>.
    /// </summary>
    public int Height { get; init; }

    /// <summary>
    /// Start position (top left) to calculate the rest of vertices from.
    /// </summary>
    public Point Position { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Rectangle"/> with the given parameters.
    /// </summary>
    /// <param name="position">Position of the <see cref="Rectangle"/>.</param>
    /// <param name="width">Width of the <see cref="Rectangle"/>.</param>
    /// <param name="height">Height of the <see cref="Rectangle"/>.</param>
    public Rectangle(Point position, int width, int height) : base(new Point[4])
    {
        if (width <= 0 || height <= 0) throw new ArgumentException("Width and height cannot be 0 or negative.");

        Position = position;
        (Width, Height) = (width, height);
        Vertices[0] = position;
        Vertices[1] = position + (width, 0);
        Vertices[2] = position + (width, height);
        Vertices[3] = position + (0, height);
    }

    /// <summary>
    /// Creates an instance of <see cref="Rectangle"/> with the given parameters.
    /// </summary>
    /// <param name="position">Position of the <see cref="Rectangle"/>.</param>
    /// <param name="width">Width of the <see cref="Rectangle"/>.</param>
    /// <param name="height">Height of the <see cref="Rectangle"/>.</param>
    /// <param name="lineColor"><see cref="MonoColor"/> of the outline of the <see cref="Rectangle"/>.</param>
    public Rectangle(Point position, int width, int height, MonoColor lineColor) : this(position, width, height)
    {
        LineColor = lineColor;
    }

    /// <summary>
    /// Generates a random <see cref="Rectangle"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Rectangle"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public static Rectangle GetRandomRectangle(Canvas canvas, int minLineLength, int maxLineLength) =>
        GetRandomRectangle(canvas.Area, minLineLength, maxLineLength);

    /// <summary>
    /// Generates a random <see cref="Rectangle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Rectangle"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public static Rectangle GetRandomRectangle(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength)
    {
        if (minLineLength <= 0 || maxLineLength <= 0) throw new ArgumentException("Rectangle constraints cannot be 0 or negative.");
        if (maxLineLength < minLineLength) throw new ArgumentException("Max length cannot be smaller than min length.");
        if (area.Width < minLineLength || area.Height < minLineLength) throw new ArgumentException("Area width cannot be smaller than min length.");

        while (true)
        {
            var pos = Canvas.GetRandomPosition(area);
            var maxWidth = area.Width - pos.X;
            var maxHeight = area.Height - pos.Y;
            if (maxWidth >= minLineLength && maxHeight >= minLineLength)
            {
                int width = Canvas.GetRandomInt(minLineLength, maxWidth);
                int height = Canvas.GetRandomInt(minLineLength, maxHeight);

                if (width <= maxLineLength && height <= maxLineLength)
                    return new Rectangle(pos, width, height, Canvas.GetRandomColor());
            }
        }
    }
}