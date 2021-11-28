namespace SadCanvas.Shapes;

/// <summary>
/// A primitive line that can be drawn on <see cref="Canvas"/>.
/// </summary>
/// <param name="Start">Start <see cref="Point"/> for the line.</param>
/// <param name="End">End <see cref="Point"/> for the line.</param>
public record Line(Point Start, Point End) : Shape(MonoColor.White)
{
    double? _length;

    /// <summary>
    /// Calculates distance between two points.
    /// </summary>
    /// <returns>Distance between points <paramref name="p1"/> and <paramref name="p2"/>.</returns>
    public static double GetDistance(Point p1, Point p2) => Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));

    /// <summary>
    /// Creates an instance of a <see cref="Line"/> with the given parameters.
    /// </summary>
    /// <param name="start">Start <see cref="Point"/> for the line.</param>
    /// <param name="end">End <see cref="Point"/> for the line.</param>
    /// <param name="color"><see cref="MonoColor"/> for the line.</param>
    public Line(Point start, Point end, MonoColor color) : this(start, end)
    {
        LineColor = color;
    }

    /// <summary>
    /// Length of this <see cref="Line"/>.
    /// </summary>
    public double Length
    {
        get
        {
            if (_length.HasValue) return _length.Value;
            else
            {
                _length = GetDistance(Start, End);
                return _length.Value;
            }
        }
    }

    /// <summary>
    /// Generates a random <see cref="Line"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Line"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public static Line GetRandom(Canvas canvas, int minLineLength, int maxLineLength) =>
        GetRandom(canvas.Area, minLineLength, maxLineLength);

    /// <summary>
    /// Generates a random <see cref="Line"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="area"><see cref="SadRogue.Primitives.Rectangle"/> to generate a random <see cref="Line"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public static Line GetRandom(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength)
    {
        if (minLineLength <= 0 || maxLineLength <= 0) throw new ArgumentException("Line constraints cannot be 0 or negative.");
        if (maxLineLength < minLineLength) throw new ArgumentException("Max length cannot be smaller than min length.");
        if (GetDistance((0,0), (area.Width-1, area.Height-1)) < minLineLength) throw new ArgumentException("Area diameter cannot be smaller than min line length.");

        while (true)
        {
            var line = new Line(Canvas.GetRandomPosition(area), Canvas.GetRandomPosition(area));
            if (line.Length >= minLineLength && line.Length <= maxLineLength) 
                return line;
        }
    }
}