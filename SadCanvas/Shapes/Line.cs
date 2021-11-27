namespace SadCanvas.Shapes;

/// <summary>
/// A primitive line that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Line : Shape
{
    /// <summary>
    /// Stard <see cref="Point"/> for the line.
    /// </summary>
    public Point Start { get; init; }

    /// <summary>
    /// End <see cref="Point"/> for the line.
    /// </summary>
    public Point End { get; init; }

    /// <summary>
    /// Length of this <see cref="Line"/>.
    /// </summary>
    public double Length { get; init; }

    /// <summary>
    /// Creates an instance of a <see cref="Line"/> with the given <paramref name="start"/> and <paramref name="end"/>.
    /// </summary>
    /// <param name="start">Start <see cref="Point"/> for the line.</param>
    /// <param name="end">End <see cref="Point"/> for the line.</param>
    public Line(Point start, Point end)
    {
        (Start, End) = (start, end);
        Length = Line.GetDistance(Start, End);
    }

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
    /// Generates a random <see cref="Line"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Line"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public Line(Canvas canvas, int minLineLength = MinLength, int maxLineLength = MaxLength)
    {
        while(true)
        {
            var start = canvas.GetRandomPosition();
            var end = canvas.GetRandomPosition();
            double distance = GetDistance(end, start);
            if (distance >= minLineLength && distance <= maxLineLength)
            {
                Start = start;
                End = end;
                LineColor = Canvas.GetRandomColor();
                Length = distance;
                break;
            }
        }
    }

    /// <summary>
    /// Calculates distance between two points.
    /// </summary>
    /// <returns>Distance between points <paramref name="p1"/> and <paramref name="p2"/>.</returns>
    public static double GetDistance(Point p1, Point p2) => Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
}