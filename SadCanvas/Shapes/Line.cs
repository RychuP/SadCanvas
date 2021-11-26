namespace SadCanvas.Shapes;

/// <summary>
/// A basic line that can be drawn on <see cref="Canvas"/>.
/// </summary>
public struct Line : IShape
{
    /// <summary>
    /// A constructor creating a basic, white line.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public Line(Point start, Point end)
    {
        Vertices = new Point[]
        {
            start,
            end
        };
        Length = Point.EuclideanDistanceMagnitude(Vertices[0], Vertices[1]);
    }

    /// <summary>
    /// Start point of the line.
    /// </summary>
    public Point Start => Vertices[0];

    /// <summary>
    /// End point of the line.
    /// </summary>
    public Point End => Vertices[1];

    /// <summary>
    /// Length of this <see cref="Line"/>.
    /// </summary>
    public double Length { get; init; } = 0;

    /// <inheritdoc/>
    public double Area => Perimeter;

    /// <inheritdoc/>
    public double Perimeter => Length;

    /// <inheritdoc/>
    public Point[] Vertices { get; init; }

    /// <inheritdoc/>
    public MonoColor OutlineColor { get; init; } = MonoColor.White;

    /// <inheritdoc/>
    public MonoColor FillColor { get; init; } = MonoColor.White;
}
