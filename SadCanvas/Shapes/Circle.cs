namespace SadCanvas.Shapes;

/// <summary>
/// A primitive circle.
/// </summary>
public record Circle : Polygon
{
    /// <summary>
    /// Radius of the <see cref="Circle"/>.
    /// </summary>
    public int Radius { get; init; }

    /// <summary>
    /// Center point.
    /// </summary>
    public Point Center { get; init; }

    /// <summary>
    /// Number of sides (more means smoother edges).
    /// </summary>
    public int SideCount { get; init; }

    /// <summary>
    /// Creates an instance with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radius">Radius length.</param>
    /// <param name="sideCount">Number of sides (more means smoother edges).</param>
    public Circle(Point center, int radius, int sideCount) : base(Array.Empty<Point>())
    {
        if (radius <= 0) throw new ArgumentOutOfRangeException("Radius cannot be 0 or negative.");
        SideCount = sideCount > 3 ? sideCount : 3;
        Center = center;
        Radius = radius;
        Vertices = new Point[SideCount];
        GenerateVertices();
    }

    /// <summary>
    /// Creates an instance of <see cref="Circle"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center <see cref="Point"/> of the <see cref="Circle"/>.</param>
    /// <param name="radius">Radius of the <see cref="Circle"/>.</param>
    /// <param name="sideCount">Number of sides of the <see cref="Circle"/> (more means smoother edges).</param>
    /// <param name="lineColor"><see cref="MonoColor"/> of the outline.</param>
    public Circle(Point center, int radius, int sideCount, MonoColor lineColor) : this(center, radius, sideCount)
    {
        LineColor = lineColor;
    }

    /// <summary>
    /// Generates a random <see cref="Circle"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Circle"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusLength">Maximum radius length.</param>
    public static Circle GetRandomCircle(Canvas canvas, int minRadiusLength, int maxRadiusLength) =>
        GetRandomCircle(canvas.Area, minRadiusLength, maxRadiusLength);

    /// <summary>
    /// Generates a random <see cref="Circle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Circle"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusLength">Maximum radius length.</param>
    public static Circle GetRandomCircle(SadRogue.Primitives.Rectangle area, int minRadiusLength, int maxRadiusLength) 
    {
        if (minRadiusLength <= 0 || maxRadiusLength <= 0) throw new ArgumentException("Circle constraints cannot be 0 or negative.");
        if (maxRadiusLength < minRadiusLength) throw new ArgumentException("Max radius cannot be smaller than min radius.");
        if (area.Width < minRadiusLength * 2 || area.Height < minRadiusLength * 2) throw new ArgumentException("Area width and height cannot be smaller than min diameter of the circle.");

        while (true)
        {
            var pos = Canvas.GetRandomPosition(area);
            int xDif = area.Width - pos.X;
            int yDif = area.Height - pos.Y;
            int maxRadiusXFromPos = Math.Min(xDif, pos.X);
            int maxRadiusYFromPos = Math.Min(yDif, pos.Y);
            int maxRadius = Math.Min(maxRadiusXFromPos, maxRadiusYFromPos);
            if (maxRadius >= minRadiusLength)
            {
                int radius = Canvas.GetRandomInt(minRadiusLength, maxRadius);
                if (radius <= maxRadiusLength)
                {
                    var sideCount = radius;
                    return new Circle(pos, radius, sideCount, Canvas.GetRandomColor());
                }
            }
        }
    }

    void GenerateVertices()
    {
        var t = 0.0;
        var dt = 2.0 * Math.PI / SideCount;
        for (var i = 0; i < SideCount; i++, t += dt)
        {
            var x = Convert.ToInt32(Radius * Math.Cos(t));
            var y = Convert.ToInt32(Radius * Math.Sin(t));
            Vertices[i] = Center + (x, y);
        }
    }
}