namespace SadCanvas.Shapes;

/// <summary>
/// A primitive line that connects two distinct points.
/// </summary>
public class Line : Shape
{
    /// <summary>
    /// Start point.
    /// </summary>
    public Point Start => Vertices[0].ToSadPoint();

    /// <summary>
    /// End point.
    /// </summary>
    public Point End => Vertices[1].ToSadPoint();

    /// <inheritdoc/>
    public override Vector2[] Vertices { get; init; } = new Vector2[2];

    /// <summary>
    /// Creates an instance of a <see cref="Line"/> with the given parameters.
    /// </summary>
    /// <param name="start">Start <see cref="Point"/> for the line.</param>
    /// <param name="end">End <see cref="Point"/> for the line.</param>
    /// <param name="color"><see cref="MonoColor"/> for the line.</param>
    public Line(Point start, Point end, MonoColor? color = null) : 
        this(start.ToVector(), end.ToVector(), color)
    { }

    /// <summary>
    /// Creates an instance of a <see cref="Line"/> with the given parameters.
    /// </summary>
    /// <param name="start">Start <see cref="Vector2"/> for the line.</param>
    /// <param name="end">End <see cref="Vector2"/> for the line.</param>
    /// <param name="color"><see cref="MonoColor"/> for the line.</param>
    public Line(Vector2 start, Vector2 end, MonoColor? color = null) :
        base(color)
    {
        if (start == end) throw new ArgumentException("Line constructor requires two distinct points to create an instance.");
        (Vertices[0], Vertices[1]) = (start, end);
    }

    // used by the random line generator constructor
    Line(Point[] vertices, MonoColor? color = null) :
        this(vertices[0], vertices[1], color)
    { }

    /// <summary>
    /// Generates a random <see cref="Line"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area"><see cref="SadRogue.Primitives.Rectangle"/> to generate a random <see cref="Line"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    /// <param name="color">Color of the line.</param>
    /// <param name="mode">Mode for generating an instance.</param>
    public Line(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength,
        Mode mode = Mode.Random, MonoColor? color = null) :
        this(GetRandomLine(area, minLineLength, maxLineLength, mode),
            color is null ? Canvas.GetRandomColor() : color.Value)
    { }

    /// <inheritdoc/>
    public override Line Clone(Transform? transform = null)
    {
        var line = new Line(Start, End, Color);
        if (transform is Transform t)
            Apply(t);
        return line;
    }

    /// <summary>
    /// Returns the length of the line.
    /// </summary>
    public double GetLength() =>
        GetVector().Length();

    /// <summary>
    /// Returns the squared length of the line.
    /// </summary>
    public double GetLengthSquared() =>
        GetVector().LengthSquared();

    /// <summary>
    /// Returns the unit vector of the line.
    /// </summary>
    public Vector2 GetUnitVector()
    {
        Vector2 v = GetVector();
        v.Normalize();
        return v;
    }

    /// <summary>
    /// Returns the vector of the line (End - Start).
    /// </summary>
    public Vector2 GetVector() =>
        Vertices[1] - Vertices[0];

    /// <inheritdoc/>
    public override SadRogue.Primitives.Rectangle Bounds => 
        new(Left, Top, Right - Left + 1, Bottom - Top + 1);

    /// <inheritdoc/>
    public override int Left => Math.Min(Start.X, End.X);

    /// <inheritdoc/>
    public override int Right => Math.Max(Start.X, End.X);

    /// <inheritdoc/>
    public override int Top => Math.Min(Start.Y, End.Y);

    /// <inheritdoc/>
    public override int Bottom => Math.Max(Start.Y, End.Y);

    static Point[] GetRandomLine(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength,
        Mode mode = Mode.Random)
    {
        int minLineLengthSquared = minLineLength * minLineLength;
        int maxLineLengthSquared = maxLineLength * maxLineLength;
        var areaDiameter = new Line((0, 0), (area.Width - 1, area.Height - 1));

        if (mode == Mode.Fit) throw new NotImplementedException();
        if (minLineLength <= 0 || maxLineLength <= 0) throw new ArgumentException("Line constraints cannot be 0 or negative.");
        if (maxLineLength < minLineLength) throw new ArgumentException("Max length cannot be smaller than min length.");
        if (areaDiameter.GetLengthSquared() < minLineLengthSquared) throw new ArgumentException("Area diameter cannot be smaller than min line length.");

        while (true)
        {
            Point start = area.GetRandomPosition();
            Point end = area.GetRandomPosition();
            var lengthSquared = new Line(start, end).GetLengthSquared();
            if (lengthSquared >= minLineLengthSquared && lengthSquared <= maxLineLengthSquared)
                return new Point[] { start, end };
        }
    }
}