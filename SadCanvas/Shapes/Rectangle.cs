namespace SadCanvas.Shapes;

/// <summary>
/// A primitive rectangular <see cref="Shape"/>.
/// </summary>
public record Rectangle : Polygon
{
    /// <summary>
    /// Length of the horizontal side.
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// Length of the vertical side.
    /// </summary>
    public int Height { get; init; }

    /// <summary>
    /// Start position (top left) from which the rest of the vertices was generated from.
    /// </summary>
    public Point Origin { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Rectangle"/> with the given parameters.
    /// </summary>
    /// <param name="origin">Origin point.</param>
    /// <param name="width">Length of the horizontal side.</param>
    /// <param name="height">Length of the vertical side.</param>
    public Rectangle(Point origin, int width, int height) : 
        this(origin, width, height, DefaultColor) 
    { }

    /// <summary>
    /// Creates an instance of <see cref="Rectangle"/> with the given parameters.
    /// </summary>
    /// <param name="origin">Origin point.</param>
    /// <param name="width">Length of the horizontal side.</param>
    /// <param name="height">Length of the vertical side.</param>
    /// <param name="color">Color of the edges.</param>
    public Rectangle(Point origin, int width, int height, MonoColor color) : 
        base(GetVertices(origin, width, height), color)
    {
        Origin = origin;
        (Width, Height) = (width, height);
    }

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
            var origin = area.GetRandomPosition();
            var maxWidth = area.Width - origin.X;
            var maxHeight = area.Height - origin.Y;
            if (maxWidth >= minLineLength && maxHeight >= minLineLength)
            {
                int width = Canvas.GetRandomInt(minLineLength, maxWidth);
                int height = Canvas.GetRandomInt(minLineLength, maxHeight);

                if (width <= maxLineLength && height <= maxLineLength)
                    return new Rectangle(origin, width, height, Canvas.GetRandomColor());
            }
        }
    }

    static Point[] GetVertices(Point origin, int width, int height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentException("Width and height cannot be 0 or negative.");

        return new Point[]
        {
            origin,
            origin + (width, 0),
            origin + (width, height),
            origin + (0, height)
        };
    }
}