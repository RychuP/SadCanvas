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
    /// <param name="color"><see cref="Color"/> for the line.</param>
    public Line(Point start, Point end, Color? color = null) : 
        this(start.ToVector2(), end.ToVector2(), color)
    { }

    /// <summary>
    /// Creates an instance of a <see cref="Line"/> with the given parameters.
    /// </summary>
    /// <param name="start">Start <see cref="Vector2"/> for the line.</param>
    /// <param name="end">End <see cref="Vector2"/> for the line.</param>
    /// <param name="color"><see cref="Color"/> for the line.</param>
    public Line(Vector2 start, Vector2 end, Color? color = null) :
        base(color)
    {
        (Vertices[0], Vertices[1]) = (start, end);
    }

    /// <inheritdoc/>
    public override Line Clone(Transform? transform = null)
    {
        var line = new Line(Start, End, Color);
        if (transform is Transform t)
            line.Apply(t);
        return line;
    }

    /// <summary>
    /// Returns the length of the line.
    /// </summary>
    public double GetLength() =>
        GetVector2().Length();

    /// <summary>
    /// Returns the squared length of the line.
    /// </summary>
    public double GetLengthSquared() =>
        GetVector2().LengthSquared();

    /// <summary>
    /// Returns the unit vector of the line.
    /// </summary>
    public Vector2 GetUnitVector() =>
        GetVector2().ToUnitVector();

    /// <summary>
    /// Calculates a normal vector.
    /// </summary>
    /// <returns>A unit vector perpendicular to the line pointing to its left.</returns>
    public Vector2 GetNormalVector()
    {
        var v = GetVector2();
        v.Normalize();
        return new Vector2(v.Y, -v.X);
    }

    /// <summary>
    /// Returns the vector of the line (End - Start).
    /// </summary>
    public Vector2 GetVector2() =>
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
}