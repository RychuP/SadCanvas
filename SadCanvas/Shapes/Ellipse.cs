namespace SadCanvas.Shapes;

/// <summary>
/// A primitive ellipse that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Ellipse : Polygon
{
    /// <summary>
    /// Horizontal radius of the <see cref="Ellipse"/>.
    /// </summary>
    public int RadiusX { get; init; }

    /// <summary>
    /// Vertical radius of the <see cref="Ellipse"/>.
    /// </summary>
    public int RadiusY { get; init; }

    /// <summary>
    /// Center point.
    /// </summary>
    public Point Center { get; init; }

    /// <summary>
    /// Number of sides of the <see cref="Ellipse"/> (more means smoother edges).
    /// </summary>
    public int SideCount { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Ellipse"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radiusX">Horizontal radius.</param>
    /// <param name="radiusY">Vertical radius.</param>
    /// <param name="sideCount">Number of edges (more means smoother).</param>
    public Ellipse(Point center, int radiusX, int radiusY, int sideCount) : base(Array.Empty<Point>())
    {
        if (radiusX <= 0 || radiusY <= 0) throw new ArgumentException("Radius cannot be 0 or negative.");
        if (sideCount < 3) sideCount = 3;
        (Center, RadiusX, RadiusY, SideCount) = (center, radiusX, radiusY, sideCount);
        Vertices = new Point[SideCount];
        CalculateVertices();
    }

    /// <summary>
    /// Creates an instance of <see cref="Ellipse"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radiusX">Horizontal radius.</param>
    /// <param name="radiusY">Vertical radius.</param>
    /// <param name="sideCount">Number of edges (more means smoother).</param>
    /// <param name="lineColor">Outline color.</param>
    public Ellipse(Point center, int radiusX, int radiusY, int sideCount, MonoColor lineColor) :
        this(center, radiusX, radiusY, sideCount)
    {
        LineColor = lineColor;
    }

    /// <summary>
    /// Generates a random <see cref="Ellipse"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Ellipse"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusXLength">Maximum radiusX length.</param>
    /// <param name="maxRadiusYLength">Maximum radiusY length.</param>
    public static Ellipse GetRandomEllipse(Canvas canvas, int minRadiusLength, int maxRadiusXLength, int maxRadiusYLength) =>
        GetRandomEllipse(canvas.Area, minRadiusLength, maxRadiusXLength, maxRadiusYLength);

    /// <summary>
    /// Generates a random <see cref="Ellipse"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area"><see cref="SadRogue.Primitives.Rectangle"/> to generate a random <see cref="Ellipse"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusXLength">Maximum radiusX length.</param>
    /// <param name="maxRadiusYLength">Maximum radiusY length.</param>
    public static Ellipse GetRandomEllipse(SadRogue.Primitives.Rectangle area, int minRadiusLength, int maxRadiusXLength, int maxRadiusYLength)
    {
        if (minRadiusLength <= 0 || maxRadiusXLength <= 0 || maxRadiusYLength <= 0) throw new ArgumentException("Ellipse constraints cannot be 0 or negative.");
        if (maxRadiusXLength < minRadiusLength || maxRadiusYLength < minRadiusLength) throw new ArgumentException("Max radius cannot be smaller than min radius.");
        if (area.Width < minRadiusLength * 2 || area.Height < minRadiusLength * 2) throw new ArgumentException("Area width and height cannot be smaller than min diameter of the ellipse.");

        while (true)
        {
            var pos = Canvas.GetRandomPosition(area);
            int xDif = area.Width - pos.X;
            int yDif = area.Height - pos.Y;
            int maxRadiusXFromPos = Math.Min(xDif, pos.X);
            int maxRadiusYFromPos = Math.Min(yDif, pos.Y);
            if (maxRadiusXFromPos >= minRadiusLength && maxRadiusYFromPos >= minRadiusLength)
            {
                int radiusX = Canvas.GetRandomInt(minRadiusLength, maxRadiusXFromPos);
                int radiusY = Canvas.GetRandomInt(minRadiusLength, maxRadiusYFromPos);
                if (radiusX <= maxRadiusXLength && radiusY <= maxRadiusYLength)
                {
                    var noOfSides = Math.Max(radiusX, radiusY);
                    return new Ellipse(pos, radiusX, radiusY, noOfSides, Canvas.GetRandomColor());
                }
            }
        }
    }

    void CalculateVertices()
    {
        var t = 0.0;
        var dt = 2.0 * Math.PI / SideCount;
        for (var i = 0; i < SideCount; i++, t += dt)
        {
            var x = Convert.ToInt32(RadiusX * Math.Cos(t));
            var y = Convert.ToInt32(RadiusY * Math.Sin(t));
            Vertices[i] = Center + (x, y);
        }
    }
}