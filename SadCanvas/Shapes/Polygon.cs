namespace SadCanvas.Shapes;

/// <summary>
/// A primitive polygon.
/// </summary>
/// <param name="Vertices">Points for the edges.</param>
public record Polygon(Point[] Vertices) : Shape(MonoColor.White)
{
    /// <summary>
    /// <see cref="MonoColor"/> used to fill the area.
    /// </summary>
    public MonoColor FillColor { get; init; } = MonoColor.White;

    /// <summary>
    /// A primitive polygon with given <paramref name="vertices"/> and <paramref name="lineColor"/>.
    /// </summary>
    /// <param name="vertices">Points for the edges.</param>
    /// <param name="lineColor">Outline color.</param>
    public Polygon(Point[] vertices, MonoColor lineColor) : this(vertices)
    {
        LineColor = lineColor;
    }

    /// <summary>
    /// Generates a random <see cref="Polygon"/> that will fit within the constraints of the <paramref name="Canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a <see cref="Polygon"/> for.</param>
    /// <param name="minNumberOfVertices">Minimum number of vertices.</param>
    /// <param name="maxNumberOfVertices">Maximum number of vertices.</param>
    /// <returns></returns>
    public static Polygon GetRandomPolygon(Canvas canvas, int minNumberOfVertices, int maxNumberOfVertices) =>
        GetRandomPolygon(canvas.Area, minNumberOfVertices, maxNumberOfVertices);

    /// <summary>
    /// Generates a random <see cref="Polygon"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a <see cref="Polygon"/> for.</param>
    /// <param name="minNumberOfVertices">Minimum number of vertices.</param>
    /// <param name="maxNumberOfVertices">Maximum number of vertices.</param>
    /// <returns></returns>
    public static Polygon GetRandomPolygon(SadRogue.Primitives.Rectangle area, int minNumberOfVertices, int maxNumberOfVertices)
    {
        int numberOfVertices = Canvas.GetRandomInt(minNumberOfVertices, maxNumberOfVertices);
        Point[] vertices = new Point[numberOfVertices];
        for (int i = 0; i < numberOfVertices; i++)
            vertices[i] = Canvas.GetRandomPosition(area);
        return new Polygon(vertices, Canvas.GetRandomColor());
    }
}