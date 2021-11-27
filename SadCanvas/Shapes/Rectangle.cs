namespace SadCanvas.Shapes;

/// <summary>
/// A primitive rectangle that can be drawn on <see cref="Canvas"/>.
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
    /// Creates an instance of <see cref="Rectangle"/> with the given parameters.
    /// </summary>
    /// <param name="position">Position of the <see cref="Rectangle"/>.</param>
    /// <param name="width">Width of the <see cref="Rectangle"/>.</param>
    /// <param name="height">Height of the <see cref="Rectangle"/>.</param>
    public Rectangle(Point position, int width, int height)
    {
        Position = position;
        Vertices = new Point[4]
        {
            Position,
            Position + (width, 0),
            Position + (width, height),
            Position + (0, height)
        };
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
    public Rectangle(Canvas canvas, int minLineLength = MinLength, int maxLineLength = MaxLength)
    {
        while (true)
        {
            var pos = canvas.GetRandomPosition();
            var maxWidth = canvas.Width - pos.X;
            var maxHeight = canvas.Height - pos.Y;
            if (maxWidth >= minLineLength && maxHeight >= minLineLength)
            {
                int width = Game.Instance.Random.Next(minLineLength, maxWidth);
                int height = Game.Instance.Random.Next(minLineLength, maxHeight);

                if (width <= maxLineLength && height <= maxLineLength)
                {
                    Position = pos;
                    Vertices = new Point[]
                    {
                        Position,
                        Position + (width, 0),
                        Position + (width, height),
                        Position + (0, height)
                    };
                    LineColor = Canvas.GetRandomColor();
                    break;
                }
            }
        }
    }
}