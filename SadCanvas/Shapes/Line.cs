namespace SadCanvas.Shapes;

/// <summary>
/// A primitive line that connects two distinct points.
/// </summary>
public class Line : Shape
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
    public override Point[] Vertices { get; init; } = new Point[2];

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

    /// <inheritdoc/>
    public override Line Clone(Transform? transform = null)
    {
        var line = new Line(Start, End, Color);
        if (transform is Transform t)
            Apply(t);
        return line;
    }

    /// <summary>
    /// Length according to euclidean distance formula with the square root.
    /// </summary>
    public double GetLength() => GetLength(Start, End);

    /// <summary>
    /// Length according to euclidean distance formula without the square root.
    /// </summary>
    public double GetMagnitude() => GetLenSquared(Start, End);

    /// <summary>
    /// Returns a unit vector of the line.
    /// </summary>
    public (float X, float Y) GetUnit() => GetUnit(Start, End);

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
        int minLineLengthSquared = minLineLength * minLineLength;
        int maxLineLengthSquared = maxLineLength * maxLineLength;

        if (mode == Mode.Fit) throw new NotImplementedException();
        if (minLineLength <= 0 || maxLineLength <= 0) throw new ArgumentException("Line constraints cannot be 0 or negative.");
        if (maxLineLength < minLineLength) throw new ArgumentException("Max length cannot be smaller than min length.");
        if (GetLenSquared((0, 0), (area.Width - 1, area.Height - 1)) < minLineLengthSquared) throw new ArgumentException("Area diameter cannot be smaller than min line length.");

        while (true)
        {
            Point start = area.GetRandomPosition();
            Point end = area.GetRandomPosition();
            var lengthSquared = GetLenSquared(start, end);
            if (lengthSquared >= minLineLengthSquared && lengthSquared <= maxLineLengthSquared)
                return new Point[] { start, end };
        }
    }

    /// <summary>
    /// Calculates distance between two points using the euclidean formula and the square root.
    /// </summary>
    /// <returns>Distance between points <paramref name="p1"/> and <paramref name="p2"/>.</returns>
    public static double GetLength(Point p1, Point p2) =>
        Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));

    /// <summary>
    /// Calculates distance between two points using the euclidean formula without the square root.
    /// </summary>
    public static double GetLenSquared(Point p1, Point p2) =>
        Point.EuclideanDistanceMagnitude(p1, p2);


    /// <summary>
    /// Returns a unit vector of the line formed by the two points.
    /// </summary>
    /// <param name="p1">First point of the line.</param>
    /// <param name="p2">Second point of the line.</param>
    /// <returns>Unit vector.</returns>
    public static (float X, float Y) GetUnit(Point p1, Point p2)
    {
        float length = (float) GetLength(p1, p2);
        return ((p1.X - p2.X) / length, (p1.Y - p2.Y) / length);
    }
        
}