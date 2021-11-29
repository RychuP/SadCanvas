namespace SadCanvas.Shapes;

/// <summary>
/// A primitive line that connects two distinct points.
/// </summary>
public record Line : Shape
{
    double? _length;
    double? _euclideanLength;
    Point? _center;

    /// <summary>
    /// Start point.
    /// </summary>
    public Point Start { get; init; }

    /// <summary>
    /// End point.
    /// </summary>
    public Point End { get; init; }

    /// <summary>
    /// Calculates distance between two points using a Pythagoras formula and a square root.
    /// </summary>
    /// <returns>Distance between points <paramref name="p1"/> and <paramref name="p2"/>.</returns>
    public static double GetDistance(Point p1, Point p2) => 
        Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));

    /// <summary>
    /// Creates an instance of a <see cref="Line"/> with the given parameters.
    /// </summary>
    /// <param name="start">Start <see cref="Point"/> for the line.</param>
    /// <param name="end">End <see cref="Point"/> for the line.</param>
    public Line(Point start, Point end) : 
        this(start, end, DefaultColor) 
    { }

    /// <summary>
    /// Creates an instance of a <see cref="Line"/> with the given parameters.
    /// </summary>
    /// <param name="start">Start <see cref="Point"/> for the line.</param>
    /// <param name="end">End <see cref="Point"/> for the line.</param>
    /// <param name="color"><see cref="MonoColor"/> for the line.</param>
    public Line(Point start, Point end, MonoColor color) : 
        base(color)
    {
        if (start == end) throw new ArgumentException("Line needs two distinct points.");
        (Start, End) = (start, end);
    }

    /// <summary>
    /// Length according to pythagorean distance formula with the square root.
    /// </summary>
    public double Length
    {
        get
        {
            if (_length.HasValue) 
                return _length.Value;
            else
            {
                _length = GetDistance(Start, End);
                return _length.Value;
            }
        }
    }

    /// <summary>
    /// Length according to euclidean distance formula without the square root.
    /// </summary>
    public double EuclideanLength
    {
        get
        {
            if (_euclideanLength.HasValue)
                return _euclideanLength.Value;
            else
            {
                _euclideanLength = Point.EuclideanDistanceMagnitude(Start, End);
                return _euclideanLength.Value;
            }
        }
    }

    /// <inheritdoc/>
    public override Point Center
    {
        get
        {
            if (_center.HasValue)
                return _center.Value;
            else
            {
                _center = (End - Start) / 2;
                return _center.Value;
            }
        }
    }

    /// <summary>
    /// Generates a random <see cref="Line"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area"><see cref="SadRogue.Primitives.Rectangle"/> to generate a random <see cref="Line"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public static Line GetRandomLine(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength)
    {
        if (minLineLength <= 0 || maxLineLength <= 0) throw new ArgumentException("Line constraints cannot be 0 or negative.");
        if (maxLineLength < minLineLength) throw new ArgumentException("Max length cannot be smaller than min length.");
        if (GetDistance((0,0), (area.Width-1, area.Height-1)) < minLineLength) throw new ArgumentException("Area diameter cannot be smaller than min line length.");

        while (true)
        {
            var line = new Line(area.GetRandomPosition(), area.GetRandomPosition());
            if (line.Length >= minLineLength && line.Length <= maxLineLength) 
                return line;
        }
    }
}