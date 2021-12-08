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
    /// <param name="fillColor">Color of the interior area.</param>
    public Ellipse(Point center, int radiusX, int radiusY, MonoColor? color = null, MonoColor? fillColor = null, int? edgeCount = null) :
        base(GetVertices(center, radiusX, radiusY, edgeCount), color, fillColor)
    {
        (Center, RadiusX, RadiusY) = (center.ToVector(), RadiusX, RadiusY);
    }

    /// <summary>
    /// Creates an instance of <see cref="Ellipse"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radiusX">Horizontal radius.</param>
    /// <param name="radiusY">Vertical radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    /// <param name="randomColors">Colors will be random or default.</param>
    public Ellipse(Point center, int radiusX, int radiusY, bool randomColors = false, int? edgeCount = null) :
        base(GetVertices(center, radiusX, radiusY, edgeCount), randomColors)
    {
        (Center, RadiusX, RadiusY) = (center.ToVector(), RadiusX, RadiusY);
    }

    /// <summary>
    /// Generates a random <see cref="Ellipse"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area"><see cref="SadRogue.Primitives.Rectangle"/> to generate a random <see cref="Ellipse"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusXLength">Maximum radiusX length.</param>
    /// <param name="maxRadiusYLength">Maximum radiusY length.</param>
    /// <param name="mode">Mode for generating an instance.</param>
    /// <param name="circle">Make each radius equal length.</param>
    /// <remarks>Colors are random by default.</remarks>
    public Ellipse(SadRogue.Primitives.Rectangle area, int minRadiusLength, int maxRadiusXLength, int maxRadiusYLength,
        Mode mode = Mode.Random, bool circle = false) :
        base(GetRandomEllipse(area, minRadiusLength, maxRadiusXLength, maxRadiusYLength, mode, circle), true)
    { }

    /// <inheritdoc/>
    public override Ellipse Clone(Transform? transform = null)
    {
        var ellipse = new Ellipse(Center.ToSadPoint(), RadiusX, RadiusY, Color, FillColor, Vertices.Length);
        if (transform is Transform t)
            ellipse.Apply(t);
        return ellipse;
    }

    static Vector2[] GetRandomEllipse(SadRogue.Primitives.Rectangle area, int minRadiusLength, int maxRadiusXLength, 
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
    static Vector2[] GetVertices(Point center, int radiusX, int radiusY, int? edgeCount = null)
    {
        if (radiusX <= 0 || radiusY <= 0) throw new ArgumentException("Radius cannot be 0 or negative.");

        edgeCount = edgeCount is null ?
            Math.Max(radiusX, radiusY) :
            edgeCount < 3 ? 3 : edgeCount.Value;

        var c = center.ToVector();
        Vector2[] points = new Vector2[edgeCount.Value];
        double currentAngle = 0d;
        double step = 2.0 * Math.PI / edgeCount.Value;
        for (var i = 0; i < edgeCount.Value; i++, currentAngle += step)
        {
            var x = Convert.ToInt32(radiusX * Math.Cos(currentAngle));
            var y = Convert.ToInt32(radiusY * Math.Sin(currentAngle));
            points[i] = c + new Vector2(x, y);
        }
        return points;
    }
}