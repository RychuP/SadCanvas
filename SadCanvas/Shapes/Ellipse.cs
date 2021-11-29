namespace SadCanvas.Shapes;

/// <summary>
/// A primitive elliptical <see cref="Shape"/>.
/// </summary>
public record Ellipse : Polygon
{
    readonly Point _center;

    /// <summary>
    /// Horizontal radius length.
    /// </summary>
    public int RadiusX { get; init; }

    /// <summary>
    /// Vertical radius length.
    /// </summary>
    public int RadiusY { get; init; }

    /// <summary>
    /// Center point.
    /// </summary>
    public override Point Center => _center;

    /// <summary>
    /// Creates an instance of <see cref="Ellipse"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radiusX">Horizontal radius.</param>
    /// <param name="radiusY">Vertical radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    public Ellipse(Point center, int radiusX, int radiusY, int edgeCount) : 
        this(center, radiusX, radiusY, edgeCount, DefaultColor)
    { }

    /// <summary>
    /// Creates an instance of <see cref="Ellipse"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radiusX">Horizontal radius.</param>
    /// <param name="radiusY">Vertical radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    /// <param name="color">Color of the edges.</param>
    public Ellipse(Point center, int radiusX, int radiusY, int edgeCount, MonoColor color) :
        base(GetVertices(center, radiusX, radiusY, edgeCount), color)
    {
        (_center, RadiusX, RadiusY) = (center, RadiusX, RadiusY);
    }

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
            var pos = area.GetRandomPosition();
            int xDif = area.Width - pos.X;
            int yDif = area.Height - pos.Y;
            int maxRadiusXFromPos = Math.Min(xDif, pos.X);
            int maxRadiusYFromPos = Math.Min(yDif, pos.Y);

            // ellipse
            if (maxRadiusXLength != maxRadiusYLength)
            {
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
            // circle
            else
            {
                int maxRadius = Math.Min(maxRadiusXFromPos, maxRadiusYFromPos);
                if (maxRadius >= minRadiusLength)
                {
                    int radius = Canvas.GetRandomInt(minRadiusLength, maxRadius);
                    if (radius <= maxRadiusXLength)
                    {
                        var sideCount = radius;
                        return new Circle(pos, radius, sideCount, Canvas.GetRandomColor());
                    }
                }
            }
        }
    }

    /// <summary>
    /// Generates vertices for an <see cref="Ellipse"/>.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radiusX">Horizontal radius.</param>
    /// <param name="radiusY">Vertical radius.</param>
    /// <param name="sideCount">Number of edges.</param>
    /// <returns>Points that will form an <see cref="Ellipse"/>.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static Point[] GetVertices(Point center, int radiusX, int radiusY, int sideCount)
    {
        if (radiusX <= 0 || radiusY <= 0) throw new ArgumentException("Radius cannot be 0 or negative.");
        if (sideCount < 3) sideCount = 3;

        Point[] points = new Point[sideCount];
        var t = 0.0;
        var dt = 2.0 * Math.PI / sideCount;
        for (var i = 0; i < sideCount; i++, t += dt)
        {
            var x = Convert.ToInt32(radiusX * Math.Cos(t));
            var y = Convert.ToInt32(radiusY * Math.Sin(t));
            points[i] = center + (x, y);
        }
        return points;
    }
}