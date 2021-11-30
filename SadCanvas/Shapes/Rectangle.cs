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
    /// <param name="color">Color of the edges.</param>
    public Rectangle(Point origin, int width, int height, MonoColor? color = null) : 
        base(GetVertices(origin, width, height), color)
    {
        Origin = origin;
        (Width, Height) = (width, height);
    }

    /// <summary>
    /// Generates a <see cref="Rectangle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Rectangle"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    /// <param name="color">Color of the rectangle.</param>
    /// <param name="mode">Mode for generating an instance.</param>
    /// <param name="square">Make each side equal length.</param>
    public Rectangle(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength, 
        Mode mode = Mode.Random, MonoColor? color = null, bool square = false) :
        base(GenerateRectangle(area, minLineLength, maxLineLength, minLineLength, maxLineLength, mode, square),
            color is null ? Canvas.GetRandomColor() : color.Value)
    {
        FillColor = Canvas.GetRandomColor();
    }

    /// <summary>
    /// Generates a <see cref="Rectangle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Rectangle"/> for.</param>
    /// <param name="maxWidth">Maximum width.</param>
    /// <param name="minWidth">Minimum width.</param>
    /// <param name="maxHeight">Maximum height.</param>
    /// <param name="minHeight">Minimum height.</param>
    /// <param name="color">Color of the rectangle.</param>
    /// <param name="mode">Mode for generating an instance.</param>
    /// <param name="square">Make each side equal length.</param>
    public Rectangle(SadRogue.Primitives.Rectangle area, int minWidth, int maxWidth, int minHeight, int maxHeight,
        Mode mode = Mode.Random, MonoColor? color = null, bool square = false) :
        base(GenerateRectangle(area, minWidth, maxWidth, minHeight, maxHeight, mode, square), 
            color is null ? Canvas.GetRandomColor() : color.Value)
    {
        FillColor = Canvas.GetRandomColor();
    }

    static Point[] GenerateRectangle(SadRogue.Primitives.Rectangle area, int minWidth, int maxWidth, int minHeight, int maxHeight,
        Mode mode = Mode.Random, bool square = false)
    {
        if (mode == Mode.Fit) throw new NotImplementedException();
        if (minWidth <= 0 || minHeight <= 0) throw new ArgumentException("Rectangle constraints cannot be 0 or negative.");
        if (maxWidth < minWidth) throw new ArgumentException("Max width cannot be smaller than min.");
        if (maxHeight < minHeight) throw new ArgumentException("Max height cannot be smaller than min.");
        if (area.Width < maxWidth || area.Height < maxHeight)
        {
            maxWidth = area.Width;
            maxHeight = area.Height;
        }

        while (true)
        {
            var origin = area.GetRandomPosition();
            var maxWidthFromPos = area.Width - origin.X;
            var maxHeightFromPos = area.Height - origin.Y;
            if (!square)
            {
                if (maxWidthFromPos >= minWidth && maxHeightFromPos >= minHeight)
                {
                    int width = Canvas.GetRandomInt(minWidth, maxWidthFromPos);
                    int height = Canvas.GetRandomInt(minHeight, maxHeightFromPos);

                    if (width <= maxWidth && height <= maxHeight)
                        return GetVertices(origin, width, height);
                }
            }
            else
            {
                int maxSideLength = Math.Min(maxWidthFromPos, maxHeightFromPos);
                if (maxSideLength >= minWidth)
                {
                    int sideLength = Canvas.GetRandomInt(minWidth, maxSideLength);
                    if (sideLength <= maxWidth)
                        return GetVertices(origin, sideLength, sideLength);
                }
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