namespace SadCanvas.Shapes;

/// <summary>
/// A primitive elliptical <see cref="Shape"/>.
/// </summary>
public class Ellipse : Polygon
{
    /// <summary>
    /// Horizontal radius length.
    /// </summary>
    public int RadiusX { get; init; }

    /// <summary>
    /// Vertical radius length.
    /// </summary>
    public int RadiusY { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Ellipse"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radiusX">Horizontal radius.</param>
    /// <param name="radiusY">Vertical radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    /// <param name="color">Color of the edges.</param>
    public Ellipse(Point center, int radiusX, int radiusY, MonoColor? color = null, int? edgeCount = null) :
        base(GetVertices(center, radiusX, radiusY, edgeCount), color)
    {
        (Center, RadiusX, RadiusY) = (center, RadiusX, RadiusY);
    }

    /// <summary>
    /// Generates a random <see cref="Ellipse"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area"><see cref="SadRogue.Primitives.Rectangle"/> to generate a random <see cref="Ellipse"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusXLength">Maximum radiusX length.</param>
    /// <param name="maxRadiusYLength">Maximum radiusY length.</param>
    /// <param name="color">Color of the ellipse.</param>
    /// <param name="mode">Mode for generating an instance.</param>
    /// <param name="circle">Make each radius equal length.</param>
    public Ellipse(SadRogue.Primitives.Rectangle area, int minRadiusLength, int maxRadiusXLength, int maxRadiusYLength,
        Mode mode = Mode.Random, MonoColor? color = null, bool circle = false) :
        base(GenerateEllipse(area, minRadiusLength, maxRadiusXLength, maxRadiusYLength, mode, circle),
            color is null ? Canvas.GetRandomColor() : color.Value)
    {
        FillColor = Canvas.GetRandomColor();
    }

    /// <inheritdoc/>
    public override Ellipse Clone(Transform? transform = null)
    {
        var ellipse = new Ellipse(Center, RadiusX, RadiusY, Color)
            { FillColor = FillColor };
        if (transform is Transform t)
            Apply(t);
        return ellipse;
    }

    static Point[] GenerateEllipse(SadRogue.Primitives.Rectangle area, int minRadiusLength, int maxRadiusXLength, 
        int maxRadiusYLength, Mode mode, bool circle)
    {
        if (mode == Mode.Fit) throw new NotImplementedException();
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

            if (circle)
            {
                int maxRadius = Math.Min(maxRadiusXFromPos, maxRadiusYFromPos);
                if (maxRadius >= minRadiusLength)
                {
                    int radius = Canvas.GetRandomInt(minRadiusLength, maxRadius);
                    if (radius <= maxRadiusXLength)
                        return GetVertices(pos, radius, radius);
                }
            }
            else // ellipse
            {
                if (maxRadiusXFromPos >= minRadiusLength && maxRadiusYFromPos >= minRadiusLength)
                {
                    int radiusX = Canvas.GetRandomInt(minRadiusLength, maxRadiusXFromPos);
                    int radiusY = Canvas.GetRandomInt(minRadiusLength, maxRadiusYFromPos);
                    if (radiusX <= maxRadiusXLength && radiusY <= maxRadiusYLength)
                        return GetVertices(pos, radiusX, radiusY);
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
    /// <param name="edgeCount">Number of edges.</param>
    /// <returns>Points that will form an <see cref="Ellipse"/>.</returns>
    /// <exception cref="ArgumentException"></exception>
    static Point[] GetVertices(Point center, int radiusX, int radiusY, int? edgeCount = null)
    {
        if (radiusX <= 0 || radiusY <= 0) throw new ArgumentException("Radius cannot be 0 or negative.");

        edgeCount = edgeCount is null ?
            Math.Max(radiusX, radiusY) :
            edgeCount < 3 ? 3 : edgeCount.Value;

        Point[] points = new Point[edgeCount.Value];
        var t = 0.0;
        var dt = 2.0 * Math.PI / edgeCount.Value;
        for (var i = 0; i < edgeCount.Value; i++, t += dt)
        {
            var x = Convert.ToInt32(radiusX * Math.Cos(t));
            var y = Convert.ToInt32(radiusY * Math.Sin(t));
            points[i] = center + (x, y);
        }
        return points;
    }
}