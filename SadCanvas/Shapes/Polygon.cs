namespace SadCanvas.Shapes;

/// <summary>
/// A primitive <see cref="Shape"/> with a minimum of 3 distinct points that can be filled with <see cref="MonoColor"/><br></br>
/// and which does not form edges that come back on themselves.
/// </summary>
public record Polygon : Shape
{
    /// <summary>
    /// Lines that form edges of this <see cref="Polygon"/>.
    /// </summary>
    public Line[] Edges { get; init; }

    /// <summary>
    /// Number of edges.
    /// </summary>
    public int EdgeCount => Edges.Length;

    /// <summary>
    /// End points of edges.
    /// </summary>
    public Point[] Vertices =>
        (from line in Edges select line.Start).ToArray();

    /// <inheritdoc/>
    public override Point Center => throw new NotImplementedException();

    /// <summary>
    /// <see cref="MonoColor"/> used to fill the area.
    /// </summary>
    public MonoColor FillColor { get; init; } = MonoColor.White;

    /// <summary>
    /// Creates an instance of <see cref="Polygon"/> based on the provided <paramref name="vertices"/>.
    /// </summary>
    /// <param name="vertices">Points for the edges.</param>
    public Polygon(Point[] vertices) : 
        this(vertices, MonoColor.White) 
    { }

    /// <summary>
    /// Creates an instance of <see cref="Polygon"/> with given parameters.
    /// </summary>
    /// <param name="vertices">Points for the edges.</param>
    /// <param name="color">Color of the edges.</param>
    public Polygon(Point[] vertices, MonoColor color) : 
        base(color)
    {
        if (vertices.Length < 3) throw new ArgumentException("A minimum of three points are needed to create a polygon.");

        // check vertices
        Point prev = vertices[0], current, next;
        List<Point> points = new() { vertices[0] };
        for (int i = 1, length = vertices.Length; i < length; i++)
        {
            current = vertices[i];

            // skip vertices that double up...
            next = i < length - 1 ? vertices[i + 1] : vertices[0];
            if (prev == current || current == next)
            {
                i++;
                continue;
            }
            // TODO: check for edges that come back on themselves but extend further or closer than the prev point
            // ... or form edges that come back on themselves
            else if (next == prev)
            {
                i += 2;
                continue;
            }

            points.Add(current);
            prev = current;
        }

        if (points.Count < 3)
            throw new ArgumentException("Provided vertices does not provide enough edges to create a polygon as defined in this record.");

        // create edges
        Edges = new Line[points.Count];
        for (int i = 0, length = points.Count; i < length; i++)
        {
            Point start = points[i];
            Point end = points[i < length - 1 ? i + 1 : 0];
            var line = new Line(start, end, Color);
            Edges[i] = line;
        }
    }

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
            vertices[i] = area.GetRandomPosition();
        var polygon = new Polygon(vertices, Canvas.GetRandomColor());
        polygon.SortVertices();
        return polygon;
    }

    /// <summary>
    /// This supposed to sort vertices so that they don't intersect each other... Does not work as it should yet.
    /// </summary>
    public void SortVertices()
    {
        int y = (from p in Vertices
                 select p.Y).Min();
        int x = (from p in Vertices
                 select p.X).Max();
        Point refPoint = (x, y);
        Array.Sort(Vertices, (a, b) => CompareAngles(refPoint, a, b));
    }

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
        var distA = (int) Point.EuclideanDistanceMagnitude(refPoint, a);
        var distB = (int) Point.EuclideanDistanceMagnitude(refPoint, b);
        return distA - distB;
    }
}