namespace SadCanvas.Shapes;

/// <summary>
/// A primitive <see cref="Shape"/> with a minimum of 3 distinct points that can be filled with <see cref="MonoColor"/>.
/// </summary>
public class Polygon : Shape
{
    /// <summary>
    /// Returns a reference to a new instance of <see cref="PolygonCreator"/>, calls to methods of which can be chained to create a <see cref="Polygon"/>.
    /// </summary>
    /// <param name="x">X coordinate of the start point.</param>
    /// <param name="y">Y coordinate of the start point.</param>
    /// <returns>Reference to an instance of <see cref="PolygonCreator"/>.</returns>
    public static PolygonCreator Create(int x, int y) => 
        new(x, y);

    /// <summary>
    /// Lines that form edges of this <see cref="Polygon"/>.
    /// </summary>
    public Line[] Edges
    {
        get
        {
            int edgeCount = Vertices.Length;
            var edges = new Line[edgeCount];
            for (int i = 0; i < edgeCount; i++)
            {
                var start = Vertices[i].ToSadPoint();
                var end = Vertices[i < edgeCount - 1 ? i + 1 : 0].ToSadPoint();
                edges[i] = new Line(start, end, Color);
            }
            return edges;
        }
    }

    /// <summary>
    /// End points of edges.
    /// </summary>
    public override Vector2[] Vertices { get; init; }

    /// <summary>
    /// Default color to be assigned as <see cref="FillColor"/> to all instances of this class.
    /// </summary>
    public static MonoColor DefaultFillColor { get; set; } = MonoColor.White;

    /// <summary>
    /// Color to fill the interior area.
    /// </summary>
    public MonoColor FillColor { get; set; }

    /// <summary>
    /// Creates an instance of <see cref="Polygon"/> with given parameters.
    /// </summary>
    /// <param name="points">Points for the edges.</param>
    /// <param name="randomColors">Whether to generate random colors or use defaults.</param>
    public Polygon(Point[] points, bool randomColors = false) :
        this(ConvertPoints(points), randomColors ? Canvas.GetRandomColor() : null, randomColors ? Canvas.GetRandomColor() : null)
    { }

    /// <summary>
    /// Creates an instance of <see cref="Polygon"/> with given parameters.
    /// </summary>
    /// <param name="points">Points for the edges.</param>
    /// <param name="color">Color of the edges.</param>
    /// <param name="fillColor">Color for the interior area.</param>
    public Polygon(Point[] points, MonoColor? color = null, MonoColor? fillColor = null) :
        this(ConvertPoints(points), color, fillColor)
    { }

    /// <summary>
    /// Creates an instance of <see cref="Polygon"/> with given parameters.
    /// </summary>
    /// <param name="vertices">Points for the edges.</param>
    /// <param name="randomColors">Whether to generate random colors or use defaults.</param>
    public Polygon(Vector2[] vertices, bool randomColors = false) :
        this(vertices, randomColors ? Canvas.GetRandomColor() : null, randomColors ? Canvas.GetRandomColor() : null)
    { }

    /// <summary>
    /// Creates an instance of <see cref="Polygon"/> with given parameters.
    /// </summary>
    /// <param name="vertices">Points for the edges.</param>
    /// <param name="color">Color of the edges.</param>
    /// <param name="fillColor">Color for the interior area.</param>
    public Polygon(Vector2[] vertices, MonoColor? color = null, MonoColor? fillColor = null) : 
        base(color)
    {
        if (vertices.Length < 3) throw new ArgumentException("A minimum of three points are needed to create a polygon.");

        FillColor = fillColor is null ? DefaultFillColor : fillColor.Value;

        Vector2 prev = vertices[0], current, next;
        List<Vector2> points = new() { vertices[0] };
        for (int i = 1, length = vertices.Length; i < length; i++)
        {
            current = vertices[i];

            // skip vertices that double up
            next = i < length - 1 ? vertices[i + 1] : vertices[0];
            if (Vector2.DistanceSquared(prev, current) < 1 || Vector2.DistanceSquared(current, next) < 1)
            {
                i++;
                continue;
            }

            points.Add(current);
            prev = current;
        }

        if (points.Count < 3)
            throw new ArgumentException("Vertices do not provide enough edges to create a polygon as defined in this class.");

        Vertices = points.ToArray();
    }

