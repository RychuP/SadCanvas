namespace SadCanvas.Shapes;

/// <summary>
/// A primitive <see cref="Shape"/> with a minimum of 3 distinct points that can be filled with <see cref="MonoColor"/>.
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
    public override Point[] Vertices => (from line in Edges select line.Start).ToArray();

    /// <inheritdoc/>
    public override Point Center => throw new NotImplementedException();

    /// <summary>
    /// <see cref="MonoColor"/> used to fill the area.
    /// </summary>
    public MonoColor FillColor { get; set; } = MonoColor.White;

    /// <summary>
    /// Creates an instance of <see cref="Polygon"/> with given parameters.
    /// </summary>
    /// <param name="vertices">Points for the edges.</param>
    /// <param name="color">Color of the edges.</param>
    public Polygon(Point[] vertices, MonoColor? color = null) : 
        base(color)
    {
        if (vertices.Length < 3) throw new ArgumentException("A minimum of three points are needed to create a polygon.");

        Point prev = vertices[0], current, next;
        List<Point> points = new() { vertices[0] };
        for (int i = 1, length = vertices.Length; i < length; i++)
        {
            current = vertices[i];

            // skip vertices that double up
            next = i < length - 1 ? vertices[i + 1] : vertices[0];
            if (prev == current || current == next)
            {
                i++;
                continue;
            }

            points.Add(current);
            prev = current;
        }

        if (points.Count < 3)
            throw new ArgumentException("Vertices do not provide enough edges to create a polygon as defined in this class.");

        // create edges
        Edges = new Line[points.Count];
        CreateEdges(points.ToArray());
    }

    /// <summary>
    /// Generates a random <see cref="Polygon"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a <see cref="Polygon"/> for.</param>
    /// <param name="minNumberOfVertices">Minimum number of vertices.</param>
    /// <param name="maxNumberOfVertices">Maximum number of vertices.</param>
    /// <param name="color">Color of the polygon.</param>
    /// <param name="mode">Mode for generating an instance.</param>
    public Polygon(SadRogue.Primitives.Rectangle area, int minNumberOfVertices, int maxNumberOfVertices,
        Mode mode = Mode.Random, MonoColor? color = null) :
        this(GeneratePolygon(area, minNumberOfVertices, maxNumberOfVertices, mode),
            color is null ? Canvas.GetRandomColor() : color.Value)
    {
        FillColor = Canvas.GetRandomColor();
    }

    void CreateEdges(Point[] vertices)
    {
        for (int i = 0, length = vertices.Length; i < length; i++)
        {
            Point start = vertices[i];
            Point end = vertices[i < length - 1 ? i + 1 : 0];
            var line = new Line(start, end, Color);
            Edges[i] = line;
        }
    }

    /// <inheritdoc/>
    public override void Rotate(float angle)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void Scale(float scale)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void Translate(Point vector)
    {
        Point[] vertices = new Point[EdgeCount];
        for (int i = 0; i < EdgeCount; i++)
            vertices[i] = Vertices[i] + vector;
        CreateEdges(vertices);
    }

    /// <inheritdoc/>
    public override SadRogue.Primitives.Rectangle Bounds => 
        new(Left, Top, Right - Left + 1, Bottom - Top + 1);

    /// <inheritdoc/>
    public override int Left => Vertices.Min(v => v.X);

    /// <inheritdoc/>
    public override int Right => Vertices.Max(v => v.X);

    /// <inheritdoc/>
    public override int Top => Vertices.Min(v => v.Y);

    /// <inheritdoc/>
    public override int Bottom => Vertices.Max(v => v.Y);

    static Point[] GeneratePolygon(SadRogue.Primitives.Rectangle area, int minNumberOfVertices, int maxNumberOfVertices,
        Mode mode = Mode.Random)
    {
        if (mode == Mode.Fit) throw new NotImplementedException();
        if (minNumberOfVertices <= 0 || maxNumberOfVertices <= 0) throw new ArgumentException("Min and max of vertices cannot be 0 or negative.");
        if (maxNumberOfVertices < minNumberOfVertices) throw new ArgumentException("Max cannot be smaller than min.");

        int numberOfVertices = Canvas.GetRandomInt(minNumberOfVertices, maxNumberOfVertices);
        Point[] points = new Point[numberOfVertices];
        for (int i = 0; i < numberOfVertices; i++)
            points[i] = area.GetRandomPosition();
        return points;
    }

    /// <summary>
    /// This is supposed to sort vertices, so that edges won't intersect each other... Does not work as it should yet.
    /// </summary>
    public void SortVertices()
    {
        int y = (from p in Vertices
                 select p.Y).Min();
        int x = (from p in Vertices
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
}