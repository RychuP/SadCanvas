namespace SadCanvas.Shapes;

/// <summary>
/// A primitive line that connects two distinct points.
/// </summary>
public record Line : Shape
{
    /// <summary>
    /// Start point.
    /// </summary>
    public Point Start => Vertices[0];

    /// <summary>
    /// End point.
    /// </summary>
    public Point End => Vertices[1];

    /// <inheritdoc/>
    public override Point[] Vertices { get; } = new Point[2];

    /// <summary>
    /// Creates an instance of a <see cref="Line"/> with the given parameters.
    /// </summary>
    /// <param name="start">Start <see cref="Point"/> for the line.</param>
    /// <param name="end">End <see cref="Point"/> for the line.</param>
    /// <param name="color"><see cref="MonoColor"/> for the line.</param>
    public Line(Point start, Point end, MonoColor? color = null) : 
        this(new Point[] { start, end }, color)
    { }

    /// <summary>
    /// Creates an instance of a <see cref="Line"/> with the given parameters.
    /// </summary>
    /// <param name="vertices">Array with vertices (needs 2 distinct points).</param>
    /// <param name="color"><see cref="MonoColor"/> for the line.</param>
    public Line(Point[] vertices, MonoColor? color = null) : 
        base(color)
    {
        if (vertices.Length != 2) throw new ArgumentException("Line constructor takes only two vertices to create a line.");
        if (vertices[0] == vertices[1]) throw new ArgumentException("Line constructor requires two distinct points to create an instance.");
        Vertices = vertices;
    }

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
        this(Generateline(area, minLineLength, maxLineLength, mode),
            color is null ? Canvas.GetRandomColor() : color.Value)
    { }

    /// <summary>
    /// Length according to pythagorean distance formula with the square root.
    /// </summary>
    public double Length => GetDistance(Start, End);

    /// <summary>
    /// Length according to euclidean distance formula without the square root.
    /// </summary>
    public double DistanceMagnitude => Point.EuclideanDistanceMagnitude(Start, End);

    /// <inheritdoc/>
    public override void Rotate(float angle)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void Scale(float scale)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void Translate(Point vector)
    {
        Vertices[0] += vector;
        Vertices[1] += vector;
    }

    /// <inheritdoc/>
    public override Point Center => (End - Start) / 2;

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

    static Point[] Generateline(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength,
        Mode mode = Mode.Random)
    {
        if (mode == Mode.Fit) throw new NotImplementedException();
        if (minLineLength <= 0 || maxLineLength <= 0) throw new ArgumentException("Line constraints cannot be 0 or negative.");
        if (maxLineLength < minLineLength) throw new ArgumentException("Max length cannot be smaller than min length.");
        if (GetDistance((0, 0), (area.Width - 1, area.Height - 1)) < minLineLength) throw new ArgumentException("Area diameter cannot be smaller than min line length.");

        while (true)
        {
            Point start = area.GetRandomPosition();
            Point end = area.GetRandomPosition();
            var length = GetDistance(start, end);
            if (length >= minLineLength && length <= maxLineLength)
                return new Point[] { start, end };
        }
    }

    /// <summary>
    /// Calculates distance between two points using the pythagorean formula and the square root.
    /// </summary>
    /// <returns>Distance between points <paramref name="p1"/> and <paramref name="p2"/>.</returns>
    public static double GetDistance(Point p1, Point p2) =>
        Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
}