    /// <summary>
    /// Generates a random <see cref="Polygon"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a <see cref="Polygon"/> for.</param>
    /// <param name="minNumberOfVertices">Minimum number of vertices.</param>
    /// <param name="maxNumberOfVertices">Maximum number of vertices.</param>
    /// <param name="mode">Mode for generating an instance.</param>
    /// <remarks>Colors are random by default.</remarks>
    public Polygon(SadRogue.Primitives.Rectangle area, int minNumberOfVertices, int maxNumberOfVertices, Mode mode = Mode.Random) :
        this(GetRandomPolygon(area, minNumberOfVertices, maxNumberOfVertices, mode), true)
    { }

    /// <inheritdoc/>
    public override Polygon Clone(Transform? transform = null)
    {
        var polygon = new Polygon(Vertices, Color, FillColor);
        if (transform is Transform t)
            polygon.Apply(t);
        return polygon;
    }

    /// <inheritdoc/>
    public override SadRogue.Primitives.Rectangle Bounds => 
        new(Left, Top, Right - Left + 1, Bottom - Top + 1);

    /// <inheritdoc/>
    public override int Left => Convert.ToInt32(Vertices.Min(v => v.X));

    /// <inheritdoc/>
    public override int Right => Convert.ToInt32(Vertices.Max(v => v.X));

    /// <inheritdoc/>
    public override int Top => Convert.ToInt32(Vertices.Min(v => v.Y));

    /// <inheritdoc/>
    public override int Bottom => Convert.ToInt32(Vertices.Max(v => v.Y));

    static Vector2[] GetRandomPolygon(SadRogue.Primitives.Rectangle area, int minNumberOfVertices, int maxNumberOfVertices,
        Mode mode = Mode.Random)
    {
        if (mode == Mode.Fit) throw new NotImplementedException();
        if (minNumberOfVertices <= 0 || maxNumberOfVertices <= 0) throw new ArgumentException("Min and max of vertices cannot be 0 or negative.");
        if (maxNumberOfVertices < minNumberOfVertices) throw new ArgumentException("Max cannot be smaller than min.");

        int numberOfVertices = Canvas.GetRandomInt(minNumberOfVertices, maxNumberOfVertices);
        Vector2[] points = new Vector2[numberOfVertices];
        for (int i = 0; i < numberOfVertices; i++)
            points[i] = area.GetRandomPosition().ToVector();
        return points;
    }

    static Vector2[] ConvertPoints(Point[] points)
    {
        Vector2[] vectors = new Vector2[points.Length];
        for (int i = 0; i < points.Length; i++)
            vectors[i] = points[i].ToVector();
        return vectors;
    }

    /*
    public void SortVertices()
    {
        int y = (int)(from p in Vertices
                 select p.Y).Min();
        int x = (int)(from p in Vertices
                 select p.X).Max();
        Point refPoint = (x, y);
        Array.Sort(Vertices, (a, b) => CompareAngles(refPoint, a, b));

        int CompareAngles(Point refPoint, Point a, Point b)
        {
            var left = IsLeft(refPoint, a, b);
            if (left == 0) return CompareDistance(refPoint, a, b);
            return left;
        }

        int IsLeft(Point refPoint, Point a, Point b)
        {
            return (a.X - refPoint.X) * (b.Y - refPoint.Y) - (b.X - refPoint.X) * (a.Y - refPoint.Y);
        }

        int CompareDistance(Point refPoint, Point a, Point b)
        {
            var distA = (int)Point.EuclideanDistanceMagnitude(refPoint, a);
            var distB = (int)Point.EuclideanDistanceMagnitude(refPoint, b);
            return distA - distB;
        }
    }
    */
}