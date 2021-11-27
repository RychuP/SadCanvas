namespace SadCanvas.Shapes;

/// <summary>
/// A primitive square that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Square : Rectangle
{
    /// <summary>
    /// Length of the side of the <see cref="Square"/>.
    /// </summary>
    public int SideLength { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Square"/> with the given parameters.
    /// </summary>
    /// <param name="position">Position of the <see cref="Square"/>.</param>
    /// <param name="sideLength">Length of the side of the <see cref="Square"/>.</param>
    public Square(Point position, int sideLength) : base(position, sideLength, sideLength)
    {
        SideLength = sideLength;
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
    public Square(Canvas canvas, int minLineLength = MinLength, int maxLineLength = MaxLength) : 
        base(canvas, minLineLength, maxLineLength)
    {
        SideLength = Width;
        Vertices = new Point[]
        {
            Position,
            Position + (Width, 0),
            Position + (Width, Width),
            Position + (0, Width)
        };
    }
}