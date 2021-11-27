namespace SadCanvas.Shapes;

/// <summary>
/// A primitive polygon that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Polygon : Shape
{
    static readonly string InadequateNumberOfVertices = "Polygon needs at least 3 vertices.";

    /// <summary>
    /// Points that make edges of the polygon.
    /// </summary>
    public Point[] Vertices = Array.Empty<Point>();

    /// <summary>
    /// Position of the <see cref="Polygon"/>.
    /// </summary>
    public Point Position { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Polygon"/> with given <paramref name="vertices"/>.
    /// </summary>
    /// <param name="vertices">Points that form edges of the <see cref="Polygon"/>.</param>
    public Polygon(params Point[] vertices)
    {
        Vertices = vertices;
        if (vertices.Length > 0)
            Position = vertices[0];
    }

    /// <summary>
    /// Create an 
    /// </summary>
    public Polygon() { }

    /// <summary>
    /// Generates a random <see cref="Polygon"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Polygon"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public Polygon GetRandomShape(Canvas canvas, int minLineLength = MinLength, int maxLineLength = MaxLength)
    {
        throw new NotImplementedException();
    }
}